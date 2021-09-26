using System;
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
    }
}