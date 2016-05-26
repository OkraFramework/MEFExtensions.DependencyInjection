﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Composition.Hosting;
using System.Composition.Convention;
using Microsoft.Extensions.DependencyInjection;
using System.Composition;

namespace Okra.MEF.DependencyInjection
{
    [Export(typeof(IServiceProvider))]
    internal class MefServiceProvider : IServiceProvider
    {
        public const string SHARING_BOUNDARY = "Scope";

        // *** Fields ***

        private CompositionContext _compositionContext;

        // *** Constructors ***

        [ImportingConstructor]
        public MefServiceProvider(CompositionContext compositionContext)
        {
            _compositionContext = compositionContext;
        }

        // *** Methods ***

        public object GetService(Type serviceType)
        {
            object export;

            if (_compositionContext.TryGetExport(serviceType, out export))
                return export;
            else
                return null;
        }
    }
}
