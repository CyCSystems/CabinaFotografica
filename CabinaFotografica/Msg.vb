Public Class Msg

    Public restante As Integer = 0

    Private Sub Msg_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        lblMsg.Text = ""
        Timer1.Interval = 1000
        FormBorderStyle = Windows.Forms.FormBorderStyle.None
        Me.Top = (Screen.PrimaryScreen.WorkingArea.Height - Me.Height) / 2
        Me.Left = (Screen.PrimaryScreen.WorkingArea.Width - Me.Width) / 2
    End Sub

    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        If restante >= 0 Then

            lblMsg.Text = ""

            restante = restante - 1

        Else

            Timer1.Enabled = False

            Me.Close()

        End If

    End Sub
    Public Sub TimerOn()

        If restante > 0 Then

            Timer1.Enabled = True

        Else

            Timer1.Enabled = False

        End If
    End Sub
End Class