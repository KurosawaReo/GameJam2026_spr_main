using UnityEngine;

public class LuggageImageManager : MonoBehaviour
{
    #region ===== ‰æ‘œƒŠƒXƒg =====
    [SerializeField] LuggageSettingsList luggageSettingsList;
    #endregion

    #region ===== ‰æ‘œæ“¾ =====
    public Sprite GetSprite(LuggageType type)
    {
        Luggage luggage = luggageSettingsList.GetLuggage(type);
        return luggage.sprite;
    }
    #endregion
}