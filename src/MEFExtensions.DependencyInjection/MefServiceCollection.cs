using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Composition.Hosting;

namespace MEFExtensions.DependencyInjection
{
    /// <summary>
    /// Default implementation of <see cref="IServiceCollection"/>.
    /// </summary>
    public class MefServiceCollection : IMefServiceCollection
    {
        private readonly List<ServiceDescriptor> _descriptors;
        private readonly List<Action<ContainerConfiguration>> _mefDescriptors = new List<Action<ContainerConfiguration>>();

        public MefServiceCollection()
        {
            this._descriptors = new List<ServiceDescriptor>();
        }

        public MefServiceCollection(IEnumerable<ServiceDescriptor> services)
        {
            this._descriptors = new List<ServiceDescriptor>(services);
        }

        /// <inheritdoc />
        public int Count => _descriptors.Count;

        /// <inheritdoc />
        public bool IsReadOnly => false;

        public ServiceDescriptor this[int index]
        {
            get
            {
                return _descriptors[index];
            }
            set
            {
                _descriptors[index] = value;
            }
        }

        public IList<Action<ContainerConfiguration>> MefServiceDescriptors
        {
            get
            {
                return _mefDescriptors;
            }
        }

        /// <inheritdoc />
        public void Clear()
        {
            _descriptors.Clear();
        }

        /// <inheritdoc />
        public bool Contains(ServiceDescriptor item)
        {
            return _descriptors.Contains(item);
        }

        /// <inheritdoc />
        public void CopyTo(ServiceDescriptor[] array, int arrayIndex)
        {
            _descriptors.CopyTo(array, arrayIndex);
        }

        /// <inheritdoc />
        public bool Remove(ServiceDescriptor item)
        {
            return _descriptors.Remove(item);
        }

        /// <inheritdoc />
        public IEnumerator<ServiceDescriptor> GetEnumerator()
        {
            return _descriptors.GetEnumerator();
        }

        void ICollection<ServiceDescriptor>.Add(ServiceDescriptor item)
        {
            _descriptors.Add(item);
        }

        void IMefServiceCollection.Add(Action<ContainerConfiguration> item)
        {
            _mefDescriptors.Add(item);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public int IndexOf(ServiceDescriptor item)
        {
            return _descriptors.IndexOf(item);
        }

        public void Insert(int index, ServiceDescriptor item)
        {
            _descriptors.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            _descriptors.RemoveAt(index);
        }
    }

}
