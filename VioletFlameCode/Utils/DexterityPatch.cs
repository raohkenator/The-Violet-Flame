using System.Reflection;
using BaseLib.Extensions;
using HarmonyLib;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using VioletFlame.VioletFlameCode.Interface;

namespace VioletFlame.VioletFlameCode.Utils;

[HarmonyPatch(typeof(DexterityPower))]
public static class DexterityPatch {
    static MethodBase TargetMethod() => AccessTools.DeclaredMethod(typeof(DexterityPower), "ModifyBlockAdditive");
    
    [HarmonyPostfix]
    public static void ModifyBlock(CardModel? cardSource, ref decimal __result) {
        GetInverse(cardSource, out int dexMult, out InvertUtils.DexterityMultVar? variable);
        if (cardSource is IDexterityNull)
            dexMult = 0;
        __result *= dexMult;
    }

    public static bool GetInverse(CardModel? card, out int dexMult, out InvertUtils.DexterityMultVar? variable) {
        dexMult = 0;
        variable = null;
        bool b = false;
        if (card != null) {
            b = card.DynamicVars.TryGetValue(InvertUtils.DexterityMultVar.Key, out var vb);
            int bass = b && card.Owner.Creature.GetPowerAmount<DexterityPower>().LessThan(0) ? vb!.IntValue : 1;
            dexMult += bass;
            variable ??= b ? (InvertUtils.DexterityMultVar)vb! : null;
            return b;
        }
        dexMult = 1;
        return b;
    }
}