using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    [Header("- Animation -")]
    [SerializeField] AnimSceneMove animSceneIn;

    void Start()
    {
        Debug.Log("point" + AllSceneData.instance?.ResultPoint);
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
    /// ゲーム開始.
    /// </summary>
    public void PushStart()
    {
        animSceneIn.AnimExe(); //アニメーション実行.
    }

    /// <summary>
    /// 次のシーンへ.
    /// </summary>
    public void NextScene()
    {
        SceneManager.LoadScene("CardboardBoxScene");
    }
}