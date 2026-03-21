using UnityEngine;

/// <summary>
/// トラックフェーズ用の荷物prefab(落下物)につけるscript
/// </summary>
public class DropLuggage : MonoBehaviour
{
    #region ===== 変数 =====
    private LuggageType      type;    //何の荷物か.
    private CardboardBox     boxData; //段ボール箱データ(段ボール箱の場合)

    private Rigidbody2D      rb;
    private bool             isHolding = true;
    private FurnitureSpawner spawner;
    #endregion

    //get, set.
    public LuggageType Type { 
        get => type; set => type = value; 
    }
    public CardboardBox BoxData { 
        get => boxData; set => boxData = value; 
    }

    #region ===== 初期化 =====
    public void Init(FurnitureSpawner spawner)
    {
        this.spawner = spawner;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // 保険
        if (spawner == null)
        {
            spawner = FindAnyObjectByType<FurnitureSpawner>();
        }

        rb.gravityScale = 0f;
        rb.bodyType = RigidbodyType2D.Kinematic;
//      gameObject.layer = LayerMask.NameToLayer("notsettings");
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
                Drop();
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


    #region ===== 落下処理 =====
    void Drop()
    {
        isHolding = false;

//        gameObject.layer = LayerMask.NameToLayer("settings");

        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.gravityScale = 1.5f;

        // 念のため回転のブレ軽減
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
    #region =====乗ってるものチェック=====

    #endregion

    #region ===== 範囲外チェック =====
    void CheckOutOfBounds()
    {
        if (transform.position.y < -5f)
        {
            //  落ちたら戻す
            if (spawner != null)
            {
                spawner.OnBoxLost(this);
            }

            Destroy(gameObject);
        }
    }
    #endregion
}