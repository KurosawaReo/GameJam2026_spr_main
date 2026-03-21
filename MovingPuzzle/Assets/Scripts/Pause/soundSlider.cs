using UnityEngine;

public class soundSlider : MonoBehaviour
{
    soundManager soundmanager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        soundmanager = FindFirstObjectByType<soundManager>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void BGMVolume(float volume)
    {
        soundmanager.BGMVolume(volume);
        
    }
    public void SEVolume(float volume)
    {
        soundmanager.SEVolume(volume);
        
    }

}

