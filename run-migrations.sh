#!/bin/bash

set -e

export FLYWAY_URL="jdbc:postgresql://localhost:5432/expensi"
export FLYWAY_USER="postgres"
export FLYWAY_PASSWORD="your_strong_password"

docker run --rm \
  -v "$(pwd)/src/Expensi.Infrastructure/Persistence/Migrations/:/flyway/migrations/" \
  -e FLYWAY_URL \
  -e FLYWAY_USER \
  -e FLYWAY_PASSWORD \
  --network="host" \
  flyway/flyway:12.0.0 migrate -locations=filesystem:/flyway/migrations

exit 0
