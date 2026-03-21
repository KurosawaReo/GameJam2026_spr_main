using UnityEngine;

public class soundManager : MonoBehaviour
{
    // BGM
    [SerializeField] AudioSource TitleBGM;
    [SerializeField] AudioSource CardBoxBGM;
    [SerializeField] AudioSource TruckBGM;
    [SerializeField] AudioSource ResultBGM;

    // 共用SE
    [SerializeField] AudioSource PressButtonSE;         // ボタン
    [SerializeField] AudioSource TakeLuggageSE;         // 荷物を取る
    [SerializeField] AudioSource CompleteSE;            // 完成
    [SerializeField] AudioSource GameEndSE;             // ゲーム終了

    // 段ボールSE
    [SerializeField] AudioSource CardBoxStartCountSE;   // 段ボール ゲーム開始前カウント
    [SerializeField] AudioSource CardBoxStartSE;        // 段ボール ゲーム開始
    [SerializeField] AudioSource CardBoxPutLightSE;     // 置く(段ボール 軽い)
    [SerializeField] AudioSource CardBoxPutMiddleSE;    // 置く(段ボール 普通)
    [SerializeField] AudioSource CardBoxPutSmallSE;     // 置く(段ボール 小物)
    [SerializeField] AudioSource CardBoxPutIronSE;      // 置く(段ボール 鉄製)

    // トラックSE
    [SerializeField] AudioSource TruckEngineSE;         // トラックの走る音
    [SerializeField] AudioSource TruckPutLightSE;       // 置く(トラック 軽い)
    [SerializeField] AudioSource TruckPutHeavySE;       // 置く(トラック 重い)


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BGMVolume(float volume)
    {
        TitleBGM.volume = volume;
        CardBoxBGM.volume = volume;
        TruckBGM.volume = volume;
        ResultBGM.volume = volume;

    }

    public void SEVolume(float volume)
    {
        // 共用SE
        PressButtonSE.volume = volume;         // ボタン
        TakeLuggageSE.volume = volume;         // 荷物を取る
        CompleteSE.volume = volume;            // 完成
        GameEndSE.volume = volume;             // ゲーム終了

        // 段ボールSE
        CardBoxStartCountSE.volume = volume;   // 段ボール ゲーム開始前カウント
        CardBoxStartSE.volume = volume;        // 段ボール ゲーム開始
        CardBoxPutLightSE.volume = volume;     // 置く(段ボール 軽い)
        CardBoxPutMiddleSE.volume = volume;    // 置く(段ボール 普通)
        CardBoxPutSmallSE.volume = volume;     // 置く(段ボール 小物)
        CardBoxPutIronSE.volume = volume;      // 置く(段ボール 鉄製)

        // トラックSE
        TruckEngineSE.volume = volume;         // トラックの走る音
        TruckPutLightSE.volume = volume;       // 置く(トラック 軽い)
        TruckPutHeavySE.volume = volume;       // 置く(トラック 重い)

    }

}
