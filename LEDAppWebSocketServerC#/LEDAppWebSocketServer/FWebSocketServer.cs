using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace LEDServerApp
{
    public partial class FWebSocketServer : Form
    {
        static List<TcpClient> clients = new List<TcpClient>();
        static TcpListener server;
        static readonly Object serverLock = new Object();

        private static byte ledStateMain = 0x0f;
        private static byte ledStateRemote = 0x00;

        public FWebSocketServer()
        {
            InitializeComponent();
            UpdateLEDState();

            //Gets the IP Address
            string hostName = Dns.GetHostName(); // Retrive the Name of HOST            
            IPHostEntry hostname = Dns.GetHostEntry(hostName);
            IPAddress[] ip = hostname.AddressList;
            foreach (IPAddress item in ip)
            {
                if (item.AddressFamily == AddressFamily.InterNetwork)
                {
                    textHostIPAddress.Text = item.ToString();
                }
            }

        }

        private void setInitLEDColors()
        {
            leftLED0.BackColor = Color.Red;
            leftLED1.BackColor = Color.Blue;
            leftLED2.BackColor = Color.Green;
            leftLED3.BackColor = Color.Orange;
            rightLED0.BackColor = Color.Black;
            rightLED1.BackColor = Color.Black;
            rightLED2.BackColor = Color.Black;
            rightLED3.BackColor = Color.Black;
        }

        private void buttonConnect_Click(object sender, EventArgs e)
        {
            try
            {
                String hostIP = textHostIPAddress.Text.Trim();
                int portNo = Convert.ToInt32(textPortNo.Text.Trim());
                server = new TcpListener(IPAddress.Parse(hostIP), portNo);
                server.Start();

                //Starting the Client listening to any incoming Data from Server.
                Thread t = new Thread(addIncomingClients);
                t.IsBackground = true;
                t.Start();

                buttonConnect.Text = "Running";
            }
            catch (Exception ex)
            {
                Console.WriteLine("FormMainServer.buttonConnect+Click() " + ex.Message);         
            }

        }

        private void addIncomingClients()
        {
            while (true)
            {
                try
                {
                    TcpClient client = server.AcceptTcpClient();
                    //Let the client object listen for any incoming              
                    Thread t = new Thread(listenToClientForData);
                    t.IsBackground = true;
                    t.Start(client);

                    lock (serverLock)
                    {
                        clients.Add(client);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("FormMainServer.addIncomingClients()" + e.Message);
                }

            }
        }

        private void listenToClientForData(Object pclient)
        {
            TcpClient client;
            NetworkStream stream;

            try
            {
                lock (serverLock)
                {
                    client = (TcpClient)pclient;
                    stream = client.GetStream();
                }

                while (true)
                {

                    lock (serverLock)
                    {
                        if (stream.DataAvailable)
                        {

                            Byte[] bytes = new Byte[client.Available];

                            stream.Read(bytes, 0, bytes.Length);

                            String data = Encoding.UTF8.GetString(bytes);

                            if (new Regex("^GET").IsMatch(data))
                            {
                                sendHandshake(stream, data);
                                Console.WriteLine("Done Handshake");
                            }
                            else
                            {
                                byte[] message=decodeMessage(bytes);
                                Console.WriteLine("Working fine");
                                switch (message[0])
                                {
                                    case 1:  // SETLED
                                        ledStateMain = message[1];
                                        ledStateRemote = message[2];
                                        UpdateLEDState();
                                        sendMessageToAllClients();
                                        break;
                                    case 2:  // GETLED
                                        Console.WriteLine("Return get command");
                                        sendMessage(stream);
                                        break;
                                }
                            }

                        }

                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("FormMainServer.listenToClientForData()" + e.Message);
            }

        }

        private static void sendHandshake(NetworkStream stream, String data)
        {
            Byte[] response = Encoding.UTF8.GetBytes("HTTP/1.1 101 Switching Protocols" + Environment.NewLine
+ "Connection: Upgrade" + Environment.NewLine
+ "Upgrade: websocket" + Environment.NewLine
+ "Sec-WebSocket-Accept: " + Convert.ToBase64String(
    SHA1.Create().ComputeHash(
        Encoding.UTF8.GetBytes(
            new Regex("Sec-WebSocket-Key: (.*)").Match(data).Groups[1].Value.Trim() + "258EAFA5-E914-47DA-95CA-C5AB0DC85B11"
        )
    )
) + Environment.NewLine
+ Environment.NewLine);

            stream.Write(response, 0, response.Length);
        }

        private static void sendMessage(NetworkStream stream)
        {
            //Sending message to client
            // GETLED return command
            try
            {
                //Data to be send
                byte[] rawData = {3,ledStateMain,ledStateRemote };
               
                int frameCount = 0;
                byte[] frame = new byte[10];

                frame[0] = (byte)130;

                if (rawData.Length <= 125)
                {
                    frame[1] = (byte)rawData.Length;
                    frameCount = 2;
                }
                else if (rawData.Length >= 126 && rawData.Length <= 65535)
                {
                    frame[1] = (byte)126;
                    int len = rawData.Length;
                    frame[2] = (byte)((len >> 8) & (byte)255);
                    frame[3] = (byte)(len & (byte)255);
                    frameCount = 4;
                }
                else
                {
                    frame[1] = (byte)127;
                    int len = rawData.Length;
                    frame[2] = (byte)((len >> 56) & (byte)255);
                    frame[3] = (byte)((len >> 48) & (byte)255);
                    frame[4] = (byte)((len >> 40) & (byte)255);
                    frame[5] = (byte)((len >> 32) & (byte)255);
                    frame[6] = (byte)((len >> 24) & (byte)255);
                    frame[7] = (byte)((len >> 16) & (byte)255);
                    frame[8] = (byte)((len >> 8) & (byte)255);
                    frame[9] = (byte)(len & (byte)255);
                    frameCount = 10;
                }

                int bLength = frameCount + rawData.Length;

                byte[] reply = new byte[bLength];

                int bLim = 0;
                for (int ii = 0; ii < frameCount; ii++)
                {
                    reply[bLim] = frame[ii];
                    bLim++;
                }
                for (int ii = 0; ii < rawData.Length; ii++)
                {
                    reply[bLim] = rawData[ii];
                    bLim++;
                }

                stream.Write(reply, 0, reply.Length);
            }
            catch (Exception e)
            {
                Console.WriteLine("FormMainServer.sendMessage()" + e.Message);
            }


            
        }

        private static void sendMessageToAllClients()
        {
            lock (serverLock)
            {
                for (int i = 0; i < clients.Count; i++)
                {
                    try
                    {
                        TcpClient client = clients.ElementAt(i);
                        NetworkStream stream = client.GetStream();
                        //Sending Back the current LED Status
                        sendMessage(stream);                        
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("FormMainServer.sendDataToAllClients()" + e.Message);
                    }

                }
            }
        }

        private static Byte[] decodeMessage(Byte[] bytes)
        {
            Console.WriteLine("Received a data in decode message!!!" + bytes.Length);
          
            byte rLength = 0;
            int rMaskIndex = 2;
            int rDataStart = 0;
            //b[0] is always binary so no need to check;
            
            rLength = (byte)(bytes[1] & (byte)127);

            if (rLength == (byte)126) rMaskIndex = 4;
            if (rLength == (byte)127) rMaskIndex = 10;

            byte[] masks = new byte[4];

            for (int i = rMaskIndex, j = 0; i < (rMaskIndex + 4); i++, j++)
            {
                masks[j] = bytes[i];
            }

            rDataStart = rMaskIndex + 4;

            int messageLen = bytes.Length - rDataStart;

            byte[] message = new byte[messageLen];
            
            for (int i = rDataStart, j = 0; i < bytes.Length; i++, j++)
            {
                message[j] = (byte)(bytes[i] ^ masks[j % 4]);
                Console.Write(message[j]+" ");
            }
            return message;
        }
     
        private void UpdateLEDState()
        {
            // Update Main LEDs
            leftLED0.BackColor = ((ledStateMain & 0x01) != 0) ? Color.Red : Color.Black;
            leftLED1.BackColor = ((ledStateMain & 0x02) != 0) ? Color.Blue : Color.Black;
            leftLED2.BackColor = ((ledStateMain & 0x04) != 0) ? Color.Green : Color.Black;
            leftLED3.BackColor = ((ledStateMain & 0x08) != 0) ? Color.Orange : Color.Black;

            // Update Remote LEDs
            rightLED0.BackColor = ((ledStateRemote & 0x01) != 0) ? Color.Red : Color.Black;
            rightLED1.BackColor = ((ledStateRemote & 0x02) != 0) ? Color.Blue : Color.Black;
            rightLED2.BackColor = ((ledStateRemote & 0x04) != 0) ? Color.Green : Color.Black;
            rightLED3.BackColor = ((ledStateRemote & 0x08) != 0) ? Color.Yellow : Color.Black;
        }

        
        private void leftLED0_Click(object sender, EventArgs e)
        {
            //Toggle Back Color of the button
            leftLED0.BackColor = leftLED0.BackColor == Color.Black ? Color.Red : Color.Black;
            
            if ((ledStateMain & 0x01) == 0)
            {
                ledStateMain = (byte)(ledStateMain | 0x01);
            }
            else
            {
                ledStateMain = (byte)(ledStateMain & 0x0E);
            }
            
        }

        private void leftLED1_Click(object sender, EventArgs e)
        {
            //Toggle Back Color of the button
            leftLED1.BackColor = leftLED1.BackColor == Color.Black ? Color.Blue : Color.Black;

            if ((ledStateMain & 0x02) == 0)
            {
                ledStateMain = (byte)(ledStateMain | 0x02);
            }
            else
            {
                ledStateMain = (byte)(ledStateMain & 0x0D);
            }
        }

        private void leftLED2_Click(object sender, EventArgs e)
        {
            //Toggle Back Color of the button
            leftLED2.BackColor = leftLED2.BackColor == Color.Black ? Color.Green : Color.Black;

            if ((ledStateMain & 0x04) == 0)
            {
                ledStateMain = (byte)(ledStateMain | 0x04);
            }
            else
            {
                ledStateMain = (byte)(ledStateMain & 0x0B);
            }
        }

        private void leftLED3_Click(object sender, EventArgs e)
        {
            //Toggle Back Color of the button
            leftLED3.BackColor = leftLED3.BackColor == Color.Black ? Color.Orange : Color.Black;

            if ((ledStateMain & 0x08) == 0)
            {
                ledStateMain = (byte)(ledStateMain | 0x08);
            }
            else
            {
                ledStateMain = (byte)(ledStateMain & 0x07);
            }
        }

        private void rightLED0_Click(object sender, EventArgs e)
        {
            //Toggle Back Color of the button
            rightLED0.BackColor = rightLED0.BackColor == Color.Black ? Color.Red : Color.Black;

            if ((ledStateRemote & 0x01) == 0)
            {
                ledStateRemote = (byte)(ledStateRemote | 0x01);
            }
            else
            {
                ledStateRemote = (byte)(ledStateRemote & 0x0E);
            }
        }

        private void rightLED1_Click(object sender, EventArgs e)
        {
            //Toggle Back Color of the button
            rightLED1.BackColor = rightLED1.BackColor == Color.Black ? Color.Blue : Color.Black;

            if ((ledStateRemote & 0x02) == 0)
            {
                ledStateRemote = (byte)(ledStateRemote | 0x02);
            }
            else
            {
                ledStateRemote = (byte)(ledStateRemote & 0x0D);
            }
        }

        private void rightLED2_Click(object sender, EventArgs e)
        {
            //Toggle Back Color of the button
            rightLED2.BackColor = rightLED2.BackColor == Color.Black ? Color.Green : Color.Black;

            if ((ledStateRemote & 0x04) == 0)
            {
                ledStateRemote = (byte)(ledStateRemote | 0x04);
            }
            else
            {
                ledStateRemote = (byte)(ledStateRemote & 0x0B);
            }
        }

        private void rightLED3_Click(object sender, EventArgs e)
        {
            //Toggle Back Color of the button
            rightLED3.BackColor = rightLED3.BackColor == Color.Black ? Color.Yellow : Color.Black;

            if ((ledStateRemote & 0x08) == 0)
            {
                ledStateRemote = (byte)(ledStateRemote | 0x08);
            }
            else
            {
                ledStateRemote = (byte)(ledStateRemote & 0x07);
            }
        }

    }
}
