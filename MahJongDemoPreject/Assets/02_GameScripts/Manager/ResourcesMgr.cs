/************************************************************************
*	@brief		：Resources管理类.
*		
*	@author		：李经纬
*	@copyright	：时光科技   2017
*	
*	@date		：2017年2月28日10:59:57
************************************************************************/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum ResourceType
{
    Table,
    DiTan,
    none,
    Hand,
    Audio
}
[System.Serializable]
public class ResourceItem
{
   public int handle = -1;
   public GameObject obj = null;
    public string path = string.Empty;
}
public class ResourcesMgr : MonoBehaviour {
    public static ResourcesMgr m_Instance;

    private Dictionary<string, GameObject> _prefabCach = new Dictionary<string, GameObject>();
    [SerializeField]
    private List<ResourceItem> cacheList = new List<ResourceItem>();
    private int handle = 100;

    [Header("透明材质")]
    private Material m_transparent = null;
    public Material M_transparent
    {
        set { m_transparent = value; }
        get { return m_transparent; }
    }
    private void Awake()
    {
        m_Instance = this;
        m_transparent = Resources.Load("Materials/Transparent") as Material;
    }
    public GameObject InstantiateGameObject(string name , ResourceType type = ResourceType.Table)
    {
        string path = GetPath(name, type);
        return (GameObject)Instantiate(GetPrefab(path));
    }
    public GameObject InstantiateGameObjectWithType(string name, ResourceType type = ResourceType.Table)
    {
        string path = GetPath(name, type);
        return GetGameObject(path);
    }
    #region  
    public GameObject GetGameObject(string path)
    {
        
        ResourceItem item = FindGameObject(path);
        Debug.Log(path + ":" +item);
        item.handle = GetHandle();
        item.obj.transform.SetParent(null);
        item.obj.SetActive(true);
        return item.obj;
    }
    ResourceItem FindGameObject(string path)
    {
        ResourceItem item = null;
        for(int i = 0;i < cacheList.Count;i++)
        {
            if(cacheList[i].path == path && cacheList[i].handle==-1)
            {
                item = cacheList[i];
            }
        }
        if(item==null)
        {
            item = CreateGameObject(path);
        }
        return item;
    }
    ResourceItem CreateGameObject(string path)
    {
        ResourceItem item = new ResourceItem();
        item.obj = Instantiate(GetPrefab(path));
        if (item.obj == null)
            Debug.LogError(path + "路径下无法找到资源");
        item.handle = -1;
        item.path = path;
        cacheList.Add(item);
        return item;
    }
    public void RemoveGameObject(int handles)
    {
        ResourceItem item = FindGameObjectToRemove(handle);
        if(item!=null)
        {
            item.handle = -1;
            item.obj.transform.SetParent(this.transform);
            item.obj.SetActive(false);
        }
    }
    public void RemoveGameObject(GameObject obj)
    {
        ResourceItem item = FindGameObjectToRemove(obj);
        if (item != null)
        {
            item.handle = -1;
            item.obj.transform.SetParent(this.transform);
            item.obj.SetActive(false);
        }
    }
    ResourceItem FindGameObjectToRemove(int handle)
    {
        ResourceItem item = null;
        for (int i = 0; i < cacheList.Count; i++)
        {
            if (cacheList[i].handle.Equals(handle))
            {
                item = cacheList[i];
            }
        }
        return item;
    }
    ResourceItem FindGameObjectToRemove(GameObject obj)
    {
        ResourceItem item = null;
        for(int i =0; i < cacheList.Count;i++)
        {
            if(cacheList[i].obj.Equals(obj) && cacheList[i].handle!=-1)
            {
                item = cacheList[i];
            }
        }
        return item;
    }
    int GetHandle()
    {
        return handle++;
    }
    #endregion
    /// <summary>
    /// 获取路径
    /// </summary>
    /// <param name="name"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    string GetPath(string name, ResourceType type = ResourceType.Table)
    {
        string path = string.Empty;
        switch (type)
        {
            case ResourceType.Table:
                {
                    path = "Prefab/" + "MAJIANGTABLE" + "/" + name;
                    break;
                }
            case ResourceType.DiTan:
                {
                    path = "Prefab/" + "DITAN" + "/" + name;
                    break;
                }
            case ResourceType.Audio:
                {
                    path = "Prefab/" + "Audio" + "/" + name;
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
        if (!_prefabCach.ContainsKey(path))
        {
            GameObject prefab = Resources.Load(path) as GameObject;
            _prefabCach.Add(path, prefab);
        }
        return _prefabCach[path];
    }
}
