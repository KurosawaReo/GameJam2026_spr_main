/*
   - LuggageSettingsList -
   荷物の設定リスト.
*/
using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Createメニューから作成できるリストの定義.
/// </summary>
[CreateAssetMenu(
    fileName = "New List",                  //デフォルトファイル名.
    menuName = "MyGame/LuggageSettingsList" //Createメニューのパス.
)]
public class LuggageSettingsList : ScriptableObject
{
    [SerializeField] List<Luggage> list; //荷物リスト.

    /// <summary>
    /// 荷物データを取得.
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public Luggage GetLuggage(LuggageType type)
    {
        foreach (var item in list)
        {
            //一致する荷物を見つけたら荷物データを返す.
            if (item.type == type) { return item; }
        }
        return null; //エラー.
    }

    /// <summary>
    /// 種類からpoint取得.
    /// </summary>
    public int GetPoint(LuggageType type)
    {
        foreach (var item in list)
        {
            //一致する荷物を見つけたらpointを返す.
            if (item.type == type) { return item.point; }
        }
        return 0;
    }
}
