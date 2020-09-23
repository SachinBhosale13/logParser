using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using logParser;

namespace logParserMockTest
{
    public class MyClass: IArgumentParser
    {
        private readonly IArgumentParser iArgParser;
        private readonly ILogParser iLogParser;

        public MyClass(IArgumentParser iArg)
        {
            iArgParser = iArg;
        }

        public MyClass(ILogParser iLog) {

            iLogParser = iLog;
        }


        public Tuple<bool, Dictionary<string, string>> ValidateUserInputs(string[] args)
        {
            //return iArgParser.ValidateUserInputs(args);
            //ArgumentParser objArg = new ArgumentParser();
            Tuple<bool, Dictionary<string, string>> tuple = null;
            var vTuple = tuple;

            try
            {
                vTuple = iArgParser.ValidateUserInputs(args);
                //vTuple = objArg.ValidateUserInputs(args);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return vTuple;
        }

        public bool GetCSV(string[] args)
        {
            //LogParser objLog = new LogParser();
            bool result = false;

            try
            {
                result = iLogParser.GetCSV(args);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }


    }
}
