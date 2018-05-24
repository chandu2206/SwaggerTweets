Prerequisite: VS 2017
1. Run code in Visual Studio 2017
2. http://localhost:28866/ or http://localhost:28866/tweet/index will get tweets between 1/1/2016 and 12/31/2017. Will take 2 minutes as am looping to get all data and finally filtering out duplicate tweets
3. Default route is configured "tweet/index". 
4. Implementation logic is available in TweetController.cs file
5. Start and End date time is set to 1/1/2016 and 12/31/2017 respectively in Web Config file. You can alter accordingly
6. API request and Data display is done with 0 offset. Let me know for any modification