WinRT Sample
============

This sample will allow WinRT's developers to quickly add Backend to their apps & games without any server or infrastructure hassles.

__About__ 

In order to describe this sample contains User Service, Storage Service, Leaderboard Service & Achievement Service api to build your apps/games. 

* __User Service__ :- [User Management](http://api.shephertz.com/app42-docs/user-management-service/) for any Mobile or Web App which enables User registration, retrieval, state management e.g., Lock, Delete, Authentication and more. Along with User management, the library provides API’s for persistent Session Management.
* __LeaderBoard Service__ :- This [service](http://api.shephertz.com/app42-docs/leaderboard-service/) will allow Game Developer to add a game and maintain the game scoring. It also allows to maintain a Scoreboard across game sessions, one can query for average or highest score for user for a Game and highest and average score across users for a Game. It also records and provides ranking of the user against other users for a particular game. The Reward and Reward Points allow the Game Developer to assign rewards to a user and redeem the rewards. For instance, one can give Swords or Energy etc.
* __Storage Service__ :- This [service](http://api.shephertz.com/app42-docs/nosql-storage-service/) provides an efficient way to manage JSON documents in NoSQL database on the cloud. You can store, update, search a JSON document and can also apply map-reduce search for storing documents. For example if you try to store a JSON document “{"Company":"Shephertz"}”, it will be stored with unique Object Id in the format: {“Company”:”Shephertz”,”_id”:{“$oid”:”4f423dcce1603b3f0bd560cf”}}. This oid can be used later to access/search the document.

__Sample Prerequisites & Run Settings__

* [Register/Login](https://apphq.shephertz.com/) With App42
* Copy Your API Key and Secret Key which you have received after the success of App Creation
* [Download](https://github.com/shephertz/App42_WINDOWS_SDK/archive/master.zip) App42 WinRT Sdk
* Open your Project in Windows Editor
* Edit Constant.cs file with API key and Secret key and replace the dbName if you are already created in your app
* Build your project and run


