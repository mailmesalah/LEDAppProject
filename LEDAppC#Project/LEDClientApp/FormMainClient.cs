using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LEDClientApp
{
    public partial class FormMain : Form
    {
        
        static TcpClient client = new TcpClient();
        static NetworkStream stream;
        static readonly Object clientLock = new Object();
        static bool clientAvailable = false;
        public FormMain()
        {
            InitializeComponent();
            setInitLEDColors();
        }

        private void setInitLEDColors()
        {
            leftLED0.BackColor = Color.Black;
            leftLED1.BackColor = Color.Black;
            leftLED2.BackColor = Color.Black;
            leftLED3.BackColor = Color.Black;
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
                client = new TcpClient(hostIP,portNo);
                lock (clientLock)
                {
                    stream = client.GetStream();
                    clientAvailable = true;                    
                }
                //Starting the Client listening to any incoming Data from Server.
                Thread t=new Thread(listenToServerData);
                t.IsBackground = true;
                t.Start();
                buttonConnect.Text = "Connected";

                new Thread(sendDataToServer).Start(2);

            }catch(Exception ex){
                Console.WriteLine("FormMain.buttonConnect+Click() "+ex.Message);
                clientAvailable = false;
            }
            
        }

        private void listenToServerData()
        {
            try
            {
                while (clientAvailable)
                {
                    lock (clientLock)
                    {
                        if (stream.DataAvailable)
                        {
                            byte[] data = new byte[3];
                            stream.Read(data, 0, data.Length);
                            updateLEDs(data);
                        }

                    }
                }
            }catch(Exception e){
                Console.WriteLine("FormMainClient listenToServerData() "+e.Message);                
            }
            
        }

        private void updateLEDs(byte[] data)
        {
            //Setting Left LEDs
            setValue((data[1] & 0X01) > 0, 0);
            setValue((data[1] & 0X02) > 0, 1);
            setValue((data[1] & 0X04) > 0, 2);
            setValue((data[1] & 0X08) > 0, 3);

            //Setting Right LEDs
            setValue((data[2] & 0X01) > 0, 4);
            setValue((data[2] & 0X02) > 0, 5);
            setValue((data[2] & 0X04) > 0, 6);
            setValue((data[2] & 0X08) > 0, 7);
        }

        delegate void SetValueCallback(bool b, int control);

        private void setValue(bool b, int control)
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
                        leftLED0.BackColor =b?Color.Red:Color.Black;                        
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

        private void sendDataToServer(Object command)
        {
            try
            {
                lock (clientLock)
                {
                    int cmd = (int)command;
                    if (cmd == 1)
                    {
                        //Set LEDs
                        byte[] data = getByteForSETLED();
                        stream.Write(data, 0, data.Length);
                    }
                    else if (cmd == 2)
                    {
                        //Get LEDs
                        byte[] data = getByteForGETLED();
                        stream.Write(data, 0, data.Length);
                    }

                }
            }
            catch (Exception e)
            {
                Console.WriteLine("FormMainClient sendDataToServer() " + e.Message);                
            }
            
        }

        private byte[] getByteForSETLED()
        {
            byte[] data = new byte[3];
            data[0] = 1;//Command
            data[1] = 0X00;
            data[2] = 0X00;
            //Setting Left LEDS
            byte val = (byte)(leftLED0.BackColor==Color.Red ? 0X01 : 0X00);
            data[1] = (byte)(data[1] | val);
            val = (byte)(leftLED1.BackColor==Color.Blue ? 0X02 : 0X00);
            data[1] = (byte)(data[1] | val);
            val = (byte)(leftLED2.BackColor==Color.Green ? 0X04 : 0X00);
            data[1] = (byte)(data[1] | val);
            val = (byte)(leftLED3.BackColor==Color.Orange ? 0X08 : 0X00);
            data[1] = (byte)(data[1] | val);
            //Setting Right LEDS
            val = (byte)(rightLED0.BackColor == Color.Red ? 0X01 : 0X00);
            data[2] = (byte)(data[2] | val);
            val = (byte)(rightLED1.BackColor == Color.Blue ? 0X02 : 0X00);
            data[2] = (byte)(data[2] | val);
            val = (byte)(rightLED2.BackColor == Color.Green ? 0X04 : 0X00);
            data[2] = (byte)(data[2] | val);
            val = (byte)(rightLED3.BackColor == Color.Yellow ? 0X08 : 0X00);
            data[2] = (byte)(data[2] | val);
            return data;
        }

        private byte[] getByteForGETLED()
        {
            byte[] data = new byte[3];
            data[0] = 2;//Command
            data[1] = 0X00;
            data[2] = 0X00;
            
            return data;
        }
        
        private void buttonRefresh_Click(object sender, EventArgs e)
        {
            new Thread(sendDataToServer).Start(2);
        }

        private void leftLED0_Click(object sender, EventArgs e)
        {
            //Toggle Back Color of the button
            leftLED0.BackColor =leftLED0.BackColor==Color.Black?Color.Red:Color.Black;
            
            new Thread(sendDataToServer).Start(1);
        }

        private void leftLED1_Click(object sender, EventArgs e)
        {
            //Toggle Back Color of the button
            leftLED1.BackColor = leftLED1.BackColor == Color.Black ? Color.Blue : Color.Black;

            new Thread(sendDataToServer).Start(1);
        }

        private void leftLED2_Click(object sender, EventArgs e)
        {
            //Toggle Back Color of the button
            leftLED2.BackColor = leftLED2.BackColor == Color.Black ? Color.Green : Color.Black;

            new Thread(sendDataToServer).Start(1);
        }

        private void leftLED3_Click(object sender, EventArgs e)
        {
            //Toggle Back Color of the button
            leftLED3.BackColor = leftLED3.BackColor == Color.Black ? Color.Orange : Color.Black;

            new Thread(sendDataToServer).Start(1);
        }

        private void rightLED0_Click(object sender, EventArgs e)
        {
            //Toggle Back Color of the button
            rightLED0.BackColor = rightLED0.BackColor == Color.Black ? Color.Red : Color.Black;

            new Thread(sendDataToServer).Start(1);
        }

        private void rightLED1_Click(object sender, EventArgs e)
        {
            //Toggle Back Color of the button
            rightLED1.BackColor = rightLED1.BackColor == Color.Black ? Color.Blue : Color.Black;

            new Thread(sendDataToServer).Start(1);
        }

        private void rightLED2_Click(object sender, EventArgs e)
        {
            //Toggle Back Color of the button
            rightLED2.BackColor = rightLED2.BackColor == Color.Black ? Color.Green : Color.Black;

            new Thread(sendDataToServer).Start(1);
        }

        private void rightLED3_Click(object sender, EventArgs e)
        {
            //Toggle Back Color of the button
            rightLED3.BackColor = rightLED3.BackColor == Color.Black ? Color.Yellow : Color.Black;

            new Thread(sendDataToServer).Start(1);
        }

        
    }
}
