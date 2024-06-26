using Microsoft.VisualStudio.TestTools.UnitTesting;
using Neo.VM;
using Neo.VM.Types;
using System.Numerics;
using Neo.SmartContract.TestEngine;

namespace Neo.Compiler.CSharp.UnitTests.OldEngine
{
    [TestClass]
    public class UnitTest_Foreach
    {
        private TestEngine _engine;

        [TestInitialize]
        public void Init()
        {
            _engine = new TestEngine();
            _engine.AddEntryScript(Utils.Extensions.TestContractRoot + "Contract_Foreach.cs");
        }

        [TestMethod]
        public void intForeach_test()
        {
            _engine.Reset();
            var result = _engine.ExecuteTestCaseStandard("intForeach");

            Assert.AreEqual(VMState.HALT, _engine.State);
            Assert.AreEqual(10, result.Pop().GetInteger());

            _engine.Reset();
            result = _engine.ExecuteTestCaseStandard("intForeachBreak", 3);

            Assert.AreEqual(VMState.HALT, _engine.State);
            Assert.AreEqual(6, result.Pop().GetInteger());
        }

        [TestMethod]
        public void intForloop_test()
        {
            _engine.Reset();
            var result = _engine.ExecuteTestCaseStandard("intForloop");

            Assert.AreEqual(VMState.HALT, _engine.State);
            Assert.AreEqual(10, result.Pop().GetInteger());

            _engine.Reset();
            result = _engine.ExecuteTestCaseStandard("intForeachBreak", 3);

            Assert.AreEqual(VMState.HALT, _engine.State);
            Assert.AreEqual(6, result.Pop().GetInteger());
        }

        [TestMethod]
        public void stringForeach_test()
        {
            _engine.Reset();
            var result = _engine.ExecuteTestCaseStandard("stringForeach");

            Assert.AreEqual(VMState.HALT, _engine.State);
            Assert.AreEqual("abcdefhij", result.Pop().GetString());
        }

        [TestMethod]
        public void byteStringEmpty_test()
        {
            _engine.Reset();
            var result = _engine.ExecuteTestCaseStandard("byteStringEmpty");

            Assert.AreEqual(VMState.HALT, _engine.State);
            Assert.AreEqual(0, result.Pop().GetInteger());
        }

        [TestMethod]
        public void bytestringForeach_test()
        {
            _engine.Reset();
            var result = _engine.ExecuteTestCaseStandard("byteStringForeach");

            Assert.AreEqual(VMState.HALT, _engine.State);
            Assert.AreEqual("abcdefhij", result.Pop().GetString());
        }

        [TestMethod]
        public void structForeach_test()
        {
            _engine.Reset();
            var result = _engine.ExecuteTestCaseStandard("structForeach");

            Assert.AreEqual(VMState.HALT, _engine.State);
            var map = result.Pop() as Map;
            Assert.AreEqual(map["test1"].GetInteger(), 1);
            Assert.AreEqual(map["test2"].GetInteger(), 2);
        }

        [TestMethod]
        public void byteArrayForeach_test()
        {
            _engine.Reset();
            var result = _engine.ExecuteTestCaseStandard("byteArrayForeach");

            Assert.AreEqual(VMState.HALT, _engine.State);
            var array = result.Pop() as Array;
            Assert.AreEqual(array[0].GetInteger(), 1);
            Assert.AreEqual(array[1].GetInteger(), 10);
            Assert.AreEqual(array[2].GetInteger(), 17);
        }

        [TestMethod]
        public void uint160Foreach_test()
        {
            _engine.Reset();
            var result = _engine.ExecuteTestCaseStandard("uInt160Foreach");

            Assert.AreEqual(VMState.HALT, _engine.State);
            var array = result.Pop() as Array;
            Assert.AreEqual(array.Count, 2);
            Assert.AreEqual((array[0] as ByteString).GetSpan().ToHexString(), "0000000000000000000000000000000000000000");
            Assert.AreEqual((array[1] as ByteString).GetSpan().ToHexString(), "0000000000000000000000000000000000000000");
        }

        [TestMethod]
        public void uint256Foreach_test()
        {
            _engine.Reset();
            var result = _engine.ExecuteTestCaseStandard("uInt256Foreach");

            Assert.AreEqual(VMState.HALT, _engine.State);
            var array = result.Pop() as Array;
            Assert.AreEqual(array.Count, 2);
            Assert.AreEqual((array[0] as ByteString).GetSpan().ToHexString(), "0000000000000000000000000000000000000000000000000000000000000000");
            Assert.AreEqual((array[1] as ByteString).GetSpan().ToHexString(), "0000000000000000000000000000000000000000000000000000000000000000");
        }

        [TestMethod]
        public void ecpointForeach_test()
        {
            _engine.Reset();
            var result = _engine.ExecuteTestCaseStandard("eCPointForeach");

            Assert.AreEqual(VMState.HALT, _engine.State);
            var array = result.Pop() as Array;
            Assert.AreEqual(array.Count, 2);
            Assert.AreEqual((array[0] as ByteString).GetSpan().ToHexString(), "024700db2e90d9f02c4f9fc862abaca92725f95b4fddcc8d7ffa538693ecf463a9");
            Assert.AreEqual((array[1] as ByteString).GetSpan().ToHexString(), "024700db2e90d9f02c4f9fc862abaca92725f95b4fddcc8d7ffa538693ecf463a9");
        }

        [TestMethod]
        public void bigintegerForeach_test()
        {
            _engine.Reset();
            var result = _engine.ExecuteTestCaseStandard("bigIntegerForeach");
            BigInteger[] expected = new BigInteger[] { 10_000, 1000_000, 1000_000_000, 1000_000_000_000_000_000 };

            Assert.AreEqual(VMState.HALT, _engine.State);
            var array = result.Pop() as Array;
            Assert.AreEqual(array.Count, 4);
            for (int i = 0; i < 4; i++)
            {
                Assert.AreEqual(array[i].GetInteger(), expected[i]);
            }
        }

        [TestMethod]
        public void objectarrayForeach_test()
        {
            _engine.Reset();
            var result = _engine.ExecuteTestCaseStandard("objectArrayForeach");

            Assert.AreEqual(VMState.HALT, _engine.State);
            var array = result.Pop() as Array;
            Assert.AreEqual(array.Count, 3);
            ByteString firstitem = (array[0] as Buffer).InnerBuffer.ToArray();
            ByteString bytearray = new byte[] { 0x01, 0x02 };
            Assert.IsTrue(Equals(firstitem, bytearray));
            Assert.AreEqual(array[1].GetString(), "test");
            Assert.AreEqual(array[2].GetInteger(), 123);
        }
    }
}
