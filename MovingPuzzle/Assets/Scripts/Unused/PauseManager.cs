using UnityEngine;

public class PauseManager : MonoBehaviour
{
    Timer timeManager;

    [SerializeField] GameObject PausePanel;

    public bool isPause;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        timeManager = FindFirstObjectByType<Timer>();
        PausePanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (timeManager != null)
        {
            if (timeManager.time <= 0)
            {
                isPause = false;
                PausePanel.SetActive(false);
            }
        }
    }

    public void PressPause()
    {
        if (timeManager.time >= 1)
        {
            if (isPause)
            {
                isPause = false;
                Time.timeScale = 1;
                PausePanel.SetActive(false);
            }
            else
            {
                isPause = true;
                Time.timeScale = 0;
                PausePanel.SetActive(true);
            }
        }
        
    }
}
