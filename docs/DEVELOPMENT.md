# Developing Xen Enforce Bot

## Table of Contents

1. [Dependencies](#dependencies)
2. [CAPTCHA](#captcha)
   1. [hCaptcha](#hcaptcha)
   2. [reCAPTCHA](#recaptcha)
3. [Telegram Bot](#telegram-bot)
4. [Deploying](#deploying)
   1. [Deploying Manually](#deploying-manually)
   2. [Deploying on Vagrant](#deploying-on-vagrant)
   3. [Deploying With Ansible](#deploying-with-ansible)
5. [Management](#management)
6. [Backups](#backups)
   1. [Creating a Database Backup](#creating-a-database-backup)
   2. [Restoring a Database Backup](#restoring-a-database-backup)

## Dependencies

XenEnforceBot requires a several different dependencies in order to run.

Production Environment: XenEnforceBot is known to work with the following
dependencies. In general, any http server or MySQL compatible database engine
can be used. You'll also want a domain name!

| Dependency                                                                                       | Version | Description                       |
| ------------------------------------------------------------------------------------------------ | ------- | --------------------------------- |
| [Apache Server](https://httpd.apache.org/)                                                       |     2.4 | HTTP server.                      |
| [Dotnet](https://dotnet.microsoft.com/)                                                          |     3.1 | Bot programming language.         |
| [Percona XtraDB Cluster](https://www.percona.com/software/mysql-database/percona-xtradb-cluster) |     5.7 | MySQL compatible database server. |
| [PHP](https://www.php.net/)                                                                      |     7.4 | UI programming language.          |

Vagrant Environment: XenEnforceBot also provides a Vagrant configuration for
local development and testing. It is known to work with the following versions.
In addition, your hardware must support virtualization (and have that support
enabled).

| Dependency                                                                                       | Version | Description                       |
| ------------------------------------------------------------------------------------------------ | ------- | --------------------------------- |
| [Vagrant](https://www.vagrantup.com/)                                                            |     2.2 | VM configuration system.          |
| [Virtualbox](https://www.virtualbox.org/)                                                        |     6.1 | Hypervisor.                       |

Captcha services: You'll need two sign up with captcha services so that
XenEnforceBot can properly challenge people. The bot currently supports two.

- [hCaptcha](https://www.hcaptcha.com/)
- [reCAPTCHA v2](https://www.google.com/recaptcha/about/)

And of course, being a Telegram bot requires signing up for a Telegram API
key with [@botfather](https://t.me/botfather).

## CAPTCHA

### hCaptcha

[hCaptcha](https://www.hcaptcha.com/) is a free captcha service.

1. Sign up for an account and log in.
2. On the dashboard, click `+ New Site`.
   - Give the site a distinct name.
   - Add the hostname that your bot will be deployed at, if known.
3. Click `Save`.
4. Next to the site you just created, click `Settings`.
5. Stash the site key in a password manager. You'll need it later.
6. Click on your username dropdown, then click on `Settings`.
7. Stash the secret key in a password manager. You'll need it later too.

### reCAPTCHA

reCAPTCH is a free captcha service provided by Google.

1. Sign up for a Google account and log in.
2. Go to the [reCAPTCHA Admin Portal](https://www.google.com/recaptcha/admin).
3. Click the `+` button to create a new site.
   - Give the site a distinct label.
   - Select `reCAPTCHA v2` and select `"I'm not a robot" Checkbox`.
   - Add the domain that your bot will be deployed at, if known.
   - Accept the terms of service.
   - Click `Submit`.
4. Stash the site key and secret key in a password manager. You'll need them
   later.

## Telegram Bot

XenEnforceBot must have a Telegram API key to connect to Telegram. You can get
one by contacting [@botfather](https://t.me/botfather) on Telegram. Send the
`/newbot` command, then provide a display name and username when prompted.
Stash the API key in a password manager. You may also want to set a
`/description` or `/setcommands`. See [README](../README.md) for all available
commands.

The bot can be added to any group chat, and must have the `Delete Messages` and
`Ban Users` admin privileges.

## Deploying

### Deploying on Vagrant

The Vagrant configuration will use Virtualbox to spin up a fully configured VM
for local testing. All you need to do is provide the necessary keys for the
captcha services and Telegram.

To bring up the Vagrant vm, run

```
$ export xen_api_endpoint='http://127.0.0.1:8080/xen-enforce-bot/verify?actid=%s'
$ export xen_hcaptcha_secret='<hcaptcha secret>'
$ export xen_hcaptcha_sitekey='<hcaptcha site key>'
$ export xen_recaptcha_secret='<recaptcha secret>'
$ export xen_recaptcha_sitekey='<recaptcha site key>'
$ export xen_telegram_api_key='<telegram api key>'
$ vagrant up
```

Standard caveats about exporting secrets on the command line abound. You may
wish to export directly from the clipboard instead. The Vagrant up process may
take a little while to download the base VM image and then perform
configuration.

Once configuration has succeeded, verify that you can connect to the bot at
http://127.0.0.1:8080/xen-enforce-bot. You can also `vagrant ssh` to access the
local VM.

### Deploying With Ansible

XenEnforceBot also provides an [Ansible](https://www.ansible.com/).
Configuration for automating deployments. Currently, the only supported
environment is AWS running Amazon Linux 2. The `aws.yml` playbook will configure
database encryption, configure ssl/tls using Let's Encrypt, and configure
backups to an AWS S3 bucket.

For information on database encryption, see:

- [Data at Rest Encryption](https://www.percona.com/doc/percona-xtradb-cluster/5.7/management/data_at_rest_encryption.html)
- [Encrypting PXC Traffic](https://www.percona.com/doc/percona-xtradb-cluster/5.7/security/encrypt-traffic.html)
- [Encrypted Backup](https://www.percona.com/doc/percona-xtrabackup/2.4/backup_scenarios/encrypted_backup.html)

For Let's Encrypt, see:

- [Certbot](https://certbot.eff.org/docs/using.html)

For additional information on role variables and required values, see the
READMEs for each role under `Xen-Enforce-Bot/playbooks/roles`.

Running the AWS playbook will look something like this:

```
$ ansible-playbook \
    --user='<host username>' \
    --private-key='<path to host ssh key>' \
    -i '<hostname>,' \
    -e certbot_domain='<hostname>' \
    -e certbot_email='<contact email address>' \
    -e percona_aws_access_key_id='<aws access key id>' \
    -e percona_aws_secret_access_key='<aws secret access key>' \
    -e percona_aws_s3_bucket='<s3 backup bucket>' \
    -e percona_backup_encryption_key='<backup encryption key>' \
    -e percona_mysql_password='<mysql root password>' \
    -e percona_sst_password='<mysql sst password>' \
    -e xen_enforce_https="true" \
    -e xen_httpd_server_name='<hostname>' \
    -e xen_telegram_api_key='<telegram api key>' \
    -e xen_api_endpoint='https://<hostname>/xen-enforce-bot/verify?actid=%s' \
    -e xen_recaptcha_sitekey='<recaptcha sitekey>' \
    -e xen_recaptcha_secret='<recaptcha secret>' \
    -e xen_hcaptcha_sitekey='<hcaptcha sitekey>' \
    -e xen_hcaptcha_secret='hcaptcha secret' \
    -e xen_mysql_root_password='<mysql root password>' \
    -e xen_mysql_user_password='<xen mysql user password>' \
    aws.yml

```

## Management

Both automated deployments (Vagrant and AWS) run XenEnforceBot as a service.
You can check the status with `systemctl status xen-enforce-bot` and follow the
application logs with `journalctl -fu xen-enforce-bot`.

## Backups

Backups are currently only supported on AWS and are stored in a specified S3
bucket. The backup script will archive, compress, and encrypt each backup before
uploading with the Standard Infrequent Access storage class. We recommend
turning on bucket versioning and creating a storage policy to transition old
versions to Glacier and/or delete them.

The AWS playbook configures full backups to run weekly.

### Creating a Database Backup

To create a database backup, run `sudo /backups/scripts/create-backup.sh`.

### Restoring a Database Backup

Restoring a backup requires the host to be configured appropriately. You may
need to rerun Ansible to verify the configuration.

To restore a backup, first download it. You can do this using the `aws s3` CLI.
Then run `sudo /backups/scripts/restore-backup.sh <backup-file>`.
