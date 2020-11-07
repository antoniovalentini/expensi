# expensi
A simple budget management application.

## API Setup
Add these configuration parameters to test on localhost:
```javascript
{
  "MongoDbConnection": "mongodb://localhost",
  "MongoDbName":"expensi",
  "MongoCollectionName": "expenses"
}
```
Then create a database named "expensi" and a collection named "expenses".

## DbSeed
In order to make it work, create a file appsettings_secret.json and set it to be copied to output directory.
Then paste the following inside it:
```javascript
{
  "MongoDbConfig": {
    "connectionstring": "<your_connectionstring>",
    "database": "<your_database>",
    "collection": "<your_collection>"
  }
}
```

**WARNING**
DbSeed application will erase the collection you define inside the appsettings_secret.json.
