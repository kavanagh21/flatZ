Imports ImageMagick

Public Class mainForm
    Dim image1 As MagickImage
    Dim image2 As MagickImage
    Dim compImage As MagickImage
    Dim hndl As Long

    Declare Function WindowFromPoint Lib "user32.dll" (ByVal point As Point) As IntPtr


    Private Sub PictureBox2_Click(sender As Object, e As EventArgs) Handles PictureBox2.Click

    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        hndl = WindowFromPoint(MousePosition)
        labelHandle.Text = hndl.ToString & " [X:" & MousePosition.X & "/Y:" & MousePosition.Y & "]"
    End Sub

    Private Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox1.CheckedChanged
        If CheckBox1.Checked = True Then Timer1.Enabled = True Else Timer1.Enabled = False

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

        If CheckBox4.Checked = True Then Threading.Thread.Sleep(5000)


        Dim sc As New ScreenShot.ScreenCapture
        image1.Read(sc.CaptureWindow(hndl))



        If checkImpDim.Checked = True Then
            Dim qDim As New MagickGeometry
            qDim.X = CInt(impTL.Text)
            qDim.Y = CInt(impTR.Text)
            qDim.Height = CInt(impBR.Text)
            qDim.Width = CInt(impBL.Text)
            image1.Crop(qDim)
        End If

        If CheckBox3.Checked = False Then
            PictureBox1.Image = image1.ToBitmap
            PictureBox1.SizeMode = PictureBoxSizeMode.Zoom
        Else
            Dim tmpImage As New MagickImage(image1)
            tmpImage.Level(CUShort(threshMin.Value), CUShort(threshMax.Value))
            PictureBox1.Image = tmpImage.ToBitmap
            tmpImage.Dispose()
        End If


        Label1.Text = "Image 1: " & image1.Width & " x " & image1.Height

    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles Me.Load

        image1 = New MagickImage("blank.bmp")
        PictureBox1.Image = image1.ToBitmap

        image2 = New MagickImage("blank.bmp")
        PictureBox2.Image = image2.ToBitmap

        compImage = New MagickImage("blank.bmp")
        PictureBox3.Image = compImage.ToBitmap

        ComboBox1.SelectedIndex = 0


    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        If CheckBox4.Checked = True Then Threading.Thread.Sleep(5000)


        Dim sc As New ScreenShot.ScreenCapture
        image2.Read(sc.CaptureWindow(hndl))

        If checkImpDim.Checked = True Then
            Dim qDim As New MagickGeometry
            qDim.X = CInt(impTL.Text)
            qDim.Y = CInt(impTR.Text)
            qDim.Height = CInt(impBR.Text)
            qDim.Width = CInt(impBL.Text)
            image2.Crop(qDim)
        End If

        If CheckBox3.Checked = False Then
            PictureBox2.Image = image2.ToBitmap
            PictureBox2.SizeMode = PictureBoxSizeMode.Zoom
        Else
            Dim tmpImage As New MagickImage(image2)
            tmpImage.Level(CUShort(threshMin.Value), CUShort(threshMax.Value))
            PictureBox2.Image = tmpImage.ToBitmap
            tmpImage.Dispose()
        End If

        Label2.Text = "Image 2: " & image2.Width & " x " & image2.Height

    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Dim chanNum As Integer
        Dim pxbounds As New MagickGeometry
        wait.Visible = True
        Me.Refresh()



        If ComboBox1.SelectedItem.ToString = "Red" Then chanNum = 0
        If ComboBox1.SelectedItem.ToString = "Green" Then chanNum = 1
        If ComboBox1.SelectedItem.ToString = "Blue" Then chanNum = 2

        'set the compimage to equal image1

        Dim maxX As Integer = image1.Width
        If image2.Width < maxX Then maxX = image2.Width
        If compImage.Width < maxX Then maxX = compImage.Width

        Dim maxY As Integer = image1.Height
        If image2.Height < maxY Then maxY = image2.Height
        If compImage.Height < maxY Then maxY = compImage.Height

        Dim bigRec As New ImageMagick.DrawableRectangle(0, 0, maxX, maxY)
        Dim bigCol As New ImageMagick.DrawableFillColor(Color.Black)

        compImage.Draw(bigCol, bigRec)
        Dim cropGeom As New ImageMagick.MagickGeometry(0, 0, maxX, maxY)
        compImage.Crop(cropGeom)

        PictureBox3.Image = compImage.ToBitmap
        'Me.Refresh()

        Dim stepSize As Integer
        If CheckBox2.Checked = True Then stepSize = 2 Else stepSize = 1

        'compImage.Composite(image1)

        Dim image1px As IPixelCollection = image1.GetPixels()
        Dim image2px As IPixelCollection = image2.GetPixels()
        Dim comppx As IPixelCollection = compImage.GetPixels()


        Dim img1Higher As Integer
        Dim img2higher As Integer

        For x = 0 To maxX - 1 Step stepSize
            For y = 0 To maxY - 1 Step stepSize
                Dim newx As Integer = x + xoffset2.Value
                Dim newy As Integer = y + yoffset2.Value

                If newx > 0 And newy > 0 And newx < maxX - 1 And newy < maxY - 1 Then
                    Dim px1val As UShort = image1px(x, y).GetChannel(chanNum)
                    Dim px2val As UShort = image2px(newx, newy).GetChannel(chanNum)
                    If px1val < px2val Then
                        comppx(x, y).SetChannel(0, image2px(newx, newy).GetChannel(0))
                        comppx(x, y).SetChannel(1, image2px(newx, newy).GetChannel(1))
                        comppx(x, y).SetChannel(2, image2px(newx, newy).GetChannel(2))
                        img2higher += 1
                    Else
                        comppx(x, y).SetChannel(0, image1px(x, y).GetChannel(0))
                        comppx(x, y).SetChannel(1, image1px(x, y).GetChannel(1))
                        comppx(x, y).SetChannel(2, image1px(x, y).GetChannel(2))
                        img1Higher += 1
                    End If
                Else
                    'it's out of bounds from the converted space
                    comppx(x, y).SetChannel(0, image1px(x, y).GetChannel(0))
                    comppx(x, y).SetChannel(1, image1px(x, y).GetChannel(1))
                    comppx(x, y).SetChannel(2, image1px(x, y).GetChannel(2))
                End If

                'Debug.Print("IMG1: " & px1val & "; IMG2: " & px2val & "; X: " & x & "; Y: " & y & "; NEWX: " & newx & "; NEWY: " & newy)


            Next
        Next

        'MessageBox.Show("IMG1: " & img1Higher & "; IMG2: " & img2higher)

        If CheckBox3.Checked = False Then
            PictureBox1.Image = image1.ToBitmap
            PictureBox1.SizeMode = PictureBoxSizeMode.Zoom
        Else
            Dim tmpImage1 As New MagickImage(image1)
            tmpImage1.Level(CUShort(threshMin.Value), CUShort(threshMax.Value))
            PictureBox1.Image = tmpImage1.ToBitmap
            tmpImage1.Dispose()
        End If

        If CheckBox3.Checked = False Then
            PictureBox2.Image = image2.ToBitmap
            PictureBox2.SizeMode = PictureBoxSizeMode.Zoom
        Else
            Dim tmpImage2 As New MagickImage(image2)
            tmpImage2.Level(CUShort(threshMin.Value), CUShort(threshMax.Value))
            PictureBox2.Image = tmpImage2.ToBitmap
            tmpImage2.Dispose()
        End If


        'If CheckBox3.Checked = False Then
        ' PictureBox3.Image = compImage.ToBitmap
        ' PictureBox3.SizeMode = PictureBoxSizeMode.Zoom
        ' Else
        Dim tmpImage3 As New MagickImage(compImage)
        tmpImage3.Level(CUShort(threshMin.Value), CUShort(threshMax.Value))
            PictureBox3.Image = tmpImage3.ToBitmap
            tmpImage3.Dispose()
        'End If

        wait.Visible = False


    End Sub

    Private Sub Label1_Click(sender As Object, e As EventArgs) Handles Label1.Click

    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        wait.Visible = True
        Me.Refresh()
        Dim image1px As IPixelCollection = image1.GetPixels()
        Dim comppx As IPixelCollection = compImage.GetPixels()


        For x = 0 To compImage.Width - 1
            For y = 0 To compImage.Height - 1
                image1px(x, y).SetChannel(0, comppx(x, y).GetChannel(0))
                image1px(x, y).SetChannel(1, comppx(x, y).GetChannel(1))
                image1px(x, y).SetChannel(2, comppx(x, y).GetChannel(2))
            Next
        Next
        wait.Visible = False


        If CheckBox3.Checked = False Then
            PictureBox1.Image = image1.ToBitmap
            PictureBox1.SizeMode = PictureBoxSizeMode.Zoom
        Else
            Dim tmpImage1 As New MagickImage(image1)
            tmpImage1.Level(CUShort(threshMin.Value), CUShort(threshMax.Value))
            PictureBox1.Image = tmpImage1.ToBitmap
            tmpImage1.Dispose()
        End If
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click

        Clipboard.SetImage(compImage.ToBitmap)

    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        PictureBox1.Image = image1.ToBitmap
        PictureBox2.Image = image2.ToBitmap
        PictureBox3.Image = compImage.ToBitmap

    End Sub

    Private Sub lowthreshslider_Scroll(sender As Object, e As EventArgs) Handles lowthreshslider.Scroll
        threshMin.Value = sender.value
        Dim tmpImage As New MagickImage(compImage)
        tmpImage.Level(CUShort(threshMin.Value), CUShort(threshMax.Value))
        PictureBox3.Image = tmpImage.ToBitmap
        tmpImage.Dispose()
        If CheckBox3.Checked = True Then
            Dim tmpImage2 As New MagickImage(image1)
            tmpImage2.Level(CUShort(threshMin.Value), CUShort(threshMax.Value))
            PictureBox1.Image = tmpImage2.ToBitmap
            tmpImage.Dispose()

            Dim tmpImage3 As New MagickImage(image2)
            tmpImage3.Level(CUShort(threshMin.Value), CUShort(threshMax.Value))
            PictureBox2.Image = tmpImage3.ToBitmap
            tmpImage3.Dispose()
        End If
    End Sub

    Private Sub upperthreshslider_Scroll(sender As Object, e As EventArgs) Handles upperthreshslider.Scroll
        threshMax.Value = sender.value
        Dim tmpImage As New MagickImage(compImage)
        tmpImage.Level(CUShort(threshMin.Value), CUShort(threshMax.Value))
        PictureBox3.Image = tmpImage.ToBitmap
        tmpImage.Dispose()
        If CheckBox3.Checked = True Then
            Dim tmpImage2 As New MagickImage(image1)
            tmpImage2.Level(CUShort(threshMin.Value), CUShort(threshMax.Value))
            PictureBox1.Image = tmpImage2.ToBitmap
            tmpImage.Dispose()

            Dim tmpImage3 As New MagickImage(image2)
            tmpImage3.Level(CUShort(threshMin.Value), CUShort(threshMax.Value))
            PictureBox2.Image = tmpImage3.ToBitmap
            tmpImage3.Dispose()
        End If
    End Sub

    Private Sub Button7_Click(sender As Object, e As EventArgs) Handles Button7.Click
        Dim tmpImage As New MagickImage(compImage)
        tmpImage.Level(CUShort(threshMin.Value), CUShort(threshMax.Value))
        PictureBox3.Image = tmpImage.ToBitmap
        tmpImage.Dispose()
        If CheckBox3.Checked = True Then
            Dim tmpImage2 As New MagickImage(image1)
            tmpImage2.Level(CUShort(threshMin.Value), CUShort(threshMax.Value))
            PictureBox1.Image = tmpImage2.ToBitmap
            tmpImage.Dispose()

            Dim tmpImage3 As New MagickImage(image2)
            tmpImage3.Level(CUShort(threshMin.Value), CUShort(threshMax.Value))
            PictureBox2.Image = tmpImage3.ToBitmap
            tmpImage3.Dispose()
        End If

    End Sub

    Private Sub Button8_Click(sender As Object, e As EventArgs) Handles Button8.Click
        If MessageBox.Show("Are you sure you want to permanently apply this thresholding?", "Confirm", MessageBoxButtons.YesNo) = DialogResult.No Then Exit Sub
        compImage.Level(CUShort(threshMin.Value), CUShort(threshMax.Value))
        PictureBox3.Image = compImage.ToBitmap

    End Sub

    Private Sub Button9_Click(sender As Object, e As EventArgs) Handles Button9.Click
        wait.Visible = True
        Me.Refresh()
        Dim chanNum As Integer
        Dim sumInt As Double
        Dim totalArea As Double
        Dim pxcount As Double


        Dim comppx As IPixelCollection = compImage.GetPixels()
        If ComboBox1.SelectedItem.ToString = "Red" Then chanNum = 0
        If ComboBox1.SelectedItem.ToString = "Green" Then chanNum = 1
        If ComboBox1.SelectedItem.ToString = "Blue" Then chanNum = 2

        For x = 0 To compImage.Width - 1
            For y = 0 To compImage.Height - 1

                sumInt += CInt(comppx(x, y).GetChannel(chanNum))
                pxcount += 1
                If CInt(comppx(x, y).GetChannel(chanNum)) > 0 Then
                    totalArea += 1
                End If

            Next
        Next
        wait.Visible = False

        ListBox1.Items.Clear()
        ListBox1.Items.Add("Pixel count: " & pxcount)
        ListBox1.Items.Add("Sum intensity: " & sumInt)
        ListBox1.Items.Add("Total area: " & totalArea)



        PictureBox1.Image = image1.ToBitmap
        PictureBox1.SizeMode = PictureBoxSizeMode.Zoom

    End Sub

    Private Sub Button10_Click(sender As Object, e As EventArgs) Handles Button10.Click
        SaveFileDialog1.Filter = "Bitmap file (*.bmp)|*.bmp|PNG file (*.png)|*.png|TIFF file (*.tif)|*.tif"
        SaveFileDialog1.FilterIndex = 1
        SaveFileDialog1.CheckPathExists = True
        SaveFileDialog1.AddExtension = True

        SaveFileDialog1.ShowDialog()

        If SaveFileDialog1.FileName = "" Then Exit Sub
        wait.Visible = True
        compImage.Write(SaveFileDialog1.FileName)
        wait.Visible = False

    End Sub

    Private Sub CheckBox3_CheckStateChanged(sender As Object, e As EventArgs) Handles CheckBox3.CheckStateChanged

        If CheckBox3.Checked = False Then
            PictureBox1.Image = image1.ToBitmap
            PictureBox1.SizeMode = PictureBoxSizeMode.Zoom
        Else
            Dim tmpImage1 As New MagickImage(image1)
            tmpImage1.Level(CUShort(threshMin.Value), CUShort(threshMax.Value))
            PictureBox1.Image = tmpImage1.ToBitmap
            tmpImage1.Dispose()
        End If

        If CheckBox3.Checked = False Then
            PictureBox2.Image = image2.ToBitmap
            PictureBox2.SizeMode = PictureBoxSizeMode.Zoom
        Else
            Dim tmpImage2 As New MagickImage(image2)
            tmpImage2.Level(CUShort(threshMin.Value), CUShort(threshMax.Value))
            PictureBox2.Image = tmpImage2.ToBitmap
            tmpImage2.Dispose()
        End If


        If CheckBox3.Checked = False Then
            PictureBox3.Image = compImage.ToBitmap
            PictureBox3.SizeMode = PictureBoxSizeMode.Zoom
        Else
            Dim tmpImage3 As New MagickImage(compImage)
            tmpImage3.Level(CUShort(threshMin.Value), CUShort(threshMax.Value))
            PictureBox3.Image = tmpImage3.ToBitmap
            tmpImage3.Dispose()
        End If
    End Sub

    Private Sub CheckBox3_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox3.CheckedChanged

    End Sub

    Private Sub Button11_Click(sender As Object, e As EventArgs) Handles Button11.Click
        wait.Visible = True
        Me.Refresh()
        Dim chanNum As Integer
        Dim sumInt As Double
        Dim totalArea As Double
        Dim pxcount As Double


        Dim comppx As IPixelCollection = compImage.GetPixels()
        If ComboBox1.SelectedItem.ToString = "Red" Then chanNum = 0
        If ComboBox1.SelectedItem.ToString = "Green" Then chanNum = 1
        If ComboBox1.SelectedItem.ToString = "Blue" Then chanNum = 2

        For x = 0 To compImage.Width - 1
            For y = 0 To compImage.Height - 1

                sumInt += CInt(comppx(x, y).GetChannel(chanNum))
                pxcount += 1
                If CInt(comppx(x, y).GetChannel(chanNum)) > 0 Then
                    totalArea += 1
                End If

            Next
        Next

        Clipboard.SetText(pxcount & vbTab & sumInt & vbTab & totalArea)

        wait.Visible = False


    End Sub

    Private Sub Button12_Click(sender As Object, e As EventArgs) Handles Button12.Click
        If MessageBox.Show("Are you sure you want to permanently apply this despeckle/denoise?", "Confirm", MessageBoxButtons.YesNo) = DialogResult.No Then Exit Sub
        compImage.Despeckle()
        Button7_Click(sender, e)



    End Sub

    Private Sub Button13_Click(sender As Object, e As EventArgs) Handles Button13.Click
        If MessageBox.Show("Are you sure you want to permanently apply this despeckle/denoise?", "Confirm", MessageBoxButtons.YesNo) = DialogResult.No Then Exit Sub

        compImage.ReduceNoise(3)
        Button7_Click(sender, e)

    End Sub

    Private Sub Label8_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub CheckBox4_CheckedChanged(sender As Object, e As EventArgs) Handles checkImpDim.CheckedChanged

    End Sub

    Private Sub Button14_Click(sender As Object, e As EventArgs) Handles Button14.Click
        xoffset1.Value = 0
        yoffset1.Value = 0

    End Sub

    Private Sub Button15_Click(sender As Object, e As EventArgs) Handles Button15.Click
        xoffset2.Value = 0
        yoffset2.Value = 0

    End Sub

    Private Sub yoffset2_Scroll(sender As Object, e As ScrollEventArgs) Handles yoffset2.Scroll

    End Sub

    Private Sub xoffset2_Scroll(sender As Object, e As ScrollEventArgs) Handles xoffset2.Scroll

    End Sub

    Private Sub CheckBox5_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox5.CheckedChanged

    End Sub

    Private Sub Timer2_Tick(sender As Object, e As EventArgs) Handles Timer2.Tick
        If CheckBox5.Checked = True Then
            Button2_Click(Nothing, Nothing)
        End If
        If CheckBox6.Checked = True Then
            Button1_Click(Nothing, Nothing)
        End If
    End Sub

    Private Sub Button16_Click(sender As Object, e As EventArgs) Handles Button16.Click
        Timer2.Interval = Val(TextBox1.Text)
    End Sub

    Private Sub Button17_Click(sender As Object, e As EventArgs) Handles Button17.Click
        OpenFileDialog1.ShowDialog()
        If OpenFileDialog1.FileName = "" Then Exit Sub

        compImage.Read(OpenFileDialog1.FileName)
        Dim tmpImage3 As New MagickImage(compImage)
        tmpImage3.Level(CUShort(threshMin.Value), CUShort(threshMax.Value))
        PictureBox3.Image = tmpImage3.ToBitmap
        tmpImage3.Dispose()
        'End If

    End Sub
End Class
