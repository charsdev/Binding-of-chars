using System.Collections.Generic;

public class Blackboard
{
    private Dictionary<string, object> _data = new Dictionary<string, object>();

    public T Get<T>(string key, T defaultValue = default(T)) 
    { 
        if (_data.ContainsKey(key))
        {
            return (T)_data[key];
        }

        return defaultValue;
    }
    
    public void Set(string key, object value)
    {
        if (_data.ContainsKey(key))
        {
            _data[key] = value;
        }
        else
        {
            _data.Add(key, value);
        }
    }
}
