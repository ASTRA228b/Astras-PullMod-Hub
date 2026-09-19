using UnityEngine;
using BepInEx;
using Astras_PullMod_Hub.Core;
using Astras_PullMod_Hub.Stuff;

namespace Astras_PullMod_Hub.Plugin;

[BepInPlugin(Constantss.GUID, Constantss.Name, Constantss.Version)]
public class Plugin : BaseUnityPlugin
{
    void Awake()
    {
        GameObject Plugin = new GameObject(Constantss.ObjectName);
        Plugin.AddComponent<Main>();
        DontDestroyOnLoad(Plugin);
    }
}
