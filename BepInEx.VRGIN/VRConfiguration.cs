using System;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;
using VRGIN.Core;
using VRGIN.Visuals;

namespace BepInEx.VRGIN
{
    public class VRConfiguration : IVRManagerContext
    {
        public string GuiLayer { get; }
        public string UILayer { get; }
        public int UILayerMask { get; }
        public int IgnoreMask { get; }
        public Color PrimaryColor { get; }
        public IMaterialPalette Materials { get; }
        public VRSettings Settings { get; }
        public string InvisibleLayer { get; }
        public bool SimulateCursor { get; }
        public bool GUIAlternativeSortingMode { get; }
        public Type VoiceCommandType { get; }
        public float GuiNearClipPlane { get; }
        public float GuiFarClipPlane { get; }
        public float NearClipPlane { get; }
        public float UnitToMeter { get; }
        public bool EnforceDefaultGUIMaterials { get; }
        public bool ConfineMouse { get; }
        public GUIType PreferredGUI { get; }
        
        public static IVRManagerContext CreateContext(string path) {
            var serializer = new XmlSerializer(typeof(VRConfiguration));

            if(File.Exists(path))
            {
                // Attempt to load XML
                using (var file = File.OpenRead(path))
                {
                    try
                    {
                        return serializer.Deserialize(file) as VRConfiguration;
                    }
                    catch (Exception e)
                    {
                        Debug.unityLogger.LogError("Failed to deserialize {0} -- using default", path);
                    }
                }
            }

            // Create and save file
            var context = new VRConfiguration();
            try
            {
                using (var file = new StreamWriter(path))
                {
                    file.BaseStream.SetLength(0);
                    serializer.Serialize(file, context);
                }
            } catch(Exception e)
            {
                Debug.unityLogger.LogError("Failed to write {0}", path);
            }

            return context;
        }
    }
}