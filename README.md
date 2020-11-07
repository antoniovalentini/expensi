# expensi
A simple budget management application.

## API Setup
Change these configuration parameters to test on localhost inside the appsettings.json:
```javascript
{
  "MongoDbConnection": "mongodb://localhost",
  "MongoDbName":"expensi",
  "MongoCollectionName": "expenses"
}
```
Then create a database named "expensi" and a collection named "expenses".

## DbSeed
In order to make it work, open the appsettings.json and change the following values based on you mongodb configuration:
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
