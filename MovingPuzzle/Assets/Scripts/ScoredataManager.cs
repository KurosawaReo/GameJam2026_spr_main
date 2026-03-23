using UnityEngine;

public class ScoreDataManager : MonoBehaviour
{
    public static ScoreDataManager instance;

    private int totalScore = 0;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    #region ===== スコア加算 =====
    public void AddScore(int point)
    {
        totalScore += point;
    }
    #endregion

    #region ===== スコア取得 =====
    public int GetScore()
    {
        return totalScore;
    }
    #endregion

    #region ===== リセット =====
    public void ResetScore()
    {
        totalScore = 0;
    }
    #endregion
}