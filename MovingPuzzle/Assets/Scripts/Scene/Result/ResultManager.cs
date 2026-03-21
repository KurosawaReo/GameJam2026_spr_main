using UnityEngine;
using UnityEngine.SceneManagement;

public class ResultManager : MonoBehaviour
{
    [Header("- Animation -")]
    [SerializeField] AnimSceneMove animSceneIn;

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
