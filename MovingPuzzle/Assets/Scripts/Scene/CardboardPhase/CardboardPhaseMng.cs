using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; 

/// <summary>
/// 段ボールフェーズの管理.
/// </summary>
public class CardboardPhaseMng : MonoBehaviour
{
    [Header("タイマー設定")]
    [Tooltip("制限時間（秒）")]
    public float timeLimit = 60f;
    [Tooltip("遷移先のシーン名")]
    public string nextSceneName;
    [Tooltip("残り時間を表示するUIテキスト（任意）")]
    public Text timerText;

    [Header("アニメーション")]
    [SerializeField] AnimSceneMove animSceneIn;

    private float currentTime;
    private bool isFinished = false;

    void Start()
    {
        currentTime = timeLimit;
        UpdateTimerUI();
    }

    void Update()
    {
        UpdateTimer();
    }

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
}
