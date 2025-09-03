#!/bin/bash
echo "Setting permissions for /var/www/tamilapp..."
chown -R www-data:www-data /var/www/tamilapp
chmod -R 755 /var/www/tamilapp

echo "Setting permissions for /var/www/database..."
# Ensure the database directory exists
mkdir -p /var/www/database
# Set ownership for the database directory
chown -R www-data:www-data /var/www/database
# Set read/write permissions for the database files
chmod -R 660 /var/www/database/*.db || true # Ensure files are writable by owner/group
chmod -R 770 /var/www/database || true # Ensure directory is accessible by owner/group
echo "Permissions set."