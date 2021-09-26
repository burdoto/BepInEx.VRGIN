using System;
using System.IO;
using System.Reflection;
using System.Xml.Serialization;
using BepInEx;
using BepInEx.VRGIN.Core;
using HarmonyLib;
using UnityEngine;
using VRGIN.Core;
using Debug = UnityEngine.Debug;

namespace BepInEx.VRGIN
{
    [BepInPlugin(VRCore.ModGuid, "BepInEx VRGIN Loader", VRCore.Version)]
    [BepInDependency(VRCore.ModGuid_Core)] // context provider core module
    [BepInDependency(VRCore.ModGuid_Context)] // per-game-defined context module
    public class VRPlugin : BaseUnityPlugin
    {
        private void Awake()
        {
        }
        
        private IVRManagerContext CreateContext(string path) {
            var serializer = new XmlSerializer(VRCore.VrContextType);

            if(File.Exists(path))
            {
                // Attempt to load XML
                using (var file = File.OpenRead(path))
                {
                    try
                    {
                        return serializer.Deserialize(file) as IVRManagerContext;
                    }
                    catch (Exception e)
                    {
                        VRLog.Error("Failed to deserialize {0} -- using default", path);
                    }
                }
            }

            // Create and save file
            var context = VRCore.VrContextType.GetConstructor(Type.EmptyTypes)
                ?.Invoke(Array.Empty<object>()) as IVRManagerContext;
            try
            {
                using (var file = new StreamWriter(path))
                {
                    file.BaseStream.SetLength(0);
                    serializer.Serialize(file, context);
                }
            } catch(Exception e)
            {
                VRLog.Error("Failed to write {0}", path);
            }

            return context;
        }
    }
}