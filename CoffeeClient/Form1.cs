using System;
using System.Net.Sockets;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace CoffeeClient
{
    public partial class Form1 : Form
    {
        TcpClient client;
        NetworkStream ns;
        StreamReader sr;
        StreamWriter sw;
        Thread listenThread;

        public Form1()
        {
            InitializeComponent();
            try
            {
                client = new TcpClient("127.0.0.1", 8888);
                ns = client.GetStream();
                sr = new StreamReader(ns);
                sw = new StreamWriter(ns) { AutoFlush = true };

                // Read server greeting and menu
                textBox1.Text = sr.ReadLine();
                textBox1.Text += "\r\n" + sr.ReadLine();
                textBox1.Text += "\r\n" + sr.ReadLine();
                textBox1.Text += "\r\n" + sr.ReadLine();
                textBox1.Text += "\r\n" + sr.ReadLine();

                listenThread = new Thread(ListenForMessages);
                listenThread.IsBackground = true;
                listenThread.Start();
            }
            catch
            {
                MessageBox.Show("Cannot connect to server.");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox2.Text != "")
            {
                sw.WriteLine(textBox2.Text);
                textBox1.Text += "\r\nClient >> " + textBox2.Text;
                textBox2.Clear();
            }
        }

        private void ListenForMessages()
        {
            try
            {
                while (true)
                {
                    string msg = sr.ReadLine();
                    if (msg == null) break;

                    Invoke(new Action(() =>
                    {
                        textBox1.Text += "\r\n" + msg;
                    }));
                }
            }
            catch { }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            listenThread.Abort();
            sr.Close();
            sw.Close();
            ns.Close();
            client.Close();
        }
    }
}
