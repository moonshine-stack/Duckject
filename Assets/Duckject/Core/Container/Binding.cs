using System;
using System.Collections.Generic;

namespace Duckject.Core.Container
{
    public class Binding
    {
        #region Fields

        private InstanceType _instanceType = InstanceType.Cached;

        private readonly List<Type> _targetTypes = new List<Type>();

        private readonly List<object> _targets = new List<object>();

        #endregion

        #region Constructors

        public Binding(Type serviceType) => ServiceType = serviceType;

        #endregion

        #region Properties

        public object Service { get; private set; }

        public Type ServiceType { get; }
        
        public bool IsNonLazy { get; private set; }
        
        public object Identifier { get; private set; }

        public InstanceType InstanceType => _instanceType;

        public IEnumerable<object> Targets => _targets;

        public IEnumerable<Type> TargetTypes => _targetTypes;

        #endregion

        #region Public Methods

        public void To(params object[] targets) => _targets.AddRange(targets);

        public void To(params Type[] types) => _targetTypes.AddRange(types);

        public void To<T>() => _targetTypes.Add(typeof(T));

        public void FromInstance<T>(T instance) => Service = instance;

        public void SetInstanceType(InstanceType instanceType) => _instanceType = instanceType;

        public void SetNonLazy() => IsNonLazy = true;

        public void SetIdentifier(object identifier) => Identifier = identifier;

        #endregion
    }
}
