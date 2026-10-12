using System.Security.Cryptography;
using BaseLib.Extensions;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace VioletFlame.VioletFlameCode.Utils;

public class InvertUtils
{
    public class StrengthMultVar : DynamicVar {
        public const string Key = "StrMult";
        public bool keyword;

        public StrengthMultVar(decimal strMult, bool keyword = false, bool showTooltip = false) : base("StrMult", strMult) {
            this.keyword = keyword;
            if (keyword && showTooltip) this.WithTooltip();
        }
    }

    public class DexterityMultVar : DynamicVar
    {
        public const string Key = "DexMult";
        public bool keyword;

        public DexterityMultVar(decimal dexMult, bool keyword = false, bool showTooltip = false) : base("DexMult", dexMult)
        {
            this.keyword = keyword;
            if (keyword && showTooltip) this.WithTooltip();
        }
    }
}