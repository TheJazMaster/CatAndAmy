using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using daisyowl.text;
using HarmonyLib;
using Microsoft.Extensions.Logging;
using Nanoray.PluginManager;
using Nickel;
using static TheJazMaster.CatAndAmy.Features.PairManager;

namespace TheJazMaster.CatAndAmy.Features;

[HarmonyPatch]
public static partial class PortraitSwitchingManager {
    
    private static IModData ModData => ModEntry.Instance.Helper.ModData;

    private static readonly string AnimationChangeGlyphKey = "AnimationChangeGlyph";
    private static readonly string ShoutPauseKey = "ShoutPause";


    public static void Initialize(IPluginPackage<IModManifest> package, IModHelper helper) {

    }

    private static Shout? lastShout = null;

    [HarmonyPostfix]
    [HarmonyPatch(typeof(Shout), nameof(Shout.GetText))]
    private static void Shout_GetText_Postfix(ref string __result, Shout __instance) {
        if (lastShout != __instance && __instance.who == ModEntry.Instance.CatAndAmyDeck.Deck.Key()) {
            // ModEntry.Instance.Logger.LogInformation("ORIGINAL: " + __result);
                // ModEntry.Instance.Logger.LogInformation("generate   " + lastShout?.GetHashCode() + " " + __instance.GetHashCode());
            // GenerateAnimationSwitchGlyphs(__instance, StripColorTagsRegex().Replace(__result, ""));
            if (!MyRegex().Match(__result).Success) return;
            __result = MyRegex2().Replace(MyRegex3().Replace(MyRegex4().Replace(__result, $"<c={ModEntry.CAT_COLOR}>"), ""), $"<c={ModEntry.AMY_COLOR}>");
            GenerateAnimationSwitchGlyphs(__instance, __result, MG.inst.g.state.route is Dialogue ? 158 : 80);
            __instance._textCache = __result;
        }
        lastShout = __instance;
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(Shout), nameof(Shout.Update))]
    private static void Shout_Update_Postfix(G g, Shout __instance) {
        bool found = false;
        if (__instance.who == ModEntry.Instance.CatAndAmyDeck.Deck.Key() && ModData.TryGetModData(__instance, AnimationChangeGlyphKey, out List<(int position, Girl who)>? glyphs) && !__instance.IsDonePrinting()) {
            for(int i = glyphs!.Count - 1; i >= 0; i--) {
                var (position, who) = glyphs[i];
                if (__instance.progress >= position) {
                    __instance.loopTag = MyRegex1().Replace(__instance.loopTag, "") + (who switch
                    {
                        Girl.CAT => "_cat",
                        Girl.AMY => "_amy",
                        Girl.BOTH => "",
                        _ => throw new NotImplementedException(),
                    });
                    // ModEntry.Instance.Logger.LogInformation("New tag: " + __instance.GetText());
                    found = true;
                }
                if (found)
                {
                    if (position != 0 && !DB.currentLocale.isHighRes) {
                        __instance.progress = position;
                        ModData.SetModData(__instance, ShoutPauseKey, 0.3);
                    }
                    glyphs.RemoveAt(i);
                    break;
                }
            }
            ModData.SetModData(__instance, AnimationChangeGlyphKey, glyphs);
        }
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(Shout), nameof(Shout.AnimationFrame))]
    private static bool Shout_Update_Prefix(ref double __result, Shout __instance) {
        if (ModData.TryGetModData(__instance, ShoutPauseKey, out double dur) && dur > 0) {
            __result = 0;
            return false;
        }
        return true;
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(Shout), nameof(Shout.Update))]
    private static bool Shout_Update_Prefix(G g, Shout __instance) {
        if (ModData.TryGetModData(__instance, ShoutPauseKey, out double dur)) {
            dur = Math.Max(0, dur - g.dt);
            if (dur != 0) {
                ModData.SetModData(__instance, ShoutPauseKey, dur);
                return false;
            }
            else {
                ModData.RemoveModData(__instance, ShoutPauseKey);
                return true;
            }
        }
        return true;
    }

    internal static uint AmyColorInt = new Color(ModEntry.AMY_COLOR).ToInt();
    internal static uint CatColorInt = new Color(ModEntry.CAT_COLOR).ToInt();

    // maxWidth: 80 if it's a blurb, 158 if it's a dialogue
    private static void GenerateAnimationSwitchGlyphs(Shout shout, string text, double maxWidth) {
        float letterSpacing = DB.currentLocale.isHighRes ? 0 : 1;
        double fontSize = DB.currentLocale.isHighRes ? (5 / 48) : 1;
        GlyphPlan gp = TextParser.LayoutGlyphs(text, fontSize, DB.pinch.metrics, Colors.textMain, maxWidth: maxWidth, letterSpacing: letterSpacing);

        List<(int position, Girl who)> glyphs = [];
        if (gp.glyphs.Count == 0) return;
        uint lastColor = Colors.textMain.ToInt();
        for (int i = 0; i < gp.glyphs.Count; i++) {
            Glyph g = gp.glyphs[i];
            if (g.color != 0 && lastColor != g.color) {
                if (g.color == AmyColorInt)
                    glyphs.Add((i, Girl.AMY));
                else if (g.color == CatColorInt)
                    glyphs.Add((i, Girl.CAT));
                else
                    glyphs.Add((i, Girl.BOTH));
                // glyphs.Add((i, g.color switch
                // {
                //     4294745833 => Girl.AMY,
                //     4282724539 => Girl.CAT,
                //     _ => Girl.BOTH
                // }));
                lastColor = g.color;
            }
        }
        ModData.SetModData(shout, AnimationChangeGlyphKey, glyphs);
    }

    [GeneratedRegex("<a=.>")]
    private static partial Regex MyRegex();
    [GeneratedRegex("_(amy|cat)")]
    private static partial Regex MyRegex1();
    [GeneratedRegex("(<c=.*>)|(\n)")]
    private static partial Regex StripColorTagsRegex();
    [GeneratedRegex("<a=a>")]
    private static partial Regex MyRegex2();
    [GeneratedRegex("<a=b>")]
    private static partial Regex MyRegex3();
    [GeneratedRegex("<a=c>")]
    private static partial Regex MyRegex4();
}