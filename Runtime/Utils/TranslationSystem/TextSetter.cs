using System;
using TMPro;
using UnityEngine;

namespace SkalluUtils.Utils.TranslationSystem
{
    /// <summary>
    /// <remarks>
    /// Requires execution order after TranslationManager
    /// </remarks>>
    /// </summary>
    public class TextSetter : MonoBehaviour
    {
        public static event Action<TextSetter> OnTextSetterInitialized;
        public static event Func<string ,string> OnTextSetterSet;
        
        [SerializeField] private TMP_Text _label;
        [field:Space]
        [field: SerializeField] public string NameTag { get; private set; }
        [SerializeField, Multiline] private string _alternativeText;
        
#if UNITY_EDITOR
        private void Reset()
        {
            if (_label == null)
            {
                _label = GetComponent<TMP_Text>();
            }
        }
#endif
        
        private void Start()
        {
            if (_label == null)
            {
                Debug.LogError($"{gameObject.name} {name}: Label cannot be null!");
                return;
            }

            OnTextSetterInitialized?.Invoke(this);
        }

        /// <summary>
        /// Sets label's text based on nameTag
        /// </summary>
        /// <param name="nameTag"> a unique tag that identifies the searched text (e.g. ID) </param>
        /// <param name="parameters"> additional text parameters </param>
        public void SetText(string nameTag, params object[] parameters)
        {
            string text = OnTextSetterSet?.Invoke(nameTag);
            if (string.IsNullOrEmpty(text))
            {
                text = _alternativeText;
                Debug.LogError($"{gameObject.name} {name}: Name tag: {nameTag} not found!");
            }
            else
            {
                for (int i = 0; i < parameters.Length; i++)
                {
                    text = text.Replace($"{{{i}}}", parameters[i].ToString());
                }
                
                NameTag = nameTag;
            }

            _label.SetText(text);
        }

        /// <summary>
        /// Gets the text from label
        /// </summary>
        /// <returns> text from label as string </returns>
        public string GetLabelText()
        {
            return _label.text;
        }
    }
}