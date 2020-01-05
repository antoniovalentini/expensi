# expensi
A simple budget management application.

## API Setup
Add these configuration parameters to test on localhost:
```javascript
{
  "MongoDbConfig": "mongodb://localhost",
  "Authority": "<your_authority_domain>",
  "Audience": "<your_audience>",
}
```
Then create a database named "expensi" and a collection named "expenses".
In order to quick start the project with authentication, you can create a free account on [Auth0](https://auth0.com/).

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