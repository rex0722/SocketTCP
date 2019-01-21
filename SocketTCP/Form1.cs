using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SocketTCP
{
    public partial class Form1 : Form
    {
        const int listenPort = 3456;
        IPAddress[] ips = Dns.GetHostAddresses("");
        Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            labIP.Text = ips[3].ToString();

            IPEndPoint ipont = new IPEndPoint(IPAddress.Any, listenPort);
            serverSocket.Bind(ipont);
            serverSocket.Listen(10);

            new Thread(new ThreadStart(connect)).Start();

        }

        private void connect()
        {
            while (true)
            {
                Console.WriteLine("Waiting...connect");
                Socket clientSocket = serverSocket.Accept();
                IPEndPoint clientip = (IPEndPoint)clientSocket.RemoteEndPoint;
                SocketListener listener = new SocketListener(clientSocket);
                new Thread(new ThreadStart(listener.run)).Start();
            }

        }

        class SocketListener
        {
            private Socket socket;
            public SocketListener(Socket socket)
            {
                this.socket = socket;
            }

            public void run()
            {
                while (true)
                {
                    Console.WriteLine("Waiting...Message");
                    byte[] data = new byte[1024];
                    int datalenght = socket.Receive(data);
                    if (datalenght == 0) break;
                    string input = Encoding.UTF8.GetString(data, 0, datalenght);
                    Console.WriteLine("Get Message:" + input);
                }
                socket.Close();
            }
        }

    }
}
