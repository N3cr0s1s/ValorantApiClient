namespace ValorantClient.Lib.RiotFiles.Lock
{
    public interface ILockFileService
    {

        public Task<LockFile> LoadLockFileAsync(string path);

        public Task<LockFile> BuildLockFileAsync(string[] lockFileData);

    }
}
