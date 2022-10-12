using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RandomBattleButton : MonoBehaviourPunCallbacks
{
    [SerializeField] TextMeshProUGUI text;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClick()
    {
        if(text.text.Length > 1)
        {
            TimeTableData.playername = text.text;
            PhotonNetwork.JoinRandomOrCreateRoom();
        }
        //TODO 連続使用防止
    }

    public override void OnJoinedRoom()
    {
        SceneManager.LoadScene("RoomScene");
    }
}
