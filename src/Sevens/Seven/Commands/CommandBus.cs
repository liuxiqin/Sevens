using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Seven.Commands
{
    public class CommandBus : ICommandBus
    {
        private readonly IDictionary<Type, Action<ICommandContext, ICommand>> _commandHandlerProvider;

        public CommandBus(Assembly assembly)
        {
            _commandHandlerProvider = new ConcurrentDictionary<Type, Action<ICommandContext, ICommand>>();
            Init(assembly);
        }

        public void Init(Assembly assembly)
        {
            var targetType = typeof(ICommandHandler<>);
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
            Action<ICommandContext, ICommand> commandHandler = (context, cmd) => handler.Handle(new CommandContext(null), (TCommand)cmd);

            _commandHandlerProvider.Add(commandType, commandHandler);
        }

        public void UnRegister<T>(Type commandType) where T : class, ICommand
        {
            if (_commandHandlerProvider.ContainsKey(typeof(T)))
            {
                _commandHandlerProvider.Remove(typeof(T));
            }
        }

        public void Send(ProcessCommand processCommand)
        {
            Dispatch(processCommand);
        }

        public void Dispatch(ProcessCommand processCommand)
        {
            if (_commandHandlerProvider.ContainsKey(processCommand.GetType()))
            {
                var commandHandler = _commandHandlerProvider[processCommand.GetCommandType];
                commandHandler(processCommand.CommandContext, processCommand.Command);
            }
        }
    }


}

