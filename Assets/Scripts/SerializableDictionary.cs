using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SerializableDictionary<TKey, TValue>
{
    [SerializeField]
    private List<TKey> keys = new List<TKey>();

    [SerializeField]
    private List<TValue> values = new List<TValue>();

    public void Add(TKey key, TValue value)
    {
        keys.Add(key);
        values.Add(value);
    }

    public bool TryGetValue(TKey key, out TValue value)
    {
        int index = keys.IndexOf(key);
        if (index != -1)
        {
            value = values[index];
            return true;
        }

        value = default(TValue);
        return false;
    }
}