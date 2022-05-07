using System.Collections.Generic;
using System.Linq;

namespace net.shutosg.UnityEditor
{
    public static class CurveEditorExtension
    {
        /// <summary>
        /// CurveSelection(keyframe1つ分に関する情報)の配列を、CurveSelectionが所属するカーブ毎にグルーピングして返す
        /// </summary>
        /// <param name="curveEditor"></param>
        /// <returns></returns>
        public static Dictionary<CurveWrapper, CurveSelection[]> GetSelectingCurveGroups(this CurveEditor curveEditor)
        {
            return curveEditor.SelectedCurves
                .Select(c => (curveId: c.CurveId, selection: c))
                .GroupBy(x => x.curveId)
                .ToDictionary(g1 => curveEditor.GetCurve(g1.Key), g2 => g2.Select(g3 => g3.selection).ToArray());
        }
    }
}
