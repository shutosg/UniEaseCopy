namespace net.shutosg.UnityEditor
{
    public class AnimationWindowState : UnityEditorWrapperBase
    {
        public AnimationWindowState(object original) : base(original) { }

        public bool ShowCurveEditor
        {
            get => GetPublicField<bool>("showCurveEditor");
            set => SetPublicField("showCurveEditor", value);
        }
    }
}
