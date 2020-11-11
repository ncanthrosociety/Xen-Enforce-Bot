# Percona XtraDB Cluster

Install [Percona XtraDB Cluster](https://www.percona.com/software/mysql-database/percona-xtradb-cluster)
and configure it to run as a single node cluster.

## Requirements

This role relies on the `community.mysql` collection.

## Role Variables

| Variable                 | Type   | Default                     | Description                                                        |
| ------------------------ | ------ | --------------------------- | ------------------------------------------------------------------ |
| `percona_product_code`   | string | `pxc57`                     | Percona product code. This is the value used by `percona-release`. |
| `percona_package_name`   | string | `Percona-XtraDB-Cluster-57` | Name of the yum package to install.                                |
| `percona_mysql_username` | string | `root`                      | Username of the default root/admin user.                           |
| `percona_mysql_password` | string |                             | Password of the default root/admin user.                           |
| `percona_sst_username`   | string | `sstuser`                   | Username of the Percona sst user.                                  |
| `percona_sst_password`   | string |                             | Password of the Percona sst user.                                  |

## Example Playbook

```yaml
- hosts: all
  tasks:
    - include_role:
      name: percona-xtradb-cluster
```

## Author Information

[NC Anthro Society](https://ncanthrosociety.com)
