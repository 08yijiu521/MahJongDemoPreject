using UnityEngine;
using System.Collections;

public class Img_DuiJuViewCtr : MonoBehaviour {
    public UILabel m_CurrentCount_Label;//当前局数
	void Start () {
        //SetGameCount();
    }
    public void SetGameCount(int count)
    {
        m_CurrentCount_Label.text = count.ToString();
    }
    //public void SetGameCount()
    //{
    //    SetGameCount(++m_count);
    //}
}
