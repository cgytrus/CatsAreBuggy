using System;

using CalApi.Patches;

using HarmonyLib;

using MonoMod.Cil;
using MonoMod.RuntimeDetour;

namespace CatsAreBuggy.Fixes;

// ReSharper disable once UnusedType.Global
internal class RestartPauseFix : ConfigurablePatch {
    public RestartPauseFix() : base(CatsAreBuggyPlugin.instance!.Config, "Fixes", null, true,
        "Fix the pause menu not going away when restarting.") { }

    private static readonly Action<PauseScreen> setUICooldown =
        (Action<PauseScreen>)Delegate.CreateDelegate(
            typeof(Action<PauseScreen>), AccessTools.Method(typeof(PauseScreen), "SetUICooldown"));

    public override void Apply() {
        // remove the ui cooldown stuff from TogglePause...
        IDetour loadGameDetour =
            new ILHook(AccessTools.Method(AccessTools.TypeByName("<TogglePause>d__36"), "MoveNext"), il => {
                ILCursor cursor = new(il);
                cursor.GotoNext(code => code.MatchCall<PauseScreen>("SetUICooldown"));
                cursor.Index -= 7;
                cursor.RemoveRange(8);
            });
        loadGameDetour.Apply();

        // ...and move it to TogglePauseButton
        On.PauseScreen.TogglePauseButton += (orig, self) => {
            float uiCooldown = (float)AccessTools.Field(typeof(PauseScreen), "uiCooldown").GetValue(self);
            if(uiCooldown > 0f) return;
            setUICooldown(self);
            orig(self);
        };
    }
}
