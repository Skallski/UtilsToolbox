﻿using System.Collections.Generic;
using UnityEngine;

namespace SkalluUtils.Extensions
{
    namespace GameObjectExtensions
    {
        public static class GameObjectExtensions
        {
            /// <summary>
            /// Destroys all components of game object except the one provided as method parameter
            /// </summary>
            /// <param name="gameObject"> gameObject on which the method will be called </param>
            /// <param name="componentToKeep"> component to not destroy </param>
            public static void DestroyComponentsExceptProvided(this GameObject gameObject, Component componentToKeep)
            {
                var allComponents = gameObject.GetComponents(typeof(Component));

                foreach (var component in allComponents)
                {
                    if (component != null)
                    {
                        if (component.GetType() != componentToKeep.GetType() && component.GetType() != typeof(Transform)) // Transform component cannot be destroyed
                            Object.Destroy(component);
                    }
                }
            }
            
            /// <summary>
            /// Destroys all components of game object except the list of those provided as method parameter
            /// </summary>
            /// <param name="gameObject"> gameObject on which the method will be called </param>
            /// <param name="componentsToKeep"> list of components to not destroy </param>
            public static void DestroyComponentsExceptProvided(this GameObject gameObject, List<Component> componentsToKeep)
            {
                var allComponents = gameObject.GetComponents(typeof(Component));

                foreach (var component in allComponents)
                {
                    if (component != null && !componentsToKeep.Contains(component))
                    {
                        if (component.GetType() != typeof(Transform)) // Transform component cannot be destroyed
                            Object.Destroy(component);
                    }
                }
            }
            
            /// <summary>
            /// Destroys all components of game object
            /// </summary>
            /// <param name="gameObject"> gameObject on which the method will be called </param>
            public static void DestroyAllComponents(this GameObject gameObject)
            {
                var allComponents = gameObject.GetComponents(typeof(Component));
        
                foreach (var component in allComponents)
                {
                    if (component != null)
                    {
                        if (component.GetType() != typeof(Transform)) // Transform component cannot be destroyed
                            Object.Destroy(component);
                    }
                }
            }
        }
    }
}