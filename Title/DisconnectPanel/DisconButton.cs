using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 切断パネルを閉じるボタン。
/// </summary>
public class DisconButton : MonoBehaviour
{
    [SerializeField] GameObject tab;
    public void OnClick()
    {
        tab.SetActive(false);        
    }
}
