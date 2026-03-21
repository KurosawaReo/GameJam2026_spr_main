using UnityEngine;

public class LuggageImageManager : MonoBehaviour
{
    #region ===== 画像リスト =====
    [SerializeField] LuggageSettingsList luggageSettingsList; //荷物設定リスト.
    #endregion

    #region ===== 画像取得 =====
    public Sprite GetSprite(LuggageType type)
    {
        //荷物取得.
        Luggage luggage = luggageSettingsList.GetLuggage(type);
        //画像を返す.
        return luggage.sprite;
    }
    #endregion
}