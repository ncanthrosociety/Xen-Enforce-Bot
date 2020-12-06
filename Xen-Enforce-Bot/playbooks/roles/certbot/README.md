# Certbot

Configure SSL/TLS on Amazon Linux 2 using Let's Encrypt
[Certbot](https://certbot.eff.org/docs/).

See additional information for Amazon Linux 2 in the
[AWS Docs](https://docs.aws.amazon.com/AWSEC2/latest/UserGuide/SSL-on-amazon-linux-2.html#letsencrypt).

## Role Variables

| Variable         | Type   | Default                       | Description                                               |
| ---------------- | ------ | ----------------------------- | --------------------------------------------------------- |
| `certbot_domain` | string | `127.0.0.1`                   | Certificate domain. Should match the Apache `ServerName`. |
| `certbot_email`  | string | `contact@ncanthrosociety.com` | Contact email for Let's Encrypt messages                  |

## Example Playbook

```yaml
- hosts: all
  tasks:
    - include_role:
      name: certbot
```

## Verification

You can verify the cert with

```
echo | openssl s_client -showcerts -servername <certbot-domain> -connect <certbot-domain>:443 2>/dev/null | openssl x509 -inform pem -noout -text
```

## Author Information

[NC Anthro Society](https://ncanthrosociety.com)
