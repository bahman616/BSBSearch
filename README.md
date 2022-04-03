# Introduction
This is an application to list and query BSB numbers and notify subscribers about BSB updates.

# Instructions to use the application 

## How to run
You need .Net 6 SDK to be able to run the application. Go to "BsbSearch" folder and run `dotnet run`.

If you have Visual Studio 2022 then you can open the solution and press F5 to run.

## How to Use

After you ran the application, you should land on the home page that doesn't have much information.

You can go to "All BSB Numbers" page to see the full list of BSB numbers.

You can use one of the BSB Numbers and go to "Search BSB Number" to look for one and edit it. After you submit any change, then all the subscribed teams will be notified.

You can go to "All Requests" page to see the request history. This page shows how other teams are getting the data about the changed BSB record.

# Behind the scenes

## BSB Edit
Based on the problem statement we should subscribe to BSB changes and notify other teams that use our service. 

To make it simple I simulated the BSB change within the application. So you can simply edit a BSB. However in the real World, editing event can come from another system that we can subscribe to. Or we get notified using a webhook.

After user updates a BSB record then we will notify all other teams (partners) with webhooks. The list of teams is kept in `Data/Partners.json` file. 

Each team has a team name, a key and a webhook URL. Each api call needs to be authorised with the team name and the key. For that I added `AuthenticationMiddleware`. This middleware expects these two HTTP Headers: `team-name` and `very-very-secure`. 

Each team registered a webhook URL to be notified when there is a BSB change. To make it simple, I created `FakeTeamsController` to simulate these webhooks. In this controller, there are 2 endpoints for team1 and team2. These endpoints are registered in `Data/Partners.json` file.

When there is a BSB record update, we make HTTP post call on the webhooks. This record has a URL (in the body) to the changed BSB record. Each team needs to send a HTTP get to this URL to retrieve the BSB details. 

## Data Persistence 

This application needs a database. In a real Word scenario most probably I would have gone with something like CosmosDB. because it is distributed and can help the app to be scalable. But here for simplicity I used local json files to store the data. 

Using nosql database makes sense here because we only need to retrieve the BSB records using the BSB number or showing them all. We can use the BSB number as an Index to make things fast.

Ideally, some sensitive data like the teams' credentials should be kept somewhere safer like Azure Key Vault or should be kept encrypted in a production scenario.

## Reporting
Because there is a requirement to report the usage of this application I created a simple page that shows the endpoint call history. This page shows a log of how other teams are using our endpoints to retrieve BSB records.

Obviously, this is a very simple scenario and not very realistic for production, especially with the use of json files to persist the data.

If there is a serious need to prepare customised report on the data, we can think of using something like Azure Data Factory to transform the data and prepare it for reports. We can even push the data to Azure database for further reporting usage. It can be the source to business intelligence systems like Microsoft Power BI.

## User Interface
This was the first time I used Blazor server! I chose it because I wanted to make a simple and fast UI that can integrate with the backend easily. Because it was my first time working with Blazor I had some challenges but it turned out to be a good choice for the sake of this exercise. 

## Tests
I have added unit tests to areas that I thought are important like the backend services. However I could have added more tests, especially to the UI code. 

This application can also benefit from having some integration tests, especially because we need to work with other systems to subscribe to BSB changes and notify other teams.

# Improvements
- I am not handling errors much. This can be improved.
- There are unit tests but we can even have more.
- The UI can be improved. Especially the paging in "All BSB Numbers" and "All Requests" can be better. 
- The code in the razor pages can be improved and refactored. There is a bit of duplication.
- If I want to host this application on Azure I have multiple options. I think something like a web app or an Azure function would be an good approach here. They are scalable for future expansion as well.
- I am not sure if we need to limit the request rates but if we need, we can either do it in the app as middleware or use something like Azure API Management or Azure Frontdoor.  
-  Caching: The entire list of BSB numbers is about 15097 records at the moment. Each BSB record has 8 fields based on what I can see in the data. Each BSB record needs around 300 characters. If we dedicate 1000 characters to be safe, we should be able to store each record. Each character is taking 2 bytes, then we only need about 30Mb to store the entire BSB records. Now if the number of BSB records doubles, we still need only 60Mb. For this reason I think we can use a caching mechanism. To start we can cache the entire DB in the memory. However this approach can have some scalability problems to solve. Instead I would think of using something like Redis cache in the future. 

