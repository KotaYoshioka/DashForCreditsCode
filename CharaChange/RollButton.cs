using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プレビューのキャラを回転させるボタン。
/// </summary>
public class RollButton : MonoBehaviour
{
    
    [SerializeField] bool right;
    [SerializeField] CharaSelectManager csm;

    /// <summary>
    /// ボタンを押したら、回転させる処理。
    /// </summary>
    public void OnClick()
    {
        csm.Roll(right);
    }
}
