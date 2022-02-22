using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Discord;
using Discord.WebSocket;
using System.Threading;
using System.IO;
using System.Linq;

namespace SmartFalcon
{
    public class JankenRankData
    {
        public string name { get; set; }
        public int point { get; set; }
    }

    public partial class Form1 : Form
    {
        private readonly DiscordSocketClient _client;
        private readonly ulong otherID = 944029368584388701;
        private string token = "";
        private bool isSilent = false;

        private Dictionary<ulong, string> callNameList = new Dictionary<ulong, string>();
        private Dictionary<ulong, JankenRankData> jankenRankList = new Dictionary<ulong, JankenRankData>();

        public Form1()
        {
            this.ShowInTaskbar = false;

            NotifyIcon icon = new NotifyIcon();
            icon.Icon = new Icon("Resources/icon.ico");
            icon.Visible = true;
            icon.Text = "スマートファルコンbot";

            ContextMenuStrip menu = new ContextMenuStrip();
            ToolStripMenuItem menuItem = new ToolStripMenuItem();
            menuItem.Text = "&終了";
            menuItem.Click += new EventHandler(Close_Click);
            menu.Items.Add(menuItem);
            icon.ContextMenuStrip = menu;

            //トークン取得
            GetToken();

            //呼び名読み込み
            LoadCallName();

            //じゃんけんランク読み込み
            LoadJankenRank();

            _client = new DiscordSocketClient();
            _client.Log += Log;
            _client.Ready += onReady;
            _client.JoinedGuild += JoinedGuild;
            _client.MessageReceived += onMessage;

            Login();

            InitializeComponent();
        }

        private void GetToken()
        {
            //ファイル読み込み
            StreamReader sr = new StreamReader("Resources/token.txt");

            //1行目のみ使用
            token = sr.ReadLine();

            sr.Close();
        }

        private async void Login()
        {
            await _client.LoginAsync(TokenType.Bot, token);
            await _client.StartAsync();

            await Task.Delay(Timeout.Infinite);
        }

        private void Close_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private Task Log(LogMessage log)
        {
            Console.WriteLine(log.ToString());
            return Task.CompletedTask;
        }

        private Task onReady()
        {
            Console.WriteLine($"{_client.CurrentUser} is Running!!");
            return Task.CompletedTask;
        }

        //参加時テキスト
        private async Task JoinedGuild(SocketGuild socketGuild)
        {
            await socketGuild.DefaultChannel.SendMessageAsync("こんにちは！スマートファルコンです☆\nまだまだできることは少ないけど、トレーナーのみなさんを元気にできるように頑張ります！よろしくね～！");
        }

        private async Task onMessage(SocketMessage message)
        {
            //自分自身だったらリターン
            if (message.Author.Id == _client.CurrentUser.Id)
            {
                return;
            }
            //メッセージが空だったらリターン
            if (message == null)
            {
                return;
            }
            //ボットからのメッセージだったらリターン
            if (message.Author.IsBot)
            {
                return;
            }

            //自分へのメンションか
            bool isMention = message.Content.Contains(_client.CurrentUser.Id.ToString()) || message.Content.Contains(otherID.ToString());

            //呼び名
            string authorName = message.Author.Username + "さん";
            if (callNameList.ContainsKey(message.Author.Id))
            {
                authorName = callNameList[message.Author.Id];
            }

            if (isMention)
            {
//#if DEBUG
                if (message.Content.Contains("応答せよ"))
                {
                    await message.Channel.SendMessageAsync("私だ。");
                }
//#endif


                if (message.Content.Contains("こんにちは"))
                {
                    await message.Channel.SendMessageAsync("こんにちは、" + authorName + "！");
                }
                else if (message.Content.Contains("おはよ"))
                {
                    await message.Channel.SendMessageAsync("おはよう、" + authorName + "！");
                }
                else if (message.Content.Contains("こんばんは"))
                {
                    await message.Channel.SendMessageAsync("こんばんは、" + authorName + "！");
                }
                else if (message.Content.Contains("おやすみ"))
                {
                    await message.Channel.SendMessageAsync("おやすみ、" + authorName + "～...");
                }
                else if (message.Content.Contains("ファ・ル・子"))
                {
                    await message.Channel.SendMessageAsync(authorName + "！応援ありがと～～～！！♡");
                }
                else if (message.Content.Contains("静かに"))
                {
                    await message.Channel.SendMessageAsync("はーい、しばらく静かにしてます...");

                    isSilent = true;
                }
                else if (message.Content.Contains("もういいよ"))
                {
                    await message.Channel.SendMessageAsync("はい！ファル子、みんなの会話聞いちゃいます☆");

                    isSilent = false;
                }
                else if (message.Content.Contains("好き"))
                {
                    Random rand = new Random();
                    int num = rand.Next(0, 3);

                    if (num == 0)
                    {
                        await message.Channel.SendMessageAsync(authorName + "...えへへ、なんだか照れちゃうな...♡");
                    }
                    else if (num == 1)
                    {
                        await message.Channel.SendMessageAsync(authorName + "...！ありがとう！！♡");
                    }
                    else if (num == 2)
                    {
                        await message.Channel.SendMessageAsync(authorName + "もファル子のかわいさがだんだんわかってきたね～♡");
                    }
                }
                else if (message.Content.Contains("かわいい") || message.Content.Contains("可愛い"))
                {
                    Random rand = new Random();
                    int num = rand.Next(0, 3);

                    if (num == 0)
                    {
                        await message.Channel.SendMessageAsync("えへへ～～、ありがとっ！" + authorName + "！♡");
                    }
                    else if (num == 1)
                    {
                        await message.Channel.SendMessageAsync("ファル子のかわいさ、もっと伝えちゃうぞ～☆");
                    }
                    else if (num == 2)
                    {
                        await message.Channel.SendMessageAsync("もう～～、褒めすぎだよ～～っ♡");
                    }
                }
                else if (message.Content.Contains("って呼んで"))
                {
                    //って呼んで　までの文字列抜き出し
                    int start = _client.CurrentUser.Mention.Length;
                    string name = message.Content.Substring(start, message.Content.IndexOf("って呼んで") - start);

                    //先頭が半角スペースだったら削除
                    if (name.StartsWith(" "))
                    {
                        name = name.Substring(1);
                    }

                    //既に存在していたら書き換え
                    if (callNameList.ContainsKey(message.Author.Id))
                    {
                        callNameList[message.Author.Id] = name;
                    }
                    //なければ追加
                    else
                    {
                        callNameList.Add(message.Author.Id, name);
                    }

                    //テキストファイルに上書き保存
                    SaveCallName();

                    //読み直し
                    LoadCallName();

                    await message.Channel.SendMessageAsync("は～い、これからは" + callNameList[message.Author.Id] + "って呼ぶね！");
                }
                else if (message.Content.Contains("しゃい☆"))
                {
                    if (message.Content.Contains("しゃいしゃい☆"))
                    {
                        await message.Channel.SendMessageAsync("っしゃいしゃいしゃ～いっ☆");
                    }
                    else
                    {
                        Random rand = new Random();
                        int num = rand.Next(0, 2);

                        if (num == 0)
                        {
                            await message.Channel.SendMessageAsync("う～～～～...............っしゃい！！");
                        }
                        else
                        {
                            await message.Channel.SendMessageAsync("っしゃいしゃ～いっ☆");
                        }
                    }
                }
                else if (message.Content.Contains("占って"))
                {
                    //20220101のようなシード値
                    int seedDay = DateTime.Today.Year * 10000 + DateTime.Today.Month * 1000 + DateTime.Today.Day;
                    //メッセージの送り主によって決まる値
                    int seedID = (int)message.Author.Id % 9999;
                    //2つを足した値をシード値にする
                    int seed = seedDay + seedID;

                    //シード値から乱数生成 (日替わりで違う結果になる)
                    Random rand = new Random(seed);
                    int num = rand.Next(0, 100);

                    //送る文章
                    string send = authorName + "の今日の運勢は...";

                    //大大吉 (5/100)
                    if (num < 5)
                    {
                        send += "**大大吉**！！！\nすご～～い！おめでとう！！今日はとってもいいことがあるかも！！";
                    }
                    //大吉 (15/100)
                    else if (num < 20)
                    {
                        send += "**大吉**！\nやった～！今日の運勢はバッチリ！";
                    }
                    //中吉 (20/100)
                    else if (num < 40)
                    {
                        send += "**中吉**！\n今日も元気にがんばろう～！";
                    }
                    //吉 (25/100)
                    else if (num < 65)
                    {
                        send += "**吉**！\n今日も無事に一日を過ごせますように！";
                    }
                    //小吉 (20/100)
                    else if (num < 85)
                    {
                        send += "**小吉**！\n小さな幸せ、あるかも？";
                    }
                    //凶 (10 /100)
                    else if (num < 95)
                    {
                        send += "**凶**...\nお、落ち込まないで！いいことあるよ...！きっと！";
                    }
                    //大凶 (4 /100)
                    else if (num < 99)
                    {
                        send += "**大凶**......\n今日は身の周りに注意かも...";
                    }
                    //超大吉 (1/100)
                    else if (num < 100)
                    {
                        send += "**超大吉**！？！？\nえ～～～っ！！なにこれ！！すごすぎるよ～～！！！\n今日ガシャを引いたらもしかしちゃうかも！？！？";
                    }

                    //送信
                    await message.Channel.SendMessageAsync(send);
                }
                else if (message.Content.Contains("慰めて"))
                {
                    //送信
                    await message.Channel.SendMessageAsync("よしよし...つらかったね...どんなにつらくてもファル子がいるからね...！");
                }
                else if (message.Content.Contains("じゃんけん"))
                {
                    //ランキング取得
                    if (message.Content.Contains("ランキング"))
                    {
                        //ランキング取得
                        string output = GetRankStr();

                        await message.Channel.SendMessageAsync(output);
                    }
                    else
                    {
                        //ランダムで手を決める
                        Random rand = new Random();
                        int num = rand.Next(0, 3);
                        int result = -1;

                        string falcoHand = "";

                        //グー
                        if (num == 0)
                        {
                            if (message.Content.Contains("グー") || message.Content.Contains("ぐー") || message.Content.Contains(":fist:") || message.Content.Contains("✊"))
                            {
                                result = 2;
                            }
                            else if (message.Content.Contains("チョキ") || message.Content.Contains("ちょき") || message.Content.Contains(":v:") || message.Content.Contains("✌"))
                            {
                                result = 1;
                            }
                            else if (message.Content.Contains("パー") || message.Content.Contains("ぱー") || message.Content.Contains(":hand_splayed:") || message.Content.Contains("✋"))
                            {
                                result = 0;
                            }

                            falcoHand = ":fist:";
                        }
                        //チョキ
                        else if (num == 1)
                        {
                            if (message.Content.Contains("グー") || message.Content.Contains("ぐー") || message.Content.Contains(":fist:") || message.Content.Contains("✊"))
                            {
                                result = 0;
                            }
                            else if (message.Content.Contains("チョキ") || message.Content.Contains("ちょき") || message.Content.Contains(":v:") || message.Content.Contains("✌"))
                            {
                                result = 2;
                            }
                            else if (message.Content.Contains("パー") || message.Content.Contains("ぱー") || message.Content.Contains(":hand_splayed:") || message.Content.Contains("✋"))
                            {
                                result = 1;
                            }

                            falcoHand = ":v:";
                        }
                        //パー
                        else if (num == 2)
                        {
                            if (message.Content.Contains("グー") || message.Content.Contains("ぐー") || message.Content.Contains(":fist:") || message.Content.Contains("✊"))
                            {
                                result = 1;
                            }
                            else if (message.Content.Contains("チョキ") || message.Content.Contains("ちょき") || message.Content.Contains(":v:") || message.Content.Contains("✌"))
                            {
                                result = 0;
                            }
                            else if (message.Content.Contains("パー") || message.Content.Contains("ぱー") || message.Content.Contains(":hand_splayed:") || message.Content.Contains("✋"))
                            {
                                result = 2;
                            }

                            falcoHand = ":hand_splayed:";
                        }

                        //手を指定してなかったら指定するようにいう
                        if (result == -1)
                        {
                            await message.Channel.SendMessageAsync("じゃんけんの手を指定してね！\n例:じゃんけん グー");
                        }
                        else if (result == 0)
                        {
                            await message.Channel.SendMessageAsync(falcoHand + "\nファル子の負け～～...\n悔しい～～！ 次は勝つからね！！");


                            //既に存在していたら書き換え
                            if (jankenRankList.ContainsKey(message.Author.Id))
                            {
                                //ポイント加算
                                jankenRankList[message.Author.Id].point += 10;
                            }
                            //なければ追加
                            else
                            {
                                JankenRankData data = new JankenRankData();
                                data.name = message.Author.Username;
                                data.point = 10;
                                jankenRankList.Add(message.Author.Id, data);
                            }

                            SaveJankenRank();

                            LoadJankenRank();

                            //現在のランク取得
                            int nowRank = GetJankenRanking(message.Author.Id);

                            await message.Channel.SendMessageAsync("現在のポイント:" + jankenRankList[message.Author.Id].point + "pt, " + nowRank + "位");
                        }
                        else if (result == 1)
                        {
                            await message.Channel.SendMessageAsync(falcoHand + "\nファル子の勝ち～！！ やった～～～☆");

                            //既に存在していたら書き換え
                            if (jankenRankList.ContainsKey(message.Author.Id))
                            {
                                //ポイント減算
                                jankenRankList[message.Author.Id].point -= 5;
                            }
                            //なければ追加
                            else
                            {
                                JankenRankData data = new JankenRankData();
                                data.name = message.Author.Username;
                                data.point = -5;
                                jankenRankList.Add(message.Author.Id, data);
                            }

                            SaveJankenRank();

                            LoadJankenRank();

                            //現在のランク取得
                            int nowRank = GetJankenRanking(message.Author.Id);

                            await message.Channel.SendMessageAsync("現在のポイント:" + jankenRankList[message.Author.Id].point + "pt, " + nowRank + "位");
                        }
                        else if (result == 2)
                        {
                            await message.Channel.SendMessageAsync(falcoHand + "\nあいこ！もう一回じゃんけんしよ！");
                        }
                    }
                }
            }
            else
            {
                //静かにしてるよう言われてたら発言を拾わない
                if (isSilent)
                {
                    return;
                }

                if (message.Content.Contains("アイドル"))
                {
                    Random rand = new Random();
                    int num = rand.Next(0, 3);

                    if (num == 0)
                    {
                        await message.Channel.SendMessageAsync("ウマドルですっ");
                    }
                    else if (num == 1)
                    {
                        await message.Channel.SendMessageAsync("ウマドルだよっ");
                    }
                    else if (num == 2)
                    {
                        await message.Channel.SendMessageAsync("ウマドルのことかな？");
                    }

                }
                else if (message.Content.Contains("ファル子"))
                {
                    Random rand = new Random();
                    int num = rand.Next(0, 3);

                    if (num == 0)
                    {
                        await message.Channel.SendMessageAsync("ファル子で～す☆");
                    }
                    else if (num == 1)
                    {
                        await message.Channel.SendMessageAsync("呼んだ～？☆");
                    }
                    else if (num == 2)
                    {
                        await message.Channel.SendMessageAsync("はい！ファル子、頑張っちゃう♡");
                    }

                }
            }
        }

        //呼び名ロード
        private void LoadCallName()
        {
            //まずリストを空に。
            callNameList.Clear();

            //ファイルがなかったらスルー
            if (!File.Exists("Resources/CallNameList.txt"))
            {
                return;
            }

            //ファイル読み込み
            StreamReader sr = new StreamReader("Resources/CallNameList.txt");

            while (!sr.EndOfStream)
            {
                string line = sr.ReadLine();
                string[] str = line.Split(',');

                //0番目にID、1番目に呼び名が入る
                ulong id;
                try
                {
                    id = ulong.Parse(str[0]);
                }
                catch(Exception e)
                {
                    id = 0;
                }
                string callName = str[1];

                //追加
                if (id != 0 && string.IsNullOrEmpty(callName) == false)
                {
                    callNameList.Add(id, callName);
                }
            }

            sr.Close();
        }

        //呼び名セーブ
        private void SaveCallName()
        {
            StreamWriter sr = new StreamWriter("Resources/CallNameList.txt");
            foreach(var v in callNameList)
            {
                sr.WriteLine(v.Key + "," + v.Value);
            }
            sr.Close();
        }

        //じゃんけんランキングロード
        private void LoadJankenRank()
        {
            //まずリストを空に。
            jankenRankList.Clear();

            //ファイルがなかったらスルー
            if (!File.Exists("Resources/JankenRankList.txt"))
            {
                return;
            }

            //ファイル読み込み
            StreamReader sr = new StreamReader("Resources/JankenRankList.txt");

            while (!sr.EndOfStream)
            {
                string line = sr.ReadLine();
                string[] str = line.Split(',');

                //0番目にID、1番目に呼び名が入る
                ulong id;
                try
                {
                    id = ulong.Parse(str[0]);
                }
                catch (Exception e)
                {
                    id = 0;
                }

                string name = str[1];
                int point = int.Parse(str[2]);

                //追加
                if (id != 0)
                {
                    JankenRankData data = new JankenRankData();
                    data.name = name;
                    data.point = point;
                    jankenRankList.Add(id, data);
                }
            }

            sr.Close();
        }

        //じゃんけんランキングセーブ
        private void SaveJankenRank()
        {
            StreamWriter sr = new StreamWriter("Resources/JankenRankList.txt");
            foreach (var v in jankenRankList)
            {
                sr.WriteLine(v.Key + "," + v.Value.name + "," + v.Value.point);
            }
            sr.Close();
        }

        //現在の順位取得
        private int GetJankenRanking(ulong id)
        {
            //ランキングに登録されていなかったら-1を返す
            if (jankenRankList.ContainsKey(id) == false)
            {
                return -1;
            }

            // ソート
            var sorted = jankenRankList.OrderByDescending(pair => pair.Value.point);
            var list = sorted.ToDictionary(x => x.Key);

            int count = 1;
            foreach (var v in list)
            {
                if (v.Key == id)
                {
                    return count;
                }

                count++;
            }

            return -1;
        }

        private string GetRankStr()
        {
            string result = "---ファル子じゃんけんランキング---\n";

            // ソート
            var sorted = jankenRankList.OrderByDescending(pair => pair.Value.point);
            var list = sorted.ToDictionary(x => x.Key);

            int count = 1;
            foreach (var v in list)
            {
                result += count + "位:" + v.Value.Value.name + "\t" + v.Value.Value.point + "pt\n";

                count++;
            }

            return result;
        }
    }
}
