using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;

namespace logParser
{
    public class Program
    {
        static void Main(string[] args)
        {
            MyClass objMyClass = new MyClass();
            Tuple<bool, Dictionary<string, string>> tuple = null;
            var vTuple = tuple;

            Console.WriteLine("=================================================================================================");
            Console.WriteLine("***For help please use 'logParser.exe --help'");

            try
            {
                if (args.Contains("--help"))
                {
                    Console.WriteLine("Usage: logParser --log-dir <dir> --log-level <level> --csv <out>");
                    Console.WriteLine("--log-dir   Directory to parse recursively for .log files");
                    Console.WriteLine("--log-level   Log level i.e. info or warn or debug etc. --log-level can be passed multiple times.");
                    Console.WriteLine("--csv   Out file-path (absolute/relative)");
                    Console.WriteLine("e.g. ");
                    Console.WriteLine("logParser.exe --csv 'D:\\csv\\new.csv' --log-dir 'D:\\logParser\\logs' --log-level 'warn' --log-level 'debug'");
                    //Console.WriteLine("---------------------------------------------------------------------------------------------");
                }
                else
                {
                    #region Validations                

                    vTuple = objMyClass.ValidateUserInputs(args);

                    if (vTuple.Item1 == false)
                    {
                        Console.WriteLine("User inputs are not valid.");
                    }
                    if (vTuple.Item2.Count > 0)
                    {
                        Console.WriteLine("Error list:");
                        int num = 1;
                        foreach (var err in vTuple.Item2)
                        {
                            Console.WriteLine(num + ") " + "Param:- " + err.Key + ", " + "Error:- " + err.Value);
                            num++;
                        }
                    }
                    else
                    {
                        Console.WriteLine("All user inputs are valid.");
                    }
                    #endregion
                }
                Console.WriteLine("=================================================================================================");

                if (vTuple != null && vTuple.Item1 == true)
                {
                    objMyClass.GetCSV(args);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
