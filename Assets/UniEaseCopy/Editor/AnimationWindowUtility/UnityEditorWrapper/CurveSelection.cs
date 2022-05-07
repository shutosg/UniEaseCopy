namespace net.shutosg.UnityEditor
{
    public class CurveSelection : UnityEditorWrapperBase
    {
        public enum SelectionType
        {
            Key = 0,
            InTangent = 1,
            OutTangent = 2,
            Count = 3,
        }

        public int CurveId { get; }
        public int Key { get; }
        public bool SemiSelected { get; }
        public SelectionType Type { get; }

        public CurveSelection(object original) : base(original)
        {
            CurveId = GetPublicField<int>("curveID");
            Key = GetPublicField<int>("key");
            SemiSelected = GetPublicField<bool>("semiSelected");
            Type = GetPublicField<SelectionType>("type");
        }
    }
}
