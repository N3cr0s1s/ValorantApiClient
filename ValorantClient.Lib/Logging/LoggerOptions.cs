﻿namespace ValorantClient.Lib.Logging
{
    public class LoggerOptions
    {

        public bool Debug { get; set; } = false;
        public bool Disable { get; set; } = false;
        public Type Logger { get; set; } = typeof(ConsoleLogger<>);

    }
}
