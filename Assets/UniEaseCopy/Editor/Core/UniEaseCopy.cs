using System;
using System.Linq;
using net.shutosg.UnityEditor;
using UnityEditor;
using UnityEngine;

namespace net.shutosg.UniEaseCopy
{
    public static class UniEaseCopy
    {
        private static Keyframe[] copiedKeyframes;

        private static AnimEditor GetAnimEditor(Action<UniEaseCopyLog> onLogged = null)
        {
            AnimEditor animEditor;
            try
            {
                animEditor = AnimationWindowUtil.GetAnimEditor();
            }
            catch (AnimationWindowNotFoundException)
            {
                LogUtil.LogError("Open \"Animation\" window, then select keyframes.", onLogged);
                return null;
            }
            catch (AmbiguousAnimationWindowException)
            {
                LogUtil.LogError("Active one \"Animation\" window, then select keyframes.", onLogged);
                return null;
            }

            if (!animEditor.State.ShowCurveEditor)
            {
                LogUtil.LogError("Open \"Curves\" panel, then select keyframes.", onLogged);
                return null;
            }
            return animEditor;
        }

        public static void CopyKeyframes(Action<Keyframe[]> onCopied = null, Action<UniEaseCopyLog> onLogged = null)
        {
            var animEditor = GetAnimEditor(onLogged);
            if (animEditor == null) return;

            var curveEditor = animEditor.CurveEditor;
            var selectedCurves = curveEditor.SelectedCurves;
            if (selectedCurves.Count == 0)
            {
                LogUtil.LogError("There is no keyframe selected.", onLogged);
                return;
            }
            if (selectedCurves.Select(c => c.CurveId).Distinct().Count() != 1)
            {
                LogUtil.LogError("Selected keyframes belong to multiple curves.", onLogged);
                return;
            }

            copiedKeyframes = new Keyframe[selectedCurves.Count];
            var targetCurve = curveEditor.GetCurve(selectedCurves[0]);
            var offset = selectedCurves[0].Key;
            for (var i = 0; i < copiedKeyframes.Length; i++)
            {
                var c = curveEditor.SelectedCurves[i];
                var kf = curveEditor.GetKeyframe(c);
                var isFirst = i == 0;
                var isLast = i == copiedKeyframes.Length - 1;
                if (!isFirst)
                {
                    var prev = targetCurve.GetKeyframe(offset + i - 1);
                    var dt = kf.time - prev.time;
                    var dy = kf.value - prev.value;
                    kf.inTangent = kf.inTangent / dy * dt;
                }
                if (!isLast)
                {
                    var next = targetCurve.GetKeyframe(offset + i + 1);
                    var dt = next.time - kf.time;
                    var dy = next.value - kf.value;
                    kf.outTangent = kf.outTangent / dy * dt;
                }
                if (copiedKeyframes.Length == 1)
                {
                    // コピーしたkeyframeが1つの場合はtangentの状態も全てそのまま
                }
                // 使わないので0を入れておく
                else if (isFirst)
                {
                    kf.inTangent = 0;
                    kf.inWeight = 0;
                }
                else if (isLast)
                {
                    kf.outTangent = 0;
                    kf.outWeight = 0;
                }
                copiedKeyframes[i] = kf;
            }

            onCopied?.Invoke(copiedKeyframes);
            LogUtil.LogSuccess($"{copiedKeyframes.Length} keyframes have been copied.", onLogged);
        }

        public static void PasteEase(Action<UniEaseCopyLog> onLogged = null)
        {
            var animEditor = GetAnimEditor(onLogged);
            if (copiedKeyframes == null)
            {
                LogUtil.LogError("There is no keyframe copied.", onLogged);
                return;
            }
            if (animEditor == null) return;
            var curveEditor = animEditor.CurveEditor;
            var targetCurveGroups = curveEditor.GetSelectingCurveGroups();
            foreach (var t in targetCurveGroups)
            {
                var targetCurve = t.Key;
                var selections = t.Value;
                var offset = selections[0].Key;
                var scopeLength = Mathf.Min(t.Value.Length, copiedKeyframes.Length);
                for (var i = 0; i < scopeLength; i++)
                {
                    var curveSelection = selections[i];
                    var kf = curveEditor.GetKeyframe(curveSelection);
                    var copied = copiedKeyframes[i];
                    var copiedLeftTangent = AnimationUtil.GetKeyLeftTangentMode(copied);
                    var copiedRightTangent = AnimationUtil.GetKeyRightTangentMode(copied);
                    var isFirst = i == 0;
                    var isLast = i == scopeLength - 1;
                    if (isFirst || isLast)
                    {
                        SetBroken(ref kf);
                    }
                    else
                    {
                        if (AnimationUtil.GetKeyBroken(copied))
                        {
                            SetBroken(ref kf);
                        }
                        else
                        {
                            // TODO:
                            // AnimationUtil.SetKeyBroken(ref kf, false);
                        }
                    }

                    if (scopeLength == 1)
                    {
                        AnimationUtil.SetKeyLeftTangentMode(ref kf, copiedLeftTangent);
                        AnimationUtil.SetKeyRightTangentMode(ref kf, copiedRightTangent);
                        kf.weightedMode = copied.weightedMode;
                        kf.inTangent = copied.inTangent;
                        kf.inWeight = copied.inWeight;
                        kf.outTangent = copied.outTangent;
                        kf.outWeight = copied.outWeight;
                    }
                    else if (!isFirst)
                    {
                        AnimationUtil.SetKeyLeftTangentMode(ref kf, copiedLeftTangent);
                        AnimationUtil.SetKeyLeftWeightedMode(ref kf, AnimationUtil.GetKeyLeftWeighted(copied));
                        var prev = targetCurve.GetKeyframe(offset + i - 1);
                        var dt = kf.time - prev.time;
                        var dy = kf.value - prev.value;
                        kf.inTangent = copied.inTangent * dy / dt;
                        kf.inWeight = copied.inWeight;
                    }
                    else if (!isLast)
                    {
                        AnimationUtil.SetKeyRightTangentMode(ref kf, copiedRightTangent);
                        AnimationUtil.SetKeyRightWeightedMode(ref kf, AnimationUtil.GetKeyRightWeighted(copied));
                        var next = targetCurve.GetKeyframe(offset + i + 1);
                        var dt = next.time - kf.time;
                        var dy = next.value - kf.value;
                        kf.outTangent = copied.outTangent * dy / dt;
                        kf.outWeight = copied.outWeight;
                    }
                    targetCurve.MoveKey(curveSelection.Key, ref kf);
                    targetCurve.Changed = true;
                }
            }
            animEditor.SaveChangedCurvesFromCurveEditor("Paste Ease (UniEaseCopy)");
            LogUtil.LogSuccess("Pasted \"Ease\".", onLogged);
        }

        private static void SetBroken(ref Keyframe key)
        {
            AnimationUtil.SetKeyBroken(ref key, true);
            if (AnimationUtil.GetKeyRightTangentMode(key) == AnimationUtility.TangentMode.ClampedAuto || AnimationUtil.GetKeyRightTangentMode(key) == AnimationUtility.TangentMode.Auto)
                AnimationUtil.SetKeyRightTangentMode(ref key, AnimationUtility.TangentMode.Free);
            if (AnimationUtil.GetKeyLeftTangentMode(key) == AnimationUtility.TangentMode.ClampedAuto || AnimationUtil.GetKeyLeftTangentMode(key) == AnimationUtility.TangentMode.Auto)
                AnimationUtil.SetKeyLeftTangentMode(ref key, AnimationUtility.TangentMode.Free);
        }

        public static void PasteValue(Action<UniEaseCopyLog> onLogged = null)
        {
            var animEditor = GetAnimEditor(onLogged);
            if (copiedKeyframes == null)
            {
                LogUtil.LogError("There is no keyframe copied.", onLogged);
                return;
            }
            if (animEditor == null) return;
            var curveEditor = animEditor.CurveEditor;
            var targetCurveGroups = curveEditor.GetSelectingCurveGroups();
            foreach (var t in targetCurveGroups)
            {
                var targetCurve = t.Key;
                var selections = t.Value;
                var offset = selections[0].Key;

                var scopeLength = Mathf.Min(selections.Length, copiedKeyframes.Length);
                var tmpKeys = new Keyframe[scopeLength];
                // 元のTangentの保存
                ExecKeyIteration(x =>
                {
                    var i = x.i;
                    var isFirst = x.isFirst;
                    var isLast = x.isLast;
                    var kf = x.key;
                    if (!isFirst)
                    {
                        var prev = targetCurve.GetKeyframe(offset + i - 1);
                        var dt = kf.time - prev.time;
                        var dy = kf.value - prev.value;
                        tmpKeys[i].inTangent = kf.inTangent / dy * dt;
                    }
                    if (!isLast)
                    {
                        var next = targetCurve.GetKeyframe(offset + i + 1);
                        var dt = next.time - kf.time;
                        var dy = next.value - kf.value;
                        tmpKeys[i].outTangent = kf.outTangent / dy * dt;
                    }
                });
                // valueのペースト
                ExecKeyIteration(x =>
                {
                    var kf = x.key;
                    var copied = x.copied;
                    kf.value = copied.value;
                    targetCurve.MoveKey(x.curveSelection.Key, ref kf);
                });
                // 元のTangentを復元
                ExecKeyIteration(x =>
                {
                    var i = x.i;
                    var isFirst = x.isFirst;
                    var isLast = x.isLast;
                    var kf = x.key;
                    if (!isFirst)
                    {
                        var prev = targetCurve.GetKeyframe(offset + i - 1);
                        var dt = kf.time - prev.time;
                        var dy = kf.value - prev.value;
                        kf.inTangent = tmpKeys[i].inTangent * dy / dt;
                    }
                    if (!isLast)
                    {
                        var next = targetCurve.GetKeyframe(offset + i + 1);
                        var dt = next.time - kf.time;
                        var dy = next.value - kf.value;
                        kf.outTangent = tmpKeys[i].outTangent * dy / dt;
                    }
                    targetCurve.MoveKey(x.curveSelection.Key, ref kf);
                });

                void ExecKeyIteration(Action<(int i, CurveSelection curveSelection, Keyframe key, Keyframe copied, bool isFirst, bool isLast)> eachExec)
                {
                    for (var i = 0; i < scopeLength; i++)
                    {
                        var curveSelection = selections[i];
                        var kf = curveEditor.GetKeyframe(curveSelection);
                        var copied = copiedKeyframes[i];
                        var isFirst = i == 0;
                        var isLast = i == scopeLength - 1;
                        eachExec((i, curveSelection, kf, copied, isFirst, isLast));
                    }
                }

                targetCurve.Changed = true;
            }
            animEditor.SaveChangedCurvesFromCurveEditor("Paste Value (UniEaseCopy)");
            LogUtil.LogSuccess("Pasted \"Value\".", onLogged);
        }

        public static void LogCopied()
        {
            if (copiedKeyframes == null) return;
            foreach (var kf in copiedKeyframes)
            {
                Debug.Log($"[copied] {kf.Dump()}");
            }
        }

        public static void LogSelected()
        {
            var animEditor = GetAnimEditor();
            if (animEditor == null) return;
            var curveEditor = animEditor.CurveEditor;
            var targetCurveGroups = curveEditor.GetSelectingCurveGroups();
            foreach (var target in targetCurveGroups)
            {
                var selections = target.Value;
                for (var i = 0; i < selections.Length; i++)
                {
                    var curveSelection = selections[i];
                    var kf = curveEditor.GetKeyframe(curveSelection);
                    Debug.Log($"[selected] {kf.Dump()}");
                }
            }
        }
    }
}
