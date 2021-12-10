using System.Collections;

using CalApi.Patches;

using HarmonyLib;

using UnityEngine.UI;

namespace CatsAreBuggy.Fixes {
    // ReSharper disable once UnusedType.Global
    internal class SpeedrunTimerFix : ConfigurablePatch {
        public SpeedrunTimerFix() : base(CatsAreBuggyPlugin.instance!.Config, "Fixes", null, true,
            "Fix the speedrun timer not displaying the correct time on the first room of the run.") { }

        public override void Apply() {
            // handle the coroutine manually...
            // and by "handle manually" i mean run it till the first "yield return ..."
            // and just throw it out of the window because we don't need that sussy manual OnLapStart call
            // instead we just disable comparisonToPreviousText...
            On.SpeedrunTimeIndicator.Start += (orig, self) => {
                IEnumerator ret = orig(self);
                if(!enabled) return ret;

                ret.MoveNext();
                ((Text)AccessTools.Field(typeof(SpeedrunTimeIndicator), "comparisonToPreviousText").GetValue(self))
                    .enabled = false;
                IEnumerator Break() { yield break; }
                return Break();
            };

            // ...and when the run starts, we also make the game think it was a lap,
            // forcing the text to update to the right value
            SpeedrunMode.speedrunStartAction += () => {
                if(enabled) SpeedrunMode.lapStartAction?.Invoke();
            };
        }
    }
}
