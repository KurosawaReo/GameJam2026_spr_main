using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class fadeOutCount : MonoBehaviour
{
    timer timemanager;

    [SerializeField] Text countText;

    Image image;

    public float alpha;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        timemanager = FindFirstObjectByType<timer>();
        image = GetComponent<Image>();
        countText.text = "";
        FadeOut();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void FadeOut()
    {
        StartCoroutine(FadeOutCoroutine());
    }

    public IEnumerator FadeOutCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.01f);
            image.color = new Color(image.color.r, image.color.g, image.color.b, alpha);
            if (alpha <= 0)
            {
                break;
            }
            alpha -= 0.1f;
        }
        yield return new WaitForSeconds(0.5f);
        countText.text = "3";
        yield return new WaitForSeconds(1);
        countText.text = "2";
        yield return new WaitForSeconds(1);
        countText.text = "1";
        yield return new WaitForSeconds(1);
        countText.text = "ŠJŽn";
        yield return new WaitForSeconds(1);
        countText.text = "";
        timemanager.timerStart = true;
        Destroy(gameObject);

    }
}
