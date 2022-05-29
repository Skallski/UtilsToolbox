﻿using System.Collections.Generic;
 using UnityEngine;

namespace SkalluUtils.Extensions
{
    namespace StringExtensions
    {
        public static class StringExtensions
        {
            /// <summary>
            /// Changes color of log message to one provided as method parameter
            /// Call example: Debug.Log("sample text".Color(Color.blue));
            /// </summary>
            /// <param name="text"> string on which the method will be called </param>
            /// <param name="color"> color to apply </param>
            /// <returns> colored text </returns>
            public static string Color(this string text, Color color)
            {
                return $"<color=#{ColorUtility.ToHtmlStringRGB(color)}>{text}</color>";
            }
        }
    }
}