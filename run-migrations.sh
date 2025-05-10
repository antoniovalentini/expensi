#!/bin/bash

set -e

docker run --rm \
  -v "$(pwd)/src/Expensi.Infrastructure/Persistence/Migrations/:/flyway/sql/" \
  -e FLYWAY_URL \
  -e FLYWAY_USER \
  -e FLYWAY_PASSWORD \
  --network="host" \
  flyway/flyway migrate

exit 0
