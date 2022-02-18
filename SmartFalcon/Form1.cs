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

namespace SmartFalcon
{
    public partial class Form1 : Form
    {
        private readonly DiscordSocketClient _client;
        private readonly string token = "OTQzOTI0MTk0NDQ0NDQ3ODE0.Yg6H6Q.fr6q4kv61lV2q-0rs1SuNF94Nr8";

        private Dictionary<SocketUser, string> callNameList = new Dictionary<SocketUser, string>();

        public Form1()
        {
            this.ShowInTaskbar = false;

            NotifyIcon icon = new NotifyIcon();
            icon.Icon = new Icon("icon.ico");
            icon.Visible = true;
            icon.Text = "スマートファルコンbot";

            ContextMenuStrip menu = new ContextMenuStrip();
            ToolStripMenuItem menuItem = new ToolStripMenuItem();
            menuItem.Text = "&終了";
            menuItem.Click += new EventHandler(Close_Click);
            menu.Items.Add(menuItem);
            icon.ContextMenuStrip = menu;

            _client = new DiscordSocketClient();
            _client.Log += Log;
            _client.Ready += onReady;
            _client.JoinedGuild += JoinedGuild;
            _client.MessageReceived += onMessage;

            Login();

            InitializeComponent();
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
            bool isMention = message.Content.Contains(_client.CurrentUser.Mention);

            if (isMention)
            {
                if (message.Content.Contains("こんにちは"))
                {
                    if (callNameList.ContainsKey(message.Author))
                    {
                        await message.Channel.SendMessageAsync(message.Author.Mention + "こんにちは、" + callNameList[message.Author] + "！");
                    }
                    else
                    {
                        await message.Channel.SendMessageAsync(message.Author.Mention + "こんにちは、" + message.Author.Username + "さん！");
                    }
                }
                else if (message.Content.Contains("ファ・ル・子"))
                {
                    if (callNameList.ContainsKey(message.Author))
                    {
                        await message.Channel.SendMessageAsync(message.Author.Mention + callNameList[message.Author] + "！応援ありがと～～～！！♡");
                    }
                    else
                    {
                        await message.Channel.SendMessageAsync(message.Author.Mention + message.Author.Username + "さ～ん！応援ありがと～～～！！♡");
                    }
                }
                else if (message.Content.Contains("って呼んで"))
                {
                    //って呼んで　までの文字列抜き出し
                    int start = _client.CurrentUser.Mention.Length;
                    string name = message.Content.Substring(start, message.Content.IndexOf("って呼んで") - start);

                    //既に存在していたら書き換え
                    if (callNameList.ContainsKey(message.Author))
                    {
                        callNameList[message.Author] = name;
                    }
                    //なければ追加
                    else
                    {
                        callNameList.Add(message.Author, name);
                    }

                    await message.Channel.SendMessageAsync(message.Author.Mention + "は～い、これからは" + callNameList[message.Author] + "って呼ぶね！");
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
                    string send = message.Author.Mention;

                    if (callNameList.ContainsKey(message.Author))
                    {
                        send += callNameList[message.Author] + "の今日の運勢は...";
                    }
                    else
                    {
                        send += message.Author.Username + "さんの今日の運勢は...";
                    }

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
                        send += "**吉_**！\n今日も無事に一日を過ごせますように！";
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
            }
            else
            {
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
            }
        }
    }
}
