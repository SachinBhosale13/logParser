using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace logParser
{
    public class Program
    {
        static void Main(string[] args)
        {
            //Console.ForegroundColor = ConsoleColor.Red;

            try
            {
                if (args.Length == 0)
                {
                    Console.WriteLine("Please pass the arguments. You can use 'logParser --help' command for help.");
                }
                else if (args.Contains("--help"))
                {
                    Console.WriteLine("Usage: logParser --log-dir <dir> --log-level <level> --csv <out>");
                    Console.WriteLine("--log-dir   Directory to parse recursively for .log files");
                    Console.WriteLine("--csv       Out file-path (absolute/relative)");
                    Console.WriteLine("------------------");
                }
                else if (args.Contains("--log-dir") && args.Contains("--log-level") && args.Contains("--csv"))
                {
                    string[] logLvlArr = new string[] { };
                    List<string> lstLogLvl = new List<string>() { };

                    #region logDir
                    int logDirIndex = Array.IndexOf(args, "--log-dir");
                    //Console.WriteLine(logDirIndex);
                    string logDirPath = @args[logDirIndex + 1];

                    Console.WriteLine("===============================================");
                    Console.WriteLine("Entered Log file Path is >" + logDirPath);


                    if (!Directory.Exists(logDirPath))
                    {
                        Console.WriteLine("Invalid directory for Log files.");
                    }

                    #endregion

                    #region LogLevels

                    for (int i = 0; i < args.Length; i++)
                    {
                        if (args[i] == "--log-level")
                        {
                            //Console.WriteLine("hiiiiiiii");
                            lstLogLvl.Add(args[i + 1].ToUpper());
                            //logLvlArr.Append(args[i + 1].ToUpper());
                            //Console.WriteLine(args[i + 1].ToUpper() + ", ");
                        }
                    }

                    logLvlArr = lstLogLvl.ToArray();

                    Console.WriteLine("Requested Log Levels are: ");
                    for (int i = 0; i < logLvlArr.Length; i++)
                    {
                        Console.WriteLine(Convert.ToInt32(i + 1) + " " + logLvlArr[i] + ", ");
                    }
                    #endregion


                    #region CSV path
                    int csvIndex = Array.IndexOf(args, "--csv");
                    string csvPath = @args[csvIndex + 1];
                    Console.WriteLine("Entered CSV Path is >" + csvPath);
                    string directoryPath = string.Empty;
                    string csvFileName = string.Empty;

                    if (csvPath.EndsWith(".csv"))
                    {
                        int idx = csvPath.LastIndexOf('\\');

                        directoryPath = csvPath.Substring(0, idx);
                        csvFileName = csvPath.Substring(idx + 1);
                        //Console.WriteLine(directoryPath);
                        //Console.WriteLine(csvFileName);

                        if (!Directory.Exists(directoryPath))
                        {
                            Directory.CreateDirectory(directoryPath);
                        }
                    }
                    else
                    {
                        csvFileName = "LogToCsv_" + DateTime.Now.Day + "-" + DateTime.Now.Month + "-" + DateTime.Now.Year + ".csv";
                        if (!Directory.Exists(csvPath))
                        {
                            Directory.CreateDirectory(csvPath);
                        }
                        directoryPath = csvPath;
                        csvPath = csvPath + "\\" + csvFileName;
                        Console.WriteLine(csvPath);
                        //Console.WriteLine("Csv file name should end with .csv");
                    }

                    Console.WriteLine("===============================================");

                    #endregion


                    #region log to csv
                    if (Directory.Exists(logDirPath) && csvPath.EndsWith(".csv") && Directory.Exists(directoryPath))
                    {
                        string[] logFilePaths = Directory.GetFiles(logDirPath, "*.log", SearchOption.AllDirectories);

                        if (logFilePaths.Count() == 0)
                        {
                            Console.WriteLine("There are no log files in log directory and subdirectories.");
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


                                    for (int i = 0; i < logLvlArr.Length; i++)
                                    {

                                        if (inputText[x].Contains(logLvlArr[i]))
                                        {
                                            wordsInLine = inputText[x].Split(logLvlArr, 2, StringSplitOptions.RemoveEmptyEntries);
                                            //wordsInLine = line.Split(new char[] { ' ', '\t' }, 2, StringSplitOptions.RemoveEmptyEntries);

                                            //Console.WriteLine(Convert.ToString(line));

                                            //Console.WriteLine(number);
                                            logLevel = logLvlArr[i];

                                            dt = wordsInLine[0].Split(' ')[0] + "/" + DateTime.Now.Year.ToString();

                                            tm = wordsInLine[0].Split(' ')[1];

                                            string dtTmStr = dt + " " + tm;
                                            //Console.WriteLine(dtTmStr);

                                            try
                                            {
                                                DateTime dtTm = DateTime.ParseExact(dtTmStr, "MM/dd/yyyy HH:mm:ss", null);

                                                //Console.WriteLine(dtTm.ToString());

                                                dt = dtTm.ToString("dd MMM yyyy");
                                                tm = dtTm.ToString().Split(' ')[1] + " " + dtTm.ToString().Split(' ')[2];
                                            }
                                            catch (Exception ex)
                                            {
                                                Console.WriteLine("Exception occurred while Parsing date: ");
                                                Console.WriteLine(ex.Message.ToString());
                                            }

                                            textDetails = wordsInLine[1].Trim();

                                            sb.Clear().Append(number).Append(',').Append(logLevel).Append(',').Append(dt).Append(',').Append(tm).Append(',').Append(textDetails);
                                            number = number + 1;
                                            output.WriteLine(sb.ToString());
                                            //Console.WriteLine(sb.ToString());
                                        }
                                    }

                                }
                                Console.WriteLine("------------------------------------");
                                Console.WriteLine("***CSV file created at location: " + csvPath);
                            }
                        }                          

                    }
                    #endregion                    
                }
                else
                {
                    Console.WriteLine("Please pass the valid arguments. You can use 'logParser --help' command for help.");
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
