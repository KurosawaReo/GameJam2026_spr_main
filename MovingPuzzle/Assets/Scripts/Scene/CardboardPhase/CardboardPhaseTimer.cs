using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; 

public class CardboardPhaseTimer : MonoBehaviour
{
    [Header("タイマー設定")]
    [Tooltip("制限時間（秒）")]
    public float timeLimit = 60f;
    
    [Tooltip("遷移先のシーン名")]
    public string nextSceneName;
    
    [Tooltip("残り時間を表示するUIテキスト（任意）")]
    public Text timerText; 

    private float currentTime;
    private bool isFinished = false;

    void Start()
    {
        currentTime = timeLimit;
        UpdateTimerUI();
    }

    void Update()
    {
        if (isFinished) return;

        currentTime -= Time.deltaTime;
        if (currentTime <= 0f)
        {
            currentTime = 0f;
            isFinished = true;
            OnTimeUp();
        }
        UpdateTimerUI();
    }

    void UpdateTimerUI()
    {
        if (timerText != null)
        {
            timerText.text = $"{Mathf.Ceil(currentTime)}";
        }
    }

    void OnTimeUp()
    {
//      Debug.Log("時間切れ！トラックフェーズへ移行します。");
        
        // --- 今回追加：時間切れのタイミングで荷物の合計ポイントを計算する ---
        CardboardBoxArea boxArea = FindFirstObjectByType<CardboardBoxArea>();
        if (boxArea != null)
        {
            boxArea.UpdateTotalPoints();
        }
        // --------------------------------------------------------------------

        if (!string.IsNullOrEmpty(nextSceneName))
        {
            SceneManager.LoadScene(nextSceneName);
        }
    }
}
