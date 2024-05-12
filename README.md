# Logreporter
LogReporter exposes a web API for generating various log reports based on user needs, supporting keyword-based search in log files from a specified date range for various mock services.

##Project architecture

The architecture used for this project consists of:
1. BusinessLogic: This module implements the logic for log file analysis, including CSV file handling for generating .csv reports.
2. Web API: Provides a basic endpoint for consuming LogReporter on the client side.
3. Mock Services (Service 1, 2, 3): These mock real-life services and provide logs for analysis.

Log analysis is performed using the producer-consumer pattern in parallel. The producer extracts lines from log files where the keyword is found, while the consumer stores these lines in a collection, which is then sorted by service name and log file.
After the producer-consumer phase, the stored and sorted data is written to a .csv report file, located in the 'Result' folder within the Web API project.


