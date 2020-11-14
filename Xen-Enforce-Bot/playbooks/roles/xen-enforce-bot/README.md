# Xen Enforce Bot

Install [Xen Enforce Bot](https://github.com/ncanthrosociety/Xen-Enforce-Bot) and run it as a service.

## Requirements

This role relies on the `community.general` and `community.mysql` collections.

## Role Variables

| Variable                  | Type   | Default       | Description                                                                                        |
| ------------------------- | ------ | ------------- | -------------------------------------------------------------------------------------------------- |
| `xen_api_endpoint`        | string |               | Xen Enforce Bot UI template URL. See `config.example.ini` for the expected format.                 |
| `xen_mysql_host`          | string | 127.0.0.1     | MySQL hostname.                                                                                    |
| `xen_mysql_root_username` | string | `root`        | MySQL root/admin username.                                                                         |
| `xen_mysql_root_password` | string |               | MySQL root/admin password.                                                                         |
| `xen_mysql_user_username` | string | `enforce_bot` | MySQL Xen Enforce Bot username.                                                                    |
| `xen_mysql_user_host`     | string | `localhost`   | MySQL Xen Enforce Bot user host. For use with MySQL's `user@host` restrictions.                    |
| `xen_mysql_user_password` | string |               | MySQL Xen Enforce Bot user password.                                                               |
| `xen_mysql_database`      | string | `xenfbot`     | MySQL Xen Enforce Bot database name. Should correspond to the in `xenfbot4schema.sql`.             |
| `xen_mysql_verify_table`  | string | `verify`      | MySQL Xen Enforce Bot verification table. Should correspond to the schema in `xenfbot4schema.sql`. |
| `xen_recaptcha_sitekey`   | string |               | [reCAPTCHA](https://developers.google.com/recaptcha/intro) sitekey.                                |
| `xen_recaptcha_secret`    | string |               | [reCAPTCHA](https://developers.google.com/recaptcha/intro) secret.                                 |
| `xen_hcaptcha_sitekey     | string |               | [hCaptcha](https://docs.hcaptcha.com/) sitekey.                                                    |
| `xen_hcaptcha_secret`     | string |               | [hCaptcha](https://docs.hcaptcha.com/) secret.                                                     |
| `xen_is_vagrant`          | bool   | `false`       | For use by vagrant to skip cloning the project (vagrant auto mounts into the vm).                  |
| `xen_telegram_api_key`    | string |               | [Telegram](https://core.telegram.org/bots) API key from @Botfather.                                |

## Example Playbook

```yaml
- hosts: all
  tasks:
    - name: Deploy Xen Enforce Bot
      include_role:
        name: xen-enforce-bot
      vars:
        xen_api_endpoint: "<endpoint>"
        xen_recaptcha_sitekey: "<sitekey>"
        xen_recaptcha_secret: "<secret>"
        xen_hcaptcha_sitekey: "<sitekey>"
        xen_hcaptcha_secret: "<secret>"
        xen_telegram_api_key: "<apikey>"
```

## Author Information

[NC Anthro Society](https://ncanthrosociety.com)
