using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomManager : MonoBehaviourPunCallbacks
{
    //関連変数
    [SerializeField] TextMeshProUGUI roomamount,startcount,playerlist;
    [SerializeField] GameObject playerList;
    [SerializeField] GameObject fusenBody;
    [SerializeField] ScreenFadeScript sfs;

    //最小プレイヤー数
    int minPlayer = 2;

    //ルーム内時間
    int reftTime = 30;
    Coroutine countdownCoroutine = null;

    //ルーム内のプレイヤー名
    Dictionary<string, string> playernames = new Dictionary<string, string>();


    void Start()
    {
        //入ってきた人用に更新する
        UpdateRoomCount();

        //自分の名前の設定
        string name = TimeTableData.playername;
        name = RenameAvoidSame();
        var hashtable = new ExitGames.Client.Photon.Hashtable();
        hashtable["Name"] = name;
        PhotonNetwork.LocalPlayer.SetCustomProperties(hashtable);
        
        //ルームの戦闘中の初期化
        ChangeRoomGameStatus(false);
        sfs.SetFade(false);
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.CurrentRoom.MaxPlayers = 12;
        }
    }

    /// <summary>
    /// 同じプレイヤー名を避けるために、「名前(数字)」とする処理。
    /// </summary>
    private string RenameAvoidSame()
    {
        int namecounter = 1;
        string playername = TimeTableData.playername;
        while (HasSameName(playername))
        {
            namecounter++;
            playername = TimeTableData.playername + namecounter;
        }
        TimeTableData.playername = playername;
        return playername;
    }
    /// <summary>
    /// 同じプレイヤー名の人がいるかを判定する処理。
    /// </summary>
    private bool HasSameName(string name)
    {
        foreach (Player p in PhotonNetwork.PlayerList)
        {
            if (PhotonNetwork.LocalPlayer != p)
            {
                if (p.CustomProperties["Name"].Equals(name))
                {
                    return true;
                }
            }
        }
        return false;
    }

    /// <summary>
    /// 部屋の中の人数表示を最新のものに変更する。
    /// </summary>
    void UpdateRoomCount()
    {
        roomamount.text = "ルーム人数：" + PhotonNetwork.CurrentRoom.PlayerCount + "/12人";
    }

    void ChangeRoomGameStatus(bool changed)
    {
        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            var roomhash = new ExitGames.Client.Photon.Hashtable();
            roomhash["NowGame"] = false;
            PhotonNetwork.CurrentRoom.SetCustomProperties(roomhash);
        }
    }

    /// <summary>
    /// 誰かが入ってきた時、画面を更新する処理。
    /// </summary>
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
        //画面を更新する。
        UpdateRoomCount();

        //最少人数を超えて、カウントしてなければ、カウントを開始する。
        if (PhotonNetwork.IsMasterClient)
        {
            if (PhotonNetwork.CurrentRoom.PlayerCount >= minPlayer && countdownCoroutine == null)
            {
                reftTime = 30;
                countdownCoroutine = StartCoroutine(nameof(Countdown));
            }
        }
    }
    /// <summary>
    /// 誰かが抜けた時、画面を更新する処理。
    /// </summary>
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);
        //画面の更新
        UpdateRoomCount();
        UpdatePlayerList();

        //カウントを止める
        if (PhotonNetwork.CurrentRoom.PlayerCount < minPlayer && !startcount.text.Equals(""))
        {
            StopCoroutined();
        }
    }
    /// <summary>
    /// プレイヤー名の変更が発生した際の画面の更新
    /// </summary>
    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        base.OnPlayerPropertiesUpdate(targetPlayer, changedProps);
        UpdatePlayerList();
    }

    /// <summary>
    /// プレイヤー名が書かれた掲示板を更新する処理
    /// </summary>
    void UpdatePlayerList()
    {
        if(playerList != null)
        {
            List<GameObject> children = new List<GameObject>();
            //現在、掲示板に貼ってあるプレイヤー名を全て取得
            for(int i = 0; i < playerList.transform.childCount; i++)
            {
                children.Add(playerList.transform.GetChild(i).gameObject);
            }

            //貼ってあるプレイヤーを全て消す
            foreach(GameObject child in children)
            {
                GameObject.Destroy(child);
            }

            //新たなプレイヤーリストを参照し、掲示板に貼りなおす。
            foreach(Player p in PhotonNetwork.PlayerList)
            {
                GameObject fusen = Instantiate(fusenBody);
                TextMeshProUGUI text = fusen.GetComponentInChildren<TextMeshProUGUI>();
                string newname = p.CustomProperties["Name"].ToString();
                text.text = newname;
                fusen.transform.parent = playerList.transform;
                Vector3 scale = fusen.transform.localScale;
                scale.x = 1;
                scale.y = 1;
                scale.z = 1;
                fusen.transform.localScale = scale;
            }
        }
    }

    /// <summary>
    /// 再帰的に1秒ずつカウントダウンをしていく処理
    /// </summary>
    IEnumerator Countdown()
    {
        yield return new WaitForSeconds(1);

        reftTime--;
        GetComponent<PhotonView>().RPC(nameof(RPCCountdown), RpcTarget.AllBufferedViaServer, reftTime);
        //残り1秒ならゲーム移動
        if (reftTime == 1)
        {
            GetComponent<PhotonView>().RPC(nameof(RPCSceneChange), RpcTarget.AllBufferedViaServer);
            PhotonNetwork.CurrentRoom.IsOpen = false;
        }
        //それ以外はカウントダウン(再帰)
        else
        {
            countdownCoroutine = StartCoroutine(nameof(Countdown));
        }
    }

    /// <summary>
    /// 共有して、残り時間を表示する。
    /// </summary>
    [PunRPC]
    void RPCCountdown(int nr)
    {
        reftTime = nr;
        startcount.text = reftTime.ToString();
    }
    /// <summary>
    /// カウントダウンを止めて、残り30秒にリセットする。
    /// </summary>
    void StopCoroutined()
    {
        if (countdownCoroutine != null)
        {
            StopCoroutine(countdownCoroutine);
        }
        countdownCoroutine = null;
        reftTime = 30;
        startcount.text = "";
        //GetComponent<PhotonView>().RPC(nameof(RPCStopCountdown), RpcTarget.All);
    }

    [PunRPC]
    void RPCStopCountdown()
    {
        sfs.ResetFade();
    }
    /// <summary>
    /// 1秒後、ゲーム画面に飛ばす。
    /// </summary>
    [PunRPC]
    void RPCSceneChange()
    {
        StartCoroutine(nameof(ReadyGo));
    }
    /// <summary>
    /// ゲーム画面に飛ばす。
    /// </summary>
    IEnumerator ReadyGo()
    {
        yield return new WaitForSeconds(1);
        PhotonNetwork.IsMessageQueueRunning = false;
        SceneManager.LoadScene("GameScene");
    }
}