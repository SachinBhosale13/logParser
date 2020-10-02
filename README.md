# Testing

1.If no arguments are passed then it will throw error
2.if --help argument is passed then it will show how to pass arguments
3.If All three arguments are passed then,
  -it will check if log directory exists
  -it will check all the requested log levels 
  -it will check if csv path is absolute or not, if not then it will create the directory if there is no such directory
4.when log directory exists, csv directory exists and CSV file name is set to save,
  -it will check all the log files in mentioned directory and its subdirectories, 
  -if no log files are found then it will show error,
  -if log files are found then it will read all the lines one by one.
  -while writing to the csv file it will first check whether the current line has log levels passed in arguments
  -if log level found then it will find date, time, text and will write into csv file
========================
command for testing:-

Testing:
->to check if log directory has no log files
logParser.exe --log-dir "D:\sachin\nolog" --log-level "event" --log-level "error" --csv "D:\Sachin\Github\logParser\logParser\csv\xyz.csv"
--------------------------------------
->To check multiple log levels with any order of argument
logParser.exe --log-dir "D:\Sachin\Github\logParser\logParser\logs" --csv "D:\Sachin\Github\logParser\xyz\csv" --log-level "warn" --log-level "debug"
---------------------------------------
->To check csv absolute path:-
logParser.exe --log-dir "D:\Sachin\Github\logParser\logParser\logs" --log-level "event" --csv "D:\Sachin\Github\logParser\logParser\csv\xyz.csv"
---------------------------------------
->to check csv non absolute path:-
logParser.exe --log-dir "D:\Sachin\Github\logParser\logParser\logs" --log-level "event" --csv "D:\Sachin\Github\logParser\logParser"

================================================================================
# Problem Statement

Write a C# console application that would convert a set of log files into a
single CSV.  

The program should accept input arguments from command line as below:

```command
$ logParser.exe --log-dir <Dir-Path> --log-level <info|warn|debug>  
--log-level <info|warn|debug> --csv <Out-FilePath> 
```
- `--log-level` could be passed multiple times to allow multiple log-levels to be
  filtered (OR Condition)
- parameters could be passed in any order
- design mandatory/optional parameters as per your design.

As an **optional** feature provide --help option to print usage of your
program:  
``` help command
$ logParser.exe --help
Usage: logParser --log-dir <dir> --log-level <level> --csv <out>
    --log-dir   Directory to parse recursively for .log files
    --csv       Out file-path (absolute/relative)
    .......
    ..........
    .......

```  
_You may choose to show this help when user inputs invalid parameters too._

## Considerations:
- The input dir-path should be validated.
- Find ***.log***  files recursively in the input directory.
- Write Unit-Tests for maximum pieces/units of code. Optionally generate a
  code-coverage report too.
- To make your code review-able write readme file in the root directory of
  your project describing the project contents very briefly only.


## Log file format
Every .log file will be in a specific format as below
```
03/25 08:52:51 INFO   :......rpapi_getPolicyData: ReadBuffer:  Entering
03/25 08:52:51 WARN   :......rpapi_getPolicyData: ReadBuffer:  Exiting
03/25 08:52:51 DEBUG   :......rpapi_getPolicyData: RSVPFindServiceDetailsOnActName:  Result = 0
```
***Sample input directory is provided in `logs` directory. Please use that for your
initial testing.***    
Feel free to add your own data to this dir/file to test more.  


## Output CSV format

CSV format is expected to be in this format

| No | Level | Date        | Time     | Text |
|----|-------|-------------|----------|------|
| 1  | ERROR | 26 Mar 2020 | 10:04 PM | ABC  |
| 2  | ERROR | 26 Mar 2020 | 11:24 AM | XYZ  |
| 3  | WARN  | 25 Mar 2020 | 12:00 AM | RAJ  |


# Hints
- Use Proper validations for inputs
- Use Exception-Handling appropriately
- Do not use any third-party libraries. Write it yourself!
- If you use regular expressions, you will love yourself more!
- Write Unit-Tests that means write modular code and test all the small pieces

# Other files 
- `logs` directory contains sub-directory and log files for testing


