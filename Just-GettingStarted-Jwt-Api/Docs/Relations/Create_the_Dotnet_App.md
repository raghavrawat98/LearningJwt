# Create_the_Dotnet_App
Solution name = `LearningJwt`
Project name = `Just-GettingStarted-Jwt-Api`

Following tutorials of
[Microsoft's Link](https://learn.microsoft.com/en-us/aspnet/core/tutorials/first-mongo-app?view=aspnetcore-10.0&tabs=visual-studio)

[MongoDB ... but I didn't follow](https://www.mongodb.com/docs/drivers/csharp/current/crud/restful-api-tutorial/)

``` json
"AuthMongoDB": {
  "ConnectionString": "*************",
  "Database": "***************",
  "Collection": "users"
}
```

![alt text](../Assets/Pasted%20image%2020260408011827.png)

I am using Auth MongoDB everywhere, later I'll remove because it feels more readable. <br>

For Register I want to check if user exists or not <br>
[Stack Overflow effiecient way](https://stackoverflow.com/questions/72629886/efficient-way-to-check-if-username-email-already-exist-in-mongodb-for-registrati)

Didn't got much so I checked <br>
[Gemini: How to check if user exists](https://gemini.google.com/share/688e4178d325)

I found this thing <br>
even I wrapped everything try catch was still failing <br>
[On debugging this needs to be dealt as](https://gemini.google.com/share/d920913594fe)

It failed in constructor because client is lazy. <br>
So when req starts and it tries to initialize db client <br>
it fails unhandily <br>

[|-ConnString showing in API req](ConnString_showing_in_API_req.md)

[|-Resuming Back](../Journey.md)