﻿using UnityEngine;

namespace UtilsToolbox.Extensions
{
    public static class ColorExtensions
    {
        /// <summary>
        /// Sets color's red value
        /// </summary>
        /// <param name="c"> color on which the method will be called </param>
        /// <param name="r"> red value to set </param>
        public static Color SetR(this Color c, float r)
        {
            return new Color(r, c.g, c.b, c.a);
        }

        /// <summary>
        /// Sets color's green value
        /// </summary>
        /// <param name="c"> color on which the method will be called </param>
        /// <param name="g"> green value to set </param>
        public static Color SetG(this Color c, float g)
        {
            return new Color(c.r, g, c.b, c.a);
        }

        /// <summary>
        /// Sets color's blue value
        /// </summary>
        /// <param name="c"> color on which the method will be called </param>
        /// <param name="b"> blue value to set </param>
        public static Color SetB(this Color c, float b)
        {
            return new Color(c.r, c.g, b, c.a);
        }

        /// <summary>
        /// Sets color's alpha value
        /// </summary>
        /// <param name="c"> color on which the method will be called </param>
        /// <param name="a"> alpha value to set </param>
        public static Color SetA(this Color c, float a)
        {
            return new Color(c.r, c.g, c.b, a);
        }
    }
}