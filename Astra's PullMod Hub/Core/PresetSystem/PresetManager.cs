using BepInEx;
using UnityEngine;
using Astras_PullMod_Hub.Core.MainSystems;

namespace Astras_PullMod_Hub.Core.PresetSystem;

public static class PresetManager
{
    public static string Folder => Path.Combine(Paths.ConfigPath, "Astras_PullMod_Hub", "Presets");

    public static void Init()
    {
        Directory.CreateDirectory(Folder);
    }

    public static void Save(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return;

        Init();

        PresetData data = new()
        {
            Name = name,
            Enabled = PullSystem.Enabled,

            Method = (int)PullMethods.Method,
            PullPower = PullMethods.PullPower,
            UpHillPower = PullMethods.UpHillPower,
            Momentum = PullMethods.Momentum,
            MaxPull = PullMethods.MaxPull,
            MaxVelocity = PullMethods.MaxVelocity,
            ExtraPower = PullMethods.ExtraPower,
            ClampVelocity = PullMethods.ClampVelocity,

            Input = (int)InputManager.Input,
            Hand = (int)PullSystem.Hand,
            Activation = (int)PullSystem.Activation,

            ReleaseWindow = PullSystem.ReleaseWindow,
            TouchLinger = PullSystem.TouchLinger
        };

        File.WriteAllText(Path.Combine(Folder, SafeName(name) + ".json"), JsonUtility.ToJson(data, true));
    }

    public static void Load(string name)
    {
        string path = Path.Combine(Folder, SafeName(name) + ".json");

        if (!File.Exists(path))
            return;

        PresetData data = JsonUtility.FromJson<PresetData>(File.ReadAllText(path));

        if (data == null)
            return;

        PullSystem.Enabled = data.Enabled;

        PullMethods.Method = (PullMethods.PullMethod)data.Method;
        PullMethods.PullPower = data.PullPower;
        PullMethods.UpHillPower = data.UpHillPower;
        PullMethods.Momentum = data.Momentum;
        PullMethods.MaxPull = data.MaxPull;
        PullMethods.MaxVelocity = data.MaxVelocity;
        PullMethods.ExtraPower = data.ExtraPower;
        PullMethods.ClampVelocity = data.ClampVelocity;

        InputManager.Input = (InputManager.InputType)data.Input;
        PullSystem.Hand = (PullSystem.HandMode)data.Hand;
        PullSystem.Activation = (PullSystem.ActivationMode)data.Activation;

        PullSystem.ReleaseWindow = data.ReleaseWindow;
        PullSystem.TouchLinger = data.TouchLinger;
    }

    public static void Delete(string name)
    {
        string path = Path.Combine(Folder, SafeName(name) + ".json");

        if (File.Exists(path))
            File.Delete(path);
    }

    public static List<string> GetPresets()
    {
        Init();

        List<string> presets = new();

        foreach (string file in Directory.GetFiles(Folder, "*.json"))
            presets.Add(Path.GetFileNameWithoutExtension(file));

        presets.Sort();

        return presets;
    }

    private static string SafeName(string name)
    {
        foreach (char c in Path.GetInvalidFileNameChars())
            name = name.Replace(c.ToString(), "");

        return name.Trim();
    }
}