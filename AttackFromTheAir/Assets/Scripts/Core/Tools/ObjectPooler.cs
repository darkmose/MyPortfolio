using Core.UI;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Tools
{

    public class ObjectPooler<TPool> : IDisposable where TPool : MonoBehaviour
    {
        private Dictionary<string, Queue<TPool>> _poolDictionary = new Dictionary<string, Queue<TPool>>();
        public Transform rootOfPooledGameobjects;
        public Transform rootOfTakenGameobjects;

        public ObjectPooler(Transform rootOfObjects, Transform rootOfTakenObjects)
        {
            rootOfPooledGameobjects = rootOfObjects;
            rootOfTakenGameobjects = rootOfTakenObjects;
        }

        public void ReturnObjects()
        {
            foreach (var pool in _poolDictionary)
            {
                foreach (var obj in pool.Value)
                {
                    obj.gameObject.SetActive(false);
                    obj.transform.SetParent(rootOfPooledGameobjects);
                }
            }
        }

        public bool IsInPool(TPool obj)
        {
            return obj.transform.IsChildOf(rootOfPooledGameobjects);
        }

        public void ReturnObject(TPool obj)
        {
            obj.gameObject.SetActive(false);
            obj.transform.SetParent(rootOfPooledGameobjects);
        }

        public TPool GetPooledGameObject(string tag, bool isActive = true)
        {
            if (_poolDictionary.ContainsKey(tag))
            {
                Queue<TPool> pool = _poolDictionary[tag];
                TPool obj = pool.Dequeue();
                obj.gameObject.SetActive(isActive);
                _poolDictionary[tag].Enqueue(obj);
                obj.transform.SetParent(rootOfTakenGameobjects);
                return obj;
            }
            else
            {
                throw new System.ArgumentException($"[Pooler] Pool with tag {tag} doesn't exist");
            }

        }

        public void CreatePool(string tagName, TPool prefab, int count, Action<TPool> afterCreateAction = null)
        {
            if (_poolDictionary.ContainsKey(tagName))
            {
                return;
            }
            Queue<TPool> objects = new Queue<TPool>();

            for (int i = 0; i < count; i++)
            {
                TPool obj = GameObject.Instantiate(prefab, rootOfPooledGameobjects, false);
                afterCreateAction?.Invoke(obj);
                obj.gameObject.SetActive(false);
                objects.Enqueue(obj);
            }
            _poolDictionary.Add(tagName, objects);
        }

        public void Dispose(Action<TPool> beforeDestroyAction = null)
        {
            foreach (var pool in _poolDictionary.Values)
            {
                foreach (var item in pool)
                {
                    beforeDestroyAction?.Invoke(item);
                    GameObject.Destroy(item.gameObject);
                }
            }
            _poolDictionary.Clear();
        }

        public void DisposePool(string poolName)
        {
            if (_poolDictionary.TryGetValue(poolName, out var queue))
            {
                foreach (var item in queue)
                {
                    GameObject.Destroy(item.gameObject);
                }
                queue.Clear();
                _poolDictionary.Remove(poolName);
            }
        }

        public void Dispose()
        {
            Dispose(null);
        }
    }
}