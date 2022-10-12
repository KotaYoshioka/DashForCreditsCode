using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// チュートリアル用のフェードイン。
/// </summary>
public class FadeTutorial : MonoBehaviour
{
    [SerializeField] Image image;
    [SerializeField] TutorialScript ts;
    bool fade = false;
    int a = 0;
 
    /// <summary>
    /// fadeがtrueになり次第、フェードを明かす。
    /// 明かし次第、チュートリアルが次に進められる。
    /// </summary>
    void Update()
    {
        if (fade)
        {
            float f = 1 - a * 0.002f;
            image.color = new Color(0, 0, 0, f);
            a++;
            if(f <= 0)
            {
                fade = false;
                ts.OKNext();
                gameObject.SetActive(false);
            }
        }
    }
    /// <summary>
    /// フェードを明かす。
    /// </summary>
    public void Fade()
    {
        fade = true;
    }
}
