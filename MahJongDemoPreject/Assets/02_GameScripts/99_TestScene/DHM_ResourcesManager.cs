using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
//public enum ResourceType
//{
//    Table,
//    none
//}
public class DHM_ResourcesManager : MonoBehaviour {

    private static DHM_ResourcesManager _instance;
    public static DHM_ResourcesManager Instance()
    {
        if (_instance == null)
        {
            GameObject obj = new GameObject(typeof(DHM_ResourcesManager).ToString());
            _instance = obj.AddComponent<DHM_ResourcesManager>();
        }
        return _instance;
    }
    private string basePath = "Prefab/";
    private Dictionary<string, GameObject> _prefabCach = new Dictionary<string, GameObject>();
    void Awake()
    {
        if (_instance == null)
            _instance = GetComponent<DHM_ResourcesManager>();
        else
        {
            DestroyImmediate(this);
        }
    }
   /// <summary>
   /// 根据资源名字和类型获取游戏对象
   /// </summary>
   /// <param name="name"></param>
   /// <param name="type"></param>
   /// <returns></returns>
    public GameObject GetGameObject(string name, ResourceType type = ResourceType.Table)
    {
        string path = GetPath(name, type);
        Debug.Log("错误路径：" + path);
        return (GameObject)Instantiate(GetPrefab(path));
    }
    /// <summary>
    /// 获取路径
    /// </summary>
    /// <param name="name"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    string GetPath(string name,ResourceType type = ResourceType.Table)
    {
        string path = string.Empty;
        switch (type)
        {
            case ResourceType.Table:
                {
                    path = "Prefab/" + "MAJIANGTABLE" + "/" + name;
                    break;
                }
            case ResourceType.none:
                {
                    //
                    break;
                }
            case ResourceType.Hand:
                {
                    //
                    path = "Prefab/" + "HAND" + "/" + name;
                    break;
                }
        }
        return path;
    }
    /// <summary>
    /// 获取预设体
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public GameObject GetPrefab(string path)
    {
        if(!_prefabCach.ContainsKey(path))
        {
            GameObject prefab = Resources.Load(path) as GameObject;
            _prefabCach.Add(path, prefab);
        }
        return _prefabCach[path];
    }
}
