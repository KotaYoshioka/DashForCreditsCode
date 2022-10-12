using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// チュートリアル用のプレイヤー操作
/// </summary>
public class TutorialPlayer : MonoBehaviour
{

    SpriteRenderer sr;
    Rigidbody2D rigid;
    bool canMove = false;
    bool canEnter = false;

    //階段の移動中か
    bool stepmove = false;

    bool first = true;
    [SerializeField]TutorialScript ts;

    [SerializeField] GameObject nr;

    [SerializeField] TextMeshProUGUI location;
    //近くに教室があるか
    bool nearRoom = false;
    //近くの教室の名前
    string nearRoomName = "";

    void Start()
    {
        sr = gameObject.GetComponent<SpriteRenderer>();
        rigid = gameObject.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        //WASDによる移動
        if (canMove)
        {
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
            if (direction != 0)
            {
                ChangePlayerDirection(direction);
            }
            rigid.velocity = new Vector2(x, y);
        }

        //スペースキーで部屋に入る。
        if (canEnter)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                if (first && nearRoomName.Equals("303"))
                {
                    first = false;
                    ts.Phase();
                    gameObject.SetActive(false);
                } else if (!first && ts.GetPhase() == 15 && nearRoomName.Equals("210"))
                {
                    ts.Phase();
                    gameObject.SetActive(false);
                }
            }
        }

        //エスケープキーでチュートリアルを抜ける。
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("Title");
        }
    }

    /// <summary>
    /// プレイヤーの向き(スプライト)を変える。
    /// </summary>
    void ChangePlayerDirection(int direction)
    {
        string spritename = TimeTableData.charadata[0] + TimeTableData.colordata[0];
        sr.sprite = Resources.Load<Sprite>("Character/" + spritename + "/" + spritename + TimeTableData.directiondata[direction - 1]);
    }

    /// <summary>
    /// 動けるかどうか
    /// </summary>
    public void MoveEnable(bool enable)
    {
        canMove = enable;        
    }
    /// <summary>
    /// 入れるかどうか
    /// </summary>
    public void EnterEnable(bool enable)
    {
        canEnter = enable;
    }
    /// <summary>
    /// 部屋が遠のいたので部屋名を隠す処理
    /// </summary>
    public void SendCloseRoom()
    {
        nearRoom = false;
        nearRoomName = "";
        nr.SetActive(false);
    }
    /// <summary>
    /// 近くにある部屋の名前を表示する処理
    /// </summary>
    public void SendNearRoomName(string roomName)
    {
        nearRoomName = roomName;
        nearRoom = true;
        nr.GetComponentInChildren<TextMeshProUGUI>().text = nearRoomName + "教室";
        nr.SetActive(true);
        if (ts.GetPhase() == 10 && first && roomName.Equals("303"))
        {
            ts.Phase();
            rigid.velocity = new Vector2(0,0);
        }
    }
    /// <summary>
    /// 近くにある部屋の名前を取得する
    /// </summary>
    public string GetNearRoomName()
    {
        return nearRoomName;
    }
    /// <summary>
    /// 階段の移動中か取得する。
    /// </summary>
    public bool GetStepMove()
    {
        return stepmove;
    }
    /// <summary>
    /// 階段の移動
    /// </summary>
    public void MoveStep()
    {
        stepmove = true;
        StartCoroutine(nameof(MoveStepReverse));
    }
    /// <summary>
    /// 階段の移動クールダウンの解除
    /// これが無い場合、階段移動後、すぐに階段移動してしまう(無限ループ)
    /// </summary>
    IEnumerator MoveStepReverse()
    {
        yield return new WaitForSeconds(0.5f);
        stepmove = false;
    }
    /// <summary>
    /// 現在のフロアの名前を表示する。
    /// </summary>
    public void SetNowPositionName(string positionName, Vector2 loc)
    {
        location.text = positionName;
        gameObject.GetComponent<Transform>().position = loc;
    }
}
