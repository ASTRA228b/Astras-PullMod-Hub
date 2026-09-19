using UnityEngine;

namespace Astras_PullMod_Hub.Core.GUIHelpers;

public static class GlobalStyles
{
    public static GUIStyle? WindowStyle, ButtonStyle, TabStyle, SelectedTabStyle, SliderStyle, SliderThumbStyle;

    private static Texture2D? WindowTex, ButtonTex, SliderTex, SliderThumbTex, SelectedTex;

    private static Color WindowColor = new(0.1f, 0.1f, 0.1f, 1f);
    private static Color ButtonColor = new(0.2f, 0.2f, 0.2f, 1f);
    private static Color SliderTrackColor = new(0.15f, 0.15f, 0.15f, 1f);
    private static Color SliderThumbColor = new(0f, 0.6f, 1f, 1f);
    private static Color SelectedColor = new(0.38f, 0.10f, 0.60f, 1f);

    private static bool Loaded;

    public static void Init()
    {
        WindowTex = GlobalTex.MakeTex(1, 1, WindowColor);
        ButtonTex = GlobalTex.MakeTex(1, 1, ButtonColor);
        SliderTex = GlobalTex.MakeTex(1, 1, SliderTrackColor);
        SliderThumbTex = GlobalTex.MakeTex(1, 1, SliderThumbColor);
        SelectedTex = GlobalTex.MakeTex(1, 1, SelectedColor);

        WindowStyle = new GUIStyle(GUI.skin.window);
        ButtonStyle = new GUIStyle(GUI.skin.button);
        TabStyle = new GUIStyle(GUI.skin.button);
        SelectedTabStyle = new GUIStyle(GUI.skin.button);
        SliderStyle = new GUIStyle(GUI.skin.horizontalSlider);
        SliderThumbStyle = new GUIStyle(GUI.skin.horizontalSliderThumb);

        WindowStyle.normal.textColor = Color.white;

        ButtonStyle.normal.textColor = Color.white;
        ButtonStyle.hover.textColor = Color.blue;
        ButtonStyle.active.textColor = Color.red;

        TabStyle.normal.textColor = Color.white;
        TabStyle.hover.textColor = Color.blue;
        TabStyle.active.textColor = Color.red;
        TabStyle.focused.textColor = Color.white;
        TabStyle.onNormal.textColor = Color.blue;
        TabStyle.onHover.textColor = Color.blue;
        TabStyle.onActive.textColor = Color.blue;
        TabStyle.onFocused.textColor = Color.blue;

        SelectedTabStyle.normal.textColor = Color.white;
        SelectedTabStyle.hover.textColor = Color.white;
        SelectedTabStyle.active.textColor = Color.white;

        ApplyBackground(WindowStyle, WindowTex);
        ApplyBackground(ButtonStyle, ButtonTex);
        ApplyBackground(TabStyle, ButtonTex);
        ApplyBackground(SelectedTabStyle, SelectedTex);
        ApplyBackground(SliderStyle, SliderTex);
        ApplyBackground(SliderThumbStyle, SliderThumbTex);

        Loaded = true;
    }

    public static void EnsureLoad()
    {
        if (!Loaded)
            Init();
    }

    public static void ApplyBackground(GUIStyle style, Texture2D tex)
    {
        style.normal.background = tex;
        style.hover.background = tex;
        style.active.background = tex;
        style.focused.background = tex;
        style.onNormal.background = tex;
        style.onHover.background = tex;
        style.onActive.background = tex;
        style.onFocused.background = tex;
    }
}