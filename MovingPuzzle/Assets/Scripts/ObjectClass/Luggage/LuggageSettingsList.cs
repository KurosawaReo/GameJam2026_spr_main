/*
   - LuggageSettingsList -
   荷物の設定リスト.
*/
using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Createメニューから作成できるリストの定義.
/// </summary>
[CreateAssetMenu(menuName = "MyGame/LuggageSettingsList")] //Createメニューのパス.
public class LuggageSettingsList : ScriptableObject
{
    public List<Luggage> list;

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