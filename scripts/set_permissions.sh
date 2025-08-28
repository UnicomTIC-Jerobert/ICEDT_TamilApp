#!/bin/bash
# Set ownership for the application directory
chown -R www-data:www-data /var/www/tamilapp

# We still ensure the database directory exists and has correct ownership
mkdir -p /var/www/database
chown -R www-data:www-data /var/www/database