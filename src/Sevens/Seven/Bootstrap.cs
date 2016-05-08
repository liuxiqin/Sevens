using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Seven.Initializer;

namespace Seven
{
    public class Bootstrap
    {
        private readonly IApplictionInitializer[] _applictionInitializers;

        private readonly Assembly[] _assemblies;

        private int _queueCount = 5;

        /// <summary>
        /// {0}：topic
        /// {1}: queueNumber
        /// </summary>
        private readonly string _commandPrefix = "{0}_command_{1}";

        private readonly string _eventPrefix = "{0}_event_{1}";

        public Bootstrap(Assembly[] assemblies)
        {
            _assemblies = assemblies;

        }

        public Bootstrap()
        {

        }

        public void SetQueueCount(int queueCount)
        {
            if (queueCount <= 0)
                throw new ArgumentException("queueCount can not be less than 0.");

            _queueCount = queueCount;
        }


        public void Start()
        {
            Initializer();

            Listener();
        }

        private void Initializer()
        {
            foreach (var applictionInitializer in _applictionInitializers)
            {
                applictionInitializer.Initialize(_assemblies);
            }
        }

        private void Listener()
        {

        }

        public void Stop()
        {

        }
    }
}
