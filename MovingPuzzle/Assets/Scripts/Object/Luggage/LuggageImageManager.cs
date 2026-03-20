using UnityEngine;

public class LuggageImageManager : MonoBehaviour
{
    #region ===== ‰æ‘œƒŠƒXƒg =====
    public LuggageImageData[] imageList;
    #endregion

    #region ===== ‰æ‘œŽæ“¾ =====
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