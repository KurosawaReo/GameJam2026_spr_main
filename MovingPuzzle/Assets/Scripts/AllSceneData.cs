using UnityEngine;
using System.Collections.Generic;

using KR.Unity.Inspector;

/// <summary>
/// シーンを越えても消えないクラス.
/// </summary>
public class AllSceneData : MonoBehaviour
{
    public static AllSceneData instance;

    //完成した段ボール箱.
    [SerializeField, ReadOnly] 
    List<CardboardBox> boxs = new List<CardboardBox>();

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
        boxs.Add(box); //Listに追加.
    }

    /// <summary>
    /// 完成した段ボール箱を受け取る.
    /// </summary>
    public List<CardboardBox> GetCardboardBoxs()
    {
        return boxs;
    }
}
