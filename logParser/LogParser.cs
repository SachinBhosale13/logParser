using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.IO;

namespace logParser
{
    interface ILogParser
    {
        void GetCSV(string[] args);
    }
    public class LogParser
    {
        public void GetCSV(string[] args)
        {
            #region logDirectory
            int logDirIndex;
            string logDirPath = "";

            logDirIndex = Array.IndexOf(args, "--log-dir");

            logDirPath = @args[logDirIndex + 1];
            Console.WriteLine("->Entered Log file Path: " + logDirPath);
            #endregion

            #region LogLevels
            List<string> lstLogLvl = new List<string>() { };

            for (int i = 0; i < args.Length; i++)
            {
                if (args[i] == "--log-level")
                {
                    lstLogLvl.Add(args[i + 1].ToUpper());
                }
            }
            if (lstLogLvl.Count > 0)
            {
                Console.WriteLine("->Requested Log Levels Are: ");
                for (int i = 0; i < lstLogLvl.Count; i++)
                {
                    Console.WriteLine(i + 1 + ": " + lstLogLvl[i] + ", ");
                }
            }
            #endregion

            #region csvPath
            bool isCsvPathExist = false;
            int csvIndex = Array.IndexOf(args, "--csv");
            string csvPath = string.Empty;
            string csvDirectory = string.Empty;
            string csvFileName = string.Empty;

            Regex rexCsvPathRel = new Regex(@"^(?:[\w]\:|\\)(\\[a-z_\-\s0-9\.$])", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            Regex rexCsvPathAbs = new Regex(@"^(?:[\w]\:|\\)(\\[a-z_\-\s0-9\.]+)+\.(csv)$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

            csvPath = @args[csvIndex + 1];

            if (rexCsvPathAbs.IsMatch(csvPath))
            {
                Console.WriteLine("->Absolute CSV Path: " + csvPath);

                int idx;                
                idx = csvPath.LastIndexOf('\\');
                
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
            }
            else if (rexCsvPathRel.IsMatch(csvPath))
            {
                Console.WriteLine("-->Relative CSV Path: " + csvPath);

                csvDirectory = csvPath;
                csvPath = csvPath + "\\" + "LogsToCSV_" + DateTime.Now.ToString("dd-MM-yyyy") + ".csv";

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
            #endregion

            #region Log To CSV
            if (!string.IsNullOrEmpty(logDirPath) && lstLogLvl.Count > 0 && isCsvPathExist)
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

                                    if (inputText[x].Contains(" "+ lstLogLvl[i].ToUpper() + " "))
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
                                            Console.WriteLine("***Exception occurred while Parsing date in log files: ");
                                            throw ex;
                                        }

                                        textDetails = wordsInLine[1].Trim();

                                        sb.Clear().Append(number).Append(',').Append(logLevel).Append(',').Append(dt).Append(',').Append(tm).Append(',').Append(textDetails);
                                        number = number + 1;
                                        output.WriteLine(sb.ToString());
                                        //Console.WriteLine(sb.ToString());
                                    }
                                }

                            }
                            Console.WriteLine("=================================================================================================");
                            Console.WriteLine("***CSV file created at location: " + csvPath);
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine("Something went wrong.");
            }
            #endregion
        }
    }
}
