using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    // タイマー表示用
    [SerializeField] Text timeText;

    // 制限時間
    public int time;

    public bool timerStart = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // 制限時間(time)の表示(timeText)を更新
        timeText.text = "制限時間 : " + time.ToString("d1");
        
    }

    // Update is called once per frame
    void Update()
    {
        if (timerStart)
        {
            // タイマー(timeCoroutine)を開始
            StartCoroutine(timeCoroutine());
            Debug.Log("タイマー開始");
            Debug.Log("現在の制限時間:" + time);
            timerStart = false;
        }
    }

    // タイマー
    public IEnumerator timeCoroutine()
    {
        while (true)
        {
            // 1秒毎に実行
            yield return new WaitForSeconds(1);
            // 制限時間(time)を1ずつ減らす
            time--;
            // 制限時間(time)の表示(timeText)を更新
            timeText.text = "制限時間 : " + time.ToString("d1");
            // 制限時間(time)が0か0以下の場合
            if (time <= 0)
            {
                // 表示が-1などにならないように制限時間(time)を0にする
                time = 0;
                Debug.Log("現在の制限時間:" + time);
                Debug.Log("タイマー停止");
                // 2秒後実行
                yield return new WaitForSeconds(2);
                // トラックのシーンに切り替え(Debug.LogをLoadSceneに変更)
                Debug.Log("ここでシーン切り替え(Debug.LogをLoadSceneに変更)");
                // ループを抜ける
                break;
            }
            Debug.Log("現在の制限時間:" + time);
        }
    }
}
