﻿

#ExternalChecksum("I:\wORkShOP\Freelancer.Com\LEDAppProject\LEDAppWebSocketClientVB\WebSocketClientVB\MainPage.xaml", "{406ea660-64cf-4c82-b6f0-42d48172a799}", "E4CB9069473D48D0476A81975635185A")
'------------------------------------------------------------------------------
' <auto-generated>
'     This code was generated by a tool.
'
'     Changes to this file may cause incorrect behavior and will be lost if
'     the code is regenerated.
' </auto-generated>
'------------------------------------------------------------------------------

Option Strict Off
Option Explicit On

Namespace Global.WebSocketClientVB

    Partial Class MainPage
        Implements Global.Windows.UI.Xaml.Markup.IComponentConnector

        <Global.System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks", "4.0.0.0")>  _
        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute()>  _
        Public Sub Connect(ByVal connectionId As Integer, ByVal target As Global.System.Object) Implements Global.Windows.UI.Xaml.Markup.IComponentConnector.Connect
            If(connectionId = 1) Then
                #ExternalSource("..\..\..\MainPage.xaml",11)
                AddHandler CType(target,Global.Windows.UI.Xaml.Controls.Primitives.ButtonBase).Click, AddressOf Me.LED0Button_Click
                #End ExternalSource
            Else If(connectionId = 2) Then
                #ExternalSource("..\..\..\MainPage.xaml",17)
                AddHandler CType(target,Global.Windows.UI.Xaml.Controls.Primitives.ButtonBase).Click, AddressOf Me.ConnectButton_Click
                #End ExternalSource
            Else If(connectionId = 3) Then
                #ExternalSource("..\..\..\MainPage.xaml",18)
                AddHandler CType(target,Global.Windows.UI.Xaml.Controls.Primitives.ButtonBase).Click, AddressOf Me.RefreshButton_Click
                #End ExternalSource
            Else If(connectionId = 4) Then
                #ExternalSource("..\..\..\MainPage.xaml",19)
                AddHandler CType(target,Global.Windows.UI.Xaml.Controls.Primitives.ButtonBase).Click, AddressOf Me.LED4Button_Click
                #End ExternalSource
            Else If(connectionId = 5) Then
                #ExternalSource("..\..\..\MainPage.xaml",22)
                AddHandler CType(target,Global.Windows.UI.Xaml.Controls.Primitives.ButtonBase).Click, AddressOf Me.LED1Button_Click
                #End ExternalSource
            Else If(connectionId = 6) Then
                #ExternalSource("..\..\..\MainPage.xaml",23)
                AddHandler CType(target,Global.Windows.UI.Xaml.Controls.Primitives.ButtonBase).Click, AddressOf Me.LED5Button_Click
                #End ExternalSource
            Else If(connectionId = 7) Then
                #ExternalSource("..\..\..\MainPage.xaml",24)
                AddHandler CType(target,Global.Windows.UI.Xaml.Controls.Primitives.ButtonBase).Click, AddressOf Me.LED2Button_Click
                #End ExternalSource
            Else If(connectionId = 8) Then
                #ExternalSource("..\..\..\MainPage.xaml",25)
                AddHandler CType(target,Global.Windows.UI.Xaml.Controls.Primitives.ButtonBase).Click, AddressOf Me.LED6Button_Click
                #End ExternalSource
            Else If(connectionId = 9) Then
                #ExternalSource("..\..\..\MainPage.xaml",26)
                AddHandler CType(target,Global.Windows.UI.Xaml.Controls.Primitives.ButtonBase).Click, AddressOf Me.LED3Button_Click
                #End ExternalSource
            Else If(connectionId = 10) Then
                #ExternalSource("..\..\..\MainPage.xaml",27)
                AddHandler CType(target,Global.Windows.UI.Xaml.Controls.Primitives.ButtonBase).Click, AddressOf Me.LED7Button_Click
                #End ExternalSource
            End If
            Me._contentLoaded = true
        End Sub
    End Class

End Namespace


