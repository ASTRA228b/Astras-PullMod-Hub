using System;

namespace Astras_PullMod_Hub.Core.PresetSystem;

[Serializable]
public class PresetData
{
    public string Name = "";

    public bool Enabled;

    public int Method;
    public float PullPower;
    public float UpHillPower;
    public float Momentum;
    public float MaxPull;
    public float MaxVelocity;
    public float ExtraPower;
    public bool ClampVelocity;

    public int Input;
    public int Hand;
    public int Activation;

    public float ReleaseWindow;
    public float TouchLinger;
}