using System.Collections.Generic;
using UnityEngine;

namespace SkalluUtils.Extensions
{
    public static class GameObjectExtensions
    {
        /// <summary>
        /// Destroys provided components of gameObject
        /// </summary>
        /// <param name="gameObject"> gameObject on which the method will be called </param>
        /// <param name="componentsToDestroy"> array of components to destroy </param>
        public static void DestroyComponents(this GameObject gameObject, params Component[] componentsToDestroy)
        {
            foreach (Component component in componentsToDestroy)
            {
                if (component != null && component.GetType() != typeof(Transform))
                {
                    Object.Destroy(component);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameObject"></param>
        /// <param name="value"></param>
        /// <param name="behaviours"></param>
        public static void EnableBehaviours(this GameObject gameObject, bool value, params Behaviour[] behaviours)
        {
            foreach (Behaviour behaviour in behaviours)
            {
                if (behaviour != null && behaviour.GetType() != typeof(Transform))
                {
                    behaviour.enabled = value;
                }
            }
        }

        /// <summary>
        /// Gets all children of gameObject
        /// </summary>
        /// <param name="gameObject"> gameObject on which the method will be called </param>
        /// <returns> list of children of gameObject </returns>
        public static List<GameObject> Children(this GameObject gameObject)
        {
            List<GameObject> children = new List<GameObject>();
            Transform transform = gameObject.transform;

            for (int i = transform.childCount - 1; i >= 0; i--)
            {
                children.Add(transform.GetChild(i).gameObject);
            }

            return children;
        }

        /// <summary>
        /// Tries to find component of provided type inside gameObject's children
        /// </summary>
        /// <param name="gameObject"> gameObject on which the method will be called </param>
        /// <param name="component"> desired component to find </param>
        /// <typeparam name="T"> generic component type </typeparam>
        /// <returns> true or false, depending on whether component is found or not </returns>
        public static bool TryGetComponentInChildren<T>(this GameObject gameObject, out T component) where T : Component
        {
            component = gameObject.GetComponentInChildren<T>();
            return component != null;
        }

        /// <summary>
        /// Tries to find component of provided type inside gameObject's parent
        /// </summary>
        /// <param name="gameObject"> gameObject on which the method will be called </param>
        /// <param name="component"> desired component to find </param>
        /// <typeparam name="T"> generic component type </typeparam>
        /// <returns> true or false, depending on whether component is found or not </returns>
        public static bool TryGetComponentInParent<T>(this GameObject gameObject, out T component) where T : Component
        {
            component = gameObject.GetComponentInParent<T>();
            return component != null;
        }
        
        /// <summary>
        /// Tries to find component of provided type inside gameObject and it's children
        /// </summary>
        /// <param name="gameObject"> gameObject on which the method will be called </param>
        /// <param name="component"> desired component to find </param>
        /// <typeparam name="T"> generic component type </typeparam>
        /// <returns> true or false, depending on whether component is found or not </returns>
        public static bool TryGetComponentInLocalHierarchy<T>(this GameObject gameObject, out T component) where T : Component
        {
            component = gameObject.GetComponentInLocalHierarchy<T>();
            return component != null;
        }
        
        /// <summary>
        /// Finds component of provided type inside gameObject and it's children
        /// </summary>
        /// <param name="gameObject"> gameObject on which the method will be called </param>
        /// <typeparam name="T"> generic component type </typeparam>
        /// <returns> found component </returns>
        public static T GetComponentInLocalHierarchy<T>(this GameObject gameObject) where T : Component
        {
            T component = gameObject.GetComponent<T>();
            if (component != null)
            {
                return component;
            }

            foreach (Transform child in gameObject.transform)
            {
                component = child.gameObject.GetComponentInLocalHierarchy<T>();
                if (component != null)
                {
                    return component;
                }
            }

            return null;
        }
    }
}