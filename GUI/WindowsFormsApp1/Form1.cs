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

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        private NAudio.Wave.BufferedWaveProvider bwp;
        PipeStream buffer;
        NAudio.Wave.WaveIn wavein;
        int crash;
        TcpClient tcpclnt;
        Stream stm;
        System.Threading.Thread t;
        byte[] audioData;
        public Form1()
        {
            InitializeComponent();
            crash = 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                wavein = new NAudio.Wave.WaveIn() { DeviceNumber = 2 };
                wavein.WaveFormat = new NAudio.Wave.WaveFormat(48000, 2);
                wavein.BufferMilliseconds = 20;
                for (int n = -1; n < NAudio.Wave.WaveIn.DeviceCount; n++)
                {
                    var caps = NAudio.Wave.WaveIn.GetCapabilities(n);
                    Console.WriteLine($"{n}: {caps.ProductName}");
                }
                //wavein.DataAvailable += new EventHandler<NAudio.Wave.WaveInEventArgs>(wi_DataAvailable);
                t = new System.Threading.Thread(SendAudio);
                t.Start();

                wavein.DataAvailable += wi_DataAvailable;
                bwp = new NAudio.Wave.BufferedWaveProvider(wavein.WaveFormat);
                bwp.DiscardOnBufferOverflow = true;
                wavein.StartRecording();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                crash++;
                Console.WriteLine(crash);
            }

        }

        void wi_DataAvailable(object sender, NAudio.Wave.WaveInEventArgs e)
        {
            try
            {
                audioData = new byte[e.Buffer.Length];
                Buffer.BlockCopy(e.Buffer, 0, audioData, 0, e.Buffer.Length);

                // use the ipaddress as in the server program

                // use the ipaddress as in the server program





            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                crash++;
                Console.WriteLine(crash);
                wavein.StopRecording();
            }
        }

        public void SendAudio()
        {
            tcpclnt = new TcpClient(textBox1.Text, 8888);
            tcpclnt.NoDelay = true;
            stm = tcpclnt.GetStream();
            while (true)
            {
                //you need to use Invoke because the new thread can't access the UI elements directly
                MethodInvoker mi = delegate () { this.Text = DateTime.Now.ToString(); };
                this.Invoke(mi);
                if (audioData != null)
                {
                    stm.Write(audioData, 0, audioData.Length);
                }
            }
            tcpclnt.Close();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                TcpClient tcpclnt = new TcpClient();

                tcpclnt.Connect(textBox1.Text, 8888);
                // use the ipaddress as in the server program


                String str = textBox2.Text;
                Stream stm = tcpclnt.GetStream();

                ASCIIEncoding asen = new ASCIIEncoding();
                byte[] ba = asen.GetBytes(str);

                stm.Write(ba, 0, ba.Length);

                tcpclnt.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                crash++;
                Console.WriteLine(crash);
            }

        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                TcpClient tcpclnt = new TcpClient();
                Console.WriteLine("Connecting.....");

                tcpclnt.Connect(textBox1.Text, 8888);
                // use the ipaddress as in the server program

                Console.WriteLine("Connected");
                Console.Write("Enter the string to be transmitted : ");

                Stream stm = tcpclnt.GetStream();
                ASCIIEncoding asen = new ASCIIEncoding();

                if (button4.Text == "Connect")
                {
                    String str = textBox3.Text;
                    byte[] ba = asen.GetBytes(str);
                    Console.WriteLine("Transmitting.....");


                    stm.Write(ba, 0, ba.Length);

                    byte[] bb = new byte[100];
                    int k = stm.Read(bb, 0, 100);
                    String answer = "";
                    for (int i = 0; i < k; i++)
                        answer += Convert.ToChar(bb[i]);

                    tcpclnt.Close();
                    if (answer == "success")
                    {
                        button1.Enabled = true;
                        button3.Enabled = true;
                        button2.Enabled = true;
                        button5.Enabled = true;
                        button4.Text = "Disconnect";
                    }
                }
                else
                {
                    String str = "disconnect";
                    byte[] ba = asen.GetBytes(str);
                    Console.WriteLine("Transmitting.....");


                    stm.Write(ba, 0, ba.Length);

                    byte[] bb = new byte[100];
                    int k = stm.Read(bb, 0, 100);
                    String answer = "";
                    for (int i = 0; i < k; i++)
                        answer += Convert.ToChar(bb[i]);

                    tcpclnt.Close();
                    if (answer == "success")
                    {
                        button1.Enabled = false;
                        button3.Enabled = false;
                        button2.Enabled = false;
                        button5.Enabled = false;
                        button4.Text = "Connect";
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                crash++;
                Console.WriteLine(crash);
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
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                crash++;
                Console.WriteLine(crash);
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            
        }

        private void button6_Click_1(object sender, EventArgs e)
        {
            try
            {
                TcpClient tcpclnt = new TcpClient();
                Console.WriteLine("Connecting.....");

                tcpclnt.Connect(textBox1.Text, 8888);
                // use the ipaddress as in the server program

                Console.WriteLine("Connected");
                Console.Write("Enter the string to be transmitted : ");

                Stream stm = tcpclnt.GetStream();
                ASCIIEncoding asen = new ASCIIEncoding();

                if (button4.Text == "Connect")
                {
                    String str = "check";
                    byte[] ba = asen.GetBytes(str);
                    Console.WriteLine("Transmitting.....");


                    stm.Write(ba, 0, ba.Length);

                    byte[] bb = new byte[100];
                    int k = stm.Read(bb, 0, 100);
                    String answer = "";
                    for (int i = 0; i < k; i++)
                        answer += Convert.ToChar(bb[i]);

                    tcpclnt.Close();
                    if (answer == "success")
                    {
                        button1.Enabled = true;
                        button3.Enabled = true;
                        button2.Enabled = true;
                        button5.Enabled = true;
                        button4.Text = "Disconnect";
                    }
                }
                else
                {
                    String str = "disconnect";
                    byte[] ba = asen.GetBytes(str);
                    Console.WriteLine("Transmitting.....");


                    stm.Write(ba, 0, ba.Length);

                    byte[] bb = new byte[100];
                    int k = stm.Read(bb, 0, 100);
                    String answer = "";
                    for (int i = 0; i < k; i++)
                        answer += Convert.ToChar(bb[i]);

                    tcpclnt.Close();
                    if (answer == "success")
                    {
                        button1.Enabled = false;
                        button3.Enabled = false;
                        button2.Enabled = false;
                        button5.Enabled = false;
                        button4.Text = "Connect";
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                crash++;
                Console.WriteLine(crash);
            }
        }
    }
}
