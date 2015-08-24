Imports System.IO
Imports System.Drawing.Printing

Public Class CabinaFotografica
    Dim Posicion As Integer = 0
    Dim Nombres As String
    Dim a As Integer, b As Integer, c As Integer
    Dim Lista As ArrayList = New ArrayList(c)
    Dim archivos() As String
    Dim Cantidad As String = ""
    Dim Contador As Integer = 0
    Dim ruta_marco As String = Path.GetDirectoryName(Application.ExecutablePath) & "\Imagenes\marco\"
    Dim marcos_redimensionados As String = Path.GetDirectoryName(Application.ExecutablePath) & "\Imagenes\marco_redimensionado\marco"
    Dim pagina As Integer = 0
    Dim cont As Integer = 1
    Dim cuentamarco As Integer, adelante As Integer, atras As Integer = 0
    Dim Fecha As String = Date.Now
    Dim appPath As String = Path.GetDirectoryName(Application.ExecutablePath)
    Dim carpeta As String = Path.GetDirectoryName(Application.ExecutablePath) & "\Imagenes\" & Format(CDate(Fecha), "yyyy-MM-dd") '& "\" & Format(CDate(Fecha), "HH-mm") & "\"
    Dim ruta As String = ""
    Dim listamarco As New ListBox
   

    Private Sub Form1_Disposed(sender As Object, e As EventArgs) Handles Me.Disposed
        WebCam1.Stop()
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles Me.Load

        File.WriteAllText(appPath, String.Join("|", New String() {"1", "2", "3"}))
        lblCantidadFotos.Text = "No. de Fotografias Tomadas : 0"
        WebCam1.Start()
        My.Computer.FileSystem.DeleteDirectory(marcos_redimensionados,FileIO.DeleteDirectoryOption.DeleteAllContents)
        System.IO.Directory.CreateDirectory(ruta_marco)
        System.IO.Directory.CreateDirectory(marcos_redimensionados)

        For Each Archivo As String In My.Computer.FileSystem.GetFiles(ruta_marco, FileIO.SearchOption.SearchAllSubDirectories, "*")

            Dim fName As String = IO.Path.GetExtension(Archivo)

            Dim name As String = IO.Path.GetFileName(Archivo)

            If fName = ".jpg" Or fName = ".png" Or fName = ".bmp" Or fName = ".jpeg" Then

                Redimensionar(System.Drawing.Image.FromFile(Archivo), marcos_redimensionados & "\" & name, 384, 576)
                listamarco.Items.Add(marcos_redimensionados & "\" & name)
                cuentamarco += 1

            End If

        Next

        cuentamarco = cuentamarco - 1

        If Lista Is Nothing Then
            MessageBox.Show("No se detecto ningun marco en la carpeta C:/Imagenes/Marco")
        End If

        PictureBox10.Image = System.Drawing.Image.FromFile(listamarco.Items(0))
    End Sub

    Private Sub Form1_SizeChanged(sender As Object, e As EventArgs) Handles Me.SizeChanged
        gbGeneral.Width = Me.Width - 45
        gbGeneral.Height = Me.Height - 50
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles btnCaptura.Click
        Dim Fecha As String = Date.Now
        Dim Nom As String
        'Dim Posicion As Integer = 0

        Dim cont As Integer = 0

        Dim size As New PaperSize("foto", 400, 600)
        Cantidad = ""
        Contador = 0
        Posicion = 0
        cont = 1
        pagina = 0
        ruta = carpeta & "\" & Format(CDate(Fecha), "HH-mm") & "\"
        System.IO.Directory.CreateDirectory(ruta)

        
        Cantidad = InputBox("Ingrese la Cantidad de Fotografias Requerida", "Cantidad de Fotos")
        Contador = IIf(IsNumeric(Cantidad), Cantidad, 0)


        'Redimensionar(PictureBox1.Image, ruta_marco & "1.png")
        If Contador > 0 Then
            For i As Integer = 0 To Contador - 1


                Dim msg As New Msg
                msg.restante = 5
                msg.TimerOn()
                msg.ShowDialog()
                pbFoto.Image = WebCam1.Imagen
                'PonerMarca(pbFoto, PictureBox1)
                Nom = ruta & Posicion + 1 & ".jpg"
                Redimensionar(pbFoto.Image, Nom, 115, 144)
                'Contador = Contador - 1
                Posicion = Posicion + 1
                lblCantidadFotos.Text = "No. de Fotografias Tomadas : " & Posicion
                Galeria(pbFoto)
            Next
        End If
        If Not Cantidad.Equals("") Then
            PrintDocument1.DefaultPageSettings.PaperSize = size
            PrintDocument1.Print()

        End If
        If Not PictureBox1.Image Is Nothing Then
            ' PictureBox1.Image = Nothing
        End If
        If Not PictureBox7.Image Is Nothing Then
            PictureBox7.Image = Nothing
        End If
        If Not PictureBox2.Image Is Nothing Then
            PictureBox2.Image = Nothing
        End If
        If Not PictureBox3.Image Is Nothing Then
            PictureBox3.Image = Nothing
        End If
        If Not PictureBox4.Image Is Nothing Then
            PictureBox4.Image = Nothing
        End If
        If Not PictureBox5.Image Is Nothing Then
            PictureBox5.Image = Nothing
        End If
        If Not PictureBox6.Image Is Nothing Then
            PictureBox6.Image = Nothing
        End If
        lblCantidadFotos.Text = "No. de Fotografias Tomadas : 0"
       

    End Sub

    Private Sub Galeria(ByVal img As PictureBox)
        If PictureBox5.Image Is Nothing Then
            PictureBox5.Image = img.Image
        ElseIf PictureBox6.Image Is Nothing Then
            PictureBox6.Image = img.Image
        ElseIf PictureBox4.Image Is Nothing Then
            PictureBox4.Image = img.Image
        ElseIf PictureBox3.Image Is Nothing Then
            PictureBox3.Image = img.Image
        ElseIf PictureBox7.Image Is Nothing Then
            PictureBox7.Image = img.Image
        ElseIf PictureBox2.Image Is Nothing Then
            PictureBox2.Image = img.Image
        End If
        If Not PictureBox2.Image Is Nothing Then
            PictureBox7.Image = Nothing
            PictureBox6.Image = Nothing
            PictureBox5.Image = Nothing
            PictureBox4.Image = Nothing
            PictureBox3.Image = Nothing
            PictureBox2.Image = Nothing
        End If

    End Sub

    Private Sub Marca(ByVal watermark_bm As Bitmap, _
        ByVal result_bm As Bitmap, ByVal x As Integer, ByVal y _
        As Integer)
        Const ALPHA As Byte = 80
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

    Private Sub Redimensionar(ByVal Imagen As Image, ByVal ruta As String, ByVal ancho As Integer, ByVal alto As Integer)
        Dim imagenRedimensionada As Bitmap

        'creamos una imagen con las dimensiones que se desean
        'en este caso la creamos de 100x100 pixels
        imagenRedimensionada = New Bitmap(ancho, alto)

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
        Dim PosX As Integer = 35
        Dim PosY As Integer = 40
        Dim foto2 As String = ""
        Dim size As New PaperSize("foto", 400, 600)
        
        Try
            'ev.DefaultPageSettings.PaperSize = size
            ev.Graphics.DrawImage(PictureBox10.Image, 0, 0)
            For Each Archivo As String In My.Computer.FileSystem.GetFiles(ruta, FileIO.SearchOption.SearchTopLevelOnly, "*.jpg")
                lista.Items.Add(Archivo)
                If pagina >= Posicion Then
                    ev.HasMorePages = False
                    cont = 1
                    Exit For
                End If
                foto2 = ruta & cont & ".jpg"
                Dim newImage As Image = Image.FromFile(foto2)
                ' Dim newImage2 As Image = Image.FromFile(ruta & cont & ".jpg")
                ' ev.Graphics.DrawImage(newImage2, PosY + 150, PosX)
                If PosX = 515 And PosY = 40 Then
                    'PosX = PosX + 282
                    PosX = 195
                    PosY = 180
                    ev.Graphics.DrawImage(newImage, PosY, PosX - 160)
                Else

                    PosX = PosX + 160
                    PosY = PosY
                    ev.Graphics.DrawImage(newImage, PosY, PosX - 160)

                End If
                If PosY = 180 And PosX = 515 Then
                    ev.HasMorePages = True
                    'PosX = PosX + 185
                    'PosY = 0
                    Exit For
                    'ev.Graphics.DrawImage(newImage, PosY, PosX)

                End If
                cont += 1
                pagina += 1
               
            Next
            ' Con esta propiedad estableces si existen una o más páginas
            'ev.HasMorePages = False

            ' errores 

        Catch oe As Exception
            MsgBox(oe.Message, MsgBoxStyle.Critical)
        End Try

    End Sub

    Private Sub gbGeneral_Enter(sender As Object, e As EventArgs) Handles gbGeneral.Enter

    End Sub

    Private Sub Button1_Click_1(sender As Object, e As EventArgs) Handles Button1.Click
        atras = atras - 1
        If atras < 0 Then
            atras = cuentamarco
        End If
        PictureBox10.Image = System.Drawing.Image.FromFile(listamarco.Items(atras))
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        adelante = adelante + 1
        If adelante > cuentamarco Then
            adelante = 0
        End If
        PictureBox10.Image = System.Drawing.Image.FromFile(listamarco.Items(adelante))
    End Sub
End Class




