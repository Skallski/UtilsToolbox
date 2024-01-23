using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkalluUtils.Utils.ObjectCulling
{
    internal class ObjectCullingManager : MonoBehaviour
    {
        private static List<ObjectCuller> Cullers = new List<ObjectCuller>();

        [SerializeField, Range(0.001f, 1f)] private float _visibilityCheckRate = 0.1f;
        private float _timer;

        internal static void AddCuller(ObjectCuller objectCuller)
        {
            if (objectCuller == null)
            {
                return;
            }

            if (Cullers.Contains(objectCuller) == false)
            {
                Cullers.Add(objectCuller);
            }
        }

        internal static void RemoveCuller(ObjectCuller objectCuller)
        {
            if (objectCuller == null)
            {
                return;
            }

            if (Cullers.Contains(objectCuller))
            {
                Cullers.Remove(objectCuller);
            }
        }

        private IEnumerator Start()
        {
            while (true)
            {
                _timer += Time.deltaTime;
                if (_timer >= _visibilityCheckRate)
                {
                    CheckForVisibility();
                    
                    _timer -= _timer;
                    yield return null;
                }
            }
        }

        private void CheckForVisibility()
        {
            foreach (var culler in Cullers)
            {
                culler.CheckForVisibility();
            }
        }
    }
}