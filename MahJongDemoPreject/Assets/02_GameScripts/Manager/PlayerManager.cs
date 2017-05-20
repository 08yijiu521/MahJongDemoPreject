using UnityEngine;
using System.Collections;

public class PlayerManager : MonoBehaviour {
    [Header("东家管理")]
    public DHM_CardManager m_EastPlayer = null;
    [Header("南家管理")]
    public DHM_CardManager m_SouthPlayer = null;
    [Header("西家管理")]
    public DHM_CardManager m_WestPlayer = null;
    [Header("北家管理")]
    public DHM_CardManager m_NorthPlayer = null;
    public static PlayerManager m_instance = null;
    private void Awake()
    {
        m_instance = this;
    }
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
