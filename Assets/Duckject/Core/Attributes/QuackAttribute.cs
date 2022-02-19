using System;
using UnityEngine;

namespace Duckject.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Constructor | AttributeTargets.Method)]
    public class QuackAttribute : PropertyAttribute
    {
        public QuackAttribute()
        {
        }

        public QuackAttribute(object identifier) => Identifier = identifier;

        public object Identifier { get; private set; }
    }
}