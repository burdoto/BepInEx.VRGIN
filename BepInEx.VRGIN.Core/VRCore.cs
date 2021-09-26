using System;
using System.IO;
using System.Xml.Serialization;
using VRGIN.Core;

namespace BepInEx.VRGIN.Core
{
    [BepInPlugin(ModGuid_Core, "BepInEx VRGIN Mapping Provider", Version)]
    public static class VRCore
    {
        public const string Version = "0.0.1";
        public const string ModGuid = "BepInEx.VRGIN";
        public const string ModGuid_Core = "BepInEx.VRGIN.Core";
        public const string ModGuid_Context = "BepInEx.VRGIN.Context";
        public static Type VrContextType = typeof(IVRManagerContext);
        public static string PathPrefix = "";
        public static Func<IVRManagerContext, VRManager> GameInterpreterFactory = ctx => VRManager.Create<GameInterpreter>(ctx);

        public static IVRManagerContext LoadVrContext() => CreateContext(Path.Combine(PathPrefix, "VRContext.xml"));

        public static VRSettings LoadVrSettings() => VRSettings.Load<VRSettings>(Path.Combine(PathPrefix, "VRSettings.xml"));

        private static IVRManagerContext CreateContext(string path) {
            var serializer = new XmlSerializer(VrContextType);

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
                        VRLog.Error("Failed to deserialize {0} -- using default", path, e);
                    }
                }
            }

            // Create and save file
            var context = VrContextType.GetConstructor(Type.EmptyTypes)
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
                VRLog.Error("Failed to write {0}", path, e);
            }

            return context;
        }
    }
}