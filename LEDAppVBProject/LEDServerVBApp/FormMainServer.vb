Imports System.Net.Sockets
Imports System.Net
Imports System.Threading

Public Class FormMainServer
    Shared clients As New List(Of TcpClient)
    Shared server As TcpListener
    Shared ReadOnly serverLock As Object = New Object()
    Shared serverAvailable As Boolean = False


    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        setInitLEDColors()

        'Gets the IP Address
        Dim hostName As String = Dns.GetHostName() ' Retrive the Name of HOST            
        Dim hostnameIP As IPHostEntry = Dns.GetHostEntry(hostName)
        Dim ip() As IPAddress = hostnameIP.AddressList
        For Each item As IPAddress In ip

            If (item.AddressFamily = AddressFamily.InterNetwork) Then
                textHostIPAddress.Text = item.ToString()
            End If
        Next
    End Sub

    Private Sub setInitLEDColors()
        leftLED0.BackColor = Color.Red
        leftLED1.BackColor = Color.Blue
        leftLED2.BackColor = Color.Green
        leftLED3.BackColor = Color.Orange
        rightLED0.BackColor = Color.Black
        rightLED1.BackColor = Color.Black
        rightLED2.BackColor = Color.Black
        rightLED3.BackColor = Color.Black
    End Sub


    Private Sub buttonConnect_Click(sender As Object, e As EventArgs) Handles buttonConnect.Click
        Try

            Dim hostIP As String = textHostIPAddress.Text.Trim()
            Dim portNo As Integer = Convert.ToInt32(textPortNo.Text.Trim())
            server = New TcpListener(IPAddress.Parse(hostIP), portNo)
            server.Start()

            serverAvailable = True
            'Starting the Client listening to any incoming Data from Server.
            Dim t As Thread = New Thread(AddressOf Me.addIncomingClients)
            t.IsBackground = True
            t.Start()

            buttonConnect.Text = "Running"
        Catch ex As Exception
            Console.WriteLine("FormMainServer.buttonConnect+Click() " + ex.Message)
            serverAvailable = False
        End Try
    End Sub

    Private Sub addIncomingClients()

        While (serverAvailable)
            Try

                Dim client As TcpClient = server.AcceptTcpClient()
                'Let the client object listen for any incoming              
                Dim t As Thread = New Thread(AddressOf Me.listenToClientForData)
                t.IsBackground = True
                t.Start(client)

                SyncLock (serverLock)
                    clients.Add(client)
                End SyncLock
            Catch e As Exception
                Console.WriteLine("FormMainServer.addIncomingClients()" + e.Message)
            End Try
        End While
    End Sub

    Private Sub listenToClientForData(pclient As Object)

        Dim client As TcpClient
        Dim stream As NetworkStream

        Try

            SyncLock (serverLock)

                client = CType(pclient, TcpClient)
                stream = client.GetStream()
            End SyncLock

            While (True)


                SyncLock (serverLock)

                    If (stream.DataAvailable) Then

                        Dim mData(3) As Byte
                        stream.Read(mData, 0, mData.Length)                      
                        Select Case (mData(0))

                            Case 1 'Set LED
                                setLEDs(mData)
                                'Updates all Clients
                                sendDataToAllClients()
                                Exit Select
                            Case 2 'Get LED
                                sendDataToClient(client)
                                Exit Select
                        End Select
                    End If

                End SyncLock
            End While

        Catch e As Exception
            Console.WriteLine("FormMainServer.listenToClientForData()" + e.Message)
        End Try
    End Sub

    Private Sub setLEDs(mData As Byte())

        'Setting Left LEDs
        setValue((mData(1) And &H1) > 0, 0)
        setValue((mData(1) And &H2) > 0, 1)
        setValue((mData(1) And &H4) > 0, 2)
        setValue((mData(1) And &H8) > 0, 3)

        'Setting Right LEDs
        setValue((mData(2) And &H1) > 0, 4)
        setValue((mData(2) And &H2) > 0, 5)
        setValue((mData(2) And &H4) > 0, 6)
        setValue((mData(2) And &H8) > 0, 7)
    End Sub

    Delegate Sub SetValueCallback(b As Boolean, control As Integer)

    Private Sub setValue(b As Boolean, control As Integer)
        ' InvokeRequired required compares the thread ID of the
        ' calling thread to the thread ID of the creating thread.
        ' If these threads are different, it returns true.
        Select Case (control)

            Case 0
                If (leftLED0.InvokeRequired) Then

                    Dim d As SetValueCallback = New SetValueCallback(AddressOf setValue)
                    Me.Invoke(d, New Object() {b, control})

                Else

                    leftLED0.BackColor = IIf(b, Color.Red, Color.Black)
                End If
                Exit Select
            Case 1
                If (leftLED1.InvokeRequired) Then

                    Dim d As SetValueCallback = New SetValueCallback(AddressOf setValue)
                    Me.Invoke(d, New Object() {b, control})

                Else

                    leftLED1.BackColor = IIf(b, Color.Blue, Color.Black)
                End If
                Exit Select
            Case 2
                If (leftLED2.InvokeRequired) Then

                    Dim d As SetValueCallback = New SetValueCallback(AddressOf setValue)
                    Me.Invoke(d, New Object() {b, control})

                Else

                    leftLED2.BackColor = IIf(b, Color.Green, Color.Black)
                End If
                Exit Select
            Case 3
                If (leftLED3.InvokeRequired) Then

                    Dim d As SetValueCallback = New SetValueCallback(AddressOf setValue)
                    Me.Invoke(d, New Object() {b, control})

                Else

                    leftLED3.BackColor = IIf(b, Color.Orange, Color.Black)
                End If
                Exit Select
            Case 4
                If (rightLED0.InvokeRequired) Then

                    Dim d As SetValueCallback = New SetValueCallback(AddressOf setValue)
                    Me.Invoke(d, New Object() {b, control})

                Else

                    rightLED0.BackColor = IIf(b, Color.Red, Color.Black)
                End If
                Exit Select
            Case 5
                If (rightLED1.InvokeRequired) Then

                    Dim d As SetValueCallback = New SetValueCallback(AddressOf setValue)
                    Me.Invoke(d, New Object() {b, control})

                Else

                    rightLED1.BackColor = IIf(b, Color.Blue, Color.Black)
                End If
                Exit Select
            Case 6
                If (rightLED2.InvokeRequired) Then

                    Dim d As SetValueCallback = New SetValueCallback(AddressOf setValue)
                    Me.Invoke(d, New Object() {b, control})

                Else

                    rightLED2.BackColor = IIf(b, Color.Green, Color.Black)
                End If
                Exit Select
            Case 7
                If (rightLED3.InvokeRequired) Then

                    Dim d As SetValueCallback = New SetValueCallback(AddressOf setValue)
                    Me.Invoke(d, New Object() {b, control})

                Else
                    rightLED3.BackColor = IIf(b, Color.Yellow, Color.Black)
                End If
                Exit Select
        End Select

    End Sub

    Private Sub sendDataToClient(ByVal pclient As Object)

        Dim client As TcpClient
        Dim stream As NetworkStream

        Try

            SyncLock (serverLock)

                client = CType(pclient, TcpClient)
                stream = client.GetStream()
                'Sending Back the current LED Status
                Dim data() As Byte = getByteForGETLEDForClients()
                stream.Write(data, 0, data.Length)
            End SyncLock

        Catch e As Exception

            Console.WriteLine("FormMainServer.sendDataToClient()" + e.Message)
        End Try

    End Sub

    Private Sub sendDataToAllClients()

        SyncLock (serverLock)

            For i As Integer = 0 To clients.Count - 1

                Try
                    Dim client As TcpClient = clients.ElementAt(i)
                    Dim stream As NetworkStream = client.GetStream()
                    'Sending Back the current LED Status
                    Dim mData() As Byte = getByteForGETLEDForClients()
                    stream.Write(mData, 0, mData.Length)                    
                Catch e As Exception
                    Console.WriteLine("FormMainServer.sendDataToAllClients()" + e.Message)
                End Try

            Next
        End SyncLock
    End Sub

    Private Function getByteForGETLEDForClients() As Byte()

        Dim mData(3) As Byte
        mData(0) = 3 'Command
        mData(1) = &H0
        mData(2) = &H0

        'Setting Left LEDS
        Dim val As Byte = IIf(leftLED0.BackColor = Color.Red, &H1, &H0)
        mData(1) = (mData(1) Or val)
        val = IIf(leftLED1.BackColor = Color.Blue, &H2, &H0)
        mData(1) = (mData(1) Or val)
        val = IIf(leftLED2.BackColor = Color.Green, &H4, &H0)
        mData(1) = (mData(1) Or val)
        val = IIf(leftLED3.BackColor = Color.Orange, &H8, &H0)
        mData(1) = (mData(1) Or val)
        'Setting Right LEDS
        val = IIf(rightLED0.BackColor = Color.Red, &H1, &H0)
        mData(2) = (mData(2) Or val)
        val = IIf(rightLED1.BackColor = Color.Blue, &H2, &H0)
        mData(2) = (mData(2) Or val)
        val = IIf(rightLED2.BackColor = Color.Green, &H4, &H0)
        mData(2) = (mData(2) Or val)
        val = IIf(rightLED3.BackColor = Color.Yellow, &H8, &H0)
        mData(2) = (mData(2) Or val)

        'Returning the value        
        getByteForGETLEDForClients = mData
    End Function

    Private Sub leftLED0_Click(sender As Object, e As EventArgs) Handles leftLED0.Click
        'Toggle Back Color of the button
        leftLED0.BackColor = IIf(leftLED0.BackColor = Color.Black, Color.Red, Color.Black)
    End Sub

    Private Sub leftLED1_Click(sender As Object, e As EventArgs) Handles leftLED1.Click
        'Toggle Back Color of the button
        leftLED1.BackColor = IIf(leftLED1.BackColor = Color.Black, Color.Blue, Color.Black)
    End Sub


    Private Sub leftLED2_Click(sender As Object, e As EventArgs) Handles leftLED2.Click
        'Toggle Back Color of the button
        leftLED2.BackColor = IIf(leftLED2.BackColor = Color.Black, Color.Green, Color.Black)
    End Sub

    Private Sub leftLED3_Click(sender As Object, e As EventArgs) Handles leftLED3.Click
        'Toggle Back Color of the button
        leftLED3.BackColor = IIf(leftLED3.BackColor = Color.Black, Color.Orange, Color.Black)
    End Sub


    Private Sub rightLED0_Click(sender As Object, e As EventArgs) Handles rightLED0.Click
        'Toggle Back Color of the button
        rightLED0.BackColor = IIf(rightLED0.BackColor = Color.Black, Color.Red, Color.Black)
    End Sub

    Private Sub rightLED1_Click(sender As Object, e As EventArgs) Handles rightLED1.Click
        'Toggle Back Color of the button
        rightLED1.BackColor = IIf(rightLED1.BackColor = Color.Black, Color.Blue, Color.Black)
    End Sub

    Private Sub rightLED2_Click(sender As Object, e As EventArgs) Handles rightLED2.Click
        'Toggle Back Color of the button
        rightLED2.BackColor = IIf(rightLED2.BackColor = Color.Black, Color.Green, Color.Black)
    End Sub

    Private Sub rightLED3_Click(sender As Object, e As EventArgs) Handles rightLED3.Click
        'Toggle Back Color of the button
        rightLED3.BackColor = IIf(rightLED3.BackColor = Color.Black, Color.Yellow, Color.Black)
    End Sub
End Class
