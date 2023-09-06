Imports System.Drawing.Drawing2D
Imports System.Security.Cryptography

Public Class FrmPrincipal
    Dim cnx As New ADODB.Connection
    Dim linkcat As Double = 0
    Dim cadena As String = "Driver={IBM INFORMIX ODBC DRIVER};Host=192.9.200.1;Server=zeppelinprishm;Service=zeppelinpritcp;Protocol=onsoctcp;Database=data4gl;Uid=informix;Pwd=studioworks;dsn=;"
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

    'EVENTOS
    '---------------------------------------------------------------------------------------------
    Private Sub txtUbicacion_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtUbicacion.KeyPress
        Try
            If Not cnx.State = 1 Then cnx.Open(cadena)
            Dim estanteria As Integer
            Dim bloque As Integer
            Dim espacio As Integer
            Dim codigo As String = ""
            Dim rs As New ADODB.Recordset
            If txtUbicacion.Text.Length > 0 Then codigo = txtUbicacion.Text
            If e.KeyChar = Convert.ToChar(Keys.Enter) And Not codigo = "" Then
                Dim pos As String()
                pos = Split(codigo, "-")
                If pos.Length = 3 Then
                    estanteria = pos(0)
                    bloque = pos(1)
                    espacio = pos(2)

                    If Not cnx.State = 1 Then cnx.Open(cadena)
                    rs.Open("select * from ubicacion where estanteria = " & estanteria & " and bloque = " & bloque & " and espacio = " & espacio, cnx, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockReadOnly)
                    If Not rs.EOF Then
                        linkcat = rs.Fields("linkcat").Value
                        txtProducto.Focus()
                    Else
                        MsgBox("Ubicacion No Encontrada!", vbInformation, "UBICACION")
                    End If
                    If rs.State = 1 Then rs.Close()
                    If cnx.State = 1 Then cnx.Close()

                    If cnx.State = 1 Then cnx.Close()
                Else
                    MsgBox("La cadena de entrada no tiene el formato correcto.!" & vbCrLf & "Estanteria-Bloque-Espacio", MsgBoxStyle.Critical, "UBICACION")
                    Exit Sub
                End If
            End If


        Catch ex As Exception
            MsgBox("Error al buscar la ubicacion." & vbCrLf & ex.Message, MsgBoxStyle.Critical, "ERROR")
        End Try
    End Sub
    Private Sub txtProducto_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtProducto.KeyPress
        Dim rs As New ADODB.Recordset
        Dim rs1 As New ADODB.Recordset

        If e.KeyChar = Convert.ToChar(Keys.Enter) And txtProducto.Text.Length > 0 Then
            txtReferencia.Text = ""
            txtDescripcion.Text = ""
            txtTalla.Text = ""
            txtColor.Text = ""
            TxtExistencia.Clear()

            If cmbBodega.Text = "101" Then
                If Not cnx.State = 1 Then cnx.Open(cadena)
                cnx.CursorLocation = ADODB.CursorLocationEnum.adUseClient
                rs1.Open("select * from etiquetas where barra = " & txtProducto.Text, cnx, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockReadOnly)
                If Not rs1.EOF Then
                    rs.Open("select codart, talla, codcol, descrip, exis1a, linkcat from catal, catalg where catal.link = catalg.link and coddep = " & cmbBodega.Text & " and codart = '" & rs1.Fields("articulo").Value & "' and talla = '" & rs1.Fields("talla").Value & "' and codcol = " & rs1.Fields("Color").Value, cnx, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockReadOnly)
                    If Not rs.EOF Then
                        txtReferencia.Text = rs.Fields("codart").Value
                        txtDescripcion.Text = rs.Fields("descrip").Value
                        txtTalla.Text = "Talla: " & rs.Fields("talla").Value
                        txtColor.Text = "Color: " & rs.Fields("codcol").Value
                        TxtExistencia.Text = rs.Fields("exis1a").Value
                        txtProducto.Text = rs.Fields("linkcat").Value
                        txtPiezas.Focus()
                    End If
                    rs.Close()
                Else
                    rs.Open("select codart, talla, codcol, descrip, exis1a, linkcat from catal, catalg where catal.link = catalg.link and coddep = " & cmbBodega.Text & " and linkcat = " & txtProducto.Text, cnx, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockReadOnly)
                    If Not rs.EOF Then
                        txtReferencia.Text = rs.Fields("codart").Value
                        txtDescripcion.Text = rs.Fields("descrip").Value
                        txtTalla.Text = rs.Fields("talla").Value
                        txtColor.Text = rs.Fields("codcol").Value
                        TxtExistencia.Text = rs.Fields("exis1a").Value
                        txtProducto.Text = rs.Fields("linkcat").Value
                        txtPiezas.Focus()
                    Else
                        MsgBox("Producto no existe en bodega selecionada", MsgBoxStyle.Critical, "PRODUCTO")
                    End If
                    rs.Close()
                End If
                If cnx.State = 1 Then cnx.Close()
            End If
        End If
    End Sub

    Private Sub txtPiezazSinPaquete_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtPiezas.KeyPress
        Txtpiezaspaquete.Text = 0
        TxtPaquete.Focus()
    End Sub

    Private Sub btnFinalizar_Click(sender As Object, e As EventArgs) Handles btnFinalizar.Click
        Dim rs As New ADODB.Recordset
        Dim cant_temporal As Integer
        cant_temporal = 0

        rs.Open("select * from envmstt, envdett where envmstt.link = envdett.link and envdett.linkcat = " & txtProducto.Text, cnx, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockReadOnly)
        While Not rs.EOF
            cant_temporal = cant_temporal + rs.Fields("cantenv1").Value
            rs.MoveNext()
        End While
        rs.Close()

        rs.Open("select * from tbodtg, tbodtd where tbodtg.link = tbodtd.link and tbodtd.linkcat = " & txtProducto.Text, cnx, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockReadOnly)
        While Not rs.EOF
            cant_temporal = cant_temporal + rs.Fields("cantidad1a").Value
            rs.MoveNext()
        End While
        rs.Close()

        rs.Open("select * from oesbgt, oesbdt where empcod = 1 and oesbgt.link = oesbdt.link and oesbdt.linkcat = " & txtProducto.Text, cnx, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockReadOnly)
        While Not rs.EOF
            cant_temporal = cant_temporal + rs.Fields("cant1a").Value
            rs.MoveNext()
        End While
        rs.Close()

        rs.Open("select * from ibtg, ibtd where empcod = 1 and ibtg.link = ibtd.link and ibtd.linkcat = " & txtProducto.Text, cnx, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockReadOnly)
        While Not rs.EOF
            cant_temporal = cant_temporal + rs.Fields("cant1a").Value
            rs.MoveNext()
        End While
        rs.Close()

        Dim ri As New ADODB.Recordset

        ri.Open("select * from inventario where linkcat = " & txtProducto.Text, cnx, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockOptimistic)
        If ri.EOF Then
            ri.AddNew()
            ri.Fields("bodega").Value = cmbBodega.Text
            ri.Fields("codart").Value = txtReferencia.Text
            ri.Fields("talla").Value = txtTalla.Text
            ri.Fields("codcol").Value = txtColor.Text
            ri.Fields("linkcat").Value = txtProducto.Text
            ri.Fields("exis1a").Value = TxtExistencia.Text
            ri.Fields("exis2a").Value = 0
            ri.Fields("exis3a").Value = 0
            ri.Fields("temporal").Value = cant_temporal
            ri.Fields("fisic1").Value = Val(Txtpiezaspaquete.Text) + Val(txtPiezas.Text)
            ri.Fields("fisic2").Value = 0
            ri.Fields("fisic3").Value = 0
            ri.Fields("fecha").Value = Now
            ri.Fields("Estado").Value = "FINALIZADO"
            ri.Update()
        Else
            MsgBox("ESTE PRODUCTO YA CUENTA CON INVENTARIO DEL DIA DE HOY", MsgBoxStyle.Information, "FINALIZAR")
            MsgBox("NO SE GUARDO EL REGISTRO", MsgBoxStyle.Critical, "FINALIZAR")
        End If
        ri.Close()
        'TxtLinkcat.Enabled = True
        'TxtLinkcat.Text = ""
        'TxtLinkcat.SetFocus
        'Label2.Visible = False
        'Txtpiezas.Visible = False
        'Label4.Visible = False
        'txtExistencia.Visible = False
        'Lpaquetes.Visible = True
        'TxtPaquete.Visible = True
        'Label3.Visible = True
        'Txtpiezaspaquete.Visible = True
        'Lpaquetes.Clear
        'Txtpiezas.Text = ""
        'Txtpiezaspaquete.Text = ""
        'TxtPaquete.Text = ""
    End Sub

    Private Sub txtPaquetes_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TxtPaquete.KeyPress
        Dim rs As New ADODB.Recordset
        Dim cant1, cant2 As Integer

        If e.KeyChar = Convert.ToChar(Keys.Enter) And TxtPaquete.Text.Length > 0 Then
            rs.Open("select * from etiquetas where barra = " & TxtPaquete.Text, cnx, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockReadOnly)
            If Not rs.EOF Then
                If Trim(txtReferencia.Text) = Trim(rs.Fields("articulo").Value) Then
                    If Trim(txtTalla.Text) = Trim(rs.Fields("talla").Value) Then
                        If Trim(txtColor.Text) = Trim(rs.Fields("Color").Value) Then

                            Lpaquetes.Items.Add(TxtPaquete.Text & "--" & rs.Fields("cantidad").Value)

                            Txtpiezaspaquete.Text = Txtpiezaspaquete.Text + rs.Fields("cantidad").Value
                            cant1 = TxtExistencia.Text
                            cant2 = Val(txtPiezas.Text) + Val(Txtpiezaspaquete.Text)
                            If cant1 = cant2 Then
                                Lcolores.BackColor = Color.Green
                            Else
                                Lcolores.BackColor = Color.Red
                            End If
                            TxtPaquete.Text = ""
                        Else
                            MsgBox("Color no corresponde a este bloque", MsgBoxStyle.Critical)
                            TxtPaquete.Text = ""
                        End If
                    Else
                        MsgBox("Talla no corresponde a este bloque", MsgBoxStyle.Critical)
                        TxtPaquete.Text = ""
                    End If
                Else
                    MsgBox("Referencia no corresponde a este bloque", MsgBoxStyle.Critical)
                    TxtPaquete.Text = ""
                End If
            Else
                rs.Close()
                rs.Open("select * from catal, catalg where catal.link = catalg.link and coddep = " & cmbBodega.Text & " and catalg.linkcat = " & TxtPaquete.Text, cnx, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockReadOnly)
                If Not rs.EOF Then
                    If Trim(txtReferencia.Text) = Trim(rs.Fields("codart").Value) Then
                        If Trim(txtTalla.Text) = Trim(rs.Fields("talla").Value) Then
                            If Trim(txtColor.Text) = Trim(rs.Fields("codcol").Value) Then
                                Lpaquetes.Items.Add(TxtPaquete.Text & "--" & 1)

                                Txtpiezaspaquete.Text = Txtpiezaspaquete.Text + 1
                                cant1 = TxtExistencia.Text
                                cant2 = Val(txtPiezas.Text) + Val(1)
                                If cant1 = cant2 Then
                                    Lcolores.BackColor = Color.Green
                                Else
                                    Lcolores.BackColor = Color.Red
                                End If
                                TxtPaquete.Text = ""
                            Else
                                MsgBox("Color no corresponde a este bloque", MsgBoxStyle.Critical)
                                TxtPaquete.Text = ""
                            End If
                        Else
                            MsgBox("Talla no corresponde a este bloque", MsgBoxStyle.Critical)
                            TxtPaquete.Text = ""
                        End If
                    Else
                        MsgBox("Referencia no corresponde a este bloque", MsgBoxStyle.Critical)
                        TxtPaquete.Text = ""
                    End If
                Else
                    MsgBox("Paquete no existe o esta eliminado" & vbCrLf & " Informar a Depto de Computo", MsgBoxStyle.Critical)
                    TxtPaquete.Text = ""
                End If
            End If
        End If
    End Sub
End Class
