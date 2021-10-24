namespace CatsAreBuggy.Fixes {
    // ReSharper disable once UnusedType.Global
    internal class DamageOnLoadFix : BaseFix {
        public DamageOnLoadFix() : base("Fix the cat sometimes getting damaged when entering a door.") { }

        // the cat can get damaged while loading a level
        // if the next room has spikes at the same position as the door in the current room;
        // this fix makes the cat ignore damage until the room is loaded
        private static bool _loading;
        public override void Apply() {
            On.PolyMap.MapManager.LoadPolyMap += (orig, mapName, state, fullPath) => {
                bool ret = orig(mapName, state, fullPath);
                if(!ret) return false;
                _loading = true;
                PolyMap.MapManager.PolyMapLoadedAction += _ => { _loading = false; };
                return true;
            };

            On.Cat.CatHealth.TakeDamage += (orig, self) => { if(!enabled || !_loading) orig(self); };
            On.Cat.CatHealth.Die += (orig, self) => { if(!enabled || !_loading) orig(self); };

            On.Cat.PlayerActor.Awake += (orig, self) => {
                orig(self);
                _loading = false;
            };
        }
    }
}
