# Percona S3 Backups

Configure Percona XtraDB backups stored in Amazon S3.

## Role Variables

| Variable                        | Type   | Default                                                                                  | Description                                                                                                                |
| ------------------------------- | ------ | ---------------------------------------------------------------------------------------- | -------------------------------------------------------------------------------------------------------------------------- |
| `percona_aws_cli_version`       | string | `2.1.30`                                                                                 | AWS CLI version.                                                                                                           |
| `percona_aws_cli_url`           | string | `https://awscli.amazonaws.com/awscli-exe-linux-x86_64-{{ percona_aws_cli_version }}.zip` | Download url for the AWS cli.                                                                                              |
| `percona_aws_region`            | string | `us-east-1`                                                                              | Default AWS region for the user.                                                                                           |
| `percona_aws_access_key_id`     | string |                                                                                          | Access key id for the AWS user used to push backups to S3.                                                            |
| `percona_aws_secret_access_key` | string |                                                                                          | Access key for the AWS user used to push backups to Glacier.                                                               |
| `percona_aws_s3_bucket`         | string |                                                                                          | Name of the s3 bucket to place backup files in.                                                                            |
| `percona_backup_encryption_key` | string |                                                                                          | Encryption key for backups. See https://www.percona.com/doc/percona-xtrabackup/2.4/backup_scenarios/encrypted_backup.html. |

## S3 Backups

Backups will be stored at `{{ percona_aws_s3_bucket }}/bots/xen-enforce-bot/backup.tar.gz.xbcrypt`
using the `STANDARD_IA` storage class. This will overwrite the previous backups
unless the bucket enables versioning.

We recommend creating a bucket and lifecycle policy to automatically archive and
expire old backups, such as the following [Terraform](https://terraform.io)
config.

```hcl
resource "aws_s3_bucket" "backups" {
  bucket = "<bucket name>"
  acl    = "private"

  versioning {
    enabled = true
  }

  lifecycle_rule {
    enabled = true

    transition {
      days = 30
      storage_class = "GLACIER"
    }

    noncurrent_version_transition {
      storage_class = "GLACIER"
    }

    noncurrent_version_expiration {
      days = 90
    }
  }
}
```

## Example Playbook

```yaml
- hosts: all
  tasks:
    - include_role:
      name: percona_s3_backups
```

## Author Information

[NC Anthro Society](https://ncanthrosociety.com)
