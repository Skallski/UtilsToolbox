using SkalluUtils.Extensions.Collections;
using TMPro;
using UnityEngine;

namespace SkalluUtils.Utils.Translation
{
    /// <summary>
    /// Requires execution order after TranslationManager
    /// </summary>
    public class TextSetter : MonoBehaviour
    {
        public static event System.Action<TextSetter> OnTextSetterSetup;
        
        [SerializeField] private TMP_Text _label;
        [field:Space]
        [field: SerializeField] public string Tag { get; private set; }
        [SerializeField, Multiline] private string _alternativeText;
        
        private TranslationManager _translationManager;
        private Language _language;
        private object[] _parameters;

#if UNITY_EDITOR
        private void Reset()
        {
            if (_label == null)
            {
                _label = GetComponent<TMP_Text>();
            }
        }
#endif
        
        private void Awake()
        {
            if (_label == null)
            {
                Debug.LogError($"{gameObject.name}: translator requires valid TextMeshPro component!");
            }
            else
            {
                _translationManager = TranslationManager.Instance;
                _language = _translationManager.CurrentLanguage;
                
                OnTextSetterSetup?.Invoke(this);
            }
        }

        private void OnEnable()
        {
            if (_translationManager.CurrentLanguage != _language)
            {
                ChangeLanguage();
            }
        }

        public void SetText(string clientStringTag, params object[] parameters)
        {
            string text = _translationManager.GetTextFromTag(clientStringTag);
            if (string.IsNullOrEmpty(text))
            {
                Debug.LogError($"{gameObject.name}: translation error for tag '{clientStringTag}'!");
                text = _alternativeText;
            }
            else
            {
                Tag = clientStringTag;
                _parameters = parameters;
            }

            // set parameters
            if (parameters.IsNullOrEmpty() == false)
            {
                for (int i = 0; i < parameters.Length; i++)
                {
                    string placeHolder = $"{{{i}}}";
                    if (text.Contains(placeHolder))
                    {
                        text = text.Replace(placeHolder, parameters[i].ToString());
                    }
                }
            }

            _label.SetText(text);
        }

        internal void ChangeLanguage()
        {
            _language = _translationManager.CurrentLanguage;
            SetText(Tag, _parameters);
        }

        public string GetLabelText()
        {
            return _label.text;
        }
    }
}