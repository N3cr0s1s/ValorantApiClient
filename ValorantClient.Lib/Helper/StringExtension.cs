using System.Text.RegularExpressions;

namespace ValorantClient.Lib.Helper
{
    internal static class StringExtension
    {

        public static Task<string> ProcessEnvAsync(this string str)
        {
            string envPattern = @"env\(([^)]+)\)";
            string output = str;
            MatchCollection matches = Regex.Matches(output, envPattern);
            foreach (Match match in matches)
            {
                string envName = match.Groups[1].Value;
                string? envValue = Environment.GetEnvironmentVariable(envName);

                if (envValue is null)
                    throw new NullReferenceException($"There is no environment variable with this name {envName}");

                output = output.Replace(match.Value, envValue);
            }
            return Task.FromResult(output);
        }

        public static string ProcessEnv(this string str)
        {
            string envPattern = @"env\(([^)]+)\)";
            string output = str;
            MatchCollection matches = Regex.Matches(output, envPattern);
            foreach (Match match in matches)
            {
                string envName = match.Groups[1].Value;
                string? envValue = Environment.GetEnvironmentVariable(envName);

                if (envValue is null)
                    throw new NullReferenceException($"There is no environment variable with this name {envName}");

                output = output.Replace(match.Value, envValue);
            }
            return output;
        }
        public static string Base64Encode(this string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(plainTextBytes);
        }
    }
}
