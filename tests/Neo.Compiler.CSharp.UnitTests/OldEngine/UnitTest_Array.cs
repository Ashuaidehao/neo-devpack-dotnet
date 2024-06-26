using Microsoft.VisualStudio.TestTools.UnitTesting;
using Neo.VM.Types;
using System.Linq;
using Neo.SmartContract.TestEngine;

namespace Neo.Compiler.CSharp.UnitTests.OldEngine
{
    [TestClass]
    public class UnitTest_Array
    {
        [TestMethod]
        public void Test_JaggedArray()
        {
            var testengine = new TestEngine();
            testengine.AddEntryScript(Utils.Extensions.TestContractRoot + "Contract_Array.cs");
            var result = testengine.ExecuteTestCaseStandard("testJaggedArray");

            var arr = result.Pop<Array>();
            Assert.AreEqual(4, arr.Count);
            var element0 = (Array)arr[0];
            CollectionAssert.AreEqual(new int[] { 1, 2, 3, 4 }, element0.Cast<PrimitiveType>().Select(u => (int)u.GetInteger()).ToArray());
        }

        [TestMethod]
        public void Test_JaggedByteArray()
        {
            var testengine = new TestEngine();
            testengine.AddEntryScript(Utils.Extensions.TestContractRoot + "Contract_Array.cs");
            var result = testengine.ExecuteTestCaseStandard("testJaggedByteArray");

            var arr = result.Pop<Array>();
            Assert.AreEqual(4, arr.Count);
            var element0 = (Buffer)arr[0];
            CollectionAssert.AreEqual(new byte[] { 1, 2, 3, 4 }, element0.InnerBuffer.ToArray());
        }

        [TestMethod]
        public void Test_EmptyArray()
        {
            var testengine = new TestEngine();
            testengine.AddEntryScript(Utils.Extensions.TestContractRoot + "Contract_Array.cs");
            var result = testengine.ExecuteTestCaseStandard("testEmptyArray");

            var arr = result.Pop<Array>();
            Assert.AreEqual(0, arr.Count);
        }

        [TestMethod]
        public void Test_IntArray()
        {
            var testengine = new TestEngine();
            testengine.AddEntryScript(Utils.Extensions.TestContractRoot + "Contract_Array.cs");
            var result = testengine.ExecuteTestCaseStandard("testIntArray");

            //test 0,1,2
            var arr = result.Pop<Array>();
            CollectionAssert.AreEqual(new int[] { 0, 1, 2 }, arr.Cast<PrimitiveType>().Select(u => (int)u.GetInteger()).ToArray());
        }

        [TestMethod]
        public void Test_IntArrayInit()
        {
            var testengine = new TestEngine();
            testengine.AddEntryScript(Utils.Extensions.TestContractRoot + "Contract_Array.cs");
            var result = testengine.ExecuteTestCaseStandard("testIntArrayInit");

            //test 1,4,5
            var arr = result.Pop<Array>();
            CollectionAssert.AreEqual(new int[] { 1, 4, 5 }, arr.Cast<Integer>().Select(u => (int)u.GetInteger()).ToArray());

            testengine.Reset();
            result = testengine.ExecuteTestCaseStandard("testIntArrayInit2");

            //test 1,4,5
            arr = result.Pop<Array>();
            CollectionAssert.AreEqual(new int[] { 1, 4, 5 }, arr.Cast<Integer>().Select(u => (int)u.GetInteger()).ToArray());

            testengine.Reset();
            result = testengine.ExecuteTestCaseStandard("testIntArrayInit3");

            //test 1,4,5
            arr = result.Pop<Array>();
            CollectionAssert.AreEqual(new int[] { 1, 4, 5 }, arr.Cast<Integer>().Select(u => (int)u.GetInteger()).ToArray());
        }

        [TestMethod]
        public void Test_StructArray()
        {
            var testengine = new TestEngine();
            testengine.AddEntryScript(Utils.Extensions.TestContractRoot + "Contract_Array.cs");
            var result = testengine.ExecuteTestCaseStandard("testStructArray");

            //test (1+5)*7 == 42
            var bequal = result.Pop() as Struct != null;
            Assert.IsTrue(bequal);
        }

        [TestMethod]
        public void Test_StructArrayInit()
        {
            var testengine = new TestEngine();
            testengine.AddEntryScript(Utils.Extensions.TestContractRoot + "Contract_Array.cs");
            var result = testengine.ExecuteTestCaseStandard("testStructArrayInit");

            //test (1+5)*7 == 42
            var bequal = result.Pop() as Struct != null;
            Assert.IsTrue(bequal);
        }

        [TestMethod]
        public void Test_ByteArrayOwner()
        {
            var testengine = new TestEngine();
            testengine.AddEntryScript(Utils.Extensions.TestContractRoot + "Contract_Array.cs");
            var result = testengine.ExecuteTestCaseStandard("testByteArrayOwner");

            var bts = result.Pop() as Buffer;
            ByteString rt = bts.InnerBuffer.ToArray();
            ByteString test = new byte[] { 0xf6, 0x64, 0x43, 0x49, 0x8d, 0x38, 0x78, 0xd3, 0x2b, 0x99, 0x4e, 0x4e, 0x12, 0x83, 0xc6, 0x93, 0x44, 0x21, 0xda, 0xfe };
            Assert.IsTrue(Equals(rt, test));
        }

        [TestMethod]
        public void Test_DynamicArrayInit()
        {
            var testengine = new TestEngine();
            testengine.AddEntryScript(Utils.Extensions.TestContractRoot + "Contract_Array.cs");
            var result = testengine.ExecuteTestCaseStandard("testDynamicArrayInit", 3);

            var arr = (Array)result.Pop().ConvertTo(StackItemType.Array);

            Assert.AreEqual(3, arr.Count);
            Assert.AreEqual(0, arr[0]);
            Assert.AreEqual(1, arr[1]);
            Assert.AreEqual(2, arr[2]);
        }

        [TestMethod]
        public void Test_DynamicArrayStringInit()
        {
            var testengine = new TestEngine();
            testengine.AddEntryScript(Utils.Extensions.TestContractRoot + "Contract_Array.cs");
            var result = testengine.ExecuteTestCaseStandard("testDynamicArrayStringInit", "hello");

            var arr = (Buffer)result.Pop().ConvertTo(StackItemType.Buffer);

            Assert.AreEqual(5, arr.Size);
            Assert.AreEqual(0, arr.InnerBuffer.Span[0]);
            Assert.AreEqual(0, arr.InnerBuffer.Span[1]);
            Assert.AreEqual(0, arr.InnerBuffer.Span[2]);
            Assert.AreEqual(0, arr.InnerBuffer.Span[3]);
            Assert.AreEqual(0, arr.InnerBuffer.Span[4]);
        }

        [TestMethod]
        public void Test_ByteArrayOwnerCall()
        {
            var testengine = new TestEngine();
            testengine.AddEntryScript(Utils.Extensions.TestContractRoot + "Contract_Array.cs");
            var result = testengine.ExecuteTestCaseStandard("testByteArrayOwnerCall");

            var bts = result.Pop().ConvertTo(StackItemType.ByteString);

            ByteString test = new byte[] { 0xf6, 0x64, 0x43, 0x49, 0x8d, 0x38, 0x78, 0xd3, 0x2b, 0x99, 0x4e, 0x4e, 0x12, 0x83, 0xc6, 0x93, 0x44, 0x21, 0xda, 0xfe };
            Assert.IsTrue(Equals(bts, test));
        }

        [TestMethod]
        public void Test_StringArray()
        {
            var testengine = new TestEngine();
            testengine.AddEntryScript(Utils.Extensions.TestContractRoot + "Contract_Array.cs");
            var result = testengine.ExecuteTestCaseStandard("testSupportedStandards");

            var bts = result.Pop().ConvertTo(StackItemType.Array);
            var items = bts as Array;

            Assert.AreEqual((ByteString)"NEP-5", items[0]);
            Assert.AreEqual((ByteString)"NEP-10", items[1]);
        }

        [TestMethod]
        public void Test_Collectionexpressions()
        {
            var testengine = new TestEngine();
            testengine.AddEntryScript(Utils.Extensions.TestContractRoot + "Contract_Array.cs");
            var result = testengine.ExecuteTestCaseStandard("testCollectionexpressions");

            var arr = (Array)result.Pop().ConvertTo(StackItemType.Array);
            Assert.AreEqual(4, arr.Count);

            var element0 = (Array)arr[0];
            CollectionAssert.AreEqual(new[] { 1, 2, 3, 4, 5, 6, 7, 8 },
                element0.Cast<PrimitiveType>().Select(u => (int)u.GetInteger()).ToArray());

            var element1 = (Array)arr[1];
            CollectionAssert.AreEqual(new[] { "one", "two", "three" },
                element1.Cast<PrimitiveType>().Select(u => u.GetString()).ToArray());

            var element2 = (Array)arr[2];
            Assert.AreEqual(3, element2.Count);
            var element2_0 = (Array)element2[0];
            CollectionAssert.AreEqual(new[] { 1, 2, 3 },
                element2_0.Cast<PrimitiveType>().Select(u => (int)u.GetInteger()).ToArray());

            var element3 = (Array)arr[3];
            Assert.AreEqual(3, element3.Count);
            var element3_0 = (Array)element3[0];
            CollectionAssert.AreEqual(new[] { 1, 2, 3 },
                element3_0.Cast<PrimitiveType>().Select(u => (int)u.GetInteger()).ToArray());
        }
    }
}
