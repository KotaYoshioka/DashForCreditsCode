using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 退出ボタンの処理。
/// 部屋を出て、そのままタイトルまで戻る。
/// </summary>
public class RoomLeaveButton : MonoBehaviour
{
    public void OnClick()
    {
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.Disconnect();
        SceneManager.LoadScene("title");
    }
}
