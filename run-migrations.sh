#!/bin/bash
set -e

git -C ./expensi pull

docker run --rm \
  -v "$(pwd)/expensi/src/Expensi.Infrastructure/Persistence/Migrations/:/flyway/sql/" \
  --env-file ./flyway.env \
  --network="host" \
  flyway/flyway migrate

exit 0
