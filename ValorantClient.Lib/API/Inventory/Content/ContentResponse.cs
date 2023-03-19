namespace ValorantClient.Lib.API.Inventory.Content
{

    public class ContentResponse
    {
        public object[] DisabledIDs { get; set; }
        public Season[] Seasons { get; set; }
        public Event[] Events { get; set; }

        public class Season
        {
            public string ID { get; set; }
            public string Name { get; set; }
            public string Type { get; set; }
            public DateTime StartTime { get; set; }
            public DateTime EndTime { get; set; }
            public bool IsActive { get; set; }
        }

        public class Event
        {
            public string ID { get; set; }
            public string Name { get; set; }
            public DateTime StartTime { get; set; }
            public DateTime EndTime { get; set; }
            public bool IsActive { get; set; }
        }

    }

}
