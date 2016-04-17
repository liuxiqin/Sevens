namespace Seven.Infrastructure.IocContainer
{
    public class ObjectContainer
    {
        private static IContainerObject _containerObject;


        public static void SetContainer(IContainerObject containerObject)
        {
            _containerObject = containerObject;
        }

        public static void RegisterInstance<T>(T instance) where T : class
        {
            _containerObject.RegisterInstance(instance);
        }

        public static T Resolve<T>() where T : class
        {
            return _containerObject.Resolve<T>();
        }
    }
}