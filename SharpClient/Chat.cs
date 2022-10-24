using System.IO;
using System.Text;
using System.Windows.Forms;

namespace SharpClient
{
    public partial class Chat : Form
    {
        public static string path_user = "C:\\Users\\User\\Downloads\\MsgSockets\\Debug\\";
        static SortedDictionary<int, string> ActiveUsers = new SortedDictionary<int, string>();
        public static string Client;
        public static void HistoryWrite(string str, string uname)
        {
            using (StreamWriter stream = new StreamWriter(path_user + "history" + uname + ".txt",true))
                stream.WriteLine(str);
        }

       public static void HistoryRead(string uname, ref ListBox mlb)
        {
            try
            {
                using (StreamReader stream = new StreamReader(path_user + "history" + uname + ".txt", Encoding.Default))
                {
                    string str;
                    while ((str = stream.ReadLine()) != null)
                        mlb.Items.Add(str);
                }
            }
            catch(Exception ex)
            {
                
                MessageBox.Show("Error"+ex);
            }
        }
        static void RefreshActiveUsers(string str, ref ListBox ulb)
        {
            ActiveUsers.Clear();
            string[] buf = str.Split(' ');
            for (int i = 0; i < buf.Length - 1; i = i + 2)
            {
                ActiveUsers.Add(int.Parse(buf[i]), buf[i + 1]);
            }
            ulb.Items.Clear();
            ulb.Items.Add("All users");
            foreach (var user in ActiveUsers)
            {
                if (user.Value != Client)
                    ulb.Items.Add($"{user.Value} ({user.Key})");
            }
        }
        public static void ProcessMessages(ref Chat form, ref ListBox mlb, ref ListBox ulb)
        {
            while (true)
            {
                var m = Message.send(MessageRecipients.MR_BROKER, MessageTypes.MT_REFRESH, ActiveUsers.Count.ToString());
                if (m.GetAction() != MessageTypes.MT_DECLINE)
                {
                    RefreshActiveUsers(m.GetData(), ref ulb);
                }
                m = Message.send(MessageRecipients.MR_BROKER, MessageTypes.MT_GETDATA);
                switch (m.GetAction())
                {
                    case MessageTypes.MT_DATA:
                        mlb.Items.Add($"{ActiveUsers[(int)m.GetFrom()]}: {m.GetData()}");
                        HistoryWrite($"{ActiveUsers[(int)m.GetFrom()]}: {m.GetData()}", Client);
                        break;
                    case MessageTypes.MT_EXIT:
                        m = Message.send(MessageRecipients.MR_BROKER, MessageTypes.MT_EXIT);
                        form.Close();
                        break;
                    default:
                        Thread.Sleep(1000);
                        break;
                }
            }
        }

        public Chat()
        {
            InitializeComponent();
        }


        private void Send_But_Click(object sender, EventArgs e)
        {
            if (Clients_List.SelectedIndex < 0)
            {
                MessageBox.Show("You have not selected recipient");
            }
            else
            {
                int recipient = (int)MessageRecipients.MR_ALL;
                var msg = Messeages.Text;
                bool isPrivate = false;
                foreach (var user in ActiveUsers)
                {
                    if (Clients_List.SelectedItem.ToString().Contains(user.Value))
                    {
                        recipient = user.Key;
                        string str = $"You whispered to {user.Value}: {msg}";
                        Message_List.Items.Add(str);
                        HistoryWrite(str, Client);
                        isPrivate = true;
                        break;
                    }
                }
                if (!isPrivate)
                {
                    string str = $"You: {msg}";
                    Message_List.Items.Add(str);
                    HistoryWrite(str,Client);
                }
                Message.send((MessageRecipients)recipient, MessageTypes.MT_DATA, (recipient == (int)MessageRecipients.MR_ALL ? "" : "(private) ") + msg);
                Messeages.Clear();
            }
        }

        private void Form1_Load_1(object sender, EventArgs e)
        {
            Clients_List.Items.Add("All users");
            Control.CheckForIllegalCrossThreadCalls = false;
            ClientForm newfom = new ClientForm(this, ref Message_List, ref this.Clients_List);
            newfom.Show();
            this.Enabled = false;
        }

        private void Form1_FormClosing_1(object sender, FormClosingEventArgs e)
        { 
            Message.send(MessageRecipients.MR_BROKER, MessageTypes.MT_EXIT);
        }
    }
 }
