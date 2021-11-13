using System;
using System.Collections.Generic;

using CalApi.API;
using CalApi.Patches;

using ChallengeSystem;

using HarmonyLib;

namespace CatsAreBuggy.Fixes;

// ReSharper disable once UnusedType.Global
internal class ModdedChallengesFix : ConfigurablePatch {
    public ModdedChallengesFix() : base(CatsAreBuggyPlugin.instance!.Config, "Fixes", null, true,
        "Fix challenges not working with mods.") { }

    // some modded types can't be loaded, so the code throws an exception;
    // this fix is basically just lqd's original code except with a try-catch inside the first loop
    public override void Apply() => On.ChallengeSystem.ChallengeManager.GetAllChallenges += orig => {
        if(!enabled) {
            orig();
            return;
        }

        Util.ForEachImplementation(typeof(IChallenge),
            type =>
                ((List<IChallenge?>)AccessTools.Field(typeof(ChallengeManager), "challenges").GetValue(null))
                .Add(Activator.CreateInstance(type) as IChallenge));
    };
}