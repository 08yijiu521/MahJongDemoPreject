using UnityEngine;
using System.Collections;

public class PlayerScoreDownCtr : MonoBehaviour {

    public UISprite m_PlayerIcon;
    public UILabel m_PlayerName;
    public UISprite m_Operator;
    public UISprite m_Number1;
    public UISprite m_Number2;
    public UISprite m_Number3;
    public UIGrid m_DetailScore;

    public void SetPlayerScore(string name, string icon, int score)
    {
        Debug.Log("初始化分数界面Down");
        m_PlayerName.text = name;
        m_PlayerIcon.spriteName = icon;
        InitScoreSpriteName(score);
    }

    public void InitScoreSpriteName(int score)
    {
        string spriteName = "Img";
        if (score < 0)
        {
            spriteName = spriteName + "_";
            m_Operator.spriteName =spriteName+ "-";
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
        else if (score < 100 && score >= 10)
        {
            int num1 = score / 10;
            int num2 = score % 10;
            m_Number1.spriteName = spriteName + num1;
            m_Number2.spriteName = spriteName + num2;
            m_Number3.gameObject.SetActive(false);
        }
        else if (score >= 100 && score < 1000)
        {
            int num1 = score / 100;
            int num3 = score % 100;
            int num2 = num3 / 10;
            num3 = num3 % 10;
            m_Number1.spriteName = spriteName + num1;
            m_Number2.spriteName = spriteName + num2;
            m_Number3.spriteName = spriteName + num3;
        }
    }
    public void SetDetalScore()
    {

        GameObject obj = ResourcesMgr.m_Instance.GetGameObject("Prefab/MainScene/ResultScore/ScoreItem");
        m_DetailScore.AddChild(obj.transform);
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localScale = Vector3.one;
        obj.transform.localRotation = Quaternion.identity;

        ScoreItemCtr scoreItem = obj.GetComponent<ScoreItemCtr>();
        scoreItem.m_detailOperator.text = "自摸";
        scoreItem.m_multiple.text = "1" + "倍";
        scoreItem.m_score.text = "+6";
        scoreItem.m_player.text = "上家";
        m_DetailScore.Reposition();
        obj.SetActive(false);
        obj.SetActive(true);
    }
    public void OnDisable()
    {
        ScoreItemCtr[] arrays = m_DetailScore.GetComponentsInChildren<ScoreItemCtr>();
        foreach(var item in arrays)
        {
            ResourcesMgr.m_Instance.RemoveGameObject(item.gameObject);
        }
    }

}
