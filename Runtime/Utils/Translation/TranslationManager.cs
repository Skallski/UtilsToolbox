using System;
using System.Collections.Generic;
using JetBrains.Annotations;
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
        [field: SerializeField] public Language CurrentLanguage { get; private set; }
        
        private readonly List<TextSetter> _textSetters = new List<TextSetter>();
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
            TextSetter.OnTextSetterSetup += OnTextSetterSetup;
        }

        private void OnDisable()
        {
            TextSetter.OnTextSetterSetup -= OnTextSetterSetup;
        }

        private void OnTextSetterSetup(TextSetter textSetter)
        {
            if (_textSetters.Contains(textSetter) == false)
            {
                _textSetters.Add(textSetter);
            }
        }

        public void Setup()
        {
            if (LoadFile())
            {
                SetLanguage(CurrentLanguage); // initial language set for all text setters
            }
        }

        private bool LoadFile()
        {
            _translationDataRoot = new Root();
            
#if UNITY_EDITOR
            return JsonDataReader.Read(_editorFile, ref _translationDataRoot);
#else
            return JsonDataReader.Read(_buildFilePath, ref _translationDataRoot);
#endif
        }

        public void SetLanguage(Language language)
        {
            CurrentLanguage = language;
            
            foreach (TextSetter textSetter in _textSetters)
            {
                textSetter.ChangeLanguage();
            }
        }

        [UsedImplicitly]
        public void SetLanguage(int languageIndex)
        {
            SetLanguage((Language) languageIndex);
        }

        internal string GetTextFromTag(string clientStringTag)
        {
            return GetTextFromTag(clientStringTag, CurrentLanguage);
        }

        private string GetTextFromTag(string clientStringTag, Language language)
        {
            foreach (ClientString clientString in _translationDataRoot.ClientStrings)
            {
                if (clientString.Tag.Equals(clientStringTag))
                {
                    foreach (Translation translation in clientString.Translations)
                    {
                        if (translation.Language.Equals(language.ToString()))
                        {
                            return translation.Value;
                        }
                    }
                }
            }

            return string.Empty;
        }
    }
    
    [Serializable]
    public struct Root
    {
        public ClientString[] ClientStrings;
    }

    [Serializable]
    public struct ClientString
    {
        public string Tag;
        public Translation[] Translations;
    }

    [Serializable]
    public struct Translation
    {
        public string Value;
        public string Language;
    }
}