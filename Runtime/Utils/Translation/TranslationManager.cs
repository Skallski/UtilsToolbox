using System;
using System.Collections.Generic;
using System.Linq;
using SkalluUtils.Utils.Json;
using UnityEngine;

namespace SkalluUtils.Utils.Translation
{
    /// <summary>
    /// Requires execution order before TextSetter
    /// </summary>
    public class TranslationManager : MonoBehaviour
    {
        public static TranslationManager Instance { get; private set; }
        
#if UNITY_EDITOR
        [SerializeField] private TextAsset _editorFile;
#endif
        [SerializeField, Multiline] private string _buildFilePath;
        
        [field: Space]
        [field: SerializeField] public Language CurrentLanguage { get; protected set; }
        
        private readonly HashSet<TextSetter> _textSetters = new HashSet<TextSetter>();
        private Root _translationDataRoot;

        protected virtual void Awake()
        {
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
            TextSetter.OnTextSetterCreated += OnTextSetterCreated;
            TextSetter.OnTextSetterSet += OnTextSetterSet;
        }

        private void OnDisable()
        {
            TextSetter.OnTextSetterCreated -= OnTextSetterCreated;
            TextSetter.OnTextSetterSet -= OnTextSetterSet;
        }

        public void Setup()
        {
#if UNITY_EDITOR
            JsonDataReader.Read(_editorFile, ref _translationDataRoot);
#else
            JsonDataReader.Read(_buildFilePath, ref _translationDataRoot);
#endif
            // after text file is loaded, set texts in every text setter
            foreach (var textSetter in _textSetters)
            {
                textSetter.SetText(textSetter.NameTag);
            }
        }

        private void OnTextSetterCreated(TextSetter textSetter)
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
        
        private string OnTextSetterSet(string nameTag)
        {
            return GetTranslatedText(nameTag, CurrentLanguage);
        }

        private string GetTranslatedText(string nameTag, Language language)
        {
            var texts = _translationDataRoot.Texts;
            if (texts == null || texts.Length == 0 || string.IsNullOrEmpty(nameTag))
            {
                return null;
            }

            Text matchingText = texts.FirstOrDefault(text => text.NameTag.Equals(nameTag));

            return matchingText?.Translations.FirstOrDefault(translation => translation.Language.Equals(language))
                ?.Value ?? string.Empty;
        }

        public void SetLanguage(Language language)
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

        public void SetLanguage(int languageIndex)
        {
            if (Enum.IsDefined(typeof(Language), languageIndex))
            {
                SetLanguage((Language)languageIndex);
            }
            else
            {
                Debug.LogError($"Language with index of {languageIndex} not found!");
            }
        }
    }
    
    [Serializable]
    public class Root
    {
        public Text[] Texts;
    }

    [Serializable]
    public class Text
    {
        public string NameTag;
        public Translation[] Translations;
    }

    [Serializable]
    public class Translation
    {
        public string Value;
        public Language Language;
    }
}