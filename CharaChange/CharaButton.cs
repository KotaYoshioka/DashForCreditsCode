using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// キャラのアイコンのボタン機能
/// クリックされたら選択される仕組みに関して
/// </summary>
public class CharaButton : MonoBehaviour
{
    //ボタンのキャラを表すID、外部から書き換え可能
    [SerializeField] int charaid;
    [SerializeField] CharaSelectManager csm;
    
    /// <summary>
    /// クリックされた時の処理。
    /// 選択しているキャラのIDを登録する。
    /// それと同時に、カラーを0(通常色)に、向きを正面に設定する。
    /// </summary>
    public void OnClick()
    {
        TimeTableData.charaID = charaid;
        TimeTableData.colorID = 0;
        csm.direction = 1;
        csm.Alter();
    }

    /// <summary>
    /// 選択時は灰色に。
    /// 未選択時は通常色に。
    /// </summary>
    public void ChangeSelectedColor()
    {
        if(charaid == TimeTableData.charaID)
        {
            GetComponent<Image>().color = new Color(180/255f, 180/255f, 180/255f);
        }
        else
        {
            GetComponent<Image>().color = new Color(1,1,1);
        }
    }
}
