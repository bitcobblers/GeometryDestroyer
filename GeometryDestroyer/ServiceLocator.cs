using System.Collections.Generic;

namespace GeometryDestroyer
{
    public static class ServiceLocator
    {
        private readonly static Dictionary<string, object> registrations = new Dictionary<string, object>();

        public static void Register<T>(T instance) => registrations[typeof(T).Name] = instance;

        public static T Get<T>() => (T)registrations[typeof(T).Name];
    }
}