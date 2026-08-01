# ConnString_showing_in_API_req
When code looked like
![alt text](../Assets/Pasted%20image%2020260408031154.png)

Error was this
![alt text](../Assets/Pasted%20image%2020260408031245.png)

I did changes like
![alt text](../Assets/Pasted%20image%2020260408031439.png)

so now error is coming as 
![alt text](../Assets/Pasted%20image%2020260408031551.png)

Livable for now 

a bit more optimization 
``` csharp
var builder = WebApplication.CreateBuilder(args);

// Bind settings and register MongoClient as a Singleton
builder.Services.Configure<MongoDbSettings>(builder.Configuration.GetSection("MongoDbSettings"));
builder.Services.AddSingleton<IMongoClient>(sp => {
    var settings = sp.GetRequiredService<IOptions<MongoDbSettings>>().Value;
    return new MongoClient(settings.ConnectionString);
});

// Register IMongoDatabase as Scoped
builder.Services.AddScoped(sp => {
    var settings = sp.GetRequiredService<IOptions<MongoDbSettings>>().Value;
    var client = sp.GetRequiredService<IMongoClient>();
    return client.GetDatabase(settings.DatabaseName);
});

```

[|-Resuming Back](../Journey.md)