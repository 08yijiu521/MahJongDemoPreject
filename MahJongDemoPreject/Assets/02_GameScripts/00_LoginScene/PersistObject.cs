/************************************************************************
*	@brief		：确保一个Object在切换场景的时候不会被析构. 主要用于全局的一些对象.
*	                ※注意※:当重复进入一个场景,以前场景中使用此脚本的对象可能会出现多个.
 *	                        此脚本尽量只使用在个别全局的脚本上面,并确保只有一个(游戏打开的时候进入一个空场景,以后不再进入此场景)
************************************************************************/

using UnityEngine;
using System.Collections;

public class PersistObject : MonoBehaviour
{
    public bool isStaticObject = true;
    void Awake()
    {
        gameObject.isStatic = isStaticObject;
        DontDestroyOnLoad(gameObject);
    }
}
