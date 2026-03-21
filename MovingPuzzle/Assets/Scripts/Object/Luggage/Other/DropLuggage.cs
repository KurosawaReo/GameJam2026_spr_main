using UnityEngine;

public class DropLuggage : MonoBehaviour
{
    #region ===== 変数 =====
    private Rigidbody2D rb;
    private bool isHolding = true;
    private FurnitureSpawner spawner;

    private LuggageType type;

    [SerializeField] private float checkRadius = 0.5f;
    [SerializeField] private LayerMask blockLayer; // ←追加
    #endregion

    //settings notsettings track のレイヤー追加必須

    #region ===== 初期化 =====
    public void Init(FurnitureSpawner spawnerRef)
    {
        spawner = spawnerRef;
    }

    public void SetType(LuggageType t)
    {
        type = t;
    }

    public LuggageType GetLuggageType()
    {
        return type;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        if (spawner == null)
        {
            spawner = FindAnyObjectByType<FurnitureSpawner>();
        }

        rb.gravityScale = 0f;
        rb.bodyType = RigidbodyType2D.Kinematic;

        gameObject.layer = LayerMask.NameToLayer("notsettings");
    }
    #endregion


    #region ===== 更新 =====
    void Update()
    {
        if (isHolding)
        {
            FollowMouse();
            HandleRotation();

            if (Input.GetMouseButtonUp(0))
            {
                TryDrop(); // ←変更
            }
        }
        else
        {
            CheckOutOfBounds();
        }
    }
    #endregion


    #region ===== マウス追従 =====
    void FollowMouse()
    {
        Vector3 mouse = Input.mousePosition;
        mouse.z = 10f;
        Vector3 pos = Camera.main.ScreenToWorldPoint(mouse);

        transform.position = new Vector3(pos.x, pos.y, 0);
    }
    #endregion


    #region ===== 回転処理 =====
    void HandleRotation()
    {
        float scroll = Input.mouseScrollDelta.y;

        if (scroll != 0)
        {
            transform.Rotate(0, 0, scroll * 10f);
        }
    }
    #endregion


    #region ===== 落下判定 =====
    void TryDrop()
    {
        if (IsOverlapping())
        {
            Debug.Log("ここには置けない！");
            return;
        }

        Drop();
    }

    bool IsOverlapping()
    {
        Collider2D hit = Physics2D.OverlapCircle(
            transform.position,
            checkRadius,
            blockLayer
        );

        return hit != null;
    }
    #endregion


    #region ===== 落下処理 =====
    void Drop()
    {
        isHolding = false;

        gameObject.layer = LayerMask.NameToLayer("settings");

        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.gravityScale = 1.5f;
        rb.angularVelocity = 0f;

        if (spawner != null)
        {
            spawner.OnDropped();
        }
        else
        {
            Debug.LogError("Spawnerが設定されてない！");
        }
    }
    #endregion


    #region ===== 範囲外チェック =====
    void CheckOutOfBounds()
    {
        if (transform.position.y < -5f)
        {
            if (spawner != null)
            {
                spawner.OnBoxLost(this);
            }

            Destroy(gameObject);
        }
    }
    #endregion


    #region ===== デバッグ =====
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, checkRadius);
    }
    #endregion
}