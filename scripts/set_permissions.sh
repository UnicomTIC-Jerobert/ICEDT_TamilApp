#!/bin/bash
# Set ownership for the application directory
echo "Setting permissions for /var/www/tamilapp..."
chown -R www-data:www-data /var/www/tamilapp
chmod -R 755 /var/www/tamilapp

# We still ensure the database directory exists and has correct ownership
mkdir -p /var/www/database
echo "Setting permissions for /var/www/database..."
chown -R www-data:www-data /var/www/database

echo "Permissions set."