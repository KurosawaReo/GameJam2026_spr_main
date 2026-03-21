/*
   ただのテスト。開発には使わない。
*/
#if false
using UnityEngine;
using UnityEngine.SceneManagement;

public class Test : MonoBehaviour
{
    [SerializeField] AnimSceneMove anim;

    void Start()
    {
        //椅子のpointを取得.
//      var point = settings.GetPoint(LuggageType.Chair);

        anim.AnimExe();
    }

    void Update()
    {
        if (anim.IsFinished())
        {
            SceneManager.LoadScene("TestKR2");
        }
    }

    /// <summary>
    /// 段ボール箱登録テスト.
    /// </summary>
    private void CardboardBox()
    {
        //段ボール箱生成.
        CardboardBox box = new();
        box.point = 100;
        //完成したら保存.
        AllSceneData.instance.AddCardboardBox(box);

        var boxs = AllSceneData.instance.GetCardboardBoxs();
       /// Debug.Log(boxs[0].point);*/
    }
}
#endif