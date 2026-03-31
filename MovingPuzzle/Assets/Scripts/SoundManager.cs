using UnityEngine;
using KR.Unity.Sound;

public class SoundManager : SoundMngKR
{
    private void Start()
    {
        InitSoundMngKR();
        PlayBGM("BGM1", true);
    }
}
