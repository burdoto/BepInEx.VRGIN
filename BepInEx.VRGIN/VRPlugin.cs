using System;
using System.IO;
using System.Reflection;
using System.Xml.Serialization;
using BepInEx;
using BepInEx.VRGIN.Core;
using HarmonyLib;
using UnityEngine;
using VRGIN.Core;
using VRGIN.Helpers;
using Debug = UnityEngine.Debug;

namespace BepInEx.VRGIN
{
    [BepInPlugin(VRCore.ModGuid, "BepInEx VRGIN Loader", VRCore.Version)]
    [BepInDependency(VRCore.ModGuid_Core)] // context provider core module
    [BepInDependency(VRCore.ModGuid_Context)] // per-game-defined context module
    public class VRPlugin : BaseUnityPlugin
    {
        public static bool Active => !Environment.CommandLine.Contains("--novr") && (Environment.CommandLine.Contains("--vr") || SteamVRDetector.IsRunning);
        public static VRPlugin Instance;
        public IVRManagerContext Context;
        public VRManager Manager;

        public VRPlugin() => Instance = this;

        private void Awake()
        {
            if (Active)
            {
                Context = VRCore.LoadVrContext();
                Manager = VRCore.GameInterpreterFactory(Context);
            }
        }
    }
}