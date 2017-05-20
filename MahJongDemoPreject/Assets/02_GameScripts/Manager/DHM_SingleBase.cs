using UnityEngine;
using System.Collections;

public class DHM_SingleBase<T> :MonoBehaviour where T : MonoBehaviour{
    private static T _instance;
    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject obj = new GameObject(typeof(T).ToString());
                _instance = obj.AddComponent<T>();
            }
            return _instance;
        }
    }
    void Awake()
    {
        if (_instance == null)
            _instance = GetComponent<T>();
        else
        {
            DestroyImmediate(this);
        }
        OnAwake();
    }
    protected virtual void OnAwake()
    {

    }
	
}
