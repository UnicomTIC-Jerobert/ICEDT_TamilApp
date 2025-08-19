#!/bin/bash
# This script sets ownership for all application and database files.

# Set ownership for the main application directory
chown -R www-data:www-data /var/www/tamilapp

# Create the database directory if it doesn't exist
mkdir -p /var/www/database

# Move the new database file from the deployment package to its final location
mv /var/www/tamilapp/tamilapp.db /var/www/database/tamilapp.db

# Set ownership for the database directory and the file inside it
chown -R www-data:www-data /var/www/database