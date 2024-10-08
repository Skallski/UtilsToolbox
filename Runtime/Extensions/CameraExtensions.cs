﻿using UnityEngine;

namespace UtilsToolbox.Extensions
{
    public static class CameraExtensions
    {
        public static bool IsObjectVisible(this Camera camera, Bounds bounds)
        {
            return GeometryUtility.TestPlanesAABB(GeometryUtility.CalculateFrustumPlanes(camera), bounds);
        }
    }
}