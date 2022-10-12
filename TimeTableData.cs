using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeTableData : MonoBehaviour
{

    public static string playername;

    public static int charaID = 0;
    public static int colorID = 0;

    public static bool disc = false;

    //キャラID
    public static string[] charadata = {"Girl","Boy","NOI","EMA","SAI"};
    //各キャラの色数
    public static int[] charaColorAmount = {3,3,1,1,1};
    //各キャラの色
    public static Color[,] colors = { 
        { new Color(115/255f, 66/255f, 41/255f),new Color(0,0,0),new Color(211/255f,88/255f,137/255f)},
        { new Color(0,0,0),new Color(142/255f,102/255f,79/255f),new Color(82/255f,94/255f,66/255f)},
        { new Color(185/255f,251/255f,1),new Color(0,0,0),new Color(0,0,0)},
        { new Color(185/255f,251/255f,1),new Color(0,0,0),new Color(0,0,0)},
        { new Color(185/255f,251/255f,1),new Color(0,0,0),new Color(0,0,0)}
    };
    public static string[] colordata = {"A","B","C"};
    public static string[] directiondata = {"Back","Front","Left","Right"};


    //[0]科目名
    //[1]講義室
    //[2]前期or後期 ( trueが前期)
    //[3]曜日(月1~金5)
    //[4]時限
    //[5]履修制限があるかどうか
    public static object[,] timetable = 
    {
        //////前期
        ////月曜日
        //1時限目
        {"吉岡学","210",true,1,1,true},
        {"ガラス学","303",true,1,1,false},
        //2時限目
        {"OUC学","303",true,1,2,true},
        {"餅もち論","413",true,1,2,true},
        //3時限目
        {"時短術","413",true,1,3,true},
        {"通貨入門","210",true,1,3,false},
        //4時限目
        {"バンド学","308",true,1,4,true},
        {"謝罪入門","213",true,1,4,true},
        //5時限目
        {"VTuber学","214",true,1,5,true},
        {"岡崎体育論","401",true,1,5,false},
        ////火曜日
        //1時限目
        {"藤原学","406",true,2,1,true},
        {"基礎通販","214",true,2,1,false},
        //2時限目
        {"木村史","401",true,2,2,true},
        {"応用通販","301",true,2,2,false},
        //3時限目
        {"eスポーツ科学","307",true,2,3,true},
        {"アボカド入門","407",true,2,3,true},
        //4時限目
        {"占い学","301",true,2,4,true},
        {"木村入門","308",true,2,4,true},
        //5時限目
        {"ピクサーのひみつ","212",true,2,5,true},
        {"動物入門","307",true,2,5,false},
        ////水曜日
        //1時限目
        {"佐藤学","211",true,3,1,true},
        {"自由入門","406",true,3,1,true},
        //2時限目
        {"アイドル学","214",true,3,2,true},
        {"基礎自由","211",true,3,2,true},
        //3時限目
        {"Youtube史","308",true,3,3,true},
        {"応用自由","307",true,3,3,false},
        //4時限目
        {"戦争反対運動","406",true,3,4,true},
        {"スマホ史","401",true,3,4,true},
        //5時限目
        {"音楽史","307",true,3,5,true},
        {"漫画学","305",true,3,5,false},
        ////木曜日
        //1時限目
        {"伊賀学","214",true,4,1,true},
        {"お菓子入門","212",true,4,1,false},
        //2時限目
        {"アニメ史","307",true,4,2,true},
        {"チョコデザイン論","413",true,4,2,true},
        //3時限目
        {"ポケモン学","308",true,4,3,false},
        {"空想科学","213",true,4,3,true},
        //4時限目
        {"OUCのヒミツ","211",true,4,4,true},
        {"独学","308",true,4,4,false},
        //5時限目
        {"お手玉の会","212",true,4,5,true},
        {"サングラス入門","210",true,4,5,true},
        ////金曜日
        //1時限目
        {"佐久間学","406",true,5,1,true},
        {"基礎日本円","413",true,5,1,true},
        //2時限目
        {"料理学","307",true,5,2,true},
        {"インテリア論","305",true,5,2,true},
        //3時限目
        {"金稼ぎ学","308",true,5,3,true},
        {"SNS炎上論","305",true,5,3,true},
        //4時限目
        {"恋愛科学","212",true,5,4,false},
        {"小樽海鮮学","308",true,5,4,true},
        //5時限目
        {"世界平和学","406",true,5,5,true},
        {"小樽の冬","214",true,5,5,false},
        //////後期
        ////月曜日
        //1時限目
        {"内山学","305",false,1,1,true},
        {"世界遺産入門","406",false,1,1,true},
        //2時限目
        {"子犬会","213",false,1,2,false},
        {"応用世界遺産","401",false,1,2,true},
        //3時限目
        {"Excel学","212",false,1,3,true},
        {"子ウサギ入門","303",false,1,3,true},
        //4時限目
        {"Word学","305",false,1,4,false},
        {"さんすう","407",false,1,4,true},
        //5時限目
        {"PowerPoint学","104",false,1,5,true},
        {"星座入門","303",false,1,5,false},
        ////火曜日
        //1時限目
        {"奥山学","401",false,2,1,false},
        {"旅行学","211",false,2,1,true},
        //2時限目
        {"Adobe学","211",false,2,2,true},
        {"忘却学","307",false,2,2,false},
        //3時限目
        {"話術","305",false,2,3,false},
        {"基礎プリクラ","213",false,2,3,false},
        //4時限目
        {"応用話術","406",false,2,4,false},
        {"タガログ語","413",false,2,4,true},
        //5時限目
        {"交渉話術","413",false,2,5,true},
        {"ショナ語","401",false,2,5,false},
        ////水曜日
        //1時限目
        {"新井田学","212",false,3,1,false},
        {"バスク語","413",false,3,1,true},
        //2時限目
        {"友達の作り方","307",false,3,2,false},
        {"チーズケーキの会","401",false,3,2,false},
        //3時限目
        {"肩幅学","211",false,3,3,true},
        {"基礎茶道","301",false,3,3,true},
        //4時限目
        {"夢解析学","305",false,3,4,true},
        {"基礎生け花","211",false,3,4,false},
        //5時限目
        {"催眠術","210",false,3,5,false},
        {"ハーモニカ入門","214",false,3,5,true},
        ////木曜日
        //1時限目
        {"武市学","307",false,4,1,true},
        {"発想法","406",false,4,1,true},
        //2時限目
        {"子猫学","308",false,4,2,true},
        {"正攻法","301",false,4,2,true},
        //3時限目
        {"脱力学","407",false,4,3,false},
        {"ロック科学","308",false,4,3,false},
        //4時限目
        {"カレー学","305",false,4,4,false},
        {"優しさ入門","307",false,4,4,true},
        //5時限目
        {"お笑い研究","212",false,4,5,true},
        {"植物学","307",false,4,5,false},
        ////金曜日
        //1時限目
        {"齋藤学","406",false,5,1,true},
        {"均等ピザ論","210",false,5,1,false},
        //2時限目
        {"子犬入門","413",false,5,2,true},
        {"基礎昔話","210",false,5,2,true},
        //3時限目
        {"子猫入門","211",false,5,3,false},
        {"ゲーム入門","307",false,5,3,true},
        //4時限目
        {"美学","210",false,5,4,false},
        {"応用靴下論","401",false,5,4,true},
        //5時限目
        {"アレルギー学","308",false,5,5,false},
        {"世界平和論","406",false,5,5,true}
    };

    public static int[] GetTimeTables(bool first)
    {
        List<int> indexs = new List<int>();
        Dictionary<string, int> am = new Dictionary<string, int>();
        for (int j = 0; j < timetable.GetLength(0); j++)
        {
            if((bool)timetable[j,2] != first)
            {
                continue;
            }
            int week = (int)timetable[j,3];
            int time = (int)timetable[j, 4];
            string mix = week.ToString() + time.ToString();
            int max = 2;
            if (am.ContainsKey(mix))
            {
                max = am[mix] * 4;
            }
            else
            {
                am.Add(mix, 0);
            }
            int check = new System.Random().Next(0,max);
            if(check == 0)
            {
                am[mix] =  am[mix] + 1;
                indexs.Add(j);
            }
        }
        return indexs.ToArray();
    }

    public static int[] GetTimeTableForAWeek(bool first,int week)
    {
        List<int> indexs = new List<int>();
        Dictionary<int, int> am = new Dictionary<int, int>();
        for (int j = 0; j < timetable.GetLength(0); j++)
        {
            if ((bool)timetable[j, 2] != first)
            {
                continue;
            }
            if((int)timetable[j,3] != week)
            {
                continue;
            }
            int time = (int)timetable[j, 4];
            int max = 2;
            if (am.ContainsKey(time))
            {
                max = am[time] * 4;
            }
            else
            {
                am.Add(time, 0);
            }
            int check = new System.Random().Next(0, max);
            if (check == 0)
            {
                am[time] = am[time] + 1;
                indexs.Add(j);
            }
        }
        return indexs.ToArray();
    }

    static string[] tips = {
            "[限]と書かれている授業は「履修制限」がある授業で、そのゲームに参加している半分の人数までしか受講できないよ！競争だ！",
            "1時限目の「〇〇学」は、木村ゼミのゼミ生の名前になっているんだ！",
            "実は、リザルト画面の間も画面裏で自由に動くことが出来るよ！この間に階段の側に移動しておくと周りと差が付けられるぞ！",
            "退室時に出てくる場所は入室時の場所と同じなんだ！だから、次行きたい方向側から教室に入るとより速く移動できるね！",
            "たまに一つの時限に二つ以上の授業があるよ！様々な選択肢を考慮して、確実に取れる授業選びをしよう！",
            "授業時間中は拘束されて暇だけど、その間に先の時間割に目を通して作戦を立てると良いよ！",
            "ゲーム中に別のタブに移っちゃうとゲームから切断されることがあるから注意して！"
    };

    public static string GetTips()
    {
        return tips[Random.Range(0, tips.Length)];
    }
}
