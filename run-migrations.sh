#!/bin/bash

set -e

docker run --rm \
  -v "$(pwd)/src/Expensi.Infrastructure/Persistence/Migrations/:/flyway/sql/" \
  --env-file ./flyway.env \
  --network="host" \
  flyway/flyway migrate

exit 0
