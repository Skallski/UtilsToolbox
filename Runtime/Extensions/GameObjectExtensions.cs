﻿using System.Collections.Generic;
using UnityEngine;

namespace SkalluUtils.Extensions
{
    public static class GameObjectExtensions
    {
        /// <summary>
        /// Destroys all components of gameObject except the one provided as method parameter
        /// </summary>
        /// <param name="gameObject"> gameObject on which the method will be called </param>
        /// <param name="componentToKeep"> component to not destroy </param>
        public static void DestroyComponentsExceptProvided(this GameObject gameObject, Component componentToKeep)
        {
            Component[] allComponents = gameObject.GetComponents(typeof(Component));

            foreach (Component component in allComponents)
            {
                if (component != null)
                {
                    // Transform component cannot be destroyed
                    if (component.GetType() != componentToKeep.GetType() && component.GetType() != typeof(Transform)) 
                    {
                        Object.Destroy(component);
                    }
                }
            }
        }
        
        /// <summary>
        /// Destroys all components of gameObject except the list of those provided as method parameter
        /// </summary>
        /// <param name="gameObject"> gameObject on which the method will be called </param>
        /// <param name="componentsToKeep"> list of components to not destroy </param>
        public static void DestroyComponentsExceptProvided(this GameObject gameObject, IList<Component> componentsToKeep)
        {
            Component[] allComponents = gameObject.GetComponents(typeof(Component));

            foreach (Component component in allComponents)
            {
                if (component != null && !componentsToKeep.Contains(component))
                {
                    // Transform component cannot be destroyed
                    if (component.GetType() != typeof(Transform)) 
                    {
                        Object.Destroy(component);
                    }
                }
            }
        }

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
        /// Destroys all components of gameObject
        /// </summary>
        /// <param name="gameObject"> gameObject on which the method will be called </param>
        public static void DestroyAllComponents(this GameObject gameObject)
        {
            Component[] allComponents = gameObject.GetComponents(typeof(Component));

            foreach (Component component in allComponents)
            {
                if (component != null)
                {
                    // Transform component cannot be destroyed
                    if (component.GetType() != typeof(Transform)) 
                    {
                        Object.Destroy(component);
                    }
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
    }
}