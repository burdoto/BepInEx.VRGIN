using System;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;
using VRGIN.Core;
using VRGIN.Helpers;
using VRGIN.Visuals;

namespace BepInEx.VRGIN
{
    public abstract class VRPlugin : BaseUnityPlugin
    {
        public static VRPlugin Instance;
        public VRSettings Settings;
        public VRManager Manager;
        public IVRManagerContext Context;

        public VRPlugin()
        {
            Instance = this;
        }

        public static bool Active => !Environment.CommandLine.Contains("--novr") &&
                                     (Environment.CommandLine.Contains("--vr") || SteamVRDetector.IsRunning);

        private void Awake()
        {
            if (Active)
            {
                var pathPrefix = GetPathPrefix();
                Settings = VRSettings.Load<VRSettings>(Path.Combine(pathPrefix, "VRSettings.xml"));
                Context = CreateContext(Path.Combine(pathPrefix, "VRContext.xml"));
                Manager = CreateVRManager(Context);
            }
        }

        private IVRManagerContext CreateContext(string path)
        {
            var serializer = new XmlSerializer(typeof(DefaultContext));

            if (File.Exists(path))
                // Attempt to load XML
                using (var file = File.OpenRead(path))
                {
                    try
                    {
                        return serializer.Deserialize(file) as IVRManagerContext;
                    }
                    catch (Exception e)
                    {
                        VRLog.Error("Failed to deserialize {0} -- using default", path, e);
                    }
                }

            // Create and save file
            var context = CreateVRContext();
            try
            {
                using (var file = new StreamWriter(path))
                {
                    file.BaseStream.SetLength(0);
                    serializer.Serialize(file, context);
                }
            }
            catch (Exception e)
            {
                VRLog.Error("Failed to write {0}", path, e);
            }

            return context;
        }

        public abstract IVRManagerContext CreateVRContext();
        public abstract string GetPathPrefix();
        public abstract VRManager CreateVRManager(IVRManagerContext ctx);
    }

    public sealed class DefaultContext : IVRManagerContext
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
    }
}