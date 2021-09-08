# Black Box Test Plan

## Feature Flags

### Preconditions

The bot is running, has been added to a chat, and the bot has the permissions for Deleting messages, and Banning users.

### Important Note

When the bot kicks a user it also bans them from the chat, preventing them from re-joining using an invite link.  Between tests when your user is kicked, go to "Manage Group" --> "Permissions" --> "Removed Users", and delete the user from that list, so they can easily re-join.

### Tests

| Test                             | Steps                                                                                                                                                                       | Expected                                                                                                                               | Actual |
|----------------------------------|-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------|----------------------------------------------------------------------------------------------------------------------------------------|--------|
| Set Join Message                 | Use the command `/xesetmessage` followed by the join message.  Join message must contain `%ACTURL` for the link for the user to be verified.                                |                                                                                                                                        |        |
| Set Verify Time                  | Use the command `/xesetverifytime` followed by an integer.  This will set the time a user has to verify in minutes.                                                         |                                                                                                                                        |        |
| User Joining gets Verify message | After setting the Join Message and Verify Time, have a user join the chat.                                                                                                  | Join message is posted to the chat.                                                                                                    |        |
| Verify time - kick               | Once the user gets the Join message, they should NOT verify.  Instead, wait out the verify time so the bot auto-kicks the user.                                             | User is kicked for not completing verification.                                                                                        |        |
| Verify time - join               | Re-add the user to the chat.  At this point, they should complete the verification.                                                                                         | After verification, the bot should post a message confirming they verified. A few minutes later the bot should clean up it's messages. |        |
| Attack Enable                    | Have the user leave the chat.  Use the command `/xeattackenable`.  Have the user attempt to rejoin the chat.                                                                | User should be auto-kicked on join.                                                                                                    |        |
| Attack Disable                   | Use the command `/xeattackdisable`.  Have the user attempt to rejoin the chat.                                                                                              | User should be able to join, and will be prompted to complete verification.                                                            |        |
| Set Language                     | Use the command `/xesetlang` followed by a language code.  For testing, the `fu` code for the furry language pack can be used.  After test success, change it back to `en`. | Bot should post a message about the language change in the new language.                                                               |        |
---

## Filters

### Preconditions

Same preconditions as for Feature Flags.

### Tests

| Test                      | Steps                                                                                                                                                                                                                               | Expected                                                                 | Actual |
|---------------------------|-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|--------------------------------------------------------------------------|--------|
| Kick Unverified Media     | Use the command `/xenablefilter kickunverifiedmedia`.  Have a user join the chat, then try to post a URL or gif.  After a successful test, use the command `/xdisablefilter kickunverifiedmedia`.                                   | User is kicked.                                                          |        |
| Kick Blacklisted          | Filter will kick users that join that are on the Xenfbot's global ban list.  Not implemented.                                                                                                                                       | N/A                                                                      |        |
| Kick No Handle            | Filter will kick users that join without a handle (ex. "@myHandle").  Not easily testable.                                                                                                                                          | N/A                                                                      |        |
| Kick No Icons             | Filter will kick users that join without an icon.  Not easily testable.                                                                                                                                                             | N/A                                                                      |        |
| Phrase Ban                | Use the command `/xenablefilter phraseban`.  Have a user join the chat, then paste:<br>"His name is Dylan Phillips from Michigan"<br>After a successful test, use command `/xdisablefilter phraseban`.                              | User is kicked.                                                          |        |
| Verify Mute               | Use the command `/xenablefilter verifymute`.  Have a user join the chat, and attempt to type something.  Have them complete verification, then try typing again. After a successful test, use command `/xdisablefilter verifymute`. | User should be muted, and unable to chat until verification is complete. |        |
| Don't Delete Join Message | Use the command `/xenablefilter dontdeletejoinmessage`.  Have a user join the chat. After a successful test, use command `/xdisablefilter dontdeletejoinmessage`.                                                                   | Bot should not delete the join message after verification.               |        |