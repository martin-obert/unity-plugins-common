using System;
using System.Collections.Generic;
using System.Linq;

namespace Obert.Common.Runtime.Startup.Arguments
{
    public static class CommandLineParser
    {
        private static string[] ArgumentPairs { get; }

        static CommandLineParser()
        {
            ArgumentPairs = System.Environment.GetCommandLineArgs().ToArray();
        }

        public static string GetRaw(string name) => GetKeyValue(name)?.value;

        private static (string name, string value)? GetKeyValue(string name)
        {
            (string name, string  value)? result = null;
            foreach (var argumentPair in ArgumentPairs)
            {
                if (!argumentPair.StartsWith(name))
                {
                    continue;
                }

                var parts = argumentPair.Split(new[] { '=' }, StringSplitOptions.RemoveEmptyEntries);

                result = parts.Length > 1 ? (parts[0], parts[1]) : (parts[0], default);
            }

            return result;
        }

        
        public static int GetInt(string name, int defaultValue = 0)
        {
            var raw = GetRaw(name);
            return string.IsNullOrWhiteSpace(raw) ? defaultValue : int.Parse(raw);
        }

        public static bool GetBool(string name, bool defaultValue = false)
        {
            var raw = GetKeyValue(name);

            if (raw == null) return defaultValue;

            var tuple = raw.Value;
            
            return string.IsNullOrWhiteSpace(tuple.value) || bool.Parse(tuple.value);
        }

        public static float GetFloat(string name, float defaultValue = 0f)
        {
            var raw = GetRaw(name);
            return string.IsNullOrWhiteSpace(raw) ? defaultValue : float.Parse(raw);
        }
    }
}