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
                OnTextSetterSetup?.Invoke(this);
            }
        }

        public void SetText(string clientStringTag)
        {
            if (_label == null)
            {
                _label.SetText(_alternativeText);
            }

            string translatedText = TranslationManager.Instance.LoadText(clientStringTag);
            if (translatedText.Equals(string.Empty) == false)
            {
                _label.SetText(translatedText);
            }
            else
            {
                _label.SetText(_alternativeText);
                Debug.LogError($"{gameObject.name}: translation error!");
            }
            
            Tag = clientStringTag;
        }

        public string GetLabelText()
        {
            return _label.text;
        }
    }
}