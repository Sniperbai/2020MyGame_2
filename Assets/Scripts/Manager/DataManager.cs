using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Data
{

}

public class Data<T> : Data 
{
    public T valuy1;
}

public class Data<T,T1> : Data
{
    public T valuy1;
    public T1 valuy2;
}

public class Data<T,T1,T2> : Data
{
    public T valuy1;
    public T1 valuy2;
    public T2 valuy3;
}

public class DataManager : Singleton<DateManager>
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
}
