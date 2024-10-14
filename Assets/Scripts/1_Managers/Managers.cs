using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Resources;
using UnityEngine;
using UnityEngine.InputSystem;

public class Managers : MonoBehaviour
{
    static Managers instance = null;
    static Managers Instance { get { Init(); return instance; } }

    DataManager data = new DataManager();
    GameManager game = new GameManager();
    PoolManager pool = new PoolManager();
    ResourceManager resource = new ResourceManager();
    UIManager ui = new UIManager();
    LoadSceneManager scene = new LoadSceneManager();
    SoundManager sound = new SoundManager();
    CameraManager camera = new CameraManager();
    ReadSpreadSheet sheet = new ReadSpreadSheet();
    DirectionManager direction = new DirectionManager();

    public static GameObject SoundRoot { get; private set; }

    public static DataManager Data { get { return Instance?.data; } }
    public static GameManager Game { get { return Instance?.game; } }
    public static PoolManager Pool { get { return Instance?.pool; } }
    public static ResourceManager Resource { get { return Instance?.resource; } }
    public static UIManager UI { get { return Instance?.ui; } }
    public static LoadSceneManager Scene { get { return Instance?.scene; } }
    public static SoundManager Sound { get { return Instance?.sound; } set { Instance.sound = value; } }
    public static CameraManager Camera { get { return Instance?.camera; } }
    public static ReadSpreadSheet Sheet { get { return Instance?.sheet; } }
    public static DirectionManager Direction { get { return Instance?.direction; } }

    static void Init()
    {
        if (instance == null)
        {
            GameObject go = GameObject.Find("@Managers");
            if (go == null)
            {
                go = new GameObject { name = "@Managers" };
                go.AddComponent<Managers>();
            }
            DontDestroyOnLoad(go);
            instance = go.GetComponent<Managers>();

//            instance.data.Init();
            instance.game.Init();
            instance.pool.Init();
            instance.sound.Init();
            instance.sheet.Init();
            instance.direction.Init();
            PlayerPrefs.DeleteKey("Stage1");
        }
    }
    public static void Clear()
    {
        Scene.Clear();
        UI.Clear();
        Pool.Clear();
    }
}
