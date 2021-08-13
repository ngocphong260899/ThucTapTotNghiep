using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Bunifu.Framework.UI;
using BunifuAnimatorNS;
using uPLibrary.Networking.M2Mqtt.Messages;
using uPLibrary.Networking.M2Mqtt;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using System.IO;
namespace Do_An_Tot_Nghiep
{
    public partial class Form1 : Form
    {
        MqttClient client;
        get_Btn_state btn_State = new get_Btn_state();
        cmd_Json cmd = new cmd_Json();

        string data_recv = string.Empty;
        public Form1()
        {
            InitializeComponent();
        }
        public void mqtt_connect()
        {
            client = new MqttClient("mqtt.ngoinhaiot.com", 1111, false, null, null, MqttSslProtocols.None);
            client.MqttMsgPublishReceived += client_MqttMsgPublishReceived;
            string clientId = Guid.NewGuid().ToString();
            client.Connect(clientId, "ngocphong260899", "ngocphong260899");
            client.Subscribe(new string[] { "ngocphong260899/device" }, new byte[] { MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE });
        }

        public void get_State_device()
        {
            string value = cmd.get_State_device;
            client.Publish("ngocphong260899/app", Encoding.UTF8.GetBytes(value), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, true);
        }

        public void readFileJson()
        {
            StreamReader r = new StreamReader(@"E:\Code_App\Do_An_Tot_Nghiep\Do_An_Tot_Nghiep\File\key_name.json");
            string json_Data = r.ReadToEnd();
            Console.WriteLine("Data is: " + json_Data);
            remame_Json_key data_Parse_json = JsonConvert.DeserializeObject<remame_Json_key>(json_Data);
            string room_name = data_Parse_json.room_name;
            string sw_wifi1 = data_Parse_json.sw_wifi1;
            string sw_wifi2 = data_Parse_json.sw_wifi2;
            string sw_wifi3 = data_Parse_json.sw_wifi3;

            bunifuLabel20.Text = room_name;
            bunifuLabel6.Text = sw_wifi1;
            bunifuLabel7.Text = sw_wifi2;
            bunifuLabel8.Text = sw_wifi3;

            r.Close();

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            readFileJson();
            mqtt_connect();
            get_State_device();

        }


        private void client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            var msgs = Encoding.UTF8.GetString(e.Message);
            data_recv = msgs.ToString();
            Console.WriteLine("Debug data_recv: " + data_recv);
            try
            {
                json_Key_parse str_json = JsonConvert.DeserializeObject<json_Key_parse>(data_recv);
                if (str_json == null)
                {
                    Console.WriteLine("-***- Json not DeserializeObject -***-");
                    return;
                }

                String stt = str_json.status;
                String pos = str_json.pos;
                String ssid = str_json.ssid;
                String streng = str_json.streng; 
                int position = Int32.Parse(pos);
                int state = Int32.Parse(stt);
                

                control_render(position, state);
                view_ssid(ssid,streng);


            }
            catch
            {
                Console.WriteLine("Debug: Json not convert becasue json key not full");
            }
        }

        public void control_render(int pos, int state)
        {
            switch (pos)
            {
                case 1:
                    {
                        if (state == 1)
                        {
                            btn_State.render_state(label1, "ON");
                        }
                        else if (state == 0)
                        {
                            btn_State.render_state(label1, "OFF");
                        }
                    }
                    break;

                case 2:
                    {
                        if (state == 1)
                        {
                            btn_State.render_state(label2, "ON");
                        }
                        else if (state == 0)
                        {
                            btn_State.render_state(label2, "OFF");
                        }
                    }
                    break;

                case 3:
                    {
                        if (state == 1)
                        {
                            btn_State.render_state(label3, "ON");
                        }
                        else if (state == 0)
                        {
                            btn_State.render_state(label3, "OFF");
                        }
                    }
                    break;
            }
        }
     
        public void view_ssid(string ssid, string streng)
        {
            btn_State.render_state(label4, ssid);
            btn_State.render_state(label5, streng);

        }
        


        private void bunifuButton1_Click(object sender, EventArgs e)
        {
            bunifuPages1.SetPage("tabPage1");
        }

        private void bunifuButton2_Click(object sender, EventArgs e)
        {
            bunifuPages1.SetPage("tabPage2");
        }

        private void bunifuButton3_Click(object sender, EventArgs e)
        {
            bunifuPages1.SetPage("tabPage3");
        }

        private void bunifuButton4_Click(object sender, EventArgs e)
        {
            bunifuPages1.SetPage("tabPage4");
        }

        private void bunifuButton6_Click(object sender, EventArgs e)
        {
            try
            {
                if (label1.Text == "ON")
                {
                    string value = cmd.cmd1_off;
                    client.Publish("ngocphong260899/app", Encoding.UTF8.GetBytes(value), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, true);
                }
                else if (label1.Text == "OFF")
                {
                    string value = cmd.cmd1_on;
                    client.Publish("ngocphong260899/app", Encoding.UTF8.GetBytes(value), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, true);
                }
            }
            catch
            {
                MessageBox.Show("Phonghg56: Error data send");
            }
        }

        private void bunifuButton7_Click(object sender, EventArgs e)
        {
            try
            {
                if (label2.Text == "ON")
                {
                    string value = cmd.cmd2_off;
                    client.Publish("ngocphong260899/app", Encoding.UTF8.GetBytes(value), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, true);
                }
                else if (label2.Text == "OFF")
                {
                    string value = cmd.cmd2_on;
                    client.Publish("ngocphong260899/app", Encoding.UTF8.GetBytes(value), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, true);
                }
            }
            catch
            {
                MessageBox.Show("Phonghg56: Error data send");
            }
        }

        private void bunifuButton8_Click(object sender, EventArgs e)
        {
            try
            {
                if (label3.Text == "ON")
                {
                    string value = cmd.cmd3_off;
                    client.Publish("ngocphong260899/app", Encoding.UTF8.GetBytes(value), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, true);
                }
                else if (label3.Text == "OFF")
                {
                    string value = cmd.cmd3_on;
                    client.Publish("ngocphong260899/app", Encoding.UTF8.GetBytes(value), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, true);
                }
            }
            catch
            {
                MessageBox.Show("Phonghg56: Error data send");
            }
        }
        
        public void load_Data_doiten(string data1, string data2, string data3, string data4)
        {
            bunifuLabel20.Text = data1;
            bunifuLabel6.Text  = data2;
            bunifuLabel7.Text  = data3;
            bunifuLabel8.Text  = data4;

        }
        
        private void bunifuPictureBox2_Click(object sender, EventArgs e)
        {
            
            doi_ten f = new doi_ten();
            f.data1 = bunifuLabel20.Text;
            f.data2 = bunifuLabel6.Text;
            f.data3 = bunifuLabel7.Text;
            f.data4 = bunifuLabel8.Text;
            f.send_Main = new doi_ten.sen_Data(load_Data_doiten);
            f.ShowDialog();
           
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            
        }
    }
}
