using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;

namespace StepInto2DIndexingCallExpressionBodiedMemberBugDemo
{
    public static class Extension
    {
        public static void GenericExtensionDeath<T>(this T me) { }
        public static void ConcreteExtensionDeath(this SomeType me) { }
    }
    public class SomeType //This also fails if this is a struct
    {
        public void InstanceDeath() { }
    }

    internal class Program
    {
        static void StaticDeath(SomeType test) { }

        //if you were to replace the '=>' on ANY below with surrounding curly braces, the 2D array is one dimensional, or if ANY operation other than step into is used, the error disappears
        //Likely some more cases I did not consider, but here's what I tested

        public static void DoStuffA(SomeType[,] tests) => tests[0, 0].InstanceDeath();
        public static void DoStuffB(SomeType[,] tests) => tests[0, 0].ConcreteExtensionDeath();
        public static void DoStuffC(SomeType[,] tests) => tests[0, 0].GenericExtensionDeath();
        public static void DoStuffD(SomeType[,] tests) => StaticDeath(tests[0, 0]);

        //Stepping into the various 'Death' Function calls above, from the various 'DoStuff's triggers:

        //Fatal error.Internal CLR error. (0x80131506)
        //at StepInto2DIndexingCallExpressionBodiedMemberBugDemo.Program.DoStuffA(StepInto2DIndexingCallExpressionBodiedMemberBugDemo.SomeType[,])
        //at StepInto2DIndexingCallExpressionBodiedMemberBugDemo.Program.Main(System.String[])



        static void Main(string[] args)
        {
            SomeType[,] failingArray = new SomeType[1, 1];
            failingArray[0, 0] = new SomeType();
            DoStuffA(failingArray);
            DoStuffB(failingArray);
            DoStuffC(failingArray);
            DoStuffD(failingArray);
        }
    }
}