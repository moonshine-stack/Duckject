using System;
using System.Collections.Generic;
using System.Linq;

namespace Duckject.Core.Container
{
    public class DiContainer : IContainer
    {
        #region Fields

        private readonly List<Binding> _bindings = new List<Binding>();

        #endregion

        #region Properties

        public IEnumerable<Binding> Bindings => _bindings;

        #endregion

        #region IContainer Implementation

        IContainer IContainer.Bind<T>()
        {
            _bindings.Add(new Binding(typeof(T)));
            return this;
        }

        IContainer IContainer.FromInstance<T>(T instance)
        {
            _bindings.Last().FromInstance(instance);
            return this;
        }

        IContainer IContainer.Bind<T>(T instance)
        {
            Binding binding = new Binding(typeof(T));
            binding.FromInstance(instance);
            _bindings.Add(binding);
            return this;
        }

        IContainer IContainer.AsCached()
        {
            _bindings.Last().SetInstanceType(InstanceType.Cached);
            return this;
        }

        IContainer IContainer.AsTransient()
        {
            _bindings.Last().SetInstanceType(InstanceType.Transient);
            return this;
        }

        IContainer IContainer.To(params object[] target)
        {
            _bindings.Last().To(target);
            return this;
        }

        IContainer IContainer.To(params Type[] types)
        {
            _bindings.Last().To(types);
            return this;
        }

        IContainer IContainer.To<T>()
        {
            _bindings.Last().To<T>();
            return this;
        }

        IContainer IContainer.SetNonLazy()
        {
            _bindings.Last().SetNonLazy();
            return this;
        }

        IContainer IContainer.SetIdentifier(object identifier)
        {
            _bindings.Last().SetIdentifier(identifier);
            return this;
        }

        #endregion
    }
}