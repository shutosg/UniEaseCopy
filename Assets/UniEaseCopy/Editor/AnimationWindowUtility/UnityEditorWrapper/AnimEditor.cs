using System.Reflection;
using UnityEditor;

namespace net.shutosg.UnityEditor
{
    public class AnimEditor : UnityEditorWrapperBase
    {
        public CurveEditor CurveEditor { get; }
        public EditorWindow OwnerWindow { get; }
        public AnimationWindowState State { get; }

        public AnimEditor(object original) : base(original)
        {
            CurveEditor = GetNonPublicField("m_CurveEditor", o => new CurveEditor(o));
            OwnerWindow = GetNonPublicField<EditorWindow>("m_OwnerWindow");
            State = GetNonPublicField("m_State", o => new AnimationWindowState(o));
        }

        public void SaveChangedCurvesFromCurveEditor(string undoLabel)
        {
            InvokeNonPublicMethod("SaveChangedCurvesFromCurveEditor");
        }
    }

    public static class EditorWindowExtension
    {
        public static AnimEditor GetAnimEditor(this EditorWindow window)
        {
            var animationWindowType = UnityEditorAssembly.GetClassType("AnimationWindow");
            return new AnimEditor(animationWindowType.GetField("m_AnimEditor", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(window));
        }
    }
}
