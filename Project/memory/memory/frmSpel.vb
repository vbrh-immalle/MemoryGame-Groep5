﻿Public Class frmSpel
    Dim Juiste As Integer
    Dim AantalNodigeJuiste As Integer
    Dim Afbeeldingen As New List(Of Image)
    Dim Kaarten As New List(Of KeyValuePair(Of Integer, Image))
    Dim AfbTeZien As New List(Of Image)
    Dim GekliktePicBox As New List(Of PictureBox)

    Enum Graad
        Gemakkelijk
        Normaal
        Moeilijk
    End Enum

    Private Sub Form1_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load

        Select Case frmMenu.MOEILIJKHEIDSGRAAD
            Case Graad.Gemakkelijk
                GraadMakkelijk()
            Case Graad.Normaal
                GraadNormaal()
            Case Graad.Moeilijk
                GraadMoeilijk()
        End Select
    End Sub

    Sub GraadMakkelijk()
        ' De nodige juiste afbeeldingen instellne
        AantalNodigeJuiste = 8
        ' Stel formulier in op juiste verhoudingen
        StelFormulierIn(755, 900, 20, 650)
        ' Veld van 8x8
        MaakVeld(8)
        ' Voeg de afbeeldingen toe
        VoegAfbeeldingenToe(8)
        ' Start het spel
        Play()

    End Sub

    Sub GraadNormaal()

    End Sub

    Sub GraadMoeilijk()

    End Sub

    Sub StelFormulierIn(Formheight As Integer, Formwidth As Integer, knopMenuX As Integer, knopMenuY As Integer)
        Me.AutoSize = False
        Me.Height = Formheight
        Me.Width = Formwidth
        btnMenu.Location = New Point(knopMenuX, knopMenuY)
    End Sub

    Public Sub MaakVeld(ByVal aantalRij As Integer)
        Dim veld As Integer = aantalRij / 2
        Dim nr As Byte = 0
        For i = 0 To veld - 1
            For j = 0 To veld - 1
                Dim picBox As New PictureBox
                picBox.Size = New Size(100, 100)
                picBox.Name = nr
                picBox.AutoSize = False
                picBox.Location = New Point(170 * j, 170 * i)
                picBox.SizeMode = PictureBoxSizeMode.AutoSize
                picBox.Visible = False

                picBox.BorderStyle = BorderStyle.Fixed3D
                AddHandler picBox.MouseClick, AddressOf PictureBoxOnMouseClick
                Me.Controls.Add(picBox)
                nr += 1
            Next
        Next

        lblTweeAfbeeldingen.Location = New Point(200, 660)
        lblTweeAfbeeldingen.AutoSize = False
        lblTweeAfbeeldingen.Width = 200
    End Sub

    Sub VoegAfbeeldingenToe(aantalAfbeeldingen As Byte)
        Dim randomClass As New Random()


        For i = 1 To aantalAfbeeldingen
            Dim randomAfb As Integer = randomClass.Next(1, 46) ' Het aantal afbeeldingen dat in de map staan
            Dim afb As Image
            If randomAfb > 9 Then
                afb = Image.FromFile(frmMenu.PATH & "\Afb" & randomAfb & ".png")
                Afbeeldingen.Add(afb)
                Afbeeldingen.Add(afb)
            Else
                afb = Image.FromFile(frmMenu.PATH & "\Afb0" & randomAfb & ".png")
                Afbeeldingen.Add(afb)
                Afbeeldingen.Add(afb)
            End If

        Next

        For i = 0 To Afbeeldingen.Count - 1 Step 2
            Afbeeldingen(i).Tag = "Afb" & i
            Afbeeldingen(i + 1).Tag = "Afb" & i
        Next
    End Sub

    Function AfbeeldingInstellen() As Image
        Dim random As New Random()
        Dim randomGetal As Integer = random.Next(Afbeeldingen.Count - 1)
        Dim afb As Image
        afb = Afbeeldingen(randomGetal)

        Afbeeldingen.RemoveAt(randomGetal)
        Return afb
    End Function

    Private Sub PictureBoxOnMouseClick(ByVal sender As PictureBox, ByVal e As System.EventArgs)
        If AfbTeZien.Count = 1 Then
            sender.Image = Kaarten(sender.Name).Value
            GekliktePicBox.Add(sender)
            AfbTeZien.Add(sender.Image)
            If AfbeeldingenVergelijken(AfbTeZien(0), AfbTeZien(1)) Then
                ' De afbeeldingen zijn gelijk
                TijdAfbTonenGelijk.Start()
            Else
                ' De afbeeldingen zijn niet gelijk
                TijdAfbTonenNietGelijk.Start()
            End If
            AfbTeZien.Clear()
        Else
            sender.Image = Kaarten(sender.Name).Value
            GekliktePicBox.Add(sender)
            AfbTeZien.Add(sender.Image)
        End If



    End Sub

    Function AfbeeldingenVergelijken(ByVal image1 As Image, ByVal image2 As Image) As Boolean
        If image1.Tag = image2.Tag Then
            lblTweeAfbeeldingen.Text = "De afbeeldingen zijn gelijk"
            Return True
        Else
            lblTweeAfbeeldingen.Text = "De afbeeldingen zijn niet gelijk"
            Return False
        End If
    End Function

    Sub Play()
        Dim aantal As Byte = 0
        For Each cntrl As Control In Me.Controls
            If (TypeOf cntrl Is PictureBox) Then
                Dim picBox As PictureBox
                picBox = cntrl
                picBox.BackColor = Color.LightGray
                picBox.Image = AfbeeldingInstellen()
                Kaarten.Add(New KeyValuePair(Of Integer, Image)(aantal, picBox.Image))
                picBox.Image = My.Resources.achterkant
                picBox.Visible = True
                aantal += 1
            End If
        Next
    End Sub

    Private Sub TijdAfbTonen_Tick(sender As System.Object, e As System.EventArgs) Handles TijdAfbTonenNietGelijk.Tick
        TijdAfbTonenNietGelijk.Stop()
        GekliktePicBox(0).Image = My.Resources.achterkant
        GekliktePicBox(1).Image = My.Resources.achterkant
        GekliktePicBox.Clear()
    End Sub

    Private Sub TijdAfbTonenGelijk_Tick(sender As System.Object, e As System.EventArgs) Handles TijdAfbTonenGelijk.Tick
        TijdAfbTonenGelijk.Stop()
        GekliktePicBox(0).BackColor = Color.Aquamarine
        GekliktePicBox(1).BackColor = Color.Aquamarine
        Juiste += 1
        If Juiste = AantalNodigeJuiste Then
            MessageBox.Show("U bent gewonnen.", "Proficiat!", MessageBoxButtons.OK)


            If MessageBox.Show("Wilt u terug naar het hoofdmenu", "", MessageBoxButtons.YesNo) = Windows.Forms.DialogResult.Yes Then
                frmMenu.Show()
                Me.Close()
            Else
                Me.Close()
                frmMenu.Close()
            End If
        End If
        GekliktePicBox.Clear()

    End Sub

    Private Sub btnMenu_Click(sender As System.Object, e As System.EventArgs) Handles btnMenu.Click
        Me.Close()
        frmMenu.Show()
    End Sub
End Class
