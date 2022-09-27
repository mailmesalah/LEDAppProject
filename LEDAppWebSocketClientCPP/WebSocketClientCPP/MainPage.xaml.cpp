//
// MainPage.xaml.cpp
// Implementation of the MainPage class
//

#include "pch.h"
#include "MainPage.xaml.h"

using namespace WebSocketClientCPP;

using namespace Platform;
using namespace Windows::Foundation;
using namespace Windows::Foundation::Collections;
using namespace Windows::UI::Xaml;
using namespace Windows::UI::Xaml::Controls;
using namespace Windows::UI::Xaml::Controls::Primitives;
using namespace Windows::UI::Xaml::Data;
using namespace Windows::UI::Xaml::Input;
using namespace Windows::UI::Xaml::Media;
using namespace Windows::UI::Xaml::Navigation;
using namespace Windows::UI::Core;
using namespace concurrency;


// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

MainPage::MainPage()
{
	InitializeComponent();
	socket = ref new MessageWebSocket();
	ledStateMain = 0x00;
	ledStateRemote = 0x00;
}

void WebSocketClientCPP::MainPage::UpdateLEDState()
{

	Windows::ApplicationModel::Core::CoreApplication::MainView->CoreWindow->Dispatcher->RunAsync(CoreDispatcherPriority::Normal, ref new Windows::UI::Core::DispatchedHandler([this]()
	{
		LED0Button->Background = ((ledStateMain & 0x01) != 0) ? ref new SolidColorBrush(Windows::UI::Colors::Red) : ref new SolidColorBrush(Windows::UI::Colors::Black);
		LED1Button->Background = ((ledStateMain & 0x02) != 0) ? ref new SolidColorBrush(Windows::UI::Colors::Blue) : ref new SolidColorBrush(Windows::UI::Colors::Black);
		LED2Button->Background = ((ledStateMain & 0x04) != 0) ? ref new SolidColorBrush(Windows::UI::Colors::Green) : ref new SolidColorBrush(Windows::UI::Colors::Black);
		LED3Button->Background = ((ledStateMain & 0x08) != 0) ? ref new SolidColorBrush(Windows::UI::Colors::Orange) : ref new SolidColorBrush(Windows::UI::Colors::Black);
		LED4Button->Background = ((ledStateRemote & 0x01) != 0) ? ref new SolidColorBrush(Windows::UI::Colors::Red) : ref new SolidColorBrush(Windows::UI::Colors::Black);
		LED5Button->Background = ((ledStateRemote & 0x02) != 0) ? ref new SolidColorBrush(Windows::UI::Colors::Blue) : ref new SolidColorBrush(Windows::UI::Colors::Black);
		LED6Button->Background = ((ledStateRemote & 0x04) != 0) ? ref new SolidColorBrush(Windows::UI::Colors::Green) : ref new SolidColorBrush(Windows::UI::Colors::Black);
		LED7Button->Background = ((ledStateRemote & 0x08) != 0) ? ref new SolidColorBrush(Windows::UI::Colors::Yellow) : ref new SolidColorBrush(Windows::UI::Colors::Black);
	}));


}

void WebSocketClientCPP::MainPage::MessageWebSocketClientAsync()
{

	// You must register handlers before calling ConnectAsync       
	socket->MessageReceived += ref new TypedEventHandler<MessageWebSocket^, MessageWebSocketMessageReceivedEventArgs^>(this, &MainPage::OnMessageReceived);
	
	task<void>(socket->ConnectAsync(ref new Uri("ws://" + IPAddressText->Text + ":" + PortNoText->Text))).then([this](task<void> previousTask)
	{
		try
		{		
			OutputDebugString(L"Client Connected with Server");
			dataSend = ref new DataWriter(socket->OutputStream);

			Array<byte> ^bData = { 2, 0, 0 };
			sendMessage(bData);
		}
		catch (Exception^ exception)
		{
			
		}
	});	
	
}

void WebSocketClientCPP::MainPage::OnMessageReceived(MessageWebSocket ^sender, MessageWebSocketMessageReceivedEventArgs ^args)
{
	try
	{
		OutputDebugString(L"Message Received from Server");
		
		DataReader ^dr = args->GetDataReader();
		try
		{
			Array<byte>^ bits = ref new Array<byte>(dr->UnconsumedBufferLength);			
			dr->ReadBytes(bits);						

			ledStateMain = bits[1];
			ledStateRemote = bits[2];

			UpdateLEDState();
		}
		catch (Exception ^ex){
			OutputDebugString(L"Inside OnMessageReceived Error Occured");
		}
		
	}
	catch (Exception ^e)
	{
		OutputDebugString(L"OnMessageReceived Error Occured");
	}
}

void WebSocketClientCPP::MainPage::sendMessage(Array<byte> ^bytes)
{
	dataSend->WriteBytes(bytes);
	
	create_task(dataSend->StoreAsync()).then([this](unsigned int bytesStored)
	{
		// For the in-memory stream implementation we are using, the FlushAsync() call 
		// is superfluous, but other types of streams may require it.
		return dataSend->FlushAsync();
	});
}



void WebSocketClientCPP::MainPage::ConnectButton_Click(Platform::Object^ sender, Windows::UI::Xaml::RoutedEventArgs^ e)
{
	try
	{
		MessageWebSocketClientAsync();
		ConnectButton->Content = "Connected";
	}
	catch (Exception ^ex)
	{
		OutputDebugString(L"ConnectButton_Click Error");
	}
}


void WebSocketClientCPP::MainPage::RefreshButton_Click(Platform::Object^ sender, Windows::UI::Xaml::RoutedEventArgs^ e)
{
	if (dataSend != nullptr)
	{
		Array<byte> ^bData = { 2, 0, 0 };
		sendMessage(bData);
	}
}


void WebSocketClientCPP::MainPage::LED0Button_Click(Platform::Object^ sender, Windows::UI::Xaml::RoutedEventArgs^ e)
{
	if ((ledStateMain & 0x01) == 0)
	{
		ledStateMain = safe_cast<byte>(ledStateMain | 0x01);
		LED0Button->Background = ref new SolidColorBrush(Windows::UI::Colors::Red);
	}
	else
	{
		ledStateMain = safe_cast<byte>(ledStateMain & 0x0E);
		LED0Button->Background = ref new SolidColorBrush(Windows::UI::Colors::Black);
	}

	try
	{
		Array<byte> ^bData = { 1, ledStateMain, ledStateRemote };
		sendMessage(bData);
	}
	catch (Exception ^ex)
	{
		OutputDebugString(L"LEDButton_Click Error");
	}
}


void WebSocketClientCPP::MainPage::LED1Button_Click(Platform::Object^ sender, Windows::UI::Xaml::RoutedEventArgs^ e)
{
	if ((ledStateMain & 0x02) == 0)
	{
		ledStateMain = safe_cast<byte>(ledStateMain | 0x02);
		LED1Button->Background = ref new SolidColorBrush(Windows::UI::Colors::Blue);
	}
	else
	{
		ledStateMain = safe_cast<byte>(ledStateMain & 0x0D);
		LED1Button->Background = ref new SolidColorBrush(Windows::UI::Colors::Black);
	}

	try
	{
		Array<byte> ^bData = { 1, ledStateMain, ledStateRemote };
		sendMessage(bData);
	}
	catch (Exception ^ex)
	{
		OutputDebugString(L"LEDButton_Click Error");
	}
}


void WebSocketClientCPP::MainPage::LED2Button_Click(Platform::Object^ sender, Windows::UI::Xaml::RoutedEventArgs^ e)
{
	if ((ledStateMain & 0x04) == 0)
	{
		ledStateMain = safe_cast<byte>(ledStateMain | 0x04);
		LED2Button->Background = ref new SolidColorBrush(Windows::UI::Colors::Green);
	}
	else
	{
		ledStateMain = safe_cast<byte>(ledStateMain & 0x0B);
		LED2Button->Background = ref new SolidColorBrush(Windows::UI::Colors::Black);
	}

	try
	{
		Array<byte> ^bData = { 1, ledStateMain, ledStateRemote };
		sendMessage(bData);
	}
	catch (Exception ^ex)
	{
		OutputDebugString(L"LEDButton_Click Error");
	}
}


void WebSocketClientCPP::MainPage::LED3Button_Click(Platform::Object^ sender, Windows::UI::Xaml::RoutedEventArgs^ e)
{
	if ((ledStateMain & 0x08) == 0)
	{
		ledStateMain = safe_cast<byte>(ledStateMain | 0x08);
		LED3Button->Background = ref new SolidColorBrush(Windows::UI::Colors::Orange);
	}
	else
	{
		ledStateMain = safe_cast<byte>(ledStateMain & 0x07);
		LED3Button->Background = ref new SolidColorBrush(Windows::UI::Colors::Black);
	}

	try
	{
		Array<byte> ^bData = { 1, ledStateMain, ledStateRemote };
		sendMessage(bData);
	}
	catch (Exception ^ex)
	{
		OutputDebugString(L"LEDButton_Click Error");
	}
}


void WebSocketClientCPP::MainPage::LED4Button_Click(Platform::Object^ sender, Windows::UI::Xaml::RoutedEventArgs^ e)
{
	if ((ledStateRemote & 0x01) == 0)
	{
		ledStateRemote = safe_cast<byte>(ledStateRemote | 0x01);
		LED4Button->Background = ref new SolidColorBrush(Windows::UI::Colors::Red);
	}
	else
	{
		ledStateRemote = safe_cast<byte>(ledStateRemote & 0x0E);
		LED4Button->Background = ref new SolidColorBrush(Windows::UI::Colors::Black);
	}

	try
	{
		Array<byte> ^bData = { 1, ledStateMain, ledStateRemote };
		sendMessage(bData);
	}
	catch (Exception ^ex)
	{
		OutputDebugString(L"LEDButton_Click Error");
	}
}


void WebSocketClientCPP::MainPage::LED5Button_Click(Platform::Object^ sender, Windows::UI::Xaml::RoutedEventArgs^ e)
{
	if ((ledStateRemote & 0x02) == 0)
	{
		ledStateRemote = safe_cast<byte>(ledStateRemote | 0x02);
		LED5Button->Background = ref new SolidColorBrush(Windows::UI::Colors::Blue);
	}
	else
	{
		ledStateRemote = safe_cast<byte>(ledStateRemote & 0x0D);
		LED5Button->Background = ref new SolidColorBrush(Windows::UI::Colors::Black);
	}

	try
	{
		Array<byte> ^bData = { 1, ledStateMain, ledStateRemote };
		sendMessage(bData);
	}
	catch (Exception ^ex)
	{
		OutputDebugString(L"LEDButton_Click Error");
	}
}


void WebSocketClientCPP::MainPage::LED6Button_Click(Platform::Object^ sender, Windows::UI::Xaml::RoutedEventArgs^ e)
{
	if ((ledStateRemote & 0x04) == 0)
	{
		ledStateRemote = safe_cast<byte>(ledStateRemote | 0x04);
		LED6Button->Background = ref new SolidColorBrush(Windows::UI::Colors::Green);
	}
	else
	{
		ledStateRemote = safe_cast<byte>(ledStateRemote & 0x0B);
		LED6Button->Background = ref new SolidColorBrush(Windows::UI::Colors::Black);
	}

	try
	{
		Array<byte> ^bData = { 1, ledStateMain, ledStateRemote };
		sendMessage(bData);
	}
	catch (Exception ^ex)
	{
		OutputDebugString(L"LEDButton_Click Error");
	}
}


void WebSocketClientCPP::MainPage::LED7Button_Click(Platform::Object^ sender, Windows::UI::Xaml::RoutedEventArgs^ e)
{
	if ((ledStateRemote & 0x08) == 0)
	{
		ledStateRemote = safe_cast<byte>(ledStateRemote | 0x08);
		LED7Button->Background = ref new SolidColorBrush(Windows::UI::Colors::Yellow);
	}
	else
	{
		ledStateRemote = safe_cast<byte>(ledStateRemote & 0x07);
		LED7Button->Background = ref new SolidColorBrush(Windows::UI::Colors::Black);
	}

	try
	{
		Array<byte> ^bData = { 1, ledStateMain, ledStateRemote };
		sendMessage(bData);
	}
	catch (Exception ^ex)
	{
		OutputDebugString(L"LEDButton_Click Error");
	}
}
