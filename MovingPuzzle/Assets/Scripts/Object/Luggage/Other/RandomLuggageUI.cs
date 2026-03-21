using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// トラックフェーズのボタン.
/// </summary>
public class RandomLuggageUI : MonoBehaviour
{
    #region ===== 変数 =====
    public LuggageButton[] buttons;

    //残りの荷物リスト.
    private List<LuggageType> remainingList = new List<LuggageType>();
    #endregion

    #region ===== 初期化 =====
    void Start()
    {
        InitList();
        SetupRandom();
    }
    #endregion

    #region ===== リスト =====
    /// <summary>
    /// リスト初期化
    /// </summary>
    void InitList()
    {
        foreach (LuggageType type in System.Enum.GetValues(typeof(LuggageType)))
        {
            remainingList.Add(type);
        }
    }
    /// <summary>
    /// 落ちた時にリストに戻す処理
    /// </summary>
    /// <param name="type">荷物の種類</param>
    public void ReturnToList(LuggageType type)
    {
        remainingList.Add(type);
    }
    #endregion

    #region ===== 初期ランダム =====
    void SetupRandom()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            SetRandomToButton(buttons[i]);
        }
    }
    #endregion

    #region ===== ボタンにランダム設定 =====
    public void SetRandomToButton(LuggageButton btn)
    {
        if (remainingList.Count == 0)
        {
            Debug.Log("もう全部使った！");
            btn.gameObject.SetActive(false);
            return;
        }

        //残りのリストからランダムで選ぶ.
        int rand = Random.Range(0, remainingList.Count);

        Luggage l = new Luggage();
        l.type = remainingList[rand];

        remainingList.RemoveAt(rand); // 使ったから消す

        btn.data = l;
        btn.UpdateIcon();
    }
    #endregion
}