using net.shutosg.UnityEditor;
using UnityEditor;
using UnityEngine;

namespace net.shutosg.UniEaseCopy
{
#if UNIEASECOPY_USE_MENU_ITEM
    public class UniEaseCopyEditor : Editor
    {
        [MenuItem("UniEaseCopy/Copy", false, 0)]
        public static void Copy()
        {
            UniEaseCopy.CopyKeyframes(onLogged: ShowDialogIfError);
        }

        [MenuItem("UniEaseCopy/PasteEase", false, 100)]
        public static void PasteEase()
        {
            UniEaseCopy.PasteEase(ShowDialogIfError);
        }

        [MenuItem("UniEaseCopy/PasteValue", false, 101)]
        public static void PasteValue()
        {
            UniEaseCopy.PasteValue(ShowDialogIfError);
        }

        [MenuItem("UniEaseCopy/Log/Copied", false, 200)]
        public static void LogCopied()
        {
            UniEaseCopy.LogCopied();
        }

        [MenuItem("UniEaseCopy/Log/Selected", false, 201)]
        public static void LogSelected()
        {
            UniEaseCopy.LogSelected();
        }

        private static void ShowDialogIfError(UniEaseCopyLog log)
        {
            if (log.Type == LogType.Error) EditorUtility.DisplayDialog("UniEaseCopy Error", log.Message, "OK");
        }
    }
#endif
}
