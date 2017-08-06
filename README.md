# RV Coding Challenge

A new client wants to build a small API to allow users to pin areas they've visited and potentially share them with other users. The client included a set of sample data in `User.csv`, `City.csv`, and `State.csv`. Please implement a few basic operations on the data provided, including the following.

 - Listing the cities in a given state
 - Registering a visit to a particular city by a user
 - Removing a visit to a city
 - Listing cities visited by a user
 - Listing states visited by a user.  

You may use whatever language or tools you wish to complete the exercise.  Keep in mind that you may be asked to extend your solution in an on-site interview.


**Required endpoints**

1. List all cities in a state

	`GET /state/{state}/cities`
 
2. Allow to create rows of data to indicate they have visited a particular city.

	`POST /user/{user}/visits`

	```
	{
		"city": "Chicago",
		"state": "IL"
	}
	```
	
3. Allow a user to remove an improperly pinned visit.

	`DEL /user/{user}/visit/{visit}`

4. Return a list of cities the user has visited

	`GET /user/{user}/visits`
	
5. Return a list of states the user has visited

	`GET /user/{user}/visits/states`


## Things To Consider

- How should you deal with invalid or improperly formed requests?
- How should you handle requests that result in large data sets?


## Deliverables

- The source code for your solution.
- The database schema you use to implement your solution.
- Any additional documentation you feel is necessary to explain how your application works, or describe your thought process and design decisions.


## Bonus Points

- Handle authentication of users prior to allowing changes to their visits
- Make use of the lat/long data for cities in a creative way that provides additional functionality for the client


##Deployment Notes

- The SQL database is running in Azure with the following connection string. You can also replace appSettings.json with your own instance of the DB.
- A SQL create script is in the root Github folder.
Server=tcp:restapiprojectspiveyserver.database.windows.net,1433;Initial Catalog=restapiprojectspivey;Persist Security Info=False;User ID=restadmin;Password=Walking in the woods.;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;

- The appSettings.json needs to be updated with your local file system copy of City.csv and State.csv.

##Usage Notes
- I have a few comments throughout the source code explaining TODO's or decisions.
- User 1 is the only user in the system, because it's hard coded in the BearerTokenDecryptor class. So HTTP POST will only work with user 1.
- Valid calls that should work
1. HTTP GET http://localhost:61372/user/1/visits?skip=0&take=1
2. HTTP GET http://localhost:61372/state/al/cities
3. HTTP GET http://localhost:61372/1/al/visits
4. HTTP POST http://localhost:61372/1/visits
   HEADERS:
   Content-Type: application/json
   {
     city: "West Blocton",
     state: "AL"
   }
5. HTTP DELETE http://localhost:61372/user/1/visit/86c541a5-2ef9-410e-8481-dea3ca7947e8
6. HTTP GET http://localhost:61372/user/1/visits
7. HTTP GET http://localhost:61372/user/1/visits/states

- In any project other than a sample, you never commit credentials to a repository, whether it's in source code or notes. But for this sample project, they are included 
- in the appSettings.json for convenience and are pointing to an Azure SQL instance I'm leaving running this week.
- Time boxing the project, but the appSettings should contain an encrypted connection string.
- API paging is allowed on the following endpoint: HTTP GET http://localhost:61372/user/1/visits?skip=0&take=1
- There is no Authorization header required, but the reference implementation with TODO statements is in the Web API project's Authorization folder set of classes.
- Claims are checked before POST visit and DELETE visit, but are hard coded to always succeed. GET any resource is not checked at all.
- This API only is setup to use JSON, but a XML Media Formatter could be used to also accept XML.
- There's unit tests for good coverage for the web API project, but because of time boxing, unit tests were not made for the file and SQL repository implementations.
- The unit tests that are currently broken are because of this.Request being null in the controller, which happened after adding bearer token claims checks.
- Time boxing this project, there's currently no rate limiting, but it could be implemented in the controllers and use a server-side cache or a shared-cache if 
  it the rate limiting should be constrained across the whole server farm, or it could be constrained per user or IP address.
- Time voxing the project, but it would be good to add diagnostics so we could turn on information, verbose or error logging to see what's going on.
- The database is made compact by using ints, while the HTTP representation outward is more human readable. It's not always necessary to be so compact in a DB,
- but it's a desirable characteristic for performance and Machine Learning, which will need strings tokenized anyways. But it does make the DB less human readable.


- FileGeographyRepository.cs notes
- //The purpose of this is to demonstrate that the web application does not have to have one huge
    //database for the application, but can have multiple repositories serving different entities,
    //with each repository using SQL, MongoDB, AWS DynamoDB or anything. The tradeoff is you do want
    //these to be relatively coarse grained and not too many different repositories, but have them
    //in some logical grouping. The other tradeoff is if you use different repositories and there needs
    //to be relations between them, then that is combined in a higher layer. So it might not be as
    //operationally efficient as putting everything in one database, but it might endup being more
    //manageable or more agile by allowing new persistence technologies to be introduced and use it
    //with one repository, rather than having to rewrite the entire application to use the new
    //persistence technology. Also, the objects in .NET might homogenize on one data type, but it might
    //be discovered later that the new persistence technology doesn't allow that datatype and so you
    //have to resort to conversions. For example, Azure Table Storage doesn't allow Int16, but if you
    //already have an Int16/tinyint in your SQL implementation and your web API represents it in Int16,
    //then you will be forced to do the conversion inside the repository.
	
- SqlVisitsRepository.cs notes
- //The purpose of this is to show how it could be a SQL implementation or a new project could be created
    //for MongoDB or AWS DynamoDB and this repository implementation could be maintained or deprecated in
    //the future. But the web project won't be affected by choosing a different persistence technology. The
    //VisitId is represented in the generic repository interface entities as a string, but in the SQL
    //implementation it is being saved as a GUID. Normally you would want these data types to align, but
    //there might be a reason for having a difference between the persistence implementation and the
    //type shared in the generic repository interfaces. However, this can also cause the bad effect of
    //implementation bleed-thru, where the wider type of string can allow more options than are allowed
    //in the SQL implementation's GUID and now that lower level dependency is causing problems.