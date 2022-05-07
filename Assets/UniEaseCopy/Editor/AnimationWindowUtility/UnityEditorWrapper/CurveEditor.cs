using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace net.shutosg.UnityEditor
{
    public class CurveWrapper : UnityEditorWrapperBase
    {
        private AnimationCurve Curve { get; }
        public bool Changed
        {
            get => GetPublicProp<bool>("changed");
            set => SetPublicProp("changed", value);
        }
        public int KeyCount => Curve.length;

        public CurveWrapper(object original) : base(original)
        {
            Curve = GetPublicProp<AnimationCurve>("curve");
        }

        public int MoveKey(int index, ref Keyframe key)
        {
            return InvokePublicMethod<int>("MoveKey", new object[] { index, key });
        }

        public Keyframe GetKeyframe(int index)
        {
            return Curve.keys[index];
        }
    }

    public class CurveEditor : UnityEditorWrapperBase
    {
        private readonly Dictionary<int, int> curveIDToIndexMap;
        public CurveWrapper[] AnimationCurves { get; }
        public List<CurveSelection> SelectedCurves { get; } // 選択しているキーフレームに関する情報

        public CurveEditor(object original) : base(original)
        {
            SelectedCurves = GetNonPublicListProp("selectedCurves", o => new CurveSelection(o));
            AnimationCurves = GetPublicArrayProp("animationCurves", o => new CurveWrapper(o));
            curveIDToIndexMap = GetNonPublicProp<Dictionary<int, int>>("curveIDToIndexMap");
        }

        public Keyframe GetKeyframe(CurveSelection c) => GetCurve(c).GetKeyframe(c.Key);

        public CurveWrapper GetCurve(int curveId) => AnimationCurves[curveIDToIndexMap[curveId]];

        public CurveWrapper GetCurve(CurveSelection c) => GetCurve(c.CurveId);
    }
}
