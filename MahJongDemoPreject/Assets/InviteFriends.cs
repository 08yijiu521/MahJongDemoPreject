/************************
 * Title：
 * Function：
 * 	－ 
 * Used By：
 * Author：qwt
 * Date：
 * Version：1.0
 * Description：
 *
************************/
using UnityEngine;
using System.Collections;

public class InviteFriends : MonoBehaviour {

    public GameObject wechatPrefab;
    public TweenScale wechatTween;

    public void OnBtnClick()
    {
        NGUITools.AddChild(this.gameObject, wechatPrefab);
        wechatTween.PlayForward();
    }
}
