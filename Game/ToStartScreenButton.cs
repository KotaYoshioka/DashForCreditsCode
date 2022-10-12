using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// ゲームを終わり、スタート画面に戻るボタン
/// </summary>
public class ToStartScreenButton : MonoBehaviourPunCallbacks
{

    public void OnClick()
    {
        SceneManager.LoadScene("Title");
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.Disconnect();
    }
}
