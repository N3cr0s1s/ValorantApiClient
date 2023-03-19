namespace ValorantClient.Lib.RiotFiles.Lock
{
    public interface ILockFileService
    {
        /// <summary>
        /// Load <see cref="LockFile"/>
        /// </summary>
        /// <param name="path">If path is null,then return with the cached <see cref="LockFile"/></param>
        /// <returns></returns>
        public Task<LockFile> LoadLockFileAsync(string? path);

        public Task<LockFile> BuildLockFileAsync(string[] lockFileData);

    }
}
