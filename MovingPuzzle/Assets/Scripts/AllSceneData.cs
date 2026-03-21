using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// シーンを越えても消えないクラス.
/// </summary>
public class AllSceneData : MonoBehaviour
{
    public static AllSceneData instance;

    //完成した段ボール箱.
    Queue<CardboardBox> boxs = new Queue<CardboardBox>();
    //最終ポイント.
    int resultPoint;

    //get, set.
    public int ResultPoint { 
        get => resultPoint; set => resultPoint = value;
    }

    /// <summary>
    /// 初期化処理.
    /// </summary>
    void Awake()
    {
        //1度のみ実行.
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this); //シーン遷移で消滅しないようにする.
        }
        else
        {
            Destroy(this);
        }
    }

    /// <summary>
    /// 完成した段ボール箱を追加.
    /// </summary>
    public void AddCardboardBox(CardboardBox box)
    {
        boxs.Enqueue(box); //Queueに追加.
    }

    /// <summary>
    /// 完成した段ボール箱を受け取る.
    /// </summary>
    public Queue<CardboardBox> GetCardboardBoxs()
    {
        return boxs;
    }
}
