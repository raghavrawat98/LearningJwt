# Updating things in Mongo DB

[The MongoDb Link](https://www.mongodb.com/docs/drivers/csharp/current/crud/update-one/fields/)

Gemini

Method 1. Update a Single Property
```csharp
var filter = Builders<MyDocument>.Filter.Eq(d => d.Id, someId);
var update = Builders<MyDocument>.Update.Set(d => d.MyProperty, "New Value");

await collection.UpdateOneAsync(filter, update);

```

Method 2 Update Multiple Properties
```csharp
var update = Builders<MyDocument>.Update
    .Set(d => d.Status, "Active")
    .Set(d => d.LastModified, DateTime.UtcNow);

await collection.UpdateOneAsync(filter, update);
```

Method 3 Update Properties in an Array
```csharp
// Matches document with ID and the specific array element having ID 123
var filter = Builders<MyDocument>.Filter.And(
    Builders<MyDocument>.Filter.Eq(d => d.Id, documentId),
    Builders<MyDocument>.Filter.Eq("Items.Id", 123)
);

// Updates the first matched item in the "Items" array
var update = Builders<MyDocument>.Update.Set("Items.$.Price", 25.00);

await collection.UpdateOneAsync(filter, update);
```

Method 4 Replace an Entire Document
```csharp
var filter = Builders<MyDocument>.Filter.Eq(d => d.Id, updatedObject.Id);
await collection.ReplaceOneAsync(filter, updatedObject);

```

[|-Back to Token Problem](Token%20Generation%20problem%20-%20Didn't%20Saw%20this%20comming.md)