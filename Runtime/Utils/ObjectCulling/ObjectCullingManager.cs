using System.Collections.Generic;
using UnityEngine;

namespace SkalluUtils.Utils.ObjectCulling
{
    internal class ObjectCullingManager : MonoBehaviour
    {
        private static List<ObjectCuller> Cullers = new List<ObjectCuller>();

        [SerializeField, Range(0.001f, 1f)] private float _visibilityCheckRate = 0.1f;

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

        private void Start()
        {
            InvokeRepeating(nameof(CheckForVisibility), 0, _visibilityCheckRate);
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