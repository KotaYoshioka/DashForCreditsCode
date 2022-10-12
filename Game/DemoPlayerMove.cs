using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// ゲームで実際に動くプレイヤーの処理
/// </summary>
public class DemoPlayerMove : MonoBehaviourPunCallbacks
{

    Rigidbody2D rigid;
    OnlineGameManager ogm;
    SpriteRenderer sr;

    int charaID = 0;
    int colorID = 0;

    //階段の移動中か
    bool stepmove = false;
    //近くに教室があるか
    bool nearRoom = false;
    //近くの教室の名前
    string nearRoomName = "";
    //授業を受ける
    public bool inClass = false;
    PhotonView pv;
    [SerializeField] TextMeshProUGUI myname;
    [SerializeField] GameObject error;
    GameObject canvas;
   
    void Start()
    {
        rigid = gameObject.GetComponent<Rigidbody2D>();
        pv = GetComponent<PhotonView>();
        canvas = GameObject.Find("Canvas");
        sr = gameObject.GetComponent<SpriteRenderer>();
    }

    
    void Update()
    {
        if (pv.IsMine)
        {
            if (!inClass)
            {
                //WASDキーでの移動
                int x = 0;
                int y = 0;
                int speed = 12;
                //0:なし1:上2:下3:左4:右
                int direction = 0;
                if (Input.GetKey(KeyCode.W))
                {
                    y = speed;
                    direction = 1;
                }
                else if (Input.GetKey(KeyCode.S))
                {
                    y = -speed;
                    direction = 2;
                }
                if (Input.GetKey(KeyCode.A))
                {
                    x = -speed;
                    direction = 3;
                }
                else if (Input.GetKey(KeyCode.D))
                {
                    x = speed;
                    direction = 4;
                }
                if(direction != 0)
                {
                    GetComponent<PhotonView>().RPC(nameof(AsyncPlayerDirection), RpcTarget.All,direction);
                }
                rigid.velocity = new Vector2(x, y);

                //スペースキーで教室に入室
                if (nearRoom && Input.GetKeyDown(KeyCode.Space))
                {
                    //次がここで講義があるかどうか
                    if (!ogm.IsOpenClass(nearRoomName))
                    {
                        SummonError("次の時間、ここで講義はありません");
                        return;
                    }
                    //現在、講義中ではないかどうか
                    if (ogm.GetStudy())
                    {
                        SummonError("講義中です");
                        return;
                    }
                    //履修制限に達していないかどうか
                    if (!ogm.CanJoinOfLimit(nearRoomName))
                    {
                        SummonError("制限に達しています");
                        return;
                    }
                    //入室
                    inClass = true;
                    pv.RPC(nameof(EnterExitRoom), RpcTarget.AllBufferedViaServer, true);
                }
            }
        }
    }


    /// <summary>
    /// 真ん中に赤字で文字を登場させる
    /// </summary>
    void SummonError(string errorm)
    {
        GameObject newerror = Instantiate(error);
        newerror.transform.parent = canvas.transform;
        newerror.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
        newerror.GetComponent<TextMeshProUGUI>().text = errorm;
    }
    /// <summary>
    /// ゲームマネージャーを連携させる。
    /// </summary>
    public void SetManager(OnlineGameManager ogm)
    {
        this.ogm = ogm;
    }


    /// <summary>
    /// 部屋に近づいたときに、名前を表示するようにマネージャーに命令する
    /// </summary>
    public void SendNearRoomName(string roomName)
    {
        if (pv.IsMine)
        {
            nearRoomName = roomName;
            nearRoom = true;
            ogm.DisplaySchoolName(roomName);
        }
    }
    /// <summary>
    /// 部屋から遠のいたため、名前を閉じるようにマネージャーに命令する
    /// </summary>
    public void SendCloseRoom()
    {
        if (pv.IsMine)
        {
            nearRoom = false;
            nearRoomName = "";
            ogm.HideSchoolName();
        }
    }
    /// <summary>
    /// 部屋から抜ける処理
    /// </summary>
    public void ExitRoom()
    {
        if (pv.IsMine)
        {
            pv.RPC(nameof(EnterExitRoom), RpcTarget.AllBufferedViaServer, false);
        }
    }
    /// <summary>
    /// 部屋への抜け入りにより、プレイヤーの見える見えないを切り替える
    /// </summary>
    [PunRPC]
    void EnterExitRoom(bool enter)
    {
        gameObject.SetActive(!enter);
    }


    /// <summary>
    /// 表示されるプレイヤー名を全員に送信する
    /// </summary>
    public void SetName(string playername,int charaid,int colorid)
    {
        GetComponent<PhotonView>().RPC(nameof(AsyncNameAndCharaDatas), RpcTarget.AllBufferedViaServer, playername,charaid,colorid);
    }
    /// <summary>
    /// プレイヤー名と使用キャラ(色)を同期する
    /// </summary>
    [PunRPC]
    void AsyncNameAndCharaDatas(string ownname,int charaid,int colorid)
    {
        myname.text = ownname;
        this.charaID = charaid;
        this.colorID = colorid;
        string spritename = TimeTableData.charadata[charaID] + TimeTableData.colordata[colorID];
        gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Character/" + spritename + "/" + spritename + TimeTableData.directiondata[1]);
    }
    /// <summary>
    /// プレイヤーが現在向いている方向を同期する
    /// </summary>
    [PunRPC]
    void AsyncPlayerDirection(int direction)
    {
        string spritename = TimeTableData.charadata[charaID] + TimeTableData.colordata[colorID];
        sr.sprite = Resources.Load<Sprite>("Character/" + spritename + "/" + spritename + TimeTableData.directiondata[direction-1]);
    }


    /// <summary>
    /// 現在、階段移動中か取得する
    /// </summary>
    public bool GetStepMove()
    {
        return stepmove;
    }
    /// <summary>
    /// 階段移動のクールダウンを発生させる。
    /// </summary>
    public void MoveStep()
    {
        if (pv.IsMine)
        {
            stepmove = true;
            StartCoroutine(nameof(MoveStepReverse));
        }
    }
    /// <summary>
    /// 階段移動のクールダウンを完了させる。
    /// これがない場合、テレポート先の判定に引っかかり、階段を無限ループしてしまう。
    /// </summary>
    IEnumerator MoveStepReverse()
    {
        yield return new WaitForSeconds(0.5f);
        stepmove = false;
    }

    /// <summary>
    /// 現在のフロアの名前をセットする。
    /// </summary>
    public void SetNowPositionName(string positionName,Vector2 loc)
    {
        if (pv.IsMine)
        {
            ogm.SetLocationName(positionName);
            gameObject.GetComponent<Transform>().position = loc;
        }
    }
}
