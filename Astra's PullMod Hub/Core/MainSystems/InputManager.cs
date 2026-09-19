using Astras_PullMod_Hub.Core.Libraries;
using UnityEngine;

namespace Astras_PullMod_Hub.Core.MainSystems;

public static class InputManager
{
    public enum InputType
    {
        Grip,
        Trigger,
        Joystick,
        A,
        B,
        X,
        Y
    }

    public static InputType Input = InputType.Grip;

    public static readonly string[] InputNames =
    {
        "Grip",
        "Trigger",
        "Joystick",
        "A",
        "B",
        "X",
        "Y"
    };

    public static int SelectedIndex
    {
        get => (int)Input;
        set => Input = (InputType)Mathf.Clamp(value, 0, InputNames.Length - 1);
    }

    public static bool LeftPressed => Input switch
    {
        InputType.Grip => InputLib.LeftGrab,
        InputType.Trigger => InputLib.LeftTrigger,
        InputType.Joystick => InputLib.LeftJoystickClick,
        InputType.X => InputLib.LeftX,
        InputType.Y => InputLib.LeftY,
        _ => false
    };

    public static bool RightPressed => Input switch
    {
        InputType.Grip => InputLib.RightGrab,
        InputType.Trigger => InputLib.RightTrigger,
        InputType.Joystick => InputLib.RightJoystickClick,
        InputType.A => InputLib.RightA,
        InputType.B => InputLib.RightB,
        _ => false
    };

    public static bool Pressed => PullSystem.Hand switch
    {
        PullSystem.HandMode.Left => LeftPressed,
        PullSystem.HandMode.Right => RightPressed,
        PullSystem.HandMode.Both => LeftPressed || RightPressed,
        _ => false
    };

    public static void Reset()
    {
        Input = InputType.Grip;
    }
}