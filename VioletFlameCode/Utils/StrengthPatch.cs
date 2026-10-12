using System.Reflection;
using BaseLib.Extensions;
using HarmonyLib;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;

namespace VioletFlame.VioletFlameCode.Utils;

[HarmonyPatch(typeof(StrengthPower))]
public static class StrengthPatch {
    static MethodBase TargetMethod() => AccessTools.DeclaredMethod(typeof(StrengthPower), "ModifyDamageAdditive");
    
    [HarmonyPostfix]
    public static void ModifyDamage(CardModel? cardSource, ref decimal __result) {
        int strMult;
        GetInverse(cardSource, out strMult, out InvertUtils.StrengthMultVar variable);
        __result *= strMult;
    }

    public static bool GetInverse(CardModel? card, out int strMult, out InvertUtils.StrengthMultVar variable) {
        strMult = 0;
        variable = null;
        bool b = false;
        if (card != null) {
            b = card.DynamicVars.TryGetValue(InvertUtils.StrengthMultVar.Key, out var vb);
            int bass = b && card.Owner.Creature.GetPowerAmount<StrengthPower>().LessThan(0) ? vb.IntValue : 1;
            strMult += bass;
            variable ??= b ? (InvertUtils.StrengthMultVar) vb : null;
        }
        return b;
    }
}