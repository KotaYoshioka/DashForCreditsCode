using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// tipsパネルを表示するボタン。
/// </summary>
public class TipsButton : MonoBehaviour
{
    [SerializeField] GameObject tipsPanel;
    [SerializeField] TextMeshProUGUI tipsContent;

    public void OnClick()
    {
        tipsContent.text = TimeTableData.GetTips();
        tipsPanel.SetActive(true);
    }
}
