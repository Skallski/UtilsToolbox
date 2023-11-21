using UnityEngine;

namespace SkalluUtils.Utils.ObjectCulling
{
    [RequireComponent(typeof(ObjectCuller))]
    public class CullableObject : MonoBehaviour, ICullable
    {
        [SerializeField] private MultiSwitch.MultiSwitch _cullingSwitch;
        [SerializeField] private ObjectCuller _culler;

#if UNITY_EDITOR
        private void Reset()
        {
            if (_cullingSwitch == null) _cullingSwitch = GetComponent<MultiSwitch.MultiSwitch>();
            if (_culler == null) _culler = GetComponent<ObjectCuller>();
        }
#endif

        private void Awake()
        {
            _culler.AddCuller(this, Camera.main);
        }
        
        public void OnVisible()
        {
            _cullingSwitch.SetState(true);
        }

        public void OnInvisible()
        {
            _cullingSwitch.SetState(false);
        }

        public void OnDestroy()
        {
            _culler.RemoveCuller();
        }
    }
}