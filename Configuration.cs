using Rocket.API;

namespace SpawnProtection
{
    public class Configuration : IRocketPluginConfiguration
    {
        public bool Enabled;
        public int ProtectionTime;
        public string ColorOfMessage;

        public void LoadDefaults()
        {
            Enabled = true;
            ProtectionTime = 10;
            ColorOfMessage = "Blue";
        }
    }
}