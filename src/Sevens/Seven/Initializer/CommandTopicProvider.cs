using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Seven.Commands;

namespace Seven.Initializer
{
    public class CommandTopicProvider : IApplictionInitializer
    {
        private IDictionary<Type, string> _commandTopics;

        public CommandTopicProvider()
        {
            _commandTopics = new ConcurrentDictionary<Type, string>();
        }

        public void Initialize(params Assembly[] assemblies)
        {
            foreach (var assembly in assemblies)
            {
                var commandTypes =
                    assembly.GetTypes().Where(m => m.IsClass && typeof(ICommand).IsAssignableFrom(m)).ToList();

                commandTypes.ForEach(m =>
                {
                    if (!_commandTopics.ContainsKey(m))
                        _commandTopics.Add(m, m.Namespace);
                });


            }
        }

        public string GetCommandTopic(ICommand command)
        {
            if (_commandTopics.ContainsKey(command.GetType()))
                return _commandTopics[command.GetType()];

            return command.GetType().Namespace;
        }
    }
}
