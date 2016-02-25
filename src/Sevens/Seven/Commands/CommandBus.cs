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
        public IDictionary<Type, Action<ICommand>> subscribes;


        public CommandBus(Assembly assembly)
        {
            subscribes = new ConcurrentDictionary<Type, Action<ICommand>>();

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
            if (subscribes.ContainsKey(commandType))
            {
                return;
            }
            Action<ICommand> action = (cmd) => handler.Handle(new CommandContext(null), (TCommand)cmd);

            subscribes.Add(commandType, action);
        }

        public void UnRegister<T>(Type commandType) where T : class, ICommand
        {
            if (subscribes.ContainsKey(typeof(T)))
            {
                subscribes.Remove(typeof(T));
            }
        }

        public void Send(ICommand command)
        {
            Dispatch(command);
        }

        public void Dispatch(ICommand command)
        {
            if (subscribes.ContainsKey(command.GetType()))
            {
                var commandHandler = subscribes[command.GetType()];
                commandHandler(command);
            }
        }
    }
}

