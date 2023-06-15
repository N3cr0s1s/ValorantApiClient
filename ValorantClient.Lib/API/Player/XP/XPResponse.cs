namespace ValorantClient.Lib.API.Player.XP
{
    public class XPResponse
    {
        public int Version { get; set; }
        public string Subject { get; set; }
        public XpProgress Progress { get; set; }
        public XpHistory[] History { get; set; }
        public string LastTimeGrantedFirstWin { get; set; }
        public string NextTimeFirstWinAvailable { get; set; }

        public class XpProgress
        {
            public int Level { get; set; }
            public int XP { get; set; }
        }

        public class XpHistory
        {
            public string ID { get; set; }
            public DateTime MatchStart { get; set; }
            public Startprogress StartProgress { get; set; }
            public Endprogress EndProgress { get; set; }
            public int XPDelta { get; set; }
            public Xpsource[] XPSources { get; set; }
            public Xpmultiplier[] XPMultipliers { get; set; }
        }

        public class Startprogress
        {
            public int Level { get; set; }
            public int XP { get; set; }
        }

        public class Endprogress
        {
            public int Level { get; set; }
            public int XP { get; set; }
        }

        public class Xpsource
        {
            public string ID { get; set; }
            public int Amount { get; set; }
        }

        public class Xpmultiplier
        {
            public string ID { get; set; }
            public int Value { get; set; }
        }

    }
}
