using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class fadeOut : MonoBehaviour
{
    Image image;

    public float alpha;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        image = GetComponent<Image>();
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

    }
}
