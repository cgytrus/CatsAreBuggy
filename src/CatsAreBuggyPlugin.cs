using BepInEx;

using CalApi.API;

namespace CatsAreBuggy {
    [BepInPlugin("mod.cgytrus.plugins.calBugs", "Cats are Buggy", "0.1.0")]
    [BepInDependency("mod.cgytrus.plugins.calapi", "0.2.0")]
    public class CatsAreBuggyPlugin : BaseUnityPlugin {
        public static CatsAreBuggyPlugin instance { get; private set; }
        private void Awake() {
            instance = this;
            Util.ApplyAllPatches();
        }
    }
}
