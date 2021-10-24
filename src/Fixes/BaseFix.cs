using BepInEx.Configuration;

using CalApi.Patches;

namespace CatsAreBuggy.Fixes {
    public abstract class BaseFix : IPatch {
        protected static bool enabled { get; private set; }
        protected BaseFix(string description) {
            ConfigEntry<bool> config =
                CatsAreBuggyPlugin.instance.Config.Bind("Fixes", GetType().FullName, true, description);
            enabled = config.Value;
            config.SettingChanged += (_, _) => { enabled = config.Value; };
        }

        public abstract void Apply();
    }
}
