using UnityEngine;

namespace net.shutosg.UnityEditor
{
    public static class KeyframeExtension
    {
        public static string Dump(this Keyframe kf)
        {
            var leftTanMode = AnimationUtil.GetKeyLeftTangentMode(kf);
            var rightTanMode = AnimationUtil.GetKeyRightTangentMode(kf);
            var isBroken = AnimationUtil.GetKeyBroken(kf);
            return $"time: {kf.time}, value: {kf.value}, isBroken: {isBroken}, inTangent: {kf.inTangent}, outTangent: {kf.outTangent}, inWeight: {kf.inWeight}, outWeight: {kf.outWeight}, leftTangentMode: {leftTanMode}, rightTangentMode: {rightTanMode}, weightedMode: {kf.weightedMode}";
        }
    }
}
