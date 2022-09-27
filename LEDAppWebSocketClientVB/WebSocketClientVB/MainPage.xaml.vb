Imports Windows.Networking.Sockets
Imports Windows.Storage.Streams
Imports Windows.UI.Core

' The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

''' <summary>
''' An empty page that can be used on its own or navigated to within a Frame.
''' </summary>
Public NotInheritable Class MainPage
    Inherits Page

    Dim socket As MessageWebSocket = New MessageWebSocket()
    Dim dataSend As DataWriter

    Shared ledStateMain As Byte = &H0
    Shared ledStateRemote As Byte = &H0


    'Methods
    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        UpdateLEDState()
    End Sub


    Private Async Sub UpdateLEDState()
        ' Update Main LEDs
        Await Windows.ApplicationModel.Core.
            CoreApplication.MainView.CoreWindow.Dispatcher.
            RunAsync(CoreDispatcherPriority.Normal,
                     Function()
                         LED0Button.Background = If(((ledStateMain And &H1) <> 0), New SolidColorBrush(Windows.UI.Colors.Red), New SolidColorBrush(Windows.UI.Colors.Black))
                         LED1Button.Background = If(((ledStateMain And &H2) <> 0), New SolidColorBrush(Windows.UI.Colors.Blue), New SolidColorBrush(Windows.UI.Colors.Black))
                         LED2Button.Background = If(((ledStateMain And &H4) <> 0), New SolidColorBrush(Windows.UI.Colors.Green), New SolidColorBrush(Windows.UI.Colors.Black))
                         LED3Button.Background = If(((ledStateMain And &H8) <> 0), New SolidColorBrush(Windows.UI.Colors.Orange), New SolidColorBrush(Windows.UI.Colors.Black))

                         ' Update Remote LEDs
                         LED4Button.Background = If(((ledStateRemote And &H1) <> 0), New SolidColorBrush(Windows.UI.Colors.Red), New SolidColorBrush(Windows.UI.Colors.Black))
                         LED5Button.Background = If(((ledStateRemote And &H2) <> 0), New SolidColorBrush(Windows.UI.Colors.Blue), New SolidColorBrush(Windows.UI.Colors.Black))
                         LED6Button.Background = If(((ledStateRemote And &H4) <> 0), New SolidColorBrush(Windows.UI.Colors.Green), New SolidColorBrush(Windows.UI.Colors.Black))
                         LED7Button.Background = If(((ledStateRemote And &H8) <> 0), New SolidColorBrush(Windows.UI.Colors.Yellow), New SolidColorBrush(Windows.UI.Colors.Black))

                     End Function)

    End Sub

    Private Async Sub MessageWebSocketClientAsync()

        Debug.WriteLine("Waiting")
        ' You must register handlers before calling ConnectAsync             
        AddHandler socket.MessageReceived, AddressOf OnMessageReceived
        Debug.WriteLine("Waiting")

        Await socket.ConnectAsync(New Uri("ws://" + IPAddressText.Text.Trim() + ":" + PortNoText.Text.Trim()))

        Debug.WriteLine("Client Connected with Server")
        dataSend = New DataWriter(socket.OutputStream)

        Dim bData As Byte() = {2, 0, 0}
        sendMessage(bData)

    End Sub

    Private Sub OnMessageReceived(sender As MessageWebSocket, args As MessageWebSocketMessageReceivedEventArgs)
        Try
            Debug.WriteLine("Message Received from Server")

            Using dr As DataReader = args.GetDataReader()
                Dim bits As Byte() = New Byte(dr.UnconsumedBufferLength - 1) {}
                dr.ReadBytes(bits)

                ledStateMain = bits(1)
                ledStateRemote = bits(2)

                UpdateLEDState()
            End Using
        Catch e As Exception
            Debug.WriteLine("OnMessageReceived " + e.Message)
        End Try
    End Sub

    Private Async Sub sendMessage(bytes As Byte())
        dataSend.WriteBytes(bytes)
        Await dataSend.StoreAsync()
        ' Send a "message" to the service           
    End Sub

    Private Sub ConnectButton_Click(sender As Object, e As RoutedEventArgs)
        Try
            MessageWebSocketClientAsync()
            ConnectButton.Content = "Connected"
        Catch ex As Exception
            Debug.WriteLine("ConnectButton_Click " + ex.Message)
        End Try
    End Sub

    Private Sub RefreshButton_Click(sender As Object, e As RoutedEventArgs)
        If dataSend IsNot Nothing Then
            Dim bData As Byte() = {2, 0, 0}
            sendMessage(bData)
        End If
    End Sub

    Private Sub LED0Button_Click(sender As Object, e As RoutedEventArgs)

        If (ledStateMain And &H1) = 0 Then
            ledStateMain = CByte(ledStateMain Or &H1)
            LED0Button.Background = New SolidColorBrush(Windows.UI.Colors.Red)
        Else
            ledStateMain = CByte(ledStateMain And &HE)
            LED0Button.Background = New SolidColorBrush(Windows.UI.Colors.Black)
        End If

        Try
            Dim bData As Byte() = {1, ledStateMain, ledStateRemote}
            sendMessage(bData)
        Catch ex As Exception
            Debug.WriteLine(ex.Message)
        End Try

    End Sub

    Private Sub LED1Button_Click(sender As Object, e As RoutedEventArgs)
        If (ledStateMain And &H2) = 0 Then
            ledStateMain = CByte(ledStateMain Or &H2)
            LED1Button.Background = New SolidColorBrush(Windows.UI.Colors.Blue)
        Else
            ledStateMain = CByte(ledStateMain And &HD)
            LED1Button.Background = New SolidColorBrush(Windows.UI.Colors.Black)
        End If

        Try
            Dim bData As Byte() = {1, ledStateMain, ledStateRemote}
            sendMessage(bData)
        Catch ex As Exception
            Debug.WriteLine(ex.Message)
        End Try

    End Sub

    Private Sub LED2Button_Click(sender As Object, e As RoutedEventArgs)
        If (ledStateMain And &H4) = 0 Then
            ledStateMain = CByte(ledStateMain Or &H4)
            LED2Button.Background = New SolidColorBrush(Windows.UI.Colors.Green)
        Else
            ledStateMain = CByte(ledStateMain And &HB)
            LED2Button.Background = New SolidColorBrush(Windows.UI.Colors.Black)
        End If

        Try
            Dim bData As Byte() = {1, ledStateMain, ledStateRemote}
            sendMessage(bData)
        Catch ex As Exception
            Debug.WriteLine(ex.Message)
        End Try
    End Sub

    Private Sub LED3Button_Click(sender As Object, e As RoutedEventArgs)
        If (ledStateMain And &H8) = 0 Then
            ledStateMain = CByte(ledStateMain Or &H8)
            LED3Button.Background = New SolidColorBrush(Windows.UI.Colors.Orange)
        Else
            ledStateMain = CByte(ledStateMain And &H7)
            LED3Button.Background = New SolidColorBrush(Windows.UI.Colors.Black)
        End If

        Try
            Dim bData As Byte() = {1, ledStateMain, ledStateRemote}
            sendMessage(bData)
        Catch ex As Exception
            Debug.WriteLine(ex.Message)
        End Try
    End Sub

    Private Sub LED4Button_Click(sender As Object, e As RoutedEventArgs)
        If (ledStateRemote And &H1) = 0 Then
            ledStateRemote = CByte(ledStateRemote Or &H1)
            LED4Button.Background = New SolidColorBrush(Windows.UI.Colors.Red)
        Else
            ledStateRemote = CByte(ledStateRemote And &HE)
            LED4Button.Background = New SolidColorBrush(Windows.UI.Colors.Black)
        End If

        Try
            Dim bData As Byte() = {1, ledStateMain, ledStateRemote}
            sendMessage(bData)
        Catch ex As Exception
            Debug.WriteLine(ex.Message)
        End Try
    End Sub

    Private Sub LED5Button_Click(sender As Object, e As RoutedEventArgs)
        If (ledStateRemote And &H2) = 0 Then
            ledStateRemote = CByte(ledStateRemote Or &H2)
            LED5Button.Background = New SolidColorBrush(Windows.UI.Colors.Blue)
        Else
            ledStateRemote = CByte(ledStateRemote And &HD)
            LED5Button.Background = New SolidColorBrush(Windows.UI.Colors.Black)
        End If

        Try
            Dim bData As Byte() = {1, ledStateMain, ledStateRemote}
            sendMessage(bData)
        Catch ex As Exception
            Debug.WriteLine(ex.Message)
        End Try
    End Sub

    Private Sub LED6Button_Click(sender As Object, e As RoutedEventArgs)
        If (ledStateRemote And &H4) = 0 Then
            ledStateRemote = CByte(ledStateRemote Or &H4)
            LED6Button.Background = New SolidColorBrush(Windows.UI.Colors.Green)
        Else
            ledStateRemote = CByte(ledStateRemote And &HB)
            LED6Button.Background = New SolidColorBrush(Windows.UI.Colors.Black)
        End If

        Try
            Dim bData As Byte() = {1, ledStateMain, ledStateRemote}
            sendMessage(bData)
        Catch ex As Exception
            Debug.WriteLine(ex.Message)
        End Try
    End Sub

    Private Sub LED7Button_Click(sender As Object, e As RoutedEventArgs)
        If (ledStateRemote And &H8) = 0 Then
            ledStateRemote = CByte(ledStateRemote Or &H8)
            LED7Button.Background = New SolidColorBrush(Windows.UI.Colors.Yellow)
        Else
            ledStateRemote = CByte(ledStateRemote And &H7)
            LED7Button.Background = New SolidColorBrush(Windows.UI.Colors.Black)
        End If

        Try
            Dim bData As Byte() = {1, ledStateMain, ledStateRemote}
            sendMessage(bData)
        Catch ex As Exception
            Debug.WriteLine(ex.Message)
        End Try
    End Sub

End Class
