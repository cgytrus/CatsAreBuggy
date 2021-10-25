using System;
using System.Linq;

using BepInEx.Configuration;

using CalApi.Patches;

namespace CatsAreBuggy.Fixes {
    public abstract class BaseFix : IPatch {
        protected static bool enabled { get; private set; }

        protected BaseFix(string description) {
            string key = GetType().FullName;
            if(CatsAreBuggyPlugin.instance.Config.Select(conf => conf.Key.Key).Contains(key))
                throw new InvalidOperationException("Tried loading a fix which is already loaded.");

            ConfigEntry<bool> config =
                CatsAreBuggyPlugin.instance.Config.Bind("Fixes", key, true, description);
            enabled = config.Value;
            config.SettingChanged += (_, _) => { enabled = config.Value; };
        }

        public abstract void Apply();
    }
}
