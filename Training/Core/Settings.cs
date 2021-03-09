using System;

namespace Training
{
    public static class Settings
    {
        public static string ProjectKey { get; private set; } 
        private static Random random = new Random();

        public static int RandomInt()
        {
            return random.Next();
        }

        public static void SetCurrentProjectKey(string projectKey)
        {
            ProjectKey = projectKey;
        }
    }
}
