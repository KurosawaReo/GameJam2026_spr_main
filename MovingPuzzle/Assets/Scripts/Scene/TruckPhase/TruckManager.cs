using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TruckManager : MonoBehaviour
{
    #region ===== 変数 - タイマー =====
    [Header("タイマー設定")]
    [Tooltip("制限時間（秒）")]
    public float timeLimit = 60f;
    [Tooltip("遷移先のシーン名")]
    public string nextSceneName;
    [Tooltip("残り時間を表示するUIテキスト")]
    public Text timerText;

    [Header("アニメーション")]
    [SerializeField] AnimSceneMove animSceneIn;

    private float currentTime;
    private bool isFinished = false;
    #endregion

    #region ===== 変数 =====
    [Header("その他")]
    [SerializeField] private Transform checkAreaCenter;
    [SerializeField] private Vector2 checkAreaSize = new Vector2(5f, 2f);
    [SerializeField] private LayerMask targetLayer;

    [SerializeField] private LuggageSettingsList settingsList;

    int gamePoint = 0; //ゲームのポイント.
    private HashSet<DropLuggage> detectedLuggage = new HashSet<DropLuggage>();

    //get, set.
    public int GamePoint {
        get => gamePoint; set => gamePoint = value;
    }
    #endregion

    void Start()
    {
        currentTime = timeLimit;
        UpdateTimerUI();
    }

    #region ===== 更新 =====
    void Update()
    {
        UpdateTimer();
    }
    #endregion


    private void UpdateTimer()
    {
        //ゲーム中.
        if (!isFinished)
        {
            currentTime -= Time.deltaTime;
            if (currentTime <= 0f)
            {
                currentTime = 0f;
                isFinished = true; //一度きり実行.
                OnTimeUp();
            }
            UpdateTimerUI();
        }
        //終了後.
        else
        {
            CheckOnTruck();

            //アニメーション終了後、次のシーンへ.
            if (animSceneIn.IsFinished())
            {
                NextScene();
            }
        }
    }

    /// <summary>
    /// タイマー表示更新.
    /// </summary>
    void UpdateTimerUI()
    {
        if (timerText != null)
        {
            timerText.text = $"{Mathf.Ceil(currentTime)}";
        }
    }

    /// <summary>
    /// 時間切れになったら作動.
    /// </summary>
    void OnTimeUp()
    {
        //荷物の合計ポイントを計算.
        CardboardBoxArea boxArea = FindFirstObjectByType<CardboardBoxArea>();
        if (boxArea != null)
        {
            boxArea.UpdateTotalPoints();
        }

        animSceneIn.AnimExe(); //アニメーション実行.
    }

    /// <summary>
    /// 次のシーンへ.
    /// </summary>
    public void NextScene()
    {
        if (!string.IsNullOrEmpty(nextSceneName))
        {
            SceneManager.LoadScene(nextSceneName); //シーン移動.
        }
    }


    #region ===== 判定 =====
    /// <summary>
    /// point集計.
    /// </summary>
    void CheckOnTruck()
    {
        if (settingsList == null)
        {
            Debug.LogError("settingsListが未設定！");
            return;
        }

       
        detectedLuggage.RemoveWhere(l => l == null);

        Collider2D[] hits = Physics2D.OverlapBoxAll(
            checkAreaCenter.position,
            checkAreaSize,
            0f,
            targetLayer
        );

        foreach (var hit in hits)
        {
            DropLuggage luggage = hit.GetComponent<DropLuggage>();

            if (luggage == null) continue;
            
            if (!detectedLuggage.Add(luggage)) continue;

            LuggageType type = luggage.Type;

            int point = settingsList.GetPoint(type);

            AddScore(point, type.ToString());
        }
    }
    #endregion


    #region ===== スコア =====
    void AddScore(int point, string name)
    {
        if (point == 0)
        {
            Debug.LogWarning($"point未設定: {name}");
        }

        Debug.Log($"{name} → +{point}");

        if (ScoreDataManager.instance != null)
        {
            ScoreDataManager.instance.AddScore(point);
        }
        else
        {
            Debug.LogError("ScoreDataManagerが存在しない！");
        }
    }
    #endregion


    #region ===== リザルト送信 =====
    public void GoResult()
    {
        if (ScoreDataManager.instance == null)
        {
            Debug.LogError("ScoreDataManagerがない！");
            return;
        }

        if (AllSceneData.instance == null)
        {
            Debug.LogError("AllSceneDataがない！");
            return;
        }

        int score = ScoreDataManager.instance.GetScore();

        AllSceneData.instance.ResultPoint = score;

        Debug.Log("リザルト送信: " + score);

        SceneManager.LoadScene("ResultScene");
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