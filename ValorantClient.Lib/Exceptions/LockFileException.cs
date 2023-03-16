namespace ValorantClient.Lib.Exceptions
{
    /// <summary>
    /// Lock file exceptions
    /// </summary>
    [Serializable]
    public class LockFileException : Exception
    {
        public LockFileException() { }
        public LockFileException(string message) : base(message) { }
        public LockFileException(string message, Exception inner) : base(message, inner) { }
        protected LockFileException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
