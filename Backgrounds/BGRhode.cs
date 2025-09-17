using System;
using System.Runtime;
using FSPRO;
using HarmonyLib;
using Microsoft.Xna.Framework.Graphics;

namespace TheJazMaster.CatAndAmy.Backgrounds;

[HarmonyPatch]
public class BGRhode : BG
{
    public bool noMusic = false;
    public bool ambience = false;
    public bool glitching = false;
    public double glitchingStrength = 0;
    public bool glowing = false;
    public double glowTimer = 0;
    public double shakeTimer = 0;
    public bool rumble;
	public double rumbleTimer;
    public bool killSound;

    public override void Render(G g, double t, Vec offset)
    {
        glowTimer -= g.dt;
        if (glowTimer < 0) glowTimer = 0;
        shakeTimer -= g.dt;
        if (shakeTimer < 0) shakeTimer = 0;
        if (!glitching) glitchingStrength -= g.dt;
        if (glitchingStrength < 0) glitchingStrength = 0;

        if (glitchingStrength > 0) {
            SpriteUtil.GlitchSpriteBounded(ModEntry.Instance.RhodeBG, 0, 0, new Vec(480, 270), new Color(0, 1, 0), (int)(g.time * 5), 1, 0.15*glitchingStrength, BlendMode.Screen, (int)(glitchingStrength*20));
            SpriteUtil.GlitchSpriteBounded(ModEntry.Instance.RhodeBG, 0, 0, new Vec(480, 270), new Color(1, 0, 1), (int)(g.time * 6), 1, 0.15*glitchingStrength, BlendMode.Screen, (int)(glitchingStrength*20));
        }
        else
            Draw.Sprite(ModEntry.Instance.RhodeBG, 0, 0);

        if (glowing) {
            Glow.Draw(new Vec(100, 166), (Math.Sqrt(1 - glowTimer/3) + Math.Sin(g.time*2)/8) * 160, new Color(0.1, 0.4, 1.0));
        }

        Draw.Sprite(ModEntry.Instance.RhodeFG, 0, 0);

        g.state.shake = shakeTimer * 8;

        if (rumble) {
			rumbleTimer += g.dt;
		}
        if (rumble) {
			if (!killSound) g.state.shake = rumbleTimer / 2;
			Draw.Fill(new Color(0.25, 0.5, 1.0).gain(rumbleTimer / 3.0), BlendMode.Screen);
			Draw.Fill(new Color(1, 1, 1).fadeAlpha(rumbleTimer / 4.0));
            if (!killSound) Audio.Auto(Event.Scenes_CobaltCritical);
            else Audio.Auto(Event.Scenes_CoreAmbience);
        }
        else if (ambience)
            Audio.Auto(Event.Scenes_CoreAmbience);
    }

    public MusicState GetMusicState() {
        return new MusicState {
            scene = noMusic ? Song.Silence : Song.Birthday
        };
    }

    public override void OnAction(State s, string action)
    {
        if (action == "shake") {
            shakeTimer = 0.25;
            Audio.Play(Event.Hits_HitHurt);
            ambience = true;
        }
        else if (action == "shakeBig") {
            shakeTimer = 0.4;
            Audio.Play(Event.Hits_HitHurt);
        }
        else if (action == "glitch") {
            glitching = true;
            glitchingStrength = 3;
        }
        else if (action == "unglitch") {
            glitching = false;
        }
        else if (action == "glow") {
            glowing = true;
            glowTimer = 0;
        }
        else if (action == "enableGlow") {
            glowing = true;
            glowTimer = 3;
        }
        else if (action == "disableGlow") {
            glowing = false;
            glowTimer = 3;
        }
        else if (action == "rumble") {
            rumble = true;
        }
        else if (action == "killSound") {
            killSound = true;
        }
        else if (action == "noMusic") {
            noMusic = true;
        }
    }



    [HarmonyPrefix]
    [HarmonyPatch(typeof(Dialogue), nameof(Dialogue.GetMusic))]
    private static bool Dialogue_GetMusic_Prefix(G g, Dialogue __instance, ref MusicState? __result) {
        if (__instance.bg is BGRhode bGRhode) {
            __result = bGRhode.GetMusicState();
            return false;
        }
        return true;
    }
}

