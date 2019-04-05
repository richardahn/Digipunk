using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Memory 
{
    private Dictionary<string, object> memory;

    public Memory()
    {
        memory = new Dictionary<string, object>();
    }
    public T Get<T>(string key)
    {
        if (!memory.TryGetValue(key, out object result))
        {
            Debug.LogError("Failed to retrieve key, " + key + ", from memory because it does not exist");
            return default;
        }
        return (T)result;
    }

    public void Add<T>(string key, T value)
    {
        if (memory.ContainsKey(key))
            Debug.LogWarning("Replacing value in memory, " + memory[key] + ", with value, " + value);

        memory[key] = value;
    }

    
}
