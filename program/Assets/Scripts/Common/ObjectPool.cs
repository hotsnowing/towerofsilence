using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> : Singleton<ObjectPool<T>> where T:Component
{
    private T resource = null;
    private Queue<T> pool = new Queue<T>();

    public T Rent(RectTransform parent)
    {

        if (pool.Count > 0)
        {
            var item = pool.Dequeue();
            item.transform.SetParent(parent);
            item.gameObject.SetActive(true);
            return item;
        }
        
        if (resource == null)
        {
            resource = Resources.Load<T>(typeof(T).ToString());
        }

        var obj = Object.Instantiate(resource, parent);
        obj.gameObject.SetActive(true);
        return obj;
    }

    public void Return(T item)
    {
        pool.Enqueue(item);
        item.gameObject.SetActive(false);
    } 
}
