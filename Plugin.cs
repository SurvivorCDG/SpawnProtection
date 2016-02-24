using System.Threading;

using UnityEngine;

using Rocket.API;
using Rocket.API.Collections;

using Rocket.Core.Plugins;
using Rocket.Core.Logging;

using Rocket.Unturned.Chat;
using Rocket.Unturned.Events;
using Rocket.Unturned.Player;

namespace SpawnProtection
{
    public class Plugin : RocketPlugin<Configuration>
    {
        public static Plugin Instance = null;

        public override TranslationList DefaultTranslations
        {
            get
            {
                return new TranslationList()
                {
                    { "spawn_message_of_start", "You are protected, protection time: {0}s" },
                    { "spawn_message_of_end", "Protection has expired" }
                };
            }
        }

        protected override void Load()
        {
            if (Configuration.Instance.Enabled)
            {
                Instance = this;
                UnturnedPlayerEvents.OnPlayerRevive += OnPlayerRevive;

                Logger.LogWarning("======[SpawnProtection]======");
                Logger.LogWarning("| Enabled: " + Configuration.Instance.Enabled);
                Logger.LogWarning("| Time in sec: " + Configuration.Instance.ProtectionTime);
                Logger.LogWarning("| Author of plugin: Survivor");
                Logger.LogWarning("======[SpawnProtection]======");
            }
        }

        protected override void Unload()
        {
            if (Configuration.Instance.Enabled)
            {
                UnturnedPlayerEvents.OnPlayerRevive -= OnPlayerRevive;
                base.Unload();
            }
        }

        public void OnPlayerRevive(UnturnedPlayer player, Vector3 position, byte revive)
        {
            if (player.HasPermission("spawnprotection")) SpawnProtection(player);
        }

        public void SpawnProtection(UnturnedPlayer player)
        {
            new Thread(() =>
            {
                if (Translate("spawn_message_of_start") != "spawn_message_of_start" || Translate("spawn_message_of_start") != null)
                    UnturnedChat.Say(player, Translate("spawn_message_of_start", Configuration.Instance.ProtectionTime), UnturnedChat.GetColorFromName(Configuration.Instance.ColorOfMessage, Color.green));

                player.GodMode = true;

                Thread.Sleep(Configuration.Instance.ProtectionTime * 1000);

                player.GodMode = false;

                if (Translate("spawn_message_of_end") != "spawn_message_of_end" || Translate("spawn_message_of_end") != null)
                    UnturnedChat.Say(player, Translate("spawn_message_of_end"), UnturnedChat.GetColorFromName(Configuration.Instance.ColorOfMessage, Color.green));
            })
            {
                IsBackground = true
            }.Start();
        }
    }
}