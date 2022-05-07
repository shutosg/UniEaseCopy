using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using TangentMode = UnityEditor.AnimationUtility.TangentMode;

namespace net.shutosg.UnityEditor
{
    public static class AnimationUtil
    {
        private static readonly Type OriginalType = typeof(AnimationUtility);

        public static TangentMode GetKeyLeftTangentMode(Keyframe key)
            => ReflectionUtil.InvokeStaticMethod<TangentMode>(OriginalType, "GetKeyLeftTangentMode", new object[] { key }, BindingFlags.NonPublic);

        public static TangentMode GetKeyRightTangentMode(Keyframe key)
            => ReflectionUtil.InvokeStaticMethod<TangentMode>(OriginalType, "GetKeyRightTangentMode", new object[] { key }, BindingFlags.NonPublic);

        public static void SetKeyLeftTangentMode(ref Keyframe key, TangentMode tangentMode)
        {
            var args = new object[] { key, tangentMode };
            ReflectionUtil.InvokeStaticMethod(OriginalType, "SetKeyLeftTangentMode", args, BindingFlags.NonPublic);
            key = (Keyframe)args[0];
        }

        public static void SetKeyRightTangentMode(ref Keyframe key, TangentMode tangentMode)
        {
            var args = new object[] { key, tangentMode };
            ReflectionUtil.InvokeStaticMethod(OriginalType, "SetKeyRightTangentMode", args, BindingFlags.NonPublic);
            key = (Keyframe)args[0];
        }

        public static bool GetKeyBroken(Keyframe key)
            => ReflectionUtil.InvokeStaticMethod<bool>(OriginalType, "GetKeyBroken", new object[] { key }, BindingFlags.NonPublic);

        public static void SetKeyBroken(ref Keyframe key, bool broken)
        {
            var args = new object[] { key, broken };
            ReflectionUtil.InvokeStaticMethod(OriginalType, "SetKeyBroken", args, BindingFlags.NonPublic);
            key = (Keyframe)args[0];
        }

        private static (TangentMode leftMode, TangentMode rightMode) GetKeyTangentModes(Keyframe key)
            => (GetKeyLeftTangentMode(key), GetKeyRightTangentMode(key));

        public static bool GetKeyClampedAuto(Keyframe key)
        {
            var modes = GetKeyTangentModes(key);
            return modes.leftMode == TangentMode.ClampedAuto && modes.rightMode == TangentMode.ClampedAuto;
        }

        public static bool GetKeyAuto(Keyframe key)
        {
            var modes = GetKeyTangentModes(key);
            return modes.leftMode == TangentMode.Auto && modes.rightMode == TangentMode.Auto;
        }

        public static bool GetKeyFreeSmooth(Keyframe key)
        {
            var isBroken = GetKeyBroken(key);
            if (isBroken) return false;
            var modes = GetKeyTangentModes(key);
            return modes.leftMode == TangentMode.Free && modes.rightMode == TangentMode.Free;
        }

        public static bool GetKeyFlat(Keyframe key)
        {
            if (key.inTangent != 0 || key.outTangent != 0) return false;
            var isBroken = GetKeyBroken(key);
            if (isBroken) return false;
            var modes = GetKeyTangentModes(key);
            return modes.leftMode == TangentMode.Free && modes.rightMode == TangentMode.Free;
        }

        public static bool GetKeyLeftWeighted(Keyframe key)
            => key.weightedMode == WeightedMode.Both || key.weightedMode == WeightedMode.In;

        public static bool GetKeyRightWeighted(Keyframe key)
            => key.weightedMode == WeightedMode.Both || key.weightedMode == WeightedMode.Out;

        public static void SetKeyLeftWeightedMode(ref Keyframe key, bool isWeighted)
        {
            var isRightWeighted = key.weightedMode == WeightedMode.Both || key.weightedMode == WeightedMode.Out;
            switch (isWeighted)
            {
                case true when isRightWeighted:
                    key.weightedMode = WeightedMode.Both;
                    break;
                case true:
                    key.weightedMode = WeightedMode.In;
                    break;
                default:
                {
                    key.weightedMode = isRightWeighted ? WeightedMode.Out : WeightedMode.None;
                    break;
                }
            }
        }

        public static void SetKeyRightWeightedMode(ref Keyframe key, bool isWeighted)
        {
            var isLeftWeighted = key.weightedMode == WeightedMode.Both || key.weightedMode == WeightedMode.Out;
            switch (isWeighted)
            {
                case true when isLeftWeighted:
                    key.weightedMode = WeightedMode.Both;
                    break;
                case true:
                    key.weightedMode = WeightedMode.Out;
                    break;
                default:
                {
                    key.weightedMode = isLeftWeighted ? WeightedMode.In : WeightedMode.None;
                    break;
                }
            }
        }
    }
}
