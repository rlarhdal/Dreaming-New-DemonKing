using System.Collections.Generic;
using UnityEngine;

public class ResourceManager
{

    public Dictionary<string,Sprite> _sprites = new Dictionary<string,Sprite>();

    public T Load<T>(string path) where T : Object
    {
        if(typeof(T) == typeof(Sprite))
        {
            if (_sprites.TryGetValue(path, out var sprite))
                return sprite as T;
            Sprite sp = Resources.Load<Sprite>(path);
            _sprites.Add(path, sp);
            return sp as T;
        }
        else if (typeof(T) == typeof(GameObject))
        {
            string name = path;
            int idx = name.LastIndexOf('/');
            if(idx >=0)
                name = name.Substring(idx+1);
            GameObject go = Managers.Pool.GetOriginal(name);
            if (go != null)
                return go as T;
        }
        return Resources.Load<T>(path);
    }

    public GameObject Instantiate(string name, Transform parent=null, string path = "Prefabs/")
    {
        GameObject original = Load<GameObject>(path+name);
        if(original == null)
        {
            Debug.Log($"Fail to load prefab {name}");
            return null;
        }

        if (original.GetComponent<Poolable>() != null)
            return Managers.Pool.Pop(original, parent).gameObject;

        GameObject go = Instantiate(original, parent);
        return go;
    }

    public GameObject Instantiate(GameObject prefab, Transform parent = null)
    {
        GameObject go = GameObject.Instantiate(prefab, parent);
        go.name = prefab.name;
        return go;
    }

    public void Destroy(GameObject gameObject)
    {
        if (gameObject == null)
            return;

        //object pooling
        Poolable poolable = gameObject.GetComponent<Poolable>();
        if(poolable != null)
        {
            Managers.Pool.Push(gameObject);
            return;
        }

        Object.Destroy(gameObject);
    }
}