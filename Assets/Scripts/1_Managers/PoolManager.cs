using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PoolManager
{
    class Pool
    {
        public GameObject Origin { get; private set; }
        public Transform Root { get; private set; }

        Queue<Poolable> poolQueue = new Queue<Poolable>();
        public void Init(GameObject origin, int count = 5)
        {
            Origin = origin;
            Root = new GameObject().transform;
            Root.name = $"{origin.name}Root";
            for (int i = 0; i < count; i++)
                Push(Create());
        }
        Poolable Create()
        {
            GameObject go = Object.Instantiate(Origin);
            go.name = Origin.name;
            return go.GetOrAddComponent<Poolable>();
        }
        public void Push(Poolable poolable)
        {
            if (poolable == null)
                return;
            poolable.transform.SetParent(Root);
            poolable.gameObject.SetActive(false);
            poolable.IsUsing = false;

            poolQueue.Enqueue(poolable);
        }
        public Poolable Pop(Transform parent)
        {
            Poolable poolable;
            if (poolQueue.Count > 0)
                poolable = poolQueue.Dequeue();
            else
                poolable = Create();
            poolable.gameObject.SetActive(true);

            poolable.transform.SetParent(parent);
            poolable.IsUsing = true;
            return poolable;
        }
    }

    Dictionary<string,Pool> pool = new Dictionary<string, Pool> ();
    Transform root;

    public void Init()
    {
        if(root == null)
        {
            root = new GameObject { name = "@PoolRoot" }.transform;
            Object.DontDestroyOnLoad(root);
        }
    }
    public void CreatePool(GameObject origin, int count = 5)
    {
        Pool p = new Pool();
        p.Init(origin, count);
        p.Root.parent = root;
        pool.Add(origin.name, p);
    }
    public Transform GetPoolParent(GameObject origin)
    {
        return pool[origin.name].Root;
    }
    public void Push(GameObject obj)
    {
        string name = obj.name;
        if (pool.ContainsKey(name) && obj.TryGetComponent<Poolable>(out Poolable poolable))
        {
            poolable.gameObject.SetActive(false);
            pool[name].Push(poolable);
            return;
        }
        else
            return;
    }
    public Poolable Pop(GameObject origin, Transform parent) 
    {
        if (!pool.ContainsKey(origin.name))
            CreatePool(origin);
        return pool[origin.name].Pop(parent);
    }

    public GameObject GetOriginal(string name)
    {
        if (!pool.ContainsKey(name))
            return null;
        return pool[name].Origin;
    }
    public void Clear()
    {
        foreach(Transform child in root)
            GameObject.Destroy(child.gameObject);
        pool.Clear();
    }
}
