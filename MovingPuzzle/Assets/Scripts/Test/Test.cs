/*
   ただのテスト。開発には使わない。
*/
#if false
using KR.Unity.Inspector;
using UnityEngine;

public class Test : MonoBehaviour
{
    [SerializeField, InspectorDisable("a")]
    int b;
    [SerializeField]
    bool a;

    void Start()
    {
        //段ボール箱生成.
        CardboardBox box = new();
        box.point = 100;
        //完成したら保存.
        AllSceneData.instance.AddCardboardBox(box);

        var boxs = AllSceneData.instance.GetCardboardBoxs();
        Debug.Log(boxs[0].point);
    }
}
#endif