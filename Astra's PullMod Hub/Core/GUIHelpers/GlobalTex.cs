using UnityEngine;

namespace Astras_PullMod_Hub.Core.GUIHelpers;

public static class GlobalTex
{
    public static Texture2D MakeTex(int W, int H, Color f)
    {
        Texture2D g = new(W, H);
        g.SetPixel(0, 0, f);
        g.Apply();
        return g;
    }
}
