using KR.Unity.Sound;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// メニュー管理用.
/// </summary>
public class MenuManager : MonoBehaviour
{
    [Header("- Animation -")]
    [SerializeField] AnimSceneMove animSceneIn;

    [Header("- Object -")]
    [SerializeField] GameObject pnlMenu;
    [SerializeField] Slider     sliderBgm;
    [SerializeField] Slider     sliderSe;

    bool isActive = false;
    bool isPush   = false; //このクラスのボタンが押されたか.

    void Start()
    {
        if (SoundMngKR.Inst)
        {
            //SoundManagerの値を反映.
            sliderBgm.value = SoundMngKR.Inst.GetVolumeBGM();
            sliderSe.value  = SoundMngKR.Inst.GetVolumeSE();
        }
    }

    void Update()
    {
        if (isPush) {
            //アニメーション終了後、次のシーンへ.
            if (animSceneIn.IsFinished())
            {
                NextScene();
            }
        }
    }

    /// <summary>
    /// メニューの開閉.
    /// </summary>
    public void PushMenu()
    {
        isActive = !isActive; //切り替え.

        pnlMenu.SetActive(isActive);
    }
    /// <summary>
    /// タイトルへ戻る.
    /// </summary>
    public void PushToTitle() 
    {
        isPush = true;
        animSceneIn.AnimExe(); //アニメーション実行.
    }
    /// <summary>
    /// 次のシーンへ.
    /// </summary>
    public void NextScene()
    {
        SceneManager.LoadScene("TitleScene");
    }

    /*
       SoundMngKRは、全シーンで使うDontDestroyOnLoadのため
       スライダーからは直接指定せず、別の関数を経由する.
    */

    /// <summary>
    /// BGM音量設定.
    /// スライダーでこの関数を呼ぶ.
    /// </summary>
    public void SetVolumeBGM(float volume)
    {
        if (SoundMngKR.Inst)
        {
            SoundMngKR.Inst.SetVolumeBGM(volume);
        }
    }
    /// <summary>
    /// SE音量設定.
    /// スライダーでこの関数を呼ぶ.
    /// </summary>
    public void SetVolumeSE(float volume)
    {
        if (SoundMngKR.Inst)
        {
            SoundMngKR.Inst.SetVolumeSE(volume);
        }
    }
}