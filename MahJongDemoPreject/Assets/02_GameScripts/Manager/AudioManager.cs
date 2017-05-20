using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class AudioManager : DHM_SingleBase<AudioManager> {
    Dictionary<string, GameObject> m_AudioCacheDic = new Dictionary<string, GameObject>();

	
    public Dictionary<string, AudioClip> audioClipCach = new Dictionary<string, AudioClip>();
    public class AudioGameobjectItem
    {
        public string _path;
        public GameObject _object;
        public int _handle = -1;//值为-1时为空闲状态
        public AudioSource _audioSource;
        public float _audioLenth;
    }
    protected int handle = 400;//句柄
    public List<AudioGameobjectItem> _audioList = new List<AudioGameobjectItem>();

    [SerializeField]
    private GameObject m_AudioPrefab = null;
    [SerializeField]
    private AudioGameobjectItem m_bgm = null;

    protected override void OnAwake()
    {
        base.OnAwake();
         m_AudioPrefab = Resources.Load("Prefab/Audio/AudioPrefab") as GameObject;
    }

    public void PlayEffectAudio(string name)
    {
        string path = "Audios/" + name;
        PlayAudio(path, Vector3.zero);
    }
    public void PlayBackgrounfAudio(string name)
    {
        string path = "Audios/"+name;
        //name += "Audios/";
        if (m_bgm == null)
            m_bgm = GetGameObjectOfPath(path, Vector3.zero);

        AudioClip clip = GetAudioClip(path);
        m_bgm._audioSource.clip = clip;
        m_bgm._audioLenth = clip.length;
        m_bgm._path = path;
        StartCoroutine(PlayAudioLogic(m_bgm, true));
    }
    /// <summary>
    /// 播放万条筒的麻将音效
    /// </summary>
    /// <param name="id"></param>
    public void PlayHandCardAudio(int id)
    {
        string path = string.Empty;
        if (id < 10 && id > 0)
        {
             path = "Audios/" + id % 10 + "wan";
        }
        else if (id < 20 && id > 10)
        {
             path = "Audios/" + id % 10 + "tiao";
        }
        else if (id < 30 && id > 20)
        {
             path = "Audios/" + id % 10 + "tong";
        }
        if (path != null || !path.Equals(string.Empty))
            PlayAudio(path, Vector3.zero);
    }
    public void PlayAudio(string path, Vector3 pos)
    {
        AudioGameobjectItem item = GetGameObjectOfPath(path, pos);
        StartCoroutine(PlayAudioLogic(item));
    }
    public AudioClip GetAudioClip(string name)
    {

        if (!audioClipCach.ContainsKey(name))
        {
            AudioClip clip = Resources.Load(name) as AudioClip;
            audioClipCach.Add(name, clip);
        }
        return audioClipCach[name];
    }

    public void Dispose()
    {
        audioClipCach.Clear();
    }
    IEnumerator PlayAudioLogic(AudioGameobjectItem item,bool isBGM = false)
    {
        if(isBGM)
        {
            item._audioSource.playOnAwake = false;
            item._audioSource.loop = true;
        }
        else
        {
            item._audioSource.playOnAwake = false;
            item._audioSource.loop = false;
        }
        item._audioSource.Play();
        yield return new WaitForSeconds(item._audioLenth);
        //关闭
        if (!isBGM)
            yield return StartCoroutine(Recycle(item._handle));
    }

    IEnumerator Recycle(int handle)
    {
        RemoveGameObject(handle);
        yield break;
    }
    public AudioGameobjectItem GetGameObjectOfPath(string path, Vector3 position, Space space = Space.World)
    {
        AudioGameobjectItem audioItem = CreateGameObject(path);
        audioItem._object.SetActive(false);
        if (space == Space.Self)
        {
            audioItem._object.transform.localPosition = position;
        }
        else if (space == Space.World)
        {
            audioItem._object.transform.position = position;
        }
        audioItem._object.SetActive(true);
        return audioItem;
    }

    /// <summary>
    /// 根据句柄来销毁游戏对象
    /// </summary>
    /// <param name="handle">句柄号</param>
    /// <param name="isDestroy">是否彻底销毁，false是隐藏不销毁</param>
    public void RemoveGameObject(int handle, bool isDestroy = false)
    {
        AudioGameobjectItem resItem = FindGameObjectOfHandleInList(handle);
        if (resItem == null)
        {
#if UNITY_EDITOR
            Debug.Log("GameObject is not exist:你想要删除的 handle" + handle);
#endif
            return;
        }
        if (isDestroy)
        {
            Destroy(resItem._object);
            _audioList.Remove(resItem);
        }
        else
        {
            resItem._object.transform.SetParent(transform);
            resItem._object.SetActive(false);
            resItem._handle = -1;
        }
    }

    protected AudioGameobjectItem FindGameObjectOfHandleInList(int handle)
    {
        for (int i = 0; i < _audioList.Count; i++)
        {
            if (_audioList[i]._handle == handle)
            {
                return _audioList[i];
            }
        }
        return null;
    }

    /// <summary>
    /// 返回一个对应路径的游戏对象
    /// </summary>
    protected AudioGameobjectItem CreateGameObject(string path)
    {
        AudioGameobjectItem audioItem = FindObjectOfPathInList(path);
        if (audioItem == null)
        {
            GameObject gameobject = Instantiate(m_AudioPrefab);
            AudioClip clip = GetAudioClip(path);
            audioItem = new AudioGameobjectItem();
            audioItem._handle = GetHandle();
            audioItem._object = gameobject;
            audioItem._path = path;
            audioItem._audioSource = gameobject.GetComponent<AudioSource>();
            audioItem._audioLenth = clip.length;
            audioItem._audioSource.clip = clip;
            _audioList.Add(audioItem);
        }
        audioItem._object.SetActive(true);
        audioItem._object.transform.SetParent(null);
        return audioItem;
    }

    /// <summary>
    /// 在当前缓冲池中查找是否有空闲的对象
    /// </summary>
    /// <param name="path">路径标识</param>
    /// <returns>返回GameObject</returns>
    protected AudioGameobjectItem FindObjectOfPathInList(string path)
    {
        for (int i = 0; i < _audioList.Count; i++)
        {
            if (_audioList[i]._path.Equals(path) && _audioList[i]._handle == -1)
            {
                _audioList[i]._handle = GetHandle();
                return _audioList[i];
            }
        }
        return null;
    }

    /// <summary>
    /// 获取handle标识元素
    /// </summary>
    /// <returns>返回一个唯一的handle</returns>
    protected int GetHandle()
    {
        return ++handle;
    }
}