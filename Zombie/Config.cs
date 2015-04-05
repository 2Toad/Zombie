using System;
using System.Collections.Concurrent;
using System.Configuration;

namespace Zombie
{
    public static class Config
    {
        private static readonly ConcurrentDictionary<string, object> Cache = new ConcurrentDictionary<string, object>();
        
        public static int PasswordIterations { get { return GetSetting<int>("Security.PasswordIterations"); } }
        public static string PasswordPepper { get { return GetSetting<string>("Security.PasswordPepper"); } }
        public static string BearerTokenKey { get { return GetSetting<string>("Security.BearerToken.Key"); } }
        public static string AccessTokenHeader { get { return GetSetting<string>("Security.AccessToken.Header"); } }
        public static int AccessTokenCacheDuration { get { return GetSetting<int>("Security.AccessToken.CacheDuration"); } }

        public static string SupportEmailAddress { get { return GetSetting<string>("Email.Address.Support"); } }

        public static bool GenerateTestData { get { return GetSetting<bool>("TestData.Generate"); } }
        public static string TestDataEmailMask { get { return GetSetting<string>("TestData.EmailMask"); } }

        /// <summary>
        /// Formats a test email addess.
        /// </summary>
        /// <param name="name">The test account name.</param>
        /// <returns>A test email address.</returns>
        public static string TestEmail(string name)
        {
            return String.Format(TestDataEmailMask, name);
        }

        /// <summary>
        /// Reads a setting from the configuration file.
        /// </summary>
        /// <typeparam name="T">The value type.</typeparam>
        /// <param name="name">The name of the setting.</param>
        /// <returns>The value associated with the setting.</returns>
        private static T GetSetting<T>(string name)
        {
            object result;
            if (Cache.TryGetValue(name, out result)) return (T)result;

            result = (T)Convert.ChangeType(ConfigurationManager.AppSettings[name], typeof(T));
            return (T)(Cache[name] = result);
        }
    }
}
