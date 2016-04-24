using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Seven.Infrastructure.Repository;

namespace Seven.Commands
{
    public class CommandBus : ICommandBus
    {
        private readonly IDictionary<Type, Action<ICommandContext, ICommand>> _commandHandlerProvider;

        private readonly IRepository _repository;

        private readonly ConcurrentQueue<ICommand> _commands;

        private readonly int _maxLength = 100;

        private bool _started = false;

        public CommandBus(Assembly assembly, IRepository repository)
        {
            _commands = new ConcurrentQueue<ICommand>();

            _commandHandlerProvider = new ConcurrentDictionary<Type, Action<ICommandContext, ICommand>>();

            Init(assembly);

            _repository = repository;
        }

        public void Init(Assembly assembly)
        {
            var targetType = typeof (ICommandHandler<>);
            var types = assembly.GetExportedTypes();
            var commandExecutorTypes = types
                .Where(x =>
                    x.IsInterface == false &&
                    x.IsAbstract == false &&
                    x.GetInterface(targetType.FullName) != null);

            foreach (var executorType in commandExecutorTypes)
            {
                dynamic handler = Activator.CreateInstance(executorType, true);

                var commandType = executorType.GetInterface(targetType.FullName).GetGenericArguments().First();

                RegisterHandler(commandType, handler);

            }
        }

        public void RegisterHandler<TCommand>(Type commandType, ICommandHandler<TCommand> handler)
            where TCommand : ICommand
        {
            if (_commandHandlerProvider.ContainsKey(commandType))
            {
                return;
            }
            Action<ICommandContext, ICommand> commandHandler =
                (context, cmd) => handler.Handle(new CommandContext(null), (TCommand) cmd);

            _commandHandlerProvider.Add(commandType, commandHandler);
        }

        public void UnRegister<T>(Type commandType) where T : class, ICommand
        {
            if (_commandHandlerProvider.ContainsKey(typeof (T)))
            {
                _commandHandlerProvider.Remove(typeof (T));
            }
        }

        public void Send(ICommand command)
        {
            _commands.Enqueue(command);

            if (!_started)
            {
                _started = true;

                BeginConsumer();
            }
        }

        private void BeginConsumer()
        {
            var command = default(ICommand);

            if (_commands.TryDequeue(out command))
                Dispatch(command);
        }

        public int GetLength()
        {
            return _commands.Count;
        }

        public void Dispatch(ICommand command)
        {
            if (_commandHandlerProvider.ContainsKey(command.GetType()))
            {
                var commandHandler = _commandHandlerProvider[command.GetType()];

                var commandContext = new CommandContext(_repository);

                commandHandler(commandContext, command);
            }
        }
    }
}

