using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ResultManager : MonoBehaviour
{
    [Header("- Animation -")]
    [SerializeField] AnimSceneMove animSceneIn;

    [Header("- Text -")]
    [SerializeField] Text txtScore;

    private void Start()
    {
        //スコア読み込み.
        int score = AllSceneData.instance.ResultPoint;
        Debug.Log("score" + score);
        txtScore.text = score + " point";
    }

    void Update()
    {
        //アニメーション終了後、次のシーンへ.
        if (animSceneIn.IsFinished())
        {
            NextScene();
        }
    }

    /// <summary>
    /// タイトルへ戻る.
    /// </summary>
    public void PushToTitle()
    {
        animSceneIn.AnimExe(); //アニメーション実行.
    }

    /// <summary>
    /// 次のシーンへ.
    /// </summary>
    public void NextScene()
    {
        SceneManager.LoadScene("TitleScene");
    }
}
