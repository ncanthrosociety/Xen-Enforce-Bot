# Xen Enforce Bot

By default, all features (except the CAPTCHA) come disabled.

Features can be enabled with the following command:

`/xenablefilter <filter name>`  | enables filter

`/xdisablefilter <filter name>`  | disables filter

so an example would be `/xenablefilter kickunverifiedmedia`

Here's a list of available filter names:

* `kickunverifiedmedia` | Will kick somebody if they post URL or media before the verification was completed
* `kickblacklisted` | Will kick somebody automatically if they are on Xenfbot's global ban list (Currently, there are no entries on the global ban list. This list is reserved for spambots, and dangerous individuals only. STRONG consideration will be taken before adding anybody to this list. This feature is DISABLED by default. I will __NEVER__ kick people from your chat for something you did not explicitly enable.)
* `kicknohandle` | Will kick somebody who joins without a handle
* `kicknoicons` | Will kick somebody who joins with no icons in their profile.
* `phraseban` | Xenfbot contains a list of common spam phrases bots will say. These phrases are very specific, and if it bans for it, it's likely that you copy-pasted botspam. Again, i'm not here to dictate your chats for you, just to stop bots. This feature is disabled by default, you must explicitly enable it.
* `verifymute` | Mutes a user until they complete verification.
* `verifyannounce` | Sets whether the bot will announce when a user passes/fails verification.
* `dontdeletejoinmessage` | Leaves the user join message when cleaning up.
* `attackmode` | Same as `/xeattackenable` / `/xeattackdisable` below.
* `mediadelay` | Require users to have been verified for a certain amount of time before being able to send any media.  Configure the delay duration in hours with `/xesetmediatime <# of hours>`.


Some other configuration commands:

`/xesetmessage <message>` | Will set the message displayed upon join, must contain `%ACTURL`.

`/xesetverifytime <time>` | sets the time a user has to verify in minutes

`/xeverify` | (CURRENTLY BROKEN) Reply to the join message (or any other message from the user that joined) with this command to manually verify them.

`/xeattackenable` | Enables attack mode, kicks all new joins with no prompt, will delete / cleanup all join messages from new joins.

`/xeattackdisable` | Disables attack mode, accepts new joins.

`/xesetlang <language code>` | A language code is for example `en`, `de`, `es`, `ru`, `fr`. Currently, only supporting English and French, but there's room for other translations.

`/xesetmediatime <# of hours>` | If the `mediadelay` feature is enabled, this sets the number of hours before a verified user can post media.

See [DEVELOPMENT](docs/DEVELOPMENT.md) for information about developing and deploying Xen Enforce Bot.
