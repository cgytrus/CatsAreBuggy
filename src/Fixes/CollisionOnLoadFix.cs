using System;

using CalApi.Patches;

using Cat;

using HarmonyLib;

using UnityEngine;

namespace CatsAreBuggy.Fixes {
    // ReSharper disable once UnusedType.Global
    internal class CollisionOnLoadFix : ConfigurablePatch {
        public CollisionOnLoadFix() : base(CatsAreBuggyPlugin.instance.Config, "Fixes", null, true,
            "Fix issues when (re)loading a room.") { }

        private static readonly Action<CatPartManager, bool> setPartFreeze =
            (Action<CatPartManager, bool>)Delegate.CreateDelegate(
            typeof(Action<CatPartManager, bool>), AccessTools.Method(typeof(CatPartManager), "SetPartFreeze"));

        // the game detects collisions with the cat while loading a map, which causes a bunch of issues
        // this fix freezes the cat while loading a map so that the game doesn't detect collisions with it
        public override void Apply() {
            On.PolyMap.MapManager.LoadPolyMap += (orig, mapName, state, fullPath) => {
                bool ret = orig(mapName, state, fullPath);
                if(!enabled) return ret;
                if(!ret) return false;
                setPartFreeze(GameObject.Find("Cat").GetComponentInParent<CatPartManager>(), true);
                PolyMap.MapManager.PolyMapLoadedAction += _ => {
                    if(!enabled) return;
                    setPartFreeze(GameObject.Find("Cat").GetComponentInParent<CatPartManager>(), false);
                };
                return true;
            };

            On.Cat.PlayerActor.Awake += (orig, self) => {
                orig(self);
                if(!enabled) return;
                setPartFreeze(GameObject.Find("Cat").GetComponentInParent<CatPartManager>(), false);
            };
        }
    }
}
