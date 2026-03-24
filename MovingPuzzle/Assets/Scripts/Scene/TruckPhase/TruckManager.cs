using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Linq;

public class TruckManager : MonoBehaviour
{
    #region ===== 変数 - タイマー =====
    [Header("- Timer -")]
    [Tooltip("制限時間(秒)")]
    [SerializeField] float timeLimit = 60f;
    [Tooltip("遷移先のシーン名")]
    [SerializeField] string nextSceneName;

    [Header("- Text -")]
    [Tooltip("残り時間を表示するUIテキスト")]
    [SerializeField] Text timerText;
    [SerializeField] Text pointText;

    [Header("- Animation -")]
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
        CheckOnTruck();

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
            //アニメーション終了後、次のシーンへ.
            if (animSceneIn.IsFinished())
            {
                NextScene();
            }
        }
    }
    #endregion

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
       
        Collider2D[] hits = Physics2D.OverlapBoxAll(
            checkAreaCenter.position,
            checkAreaSize,
            0f,
            targetLayer
        );

        //ポイント計測用.
        int totalPoint = 0;
        //トラックに乗った荷物をループ.\

        foreach (var hit in hits)
        {
            DropLuggage luggage = hit.GetComponent<DropLuggage>();

            if (luggage == null) continue;

            //スコア加算.
            totalPoint += settingsList.GetPoint(luggage.Type);
        }

        //point表示.
        pointText.text = "point: " + totalPoint;
        //全シーンデータへの保存.
        if (AllSceneData.instance)
        {
            AllSceneData.instance.TotalPoint = totalPoint;
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