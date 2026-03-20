using UnityEngine;

public class FurnitureSpawner : MonoBehaviour
{
    #region ===== 変数 =====
    public GameObject[] prefabs; // enum順
    public Transform spawnPoint;

    private GameObject currentBox;

    // ⭐ 生成チェック用半径
    [SerializeField] private float spawnCheckRadius = 0.5f;
    #endregion


    #region ===== 生成位置チェック =====
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

        // ⭐ 範囲チェック
        if ((int)luggage.type >= prefabs.Length)
        {
            Debug.LogError("Prefab数が足りない！");
            return;
        }

        GameObject prefab = prefabs[(int)luggage.type];

        if (prefab == null)
        {
            Debug.LogError("Prefabが設定されてない！");
            return;
        }

        Vector3 pos = spawnPoint.position;

        // ⭐ ここが追加ポイント（生成禁止）
        if (!CanSpawnHere(pos))
        {
            return;
        }

        // ⭐ 生成
        GameObject obj = Instantiate(prefab, pos, Quaternion.identity);
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
        if (spawnPoint == null) return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(spawnPoint.position, spawnCheckRadius);
    }
    #endregion
}