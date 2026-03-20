using UnityEngine;
using System.Collections.Generic;

public class TruckManager : MonoBehaviour
{
    #region ===== 設定 =====
    [SerializeField] private Transform checkAreaCenter;
    [SerializeField] private Vector2 checkAreaSize = new Vector2(5f, 2f);

    // 検出するレイヤー
    [SerializeField] private LayerMask targetLayer;
    #endregion


    #region ===== 更新 =====
    float timer = 0f;
    float interval = 1f; 

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= interval)
        {
            timer = 0f;
            CheckOnTruck();
        }
    }
    #endregion

    #region ===== 判定処理 =====
    void CheckOnTruck()
    {
        Collider2D[] hits = Physics2D.OverlapBoxAll(
            checkAreaCenter.position,
            checkAreaSize,
            0f,
            targetLayer
        );

        foreach (var hit in hits)
        {
            // ダンボールか？
            DropLuggage box = hit.GetComponent<DropLuggage>();

            if (box != null)
            {
                Debug.Log("荷物が乗ってる: " + box.GetLuggageType());
                continue;
            }

            // それ以外（ダンボール）
            Debug.Log("ダンボールが乗ってる: " + hit.name);
        }
    }
    #endregion


    #region ===== デバッグ表示 =====
    void OnDrawGizmos()
    {
        if (checkAreaCenter == null) return;

        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(checkAreaCenter.position, checkAreaSize);
    }
    #endregion
}