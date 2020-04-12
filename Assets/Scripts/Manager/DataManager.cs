using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Data
{

}

public class Data<T> : Data 
{
    public T value1;
}

public class Data<T,T1> : Data
{
    public T value1;
    public T1 value2;
}

public class Data<T,T1,T2> : Data
{
    public T value1;
    public T1 value2;
    public T2 value3;
}

public class DataManager : Singleton<DataManager>
{
    Dictionary<string, Data> datas = new Dictionary<string, Data>();

    public void SaveData(string key, Data value)
    {
        datas[key] = value;
    }

    public Data GetData(string key)
    {
        if (datas.ContainsKey(key))
        {
            return datas[key];
        }

        return null;
    }

    public bool IsContainsData(string key)
    {
        return datas.ContainsKey(key);
    }
}
