﻿using System;
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
using AForge.Video;
using AForge.Imaging;
namespace Do_An_Tot_Nghiep
{

    public partial class Form1 : Form
    {
        MqttClient client;
        get_Btn_state btn_State = new get_Btn_state();
        cmd_Json cmd = new cmd_Json();
        DateTime data = DateTime.Now;
        string data_recv = string.Empty;
        private int hours = 0;
        private int mins = 0;
        private int seconds = 0;

        MJPEGStream stream;

        public Form1()
        {
            InitializeComponent();
            stream = new MJPEGStream("http://192.168.1.3:4747/video");
            stream.NewFrame += Stream_NewFrame;
        }

        private void Stream_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            //throw new NotImplementedException();
            Bitmap bmp = (Bitmap)eventArgs.Frame.Clone();
            pictureBox1.Image = bmp;
        }

        public void mqtt_connect()
        {
            client = new MqttClient("mqtt.ngoinhaiot.com", 1111, false, null, null, MqttSslProtocols.None);
            client.MqttMsgPublishReceived += client_MqttMsgPublishReceived;
            string clientId = Guid.NewGuid().ToString();
            client.Connect(clientId, "ngocphong260899", "ngocphong260899");
            client.Subscribe(new string[] { "ngocphong260899/device" }, new byte[] { MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE });

            if(client.IsConnected )
            {
                label13.Text = "Connect";
                label13.ForeColor = Color.Green;
            }    
        }

        public void get_State_device()
        {
            string value = cmd.get_State_device;
            client.Publish("ngocphong260899/app", Encoding.UTF8.GetBytes(value), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, true);
        }

        public void readFileJson()
        {
            StreamReader r = new StreamReader(@"E:\Thuc_Tap_Tot_Nghiep\ThucTapTN_APP\Do_An_Tot_Nghiep\File\key_name.json");
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
            stream.Start();
            timer1.Start();
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

        private void bunifuButton9_Click(object sender, EventArgs e)
        {
            String msg = numericUpDown1.Value + "hour" + numericUpDown2.Value +"minute"+" "+ "Trang thai:" + numericUpDown4.Value;

            listBox1.Items.Add(msg);

            if (numericUpDown2.Value < 1 && numericUpDown1.Value < 1)
            {
                MessageBox.Show("Bạn chưa nhập thời gian", "Thông báo!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {

                timer2.Start();
                timer1.Enabled = true;
                bunifuButton9.Enabled = false;
                bunifuButton10.Enabled = true;

            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            label16.Text = DateTime.Now.ToString("HH:mm:ss");
        }

        private string formatHour(int s)
        {
            string t = s.ToString();
            return s < 10 ? "0" + t : t;
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            String[] timetemplate = label16.Text.Split(':');
            int hoursSys = Convert.ToInt32(timetemplate[0]);
            int minsSys = Convert.ToInt32(timetemplate[1]);
            int secSys = Convert.ToInt32(timetemplate[2]);

            int hoursN = (int)numericUpDown1.Value;
            int minsN = (int)numericUpDown2.Value;
            int secN = 60;

            hours = hoursN - hoursSys;
            mins = minsN - minsSys - 1;
            seconds = secN - secSys;
            if (hours > 0 | mins > 0 | seconds > 0)
            {
                if (mins == 0 && hours > 0) { mins = 59; hours = hours - 1; }
                if (seconds == 0 && mins > 0) { seconds = 60; mins = mins - 1; }
                seconds = seconds - 1;
            }



            // Hiển thị thời gian còn lại
            label17.Text = string.Format("{0}:{1}:{2}", formatHour(hours), formatHour(mins), formatHour(seconds));

            if (label17.Text.Equals("00:00:00"))
            {
                Console.WriteLine("start timer");

                switch(numericUpDown4.Value)
                {
                    case 1:
                    {
                            string value = cmd.cmd1_off;
                            client.Publish("ngocphong260899/app", Encoding.UTF8.GetBytes(value), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, true);
                    }
                    break;
                    case 2:
                        {
                            string value = cmd.cmd1_on;
                            client.Publish("ngocphong260899/app", Encoding.UTF8.GetBytes(value), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, true);
                        }
                    break;
                    case 3:
                        {
                            string value = cmd.cmd2_off;
                            client.Publish("ngocphong260899/app", Encoding.UTF8.GetBytes(value), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, true);
                        }
                        break;
                    case 4:
                        {
                            string value = cmd.cmd3_off;
                            client.Publish("ngocphong260899/app", Encoding.UTF8.GetBytes(value), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, true);
                        }
                        break;
                    case 5:
                        {
                            string value = cmd.cmd3_on;
                            client.Publish("ngocphong260899/app", Encoding.UTF8.GetBytes(value), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, true);
                        }
                        break;
                    case 6:
                        {
                            string value = cmd.cmd3_off;
                            client.Publish("ngocphong260899/app", Encoding.UTF8.GetBytes(value), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, true);
                        }
                        break;
                }

                
                timer2.Stop();
                bunifuButton9.Enabled = true;
                bunifuButton10.Enabled = false;
                label17.Text = "00:00:00";
            }
        }

        private void bunifuButton10_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            timer2.Stop();
            bunifuButton9.Enabled = true;
            bunifuButton10.Enabled = false;
            label17.Text = "00:00:00";
        }
    }
}
