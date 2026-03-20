using UnityEngine;
using System.Collections.Generic;

// 荷物を入れるダンボール箱のコライダーにアタッチするスクリプト
// このオブジェクトのCollider2Dは isTrigger = true にしておくことを推奨します。
[RequireComponent(typeof(Collider2D))]
public class CardboardBoxArea : MonoBehaviour
{
    [Header("荷物")]
    [SerializeField] LuggageSettingsList luggageList; //荷物設定リスト.

    CardboardBox boxData; //この段ボール箱のデータ.
    Collider2D   boxCollider;

    //get.
    public CardboardBox BoxData { get => boxData; }

    //箱に入っている荷物のリスト（内部管理用）
//  private List<DraggableLuggage> containedLuggages = new List<DraggableLuggage>();

    void Awake()
    {
        boxData     = new();
        boxCollider = GetComponent<Collider2D>();
    }

    /// <summary>
    /// 荷物が箱の中に収まっているか判定.
    /// 完全に入ってる場合のみOK.
    /// </summary>
    /// <param name="luggageBounds">???</param>
    /// <returns>収まっていればtrue</returns>
    public bool ContainsBounds(Bounds luggageBounds)
    {
        if (boxCollider == null) return false;

        // 少し判定を甘くし、荷物の中心が箱のコライダー内に入っていればOKとする
        //return boxCollider.OverlapPoint((Vector2)luggageBounds.center);

        // 四隅チェック
        Vector2 min = luggageBounds.min;
        Vector2 max = luggageBounds.max;

        Vector2[] points = new Vector2[]
        {
            new Vector2(min.x, min.y),
            new Vector2(min.x, max.y),
            new Vector2(max.x, min.y),
            new Vector2(max.x, max.y),
        };

        foreach (var p in points)
        {
            if (!boxCollider.OverlapPoint(p))
                return false;
        }

        return true;
    }

    // 箱の中にある荷物の合計ポイントを計算して boxData.point に反映する関数
    // （例えばタイマー終了時に一括で呼び出すか、荷物を置くたびに呼び出す想定）
    public void UpdateTotalPoints()
    {
        int total = 0;
        List<Collider2D> results = new List<Collider2D>();
        ContactFilter2D filter = new ContactFilter2D();
        filter.NoFilter();
        
        if (boxCollider != null)
        {
            Physics2D.OverlapCollider(boxCollider, filter, results);

            foreach (var col in results)
            {
                DraggableLuggage luggage = col.GetComponent<DraggableLuggage>();

                //完全に箱の中にある荷物のpointを加算.
                if (luggage != null && ContainsBounds(col.bounds))
                {
                    //荷物の種類から、pointを取得して加算.
                    total += luggageList.GetPoint(luggage.luggageData.type);
                }
            }
        }
        
        boxData.point = total;
        Debug.Log($"段ボール箱の現在の合計ポイント: {boxData.point}");
    }
}
