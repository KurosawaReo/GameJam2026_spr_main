using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class TruckManager : MonoBehaviour
{
    #region ===== 変数 =====
    [SerializeField] private Transform checkAreaCenter;
    [SerializeField] private Vector2 checkAreaSize = new Vector2(5f, 2f);
    [SerializeField] private LayerMask targetLayer;

    [SerializeField] private LuggageSettingsList settingsList;
    private float timer = 0f;
    private float interval = 1f;

    int gamePoint = 0; //ゲームのポイント.
    private HashSet<DropLuggage> detectedLuggage = new HashSet<DropLuggage>();

    //get, set.
    public int GamePoint {
        get => gamePoint; set => gamePoint = value;
    }
    #endregion

    #region ===== 更新 =====
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


    #region ===== 判定 =====
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