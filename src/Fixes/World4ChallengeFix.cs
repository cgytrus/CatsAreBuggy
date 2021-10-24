using ChallengeSystem;

using Mono.Cecil.Cil;

using MonoMod.Cil;

namespace CatsAreBuggy.Fixes {
    // ReSharper disable once UnusedType.Global
    internal class World4ChallengeFix : BaseFix {
        public World4ChallengeFix() : base("Fix the world 4 challenge always getting marked as completed.") { }

        // remove the Save call in Cleanup as, unlike other challenges,
        // it's also used to mark the challenge as completed
        public override void Apply() => IL.ChallengeSystem.World4Challenge.Cleanup += il => {
            ILCursor cursor = new(il);
            cursor.GotoNext(code => code.MatchCall<World4Challenge>("Save"));
            cursor.Index--;
            cursor.Emit(OpCodes.Ret);
        };
    }
}
