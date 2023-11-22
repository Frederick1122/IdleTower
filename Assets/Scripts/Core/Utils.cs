using System;
using UnityEngine;

namespace Core
{
    public static class Utils
    {
        public static Vector3 GetWithoutAxis(Vector3 vec, Enums.Axis axis)
        {
            switch (axis)
            {
                case Enums.Axis.x:
                    vec.x = 0.0f;
                    break;
                case Enums.Axis.y:
                    vec.y = 0.0f;
                    break;
                case Enums.Axis.z:
                    vec.z = 0.0f;
                    break;
            }
            return vec;
        }

    }
}