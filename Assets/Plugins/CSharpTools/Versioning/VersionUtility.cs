using System.Linq;
using System.Text.RegularExpressions;

namespace CSharpTools.Versioning
{
    public class VersionUtility
    {
        private const string VersionPattern = @"(\d+)(?:\.(\d+))?(?:\.(\d+))?(?:\.(\d+))?";

        public enum VersionCompareResult { Older, Newer, Same }

        private static readonly Regex Regex;

        static VersionUtility()
        {
            Regex = new Regex(VersionPattern);
        }

        public static VersionCompareResult CompareVersions(string ownVersion, string otherVersion)
        {
            AssertVersionPattern(ownVersion, $"Failed to validate pattern of local version {ownVersion}");
            AssertVersionPattern(otherVersion, $"Failed to validate pattern of remote version {otherVersion}");

            long own = VersionToInteger(ownVersion);
            long other = VersionToInteger(otherVersion);

            if (own < other)
                return VersionCompareResult.Newer;
            
            return own > other ? VersionCompareResult.Older : VersionCompareResult.Same;
        }

        public static long VersionToInteger(string version)
        {
            AssertVersionPattern(version, $"Failed to validate version pattern for {version}");
            
            int[] own = DecodeVersion(version);
            string result = string.Empty;
            
            for (int i = 0; i < 3; i++)
                result = i >= own.Length ? $"{result}{0:D3}" : $"{result}{own[i]:D3}";

            return long.Parse(result);
        }

        private static int[] DecodeVersion(string version)
        {
            string[] parts = version.Split('.');

            if (parts.Any(p => !int.TryParse(p, out _)))
                throw new VersionPatternException($"The version identifier {version} contains non-numeric characters or cannot be interpreted as an integer value");

            if (parts.Length < 2)
                throw new VersionPatternException($"The version identifier {version} does not follow the expected pattern of at least 2 separated parts");

            return parts.Select(int.Parse).ToArray();
        }

        private static void AssertVersionPattern(string version, string errorMessage)
        {
            if (!Regex.IsMatch(version))
                throw new VersionPatternException(errorMessage);
        }
    }
}