using BepInEx;

using CalApi.API;

namespace CatsAreBuggy;

[BepInPlugin("mod.cgytrus.plugins.calBugs", "Cats are Buggy", "0.1.3")]
[BepInDependency("mod.cgytrus.plugins.calapi", "0.2.2")]
public class CatsAreBuggyPlugin : BaseUnityPlugin {
    public static CatsAreBuggyPlugin? instance { get; private set; }

    public CatsAreBuggyPlugin() => instance = this;

    private void Awake() => Util.ApplyAllPatches();
}