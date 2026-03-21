using UnityEngine;
using UnityEngine.UI;

public class LuggageButton : MonoBehaviour
{
    #region ===== 変数 =====
    public Luggage data = new Luggage();
    public Image iconImage; //子オブジェクトの画像.

    private RandomLuggageUI randomUI;
    private Button button;
    private FurnitureSpawner spawner;
    private LuggageImageManager imageManager;
    #endregion


    #region ===== 初期化 =====
    void Start()
    {
        button = GetComponent<Button>();

        //  null対策で取得
        randomUI = FindAnyObjectByType<RandomLuggageUI>();

        spawner = FindAnyObjectByType<FurnitureSpawner>();
        imageManager = FindAnyObjectByType<LuggageImageManager>();

        // ボタン登録
        if (button != null)
        {
            button.onClick.AddListener(OnClick);
        }
        else
        {
            Debug.LogError("Buttonコンポーネントがない！");
        }

        UpdateIcon();
    }
    #endregion


    #region ===== クリック処理 =====
    void OnClick()
    {
        if (spawner != null)
        {
            spawner.SpawnFurniture(data);
        }
        else
        {
            Debug.LogError("Spawnerが見つからない！");
            return;
        }

        //  押したら次の家具に変える
        if (randomUI != null)
        {
            randomUI.SetRandomToButton(this);
        }
    }
    #endregion


    #region ===== アイコン更新 =====
    public void UpdateIcon()
    {
        if (iconImage == null)
        {
            Debug.LogError("Iconが設定されてない！");
            return;
        }

        //  ここで取得する
        if (imageManager == null)
        {
            imageManager = FindAnyObjectByType<LuggageImageManager>();
        }

        if (imageManager == null)
        {
            Debug.LogError("ImageManagerが見つからない！");
            return;
        }

        Sprite sprite = imageManager.GetSprite(data.type);

        if (sprite != null)
        {
            iconImage.sprite = sprite;
        }
        else
        {
            Debug.LogWarning("対応する画像がない: " + data.type);
        }
    }
    #endregion
}