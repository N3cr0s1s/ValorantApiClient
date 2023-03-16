using ValorantClient.Lib.Exceptions;

namespace ValorantClient.Lib.RiotFiles.Lock
{
    /// <summary>
    /// <see cref="LockFile"/> service, implementation of <see cref="ILockFileService"/>
    /// </summary>
    internal class LockFileService : ILockFileService
    {

        /// <summary>
        /// Build <see cref="LockFile"/> from LockFile data
        /// </summary>
        /// <param name="lockFileData">the readed data</param>
        /// <returns><see cref="LockFile"/></returns>
        public Task<LockFile> BuildLockFileAsync(string[] lockFileData)
        {
            if (!uint.TryParse(lockFileData[1], out uint pid))
                throw new LockFileException($"PID: Cannot convert {lockFileData[1]} to uint!");

            if (!uint.TryParse(lockFileData[2], out uint port))
                throw new LockFileException($"PORT: Cannot convert {lockFileData[2]} to uint!");

            return Task.FromResult(new LockFile
            {
                Name = lockFileData[0],
                PID = pid,
                Port = port,
                Password = lockFileData[3],
                Protocol = lockFileData[4]
            });
        }

        /// <summary>
        /// Load Riot lock file async
        /// </summary>
        /// <param name="path">LockFile path</param>
        /// <returns><see cref="LockFile"/></returns>
        /// <exception cref="LockFileException">If lockfile not exist!</exception>
        /// <exception cref="LockFileException">If lockfile data is invalid</exception>
        public async Task<LockFile> LoadLockFileAsync(string path)
        {
            if (!File.Exists(path))
                //  Try this path: Env('LOCALAPPDATA')\Riot Games\Riot Client\Config\lockfile
                throw new LockFileException($"LockFile not exist with this path. Path: {path}");

            string content;
            using (StreamReader reader = new StreamReader(path))
            {
                content = await reader.ReadToEndAsync();
            }

            if (!content.Contains(":"))
                throw new LockFileException($"LockFile data is invalid. Data: {content}");

            string[] datas = content.Split(':');

            if (datas.Length < 5)
                throw new LockFileException($"LockFile datas length is {datas.Length}, expected 5! Content: {content}");

            return await BuildLockFileAsync(datas);
        }
    }
}
