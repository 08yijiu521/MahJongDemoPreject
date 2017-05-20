using UnityEngine;
using System.Collections;

public class PlayerScoreCtr : MonoBehaviour {
    public UISprite m_PlayerIcon;
    public UILabel m_PlayerName;
    public UISprite m_Operator;
    public UISprite m_Number1;
    public UISprite m_Number2;
    public UISprite m_Number3;
   public void SetPlayerScore(string name,string icon,int score)
    {
        Debug.Log("初始化分数界面："+name);
        m_PlayerName.text = name;
        m_PlayerIcon.spriteName = icon;
        InitScoreSpriteName(score);
        Debug.Log("初始化分数界面完成：" + name);
    }

    public void InitScoreSpriteName(int score)
    {
        string spriteName = "Img";
        if (score < 0)
        {
            spriteName = spriteName + "_";
            m_Operator.spriteName = spriteName + "-";
        }
        else
        {
            m_Operator.spriteName = spriteName + "+";
        }
        if (score < 10)
        {
            spriteName = spriteName + score;
            m_Number2.gameObject.SetActive(false);
            m_Number3.gameObject.SetActive(false);
        }
        else if(score < 100 && score >=10)
        {
            int num1 = score / 10;
            int num2 = score % 10;
            m_Number1.spriteName = spriteName + num1;
            m_Number2.spriteName = spriteName + num2;
            m_Number3.gameObject.SetActive(false);
        }
        else if(score >=100 && score < 1000)
        {
            int num1 = score / 100;
            int num3 = score % 100;
            int num2 = num3 / 10;
            num3 = num3 % 10;
            m_Number1.spriteName = spriteName + num1;
            m_Number2.spriteName = spriteName + num2;
            m_Number3.spriteName = spriteName+num3;
        }
    }
}
