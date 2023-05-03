using System;
using System.Collections.Generic;
using UnityEngine;

namespace SkalluUtils.Utils
{
    public enum UpdateType
    {
        FixedUpdate,
        Update,
        LateUpdate
    }

    public class UpdateManager : MonoBehaviour
    {
        private static readonly List<Action> FixedUpdateList = new List<Action>();
        private static readonly List<Action> UpdateList = new List<Action>();
        private static readonly List<Action> LateUpdateList = new List<Action>();

        [SerializeField] protected bool _paused;

        private void FixedUpdate()
        {
            if (_paused) return;

            foreach (var fixedUpdateAction in FixedUpdateList)
            {
                fixedUpdateAction?.Invoke();
            }
        }

        private void Update()
        {
            if (_paused) return;

            foreach (var updateAction in UpdateList)
            {
                updateAction?.Invoke();
            }
        }

        private void LateUpdate()
        {
            if (_paused) return;

            foreach (var lateUpdateAction in LateUpdateList)
            {
                lateUpdateAction?.Invoke();
            }
        }

        #region PUBLIC METHODS
        public static void AddListener(UpdateType updateType, Action updateAction)
        {
            if (updateAction == null)
                return;

            switch (updateType)
            {
                case UpdateType.FixedUpdate:
                {
                    if (FixedUpdateList.Contains(updateAction) == false)
                        FixedUpdateList.Add(updateAction);
                    
                    break;
                }
                case UpdateType.Update:
                {
                    if (UpdateList.Contains(updateAction) == false)
                        UpdateList.Add(updateAction);
                    
                    break;
                }
                case UpdateType.LateUpdate:
                {
                    if (LateUpdateList.Contains(updateAction) == false)
                        LateUpdateList.Add(updateAction);
                    
                    break;
                }
            }
        }

        public static void RemoveListener(UpdateType updateType, Action updateAction)
        {
            if (updateAction == null)
                return;
            
            switch (updateType)
            {
                case UpdateType.FixedUpdate:
                {
                    if (FixedUpdateList.Contains(updateAction))
                        FixedUpdateList.Remove(updateAction);
                    
                    break;
                }
                case UpdateType.Update:
                {
                    if (UpdateList.Contains(updateAction))
                        UpdateList.Remove(updateAction);
                    
                    break;
                }
                case UpdateType.LateUpdate:
                {
                    if (LateUpdateList.Contains(updateAction))
                        LateUpdateList.Remove(updateAction);
                    
                    break;
                }
            }
        }
        #endregion
    }
}