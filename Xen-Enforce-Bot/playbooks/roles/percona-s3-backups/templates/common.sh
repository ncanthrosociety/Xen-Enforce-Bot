# Common backup script definitions.


set -xeuo pipefail


export DATA_DIR="/var/lib/mysql"
export BACKUP_DIR="/tmp/backup"
export ENCRYPT_ALGORITHM="AES256"
export ENCRYPT_KEY_FILE="/backups/key"
export KEYRING_FILE="/var/lib/mysql-keyring/keyring"
