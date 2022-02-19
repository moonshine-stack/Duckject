namespace Duckject.Examples._10_Test.Scripts
{
    public class ClassWithoutConstructor
    {
        public int Number = 100;
        
        public override string ToString()
        {
            return nameof(ClassWithoutConstructor) + " " + Number;
        }
    }
}
