using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SmartFalcon
{
    public partial class Form2 : Form
    {
        Form1 form1;

        public Form2(Form1 form1)
        {
            InitializeComponent();

            this.form1 = form1;

            tbServerID.Text = "842810363304869909";

            tbChannelID.Text = "860527507001442335";

        }

        async private void btnSend_Click(object sender, EventArgs e)
        {
            await form1._client.GetGuild(ulong.Parse(tbServerID.Text)).GetTextChannel(ulong.Parse(tbChannelID.Text)).SendMessageAsync(tbContent.Text);
        }
    }
}
