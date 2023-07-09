using UnityEngine;

namespace SkalluUtils.Extensions
{
    public static class StringExtensions
    {
        #region RICH TEXT TAGS
        /// <summary>
        /// Changes color of log message to one provided as method parameter.
        /// Call example: Debug.Log("sample text".Color(Color.blue));
        /// </summary>
        /// <param name="text"> string on which the method will be called </param>
        /// <param name="color"> color to set </param>
        /// <returns> colored text </returns>
        public static string Color(this string text, Color color)
        {
            return $"<color=#{ColorUtility.ToHtmlStringRGB(color)}>{text}</color>";
        }

        /// <summary>
        /// Makes log message bold.
        /// </summary>
        /// <param name="text"> string on which the method will be called </param>
        /// <returns> bold text </returns>
        public static string Bold(this string text)
        {
            return $"<b>{text}</b>";
        }

        /// <summary>
        /// Sets size of log message to one provided as method parameter.
        /// </summary>
        /// <param name="text"> string on which the method will be called </param>
        /// <param name="size"> size to set </param>
        /// <returns> text of provided size </returns>
        public static string Size(this string text, int size)
        {
            return $"<size={size}>{text}</size>";
        }

        #endregion
    }
}