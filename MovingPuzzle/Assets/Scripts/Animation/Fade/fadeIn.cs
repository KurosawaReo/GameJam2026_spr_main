using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class fadeIn : MonoBehaviour
{
    Image image;

    public bool isFaded = false;
    public float alpha;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        image = GetComponent<Image>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!isFaded && Input.GetKeyDown(KeyCode.Space))
        {
            isFaded = true;
            FadeIn();
        }
    }

    public void FadeIn()
    {
        StartCoroutine(FadeInCoroutine());
    }

    public IEnumerator FadeInCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.01f);
            image.color = new Color(image.color.r, image.color.g, image.color.b, alpha);
            if (alpha >= 1)
            {
                break;
            }
            alpha += 0.1f;
        }
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene("TimePreview");

    }
}
