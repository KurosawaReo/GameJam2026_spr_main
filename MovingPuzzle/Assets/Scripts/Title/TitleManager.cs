using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    /// <summary>
    /// ゲーム開始.
    /// </summary>
    public void PushStart()
    {
        SceneManager.LoadScene("CardboardScene"); //ダンボールフェーズへ.
    }
}