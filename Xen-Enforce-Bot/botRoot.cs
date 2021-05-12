using System;
using System.Collections.Generic;
using System.Threading;

namespace XenfbotDN
{
    public static class botRoot
    {
        private static long lastUpdate;
        private static Dictionary<long, bool> groupError = new Dictionary<long, bool>();
        private static bool allowProcessUpdates;

        public static void Enter()
        {
            while (true)
                try
                {
                    processUpdates();
                    Verify.runTask();
                    Cleanup.runTask();
                }
                catch (Exception E)
                {
                    Helpers.writeStack(E.ToString());
                    Thread.Sleep(500);
                }
        }

        public static void processUpdates()
        {
            // When the bot starts, lastUpdate is 0.  Telegram will return all updates from the last 24 hours.
            // We grab the largest update_id + 1 for the next poll.  When this is called with that value, it confirms
            // to the Telegram server that we've received all the previous updates, causing Telegram to stop returning them, effectively skipping them.
            // In between polls, we are hoping no other updates occur, and if so then we finally set allowProcessUpdates to true.

            // TODO(tasonosenshi): Write a proper startup function to call once before starting the while loop.
            var up = Telegram.getUpdates(lastUpdate);
            if (up == null)
            {
                Console.WriteLine("TGAPI Response failure update==null");
                return;
            }

            Console.WriteLine("Updates: {0}", up.Length);
            if (up.Length == 0) allowProcessUpdates = true;

            for (var i = 0; i < up.Length; i++)
            {
                var currentUpdate = up[i];
                if (currentUpdate.update_id >= lastUpdate) lastUpdate = currentUpdate.update_id + 1;

                if (currentUpdate.edited_message != null) currentUpdate.message = currentUpdate.edited_message; // ahax.

                if (allowProcessUpdates)
                {
#if DEBUG
                    // TODO(tasonosenshi): Use a proper logging library instead of compile time flags?
                    // Console.WriteLine(JsonConvert.SerializeObject(currentUpdate));
#endif

                    // Why is this extra block here?
                    {
                        if (currentUpdate.message != null)
                            try
                            {
                                processIndividualUpdate(currentUpdate);
                            }
                            catch (Exception E)
                            {
                                Helpers.writeStack(E.ToString());
                            }
                    }
                }
                else
                {
                    Console.WriteLine("Skipping update due to startup condition....");
                }
            }
        }

        public static void processIndividualUpdate(TGUpdate update)
        {
            var msg = update.message;
            if (msg.from.is_bot) // Don't process updates from other bots
                return;

            var langcode = "en"; // default language is english
            var gc = GroupConfiguration.getConfig(update.message.chat.id);
            var VFD = Verify.getVerifyData(update.message.from, update.message.chat, update.message);
            var doubt = Verify.checkDoubt(update.message.from, update.message.chat);
            // Do captcha
            if (msg.new_chat_members != null)
            {
                var ncm = msg.new_chat_members;
                for (var i = 0; i < ncm.Length; i++)
                {
                    if (ncm[i].username == root.botUsername)
                    {
                        var cl1 = Localization.getLanguageInfo(langcode);
                        var cl = Localization.getStringLocalized(langcode, "locale/currentLangName");
                        var smsg = Localization.getStringLocalized(langcode, "basic/xenfbot",
                            " BRN 'MASTER' 4.0.8 (Noodle Dragon) ", cl, cl1.authors, cl1.version,
                            "contact@ncanthrosociety.com");
                        smsg += "\n\n";
                        smsg += Localization.getStringLocalized(langcode, "basic/welcome");

                        msg.replySendMessage(smsg);
                    }

                    if (!ncm[i].is_bot) root.callHook.Call("NewChatMember", gc, msg, VFD, doubt, ncm[i]);
                }
            }

            if (msg.text != null) root.callHook.Call("OnTextMessage", gc, msg, VFD, doubt);

            root.callHook.Call("OnRawMessage", gc, msg, VFD, doubt);
        }
    }
}
