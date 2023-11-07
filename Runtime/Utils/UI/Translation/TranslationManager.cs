using System;
using System.Collections.Generic;
using SkalluUtils.Utils.Json;
using UnityEngine;

namespace SkalluUtils.Utils.UI.Translation
{
    /// <summary>
    /// Requires execution order before TextSetter
    /// </summary>
    public class TranslationManager : MonoBehaviour
    {
        public static TranslationManager Instance { get; private set; }

        [field: SerializeField] public Language CurrentLanguage { get; private set; }
        [SerializeField] private JsonFileLocator _jsonFileLocator;
        
        private readonly List<TextSetter> _textSetters = new List<TextSetter>();
        private Root _translationDataRoot;

        private void Awake()
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
            _textSetters.Add(textSetter);
        }

        public void Setup()
        {
            // load file
            string filePath;
#if UNITY_EDITOR
            filePath = _jsonFileLocator.EditorFilePath;
#else
            filePath = _jsonFileLocator.BuildFilePath;
#endif
            _translationDataRoot = new Root();
            JsonDataReader.Read(filePath, ref _translationDataRoot);

            // initial language set for all text setters
            ChangeLanguage(CurrentLanguage);
        }

        public void ChangeLanguage(Language language)
        {
            CurrentLanguage = language;
            
            foreach (TextSetter text in _textSetters)
            {
                text.SetText(text.Tag);
            }
        }

        public void ChangeLanguage(int languageIndex)
        {
            ChangeLanguage((Language) languageIndex);
        }

        public string LoadText(string clientStringTag)
        {
            return LoadText(clientStringTag, CurrentLanguage);
        }

        private string LoadText(string clientStringTag, Language language)
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