namespace XenfbotDN {
    public static class Cleanup {
        private static readonly int delay = 30;
        private static int last;
        private static readonly string tag = "xenfbot@cleanuo";

        public static void runTask() {
            if (Helpers.getUnixTime() < last + delay) {
                return;
            }

            last = Helpers.getUnixTime();

            var data = SQL.Query("SELECT * FROM `cleanup`");
            if (data != null) {
                foreach (DataRow dr in data) {
                    var GCO = GroupConfiguration.getConfig((long) dr["group"]);

                    if ((int) dr["when"] + (int) dr["life"] < Helpers.getUnixTime()) {
                        Telegram.deleteMessage(new TGChat {id = (long) dr["group"]}, (long) dr["mid"]);
                        var qry = $"DELETE FROM `cleanup` WHERE `uid`={(long) dr["uid"]}";
                        var ra = 0;
                        SQL.NonQuery(qry, out ra);
                    }
                }
            }
        }


        public static void addMessage(TGMessage msg, int lifetime = 30) {
            if (msg == null) {
                Helpers.writeOut(tag, "Cleanup.addMessage was NULL!!");
                return;
            }

            var qry =
                $"INSERT INTO `cleanup`(`group`,`mid`,`when`,`life`) VALUES({msg.chat.id},{msg.message_id},{Helpers.getUnixTime()},{lifetime})";
            var ra = 0;
            SQL.NonQuery(qry, out ra);
        }
    }
}
