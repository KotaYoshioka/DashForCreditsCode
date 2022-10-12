using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// キャラ選択画面のマネージャー
/// </summary>
public class CharaSelectManager : MonoBehaviour
{
    //他のObjectとの連携
    [SerializeField] Image preview;
    [SerializeField] CharaButton[] buttons;
    [SerializeField] GameObject parret;
    List<GameObject> colors = new List<GameObject>();


    public int direction = 1;
    int excharaid = -1;

    void Start()
    {
        Alter();
    }

    /// <summary>
    /// キャラの変更か向き変更があった時、総合的に描画を更新する処理。
    /// </summary>
    public void Alter()
    {
        //選択ボタン、プレビューを変更する。
        bool first = false;
        if(excharaid == -1)
        {
            excharaid = TimeTableData.charaID;
            first = true;
        }
        foreach (CharaButton button in buttons)
        {
            button.ChangeSelectedColor();
        }
        PreviewUpdate();
        
        //キャラの変更 もしくは 最初の描画でカラーボタンを設置する。
        if(excharaid != TimeTableData.charaID || first)
        {
            excharaid = TimeTableData.charaID;
            SetColorButtons();
        }
    }

    /// <summary>
    /// カラーボタンを設置する処理。
    /// </summary>
    private void SetColorButtons()
    {
        //現在存在するカラーボタンを撤去する。
        foreach (GameObject color in colors)
        {
            GameObject.Destroy(color);
        }
        colors.Clear();
        
        //新たにカラーボタンを設置する。
        for (int i = 0; i < TimeTableData.charaColorAmount[TimeTableData.charaID]; i++)
        {
            GameObject color = Instantiate(Resources.Load<GameObject>("ColorButton"));
            color.transform.parent = parret.transform;
            ColorButton cb = color.GetComponent<ColorButton>();
            cb.SetColorID(i, this);
            colors.Add(color);
        }
    }

    /// <summary>
    /// キャラの変更や回転により、プレビュー画像を変更する処理。
    /// </summary>
    private void PreviewUpdate()
    {
        string spritename = TimeTableData.charadata[TimeTableData.charaID] + TimeTableData.colordata[TimeTableData.colorID];
        preview.sprite = Resources.Load<Sprite>("Character/" + spritename + "/" + spritename + TimeTableData.directiondata[direction]);
    }

    /// <summary>
    /// true：右、false：左に回転させる処理。
    /// </summary>
    /// <param name="right"></param>
    public void Roll(bool right)
    {
        int d = direction;
        if (right)
        {
            switch (d)
            {
                case 0:
                    direction = 2;
                    break;
                case 1:
                    direction = 3;
                    break;
                case 2:
                    direction = 1;
                    break;
                case 3:
                    direction = 0;
                    break;
            }
        }
        else
        {
            switch (d)
            {
                case 0:
                    direction = 3;
                    break;
                case 1:
                    direction = 2;
                    break;
                case 2:
                    direction = 0;
                    break;
                case 3:
                    direction = 1;
                    break;
            }
        }
        PreviewUpdate();
    }
}
