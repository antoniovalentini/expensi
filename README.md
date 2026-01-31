# expensi
A simple expenses tracker, suitable for family budget management.

## Prepare the environment

Start a local instance of PostgreSQL. Find more info inside the [docker-compose.yml](./docker-compose.yml) file.
For instance, you can run:
```shell
docker compose up pgadmin
```

Create an `appsettings.Development.json` file or update the existing `appsettings.json` with proper values.
You can find an example below:
```json
{
  "PostgresConnection": {
    "Host": "localhost",
    "Port": 5432,
    "Database": "expensi",
    "Username": "postgres",
    "Password": "your_strong_password"
  }
}
```

Update the [run-migrations.sh](./run-migrations.sh) script with proper database connection values.

Apply database migrations using flyway:
```shell
./run-migrations.sh
```

## Run the application
```shell
dotnet build && dotnet run --no-build
```
