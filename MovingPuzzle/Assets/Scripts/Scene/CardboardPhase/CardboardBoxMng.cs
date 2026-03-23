using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using KR.Unity.Object;
using UnityEngine.UIElements;

/// <summary>
/// 段ボール箱の管理.
/// </summary>
public class CardboardBoxMng : MonoBehaviour
{
    public static CardboardBoxMng Instance;

    [Header("- Setting -")]
    [Tooltip("新しく生成する段ボール箱のプレハブ")]
    public GameObject cardboardBoxPrefab;
    [Tooltip("段ボールが定位置にあるときの座標を示すための空オブジェクト（指定がなければ現在位置を利用）")]
    public Transform activeBoxPosition;
    [Tooltip("画面外（退場先・入場元）の座標を示すための空オブジェクト（指定がなければ右から左へ移動）")]
    public Transform offscreenPositionOut;
    public Transform offscreenPositionIn;
    
    [Header("- UI -")]
    [Tooltip("次の箱へ進むアクションをトリガーするボタン")]
    public UnityEngine.UI.Button nextBoxButton;
    [Tooltip("移動にかかる時間（秒）")]
    public float transitionDuration = 1.0f;

    [Header("- Object -")]
    [SerializeField] PrefabKR compCardboardBox; //完成した段ボール箱.

    [HideInInspector]
    public CardboardBoxArea currentBoxArea;
    private bool isTransitioning = false;

    // 初期の箱の状態を記憶（ユーザーが配置用ダミーをスケールし忘れても大きさを固定するため）
    private Vector3 initialScale = Vector3.one;
    private Quaternion initialRotation = Quaternion.identity;
    private Transform initialParent = null;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        if (nextBoxButton != null)
        {
            nextBoxButton.onClick.AddListener(OnNextBoxButtonClicked);
        }

        // 既にシーンにある箱を取得して初期設定
        currentBoxArea = FindFirstObjectByType<CardboardBoxArea>();
        
        if (currentBoxArea != null)
        {
            initialScale = currentBoxArea.transform.localScale;
            initialRotation = currentBoxArea.transform.rotation;
            initialParent = currentBoxArea.transform.parent;
        }
        
        if (activeBoxPosition == null && currentBoxArea != null)
        {
            // もしアクティブ位置が未設定なら、ダミーを作成
            GameObject dummy = new GameObject("ActiveBoxPosition_Default");
            dummy.transform.SetParent(initialParent);
            dummy.transform.position = currentBoxArea.transform.position;
            dummy.transform.rotation = initialRotation;
            dummy.transform.localScale = initialScale;
            activeBoxPosition = dummy.transform;
        }
    }

    public void OnNextBoxButtonClicked()
    {
        if (isTransitioning) return;
        StartCoroutine(TransitionBoxCoroutine());
    }

    private IEnumerator TransitionBoxCoroutine()
    {
        isTransitioning = true;
        if (nextBoxButton != null) nextBoxButton.interactable = false;

        CardboardBoxArea oldBox = currentBoxArea;
        
        // --- 1. 古い箱を画面外へ移動させる ---
        if (oldBox != null)
        {
            // 得点の集計などを行う
            oldBox.UpdateTotalPoints();

            // --- 全体管理用スクリプト (AllSceneData) にこの箱のデータを格納する ---
            if (AllSceneData.instance != null)
            {
                AllSceneData.instance.AddCardboardBox(oldBox.BoxData);
            }

            //prefab生成.
            GameObject obj = compCardboardBox.NewPrefab();
            //script取得.
            DropLuggage script = obj.GetComponent<DropLuggage>();

            if (script != null)
            {
                script.Type    = LuggageType.Box; //種類は段ボール箱.
                script.BoxData = oldBox.BoxData;  //段ボール箱データを渡す.
            }
            else
            {
                Debug.LogError("DropLuggageがついてない！");
            }

            // --- 補充処理：今回送った荷物と「全く同じ数だけ」新しく追加して上限を保つ ---
            if (LuggageSpawner.Instance != null)
            {
                // 箱の子オブジェクトになっている荷物の数を直接取得する
                DraggableLuggage[] sentLuggages = oldBox.GetComponentsInChildren<DraggableLuggage>();
                int amountToSpawn = sentLuggages.Length;
                
                if (amountToSpawn > 0)
                {
                    LuggageSpawner.Instance.SpawnLuggages(amountToSpawn);
                }
            }
            
            // 荷物は子オブジェクトになっているため、箱と一緒に自動で移動します。
            Vector3 startPos = oldBox.transform.position;
            Vector3 endPos = offscreenPositionOut != null ? offscreenPositionOut.position : oldBox.transform.position + new Vector3(20f, 0, 0);

            float t = 0;
            while (t < transitionDuration)
            {
                t += Time.deltaTime;
                float normalizedTime = t / transitionDuration;
                // Easing (SmoothStep等) を入れると動きが自然になります
                float easeT = Mathf.SmoothStep(0, 1, normalizedTime);
                
                oldBox.transform.position = Vector3.Lerp(startPos, endPos, easeT);
                
                // フェードアウト効果 (荷物ごとアルファを下げる簡易実装)
                SpriteRenderer[] renderers = oldBox.GetComponentsInChildren<SpriteRenderer>();
                foreach(var sr in renderers)
                {
                    Color c = sr.color;
                    c.a = Mathf.Lerp(1f, 0f, easeT);
                    sr.color = c;
                }

                yield return null;
            }
            // 画面外に出たら削除
            Destroy(oldBox.gameObject);
        }

        // --- 2. 新しい箱を画面外から入場させる ---
        if (cardboardBoxPrefab != null)
        {
            Vector3 spawnTargetPos = activeBoxPosition != null ? activeBoxPosition.position : Vector3.zero;
            Vector3 startPos = offscreenPositionIn != null ? offscreenPositionIn.position : spawnTargetPos + new Vector3(-20f, 0, 0);

            // インスタンス生成（回転は最初の箱と同じにする）
            GameObject newBoxObj = Instantiate(cardboardBoxPrefab, startPos, initialRotation);
            
            // ユーザーによって指定されたダミーポイント等の予期せぬスケールを無視し、強制的に最初の箱と同じスケール・親を設定する
            newBoxObj.transform.SetParent(initialParent, true);
            newBoxObj.transform.localScale = initialScale;

            currentBoxArea = newBoxObj.GetComponent<CardboardBoxArea>();

            // 新しい箱に初期からついている荷物などに対してフェードイン用初期化
            SpriteRenderer[] newRenderers = newBoxObj.GetComponentsInChildren<SpriteRenderer>();
            foreach(var sr in newRenderers)
            {
                Color c = sr.color;
                c.a = 0f;
                sr.color = c;
            }

            float t = 0;
            while (t < transitionDuration)
            {
                t += Time.deltaTime;
                float normalizedTime = t / transitionDuration;
                float easeT = Mathf.SmoothStep(0, 1, normalizedTime);
                
                newBoxObj.transform.position = Vector3.Lerp(startPos, spawnTargetPos, easeT);

                // フェードイン効果
                foreach(var sr in newRenderers)
                {
                    Color c = sr.color;
                    c.a = Mathf.Lerp(0f, 1f, easeT);
                    sr.color = c;
                }

                yield return null;
            }
            newBoxObj.transform.position = spawnTargetPos;
        }
        else
        {
            Debug.LogWarning("次の箱のプレハブが設定されていません。CardboardBoxManagerにプレハブをアタッチしてください。");
        }

        if (nextBoxButton != null) nextBoxButton.interactable = true;
        isTransitioning = false;
    }
}
