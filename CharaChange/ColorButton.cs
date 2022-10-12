using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// キャラ選択時、カラーを変更するボタン。
/// </summary>
public class ColorButton : MonoBehaviour
{
    int colorid = 0;
    CharaSelectManager manager;

    /// <summary>
    /// ボタンを押された時に、カラーを登録する処理。
    /// </summary>
    public void OnClick()
    {
        TimeTableData.colorID = colorid;
        manager.Alter();
    }

    /// <summary>
    /// カラーIDとマネージャーを登録する初期設定。
    /// </summary>
    /// <param name="colorid"></param>
    /// <param name="manager"></param>
    public void SetColorID(int colorid,CharaSelectManager manager)
    {
        this.colorid = colorid;
        GetComponent<Image>().color = TimeTableData.colors[TimeTableData.charaID, colorid];
        this.manager = manager;
    }
}
