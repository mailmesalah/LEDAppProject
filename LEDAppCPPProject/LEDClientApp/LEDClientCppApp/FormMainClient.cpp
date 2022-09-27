#include "FormMainClient.h"

using namespace System;
using namespace System::Windows::Forms;


[STAThread]
void Main(array<String^>^ args)
{
	Application::EnableVisualStyles();
	Application::SetCompatibleTextRenderingDefault(false);

	LEDClientCppApp::FormMainClient form;
	Application::Run(%form);
}