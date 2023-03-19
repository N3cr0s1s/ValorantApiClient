namespace ValorantClient.Lib.API.PreGame.SelectAgent
{

    public class SelectAgentResponse
    {
        public string ID { get; set; }
        public long Version { get; set; }
        public Team[] Teams { get; set; }
        public Allyteam AllyTeam { get; set; }
        public object EnemyTeam { get; set; }
        public object[] ObserverSubjects { get; set; }
        public object[] MatchCoaches { get; set; }
        public int EnemyTeamSize { get; set; }
        public int EnemyTeamLockCount { get; set; }
        public string PregameState { get; set; }
        public DateTime LastUpdated { get; set; }
        public string MapID { get; set; }
        public object[] MapSelectPool { get; set; }
        public object[] BannedMapIDs { get; set; }
        public Castedvotes CastedVotes { get; set; }
        public object[] MapSelectSteps { get; set; }
        public int MapSelectStep { get; set; }
        public string Team1 { get; set; }
        public string GamePodID { get; set; }
        public string Mode { get; set; }
        public string VoiceSessionID { get; set; }
        public string MUCName { get; set; }
        public string QueueID { get; set; }
        public string ProvisioningFlowID { get; set; }
        public bool IsRanked { get; set; }
        public long PhaseTimeRemainingNS { get; set; }
        public int StepTimeRemainingNS { get; set; }
        public bool altModesFlagADA { get; set; }
        public object TournamentMetadata { get; set; }
        public object RosterMetadata { get; set; }


        public class Allyteam
        {
            public string TeamID { get; set; }
            public Player[] Players { get; set; }
        }

        public class Player
        {
            public string Subject { get; set; }
            public string CharacterID { get; set; }
            public string CharacterSelectionState { get; set; }
            public string PregamePlayerState { get; set; }
            public int CompetitiveTier { get; set; }
            public Playeridentity PlayerIdentity { get; set; }
            public Seasonalbadgeinfo SeasonalBadgeInfo { get; set; }
            public bool IsCaptain { get; set; }
        }

        public class Playeridentity
        {
            public string Subject { get; set; }
            public string PlayerCardID { get; set; }
            public string PlayerTitleID { get; set; }
            public int AccountLevel { get; set; }
            public string PreferredLevelBorderID { get; set; }
            public bool Incognito { get; set; }
            public bool HideAccountLevel { get; set; }
        }

        public class Seasonalbadgeinfo
        {
            public string SeasonID { get; set; }
            public int NumberOfWins { get; set; }
            public object WinsByTier { get; set; }
            public int Rank { get; set; }
            public int LeaderboardRank { get; set; }
        }

        public class Castedvotes
        {
        }

        public class Team
        {
            public string TeamID { get; set; }
            public Player[] Players { get; set; }
        }

    }

}
