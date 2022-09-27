#include "FormMainServer.h"

using namespace System;
using namespace System::Windows::Forms;


[STAThread]
void Main(array<String^>^ args)
{
	Application::EnableVisualStyles();
	Application::SetCompatibleTextRenderingDefault(false);

	LEDServerCppApp::FormMainServer form;
	Application::Run(%form);
}