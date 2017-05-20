/************************
 * Title：
 * Function：
 * 	－ 显示剩余牌数
 * Used By：PaiduoManager
 * Author：qwt
 * Date：2017.3.21
  Version：1.0
 * Description：
 *
************************/
using UnityEngine;
using System.Collections;

public class ShowRemainCard : MonoBehaviour {

    public static ShowRemainCard instance;

    public Texture2D[] remainCardCountBlueTex;
    public Texture2D[] remainCardCountRedTex;

    private GameObject remainCardCount1;
    private GameObject remainCardCount2;

    private Renderer remainCardCount1Render;
    private Renderer remainCardCount2Render;

    public bool isShowCardCount = false;//是否开始显示牌数
    private bool isStart = true;

    private float timer = 0;
    private int deleteTimes = 0;

    private int CardSummary = 108;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        remainCardCount1 = this.transform.GetChild(0).gameObject;
        remainCardCount2 = this.transform.GetChild(1).gameObject;
        remainCardCount1Render = remainCardCount1.GetComponent<Renderer>();
        remainCardCount2Render = remainCardCount2.GetComponent<Renderer>();
    }

    void Update()
    {
        if (isShowCardCount)
        {
            //每隔0.5s显示一次剩余牌数，总共13次
            if (deleteTimes < 13)
            {
                if (isStart)
                {
                    GetRemainCardCount();
                    deleteTimes++;
                    isStart = false;
                }
                timer += Time.deltaTime;
                if (timer >= 0.2f)
                {
                    int count = GetRemainCardCount();
                    ShowCardCount(count);
                    timer = 0;
                    deleteTimes++;
                }
            }
            else
            {
                isShowCardCount = false;
            }
        }
    }

    /// <summary>
    /// 显示剩余牌数
    /// </summary>
    /// <param name="count"></param>
    public void ShowCardCount(int count)
    {
        int a = count / 10;
        int b = count % 10;
        if (count<=96)
        {
            remainCardCount1.SetActive(true);
            remainCardCount2.SetActive(true);
            if (count>20)
            {
                remainCardCount1Render.material.mainTexture = remainCardCountBlueTex[a];
                remainCardCount2Render.material.mainTexture = remainCardCountBlueTex[b];
            }else
            {
                remainCardCount1Render.material.mainTexture = remainCardCountRedTex[a];
                remainCardCount2Render.material.mainTexture = remainCardCountRedTex[b];
            }
        }
    }

    /// <summary>
    /// 从牌垛删52张牌时剩余牌数
    /// </summary>
    /// <returns></returns>
    public int GetRemainCardCount()
    {
        CardSummary -= 4;
        return CardSummary;
    }

    /// <summary>
    /// 每次从牌垛删1张牌显示剩余牌数
    /// </summary>
    public void ShowCardCountAfterDelete()
    {
        if (CardSummary>0)
        {
            CardSummary -= 1;
            ShowCardCount(CardSummary);
        }
    }

    /// <summary>
    /// 重置显示牌数
    /// </summary>
    public void ResetCardCount()
    {
        CardSummary = 108;
    }

    /// <summary>
    /// 点击下一局后将显示牌数隐藏
    /// </summary>
    public void HideCardCount()
    {
        remainCardCount1.SetActive(false);
        remainCardCount2.SetActive(false);
    }
}
