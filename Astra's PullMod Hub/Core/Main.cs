using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using Astras_PullMod_Hub.Core.MainSystems;
using Astras_PullMod_Hub.Core.PresetSystem;
using static Astras_PullMod_Hub.Core.GUIHelpers.GlobalStyles;

namespace Astras_PullMod_Hub.Core;

public class Main : MonoBehaviour
{
    private Rect Window = new(140, 140, 460, 470);
    private bool Open;
    private int CurrentTab;

    private readonly string[] Tabs = { "Pull", "Methods", "Input", "Presets" };

    private bool Advanced;

    private List<string> Presets = new();
    private string PresetName = "Preset";
    private Vector2 PresetScroll;

    private void Start()
    {
        PresetManager.Init();
        Presets = PresetManager.GetPresets();
    }

    private void OnGUI()
    {
        EnsureLoad();

        if (!Open)
            return;

        Window.width = 460f;
        Window.height = 470f;

        Window = GUILayout.Window(583291, Window, UIM, "Astra's PullMod Hub", WindowStyle);
    }

    private void Update()
    {
        if (Keyboard.current != null && Keyboard.current.pKey.wasPressedThisFrame)
            Open = !Open;
    }

    private void FixedUpdate()
    {
        PullSystem.Update();
    }

    private void UIM(int i)
    {
        CurrentTab = GUILayout.Toolbar(CurrentTab, Tabs, TabStyle);
        GUILayout.Space(10f);
        switch (CurrentTab)
        {
            case 0: Pull(); break;
            case 1: Methods(); break;
            case 2: Input(); break;
            case 3: PresetMenu(); break;
        }
        GUILayout.FlexibleSpace();
        GUILayout.BeginHorizontal();
        GUILayout.Label($"Method: {PullMethods.Method}");

        if (GUILayout.Button("Close", ButtonStyle, GUILayout.Width(80)))
            Open = false;

        GUILayout.EndHorizontal();
        GUI.DragWindow();
    }

    private void Pull()
    {
        PullSystem.Enabled = GUILayout.Toggle(PullSystem.Enabled, "Pull Mod");
        GUILayout.Space(10f);
        GUILayout.Label($"Pull Power: {PullMethods.PullPower:F3}");
        PullMethods.PullPower = GUILayout.HorizontalSlider(PullMethods.PullPower, 0.001f, 1f, SliderStyle, SliderThumbStyle);
        GUILayout.Label($"Uphill Power: {PullMethods.UpHillPower:F3}");
        PullMethods.UpHillPower = GUILayout.HorizontalSlider(PullMethods.UpHillPower, 0.001f, 0.5f, SliderStyle, SliderThumbStyle);
        GUILayout.Label($"Max Pull: {PullMethods.MaxPull:F3}");
        PullMethods.MaxPull = GUILayout.HorizontalSlider(PullMethods.MaxPull, 0.01f, 1f, SliderStyle, SliderThumbStyle);
        GUILayout.Space(5f);
        PullMethods.ClampVelocity = GUILayout.Toggle(PullMethods.ClampVelocity, "Velocity Clamp");
        if (PullMethods.ClampVelocity)
        {
            GUILayout.Label($"Max Velocity: {PullMethods.MaxVelocity:F1}");
            PullMethods.MaxVelocity = GUILayout.HorizontalSlider(PullMethods.MaxVelocity, 5f, 100f, SliderStyle, SliderThumbStyle);
        }
        GUILayout.Space(5f);
        if (GUILayout.Button(Advanced ? "Advanced ▲" : "Advanced ▼", ButtonStyle))
            Advanced = !Advanced;

        if (Advanced)
        {
            GUILayout.Label($"Momentum: {PullMethods.Momentum:F4}");
            PullMethods.Momentum = GUILayout.HorizontalSlider(PullMethods.Momentum, 0f, 0.05f, SliderStyle, SliderThumbStyle);
            GUILayout.Label($"Extra Power: {PullMethods.ExtraPower:F2}");
            PullMethods.ExtraPower = GUILayout.HorizontalSlider(PullMethods.ExtraPower, 0.1f, 10f, SliderStyle, SliderThumbStyle);
        }

        GUILayout.Space(5f);
        if (GUILayout.Button("Reset Pull", ButtonStyle))
            PullMethods.Reset();
    }

    private void Methods()
    {
        GUILayout.Label("Pull Methods");
        int method = (int)PullMethods.Method;
        method = GUILayout.SelectionGrid(method, PullMethods.MethodNames, 3, TabStyle);
        PullMethods.Method = (PullMethods.PullMethod)method;
        GUILayout.Space(10f);
        GUILayout.Label($"Selected: {PullMethods.Method}");
        GUILayout.Label(MethodInfo());
        GUILayout.Space(10f);
        MethodSettings();
    }

    private void MethodSettings()
    {
        switch (PullMethods.Method)
        {
            case PullMethods.PullMethod.PMV1:
                GUILayout.Label("Classic V1 settings.");
                break;

            case PullMethods.PullMethod.PMV2:
                GUILayout.Label($"Momentum: {PullMethods.Momentum:F4}");
                PullMethods.Momentum = GUILayout.HorizontalSlider(PullMethods.Momentum, 0f, 0.05f, SliderStyle, SliderThumbStyle);
                break;

            case PullMethods.PullMethod.Gravity:
                GUILayout.Label($"Gravity Power: {PullMethods.ExtraPower:F2}");
                PullMethods.ExtraPower = GUILayout.HorizontalSlider(PullMethods.ExtraPower, 0f, 10f, SliderStyle, SliderThumbStyle);
                break;

            case PullMethods.PullMethod.Burst:
                GUILayout.Label($"Burst Power: {PullMethods.ExtraPower:F2}");
                PullMethods.ExtraPower = GUILayout.HorizontalSlider(PullMethods.ExtraPower, 0.1f, 10f, SliderStyle, SliderThumbStyle);
                break;

            default:
                GUILayout.Label($"Extra Power: {PullMethods.ExtraPower:F2}");
                PullMethods.ExtraPower = GUILayout.HorizontalSlider(PullMethods.ExtraPower, 0.1f, 10f, SliderStyle, SliderThumbStyle);
                break;
        }
    }

    private string MethodInfo()
    {
        return PullMethods.Method switch // This is the best I can do.
        {
            PullMethods.PullMethod.PMV1 => "Astra's PullMod V1",
            PullMethods.PullMethod.PMV2 => "Astra's PullMod V2",
            PullMethods.PullMethod.Regular => "Velocity & forward movement",
            PullMethods.PullMethod.Gravity => "Pull with upward assistance",
            PullMethods.PullMethod.Surface => "Follows the surface below you",
            PullMethods.PullMethod.Burst => "Adds a velocity burst",
            PullMethods.PullMethod.Inertia => "Adds momentum directly into velocity",
            PullMethods.PullMethod.Elastic => "Speed based elastic pull",
            PullMethods.PullMethod.Directional => "Stronger when moving forward",
            PullMethods.PullMethod.Curve => "Non-linear speed scaling",
            PullMethods.PullMethod.AirPull => "Stronger while airborne",
            PullMethods.PullMethod.GroundPull => "Stronger while touching",
            _ => ""
        };
    }

    private void Input()
    {
        GUILayout.Label("Input");
        int input = InputManager.SelectedIndex;
        input = GUILayout.SelectionGrid(input, InputManager.InputNames, 4, TabStyle);
        InputManager.SelectedIndex = input;
        GUILayout.Space(10f);
        GUILayout.Label("Hand");
        int hand = (int)PullSystem.Hand;
        hand = GUILayout.Toolbar(hand, new[] { "Both", "Left", "Right" }, TabStyle);
        PullSystem.Hand = (PullSystem.HandMode)hand;
        GUILayout.Space(10f);
        GUILayout.Label("Activation");
        int activation = (int)PullSystem.Activation;
        activation = GUILayout.Toolbar(activation, new[] { "Release", "Touch", "Hold" }, TabStyle);
        PullSystem.Activation = (PullSystem.ActivationMode)activation;
        GUILayout.Space(10f);
        GUILayout.Label($"Release Window: {PullSystem.ReleaseWindow:F2}");
        PullSystem.ReleaseWindow = GUILayout.HorizontalSlider(PullSystem.ReleaseWindow, 0.01f, 0.5f, SliderStyle, SliderThumbStyle);
        GUILayout.Label($"Touch Linger: {PullSystem.TouchLinger:F2}");
        PullSystem.TouchLinger = GUILayout.HorizontalSlider(PullSystem.TouchLinger, 0f, 0.5f, SliderStyle, SliderThumbStyle);
        GUILayout.Space(10f);
        if (GUILayout.Button("Reset Input", ButtonStyle))
        {
            InputManager.Reset();
            PullSystem.Reset();
        }
    }

    private void PresetMenu()
    {
        GUILayout.Label("Preset Name");
        PresetName = GUILayout.TextField(PresetName);
        GUILayout.Space(5f);
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Save", ButtonStyle))
        {
            PresetManager.Save(PresetName);
            Presets = PresetManager.GetPresets();
        }
        if (GUILayout.Button("Refresh", ButtonStyle))
            Presets = PresetManager.GetPresets();

        GUILayout.EndHorizontal();
        GUILayout.Space(10f);
        PresetScroll = GUILayout.BeginScrollView(PresetScroll);
        foreach (string preset in Presets)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(preset);

            if (GUILayout.Button("Load", ButtonStyle, GUILayout.Width(60)))
                PresetManager.Load(preset);

            if (GUILayout.Button("Delete", ButtonStyle, GUILayout.Width(60)))
            {
                PresetManager.Delete(preset);
                Presets = PresetManager.GetPresets();

                GUILayout.EndHorizontal();
                break;
            }

            GUILayout.EndHorizontal();
        }
        GUILayout.EndScrollView();
    }
} // The tuffest 