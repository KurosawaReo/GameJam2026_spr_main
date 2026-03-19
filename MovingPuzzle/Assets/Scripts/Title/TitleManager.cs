using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    /// <summary>
    /// ゲーム開始.
    /// </summary>
    public void PushStart()
    {
        SceneManager.LoadScene("CardboardBoxScene"); //段ボール箱フェーズへ.

        Luggage l = new Luggage(); //荷物を作成.

        //荷物の設定.
        l.type  = LuggageType.Chair; //何の荷物か.
        l.point = 1;                 //何pointか.
    }
}