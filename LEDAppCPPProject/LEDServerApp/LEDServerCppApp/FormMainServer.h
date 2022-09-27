#include "Header.h"

#pragma once

namespace LEDServerCppApp {

	using namespace System;
	using namespace System::ComponentModel;
	using namespace System::Collections;
	using namespace System::Windows::Forms;
	using namespace System::Data;
	using namespace System::Drawing;
	using namespace System::Net::Sockets;
	using namespace Collections::Generic;
	using namespace System::Net;
	using namespace System::Text;
	using namespace System::Threading;
	using namespace System::Threading::Tasks;

	/// <summary>
	/// Summary for FormMainServer
	/// </summary>
	public ref class FormMainServer : public System::Windows::Forms::Form
	{
		static List<TcpClient^>^ clients = gcnew List<TcpClient^>();
		static TcpListener^ server;
		static Object^ serverLock = gcnew Object();
		static bool serverAvailable = false;
	public:
		FormMainServer(void)
		{
			InitializeComponent();
			//
			//TODO: Add the constructor code here
			//
			setInitLEDColors();

			//Gets the IP Address
			String^ hostName = Dns::GetHostName(); // Retrive the Name of HOST            
			IPHostEntry^ hostname = Dns::GetHostEntry(hostName);
			array<IPAddress^>^ ip = hostname->AddressList;
			for each (IPAddress^ item in ip)
			{
				if (item->AddressFamily == AddressFamily::InterNetwork){
					textHostIPAddress->Text = item->ToString();
				}
			}
		}

	private:
		void setInitLEDColors()
		{
			leftLED0->BackColor = Color::Red;
			leftLED1->BackColor = Color::Blue;
			leftLED2->BackColor = Color::Green;
			leftLED3->BackColor = Color::Orange;
			rightLED0->BackColor = Color::Black;
			rightLED1->BackColor = Color::Black;
			rightLED2->BackColor = Color::Black;
			rightLED3->BackColor = Color::Black;
		}

	private: void addIncomingClients()
	{
		while (serverAvailable){
			try
			{
				TcpClient^ client = server->AcceptTcpClient();
				
				//Let the client object listen for any incoming              
				Thread^ t = gcnew Thread(gcnew ParameterizedThreadStart(this, &FormMainServer::listenToClientForData));
				t->IsBackground = true;
				t->Start(client);

				{
					Lock lock(serverLock);
					clients->Add(client);
				}
			}
			catch (Exception^ e)
			{
				Console::WriteLine("FormMainServer.addIncomingClients()" + e->Message);
			}

		}
	}

	private: void listenToClientForData(Object^ pclient)
	{
		TcpClient^ client;
		NetworkStream^ stream;

		try
		{

			{
				Lock lock(serverLock);
				client = (TcpClient^)pclient;				
				stream = client->GetStream();				
			}

			while (true)
			{

				{
					Lock lock(serverLock);
					
					if (stream->DataAvailable)
					{						
						array<unsigned char>^ data= gcnew array<unsigned char>(3);
						stream->Read(data, 0, 3);
						
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
		catch (Exception^ e)
		{
			Console::WriteLine("FormMainServer.listenToClientForData()" + e->Message);
		}

	}

	private: void setLEDs(array<unsigned char>^ data){		
		//Setting Left LEDs
		setValue(((unsigned char)(data->GetValue(1)) & 0X01) > 0, 0);
		setValue(((unsigned char)(data->GetValue(1)) & 0X02) > 0, 1);
		setValue(((unsigned char)(data->GetValue(1)) & 0X04) > 0, 2);
		setValue(((unsigned char)(data->GetValue(1)) & 0X08) > 0, 3);

		//Setting Right LEDs
		setValue(((unsigned char)(data->GetValue(2)) & 0X01) > 0, 4);
		setValue(((unsigned char)(data->GetValue(2)) & 0X02) > 0, 5);
		setValue(((unsigned char)(data->GetValue(2)) & 0X04) > 0, 6);
		setValue(((unsigned char)(data->GetValue(2)) & 0X08) > 0, 7);
	}

			 delegate void SetValueCallback(bool b, int control);

	private: void setValue(bool b, int control)
	{
		// InvokeRequired required compares the thread ID of the
		// calling thread to the thread ID of the creating thread.
		// If these threads are different, it returns true.
		switch (control)
		{
		case 0:
			if (leftLED0->InvokeRequired)
			{
				SetValueCallback^ d = gcnew SetValueCallback(this, &FormMainServer::setValue);
				array<Object^> ^ data = { b, control };
				this->Invoke(d, data);
			}
			else
			{
				leftLED0->BackColor = b ? Color::Red : Color::Black;
			}
			break;
		case 1:
			if (leftLED1->InvokeRequired)
			{
				SetValueCallback^ d = gcnew SetValueCallback(this, &FormMainServer::setValue);
				array<Object^> ^ data = { b, control };
				this->Invoke(d, data);
			}
			else
			{
				leftLED1->BackColor = b ? Color::Blue : Color::Black;
			}
			break;
		case 2:
			if (leftLED2->InvokeRequired)
			{
				SetValueCallback^ d = gcnew SetValueCallback(this, &FormMainServer::setValue);
				array<Object^> ^ data = { b, control };
				this->Invoke(d, data);
			}
			else
			{
				leftLED2->BackColor = b ? Color::Green : Color::Black;
			}
			break;
		case 3:
			if (leftLED3->InvokeRequired)
			{
				SetValueCallback^ d = gcnew SetValueCallback(this, &FormMainServer::setValue);
				array<Object^> ^ data = { b, control };
				this->Invoke(d, data);
			}
			else
			{
				leftLED3->BackColor = b ? Color::Orange : Color::Black;
			}
			break;
		case 4:
			if (rightLED0->InvokeRequired)
			{
				SetValueCallback^ d = gcnew SetValueCallback(this, &FormMainServer::setValue);
				array<Object^> ^ data = { b, control };
				this->Invoke(d, data);
			}
			else
			{
				rightLED0->BackColor = b ? Color::Red : Color::Black;
			}
			break;
		case 5:
			if (rightLED1->InvokeRequired)
			{
				SetValueCallback^ d = gcnew SetValueCallback(this, &FormMainServer::setValue);
				array<Object^> ^ data = { b, control };
				this->Invoke(d, data);
			}
			else
			{
				rightLED1->BackColor = b ? Color::Blue : Color::Black;
			}
			break;
		case 6:
			if (rightLED2->InvokeRequired)
			{
				SetValueCallback^ d = gcnew SetValueCallback(this, &FormMainServer::setValue);
				array<Object^> ^ data = { b, control };
				this->Invoke(d, data);
			}
			else
			{
				rightLED2->BackColor = b ? Color::Green : Color::Black;
			}
			break;
		case 7:
			if (rightLED3->InvokeRequired)
			{
				SetValueCallback^ d = gcnew SetValueCallback(this, &FormMainServer::setValue);
				array<Object^> ^ data = { b, control };
				this->Invoke(d, data);
			}
			else
			{
				rightLED3->BackColor = b ? Color::Yellow : Color::Black;
			}
			break;
		}

	}

	private: void sendDataToAllClients()
	{

		{
			Lock lock(serverLock);
			for (int i = 0; i < clients->Count; i++)
			{
				try
				{
					TcpClient^ client = clients[i];
					NetworkStream^ stream = client->GetStream();
					//Sending Back the current LED Status
					array<unsigned char>^ data = getByteForGETLEDForClients();
					stream->Write(data, 0, data->GetLength(0));
				}
				catch (Exception^ e)
				{
					Console::WriteLine("FormMainServer.sendDataToAllClients()" + e->Message);
				}

			}
		}
	}

	private: void sendDataToClient(Object^ pclient)
	{
		TcpClient^ client ;
		NetworkStream^ stream;

		try
		{
			{
				Lock lock(serverLock);
				client = (TcpClient^)pclient;
				stream = client->GetStream();
				//Sending Back the current LED Status
				array<unsigned char>^ data = getByteForGETLEDForClients();
				stream->Write(data, 0, data->GetLength(0));				
			}
		}
		catch (Exception^ e)
		{
			Console::WriteLine("FormMainServer.sendDataToClient()" + e->Message);
		}

	}

	private: array<unsigned char>^ getByteForGETLEDForClients()
	{
		array<unsigned char>^ data = gcnew array<unsigned char>(3);
		data[0] = 3;//Command
		data[1] = 0X00;
		data[2] = 0X00;
		//Setting Left LEDS
		unsigned char val = (leftLED0->BackColor == Color::Red ? 0X01 : 0X00);
		data[1] = (data[1] | val);
		val = (leftLED1->BackColor == Color::Blue ? 0X02 : 0X00);
		data[1] = (data[1] | val);
		val = (leftLED2->BackColor == Color::Green ? 0X04 : 0X00);
		data[1] = (data[1] | val);
		val = (leftLED3->BackColor == Color::Orange ? 0X08 : 0X00);
		data[1] = (data[1] | val);
		//Setting Right LEDS
		val = (rightLED0->BackColor == Color::Red ? 0X01 : 0X00);
		data[2] = (data[2] | val);
		val = (rightLED1->BackColor == Color::Blue ? 0X02 : 0X00);
		data[2] = (data[2] | val);
		val = (rightLED2->BackColor == Color::Green ? 0X04 : 0X00);
		data[2] = (data[2] | val);
		val = (rightLED3->BackColor == Color::Yellow ? 0X08 : 0X00);
		data[2] = (data[2] | val);
		return data;
	}

	protected:
		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		~FormMainServer()
		{
			if (components)
			{
				delete components;
			}
		}
	private: System::Windows::Forms::Label^  label5;
	protected:
	private: System::Windows::Forms::Label^  label4;
	private: System::Windows::Forms::Label^  label3;
	private: System::Windows::Forms::Button^  rightLED3;
	private: System::Windows::Forms::Button^  rightLED2;
	private: System::Windows::Forms::Button^  rightLED1;
	private: System::Windows::Forms::Button^  rightLED0;
	private: System::Windows::Forms::Button^  leftLED3;
	private: System::Windows::Forms::Button^  leftLED2;
	private: System::Windows::Forms::Button^  leftLED1;
	private: System::Windows::Forms::Button^  leftLED0;
	private: System::Windows::Forms::Label^  label2;
	private: System::Windows::Forms::Label^  label1;
	private: System::Windows::Forms::Button^  buttonConnect;
	private: System::Windows::Forms::TextBox^  textPortNo;
	private: System::Windows::Forms::TextBox^  textHostIPAddress;

	protected:



	private:
		/// <summary>
		/// Required designer variable.
		/// </summary>
		System::ComponentModel::Container ^components;

#pragma region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		void InitializeComponent(void)
		{
			this->label5 = (gcnew System::Windows::Forms::Label());
			this->label4 = (gcnew System::Windows::Forms::Label());
			this->label3 = (gcnew System::Windows::Forms::Label());
			this->rightLED3 = (gcnew System::Windows::Forms::Button());
			this->rightLED2 = (gcnew System::Windows::Forms::Button());
			this->rightLED1 = (gcnew System::Windows::Forms::Button());
			this->rightLED0 = (gcnew System::Windows::Forms::Button());
			this->leftLED3 = (gcnew System::Windows::Forms::Button());
			this->leftLED2 = (gcnew System::Windows::Forms::Button());
			this->leftLED1 = (gcnew System::Windows::Forms::Button());
			this->leftLED0 = (gcnew System::Windows::Forms::Button());
			this->label2 = (gcnew System::Windows::Forms::Label());
			this->label1 = (gcnew System::Windows::Forms::Label());
			this->buttonConnect = (gcnew System::Windows::Forms::Button());
			this->textPortNo = (gcnew System::Windows::Forms::TextBox());
			this->textHostIPAddress = (gcnew System::Windows::Forms::TextBox());
			this->SuspendLayout();
			// 
			// label5
			// 
			this->label5->AutoSize = true;
			this->label5->BackColor = System::Drawing::SystemColors::ActiveCaptionText;
			this->label5->Font = (gcnew System::Drawing::Font(L"Microsoft Sans Serif", 9.75F, System::Drawing::FontStyle::Bold, System::Drawing::GraphicsUnit::Point,
				static_cast<System::Byte>(0)));
			this->label5->ForeColor = System::Drawing::SystemColors::HighlightText;
			this->label5->Location = System::Drawing::Point(60, 21);
			this->label5->Name = L"label5";
			this->label5->Size = System::Drawing::Size(186, 16);
			this->label5->TabIndex = 50;
			this->label5->Text = L"AutoHotBox LED Example";
			// 
			// label4
			// 
			this->label4->AutoSize = true;
			this->label4->BackColor = System::Drawing::SystemColors::ActiveCaptionText;
			this->label4->Font = (gcnew System::Drawing::Font(L"Microsoft Sans Serif", 9.75F, System::Drawing::FontStyle::Bold, System::Drawing::GraphicsUnit::Point,
				static_cast<System::Byte>(0)));
			this->label4->ForeColor = System::Drawing::SystemColors::HighlightText;
			this->label4->Location = System::Drawing::Point(202, 137);
			this->label4->Name = L"label4";
			this->label4->Size = System::Drawing::Size(62, 16);
			this->label4->TabIndex = 49;
			this->label4->Text = L"Remote";
			// 
			// label3
			// 
			this->label3->AutoSize = true;
			this->label3->BackColor = System::Drawing::SystemColors::ActiveCaptionText;
			this->label3->Font = (gcnew System::Drawing::Font(L"Microsoft Sans Serif", 9.75F, System::Drawing::FontStyle::Bold, System::Drawing::GraphicsUnit::Point,
				static_cast<System::Byte>(0)));
			this->label3->ForeColor = System::Drawing::SystemColors::HighlightText;
			this->label3->Location = System::Drawing::Point(60, 137);
			this->label3->Name = L"label3";
			this->label3->Size = System::Drawing::Size(41, 16);
			this->label3->TabIndex = 48;
			this->label3->Text = L"Main";
			// 
			// rightLED3
			// 
			this->rightLED3->BackColor = System::Drawing::SystemColors::ActiveCaptionText;
			this->rightLED3->ForeColor = System::Drawing::SystemColors::HighlightText;
			this->rightLED3->Location = System::Drawing::Point(194, 342);
			this->rightLED3->Name = L"rightLED3";
			this->rightLED3->Size = System::Drawing::Size(64, 53);
			this->rightLED3->TabIndex = 47;
			this->rightLED3->UseVisualStyleBackColor = false;
			this->rightLED3->Click += gcnew System::EventHandler(this, &FormMainServer::rightLED3_Click);
			// 
			// rightLED2
			// 
			this->rightLED2->BackColor = System::Drawing::SystemColors::ActiveCaptionText;
			this->rightLED2->ForeColor = System::Drawing::SystemColors::HighlightText;
			this->rightLED2->Location = System::Drawing::Point(194, 283);
			this->rightLED2->Name = L"rightLED2";
			this->rightLED2->Size = System::Drawing::Size(64, 53);
			this->rightLED2->TabIndex = 46;
			this->rightLED2->UseVisualStyleBackColor = false;
			this->rightLED2->Click += gcnew System::EventHandler(this, &FormMainServer::rightLED2_Click);
			// 
			// rightLED1
			// 
			this->rightLED1->BackColor = System::Drawing::SystemColors::ActiveCaptionText;
			this->rightLED1->ForeColor = System::Drawing::SystemColors::HighlightText;
			this->rightLED1->Location = System::Drawing::Point(194, 224);
			this->rightLED1->Name = L"rightLED1";
			this->rightLED1->Size = System::Drawing::Size(64, 53);
			this->rightLED1->TabIndex = 45;
			this->rightLED1->UseVisualStyleBackColor = false;
			this->rightLED1->Click += gcnew System::EventHandler(this, &FormMainServer::rightLED1_Click);
			// 
			// rightLED0
			// 
			this->rightLED0->BackColor = System::Drawing::SystemColors::ActiveCaptionText;
			this->rightLED0->ForeColor = System::Drawing::SystemColors::HighlightText;
			this->rightLED0->Location = System::Drawing::Point(194, 165);
			this->rightLED0->Name = L"rightLED0";
			this->rightLED0->Size = System::Drawing::Size(64, 53);
			this->rightLED0->TabIndex = 44;
			this->rightLED0->UseVisualStyleBackColor = false;
			this->rightLED0->Click += gcnew System::EventHandler(this, &FormMainServer::rightLED0_Click);
			// 
			// leftLED3
			// 
			this->leftLED3->BackColor = System::Drawing::SystemColors::ActiveCaptionText;
			this->leftLED3->ForeColor = System::Drawing::SystemColors::HighlightText;
			this->leftLED3->Location = System::Drawing::Point(47, 342);
			this->leftLED3->Name = L"leftLED3";
			this->leftLED3->Size = System::Drawing::Size(64, 53);
			this->leftLED3->TabIndex = 43;
			this->leftLED3->UseVisualStyleBackColor = false;
			this->leftLED3->Click += gcnew System::EventHandler(this, &FormMainServer::leftLED3_Click);
			// 
			// leftLED2
			// 
			this->leftLED2->BackColor = System::Drawing::SystemColors::ActiveCaptionText;
			this->leftLED2->ForeColor = System::Drawing::SystemColors::HighlightText;
			this->leftLED2->Location = System::Drawing::Point(47, 283);
			this->leftLED2->Name = L"leftLED2";
			this->leftLED2->Size = System::Drawing::Size(64, 53);
			this->leftLED2->TabIndex = 42;
			this->leftLED2->UseVisualStyleBackColor = false;
			this->leftLED2->Click += gcnew System::EventHandler(this, &FormMainServer::leftLED2_Click);
			// 
			// leftLED1
			// 
			this->leftLED1->BackColor = System::Drawing::SystemColors::ActiveCaptionText;
			this->leftLED1->ForeColor = System::Drawing::SystemColors::HighlightText;
			this->leftLED1->Location = System::Drawing::Point(47, 224);
			this->leftLED1->Name = L"leftLED1";
			this->leftLED1->Size = System::Drawing::Size(64, 53);
			this->leftLED1->TabIndex = 41;
			this->leftLED1->UseVisualStyleBackColor = false;
			this->leftLED1->Click += gcnew System::EventHandler(this, &FormMainServer::leftLED1_Click);
			// 
			// leftLED0
			// 
			this->leftLED0->BackColor = System::Drawing::SystemColors::ActiveCaptionText;
			this->leftLED0->ForeColor = System::Drawing::SystemColors::HighlightText;
			this->leftLED0->Location = System::Drawing::Point(47, 165);
			this->leftLED0->Name = L"leftLED0";
			this->leftLED0->Size = System::Drawing::Size(64, 53);
			this->leftLED0->TabIndex = 40;
			this->leftLED0->UseVisualStyleBackColor = false;
			this->leftLED0->Click += gcnew System::EventHandler(this, &FormMainServer::leftLED0_Click);
			// 
			// label2
			// 
			this->label2->AutoSize = true;
			this->label2->BackColor = System::Drawing::SystemColors::ActiveCaptionText;
			this->label2->ForeColor = System::Drawing::SystemColors::HighlightText;
			this->label2->Location = System::Drawing::Point(44, 97);
			this->label2->Name = L"label2";
			this->label2->Size = System::Drawing::Size(43, 13);
			this->label2->TabIndex = 39;
			this->label2->Text = L"Port No";
			// 
			// label1
			// 
			this->label1->AutoSize = true;
			this->label1->BackColor = System::Drawing::SystemColors::ActiveCaptionText;
			this->label1->ForeColor = System::Drawing::SystemColors::HighlightText;
			this->label1->Location = System::Drawing::Point(29, 71);
			this->label1->Name = L"label1";
			this->label1->Size = System::Drawing::Size(58, 13);
			this->label1->TabIndex = 38;
			this->label1->Text = L"IP Address";
			// 
			// buttonConnect
			// 
			this->buttonConnect->BackColor = System::Drawing::SystemColors::ActiveCaptionText;
			this->buttonConnect->ForeColor = System::Drawing::SystemColors::HighlightText;
			this->buttonConnect->Location = System::Drawing::Point(205, 66);
			this->buttonConnect->Name = L"buttonConnect";
			this->buttonConnect->Size = System::Drawing::Size(75, 23);
			this->buttonConnect->TabIndex = 37;
			this->buttonConnect->Text = L"Run";
			this->buttonConnect->UseVisualStyleBackColor = false;
			this->buttonConnect->Click += gcnew System::EventHandler(this, &FormMainServer::buttonConnect_Click);
			// 
			// textPortNo
			// 
			this->textPortNo->Location = System::Drawing::Point(97, 94);
			this->textPortNo->Name = L"textPortNo";
			this->textPortNo->Size = System::Drawing::Size(100, 20);
			this->textPortNo->TabIndex = 36;
			this->textPortNo->Text = L"8080";
			// 
			// textHostIPAddress
			// 
			this->textHostIPAddress->Location = System::Drawing::Point(97, 68);
			this->textHostIPAddress->Name = L"textHostIPAddress";
			this->textHostIPAddress->Size = System::Drawing::Size(100, 20);
			this->textHostIPAddress->TabIndex = 35;
			this->textHostIPAddress->Text = L"127.0.0.1";
			// 
			// FormMainServer
			// 
			this->AutoScaleDimensions = System::Drawing::SizeF(6, 13);
			this->AutoScaleMode = System::Windows::Forms::AutoScaleMode::Font;
			this->BackColor = System::Drawing::SystemColors::ActiveCaptionText;
			this->ClientSize = System::Drawing::Size(309, 416);
			this->Controls->Add(this->label5);
			this->Controls->Add(this->label4);
			this->Controls->Add(this->label3);
			this->Controls->Add(this->rightLED3);
			this->Controls->Add(this->rightLED2);
			this->Controls->Add(this->rightLED1);
			this->Controls->Add(this->rightLED0);
			this->Controls->Add(this->leftLED3);
			this->Controls->Add(this->leftLED2);
			this->Controls->Add(this->leftLED1);
			this->Controls->Add(this->leftLED0);
			this->Controls->Add(this->label2);
			this->Controls->Add(this->label1);
			this->Controls->Add(this->buttonConnect);
			this->Controls->Add(this->textPortNo);
			this->Controls->Add(this->textHostIPAddress);
			this->ForeColor = System::Drawing::SystemColors::HighlightText;
			this->Name = L"FormMainServer";
			this->Text = L"LED Server App";
			this->ResumeLayout(false);
			this->PerformLayout();

		}
#pragma endregion
	private: System::Void buttonConnect_Click(System::Object^  sender, System::EventArgs^  e) {
		try
		{
			String^ hostIP = textHostIPAddress->Text->Trim();
			int portNo = Convert::ToInt32(textPortNo->Text->Trim());
			server = gcnew TcpListener(IPAddress::Parse(hostIP), portNo);
			server->Start();

			serverAvailable = true;
			//Starting the Client listening to any incoming Data from Server.			
			Thread^ t1 = gcnew Thread(gcnew ThreadStart(this, &FormMainServer::addIncomingClients));
			t1->IsBackground = true;
			t1->Start();
						
			buttonConnect->Text = "Running";
		}
		catch (Exception^ ex){
			Console::WriteLine("FormMainServer.buttonConnect+Click() " + ex->Message);
			serverAvailable = false;
		}
	}
	private: System::Void leftLED0_Click(System::Object^  sender, System::EventArgs^  e) {
		//Toggle Back Color of the button
		leftLED0->BackColor = leftLED0->BackColor == Color::Black ? Color::Red : Color::Black;
	}
	private: System::Void leftLED1_Click(System::Object^  sender, System::EventArgs^  e) {
		//Toggle Back Color of the button
		leftLED1->BackColor = leftLED1->BackColor == Color::Black ? Color::Blue : Color::Black;
	}
	private: System::Void leftLED2_Click(System::Object^  sender, System::EventArgs^  e) {
		//Toggle Back Color of the button
		leftLED2->BackColor = leftLED2->BackColor == Color::Black ? Color::Green : Color::Black;
	}
	private: System::Void leftLED3_Click(System::Object^  sender, System::EventArgs^  e) {
		//Toggle Back Color of the button
		leftLED3->BackColor = leftLED3->BackColor == Color::Black ? Color::Orange : Color::Black;
	}
	private: System::Void rightLED0_Click(System::Object^  sender, System::EventArgs^  e) {
		//Toggle Back Color of the button
		rightLED0->BackColor = rightLED0->BackColor == Color::Black ? Color::Red : Color::Black;
	}
	private: System::Void rightLED1_Click(System::Object^  sender, System::EventArgs^  e) {
		//Toggle Back Color of the button
		rightLED1->BackColor = rightLED1->BackColor == Color::Black ? Color::Blue : Color::Black;
	}
	private: System::Void rightLED2_Click(System::Object^  sender, System::EventArgs^  e) {
		//Toggle Back Color of the button
		rightLED2->BackColor = rightLED2->BackColor == Color::Black ? Color::Green : Color::Black;
	}
	private: System::Void rightLED3_Click(System::Object^  sender, System::EventArgs^  e) {
		//Toggle Back Color of the button
		rightLED3->BackColor = rightLED3->BackColor == Color::Black ? Color::Yellow : Color::Black;
	}
	};
}
