using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

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
