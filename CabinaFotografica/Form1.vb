Imports System.IO
Imports System.Drawing.Printing

Public Class CabinaFotografica

    Dim Nombres As String
    Dim a As Integer, b As Integer, c As Integer
    Dim Lista As ArrayList = New ArrayList(c)
    Dim archivos() As String
    Dim Fecha As String = Date.Now
    Dim ruta As String = "C:\Imagenes\" & Format(CDate(Fecha), "yyyy-MM-dd") & "\" & Format(CDate(Fecha), "HH-mm") & "\"

    Private Sub Form1_Disposed(sender As Object, e As EventArgs) Handles Me.Disposed
        WebCam1.Stop()
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles Me.Load
        lblCantidadFotos.Text = "No. de Fotografias Tomadas : 0"
        WebCam1.Start()
    End Sub

    Private Sub Form1_SizeChanged(sender As Object, e As EventArgs) Handles Me.SizeChanged
        gbGeneral.Width = Me.Width - 45
        gbGeneral.Height = Me.Height - 50
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles btnCaptura.Click
        Dim Cantidad As String = ""
        Dim Nom As String
        Dim Posicion As Integer = 0
        Dim Contador As Integer = 0
        System.IO.Directory.CreateDirectory(ruta)
        Cantidad = InputBox("Ingrese la Cantidad de Fotografias Requerida", "Cantidad de Fotos")
        Contador = IIf(IsNumeric(Cantidad), Cantidad, 0)
        If Contador > 0 Then
            While Contador > 0
                Dim msg As New Msg
                msg.restante = 3
                msg.TimerOn()
                msg.ShowDialog()
                pbFoto.Image = WebCam1.Imagen
                PonerMarca(pbFoto, PictureBox1)
                Nom = ruta & Contador & ".jpg"
                Redimensionar(pbFoto.Image, Nom)
                Contador = Contador - 1
                Posicion = Posicion + 1
                lblCantidadFotos.Text = "No. de Fotografias Tomadas : " & Posicion
                Galeria(pbFoto)
            End While
        End If
        PrintDocument1.Print()
    End Sub

    Private Sub Galeria(ByVal img As PictureBox)
        If PictureBox1.Image Is Nothing Then
            PictureBox1.Image = img.Image
        ElseIf PictureBox2.Image Is Nothing Then
            PictureBox2.Image = img.Image
        ElseIf PictureBox3.Image Is Nothing Then
            PictureBox3.Image = img.Image
        ElseIf PictureBox4.Image Is Nothing Then
            PictureBox4.Image = img.Image
        ElseIf PictureBox5.Image Is Nothing Then
            PictureBox5.Image = img.Image
        ElseIf PictureBox6.Image Is Nothing Then
            PictureBox6.Image = img.Image
        End If

    End Sub

    Private Sub Marca(ByVal watermark_bm As Bitmap, _
        ByVal result_bm As Bitmap, ByVal x As Integer, ByVal y _
        As Integer)
        Const ALPHA As Byte = 160
        ' Set the watermark's pixels' Alpha components.
        Dim clr As Color
        For py As Integer = 0 To watermark_bm.Height - 1
            For px As Integer = 0 To watermark_bm.Width - 1
                clr = watermark_bm.GetPixel(px, py)
                watermark_bm.SetPixel(px, py, _
                    Color.FromArgb(ALPHA, clr.R, clr.G, clr.B))
            Next px
        Next py

        ' Set the watermark's transparent color.
        watermark_bm.MakeTransparent(watermark_bm.GetPixel(0, _
            0))

        ' Copy onto the result image.
        Dim gr As Graphics = Graphics.FromImage(result_bm)
        gr.DrawImage(watermark_bm, x, y)
    End Sub

    Private Sub PonerMarca(ByVal original As PictureBox, ByVal Marco As PictureBox)
        Dim watermark_bm As New Bitmap(Marco.Image)
        Dim result_bm As New Bitmap(original.Image)

        Dim x As Integer = (result_bm.Width - watermark_bm.Width) \ 2
        Dim y As Integer = (result_bm.Height - watermark_bm.Height) \ 2
        Marca(watermark_bm, result_bm, x, y)

        original.Image = result_bm
    End Sub

    Private Sub Redimensionar(ByVal Imagen As Image, ByVal ruta As String)
        Dim imagenRedimensionada As Bitmap

        'creamos una imagen con las dimensiones que se desean
        'en este caso la creamos de 100x100 pixels
        imagenRedimensionada = New Bitmap(384, 576)

        'creamos un objeto graphics desde la nueva imagen
        Using gr As Graphics = Graphics.FromImage(imagenRedimensionada)
            'en la nueva imagen "pintamos" la antigua imagen con las dimensiones de la nueva imagen
            gr.DrawImage(Imagen, 0, 0, imagenRedimensionada.Width, imagenRedimensionada.Height)
        End Using

        'se guarda la nueva imagen
        imagenRedimensionada.Save(ruta)
    End Sub

    Private Sub PrintDocument1_PrintPage(sender As Object, ev As PrintPageEventArgs) Handles PrintDocument1.PrintPage
        Dim lista As New ListBox
        Dim PosX As Integer = 0
        Dim PosY As Integer = 0
        Try
            For Each Archivo As String In My.Computer.FileSystem.GetFiles(ruta, FileIO.SearchOption.SearchTopLevelOnly, "*.jpg")
                lista.Items.Add(Archivo)
                Dim newImage As Image = Image.FromFile(ruta)
                ev.Graphics.DrawImage(newImage, PosY, PosX)
                PosX = PosX + 200
                PosY = PosY + 200
            Next
            ' Con esta propiedad estableces si existen una o más páginas
            ev.HasMorePages = True
            ' errores 
        Catch oe As Exception
            MsgBox(oe.Message, MsgBoxStyle.Critical)
        End Try
    End Sub
End Class
