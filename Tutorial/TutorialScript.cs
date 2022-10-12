using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// チュートリアル全般を担当する
/// </summary>
public class TutorialScript : MonoBehaviour
{
    [SerializeField] GameObject panel,second;
    bool firstpanel = true;
    [SerializeField] GameObject nextMessage;
    [SerializeField] TextMeshProUGUI text,time;
    int hour = 8;
    int minutes = 40;
    [SerializeField] FadeTutorial fade;
    [SerializeField] TutorialPlayer player;
    int phase = 0;
    public bool next = false;
    
    void Start()
    {
        Phase();
    }

    /// <summary>
    /// phaseが進むにつれて自動でチュートリアルが表示される。
    /// </summary>
    public void Phase()
    {
        phase++;
        switch (phase)
        {
            case 1:
                Message("小さなゼミ室から生まれた樽商密着系オンライン単位取得ゲーム「単位奪取」のチュートリアルへようこそ。",true);
                break;
            case 2:
                Message("ここでは、絶妙な操作性を誇る「単位奪取」の遊び方と操作感に慣れてもらうよ。",true);
                break;
            case 3:
                Message("面倒になったらいつでもEscキーからチュートリアルはやめられるからね。",true);
                break;
            case 4:
                Message("それじゃぁ、さっそくゲーム画面に移るよ。",false);
                fade.Fade();//フェイドイン
                break;
            case 5:
                Message("ここは、3号館3階。そして、真ん中にいるのが君だ。可愛いでしょ。",true);
                break;
            case 6:
                Message("現在地は、右上のパネルにも書いてあるよ。",true);
                break;
            case 7:
                Message("Wキーで上、Sキーで下、Aキーで左、Dキーで右に移動できるよ。自由に動き回ってみよう。",true);
                player.MoveEnable(true);
                break;
            case 8:
                Toggle();
                Message("次に左下に注目。ここはゲーム内の時間と前期/後期、そして曜日を表しているよ。ぶっちゃけ、大事なのは時間だけ。",true);
                break;
            case 9:
                Toggle();
                Message("そして左上に注目。時間割だね。作者考案オリジナル授業が書かれているよ。独創的だね。マウスのドラッグで動かせるよ。操作性はいまいちだけど重要だから、いっぱい動かしてみよう。", true);
                break;
            case 10:
                if (player.GetNearRoomName().Equals("303"))
                {
                    Phase();
                }
                else
                {
                    Message("さて、1時間目の授業は303教室で行われるみたいだ。303教室を探してみよう。", false);
                }
                break;
            case 11:
                player.MoveEnable(false);
                player.EnterEnable(true);
                Message("よく見つけたね！そしたら、スペースキーで入室出来るよ。", false);
                break;
            case 12:
                player.MoveEnable(true);
                Message("これで入室完了！姿が消えただけじゃないよ！ちゃんと入室したんだよ！",true);
                break;
            case 13:
                Toggle();
                StartCoroutine(TimePassing(10, 20));
                Message("この後、授業が終わるまでは拘束されるからちょっと待ってね。",false);
                break;
            case 14:
                player.gameObject.SetActive(true);
                Message("1時間目が終わったね、お疲れ様。これで2単位獲得となるよ。「単位奪取」は、これを繰り返して一番単位を持ってるプレイヤーが勝ちってゲームなんだ。独自性で満ち溢れてるよね。", true);
                break;
            case 15:
                Toggle();
                Message("それじゃ、最後にこれまでの説明がちゃんと出来てるかテストするよ。2時限目に開講となる「操作確認学」の単位を獲得してみよう。辿りつけるかな～？", false);
                break;
            case 16:
                Message("OK！これで「単位奪取」の操作は完璧だね。あとはオンラインで皆と実力を競おう！これで「単位奪取」のチュートリアルを終えるよ、お疲れ様でした！",true);
                break;
            case 17:
                SceneManager.LoadScene("Title");
                break;
        }
    }

    /// <summary>
    /// ゲーム内時間を進める処理
    /// </summary>
    IEnumerator TimePassing(int dhour,int dminute)
    {
        yield return new WaitForSeconds(0.1f);
        minutes++;
        if(minutes >= 60)
        {
            minutes = 0;
            hour++;
        }
        if(minutes != dminute || hour != dhour)
        {
            StartCoroutine(TimePassing(dhour, dminute));
        }
        else
        {
            if(phase < 15)
            {
                Phase();
            }
        }
        StringBuilder sb = new StringBuilder();
        if(hour < 10)
        {
            sb.Append("0");
        }
        sb.Append(hour);
        sb.Append(":");
        if(minutes < 10)
        {
            sb.Append("0");
        }
        sb.Append(minutes);
        time.text = sb.ToString();
    }

    public void OnClick()
    {
        if (next)
        {
            Phase();
        }
    }

    /// <summary>
    /// 指定されたメッセージを表示する。
    /// </summary>
    /// <param name="s"></param>
    /// <param name="clickNext">次のフェーズに行く条件がクリックの場合</param>
    public void Message(string s,bool clickNext)
    {
        next = false;
        nextMessage.SetActive(false);
        text.text = "";
        char[] cs = s.ToCharArray();
        int counter = 1;
        foreach(char c in cs)
        {
            StartCoroutine(DisplayMessage(counter, c,phase));
            counter++;
        }
        if (clickNext)
        {
            StartCoroutine(Clear(counter));
        }
    }

    /// <summary>
    /// 文字が順番ずつ出てくる処理
    /// </summary>
    IEnumerator DisplayMessage(int counter,char c,int nowphase)
    {
        yield return new WaitForSeconds(0.1f * counter);
        if (phase == nowphase)
        {
            text.text += c;
        }
    }
    /// <summary>
    /// 文字が出来った後、次に行って良いと許可を出す処理
    /// </summary>
    IEnumerator Clear(int counter)
    {
        yield return new WaitForSeconds(0.1f * counter);
        OKNext();
    }
    /// <summary>
    /// クリックしたら次に行けることを示す処理
    /// </summary>
    public void OKNext()
    {
        next = true;
        nextMessage.SetActive(true);
    }
    /// <summary>
    /// テキストウィンドウの位置を上と下で切り替える処理
    /// </summary>
    public void Toggle()
    {
        if (firstpanel)
        {
            firstpanel = false;
            panel.SetActive(false);
            second.SetActive(true);
            text = second.transform.Find("Message").GetComponent<TextMeshProUGUI>();
            nextMessage = second.transform.Find("Next").gameObject;
        }
        else
        {
            firstpanel = true;
            panel.SetActive(true);
            second.SetActive(false);
            text = panel.transform.Find("Message").GetComponent<TextMeshProUGUI>();
            nextMessage = panel.transform.Find("Next").gameObject;
        }
    }
    /// <summary>
    /// 現在のチュートリアルphaseを取得する
    /// </summary>
    public int GetPhase()
    {
        return phase;
    }
}
