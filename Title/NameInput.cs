using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// プレイヤーネームを記入できる場所のスクリプト。
/// </summary>
public class NameInput : MonoBehaviour
{
    [SerializeField] TMP_InputField inpuf;

    /// <summary>
    /// プレイヤーネームが既に登録されている場合、初期設定する。
    /// </summary>
    void Start()
    {
        if(TimeTableData.playername != null && !TimeTableData.playername.Equals(""))
        {
            inpuf.text = TimeTableData.playername;
        }
    }

    /// <summary>
    /// プレイヤーネームが登録されている場合、データに書き写す。
    /// </summary>
    private void OnDestroy()
    {
        string name = null;
        if (inpuf.text.Length > 1)
        {
            name = inpuf.text;
        }
        TimeTableData.playername = name;
    }
}
