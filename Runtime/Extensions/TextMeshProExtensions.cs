using System.Text;
using TMPro;
using UnityEngine;

namespace UtilsToolbox.Extensions
{
    public static class TextMeshProExtensions
    {
        public static void SetTextAnimated(this TMP_Text textMeshPro, string text, float eachLetterDelay)
        {
            WaitForSeconds delay = new WaitForSeconds(eachLetterDelay);
            textMeshPro.StartCoroutine(AnimateLetters());
                
            System.Collections.IEnumerator AnimateLetters()
            {
                StringBuilder animatedText = new StringBuilder();
                
                foreach (var letter in text)
                {
                    animatedText.Append(letter);
                    textMeshPro.SetText(animatedText.ToString());
                    
                    yield return delay;
                }
                
                textMeshPro.SetText(animatedText.ToString());
            }
        }
        
        public static void FadeOut(this TMP_Text textMeshPro, float duration)
        {
            textMeshPro.StartCoroutine(FadeOutRoutine());
            
            System.Collections.IEnumerator FadeOutRoutine()
            {
                float elapsedTime = 0;

                Color originalColor = textMeshPro.color;
                Color transparentColor = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);

                while (elapsedTime < duration)
                {
                    float aplha = Mathf.Lerp(1f, 0f, elapsedTime / duration);
                    textMeshPro.color = Color.Lerp(originalColor, transparentColor, aplha);

                    elapsedTime += Time.deltaTime;
                    yield return null;
                }
            }
        }
    }
}