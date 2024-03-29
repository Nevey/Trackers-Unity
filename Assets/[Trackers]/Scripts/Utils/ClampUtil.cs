using UnityEngine;

namespace Trackers.Utils
{
    public static class ClampUtil
    {
        public static float Angle(float angle, float min, float max)
        {
            angle = angle % 360;

            if ((angle >= -360f) && (angle <= 360f))
            {
                if (angle < -360f)
                {
                    angle += 360f;
                }

                if (angle > 360f)
                {
                    angle -= 360f;
                }
            }

            return Mathf.Clamp(angle, min, max);
        }
    }
}