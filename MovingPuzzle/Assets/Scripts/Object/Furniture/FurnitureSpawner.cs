using UnityEngine;

/// <summary>
/// トラックフェーズのスポナー.
/// </summary>
public class FurnitureSpawner : MonoBehaviour
{
    #region ===== 変数 =====
    public LuggageSettingsList LuggageSettingsList; //荷物設定リスト.

    private GameObject currentBox;
    #endregion


    #region ===== 生成位置チェック =====

#if false
    bool CanSpawnHere(Vector3 pos)
    {
        // ⭐ Luggageレイヤーだけ検出
        int layerMask = LayerMask.GetMask("Placed");

        Collider2D hit = Physics2D.OverlapCircle(pos, spawnCheckRadius, layerMask);

        if (hit != null)
        {
            Debug.Log("ここには出せない！（既に物がある）");
            return false;
        }

        return true;
    }
#endif
    #endregion


    #region ===== 家具生成 =====
    public void SpawnFurniture(Luggage luggage)
    {
        if (currentBox != null) return;

        if (luggage == null)
        {
            Debug.LogError("Luggageがnull！");
            return;
        }

        if (LuggageSettingsList == null)
        {
            Debug.LogError("LuggageSettingsListが未設定！");
            return;
        }

        var data = LuggageSettingsList.GetLuggage(luggage.type);

        if (data == null)
        {
            Debug.LogError("未登録のLuggageType: " + luggage.type);
            return;
        }

        if (data.prefab == null)
        {
            Debug.LogError("Prefab未設定: " + luggage.type);
            return;
        }

        GameObject obj = Instantiate(data.prefab);
        currentBox = obj;

        DropLuggage box = obj.GetComponent<DropLuggage>();

        if (box != null)
        {
            box.Init(this);
            box.SetType(luggage.type);
        }
        else
        {
            Debug.LogError("DropLuggageがPrefabについてない！");
        }
    }
    #endregion


    #region ===== 落下して消えたとき =====
    public void OnBoxLost(DropLuggage box)
    {
        if (box == null) return;

        LuggageType type = box.GetLuggageType();

        RandomLuggageUI ui = FindAnyObjectByType<RandomLuggageUI>();

        if (ui != null)
        {
            ui.ReturnToList(type);
        }
        else
        {
            Debug.LogError("RandomLuggageUIが見つからない！");
        }
    }
    #endregion


    #region ===== ドロップ完了 =====
    public void OnDropped()
    {
        currentBox = null;
    }
    #endregion


    #region ===== デバッグ表示 =====
    void OnDrawGizmos()
    {
    }
    #endregion
}