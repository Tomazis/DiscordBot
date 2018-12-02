//Speak_tcp
//Active connectbutton if internet is down
//config
//size of socket

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Net;
using System.Net.Sockets;
using NAudio;
using System.IO.Pipes;
using System.Threading;


namespace WindowsFormsApp1
{
    

    public partial class Form1 : Form
    {
        private NAudio.Wave.BufferedWaveProvider bwp;
        PipeStream buffer;
        NAudio.Wave.WaveIn wavein;
        int crash;
        System.Threading.Thread t;
        bool isConnected = false;
        int bufferSize = 1048576;
        TcpClient tcpclnt_sound;
        NetworkStream stm_s;
        public Form1()
        {
            InitializeComponent();
            crash = 0;
        }


        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                wavein = new NAudio.Wave.WaveIn() { DeviceNumber = devicelist.SelectedIndex - 1 };
                wavein.WaveFormat = new NAudio.Wave.WaveFormat(48000, 2);
                wavein.BufferMilliseconds = 200;

                //wavein.DataAvailable += new EventHandler<NAudio.Wave.WaveInEventArgs>(wi_DataAvailable);

                wavein.DataAvailable += wi_DataAvailable;
                bwp = new NAudio.Wave.BufferedWaveProvider(wavein.WaveFormat);
                bwp.DiscardOnBufferOverflow = true;
                String[] serverIP = textBox1.Text.Split(':');
                tcpclnt_sound = new TcpClient(serverIP[0], Int32.Parse(serverIP[1]) + 1);
                stm_s = tcpclnt_sound.GetStream();
                wavein.StartRecording();
                logbox.AppendText("Command \"" + "@@ speak_tcp" + "\" sended.\n");
            }
            catch (Exception ex)
            {
                logbox.AppendText(ex.Message, Color.Red);
                crash++;
            }

        }

        void wi_DataAvailable(object sender, NAudio.Wave.WaveInEventArgs e)
        {
            try
            {
                byte[] audioData = new byte[e.Buffer.Length];
                Buffer.BlockCopy(e.Buffer, 0, audioData, 0, e.Buffer.Length);

                // use the ipaddress as in the server program



                stm_s.Write(audioData, 0, audioData.Length);

                //byte[] bb = new byte[100];
                //int k = stm.Read(bb, 0, 100);
                //String answer = "";
                //for (int i = 0; i < k; i++)
                //    answer += Convert.ToChar(bb[i]);

                

            }
            catch (Exception ex)
            {
                logbox.AppendText(ex.Message, Color.Red);
                crash++;
                wavein.StopRecording();
                stm_s.Close();
                tcpclnt_sound.Close();
            }
        }

        public void SendAudio()
        {

            while (true)
            {
                //you need to use Invoke because the new thread can't access the UI elements directly
                MethodInvoker mi = delegate () { this.Text = DateTime.Now.ToString(); };
                this.Invoke(mi);

            }

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            for (int n = -1; n < NAudio.Wave.WaveIn.DeviceCount; n++)
            {
                var caps = NAudio.Wave.WaveIn.GetCapabilities(n);
                devicelist.Items.Add(caps.ProductName);
                // logbox.AppendText($"{n}: {caps.ProductName}");
            }
            devicelist.SelectedIndex = 0;
        }

        private void sendcommandbutton_Click(object sender, EventArgs e)
        {
            try
            {

                // use the ipaddress as in the server program


                String str = commandbox.Text;
                if (str == "")
                {
                    str = "fail";
                }
                String answer = tcp_Send(str);
                if (answer == "success")
                {
                    logbox.AppendText("Command \"" + str + "\" sended.\n");
                }
                else
                {
                    logbox.AppendText("Connection error!\n", Color.Red);
                }
            }
            catch (Exception ex)
            {
                logbox.AppendText(ex.Message, Color.Red);
                crash++;
            }

        }

        private void connectbutton_Click(object sender, EventArgs e)
        {
            try
            {

                if (!isConnected)
                {
                    String str = passwordbox.Text;
                    if (str == "")
                    {
                        str = "no_password";
                    }
                    String answer = tcp_Send(str);
                    if (answer == "success")
                    {
                        button1.Enabled = true;
                        button3.Enabled = true;
                        loadbutton.Enabled = true;
                        sendcommandbutton.Enabled = true;
                        textBox1.Enabled = false;
                        passwordbox.Enabled = false;
                        commandbox.Enabled = true;
                        connectbutton.Text = "Disconnect";
                        isConnected = true;
                        logbox.AppendText("Connected!\n", Color.Green);
                    }
                    else
                    {
                        logbox.AppendText("Connection error!\n", Color.Red);
                    }
                }
                else
                {
                    String str = "disconnect";
                    String answer = tcp_Send(str);
                    if (answer == "success")
                    {
                        button1.Enabled = false;
                        button3.Enabled = false;
                        loadbutton.Enabled = false;
                        sendcommandbutton.Enabled = false;
                        textBox1.Enabled = true;
                        passwordbox.Enabled = true;
                        commandbox.Enabled = false;
                        connectbutton.Text = "Connect";
                        isConnected = false;
                        logbox.AppendText("Disconnected!\n");
                    }
                    else
                    {
                        logbox.AppendText("Error!\n", Color.Red);
                    }
                }

            }
            catch (Exception ex)
            {
                logbox.AppendText(ex.Message);
                crash++;
            }
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                wavein.StopRecording();
                stm_s.Close();
                tcpclnt_sound.Close();
            }
            catch (Exception ex)
            {
                logbox.AppendText(ex.Message);
                crash++;
            }
        }

        private void loadbutton_Click(object sender, EventArgs e)
        {
            String[] serverIP = textBox1.Text.Split(':');
            TcpClient tcpclnt = new TcpClient(serverIP[0], Int32.Parse(serverIP[1])+2);
            NetworkStream stm = tcpclnt.GetStream();
            byte[] SendingBuffer = null;
            String answer = "";
            try
            {
                FileStream Fs = new FileStream(filepathbox.Text, FileMode.Open, FileAccess.Read);
                int NoOfPackets = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(Fs.Length) / Convert.ToDouble(bufferSize)));
                int TotalLength = (int)Fs.Length;
                int CurrentPacketLength, counter = 0;

                answer = SendText(Path.GetFileName(filepathbox.Text),stm);
                if (answer == "exist")
                {
                    logbox.AppendText("File Exist!\n", Color.Red);
                    return;
                }
                for (int i = 0; i < NoOfPackets; i++)
                {
                    if (TotalLength > bufferSize)
                    {
                        CurrentPacketLength = bufferSize;
                        TotalLength = TotalLength - CurrentPacketLength;
                    }
                    else
                    {
                        CurrentPacketLength = TotalLength;
                    }
                    SendingBuffer = new byte[CurrentPacketLength];
                    Fs.Read(SendingBuffer, 0, CurrentPacketLength);
                    stm.Write(SendingBuffer, 0, SendingBuffer.Length);
                    logbox.AppendText("Sended "+(i+1)+" packets of " + NoOfPackets+"\n");
                }

                Fs.Close();
                answer = SendText("end", stm);
                if (answer != "success")
                {
                    logbox.AppendText("Error!\n", Color.Red);
                    return;
                }
                logbox.AppendText("File send done!\n", Color.Green);
                stm.Close();
                tcpclnt.Close();
            }


            catch (Exception ex)
            {
                logbox.AppendText(ex.Message);
                crash++;
            }
        }

        private String SendText(String msg, NetworkStream stm)
        {
            try
            {
                
                String answer = "";
                ASCIIEncoding asen = new ASCIIEncoding();
                byte[] ba = asen.GetBytes(msg);
                stm.Write(ba, 0, ba.Length);

                byte[] bb = new byte[100];
                int k = stm.Read(bb, 0, 100);
                logbox.AppendText("Start File Transfer\n");
                answer = "";
                for (int i = 0; i < k; i++)
                    answer += Convert.ToChar(bb[i]);
                return answer;
            }
            catch (Exception ex)
            {
                logbox.AppendText(ex.Message);
                crash++;
            }
            return "error";
        }

        private void filebutton_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            filepathbox.Text = openFileDialog1.FileName;
        }

        private void logbox_TextChanged(object sender, EventArgs e)
        {
            logbox.ScrollToCaret();
        }

        private void commandbox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                sendcommandbutton_Click(sender, EventArgs.Empty);
            }
        }

        private String tcp_Send(String message)
        {
            try
            {
                //TcpClient tcpclnt = new TcpClient();
                logbox.AppendText("Connecting.....\n");

                String[] serverIP = textBox1.Text.Split(':');

                //String answer = con.StartClient(serverIP[0], Int32.Parse(serverIP[1]), "command", message);

                TcpClient tcpclnt = new TcpClient(serverIP[0], Int32.Parse(serverIP[1]));
                NetworkStream stm = tcpclnt.GetStream();

                //tcpclnt.Connect(serverIP[0], Int32.Parse(serverIP[1]));

                ASCIIEncoding asen = new ASCIIEncoding();
                byte[] ba = asen.GetBytes(message);
                logbox.AppendText("Transmitting.....\n");
                String answer = "";
                stm.Write(ba, 0, ba.Length);

                byte[] bb = new byte[100];
                int k = stm.Read(bb, 0, 100);
                answer = "";
                for (int i = 0; i < k; i++)
                    answer += Convert.ToChar(bb[i]);


                stm.Close();
                tcpclnt.Close();
                return answer;
            }
            catch (Exception ex)
            {
                logbox.AppendText(ex.Message);
                crash++;
                return "error";
            }
            return "error";
        }

    }



    public static class RichTextBoxExtensions
    {
        public static void AppendText(this RichTextBox box, String text, Color color)
        {
            box.SelectionStart = box.TextLength;
            box.SelectionLength = 0;

            box.SelectionColor = color;
            box.AppendText(text);
            box.SelectionColor = box.ForeColor;
        }
    }

   
}
