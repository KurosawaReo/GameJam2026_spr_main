using UnityEngine;
using System.Collections.Generic;

// 荷物を入れるダンボール箱のコライダーにアタッチするスクリプト
// このオブジェクトのCollider2Dは isTrigger = true にしておくことを推奨します。
[RequireComponent(typeof(Collider2D))]
public class CardboardBoxArea : MonoBehaviour
{
    private Collider2D boxCollider;

    [Header("箱のデータ")]
    [Tooltip("提供された段ボール箱クラスを統合")]
    public CardboardBox boxData = new CardboardBox();

    // 箱に入っている荷物のリスト（内部管理用）
    private List<DraggableLuggage> containedLuggages = new List<DraggableLuggage>();

    void Awake()
    {
        boxCollider = GetComponent<Collider2D>();
    }

    // 荷物が箱の中に収まっているか判定する
    public bool ContainsBounds(Bounds luggageBounds)
    {
        if (boxCollider == null) return false;
        
        // 少し判定を甘くし、荷物の中心が箱のコライダー内に入っていればOKとする
        return boxCollider.OverlapPoint((Vector2)luggageBounds.center);
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
                // 荷物であり、かつ完全に箱の中にある場合のみ加算
                if (luggage != null && ContainsBounds(col.bounds))
                {
                    total += luggage.luggageData.point;
                }
            }
        }
        
        boxData.point = total;
        Debug.Log($"段ボール箱の現在の合計ポイント: {boxData.point}");
    }
}
