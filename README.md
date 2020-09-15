# logParser
Console app that Converts the log file into csv with passing arguments for log level,log files directory, csv path

--------------------
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
logParser.exe --log-dir "D:\sachin\nolog" --log-level "event" --log-level "error" --csv "D:\Sachin\VS_Projects\logParser\logParser\csv\xyz.csv"
--------------------------------------
->To check multiple log levels
logParser.exe --log-dir "D:\Sachin\VS_Projects\logParser\logParser\logs" --log-level "event" --log-level "error" --csv "D:\Sachin\VS_Projects\logParser\logParser\csv\xyz.csv"
---------------------------------------
->To check csv absolute path:-
logParser.exe --log-dir "D:\Sachin\VS_Projects\logParser\logParser\logs" --log-level "event" --csv "D:\Sachin\VS_Projects\logParser\logParser\csv\xyz.csv"
---------------------------------------
->to check csv non absolute path:-
logParser.exe --log-dir "D:\Sachin\VS_Projects\logParser\logParser\logs" --log-level "event" --csv "D:\Sachin\VS_Projects\logParser\logParser"
