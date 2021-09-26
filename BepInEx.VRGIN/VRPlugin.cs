using System;
using BepInEx;
using HarmonyLib;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace BepInEx.VRGIN
{
    [BepInPlugin("BepInEx.VRGIN", "BepInEx VRGIN Loader", Version)]
    [BepInDependency("BepInEx.VRGIN.Context")] // per-game-defined context module
    public class VRPlugin : BaseUnityPlugin
    {
        public const string Version = "0.0.1";

        private void Awake()
        {
            throw new NotImplementedException();
        }
    }
}