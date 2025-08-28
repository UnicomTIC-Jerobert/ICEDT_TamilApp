#!/bin/bash
echo "Running database migrations from /var/www/tamilapp..."

# Use absolute paths for reliability
cd /var/www/tamilapp
/root/.dotnet/tools/dotnet-ef database update \
    --context ICEDT_TamilApp.Infrastructure.Data.ApplicationDbContext

if [ $? -eq 0 ]; then
    echo "Database migrations completed successfully."
else
    echo "ERROR: Database migrations failed."
    exit 1 # Fail the deployment if migration fails
fi