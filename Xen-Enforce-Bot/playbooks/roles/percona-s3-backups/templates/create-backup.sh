#!/usr/bin/env bash


# Usage: ./create-backup.sh [backup-file]
#
# Must have necessary permissions to read the mysql data dir and create temp
# directories (typically root, so run with sudo).
#
# If a backup file is not provided, the default /tmp/backup-<time>.tar.gz.xbcrypt
# will be used. Backups will always be compressed and encrypted. The backup file
# will be removed after successfully pushing to s3.


# Load common config and then set custom config.
source "$(dirname $0)/common.sh"
export BACKUP_FILE="${1:-/tmp/backup-$(date --iso-8601=seconds).tar.gz}"
export BACKUP_FILE_XBCRYPT="$BACKUP_FILE.xbcrypt"


# 1. Remove any old backup files.
# 2. Create and move into the backup directory.
# 3. Create the backup.
# 4. Copy the keyring file into the backup (needed for MySQL encryption).
# 5. Archive the backup.
# 6. Encrypt the backup.
rm -rf "$BACKUP_FILE" "$BACKUP_FILE_XBCRYPT" "$BACKUP_DIR"
mkdir -p "$BACKUP_DIR"
pushd "$BACKUP_DIR"
xtrabackup --backup --data-dir="$DATA_DIR" --keyring-file-data="$KEYRING_FILE" --target-dir="$BACKUP_DIR"
cp "$KEYRING_FILE" .
tar -zcvf "$BACKUP_FILE" .
xbcrypt --encrypt-algo="$ENCRYPT_ALGORITHM" --encrypt-key-file="$ENCRYPT_KEY_FILE" --input="$BACKUP_FILE" --output="$BACKUP_FILE_XBCRYPT"
popd


# Push backups to S3.
/usr/local/bin/aws s3 cp --storage-class='STANDARD_IA' "$BACKUP_FILE_XBCRYPT" s3://{{ percona_aws_s3_bucket }}/bots/xen-enforce-bot/backup.tar.gz.xbcrypt


# Remove temporary backup files and directories.
rm -rf "$BACKUP_FILE" "$BACKUP_FILE_XBCRYPT" "$BACKUP_DIR"
