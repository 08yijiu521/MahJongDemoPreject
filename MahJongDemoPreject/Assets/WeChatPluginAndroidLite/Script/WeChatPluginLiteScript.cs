using UnityEngine;
using System.Collections;

public class WeChatPluginLiteScript : MonoBehaviour
{
    private const string Version = "1.0.0";

    public string m_ShareText;
    public Texture2D[] m_Textures;
    public bool m_isMoments;

    private static AndroidJavaClass pluginClass;

    void Awake()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            pluginClass = new AndroidJavaClass("com.yym.wechatpluginandroidlite.WeChatPlugin");
        }
    }

    public void Prepare()
    {
        m_ShareText = "";
        m_Textures = null;
    }

    public void Share()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            byte[][] sendTexture = new byte[5][];

            for(int i = 0; i < sendTexture.Length; i++)
            {
                //sendTexture[i] = new Texture2D(1,1);
                if (m_Textures != null && m_Textures.Length > i)
                    sendTexture[i] = m_Textures[i].EncodeToPNG();
                else
                    sendTexture[i] = null;
            }

            pluginClass.CallStatic("_weChatLiteShare", m_ShareText, m_isMoments,
                sendTexture[0], sendTexture[1],
                sendTexture[2], sendTexture[3],
                sendTexture[4]);
        }
    }

}