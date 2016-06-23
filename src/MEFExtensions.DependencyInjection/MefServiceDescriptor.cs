using System;

namespace MEFExtensions.DependencyInjection
{
    internal class MefServiceDescriptor
    {
        public static MefServiceDescriptor WithPart(Type partType)
        {
            return new MefServiceDescriptor();
        }
    }
}