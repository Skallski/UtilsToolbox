﻿using System;
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
        public static UpdateManager Instance { get; private set; }
        
        private List<Action> _fixedUpdateList = new List<Action>();
        private List<Action> _updateList = new List<Action>();
        private List<Action> _lateUpdateList = new List<Action>();

        [SerializeField] protected bool paused;

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

        private void FixedUpdate()
        {
            if (paused)
            {
                return;
            }
            
            for (var i = 0; i < _fixedUpdateList.Count; i++)
            {
                _fixedUpdateList[i].Invoke();
            }
        }

        private void Update()
        {
            if (paused)
            {
                return;
            }
            
            for (var i = 0; i < _updateList.Count; i++)
            {
                _updateList[i].Invoke();
            }
        }

        private void LateUpdate()
        {
            if (paused)
            {
                return;
            }
            
            for (int i = 0; i < _lateUpdateList.Count; i++)
            {
                _lateUpdateList[i].Invoke();
            }
        }

        #region PUBLIC METHODS
        public void AddUpdateEvent(UpdateType updateType, Action updateAction)
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

        public void RemoveUpdateEvent(UpdateType updateType, Action updateAction)
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