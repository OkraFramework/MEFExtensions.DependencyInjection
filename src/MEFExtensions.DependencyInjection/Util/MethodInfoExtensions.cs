using System.Reflection;

namespace MEFExtensions.DependencyInjection.Util
{
    internal static class MethodInfoExtensions
    {
        public static T CreateStaticDelegate<T>(this MethodInfo methodInfo)
        {
            return (T)(object)methodInfo.CreateDelegate(typeof(T));
        }

    }
}
