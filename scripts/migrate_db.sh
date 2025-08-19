#!/bin/bash
# This script runs EF Core migrations.
# It MUST be run from the application's root directory: /var/www/tamilapp

echo "Running database migrations from /var/www/tamilapp..."

# Use the --project and --startup-project flags to be explicit and robust.
# This ensures the command works regardless of where the script is called from.
/root/.dotnet/tools/dotnet-ef database update \
    --project /var/www/tamilapp/ICEDT_TamilApp.Infrastructure.csproj \
    --startup-project /var/www/tamilapp/ICEDT_TamilApp.Web.csproj