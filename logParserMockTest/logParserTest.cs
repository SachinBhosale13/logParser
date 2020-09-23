using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using logParser;
using Moq;
using Moq.Protected;
using System.Linq;
using System.Collections.Generic;

namespace logParserMockTest
{
    [TestClass]
    public class logParserTest
    {
        [TestMethod]
        //All valid arguments are passsed
        public void ArgumentParserTest()
        {
            //Mock builder for IArgumentParser interface
            var mock = new Mock<IArgumentParser>().SetupAllProperties();

            string[] args = { "--log-dir", "D:\\Sachin\\Github\\logParser\\logParser\\logs", "--log-level", "error", "--csv", "D:\\Sachin", "--log-level","debug" };

            Tuple<bool, Dictionary<string, string>> tuple;
            Dictionary<string, string> errorList = new Dictionary<string, string>();
            //errorList.Add("--log-dir, --log-level, --csv", "Arguments Cannot be Null");
            tuple = Tuple.Create(true, errorList);

            mock.Setup(e => e.ValidateUserInputs(args)).Returns(tuple);

            MyClass myClass = new MyClass(mock.Object);

            var actualOutput = myClass.ValidateUserInputs(args); 

            Assert.AreEqual(tuple, actualOutput);
        }

        [TestMethod]
        //No Arguments are passed
        public void ArgumentParserTest2()
        {
            //Mock builder for IArgumentParser interface
            var mock = new Mock<IArgumentParser>().SetupAllProperties();

            //string[] args = { "--log-dir", "D:\\Sachin\\Github\\logParser\\logParser\\logs", "--log-level", "error", "--csv", "D:\\Sachin", "--log-level", "debug" };
            string[] args = { };

            Tuple<bool, Dictionary<string, string>> tuple;
            Dictionary<string, string> errorList = new Dictionary<string, string>();
            errorList.Add("--log-dir, --log-level, --csv", "Arguments Cannot be Null");
            tuple = Tuple.Create(false, errorList);

            mock.Setup(e => e.ValidateUserInputs(args)).Returns(tuple);

            MyClass myClass = new MyClass(mock.Object);

            var actualOutput = myClass.ValidateUserInputs(args);

            Assert.AreEqual(tuple, actualOutput);
        }

        [TestMethod]
        //all valid params are passed
        public void LogParserTest1()
        {
            var mock = new Mock<ILogParser>().SetupAllProperties();
            string[] args = { "--log-dir", "D:\\Sachin\\Github\\logParser\\logParser\\logs", "--log-level", "error", "--csv", "D:\\Sachin", "--log-level", "debug" };

            mock.Setup(e => e.GetCSV(args)).Returns(true);

            MyClass myClass = new MyClass(mock.Object);

            var actualOutput = myClass.GetCSV(args);

            Assert.AreEqual(true, actualOutput);
        }

        [TestMethod]
        //csv path not passed
        public void LogParserTest2()
        {
            var mock = new Mock<ILogParser>().SetupAllProperties();
            string[] args = { "--log-dir", "D:\\Sachin\\Github\\logParser\\logParser\\logs", "--log-level", "error", "--csv", "" };

            mock.Setup(e => e.GetCSV(args)).Returns(false);

            MyClass myClass = new MyClass(mock.Object);

            var actualOutput = myClass.GetCSV(args);
            
            Assert.AreEqual(false, actualOutput);
        }
    }
}
