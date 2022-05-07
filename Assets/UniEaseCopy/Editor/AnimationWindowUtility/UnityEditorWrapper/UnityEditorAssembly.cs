using System;
using System.Reflection;

namespace net.shutosg.UnityEditor
{
    public static class UnityEditorAssembly
    {
        private const string UnityEditor = "UnityEditor";
        private static readonly Assembly unityEditorAsm = Assembly.Load(UnityEditor);

        public static Type GetClassType(string className) => unityEditorAsm.GetType($"{UnityEditor}.{className}");
    }
}
