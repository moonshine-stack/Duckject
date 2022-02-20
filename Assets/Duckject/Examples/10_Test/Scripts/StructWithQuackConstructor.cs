using Duckject.Core.Attributes;
using UnityEngine;

namespace Duckject.Examples._10_Test.Scripts
{
    public struct StructWithQuackConstructor
    {
        public int Number;

        public string Name;

        [Quack]
        public StructWithQuackConstructor(int number, string name, ClassWithoutConstructor classWithoutConstructor,
            MonoBehaviourClass monoBehaviourClass)
        {
            (Number, Name) = (number, name);
            Debug.Log("Class without constructor is null? " + (classWithoutConstructor == null));
            Debug.Log("MonoBehaviourClass is null? " + (monoBehaviourClass == null));
        }

        public override string ToString()
        {
            return nameof(StructWithQuackConstructor) + $" {Number} {Name}";
        }
    }
}