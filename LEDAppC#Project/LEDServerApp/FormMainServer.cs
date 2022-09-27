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

namespace LEDServerApp
{
    public partial class FormMainServer : Form
    {
        static List<TcpClient> clients = new List<TcpClient>(); 
        static TcpListener server;        
        static readonly Object serverLock = new Object();
        static bool serverAvailable = false;
        public FormMainServer()
        {
            InitializeComponent();
            setInitLEDColors();

            //Gets the IP Address
            string hostName = Dns.GetHostName(); // Retrive the Name of HOST            
            IPHostEntry hostname = Dns.GetHostEntry(hostName);
            IPAddress[] ip = hostname.AddressList;
            foreach (IPAddress item in ip)
            {
                if(item.AddressFamily == AddressFamily.InterNetwork){
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
                server = new TcpListener(IPAddress.Parse(hostIP),portNo);
                server.Start();

                serverAvailable = true;
                //Starting the Client listening to any incoming Data from Server.
                Thread t=new Thread(addIncomingClients);
                t.IsBackground = true;
                t.Start();

                buttonConnect.Text = "Running";
            }catch(Exception ex){
                Console.WriteLine("FormMainServer.buttonConnect+Click() "+ex.Message);
                serverAvailable = false;
            }
            
        }

        private void addIncomingClients()
        {
            while(serverAvailable){
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
                            byte[] data = new byte[3];
                            stream.Read(data, 0, data.Length);
                            switch (data[0])
                            {
                                case 1: //Set LED
                                    setLEDs(data);
                                    //Updates all Clients
                                    sendDataToAllClients();
                                    break;
                                case 2: //Get LED
                                    sendDataToClient(client);
                                    break;
                                default:
                                    break;
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

        private void setLEDs(byte[] data)
        {
            //Setting Left LEDs
            setValue((data[1] & 0X01) > 0,0);
            setValue((data[1] & 0X02) > 0,1);
            setValue((data[1] & 0X04) > 0,2);
            setValue((data[1] & 0X08) > 0,3);

            //Setting Right LEDs
            setValue((data[2] & 0X01) > 0,4);
            setValue((data[2] & 0X02) > 0,5);
            setValue((data[2] & 0X04) > 0,6);
            setValue((data[2] & 0X08) > 0,7);
        }

        delegate void SetValueCallback(bool b,int control);

        private void setValue(bool b,int control)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            switch (control)
            {
                case 0:
                    if (leftLED0.InvokeRequired)
                    {
                        SetValueCallback d = new SetValueCallback(setValue);
                        this.Invoke(d, new object[] { b, control });
                    }
                    else
                    {
                        leftLED0.BackColor = b ? Color.Red : Color.Black;
                    }
                    break;
                case 1:
                    if (leftLED1.InvokeRequired)
                    {
                        SetValueCallback d = new SetValueCallback(setValue);
                        this.Invoke(d, new object[] { b, control });
                    }
                    else
                    {
                        leftLED1.BackColor = b ? Color.Blue : Color.Black;
                    }
                    break;
                case 2:
                    if (leftLED2.InvokeRequired)
                    {
                        SetValueCallback d = new SetValueCallback(setValue);
                        this.Invoke(d, new object[] { b, control });
                    }
                    else
                    {
                        leftLED2.BackColor = b ? Color.Green : Color.Black;
                    }
                    break;
                case 3:
                    if (leftLED3.InvokeRequired)
                    {
                        SetValueCallback d = new SetValueCallback(setValue);
                        this.Invoke(d, new object[] { b, control });
                    }
                    else
                    {
                        leftLED3.BackColor = b ? Color.Orange : Color.Black;
                    }
                    break;
                case 4:
                    if (rightLED0.InvokeRequired)
                    {
                        SetValueCallback d = new SetValueCallback(setValue);
                        this.Invoke(d, new object[] { b, control });
                    }
                    else
                    {
                        rightLED0.BackColor = b ? Color.Red : Color.Black;
                    }
                    break;
                case 5:
                    if (rightLED1.InvokeRequired)
                    {
                        SetValueCallback d = new SetValueCallback(setValue);
                        this.Invoke(d, new object[] { b, control });
                    }
                    else
                    {
                        rightLED1.BackColor = b ? Color.Blue : Color.Black;
                    }
                    break;
                case 6:
                    if (rightLED2.InvokeRequired)
                    {
                        SetValueCallback d = new SetValueCallback(setValue);
                        this.Invoke(d, new object[] { b, control });
                    }
                    else
                    {
                        rightLED2.BackColor = b ? Color.Green : Color.Black;
                    }
                    break;
                case 7:
                    if (rightLED3.InvokeRequired)
                    {
                        SetValueCallback d = new SetValueCallback(setValue);
                        this.Invoke(d, new object[] { b, control });
                    }
                    else
                    {
                        rightLED3.BackColor = b ? Color.Yellow : Color.Black;
                    }
                    break;
            }
            
        }

        private void sendDataToClient(Object pclient)
        {
            TcpClient client = (TcpClient)pclient;
            NetworkStream stream;

            try
            {
                lock (serverLock)
                {
                    client = (TcpClient)pclient;
                    stream = client.GetStream();
                    //Sending Back the current LED Status
                    byte[] data = getByteForGETLEDForClients();
                    stream.Write(data, 0, data.Length);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("FormMainServer.sendDataToClient()" + e.Message);
            }
            
        }

        private void sendDataToAllClients()
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
                        byte[] data = getByteForGETLEDForClients();
                        stream.Write(data, 0, data.Length);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("FormMainServer.sendDataToAllClients()" + e.Message);
                    }
                    
                }                
            }
        }

        private byte[] getByteForGETLEDForClients()
        {
            byte[] data = new byte[3];
            data[0] = 3;//Command
            data[1] = 0X00;
            data[2] = 0X00;
            //Setting Left LEDS
            byte val = (byte)(leftLED0.BackColor == Color.Red ? 0X01 : 0X00);
            data[1] = (byte)(data[1] | val);
            val = (byte)(leftLED1.BackColor == Color.Blue ? 0X02 : 0X00);
            data[1] = (byte)(data[1] | val);
            val = (byte)(leftLED2.BackColor == Color.Green ? 0X04 : 0X00);
            data[1] = (byte)(data[1] | val);
            val = (byte)(leftLED3.BackColor == Color.Orange ? 0X08 : 0X00);
            data[1] = (byte)(data[1] | val);
            //Setting Right LEDS
            val = (byte)(rightLED0.BackColor == Color.Red ? 0X01 : 0X00);
            data[2] = (byte)(data[2] | val);
            val = (byte)(rightLED1.BackColor == Color.Blue ? 0X02 : 0X00);
            data[2] = (byte)(data[2] | val);
            val = (byte)(rightLED2.BackColor == Color.Green ? 0X04 : 0X00);
            data[2] = (byte)(data[2] | val);
            val = (byte)(rightLED3.BackColor == Color.Yellow  ? 0X08 : 0X00);
            data[2] = (byte)(data[2] | val);
            return data;
        }

        private void leftLED0_Click(object sender, EventArgs e)
        {
            //Toggle Back Color of the button
            leftLED0.BackColor = leftLED0.BackColor == Color.Black ? Color.Red : Color.Black;
        }

        private void leftLED1_Click(object sender, EventArgs e)
        {
            //Toggle Back Color of the button
            leftLED1.BackColor = leftLED1.BackColor == Color.Black ? Color.Blue : Color.Black;
        }

        private void leftLED2_Click(object sender, EventArgs e)
        {
            //Toggle Back Color of the button
            leftLED2.BackColor = leftLED2.BackColor == Color.Black ? Color.Green : Color.Black;
        }

        private void leftLED3_Click(object sender, EventArgs e)
        {
            //Toggle Back Color of the button
            leftLED3.BackColor = leftLED3.BackColor == Color.Black ? Color.Orange : Color.Black;
        }

        private void rightLED0_Click(object sender, EventArgs e)
        {
            //Toggle Back Color of the button
            rightLED0.BackColor = rightLED0.BackColor == Color.Black ? Color.Red : Color.Black;
        }

        private void rightLED1_Click(object sender, EventArgs e)
        {
            //Toggle Back Color of the button
            rightLED1.BackColor = rightLED1.BackColor == Color.Black ? Color.Blue : Color.Black;
        }

        private void rightLED2_Click(object sender, EventArgs e)
        {
            //Toggle Back Color of the button
            rightLED2.BackColor = rightLED2.BackColor == Color.Black ? Color.Green : Color.Black;
        }

        private void rightLED3_Click(object sender, EventArgs e)
        {
            //Toggle Back Color of the button
            rightLED3.BackColor = rightLED3.BackColor == Color.Black ? Color.Yellow : Color.Black;
        }
        
    }
}
