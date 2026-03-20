using UnityEngine;
using System.Collections.Generic;

public class LuggageManager : MonoBehaviour
{
    #region ===== 荷物リスト =====
    public List<Luggage> luggageList = new List<Luggage>();
    #endregion

    #region ===== 初期化 =====
    void Start()
    {
        luggageList.Add(new Luggage { type = LuggageType.Chair, point = 10 });
        luggageList.Add(new Luggage { type = LuggageType.Desk, point = 20 });
    }
    #endregion
    

public class LuggageImageManager : MonoBehaviour
{
    #region ===== 画像リスト =====
    public LuggageImageData[] imageList;
    #endregion

    #region ===== 画像取得 =====
    public Sprite GetSprite(LuggageType type)
    {
        foreach (var data in imageList)
        {
            if (data.type == type)
            {
                return data.sprite;
            }
        }

        return null;
    }
    #endregion
}
}