using Microsoft.VisualStudio.TestTools.UnitTesting;
using Neo.SmartContract.TestEngine;

namespace Neo.Compiler.CSharp.UnitTests.OldEngine
{
    [TestClass]
    public class UnitTest_Initializer
    {
        [TestMethod]
        public void Initializer_Test()
        {
            using var testengine = new TestEngine();
            testengine.AddEntryScript(Utils.Extensions.TestContractRoot + "Contract_Initializer.cs");

            var result = testengine.ExecuteTestCaseStandard("sum");
            var value = result.Pop().GetInteger();
            Assert.AreEqual(3, value);

            testengine.Reset();
            result = testengine.ExecuteTestCaseStandard("sum1", 5, 7);

            value = result.Pop().GetInteger();
            Assert.AreEqual(12, value);

            testengine.Reset();
            result = testengine.ExecuteTestCaseStandard("sum2", 5, 7);

            value = result.Pop().GetInteger();
            Assert.AreEqual(12, value);
        }
    }
}
