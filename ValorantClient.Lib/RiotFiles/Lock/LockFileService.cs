using System.Text;
using ValorantClient.Lib.Exceptions;
using ValorantClient.Lib.Helper;
using ValorantClient.Lib.Logging;

namespace ValorantClient.Lib.RiotFiles.Lock
{
    /// <summary>
    /// <see cref="LockFile"/> service, implementation of <see cref="ILockFileService"/>
    /// </summary>
    public class LockFileService : ILockFileService
    {
        private readonly ILogger<LockFileService> _logger;
        private LockFile? _lockFile;

        public LockFileService(ILogger<LockFileService> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Build <see cref="LockFile"/> from LockFile data
        /// </summary>
        /// <param name="lockFileData">the readed data</param>
        /// <returns><see cref="LockFile"/></returns>
        public Task<LockFile> BuildLockFileAsync(string[] lockFileData)
        {
            _logger.LogDebug("Building LockFile from file content");
            _logger.LogDebug(string.Join(':',lockFileData));

            if (!uint.TryParse(lockFileData[1], out uint pid))
                throw new LockFileException($"PID: Cannot convert {lockFileData[1]} to uint!");

            if (!uint.TryParse(lockFileData[2], out uint port))
                throw new LockFileException($"PORT: Cannot convert {lockFileData[2]} to uint!");

            LockFile file = new LockFile
            {
                Name = lockFileData[0],
                PID = pid,
                Port = port,
                Password = lockFileData[3],
                Protocol = lockFileData[4]
            };

            _lockFile = file;
            _logger.LogDebug("LockFile cached.");

            return Task.FromResult(file);
        }

        /// <summary>
        /// Load Riot lock file async
        /// </summary>
        /// <param name="path">LockFile path</param>
        /// <returns><see cref="LockFile"/></returns>
        /// <exception cref="LockFileException">If lockfile not exist!</exception>
        /// <exception cref="LockFileException">If lockfile data is invalid</exception>
        public async Task<LockFile> LoadLockFileAsync(string? rawPath)
        {
            _logger.LogDebug("Loading lock file");

            if (_lockFile != null)
            {
                _logger.LogDebug("Loading cached lock file");
                return _lockFile;
            }

            string path = await rawPath.ProcessEnvAsync();
            _logger.LogDebug("New path: " + path);

            if (!File.Exists(path))
            {
                _logger.LogError("Lock file not exist with this path: " + path);
                //  Try this path: Env('LOCALAPPDATA')\Riot Games\Riot Client\Config\lockfile
                throw new LockFileException($"LockFile not exist with this path. Path: {path}");
            }

            _logger.LogInformation("Start lock file read stream");
            string content;
            using (FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (StreamReader reader = new StreamReader(fileStream, Encoding.Default))
            {
                _logger.LogInformation("Lock file loading stream ReadToEndAsync");
                content = await reader.ReadToEndAsync();
            }

            _logger.LogInformation("LockFile content: " + content);

            if (!content.Contains(":"))
            {
                _logger.LogError($"LockFile data is invalid. VersionData: {content}");
                throw new LockFileException($"LockFile data is invalid. VersionData: {content}");
            }

            string[] datas = content.Split(':');

            if (datas.Length < 5)
            {
                _logger.LogError($"LockFile datas length is {datas.Length}, expected 5! Content: {content}");
                throw new LockFileException($"LockFile datas length is {datas.Length}, expected 5! Content: {content}");
            }

            return await BuildLockFileAsync(datas);
        }
    }
}
