using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// ルーム内の準備完了ボタンに関する処理。
/// </summary>
public class AlreadyButton : MonoBehaviour
{
    [SerializeField]TextMeshProUGUI bg;
    bool nowAlready = false;

    /// <summary>
    /// クリックされた時、「Already」カスタムプロパティに準備完了の是非を更新する。
    /// </summary>
    public void OnClick()
    {
        var hashtable = new ExitGames.Client.Photon.Hashtable();
        nowAlready = !nowAlready;
        hashtable["Already"] = nowAlready;
        PhotonNetwork.LocalPlayer.SetCustomProperties(hashtable);
        UpdateText();
    }

    /// <summary>
    /// 準備完了に合わせてボタンに表示される文字を変える。
    /// </summary>
    void UpdateText()
    {
        bg.text = nowAlready ? "やっぱまだ！" : "準備完了";
    }
}
