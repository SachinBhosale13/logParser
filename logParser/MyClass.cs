using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace logParser
{
    public class MyClass: IArgumentParser, ILogParser
    {
        

        public Tuple<bool, Dictionary<string, string>> ValidateUserInputs(string[] args)
        {
            ArgumentParser objArg = new ArgumentParser();
            Tuple<bool, Dictionary<string, string>> tuple = null;
            var vTuple = tuple;

            try
            {
                vTuple = objArg.ValidateUserInputs(args);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return vTuple;
        }

        public void GetCSV(string[] args)
        {
            LogParser objLog = new LogParser();

            try
            {
                objLog.GetCSV(args);
            }
            catch (Exception ex)
            {
                throw ex;
            }
           
        }


    }
}
