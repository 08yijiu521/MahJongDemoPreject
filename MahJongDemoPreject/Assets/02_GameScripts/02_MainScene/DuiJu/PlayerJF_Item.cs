using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class PlayerJF_Item : MonoBehaviour {

    
    [Header("对局流水分数结果界面")]
    public UILabel m_Player1Name;
    public UILabel m_Player1Score;
    public UILabel m_Player2Name;
    public UILabel m_Player2Score;
    public UILabel m_Player3Name;
    public UILabel m_Player3Score;
    public UILabel m_Player4Name;
    public UILabel m_Player4Score;
    
    public UILabel m_Count;
     

    public void SetScore(DuiJuContainerWindowCtr.PlayerJFScoreItem JFScoreItem)
    {
        m_Player1Name.text = JFScoreItem.m_PlayerItemList[0].m_name;
        m_Player1Score.text = JFScoreItem.m_PlayerItemList[0].m_score.ToString();
        m_Player2Name.text = JFScoreItem.m_PlayerItemList[1].m_name;
        m_Player2Score.text = JFScoreItem.m_PlayerItemList[1].m_score.ToString();
        m_Player3Name.text = JFScoreItem.m_PlayerItemList[2].m_name;
        m_Player3Score.text = JFScoreItem.m_PlayerItemList[2].m_score.ToString();
        m_Player4Name.text = JFScoreItem.m_PlayerItemList[3].m_name;
        m_Player4Score.text = JFScoreItem.m_PlayerItemList[3].m_score.ToString();
        m_Count.text = "第" + JFScoreItem.m_Count.ToString() + "局";
    }
    public void SetTotalScore(DuiJuContainerWindowCtr.PlayerJFScoreItem JFScoreItem)
    {
        m_Player1Name.text = JFScoreItem.m_PlayerItemList[0].m_name;
        m_Player1Score.text = JFScoreItem.m_PlayerItemList[0].m_score.ToString();
        m_Player2Name.text = JFScoreItem.m_PlayerItemList[1].m_name;
        m_Player2Score.text = JFScoreItem.m_PlayerItemList[1].m_score.ToString();
        m_Player3Name.text = JFScoreItem.m_PlayerItemList[2].m_name;
        m_Player3Score.text = JFScoreItem.m_PlayerItemList[2].m_score.ToString();
        m_Player4Name.text = JFScoreItem.m_PlayerItemList[3].m_name;
        m_Player4Score.text = JFScoreItem.m_PlayerItemList[3].m_score.ToString();
    }
}
