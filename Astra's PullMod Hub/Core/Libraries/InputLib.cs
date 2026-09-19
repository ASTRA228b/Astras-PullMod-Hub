using UnityEngine;
using Valve.VR;

namespace Astras_PullMod_Hub.Core.Libraries;

internal static class InputLib
{
    public static bool RightGrab => ControllerInputPoller.instance != null && ControllerInputPoller.instance.rightControllerGripFloat > 0.5f;
    public static bool LeftGrab => ControllerInputPoller.instance != null && ControllerInputPoller.instance.leftControllerGripFloat > 0.5f;

    public static bool RightTrigger => ControllerInputPoller.instance != null && ControllerInputPoller.instance.rightControllerTriggerButton;
    public static bool LeftTrigger => ControllerInputPoller.instance != null && ControllerInputPoller.instance.leftControllerTriggerButton;

    public static bool RightA => ControllerInputPoller.instance != null && ControllerInputPoller.instance.rightControllerPrimaryButton;
    public static bool RightB => ControllerInputPoller.instance != null && ControllerInputPoller.instance.rightControllerSecondaryButton;

    public static bool LeftX => ControllerInputPoller.instance != null && ControllerInputPoller.instance.leftControllerPrimaryButton;
    public static bool LeftY => ControllerInputPoller.instance != null && ControllerInputPoller.instance.leftControllerSecondaryButton;

    // Vive wands may report the same physical button as Primary + Secondary.
    public static bool RightJoystickClick => SteamVR_Actions.gorillaTag_RightJoystickClick?.state ?? false;
    public static bool LeftJoystickClick => SteamVR_Actions.gorillaTag_LeftJoystickClick?.state ?? false;

    public static Vector2 RightJoystickAxis
    {
        get
        {
            if (SteamVR_Actions.gorillaTag_RightJoystick2DAxis == null)
                return Vector2.zero;

            var axis = SteamVR_Actions.gorillaTag_RightJoystick2DAxis.axis;
            return new Vector2(axis.x, axis.y);
        }
    }

    public static Vector2 LeftJoystickAxis
    {
        get
        {
            if (SteamVR_Actions.gorillaTag_LeftJoystick2DAxis == null)
                return Vector2.zero;

            var axis = SteamVR_Actions.gorillaTag_LeftJoystick2DAxis.axis;
            return new Vector2(axis.x, axis.y);
        }
    }
}