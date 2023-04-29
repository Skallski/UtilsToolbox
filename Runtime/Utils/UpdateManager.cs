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
        private readonly List<Action> _fixedUpdateList = new List<Action>();
        private readonly List<Action> _updateList = new List<Action>();
        private readonly List<Action> _lateUpdateList = new List<Action>();

        [SerializeField] protected bool _paused;

        private void FixedUpdate()
        {
            if (_paused) return;

            for (var i = 0; i < _fixedUpdateList.Count; i++)
            {
                _fixedUpdateList[i].Invoke();
            }
        }

        private void Update()
        {
            if (_paused) return;

            for (var i = 0; i < _updateList.Count; i++)
            {
                _updateList[i].Invoke();
            }
        }

        private void LateUpdate()
        {
            if (_paused) return;

            for (int i = 0; i < _lateUpdateList.Count; i++)
            {
                _lateUpdateList[i].Invoke();
            }
        }

        #region PUBLIC METHODS
        public void AddListener(UpdateType updateType, Action updateAction)
        {
            if (updateAction == null)
                return;

            switch (updateType)
            {
                case UpdateType.FixedUpdate:
                {
                    if (_fixedUpdateList.Contains(updateAction) == false)
                        _fixedUpdateList.Add(updateAction);
                    
                    break;
                }
                    
                case UpdateType.Update:
                {
                    if (_updateList.Contains(updateAction) == false)
                        _updateList.Add(updateAction);
                    
                    break;
                }
                    
                case UpdateType.LateUpdate:
                {
                    if (_lateUpdateList.Contains(updateAction) == false)
                        _lateUpdateList.Add(updateAction);
                    
                    break;
                }
            }
        }

        public void RemoveListener(UpdateType updateType, Action updateAction)
        {
            if (updateAction == null)
                return;
            
            switch (updateType)
            {
                case UpdateType.FixedUpdate:
                {
                    if (_fixedUpdateList.Contains(updateAction))
                        _fixedUpdateList.Remove(updateAction);
                    
                    break;
                }
                
                case UpdateType.Update:
                {
                    if (_updateList.Contains(updateAction))
                        _updateList.Remove(updateAction);
                    
                    break;
                }
                
                case UpdateType.LateUpdate:
                {
                    if (_lateUpdateList.Contains(updateAction))
                        _lateUpdateList.Remove(updateAction);
                    
                    break;
                }
            }
        }

        public void Clear(UpdateType updateType)
        {
            switch (updateType)
            {
                case UpdateType.FixedUpdate:
                    _fixedUpdateList.Clear();
                    break;

                case UpdateType.Update:
                    _updateList.Clear();
                    break;

                case UpdateType.LateUpdate:
                    _lateUpdateList.Clear();
                    break;
            }
        }
        #endregion
        
    }
}