using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 切断が発生した際にパネルを出す処理。
/// </summary>
public class DisconPanel : MonoBehaviour
{
    void Start()
    {
        if (TimeTableData.disc)
        {
            gameObject.SetActive(true);
            TimeTableData.disc = false;
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
