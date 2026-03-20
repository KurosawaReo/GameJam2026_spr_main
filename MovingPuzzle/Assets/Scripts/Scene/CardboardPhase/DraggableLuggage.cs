using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Collider2D))]
public class DraggableLuggage : MonoBehaviour
{
    private Vector3 offset;
    private Camera mainCamera;
    private bool isCarried = false;
    private bool isJustPickedUp = false;
    
    // 配置前を記憶しておく
    private Vector3 originalPosition;
    private Quaternion originalRotation;
    
    private Collider2D myCollider;

    [Header("荷物データ")]
    [Tooltip("この荷物の種類とポイント設定（提供されたLuggageクラスを統合）")]
    public Luggage luggageData = new Luggage();

    // ダンボール箱（スクリプト内で自動取得するためInspectorからは見えないようにしました）
    private CardboardBoxArea targetBox;
    
    private SpriteRenderer spriteRenderer;
    private Color originalColor;

    void Start()
    {
        mainCamera = Camera.main;
        myCollider = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }
        
        if (targetBox == null)
        {
            targetBox = GetTargetBox();
        }
    }

    private CardboardBoxArea GetTargetBox()
    {
        // 既に配置済み（箱の子になっている）ならその箱を優先する
        if (transform.parent != null)
        {
            CardboardBoxArea parentBox = transform.parent.GetComponent<CardboardBoxArea>();
            if (parentBox != null) return parentBox;
        }

        // 未配置の場合、マネージャーが管理する「現在アクティブな最新の箱」を取得
        if (CardboardBoxManager.Instance != null && CardboardBoxManager.Instance.currentBoxArea != null)
        {
            targetBox = CardboardBoxManager.Instance.currentBoxArea;
            return targetBox;
        }

        // マネージャーがない場合のフォールバック
        if (targetBox == null)
        {
            targetBox = FindFirstObjectByType<CardboardBoxArea>();
        }
        return targetBox;
    }

    void OnMouseDown()
    {
        if (mainCamera == null || isCarried) return;
        
        originalPosition = transform.position;
        originalRotation = transform.rotation;
        Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        offset = transform.position - mouseWorldPos;
        offset.z = 0; // 2DゲームなのでZ軸は固定
        
        isCarried = true;
        isJustPickedUp = true; // このフレーム内でUpdateが走って即配置されるのを防ぐ
    }

    void Update()
    {
        if (!isCarried || mainCamera == null) return;

        Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = -1f; // 確実に手前に来るように-1に固定
        transform.position = mouseWorldPos + offset;

        // --- マウスホイールによる回転 ---
        float scroll = Input.mouseScrollDelta.y;
        if (Mathf.Abs(scroll) > 0.01f)
        {
            // 1目盛りにつき22.5度回転させる
            transform.Rotate(0, 0, Mathf.Sign(scroll) * 22.5f);
        }

        // ドラッグ中の状態表示（配置可能かどうかのフィードバック）
        bool isValid = CheckPlacementValid();
        if (spriteRenderer != null)
        {
            // 配置不可なら半透明の赤色にするなど
            spriteRenderer.color = isValid ? originalColor : new Color(1f, 0.5f, 0.5f, 0.8f);
        }

        // マウスの左クリックで配置を試みる
        if (Input.GetMouseButtonDown(0))
        {
            if (isJustPickedUp)
            {
                isJustPickedUp = false;
                return;
            }

            TryPlace(isValid);
        }
    }

    void TryPlace(bool isValid)
    {
        isCarried = false;

        if (isValid)
        {
            // 配置成功（必要なら配置完了エフェクトや音声を再生）
            if (spriteRenderer != null) spriteRenderer.color = originalColor;

            CardboardBoxArea currentBox = GetTargetBox();
            bool isInsideBox = (currentBox != null && currentBox.ContainsBounds(myCollider.bounds));

            if (isInsideBox && currentBox != null)
            {
                // 箱の中に置かれた場合は箱の子オブジェクトにする
                transform.SetParent(currentBox.transform, true);
                Vector3 newPos = transform.position;
                newPos.z = -1f;
                transform.position = newPos;
            }
            else
            {
                // スポーンエリアなどに置かれた場合（箱の外）は親を解除する
                transform.SetParent(null, true);
                Vector3 newPos = transform.position;
                newPos.z = -1f;
                transform.position = newPos;
            }
        }
        else
        {
            // 配置不可の場合、元の位置・回転に戻す
            transform.position = originalPosition;
            transform.rotation = originalRotation;
            if (spriteRenderer != null) spriteRenderer.color = originalColor;
            Debug.Log("そこには配置できません！");
        }
    }

    private bool IsInsideSpawnArea(Vector3 pos)
    {
        if (LuggageSpawner.Instance == null) return false;
        
        Vector3 spawnerPos = LuggageSpawner.Instance.transform.position;
        Vector2 size = LuggageSpawner.Instance.spawnAreaSize;
        
        float minX = spawnerPos.x - size.x / 2f;
        float maxX = spawnerPos.x + size.x / 2f;
        float minY = spawnerPos.y - size.y / 2f;
        float maxY = spawnerPos.y + size.y / 2f;
        
        return pos.x >= minX && pos.x <= maxX && pos.y >= minY && pos.y <= maxY;
    }

    // 配置が有効かどうかをチェックする
    private bool CheckPlacementValid()
    {
        CardboardBoxArea currentBox = GetTargetBox();
        bool isInsideBox = (currentBox != null && currentBox.ContainsBounds(myCollider.bounds));
        bool isInsideSpawn = IsInsideSpawnArea(myCollider.bounds.center);

        // 箱の中にも入っておらず、スポーンエリア内でもなければ配置不可
        if (!isInsideBox && !isInsideSpawn)
        {
            return false;
        }

        // 箱の中に置く場合のみ、他の荷物との重なり判定を厳格に行う
        if (isInsideBox)
        {
            List<Collider2D> results = new List<Collider2D>();
            ContactFilter2D filter = new ContactFilter2D();
            filter.NoFilter(); // すべてのコライダーを対象
            filter.useTriggers = false; // トリガーは無視

            Physics2D.OverlapCollider(myCollider, filter, results);
            
            foreach(Collider2D col in results)
            {
                 if (col == myCollider) continue;

                 // 箱に入れようとしているのに他の荷物と重なっている場合はNG
                 if (col.GetComponent<DraggableLuggage>() != null)
                 {
                     return false;
                 }
            }
        }

        // スポーンエリア内に置く場合は、他の荷物と重なっていてもOK
        return true;
    }
}
