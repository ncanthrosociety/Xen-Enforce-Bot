#!/usr/bin/env bash


# Usage: ./restore-backup.sh [backup-file]
#
# Must have necessary permissions to read the mysql data dir and create temp
# directories (typically root, so run with sudo).
#
# If a backup file is not provided, the default /tmp/backup.tar.gz will
# be used. Backups are assumed to be compressed and encrypted and will always be
# decrypted and decompressed (see the create backup script for details).
#
# A backup copy of the data dir will be created before restoring. If the restore
# fails, the copy will be left on disk. If the restore succeeds, then the backup
# will be automatically removed.
#
# The original backup file will not be removed after restoring. Be sure to
# delete it when done.


# Load common config and then set custom config.
source "$(dirname $0)/common.sh"
export BACKUP_FILE_XBCRYPT="${1:-/tmp/backup.tar.gz.xbcrypt}"
export BACKUP_FILE="/tmp/backup.tar.gz"
export DATA_DIR_BAK="$DATA_DIR.bak"


# Stop MySQL and back up the data dir if it exists.
systemctl stop mysql
if [[ -d "$DATA_DIR" ]]; then
    rm -rf "$DATA_DIR_BAK"
    mv "$DATA_DIR" "$DATA_DIR_BAK"
fi


# 1. Recreate and move into the backup directory.
# 2. Decrypt the backup.
# 3. Unarchive the backup.
# 4. Copy the kyring file to the correct location.
# 5. Prepare the backup.
# 6. Restore the backup.
# 7. Reset the working directory.
rm -rf "$BACKUP_FILE" "$BACKUP_DIR"
mkdir -p "$BACKUP_DIR"
pushd "$BACKUP_DIR"
xbcrypt --decrypt --encrypt-algo="$ENCRYPT_ALGORITHM" --encrypt-key-file="$ENCRYPT_KEY_FILE" --input="$BACKUP_FILE_XBCRYPT" --output="$BACKUP_FILE"
tar -xvf "$BACKUP_FILE"
cp 'keyring' "$KEYRING_FILE"
xtrabackup --prepare --target-dir="$BACKUP_DIR" --keyring-file-data="$KEYRING_FILE"
xtrabackup --copy-back --data-dir="$DATA_DIR" --target-dir="$BACKUP_DIR"
popd


# Fix the data directory permissions post-backup and start MySQL.
chown -R mysql:mysql "$DATA_DIR"
chmod 751 "$DATA_DIR"
systemctl start mysql


# Remove temporary backup files and directories.
rm -rf "$BACKUP_FILE" "$BACKUP_DIR" "$DATA_DIR_BAK"
