using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OnlineGameManager : MonoBehaviourPunCallbacks
{
    [SerializeField] TextMeshProUGUI time,date;
    [SerializeField] GameObject chaseCamera;
    [SerializeField] GameObject schoolPanel;
    [SerializeField] TextMeshProUGUI timetableText;
    [SerializeField] ScreenFadeScript fader;
    [SerializeField] GameObject fusenBody;
    List<GameObject> fusens = new List<GameObject>();
    [SerializeField] GameObject resultPanel;
    [SerializeField] GameObject finalPanel;
    [SerializeField] TextMeshProUGUI winnerText, finalText, locationText;
    [SerializeField] Scrollbar sbbody;
    PhotonView myPhotonView;
    TextMeshProUGUI schoolPanelText;
    //自分
    GameObject player;
    //自分の名前
    string playername;
    //現在何時
    int hours = 8;
    //現在何分
    int minutes = 40;
    int starttime;
    //現在授業中かどうか
    bool nowstudy = false;
    //現在休み時間かどうか
    bool nowrest = false;
    //現在前期か後期か
    bool first;
    //これまでに使用された曜日
    List<int> exWeek = new List<int>();
    //現在の曜日
    int week;
    //現在の時間配分
    int period = 1;
    //現在、何ゲーム目か
    int gametimes = 0;
    //各授業時間の終わりと始まり
    int[,,] periodTime = {
        { {8,50},{10,20} },
        { {10,30},{12,00} },
        { {12,50},{14,20} },
        { {14,30},{16,00} },
        { {16,10},{17,40} }
    };
    //
    float divid = 1.5f;
    //現在の時間割
    List<object[]> timetable = new List<object[]>();
    //単位
    int score = 0;
    //全員の単位
    Dictionary<string, int> hashscore = new Dictionary<string, int>();
    //1日が終了したかどうか
    bool finishOneday = true;
    //参加人数
    int playersize = 0;
    //現在の時間割
    List<GameObject> timeobjects = new List<GameObject>();

    
    void Start()
    {
        locationText.text = "3号館2階";
        playername = TimeTableData.playername;
        myPhotonView = GetComponent<PhotonView>();
        schoolPanelText = schoolPanel.GetComponentInChildren<TextMeshProUGUI>();
        //通信の解除(同期ズレ対策)
        PhotonNetwork.IsMessageQueueRunning = true;

        ////自分の召喚
        player = PhotonNetwork.Instantiate("DemoPlayer", new Vector3(0, 0, 0), Quaternion.identity, 0);
        player.GetComponent<DemoPlayerMove>().SetManager(this);
        player.GetComponent<DemoPlayerMove>().SetName(playername,TimeTableData.charaID,TimeTableData.colorID);
        chaseCamera.GetComponent<CameraScript>().SetPlayer(player);
        
        SetNewDay();
    }

    /// <summary>
    /// 新たな1日を開始する処理
    /// </summary>
    void SetNewDay()
    {
        //時間割のリセット
        foreach(Object o in timeobjects)
        {
            GameObject.Destroy(o);
        }
        timeobjects.Clear();

        //時間や変数のリセット
        TextMeshProUGUI resultText = resultPanel.GetComponentInChildren<TextMeshProUGUI>();
        resultText.text = "";
        hours = 8;
        minutes = 40;
        period = 1;
        gametimes++;
        hashscore.Clear();
        playersize = PhotonNetwork.CurrentRoom.PlayerCount;

        //時間をマスターに合わせる
        if (PhotonNetwork.IsMasterClient)
        {
            var hashtable = new ExitGames.Client.Photon.Hashtable();
            hashtable["starttime"] = PhotonNetwork.ServerTimestamp;
            PhotonNetwork.CurrentRoom.SetCustomProperties(hashtable);
            //前期か後期かの決定
            bool firstResult = new System.Random().Next(0, 2) == 0;
            do
            {
                week = new System.Random().Next(1, 6);
            } while (exWeek.Contains(week));
            myPhotonView.RPC(nameof(AsyncFirst), RpcTarget.AllBufferedViaServer, firstResult,week);
        }
        starttime = PhotonNetwork.ServerTimestamp;
        finishOneday = false;

        //フェードを明かす
        fader.ToggleFade();
    }
    

    void Update()
    {
        //一定時間、ラグが発生した場合に切断する処理
        if (PhotonNetwork.InRoom)
        {
            if(Time.smoothDeltaTime > 0.4f)
            {
                PhotonNetwork.LeaveRoom();
                PhotonNetwork.Disconnect();
                TimeTableData.disc = true;
                SceneManager.LoadScene("title");
            }
        }

        //1日が終わっている間は動かないようにする
        if (finishOneday)
        {
            return;
        }

        //時間を進める処理
        if(((PhotonNetwork.ServerTimestamp - starttime) / 1000f) > (nowrest?0.1f:(nowstudy?0.1f: 1.5f)))
        {
            //分(及び時)を進める
            minutes++;
            if(minutes >= 60)
            {
                minutes = 0;
                hours++;
            }
            DisplayTime();
            starttime = PhotonNetwork.ServerTimestamp;

            //10分ごとに同期する(ズレ対策)
            if(minutes % 10 == 0)
            {
                /*
                var hashtable = new ExitGames.Client.Photon.Hashtable();
                hashtable["starttime"] = PhotonNetwork.ServerTimestamp;
                PhotonNetwork.CurrentRoom.SetCustomProperties(hashtable);
                */
                if (PhotonNetwork.IsMasterClient)
                {
                    GetComponent<PhotonView>().RPC(nameof(AsyncTime), RpcTarget.AllBufferedViaServer, hours, minutes);
                }
            }
        }
    }


    /// <summary>
    /// 時と分を同期する
    /// </summary>
    [PunRPC]
    void AsyncTime(int h, int m)
    {
        hours = h;
        minutes = m;
        starttime = PhotonNetwork.ServerTimestamp;
        DisplayTime();

        //授業の始まり　もしくは　終わりのタイミングかを判定する
        if (periodTime[period - 1, nowstudy?1:0, 0] == hours)
        {
            if (periodTime[period - 1, nowstudy?1:0, 1] == minutes)
            {
                nowstudy = !nowstudy;
                //授業終わりの場合、次の時間割に移行する
                if (!nowstudy)
                {
                    period++;
                    ChangePeriod();
                    if(period >= 6)
                    {
                        finishOneday = true;
                        fader.ToggleFade();
                        GetComponent<PhotonView>().RPC(nameof(AsyncScore), RpcTarget.AllBufferedViaServer, playername, score);
                        //一定時間後、全員に結果を渡す。
                        if (PhotonNetwork.IsMasterClient)
                        {
                            StartCoroutine(nameof(ResultDelay));
                        }
                    }
                }
            }
        }
        //昼休みの判定
        if (12 == hours)
        {
            if(0 == minutes)
            {
                nowrest = true;
            }else if(40 == minutes)
            {
                nowrest = false;
            }
        }
    }


    /// <summary>
    /// 次の日への時差
    /// </summary>
    IEnumerator NextDayDelay()
    {
        yield return new WaitForSeconds(10);
        GetComponent<PhotonView>().RPC(nameof(GoNextDay),RpcTarget.AllBufferedViaServer);
    }
    /// <summary>
    /// 次の日に行くタイミングの同期
    /// </summary>
    [PunRPC]
    void GoNextDay()
    {
        resultPanel.SetActive(false);
        SetNewDay();
    }
    /// <summary>
    /// スコアの同期
    /// </summary>
    [PunRPC]
    void AsyncScore(string playerna, int sco)
    {
        hashscore.Add(playerna, sco);
    }


    /// <summary>
    /// 時間の表示
    /// </summary>
    void DisplayTime()
    {
        //曜日と前/後期の表示
        StringBuilder sb = new StringBuilder();
        string[] weekname = {"月","火","水","木","金"};
        sb.Append((first?"前期":"後期" )+ " (" + weekname[week - 1] + ")");
        date.text = sb.ToString();
        
        //時間と分の表示
        sb = new StringBuilder();
        if (hours < 10)
        {
            sb.Append(0);
        }
        sb.Append(hours);
        sb.Append("：");
        if (minutes < 10)
        {
            sb.Append(0);
        }
        sb.Append(minutes);
        time.text = sb.ToString();
    }


    /// <summary>
    /// 教室名をパネルで表示する処理
    /// </summary>
    public void DisplaySchoolName(string roomName)
    {
        schoolPanel.SetActive(true);
        schoolPanelText.text = roomName + "教室";
    }
    /// <summary>
    /// 教室名のパネルを隠す処理
    /// </summary>
    public void HideSchoolName()
    {
        schoolPanelText.text = "";
        schoolPanel.SetActive(false);
    }

    /// <summary>
    /// その授業が開講しているかどうかを取得する
    /// </summary>
    public bool IsOpenClass(string roomName)
    {
        foreach(object[] obs in timetable)
        {
            if((int)obs[4] == period && ((string)obs[1]).Equals(roomName))
            {
                return true;
            }
        }
        return false;
    }
    /// <summary>
    /// その授業に履修制限がかかっているか取得する
    /// </summary>
    private bool IsLimitClass(string roomName)
    {
        foreach(object[] obs in timetable)
        {
            if ((int)obs[4] == period && ((string)obs[1]).Equals(roomName))
            {
                return (bool)obs[5];
            }
        }
        return false;
    }

    /// <summary>
    /// 時間割を表示する処理
    /// </summary>
    public void DisplayTimeTable()
    {
        //時間割の長さから、用意するべきパネルのサイズを設定
        GameObject viewPanel = GameObject.Find("Canvas").transform.Find("TimeTable").transform.Find("ViewPanel").transform.Find("RowPanel").gameObject;
        int amount = 0;
        for (int i = 1; i <= 5; i++)
        {
            amount++;
            foreach (object[] obs in timetable)
            {
                if ((int)obs[4] == i)
                {
                    amount++;
                }
            }
        }
        Vector2 dl = viewPanel.GetComponent<RectTransform>().sizeDelta;
        dl.y = amount * 52;
        viewPanel.GetComponent<RectTransform>().sizeDelta = dl;

        //カラー時間割
        byte[,] timecolor =
        {
            {255,95,113},
            {255,250,59},
            {53,255,87},
            {35,246,255},
            {166,82,255}
        };
        byte[,] timecontent =
        {
            {255,233,238},
            {255,255,182},
            {210,252,215},
            {204,255,254},
            {223,202,252}
        };

        //1時限目から5時限目まで順番ずつ時間割を形成していく
        for (int i = 1; i <= 5; i++)
        {
            //「〇時限目」の付箋を形成する
            GameObject row = Instantiate(Resources.Load<GameObject>("TimeTableRow"));
            row.GetComponent<Image>().color = new Color32(timecolor[i-1,0],timecolor[i-1,1],timecolor[i-1,2],125);
            row.transform.parent = viewPanel.transform;
            row.GetComponentInChildren<TextMeshProUGUI>().text = i +"時限目";
            Vector3 scale = row.transform.localScale;
            scale.x = 1;
            scale.y = 1;
            scale.z = 1;
            row.transform.localScale = scale;
            timeobjects.Add(row);
            amount++;

            //その時限に開講される講義の名前が付された付箋を形成する
            foreach(object[] obs in timetable)
            {
                if ((int)obs[4] == i)
                {
                    GameObject timerow = Instantiate(Resources.Load<GameObject>("TimeTableRow"));
                    timerow.transform.parent = viewPanel.transform;
                    timerow.GetComponentInChildren<TextMeshProUGUI>().text = ((bool)obs[5] ? "[限]" : "") + (string)obs[0] + "：" + (string)obs[1];
                    timerow.GetComponent<Image>().color = new Color32(timecontent[i - 1, 0], timecontent[i - 1, 1], timecontent[i - 1, 2], 125);
                    timeobjects.Add(timerow);
                    Vector3 scale2 = timerow.transform.localScale;
                    scale2.x = 1;
                    scale2.y = 1;
                    scale2.z = 1;
                    timerow.transform.localScale = scale2;
                    amount++;
                }
            }
        }
        StartCoroutine(nameof(SetValue));
    }
    /// <summary>
    /// スクロールバーの位置を初期値にする
    /// 遅延無しで行うと、なぜか設定されない
    /// </summary>
    IEnumerator SetValue()
    {
        yield return new WaitForSeconds(0.5f);
        sbbody.value = 1;
    }
    /// <summary>
    /// 時限の変わり目の処理
    /// プレイヤーが授業を受けていた場合、教室から出す。
    /// </summary>
    void ChangePeriod()
    {
        if (player.GetComponent<DemoPlayerMove>().inClass)
        {
            DemoPlayerMove dpm = player.GetComponent<DemoPlayerMove>();
            dpm.ExitRoom();
            dpm.inClass = false;
            score = score + 2;
        }
        PhotonNetwork.CurrentRoom.CustomProperties.Clear();

    }


    //初期同期
    [PunRPC]
    void AsyncFirst(bool first, int week)
    {
        this.first = first;
        this.week = week;
        exWeek.Add(week);
        DisplayTime();
        if (PhotonNetwork.IsMasterClient)
        {
            //時間割の同期
            int[] timetableIndexs = TimeTableData.GetTimeTableForAWeek(first, week);
            GetComponent<PhotonView>().RPC(nameof(AsyncTimetable), RpcTarget.AllBufferedViaServer, timetableIndexs);
        }
    }
    //時間割同期
    [PunRPC]
    void AsyncTimetable(int[] timetableIndexs)
    {
        timetable.Clear();
        foreach (int i in timetableIndexs)
        {
            List<object> obs = new List<object>();
            for (int k = 0; k < 6; k++)
            {
                obs.Add(TimeTableData.timetable[i, k]);
            }
            object[] data = obs.ToArray();
            timetable.Add(data);
        }
        DisplayTimeTable();
    }


    [PunRPC]
    void AddSize()
    {
        playersize++;
    }

    /// <summary>
    /// 現在講義時間か否か
    /// </summary>
    public bool GetStudy()
    {
        return nowstudy;
    }
    /// <summary>
    /// その講義は履修制限的に入れるか
    /// </summary>
    public bool CanJoinOfLimit(string roomname)
    {
        if (IsLimitClass(roomname))
        {
            int nowam = (PhotonNetwork.CurrentRoom.CustomProperties[roomname] is int value) ? value : 0;
            if (nowam >= playersize / 2d)
            {
                return false;
            }
            else
            {
                var hashtable = new ExitGames.Client.Photon.Hashtable();
                hashtable[roomname] = nowam + 1;
                PhotonNetwork.CurrentRoom.SetCustomProperties(hashtable);
                return true;
            }
        }
        return true;
    }
    /// <summary>
    /// フロアの名前を表示する
    /// </summary>
    public void SetLocationName(string locationName)
    {
        locationText.text = locationName;
    }

    //Result関連

    /// <summary>
    /// 結果発表までの遅延
    /// </summary>
    /// <returns></returns>
    IEnumerator ResultDelay()
    {
        yield return new WaitForSeconds(4);
        //3回目の結果発表の場合、最終結果発表に
        //それ以外の場合は通常の結果発表に飛ばす
        if (gametimes == 2)
        {
            GetComponent<PhotonView>().RPC(nameof(FinalResult), RpcTarget.AllBufferedViaServer);
        }
        else
        {
            GetComponent<PhotonView>().RPC(nameof(DisplayResult), RpcTarget.AllBufferedViaServer);
            if (PhotonNetwork.IsMasterClient)
            {
                StartCoroutine(nameof(NextDayDelay));
            }
        }
    }

    /// <summary>
    /// 中間結果発表
    /// </summary>
    [PunRPC]
    void DisplayResult()
    {
        foreach (GameObject fus in fusens)
        {
            GameObject.Destroy(fus);
        }
        fusens.Clear();
        //TextMeshProUGUI resultText = resultPanel.GetComponentInChildren<TextMeshProUGUI>();
        int maxscore = 0;
        foreach (int onescore in hashscore.Values)
        {
            if (maxscore < onescore)
            {
                maxscore = onescore;
            }
        }
        GameObject panel = resultPanel.transform.Find("ResultBoard").transform.Find("Panel").gameObject;
        TextMeshProUGUI title = resultPanel.transform.Find("ResultBoard").transform.Find("ResultTitle").GetComponent<TextMeshProUGUI>();
        title.text = "";
        int counter = 1;
        for (int i = maxscore; i >= 0; i--)
        {
            bool dis = false;
            int firstCounter = 0;
            string chooseone = "";
            foreach (string onename in hashscore.Keys)
            {
                if (hashscore[onename] == i)
                {
                    GameObject fusen = Instantiate(fusenBody);
                    fusen.transform.parent = panel.transform;
                    fusen.SetActive(false);
                    fusens.Add(fusen);
                    Vector3 scale = fusen.transform.localScale;
                    scale.x = 1;
                    scale.y = 1;
                    scale.z = 1;
                    fusen.transform.localScale = scale;
                    StartCoroutine(DisplayName(0.3f * counter, fusen));
                    TextMeshProUGUI tx = fusen.GetComponentInChildren<TextMeshProUGUI>();
                    tx.text = counter + "位 " + onename + "\n単位数：" + i;
                    chooseone = onename;
                    dis = true;
                    firstCounter++;
                }
            }
            if (counter == 1)
            {
                if (firstCounter == 1)
                {
                    StartCoroutine(Title(1.6f, chooseone + "が1位で独走中！"));
                }
                else if (firstCounter > 1)
                {
                    StartCoroutine(Title(1.6f, "1位が" + firstCounter + "人、並んでいるぞ！"));
                }
            }
            if (dis)
            {
                counter++;
            }
        }
        resultPanel.SetActive(true);
    }
    /// <summary>
    /// 結果を少しだけ遅らせる処理
    /// </summary>
    IEnumerator Title(float delay, string text)
    {
        yield return new WaitForSeconds(delay);
        TextMeshProUGUI title = resultPanel.transform.Find("ResultBoard").transform.Find("ResultTitle").GetComponent<TextMeshProUGUI>();
        title.text = text;
    }
    /// <summary>
    /// 掲示板に名前が張られていくのを、ちょっと遅めにやる処理
    /// </summary>
    IEnumerator DisplayName(float delay, GameObject fs)
    {
        yield return new WaitForSeconds(delay);
        fs.SetActive(true);
    }

    /// <summary>
    /// 最終結果発表をする処理。
    /// </summary>
    [PunRPC]
    void FinalResult()
    {
        //最高スコアの特定
        int maxscore = 0;
        foreach (int onescore in hashscore.Values)
        {
            if (maxscore < onescore)
            {
                maxscore = onescore;
            }
        }
        /*
        for (int i = maxscore; i >= 0; i--)
        {
            foreach (string onename in hashscore.Keys)
            {
                if (hashscore[onename] == i)
                {
                    if(i == maxscore)
                    {
                        if (winner.Equals(""))
                        {
                            winner = onename;
                        }
                        else
                        {
                            winner = winner + "," + onename;
                        }
                        
                    }
                    finalText.text = finalText.text + onename + "：" + i + "\n";
                }
            }
        }
        winnerText.text = winner + "の優勝！";
        */

        //プレイヤーのランキング掲示板の作成処理
        GameObject panel = finalPanel.transform.Find("Panel").gameObject;
        int counter = 1;
        for (int i = maxscore; i >= 0; i--)
        {
            bool dis = false;
            int firstCounter = 0;
            List<string> winnernames = new List<string>();
            foreach (string onename in hashscore.Keys)
            {
                if (hashscore[onename] == i)
                {
                    GameObject fusen = Instantiate(fusenBody);
                    fusen.transform.parent = panel.transform;
                    fusen.SetActive(false);
                    fusens.Add(fusen);
                    Vector3 scale = fusen.transform.localScale;
                    scale.x = 1;
                    scale.y = 1;
                    scale.z = 1;
                    fusen.transform.localScale = scale;
                    StartCoroutine(DisplayName(0.3f * counter, fusen));
                    TextMeshProUGUI tx = fusen.GetComponentInChildren<TextMeshProUGUI>();
                    tx.text = counter + "位 " + onename + "\n単位数：" + i;
                    winnernames.Add(onename);
                    dis = true;
                    firstCounter++;
                }
            }
            if (counter == 1)
            {
                if (firstCounter >= 1)
                {
                    StringBuilder sb = new StringBuilder();
                    foreach (string s in winnernames)
                    {
                        if (sb.ToString().Length > 0)
                        {
                            sb.Append(",");
                        }
                        sb.Append(s);
                    }
                    sb.Append("の優勝！");
                    StartCoroutine(TitleFinal(1.6f, sb.ToString()));
                }
            }
            if (dis)
            {
                counter++;
            }
        }
        finalPanel.SetActive(true);
    }
    /// <summary>
    /// 時差を付けて、勝者を表示する。
    /// </summary>
    IEnumerator TitleFinal(float delay, string text)
    {
        yield return new WaitForSeconds(delay);
        winnerText.text = text;
    }
}

