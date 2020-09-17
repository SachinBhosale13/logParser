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
            //Console.ForegroundColor = ConsoleColor.Red;
            // Console.WriteLine("args length:" + args.Length);
            try
            {
                //int argsLength = args.Length;
                bool isLogDirExist = false;
                bool isLogLevelExist = false;
                bool isCsvPathExist = false;

                Console.WriteLine("===============================================");

                if (args.Length == 0)
                {
                    //Console.WriteLine("hiii");
                    throw new ArgumentNullException("Arguments Cannot be Null. Use 'logParser.exe --help'");
                    //Console.WriteLine("Please pass the arguments. You can use 'logParser --help' command for help.");
                }
                else if (args.Contains("--help"))
                {
                    Console.WriteLine("Usage: logParser --log-dir <dir> --log-level <level> --csv <out>");
                    Console.WriteLine("--log-dir   Directory to parse recursively for .log files");
                    Console.WriteLine("--csv       Out file-path (absolute/relative)");
                    Console.WriteLine("e.g. logParser.exe --csv 'D:\\logParser\\csv\\new.csv' --log-dir 'D:\\logParser\\logParser\\logs' --log-level 'warn' --log-level 'debug'");
                    Console.WriteLine("----------------------");
                }
                else if (args.Contains("--log-dir") && args.Contains("--log-level") && args.Contains("--csv"))
                {
                    //string[] logLvlArr = new string[] { };
                    List<string> lstLogLvl = new List<string>() { };

                    #region logDir
                    int logDirIndex;
                    string logDirPath = "";

                    logDirIndex = Array.IndexOf(args, "--log-dir");

                    if (logDirIndex + 1 > args.Length || args[logDirIndex + 1].StartsWith("--") || string.IsNullOrEmpty(args[logDirIndex + 1]))
                    {
                        throw new ArgumentNullException("--log-dir", "Please Pass The Directory to Search Log Files.");
                        //Console.WriteLine("Please Pass The Log Directory Path.");
                    }
                    else
                    {
                        logDirPath = @args[logDirIndex + 1];

                        Regex rexDirectory = new Regex(@"^(?:[\w]\:|\\)(\\[a-z_\-\s0-9\.$])", RegexOptions.Compiled | RegexOptions.IgnoreCase);

                        if (rexDirectory.IsMatch(logDirPath))
                        {
                            Console.WriteLine("-> Entered Log file Path: " + logDirPath);

                            if (!Directory.Exists(logDirPath))
                            {
                                throw new ArgumentException("Directory for Log Files Does Not Exist.", "--log-dir");
                                //Console.WriteLine("Invalid Input Directory For Log Files.");
                            }
                            else
                            {
                                isLogDirExist = true;
                            }
                        }
                        else
                        {
                            throw new ArgumentException("Invalid Directory for Log Files.", "--log-dir");
                        }

                        
                    }
                    #endregion

                    #region LogLevels                    
                    for (int i = 0; i < args.Length; i++)
                    {
                        if (args[i] == "--log-level")
                        {
                            if (((i + 1) > args.Length))
                            {
                                isLogLevelExist = false;
                                throw new ArgumentNullException("--log-level", "Please Pass Required Log Level for --log-level argument at position: " + (i + 1));
                            }
                            else if (string.IsNullOrEmpty(args[i + 1]) || args[i + 1].StartsWith("--"))
                            {
                                isLogLevelExist = false;
                                throw new ArgumentException("Please Pass Required Log Level for --log-level argument at position: " + (i + 1), "--log-level");
                                //Console.WriteLine("Please Pass Required Log Level after --log-level argument at position: " + (i+1));
                            }
                            else
                            {
                                lstLogLvl.Add(args[i + 1].ToUpper());
                                isLogLevelExist = true;
                            }
                        }
                    }

                    ////logLvlArr = lstLogLvl.ToArray();
                    if (isLogLevelExist)
                    {
                        Console.WriteLine("->Requested Log Levels Are: ");
                        for (int i = 0; i < lstLogLvl.Count; i++)
                        {
                            Console.WriteLine(i + 1 + " " + lstLogLvl[i] + ", ");
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
                        isCsvPathExist = false;
                        throw new ArgumentNullException("--csv", "Please Pass Path For Output CSV File");
                        //Console.WriteLine("Please Pass Path For Output CSV File");
                    }
                    else if (string.IsNullOrEmpty(args[csvIndex + 1]) || args[csvIndex + 1].StartsWith("--"))
                    {
                        isCsvPathExist = false;
                        throw new ArgumentException("Please Pass Path For Output CSV File", "--csv");
                    }
                    else
                    {
                        Regex rexCsvPathRel = new Regex(@"^(?:[\w]\:|\\)(\\[a-z_\-\s0-9\.$])", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                        Regex rexCsvPathAbs = new Regex(@"^(?:[\w]\:|\\)(\\[a-z_\-\s0-9\.]+)+\.(csv)$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

                        csvPath = @args[csvIndex + 1];

                        if (rexCsvPathAbs.IsMatch(csvPath))
                        {
                            Console.WriteLine("->Absolute CSV Path: " + csvPath);

                            int idx;

                            //if (csvPath.Contains("\\"))
                            //{
                            idx = csvPath.LastIndexOf('\\');
                            //directoryPath
                            csvDirectory = csvPath.Substring(0, idx);
                            csvFileName = csvPath.Substring(idx + 1);

                            if (!Directory.Exists(csvDirectory))
                            {
                                isCsvPathExist = false;

                                try
                                {
                                    Directory.CreateDirectory(csvDirectory);
                                    isCsvPathExist = true;
                                }
                                catch (Exception ex)
                                {
                                    throw new ArgumentException("Such Directory for output CSV doesn't exist and cannot create also. Actual Exception: " + ex.Message, "--csv");
                                }
                            }
                            else
                            {
                                isCsvPathExist = true;
                            }
                            //}
                        }
                        else if (rexCsvPathRel.IsMatch(csvPath))
                        {
                            Console.WriteLine("-->Relative CSV Path: " + csvPath);

                            csvDirectory = csvPath;
                            csvPath = csvPath + "\\" + "LogsToCSV_" +DateTime.Now.ToString("dd-MM-yyyy") + ".csv";

                            if (!Directory.Exists(csvDirectory))
                            {
                                try
                                {
                                    Directory.CreateDirectory(csvDirectory);
                                    isCsvPathExist = true;
                                }
                                catch (Exception ex)
                                {
                                    throw new ArgumentException("Such Directory for output CSV doesn't exist and cannot create also. Actual Exception: " + ex.Message, "--csv");
                                }
                            }
                            else
                            {
                                isCsvPathExist = true;
                            }
                        }
                        else
                        {
                            throw new ArgumentException("Invalid Path for output CSV.", "--csv");
                        }


                        //Console.WriteLine("Entered CSV Path Is >" + csvPath);

                        //if (csvPath.EndsWith(".csv"))
                        //{
                        //    int idx;
                        //    if (csvPath.Contains("\\"))
                        //    {
                        //        idx = csvPath.LastIndexOf('\\');
                        //        //directoryPath
                        //        csvDirectory = csvPath.Substring(0, idx);
                        //        csvFileName = csvPath.Substring(idx + 1);

                        //        if (!Directory.Exists(csvDirectory))
                        //        {
                        //            Directory.CreateDirectory(csvDirectory);
                        //        }
                        //    }
                        //    else if(csvPath.StartsWith(".csv"))
                        //    {
                        //        throw new ArgumentException("Output CSV file should have name", "--csv");
                        //    }                            
                        //}
                        //else
                        //{
                        //    csvFileName = "LogToCsv_" + DateTime.Now.Day + "-" + DateTime.Now.Month + "-" + DateTime.Now.Year + ".csv";
                        //    if (!Directory.Exists(csvPath))
                        //    {
                        //        Directory.CreateDirectory(csvPath);
                        //    }
                        //    csvDirectory = csvPath;
                        //    csvPath = csvPath + "\\" + csvFileName;
                        //    Console.WriteLine(csvPath);
                        //    //Console.WriteLine("Csv file name should end with .csv");
                        //}
                    }
                    #endregion

                    Console.WriteLine("===============================================");

                    #region log to csv
                    //if (Directory.Exists(logDirPath) && csvPath.EndsWith(".csv") && Directory.Exists(csvDirectory))
                    if (isLogDirExist && isLogLevelExist && isCsvPathExist)
                    {
                        string[] logFilePaths = Directory.GetFiles(logDirPath, "*.log", SearchOption.AllDirectories);

                        if (logFilePaths.Count() == 0)
                        {
                            Console.WriteLine("***There are no log files found in log directory and subdirectories.");
                        }
                        else
                        {
                            string[] inputText = new string[] { };
                            Console.WriteLine(logFilePaths.Count() + " Log files found.");

                            for (int i = 0; i < logFilePaths.Length; i++)
                            {
                                inputText = File.ReadLines(@logFilePaths[i]).Where(s => s != string.Empty && !s.StartsWith(" ")).ToArray();
                                Console.WriteLine(logFilePaths[i]);
                            }

                            //var inputText = File.ReadLines(logDirPath).Where(s => s != string.Empty && !s.StartsWith(" "));
                            if (inputText.Length < 1)
                            {
                                Console.WriteLine("***Log files in directory doesn't have any entry.");
                            }
                            else
                            {
                                string dt = string.Empty;
                                string tm = string.Empty;
                                string logLevel = string.Empty;
                                string textDetails = string.Empty;

                                var sb = new StringBuilder();

                                using (var output = new StreamWriter(csvPath))
                                {
                                    output.WriteLine("No,Level,Date,Time,Text");
                                    int number = 1;

                                    //foreach (var line in inputText)
                                    for (int x = 0; x < inputText.Length; x++)
                                    {
                                        string[] wordsInLine = new string[] { };

                                        for (int i = 0; i < lstLogLvl.Count; i++)
                                        {

                                            if (inputText[x].Contains(lstLogLvl[i]))
                                            {
                                                wordsInLine = inputText[x].Split(lstLogLvl.ToArray(), 2, StringSplitOptions.RemoveEmptyEntries);
                                                //wordsInLine = line.Split(new char[] { ' ', '\t' }, 2, StringSplitOptions.RemoveEmptyEntries);
                                                                                                
                                                logLevel = lstLogLvl[i];

                                                dt = wordsInLine[0].Split(' ')[0] + "/" + DateTime.Now.Year.ToString();

                                                tm = wordsInLine[0].Split(' ')[1];

                                                string dtTmStr = dt + " " + tm;                                                

                                                try
                                                {
                                                    DateTime dtTm = DateTime.ParseExact(dtTmStr, "MM/dd/yyyy HH:mm:ss", null);
                                                    dt = dtTm.ToString("dd MMM yyyy");
                                                    tm = dtTm.ToString().Split(' ')[1] + " " + dtTm.ToString().Split(' ')[2];
                                                }
                                                catch (Exception ex)
                                                {
                                                    Console.WriteLine("***Exception occurred while Parsing date in log files: " + ex.Message.ToString());                                                    
                                                }

                                                textDetails = wordsInLine[1].Trim();

                                                sb.Clear().Append(number).Append(',').Append(logLevel).Append(',').Append(dt).Append(',').Append(tm).Append(',').Append(textDetails);
                                                number = number + 1;
                                                output.WriteLine(sb.ToString());
                                                //Console.WriteLine(sb.ToString());
                                            }
                                        }

                                    }
                                    Console.WriteLine("------------------------------------------------");
                                    Console.WriteLine("***CSV file created at location: " + csvPath);
                                }
                            }
                        }

                    }
                    #endregion                    
                }
                else
                {
                    Console.WriteLine("Please pass the valid arguments. You can use 'logParser.exe --help' command for help.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception Occured: " + ex.Message.ToString());
                //throw ex;
            }


        }
    }
}
