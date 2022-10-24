using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SharpClient
{
    public partial class ClientForm : Form
    {
        private Chat formref;
        private ListBox mlbref;
        private ListBox ulbref;
        public ClientForm(Chat form1, ref ListBox mlbref, ref ListBox ulbref)
        {
            this.formref = form1;
            this.mlbref = mlbref;
            this.ulbref = ulbref;
            InitializeComponent();
        }

        void SendName()
        {
            if (Client_Text.Text != "")
            {
                var m = Message.send(MessageRecipients.MR_BROKER, MessageTypes.MT_INIT, Client_Text.Text);
                if (m.GetAction() == MessageTypes.MT_DECLINE)
                {
                    MessageBox.Show("Error");
                    Client_Text.Text = "";
                }
                else
                {
                    Chat.Client = Client_Text.Text;
                    formref.Enabled = true;
                    mlbref.Items.Add($"server: Hello {Client_Text.Text}!");
                    if (File.Exists(Chat.path_user + "history" + Chat.Client+".txt"))
                    {
                        mlbref.Items.Add("nice");
                        Chat.HistoryRead(Chat.Client, ref mlbref);
                    }
                    Thread t = new Thread(() => Chat.ProcessMessages(ref this.formref, ref this.mlbref, ref this.ulbref));
                    t.Start();
                    this.Close();
                }
            }
            else
            {
                MessageBox.Show("Please, enter name");
            }
        }
        private void ClientName_But_Click(object sender, EventArgs e)
        {
            SendName();
        }

        private void ClientForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SendName();
            }
        }
    }
}
