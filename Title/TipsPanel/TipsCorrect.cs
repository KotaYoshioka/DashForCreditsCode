using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// tipsパネルを閉じるだけの処理。
/// </summary>
public class TipsCorrect : MonoBehaviour
{
    [SerializeField] GameObject panel;
    public void OnClick()
    {
        panel.SetActive(false);
    }
}
