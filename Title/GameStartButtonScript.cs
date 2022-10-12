using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStartButtonScript : MonoBehaviourPunCallbacks
{
    [SerializeField] TextMeshProUGUI inputField;
    [SerializeField] GameObject loadingPanel;
    [SerializeField] GameObject noNameError;

    /// <summary>
    /// ボタンを押した時、名前を確認した後、ネットに接続させる処理。
    /// 名前が違反してる場合、エラーメッセージを出す。
    /// </summary>
    public void OnClick()
    {
        //名前が2文字以上13文字以内か
        if(inputField.text.Length > 1 && inputField.text.Length < 14)
        {
            TimeTableData.playername = inputField.text;
            PhotonNetwork.ConnectUsingSettings();
            loadingPanel.SetActive(true);
        }
        else
        {
            GameObject insError = Instantiate(noNameError);
            //insError.transform.localPosition = new Vector2(0, 0);
        }

    }

    //ネットに接続後、ルームにマッチングさせる。
    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinRandomOrCreateRoom();
        
    }
    public override void OnJoinedRoom()
    {
        SceneManager.LoadScene("RoomScene");
    }
}
