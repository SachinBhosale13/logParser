using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.IO;

namespace logParser
{
    enum enumLogLevels
    {
        INFO,
        WARN,
        DEBUG,
        ERROR,
        EVENT,
        TRACE
    }
    public interface IArgumentParser
    {
        Tuple<bool,Dictionary<string,string>> ValidateUserInputs(string[] args);
    }
    public class ArgumentParser : IArgumentParser
    {
        public Tuple<bool, Dictionary<string, string>> ValidateUserInputs(string[] args)
        {
            Tuple<bool, Dictionary<string, string>> tuple = null;
            //var vTuple = tuple;

            Dictionary<string, string> errorList = new Dictionary<string, string>();

            if (args.Length == 0)
            {
                errorList.Add("--log-dir, --log-level, --csv", "Arguments Cannot be Null");
            }
            else if (args.Contains("--help"))
            {
                tuple = Tuple.Create(true, errorList);
            }
            else if (args.Contains("--log-dir") && args.Contains("--log-level") && args.Contains("--csv"))
            {
                #region logDir
                int logDirIndex;
                string logDirPath = "";

                logDirIndex = Array.IndexOf(args, "--log-dir");

                if (logDirIndex + 1 > args.Length || args[logDirIndex + 1].StartsWith("--") || string.IsNullOrEmpty(args[logDirIndex + 1]))
                {
                    errorList.Add("--log-dir", "Directory is not valid.");
                    //throw new ArgumentNullException("--log-dir", "Please Pass The Directory to Search Log Files.");
                }
                else
                {
                    logDirPath = @args[logDirIndex + 1];

                    Regex rexDirectory = new Regex(@"^(?:[\w]\:|\\)(\\[a-z_\-\s0-9\.$])", RegexOptions.Compiled | RegexOptions.IgnoreCase);

                    if (!rexDirectory.IsMatch(logDirPath))
                    {
                        errorList.Add("--log-dir", "Directory is not valid.");
                        //throw new ArgumentException("Invalid Directory for Log Files.", "--log-dir");
                    }
                    else if (!Directory.Exists(logDirPath))
                    {
                        errorList.Add("--log-dir", "Directory does not exist.");
                    }
                }
                #endregion

                #region LogLevels                 

                for (int i = 0; i < args.Length; i++)
                {
                    if (args[i] == "--log-level")
                    {
                        //Console.WriteLine("log level index:" + (i+1));
                        if (((i + 1) >= args.Length))
                        {
                            errorList.Add("--log-level", "Please Pass Required Log Level for --log-level argument at position: " + (i + 1));
                            //throw new ArgumentNullException("--log-level", "Please Pass Required Log Level for --log-level argument at position: " + (i + 1));
                        }
                        else if (string.IsNullOrEmpty(args[i + 1]) || args[i + 1].StartsWith("--"))
                        {
                            errorList.Add("--log-level", "Please Pass Required Log Level for --log-level argument at position: " + (i + 1));
                            //throw new ArgumentException("Please Pass Required Log Level for --log-level argument at position: " + (i + 1), "--log-level");
                        }
                        else if (!Enum.IsDefined(typeof(enumLogLevels), args[i+1].ToUpper()))
                        {
                            errorList.Add("--log-level", "Invalid Log Level for --log-level argument at position: " + (i + 1));
                        }
                    }
                }
                #endregion

                #region CSV path
                int csvIndex = Array.IndexOf(args, "--csv");
                string csvPath = string.Empty;
                string csvDirectory = string.Empty;
                string csvFileName = string.Empty;

                if (csvIndex + 1 > args.Length)
                {
                    errorList.Add("--csv", "Please Pass Path For Output CSV File");
                    //throw new ArgumentNullException("--csv", "Please Pass Path For Output CSV File");
                }
                else if (string.IsNullOrEmpty(args[csvIndex + 1]) || args[csvIndex + 1].StartsWith("--"))
                {
                    errorList.Add("--csv", "Please Pass Path For Output CSV File");
                    //throw new ArgumentException("Please Pass Path For Output CSV File", "--csv");
                }
                else
                {
                    Regex rexCsvPathRel = new Regex(@"^(?:[\w]\:|\\)(\\[a-z_\-\s0-9\.$])", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                    Regex rexCsvPathAbs = new Regex(@"^(?:[\w]\:|\\)(\\[a-z_\-\s0-9\.]+)+\.(csv)$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

                    csvPath = @args[csvIndex + 1];

                    if (!rexCsvPathAbs.IsMatch(csvPath) && !rexCsvPathRel.IsMatch(csvPath))
                    {
                        errorList.Add("--csv", "Invalid Path for output CSV.");
                    }
                }
                #endregion
            }
            else
            {
                errorList.Add("--log-dir, --log-level, --csv", "Please pass valid inputs. ***For Help please use 'logParser.exe --help'");
            }


            if (errorList.Count == 0)
            {
                tuple = Tuple.Create(true, errorList);
            }
            else
            {
                tuple = Tuple.Create(false, errorList);
            }

            return tuple;
        }        
    }
}
