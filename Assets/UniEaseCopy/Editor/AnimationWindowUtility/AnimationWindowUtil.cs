using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace net.shutosg.UnityEditor
{
    public static class AnimationWindowUtil
    {
        private static readonly Type AnimWindowType = UnityEditorAssembly.GetClassType("AnimationWindow");

        public static EditorWindow[] GetOpenedAnimationWindows()
        {
            return (EditorWindow[])Resources.FindObjectsOfTypeAll(AnimWindowType);
        }

        public static bool IsAnimationWindow(this EditorWindow window)
        {
            return window.GetType() == AnimWindowType;
        }

        public static EditorWindow GetAnimationWindow()
        {
            return EditorWindow.GetWindow(AnimWindowType, false, "Animation", false);
        }

        public static AnimEditor GetAnimEditor()
        {
            var focusedWindow = EditorWindow.focusedWindow;
            var openedAnimationWindows = GetOpenedAnimationWindows();
            // そもそもAnimationウィンドウを開いているかチェック
            if (openedAnimationWindows.Length == 0) throw new AnimationWindowNotFoundException();
            if (openedAnimationWindows.Length > 1 && !focusedWindow.IsAnimationWindow()) throw new AmbiguousAnimationWindowException();
            var animationWindow = GetAnimationWindow();
            var animEditor = animationWindow.GetAnimEditor();
            return animEditor;
        }

        public static Keyframe[] GetSelectedKeyframes()
        {
            var curveEditor = GetAnimEditor().CurveEditor;
            return curveEditor.SelectedCurves.Select(c => curveEditor.GetKeyframe(c)).ToArray();
        }
    }

    public class AnimationWindowNotFoundException : Exception { }

    public class AmbiguousAnimationWindowException : Exception { }
}
