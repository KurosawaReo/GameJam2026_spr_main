using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ResultManager : MonoBehaviour
{
    [Header("- Animation -")]
    [SerializeField] AnimSceneMove animSceneIn;

    [Header("- Text -")]
    [SerializeField] Text txtScore;

    void Start()
    {
        //スコア読み込み.
        if (AllSceneData.instance)
        {
            int score = AllSceneData.instance.TotalPoint;
            txtScore.text = score + " point";
        }
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
        AllSceneData.instance.TotalPoint = 0; //リセット.

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
