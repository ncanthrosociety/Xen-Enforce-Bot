namespace XenfbotDN {
    public class TGUser {
        public string first_name;
        public long id;
        public bool is_bot;
        public string language_code;
        public string last_name;
        public string username;
    }

    public class TGResponse {
        public bool ok;
        public JToken result;
    }

    public class TGUpdate {
        public TGMessage edited_message;
        public TGMessage message;
        public long update_id;
    }

    public class TGChat {
        public bool all_members_are_administrators;
        public string can_set_sticker_set;
        public string description;
        public string firstname;
        public long id;
        public string invite_link;
        public string lastname;
        public object photo; //!!
        public object pinned_message; //!!
        public string sticker_set_name;
        public string title;
        public string type;
        public string username;

        public TGMessage sendMessage(string text) {
            return Telegram.sendMessage(this, text);
        }

        public TGMessage sendMessage(string text, string markdown) {
            return Telegram.sendMessage(this, text, markdown);
        }
    }

    public class TGChatMember {
        public bool can_add_web_page_previews;
        public bool can_be_edited;
        public bool can_change_info;
        public bool can_delete_messages;
        public bool can_edit_messages;
        public bool can_invite_users;
        public bool can_pin_messages;
        public bool can_post_messages;
        public bool can_promote_members;
        public bool can_restrict_members;
        public bool can_send_media_messages;
        public bool can_send_messages;
        public bool can_send_other_messages;
        public bool is_member;
        public string status;
        public int until_date;
        public TGUser user;
    }

    public class TGProfilePhotos {
        public int total_count;
    }

    public class TGPhotoSize {
        public string file_id;
        public int file_size;
        public int height;
        public int width;
    }

    public class TGVideo {
        public string file_id;
        public string file_unique_id;
    }

    public class TGVideoNote {
        public string file_id;
        public string file_unique_id;
    }

    public class TGDocument {
        public string file_id;
        public string file_unique_id;
    }

    public class TGMessage {
        public TGChat chat;
        public int date;
        public TGDocument document;
        public int edit_date;
        public int forward_date;
        public TGUser forward_from;
        public TGChat forward_from_chat;
        public int forward_from_message_id;
        public string forward_sender_name;
        public string forward_signature;
        public TGUser from;
        public long message_id;
        public TGUser[] new_chat_members;
        public TGPhotoSize[] photo;
        public TGMessage reply_to_message;
        public string text;
        public TGVideo video;
        public TGVideoNote video_note;

        public TGMessage replySendMessage(string text) {
            if (chat != null) {
                return Telegram.sendMessage(chat, text);
            }

            Helpers.warn("[!] Tried to reply to a message that has no chat.");
            return null;
        }

        public TGMessage replyLocalizedMessage(string code, string locstring, params object[] data) {
            var message = Localization.getStringLocalized(code, locstring, data);
            if (chat != null) {
                return Telegram.sendMessage(chat, message);
            }

            Helpers.warn("[!] Tried to reply to a message that has no chat.");
            return null;
        }

        public TGMessage replyLocalizedMessage(string code, string locstring) {
            var message = Localization.getStringLocalized(code, locstring);
            if (chat != null) {
                return Telegram.sendMessage(chat, message);
            }

            Helpers.warn("[!] Tried to reply to a message that has no chat.");
            return null;
        }

        public bool delete() {
            if (chat != null) {
                return Telegram.deleteMessage(chat, this);
            }

            Helpers.warn("[!] Tried to delete a message from a null chat.");
            return false;
        }

        public bool isSenderAdmin() {
            var ChatMem = Telegram.getChatMember(chat, from);
            if (ChatMem.status == "creator" || ChatMem.status == "admin" || ChatMem.status == "administrator") {
                return true;
            }

            return false;
        }
    }
}
