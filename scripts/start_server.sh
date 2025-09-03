#!/bin/bash
echo "Running ApplicationStart hook..."
# Start the systemd service
systemctl start icedt_tamilapp.service
# Optional: Check status to ensure it started correctly
systemctl status icedt_tamilapp.service --no-pager
echo "Finished ApplicationStart hook."