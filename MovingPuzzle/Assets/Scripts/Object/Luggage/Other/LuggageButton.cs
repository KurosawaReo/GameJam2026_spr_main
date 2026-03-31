using UnityEngine;
using UnityEngine.UI;

public class LuggageButton : MonoBehaviour
{
    #region ===== 変数 =====
    [Header("- image -")]
    [SerializeField] Image iconImage; //子オブジェクトの画像.

    [Header("- script -")]
    [SerializeField] FurnitureSpawner    spawner;
    [SerializeField] LuggageImageManager imageManager;

    Luggage      luggageData = new(); //荷物データ.
    CardboardBox boxData     = new(); //段ボール箱データ.

    //get, set.
    public Luggage      LuggageData { get => luggageData; set => luggageData = value; }
    public CardboardBox BoxData     { get => boxData; set => boxData = value; }
    #endregion

    #region ===== 初期化 =====
    void Start()
    {
        NextLuggage(); //初期荷物選択.
    }
    #endregion

    #region ===== クリック処理 =====
    public void OnClick()
    {
        //ボタンを押したら.
        if (spawner != null)
        {
            spawner.Spawn(luggageData, boxData); //荷物生成.
            NextLuggage();                       //次の荷物を選択.
        }
        else
        {
            Debug.LogError("Spawnerが見つからない！");
        }
    }
    #endregion

    #region ===== 荷物 =====
    /// <summary>
    /// 次の荷物を選択.
    /// </summary>
    public void NextLuggage()
    {
        //次の荷物を取得.
        (luggageData.type, boxData) = spawner.SetupButton();

        if (iconImage == null)
        {
            Debug.LogError("Iconが設定されてない！");
            return;
        }
        if (imageManager == null)
        {
            Debug.LogError("ImageManagerが見つからない！");
            return;
        }

        //画像取得.
        Sprite sprite = imageManager.GetSprite(luggageData.type);

        if (sprite != null)
        {
            iconImage.sprite = sprite; //画像適用.
        }
        else
        {
            Debug.LogWarning("対応する画像がない: " + luggageData.type);
        }
    }
    #endregion
}