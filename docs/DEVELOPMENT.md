# Developing Xen Enforce Bot

## Table of Contents

1. [Dependencies](#dependencies)
2. [CAPTCHA](#captcha)
   1. [hCaptcha](#hcaptcha)
   2. [reCAPTCHA](#recaptcha)
3. [Telegram Bot](#telegram-bot)
4. [Deploying](#deploying)
   1. [Deploying With Docker Compose](#deploying-with-docker-compose)
5. [Management](#management)
6. [Code Linting with ReSharper CLI Tools and EditorConfig](#Code-Linting-using-ReSharper-CLI-Tools-and-EditorConfig)

## Dependencies

XenEnforceBot requires a several different dependencies in order to run.

Production Environment: XenEnforceBot is known to work with the following
dependencies. In general, any http server or MySQL compatible database engine
can be used. You'll also want a domain name!

| Dependency                                                                                       | Version | Description                       |
| ------------------------------------------------------------------------------------------------ | ------- | --------------------------------- |
| [nginx](https://nginx.org/)                                                                      |    1.23 | HTTP server.                      |
| [Dotnet](https://dotnet.microsoft.com/)                                                          |     3.1 | Bot programming language.         |
| [Percona XtraDB Cluster](https://www.percona.com/software/mysql-database/percona-xtradb-cluster) |     5.7 | MySQL compatible database server. |
| [PHP](https://www.php.net/)                                                                      |     8.5 | UI programming language.          |

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

### Deploying With Docker Compose

`docker-compose.yaml` at the repo root runs the whole stack in containers:
`nginx` (serves the PHP verify webui), `php` (php-fpm), `database`
(Percona XtraDB Cluster, MySQL-compatible), and `xen-enforce-bot` (the
.NET bot itself).

This assumes an external reverse proxy is already running on the host and
terminating TLS -- specifically an
[`nginx-proxy`](https://github.com/nginx-proxy/nginx-proxy) +
[`acme-companion`](https://github.com/nginx-proxy/acme-companion) stack,
the same convention this org uses for other services (e.g. `taiga-board`).
This stack's own `nginx` service doesn't publish port 80/443 to the
host; it advertises itself to the proxy over a shared Docker network via
`VIRTUAL_HOST`/`LETSENCRYPT_HOST`/`LETSENCRYPT_EMAIL`. Bring that proxy
stack up first, with a Docker network named `nginx-proxy`, before starting
this one.

1. `cp .env.example .env` and fill in real values. Read the comments in
   that file carefully -- the database username, password, and database
   name each have to be duplicated under several *different* variable
   names, because the .NET bot, the PHP webui, and the MySQL-compatible
   database image each read their own naming convention (e.g. the DB
   password is `MYSQL_USER_PASSWORD` for the PHP webui, `MySQLPassword`
   for the bot, and `MYSQL_PASSWORD` for the database container). They must
   all hold the same value or the services won't be able to authenticate
   to each other.
2. Set `DOMAIN` to the public hostname this bot's verify page will be
   served at, and `APIEndpoint` to `https://<that domain>/verify?actid=%s`.
3. Bring the stack up: `docker compose up -d --build`.
4. `docker compose logs xen-enforce-bot` should show it connecting to
   Telegram and to the database without restart-looping.

## Management

XenEnforceBot runs as the `xen-enforce-bot` Docker Compose service. Check
status with `docker compose ps` and follow the application logs with
`docker compose logs -f xen-enforce-bot`.

## Code Linting using ReSharper CLI Tools and EditorConfig

We are testing the use of ReSharper CLI Tools to lint the code and enforce a
consistent style.  ReSharper supports the use of a `.editorconfig` file to
specify settings.  Many IDEs also support using `.editorconfig` for specifying
standard IDE options for code style.

### ReSharper Instalation

Follow instructions [here](https://www.jetbrains.com/help/resharper/ReSharper_Command_Line_Tools.html#1d48a574)
for installation of ReSharper CLI Tools. Make sure to add the dotnet tool default
location to your PATH env var. On Linux and WSL, this should be `$HOME/.dotnet/tools`.

### ReSharper Usage

If you installed ReSharper using the above instructions, use this command to run the CleanupCode
tool from inside the project directory:

`jb cleanupcode Xen-Enforce-Bot.sln`

Note, ReSharper does not lint the Lua files.

### ReSharper using EditorConfig

As stated in the ReSharper CLI Tools docs, "ReSharper understands
[some](https://www.jetbrains.com/help/resharper/Using_EditorConfig.html#standard) standard
[EditorConfig](https://editorconfig.org) properties, most frequently used
[.NET-coding-convention EditorConfig properties](https://docs.microsoft.com/en-us/dotnet/fundamentals/code-analysis/code-style-rule-options?view=vs-2019), and provides a set of
[custom EditorConfig properties](https://www.jetbrains.com/help/resharper/EditorConfig_Index.html),
which allow for much more granular configuration of formatting, syntax,
and code inspection rules..."

#### VSCode EditorConfig plugin

VSCode needs a plugin to support overriding user settings with EditorConfig options. The official plugin can be found [here](https://marketplace.visualstudio.com/items?itemName=EditorConfig.EditorConfig).
