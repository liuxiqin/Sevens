using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Seven.Commands;


namespace Seven.Initializer
{
    public class CommandHandleProvider : IApplictionInitializer
    {
        private IDictionary<Type, Action<ICommandContext, ICommand>> _commandHandlerDics = null;

        public CommandHandleProvider()
        {
            _commandHandlerDics =
                new Dictionary<Type, Action<ICommandContext, ICommand>>();
        }

        public void Initialize(params Assembly[] assemblies)
        {
            foreach (var assembly in assemblies)
            {
                var types =
                    assembly.GetTypes().Where(m => m.IsClass && typeof(ICommandHandler).IsAssignableFrom(m)).ToList();

                foreach (var type in types)
                {

                    foreach (var methodInfo in type.GetMethods(BindingFlags.Instance | BindingFlags.Public))
                    {
                        var commandHandlers =
                            methodInfo.GetParameters()
                                .Where(m => typeof(ICommand).IsAssignableFrom(m.ParameterType))
                                .ToList();

                        if (commandHandlers.Count == 1)
                        {
                            if (!_commandHandlerDics.ContainsKey(commandHandlers[0].ParameterType))
                            {
                                _commandHandlerDics.Add(commandHandlers[0].ParameterType, (commandContext, command) =>
                                {
                                    methodInfo.Invoke(Activator.CreateInstance(type),
                                        new object[] { commandContext, command });
                                });
                            }
                        }
                    }
                }


            }
        }

        public Action<ICommandContext, ICommand> GetInternalCommandHandle(Type commandType)
        {

            if (!_commandHandlerDics.ContainsKey(commandType))
            {
                throw new Exception("can not find the commandHandle with " + commandType.FullName + " command");
            }

            return _commandHandlerDics[commandType];
        }

        public void Dispose()
        {

        }
    }
}
