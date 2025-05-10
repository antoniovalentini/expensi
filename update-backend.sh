#!/bin/bash
set -e

echo "Attempting to stop Expensi backend container (if it exists)..."
docker stop expensi-backend || true
echo "Attempting to remove Expensi backend container (if it exists)..."
docker rm expensi-backend || true
echo "Pulling the latest Expensi image..."
docker pull avdockie/expensi-backend:latest
echo "Starting new Expensi backend container..."
docker run \
  -d -p 5008:5008 \
  --name=expensi-backend \
  --restart=always \
  -e ASPNETCORE_URLS="http://*:5008" \
  -e PostgresConnection__Host="$EXPENSI_HOST" \
  -e PostgresConnection__Database="$EXPENSI_DB" \
  -e PostgresConnection__Username="$FLYWAY_USER" \
  -e PostgresConnection__Password="$FLYWAY_PASSWORD" \
  --network="expensi_default" \
  avdockie/expensi-backend:latest
echo ""
echo "Expensi backend deployment/update complete."
exit 0
