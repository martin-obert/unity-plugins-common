using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Obert.Common.Runtime.Extensions
{
    public static class GameObjectExtensions
    {
        public static void ClearChildGameObjects(this Transform source, Action<Transform> onDestroy = null)
        {
            if(!source) return;

            var childCount = source.childCount;

            for (int i = childCount - 1; i >= 0; i--)
            {
                var child = source.GetChild(i);
                if(!child) continue;
                onDestroy?.Invoke(child);
                if(!child) continue;
                Object.Destroy(child.gameObject);
            }
        }
        
        public static void ClearChildGameObjectsImmediately(this Transform source, Action<Transform> onDestroy = null)
        {
            if(!source) return;

            var childCount = source.childCount;

            for (int i = childCount - 1; i >= 0; i--)
            {
                var child = source.GetChild(i);
                if(!child) continue;
                onDestroy?.Invoke(child);
                if(!child) continue;
                Object.DestroyImmediate(child.gameObject);
            }
        }
        
        public static IEnumerable<T> GetInterfacesOfType<T>(this Component component)
        {
            if (!component) throw new ArgumentNullException(nameof(component));

            return component.GetComponents<MonoBehaviour>().OfType<T>();
        }

        public static IEnumerable<T> GetChildrenInterfacesOfType<T>(this Component component)
        {
            if (!component) throw new ArgumentNullException(nameof(component));

            return component.GetComponentsInChildren<MonoBehaviour>().OfType<T>();
        }

        public static IEnumerable<T> GetInterfacesOfType<T>(this GameObject gameObject)
        {
            if (!gameObject) throw new ArgumentNullException(nameof(gameObject));

            return gameObject.GetComponents<MonoBehaviour>().OfType<T>();
        }

        public static Transform[] GetChildren(this GameObject gameObject)
        {
            if (!gameObject) throw new ArgumentNullException(nameof(gameObject));

            var transform = gameObject.transform;

            if (!transform) throw new ArgumentNullException(nameof(transform));

            var result = new Transform[transform.childCount];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = transform.GetChild(i);
            }

            return result;
        }

        public static Transform[] GetChildren(this Transform transform)
        {
            if (!transform) throw new ArgumentNullException(nameof(transform));

            var result = new Transform[transform.childCount];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = transform.GetChild(i);
            }

            return result;
        }
    }
}