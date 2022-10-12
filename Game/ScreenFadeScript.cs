using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// フェードする処理
/// </summary>
public class ScreenFadeScript : MonoBehaviour
{
    Image img;
    bool fade = true;
    bool move = false;
    float speed = 0.005f;
    float alpha = 1;

    private void Start()
    {
        this.img = GetComponent<Image>();
    }

    /// <summary>
    /// フェードの処理
    /// </summary>
    private void FixedUpdate()
    {
        if (move)
        {
            if (fade)
            {
                FadeIn();
            }
            else
            {
                FadeOut();
            }
        }
    }
    /// <summary>
    /// フェードの切り替え
    /// </summary>
    public void ToggleFade()
    {
        move = true;
        if (!fade)
        {
            gameObject.SetActive(true);
        }
    }
    /// <summary>
    /// フェードを完了しきる処理
    /// </summary>
    public void SetFade(bool fade)
    {
        this.fade = fade;
        if (fade)
        {
            img.color = new Color(0, 0, 0, 1);
            alpha = 1;
        }
        else
        {
            img.color = new Color(0, 0, 0, 0);
            alpha = 0;
        }
    }
    /// <summary>
    /// 消える速度を指定してフェードする処理
    /// </summary>
    public void ToggleFade(float newspeed)
    {
        move = true;
        speed = newspeed;
    }
    /// <summary>
    /// フェードをなかったことにする処理
    /// </summary>
    public void ResetFade()
    {
        move = false;
        img.color = new Color(0, 0, 0, fade?1:0);
        alpha = fade ? 1 : 0;
    }
    /// <summary>
    /// フェードインする処理
    /// </summary>
    void FadeIn()
    {
        alpha = alpha - speed;
        if(alpha <= 0)
        {
            alpha = 0;
            move = false;
            fade = false;
            gameObject.SetActive(false);
        }
        img.color = new Color(0, 0, 0, alpha);
    }
    /// <summary>
    /// フェードアウトする処理
    /// </summary>
    void FadeOut()
    {
        alpha = alpha + speed;
        if(alpha >= 1)
        {
            alpha = 1;
            move = false;
            fade = true;
        }
        img.color = new Color(0, 0, 0, alpha);
    }
}
