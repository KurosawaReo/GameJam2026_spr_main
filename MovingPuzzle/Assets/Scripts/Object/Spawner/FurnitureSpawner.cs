using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// トラックフェーズのスポナー.
/// </summary>
public class FurnitureSpawner : MonoBehaviour
{
    #region ===== 変数 =====
    [Header("- luggage -")]
    [SerializeField] LuggageSettingsList luggageSettingsList; //荷物設定リスト.

    [Header("- value -")]
    [SerializeField] LuggageType[] bigLuggageTypes; //大きい荷物抽選リスト.

    //今選択してる荷物prefab.
　  //(荷物を1つだけ選択できるようにする用)
    private GameObject nowLuggage;

    //残りの段ボール箱.
    private Queue<CardboardBox> remainingBoxs = new Queue<CardboardBox>();

    #endregion

    void Start()
    {
        //段ボール箱を受け取る.
        if (AllSceneData.instance != null)
        {
            remainingBoxs = AllSceneData.instance.GetCardboardBoxs();
        }
    }

    /// <summary>
    /// ボタンの次の設定.
    /// </summary>
    /// <returns>次の種類</returns>
    public (LuggageType, CardboardBox) SetupButton()
    {
        int rnd = Random.Range(0, 99+1);

        //50%の確率, かつ段ボール箱が残っていれば.
        if (rnd < 50 && remainingBoxs.Count > 0)
        {
            var type = LuggageType.Box;         //種類は段ボール箱.
            var box  = remainingBoxs.Dequeue(); //段ボール箱データを取り出す.
            return (type, box); 
        }
        else
        {
            //大きい荷物の中から抽選.
            int idx  = Random.Range(0, bigLuggageTypes.Length-1);
            var type = bigLuggageTypes[idx];
            //必要ないため空を送る.
            var box  = new CardboardBox();
            return (type, box);
        }
    }

    #region ===== 荷物生成 =====

    /// <summary>
    /// 荷物生成.
    /// </summary>
    /// <param name="luggage">荷物データ</param>
    public void Spawn(Luggage luggage, CardboardBox box)
    {
        //出現は同時に1個だけ.
        if (nowLuggage != null) return;
        //null対策.
        if (luggage == null)
        {
            Debug.LogError("Luggageがnull！");
            return;
        }

        //設置リストからprefab取得.
        GameObject prefab = luggageSettingsList.GetLuggage(luggage.type).prefab;
        if (prefab == null)
        {
            Debug.LogError("設置リストにPrefabが設定されてない！");
            return;
        }
        
        //荷物prefab生成.
        {
            GameObject obj = Instantiate(prefab);
            //script取得.
            DropLuggage script = obj.GetComponent<DropLuggage>();

            if (script != null)
            {
                script.Init(this);
                script.Type = luggage.type;

                //段ボール箱なら.
                if (luggage.type == LuggageType.Box)
                {
                    //段ボール箱にデータを渡す.
                    script.BoxData = box;
                }
            }
            else
            {
                Debug.LogError("DropLuggageがついてない！");
            }

            nowLuggage = obj; //現在の荷物として保存.
        }
    }
    #endregion


    #region ===== 落下して消えたとき =====
    public void OnBoxLost(DropLuggage box)
    {
        if (box == null) return;

        //段ボール箱なら.
        if (box.Type == LuggageType.Box)
        {
            remainingBoxs.Enqueue(box.BoxData); //残りのリストに戻す.
        }
    }
    #endregion


    #region ===== ドロップ完了 =====
    public void OnDropped()
    {
        nowLuggage = null;
    }
    #endregion


    #region ===== デバッグ表示 =====
    void OnDrawGizmos()
    {
    }
    #endregion
}