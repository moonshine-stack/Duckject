using Duckject.Core.Attributes;

namespace Duckject.Examples._10_Test.Scripts
{
    public struct StructWithQuackConstructor
    {
        public int Number;
        
        [Quack]
        public StructWithQuackConstructor(int number) => Number = number;

        public override string ToString()
        {
            return nameof(StructWithQuackConstructor) + " " + Number;
        }
    }
}
