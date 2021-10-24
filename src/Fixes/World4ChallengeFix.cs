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
            cursor.GotoNext(code => code.MatchRet()); // end of the method; after Save()
            ILLabel jmpLabel = cursor.DefineLabel();
            cursor.GotoPrev(MoveType.Before, code => code.MatchLdarg(0)); // before Save()
            cursor.Emit<World4ChallengeFix>(OpCodes.Call, $"get_{nameof(enabled)}");
            cursor.Emit(OpCodes.Brtrue_S, jmpLabel);
        };
    }
}
