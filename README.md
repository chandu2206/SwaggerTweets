Prerequisite: VS 2017
1. Run code in Visual Studio 2017
2. http://localhost:28866/ or http://localhost:28866/tweet/index will get UI with default date time set as 1/1/2016 and 12/31/2017
3. Click on Get Tweet to extract data. Will take 3 minutes as am looping to get all data between specified date range
4. Implementation logic is available in TweetController.cs file 
6. API request and Data display is done with local offset
7. Also you can get Json response. API Structure [host name]/tweet/getdata?sDate=[Start Date]&eDate=[End Date]&offSet=[Local time offSet]
   Format
   [Start Date]:YYYY-MM-DD
   [Start Date]:YYYY-MM-DD
   [Local time offSet]: Should be in minutes. Ex: For India(GMT + 5.30): -330
   Ex: http://localhost:28866/tweet/GetData?sDate=2016-01-01&eDate=2017-12-31&offSet=-330