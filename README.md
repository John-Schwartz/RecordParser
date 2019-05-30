# RecordParser
To run the record parser on files:
Navigate to ~/RecordParser/RecordParser/Bin/Debug and run the RecordParser.exe with the cli, passing the full file paths of 
files you wish to parse. (e.g. RecordParser.exe "C:\valid\path\to\file.txt" "C:\Other\valid\path\to\File.txt") 

To run the RestfulAPI: 
Make sure the RESTfulAPI project is set as the startup project. Once running, make sure you are on 
http://(your localhost and port)/api/Records. Send POST requests with the record information in the body,
in the same delimited formats as the record information in the files the console application parses. 

API Will respond 202 Accepted if line is successfully parsed, and will contain the JSON data array of all 
parsed records from the session.

API will respond 406 Not Acceptable if the record information is invalid.
