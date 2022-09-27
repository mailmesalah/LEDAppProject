using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace WebSocketClientCSharp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        MessageWebSocket socket = new MessageWebSocket();
        DataWriter dataSend;

        private static byte ledStateMain = 0x00;
        private static byte ledStateRemote = 0x00;

        public MainPage()
        {
            this.InitializeComponent();
            UpdateLEDState();
        }

        //Methods

        private void UpdateLEDState()
        {
            // Update Main LEDs
            Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
() =>
{
    LED0Button.Background = ((ledStateMain & 0x01) != 0) ? new SolidColorBrush(Windows.UI.Colors.Red) : new SolidColorBrush(Windows.UI.Colors.Black);
    LED1Button.Background = ((ledStateMain & 0x02) != 0) ? new SolidColorBrush(Windows.UI.Colors.Blue) : new SolidColorBrush(Windows.UI.Colors.Black);
    LED2Button.Background = ((ledStateMain & 0x04) != 0) ? new SolidColorBrush(Windows.UI.Colors.Green) : new SolidColorBrush(Windows.UI.Colors.Black);
    LED3Button.Background = ((ledStateMain & 0x08) != 0) ? new SolidColorBrush(Windows.UI.Colors.Orange) : new SolidColorBrush(Windows.UI.Colors.Black);

    // Update Remote LEDs
    LED4Button.Background = ((ledStateRemote & 0x01) != 0) ? new SolidColorBrush(Windows.UI.Colors.Red) : new SolidColorBrush(Windows.UI.Colors.Black);
    LED5Button.Background = ((ledStateRemote & 0x02) != 0) ? new SolidColorBrush(Windows.UI.Colors.Blue) : new SolidColorBrush(Windows.UI.Colors.Black);
    LED6Button.Background = ((ledStateRemote & 0x04) != 0) ? new SolidColorBrush(Windows.UI.Colors.Green) : new SolidColorBrush(Windows.UI.Colors.Black);
    LED7Button.Background = ((ledStateRemote & 0x08) != 0) ? new SolidColorBrush(Windows.UI.Colors.Yellow) : new SolidColorBrush(Windows.UI.Colors.Black);
}
);

        }

        private async void MessageWebSocketClientAsync()
        {

            // You must register handlers before calling ConnectAsync       
            socket.MessageReceived += OnMessageReceived;
            await socket.ConnectAsync(new Uri("ws://" + IPAddressText.Text.Trim() + ":" + PortNoText.Text.Trim()));

            Debug.WriteLine("Client Connected with Server");
            dataSend = new DataWriter(socket.OutputStream);

            byte[] bData = { 2, 0, 0 };
            sendMessage(bData);

        }

        private void OnMessageReceived(MessageWebSocket sender, MessageWebSocketMessageReceivedEventArgs args)
        {
            try
            {
                Debug.WriteLine("Message Received from Server");

                using (DataReader dr = args.GetDataReader())
                {
                    byte[] bits = new byte[dr.UnconsumedBufferLength];
                    dr.ReadBytes(bits);

                    ledStateMain = bits[1];
                    ledStateRemote = bits[2];

                    UpdateLEDState();
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("OnMessageReceived " + e.Message);
            }
        }

        private async void sendMessage(byte[] bytes)
        {
            dataSend.WriteBytes(bytes);
            await dataSend.StoreAsync();  // Send a "message" to the service           
        }

        private void ConnectButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MessageWebSocketClientAsync();
                ConnectButton.Content = "Connected";
            }
            catch (Exception ex)
            {
                Debug.WriteLine("ConnectButton_Click " + ex.Message);
            }
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            if (dataSend != null)
            {
                byte[] bData = { 2, 0, 0 };
                sendMessage(bData);
            }

        }

        private void LED0Button_Click(object sender, RoutedEventArgs e)
        {

            if ((ledStateMain & 0x01) == 0)
            {
                ledStateMain = (byte)(ledStateMain | 0x01);
                LED0Button.Background = new SolidColorBrush(Windows.UI.Colors.Red);
            }
            else
            {
                ledStateMain = (byte)(ledStateMain & 0x0E);
                LED0Button.Background = new SolidColorBrush(Windows.UI.Colors.Black);
            }

            try
            {
                byte[] bData = { 1, ledStateMain, ledStateRemote };
                sendMessage(bData);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

        }

        private void LED1Button_Click(object sender, RoutedEventArgs e)
        {
            if ((ledStateMain & 0x02) == 0)
            {
                ledStateMain = (byte)(ledStateMain | 0x02);
                LED1Button.Background = new SolidColorBrush(Windows.UI.Colors.Blue);
            }
            else
            {
                ledStateMain = (byte)(ledStateMain & 0x0D);
                LED1Button.Background = new SolidColorBrush(Windows.UI.Colors.Black);
            }

            try
            {
                byte[] bData = { 1, ledStateMain, ledStateRemote };
                sendMessage(bData);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

        }

        private void LED2Button_Click(object sender, RoutedEventArgs e)
        {
            if ((ledStateMain & 0x04) == 0)
            {
                ledStateMain = (byte)(ledStateMain | 0x04);
                LED2Button.Background = new SolidColorBrush(Windows.UI.Colors.Green);
            }
            else
            {
                ledStateMain = (byte)(ledStateMain & 0x0B);
                LED2Button.Background = new SolidColorBrush(Windows.UI.Colors.Black);
            }

            try
            {
                byte[] bData = { 1, ledStateMain, ledStateRemote };
                sendMessage(bData);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        private void LED3Button_Click(object sender, RoutedEventArgs e)
        {
            if ((ledStateMain & 0x08) == 0)
            {
                ledStateMain = (byte)(ledStateMain | 0x08);
                LED3Button.Background = new SolidColorBrush(Windows.UI.Colors.Orange);
            }
            else
            {
                ledStateMain = (byte)(ledStateMain & 0x07);
                LED3Button.Background = new SolidColorBrush(Windows.UI.Colors.Black);
            }

            try
            {
                byte[] bData = { 1, ledStateMain, ledStateRemote };
                sendMessage(bData);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        private void LED4Button_Click(object sender, RoutedEventArgs e)
        {
            if ((ledStateRemote & 0x01) == 0)
            {
                ledStateRemote = (byte)(ledStateRemote | 0x01);
                LED4Button.Background = new SolidColorBrush(Windows.UI.Colors.Red);
            }
            else
            {
                ledStateRemote = (byte)(ledStateRemote & 0x0E);
                LED4Button.Background = new SolidColorBrush(Windows.UI.Colors.Black);
            }

            try
            {
                byte[] bData = { 1, ledStateMain, ledStateRemote };
                sendMessage(bData);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        private void LED5Button_Click(object sender, RoutedEventArgs e)
        {
            if ((ledStateRemote & 0x02) == 0)
            {
                ledStateRemote = (byte)(ledStateRemote | 0x02);
                LED5Button.Background = new SolidColorBrush(Windows.UI.Colors.Blue);
            }
            else
            {
                ledStateRemote = (byte)(ledStateRemote & 0x0D);
                LED5Button.Background = new SolidColorBrush(Windows.UI.Colors.Black);
            }

            try
            {
                byte[] bData = { 1, ledStateMain, ledStateRemote };
                sendMessage(bData);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        private void LED6Button_Click(object sender, RoutedEventArgs e)
        {
            if ((ledStateRemote & 0x04) == 0)
            {
                ledStateRemote = (byte)(ledStateRemote | 0x04);
                LED6Button.Background = new SolidColorBrush(Windows.UI.Colors.Green);
            }
            else
            {
                ledStateRemote = (byte)(ledStateRemote & 0x0B);
                LED6Button.Background = new SolidColorBrush(Windows.UI.Colors.Black);
            }

            try
            {
                byte[] bData = { 1, ledStateMain, ledStateRemote };
                sendMessage(bData);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        private void LED7Button_Click(object sender, RoutedEventArgs e)
        {
            if ((ledStateRemote & 0x08) == 0)
            {
                ledStateRemote = (byte)(ledStateRemote | 0x08);
                LED7Button.Background = new SolidColorBrush(Windows.UI.Colors.Yellow);
            }
            else
            {
                ledStateRemote = (byte)(ledStateRemote & 0x07);
                LED7Button.Background = new SolidColorBrush(Windows.UI.Colors.Black);
            }

            try
            {
                byte[] bData = { 1, ledStateMain, ledStateRemote };
                sendMessage(bData);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
    }
}
