using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace SkalluUtils.Utils.Tweening
{
    public static class Tweener
    {
        public static IEnumerator ScaleLerp_Coroutine(GameObject target, Vector2 startScale, Vector2 finishScale, 
            float duration, Action onFinish = null)
        {
            yield return Tween_Coroutine(duration, 
                tickDelta =>
                {
                    target.transform.localScale = 
                        Vector3.Lerp(startScale, finishScale, tickDelta);
                },
                () =>
                {
                    target.transform.localScale = finishScale;

                    onFinish?.Invoke();
                }
            );
        }

        public static IEnumerator AlphaLerp_Coroutine(CanvasGroup target, float startAlpha, float finishAlpha,
            float duration, Action onFinish = null)
        {
            yield return Tween_Coroutine(duration, 
                tickDelta =>
                {
                    target.alpha = Mathf.Lerp(startAlpha, finishAlpha, tickDelta);
                },
                () =>
                {
                    target.alpha = finishAlpha;
                    
                    onFinish?.Invoke();
                }
            );
        }
        
        public static IEnumerator ColorLerp_Coroutine(Graphic target, Color startColor, Color finishColor,
            float duration, Action onFinish = null)
        {
            yield return Tween_Coroutine(duration, 
                tickDelta =>
                {
                    target.color = Color.Lerp(startColor, finishColor, tickDelta);
                },
                () =>
                {
                    target.color = finishColor;
                    
                    onFinish?.Invoke();
                }
            );
        }

        public static IEnumerator AnchoredPositionLerp_Coroutine(RectTransform target, Vector2 startPosition,
            Vector2 finishPosition, float duration, Action onFinish = null)
        {
            yield return Tween_Coroutine(duration, 
                tickDelta =>
                {
                    target.anchoredPosition = Vector2.Lerp(startPosition, finishPosition, tickDelta);
                },
                () =>
                {
                    target.anchoredPosition = finishPosition;
                    
                    onFinish?.Invoke();
                }
            );
        }
        
        private static IEnumerator Tween_Coroutine(float duration, Action<float> onTick, Action onFinish = null)
        {
            var elapsedTime = 0f;
            while (elapsedTime < duration)
            {
                onTick?.Invoke(elapsedTime / duration); // tick delta
                
                elapsedTime += Time.unscaledDeltaTime;
                yield return null;
            }
            
            onFinish?.Invoke();
        }
    }
}