using UnityEngine;

/// <summary>
/// トラックフェーズのスポナー.
/// </summary>
public class FurnitureSpawner : MonoBehaviour
{
    #region ===== 変数 =====
    public LuggageSettingsList LuggageSettingsList; //荷物設定リスト.

    private GameObject currentBox; //荷物を1つだけ選択できるようにする用.
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
        // ⭐ 同時に1個だけ
        if (currentBox != null) return;

        // ⭐ null対策
        if (luggage == null)
        {
            Debug.LogError("Luggageがnull！");
            return;
        }

        //設置リストからprefab取得.
        GameObject prefab = LuggageSettingsList.GetLuggage(luggage.type).prefab;
        
        if (prefab == null)
        {
            Debug.LogError("設置リストにPrefabが設定されてない！");
            return;
        }

        // ⭐ 生成
        GameObject obj = Instantiate(prefab);
        currentBox = obj;

        // ⭐ DropBox取得
        DropLuggage box = obj.GetComponent<DropLuggage>();

        if (box != null)
        {
            box.Init(this);
            box.SetType(luggage.type);
        }
        else
        {
            Debug.LogError("DropBoxがPrefabについてない！");
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
            ui.ReturnToList(type); //残りのリストに戻す.
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