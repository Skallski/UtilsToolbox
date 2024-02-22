using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SkalluUtils.Utils.TranslationSystem
{
    /// <summary>
    /// Abstract class that represents base of translation manager
    /// Needs concrete implementation
    /// <remarks>
    /// Requires execution order before TextSetter
    /// </remarks>>
    /// </summary>
    public abstract class TranslationManagerBase<TLanguage> : MonoBehaviour where TLanguage : Enum
    {
        public static TranslationManagerBase<TLanguage> Instance { get; private set; }
        
        [field: SerializeField] public TLanguage CurrentLanguage { get; protected set; }
        
        private readonly HashSet<TextSetter> _textSetters = new HashSet<TextSetter>();
        private Root _rootDTO;

        protected virtual void Awake()
        {
            // singleton pattern implementation
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
            }
        }

        private void OnEnable()
        {
            TextSetter.OnTextSetterInitialized += OnTextSetterInitialized;
            TextSetter.OnTextSetterSet += OnTextSetterSet;
        }

        private void OnDisable()
        {
            TextSetter.OnTextSetterInitialized -= OnTextSetterInitialized;
            TextSetter.OnTextSetterSet -= OnTextSetterSet;
        }

        /// <summary>
        /// Do initialization here
        /// <remarks>
        /// E.g. Load here translation data and map to DTOs
        /// </remarks>>
        /// </summary>
        public virtual void Setup()
        {
            // Do initialization here
            
            // after text file is loaded, set texts in every text setter
            foreach (var textSetter in _textSetters)
            {
                textSetter.SetText(textSetter.NameTag);
            }
        }

        /// <summary>
        /// Called when Start() method is called on TextSetter object
        /// </summary>
        /// <param name="textSetter"> initialized TextSetter object </param>
        private void OnTextSetterInitialized(TextSetter textSetter)
        {
            if (textSetter == null)
            {
                return;
            }

            if (_textSetters.Contains(textSetter) == false)
            {
                _textSetters.Add(textSetter);
                textSetter.SetText(textSetter.NameTag);
            }
        }
        
        /// <summary>
        /// Called when SetText() method is called on TextSetter object
        /// </summary>
        /// <param name="nameTag"> a unique tag that identifies the searched text (e.g. ID) </param>
        /// <returns> string that represents translated text </returns>
        private string OnTextSetterSet(string nameTag)
        {
            return GetTranslatedText(nameTag, CurrentLanguage);
        }

        /// <summary>
        /// Gets text of provided nameTag, translated in provided language
        /// </summary>
        /// <param name="nameTag"> a unique tag that identifies the searched text (e.g. ID) </param>
        /// <param name="language"> language in which translation is needed </param>
        /// <returns> string that represents text of provided nameTag, translated in provided language </returns>
        private string GetTranslatedText(string nameTag, TLanguage language)
        {
            var texts = _rootDTO.Texts;
            if (texts == null || texts.Length == 0 || string.IsNullOrEmpty(nameTag))
            {
                return null;
            }

            Text matchingText = texts.FirstOrDefault(text => text.NameTag.Equals(nameTag));

            return matchingText?.Translations.FirstOrDefault(translation => translation.Language.Equals(language))
                ?.Value ?? string.Empty;
        }

        /// <summary>
        /// Translates every TextSetter object to provided language
        /// </summary>
        /// <param name="language"> enum that represents language </param>
        public void SetLanguage(TLanguage language)
        {
            if (CurrentLanguage.Equals(language))
            {
                return;
            }
            
            CurrentLanguage = language;
            
            // translate text in each text setter
            foreach (var textSetter in _textSetters)
            {
                textSetter.SetText(textSetter.NameTag);
            }
        }

        /// <summary>
        /// Translates every TextSetter object to provided language
        /// </summary>
        /// <param name="languageIndex"> int that represents language by index</param>
        public void SetLanguage(int languageIndex)
        {
            Type type = typeof(TLanguage);
            if (Enum.IsDefined(type, languageIndex))
            {
                TLanguage language = (TLanguage)Enum.ToObject(type, languageIndex);
                SetLanguage(language);
            }
            else
            {
                Debug.LogError($"Language with index of {languageIndex} not found!");
            }
        }

        #region TRANSLATION DTOS
        [Serializable]
        protected class Root
        {
            public Text[] Texts;
        }

        [Serializable]
        protected class Text
        {
            /// <summary>
            /// unique id that is used to identify the text
            /// </summary>
            public string NameTag;
            
            /// <summary>
            /// possible translations
            /// </summary>
            public Translation[] Translations;
        }

        [Serializable]
        protected class Translation
        {
            /// <summary>
            /// translated text as string
            /// </summary>
            public string Value;
            
            /// <summary>
            /// language in which the text is
            /// </summary>
            public TLanguage Language;
        }
        #endregion
    }
}