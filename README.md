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
-Valid calls that should work
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
- 