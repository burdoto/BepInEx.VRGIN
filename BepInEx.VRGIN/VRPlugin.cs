using System;
using System.IO;
using System.Xml.Serialization;
using VRGIN.Core;
using VRGIN.Helpers;

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
            var vrContextType = GetVrContextType();
            var serializer = new XmlSerializer(vrContextType);

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
            var context = vrContextType.GetConstructor(new[] { typeof(VRSettings) })
                ?.Invoke(new object[] { Settings }) as IVRManagerContext;
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

        public abstract Type GetVrContextType();
        public abstract string GetPathPrefix();
        public abstract VRManager CreateVRManager(IVRManagerContext ctx);
    }
}