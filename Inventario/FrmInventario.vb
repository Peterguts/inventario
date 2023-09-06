Imports System.ComponentModel.Design.Serialization
Imports System.Data.Odbc
Imports System.IO
Imports System.Runtime.InteropServices
Imports System.Threading
Imports System.Windows.Forms
Imports ADODB
Imports Microsoft.Win32.SafeHandles

Public Class FrmInventario
    Public cant As Integer = 0
    Public value As Boolean = False
    Private Sub FrmInventario_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If Not conectarLocal() Then
            MsgBox("No se pudo establer conexion con la base de datos.!", MsgBoxStyle.Critical, "ERROR DE CONEXION")
            Exit Sub
        End If

        llenarBodega(cmbBodega)
        cmbBodega.Text = ""
        cmbBodega.Focus()
    End Sub
    Private Sub txtLinkcat_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtLinkcat.KeyPress
        If Not cmbBodega.Text.Length > 0 Then
            MsgBox("Bodega no seleccionada.!", MsgBoxStyle.Critical, "CAMPO OBLIGATORIO")
            Exit Sub
        End If
        If LEstado.Text = "FINALIZADO" Then Exit Sub

        If e.KeyChar = Convert.ToChar(Keys.Enter) And txtLinkcat.Text.Length > 0 Then
            Try
                If Lbodega.Text <> "---" Then
                    Dim rs As New ADODB.Recordset
                    Dim rs1 As New ADODB.Recordset
                    Dim bloque As String = ""
                    'BLOQUE NUEVO POR ENDE ENTRA A CONSULTAR EL BLOQUE MAXIMO PARA AGREGARLO.
                    bloque = buscarBloque()
                    If Not txtBloque.Text.Length > 0 Then
                        rs1.Open("Select * from etiquetas where bodega = '" & cmbBodega.Text & "' and bloque = " & bloque & " and maquina = '" & SystemInformation.ComputerName & "'", conn, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockOptimistic)
                        txtBloque.Text = bloque
                    Else
                        rs1.Open("Select count(*) from etiquetas where bodega = '" & cmbBodega.Text & "' and bloque = " & txtBloque.Text & " and maquina = '" & SystemInformation.ComputerName & "'", conn, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockOptimistic)
                        If Not rs1.Fields(0).Value > 0 Then
                            MsgBox("Bloque no encontrado.!" & vbCrLf & "Para crear un nuevo bloque presione el boton 'NUEVO BLOQUE'", MsgBoxStyle.Critical, "BLOQUE NO EXISTE")
                            Exit Sub
                        End If
                    End If

                    conn.BeginTrans()

                    If rs1.EOF Then
                        rs.Open("Select ex.bodega, ar.referencia, ar.talla, ar.color, ar.barra, ar.descripcion, ex.existencia
                        from articulos as ar 
                        inner join existencias as ex on ex.bodega = '" & cmbBodega.Text & "' and ex.plu = ar.plu
                        where ar.barra = '" & txtLinkcat.Text & "'", conn, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockReadOnly)
                        If Not rs.EOF Then
                            If bloque = 1 Then
                                If MsgBox("¿Desea iniciar nuevo bloque en la bodega " & cmbBodega.Text & "?", MsgBoxStyle.Question Or MsgBoxStyle.YesNo, "NUEVO BLOQUE") = vbNo Then
                                    GoTo Fin
                                End If
                            End If
                            rs1.AddNew()
                            rs1.Fields("bloque").Value = bloque
                            rs1.Fields("bodega").Value = cmbBodega.Text
                            rs1.Fields("codart").Value = rs.Fields("referencia").Value.ToString.Trim
                            rs1.Fields("talla").Value = rs.Fields("talla").Value.ToString.Trim
                            rs1.Fields("color").Value = rs.Fields("color").Value.ToString.Trim
                            rs1.Fields("descrip").Value = rs.Fields("descripcion").Value.ToString.Trim
                            rs1.Fields("linkcat").Value = txtLinkcat.Text.ToString.Trim
                            rs1.Fields("exist1a").Value = rs.Fields("existencia").Value
                            rs1.Fields("exist2a").Value = 0
                            rs1.Fields("exist3a").Value = 0
                            rs1.Fields("fisic1").Value = 1
                            rs1.Fields("fisic2").Value = 0
                            rs1.Fields("fisic3").Value = 0
                            rs1.Fields("temporal").Value = 0
                            rs1.Fields("fecha").Value = Format(Now(), "yyyy-MM-dd hh:mm:ss")
                            rs1.Fields("estado").Value = "ACTIVO"
                            rs1.Fields("maquina").Value = SystemInformation.ComputerName
                            rs1.Update()
                            txtBloque.Text = bloque
                            conn.Execute("INSERT INTO bitacora VALUES (" & maxBitacora() & ", '" & cmbBodega.Text & "', '" & rs.Fields("referencia").Value.ToString.Trim & "', '" & rs.Fields("talla").Value.ToString.Trim & "', '" & rs.Fields("color").Value.ToString.Trim & "', '" & txtLinkcat.Text & "', 'NUEVO', " & rs.Fields("existencia").Value & ", 1, 0, '" & Format(Now(), "yyyy-MM-dd hh:mm:ss") & "', '" & SystemInformation.ComputerName & "', '" & bloque & "');")

                        Else
                            MsgBox("Producto no existe en bodega selecionada", MsgBoxStyle.Critical, "PRODUCTO")
                            If rs.State = 1 Then rs.Close()
                            If rs1.State = 1 Then rs1.Close()
                            conn.RollbackTrans()
                            Exit Sub
                        End If
                        If rs.State = 1 Then rs.Close()
                    Else
                        If rs1.State = 1 Then rs1.Close()
                        'SI EXISTE EL BLOQUE CONSULTAMOS QUE LA BARRA EXISTA, SI EXISTE SE LE SUMA AL FISICO Y SI NO EXISTE SE AGREGA UNA NUEVA LINEA CON ESE BLOQUE.
                        rs1.Open("Select * from etiquetas where bodega = '" & cmbBodega.Text & "' and bloque = " & txtBloque.Text & " and maquina = '" & SystemInformation.ComputerName & "' and linkcat = '" & txtLinkcat.Text & "'", conn, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockOptimistic)
                        If Not rs1.EOF Then
                            rs.Open("Select ex.bodega, ar.referencia, ar.talla, ar.color, ar.barra, ar.descripcion, ex.existencia
                            from articulos as ar 
                            inner join existencias as ex on ex.bodega = '" & cmbBodega.Text & "' and ex.plu = ar.plu
                            where ar.barra = '" & txtLinkcat.Text & "'", conn, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockReadOnly)
                            If Not rs.EOF Then
                                rs1.Fields("fisic1").Value += 1
                                rs1.Fields("fecha").Value = Format(Now(), "yyyy-MM-dd hh:mm:ss")
                                rs1.Fields("maquina").Value = SystemInformation.ComputerName
                                rs1.Update()
                                conn.Execute("INSERT INTO bitacora VALUES (" & maxBitacora() & ", '" & cmbBodega.Text & "', '" & rs.Fields("referencia").Value.ToString.Trim & "', '" & rs.Fields("talla").Value & "', '" & rs.Fields("color").Value.ToString.Trim & "', '" & txtLinkcat.Text & "', 'ACTUALIZAR', " & rs.Fields("existencia").Value & ", 1, 0, '" & Format(Now(), "yyyy-MM-dd hh:mm:ss") & "', '" & SystemInformation.ComputerName & "', '" & txtBloque.Text & "')")
                            Else
                                MsgBox("Producto no existe en bodega selecionada", MsgBoxStyle.Critical, "PRODUCTO")
                                If rs.State = 1 Then rs.Close()
                                If rs1.State = 1 Then rs1.Close()
                                conn.RollbackTrans()
                                Exit Sub
                            End If
                        Else
                            rs.Open("Select ex.bodega, ar.referencia, ar.talla, ar.color, ar.barra, ar.descripcion, ex.existencia
                            from articulos as ar 
                            inner join existencias as ex on ex.bodega = '" & cmbBodega.Text & "' and ex.plu = ar.plu
                            where ar.barra = '" & txtLinkcat.Text & "'", conn, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockReadOnly)
                            If Not rs.EOF Then
                                rs1.AddNew()
                                rs1.Fields("bloque").Value = txtBloque.Text
                                rs1.Fields("bodega").Value = cmbBodega.Text
                                rs1.Fields("codart").Value = rs.Fields("referencia").Value.ToString.Trim
                                rs1.Fields("talla").Value = rs.Fields("talla").Value.ToString.Trim
                                rs1.Fields("color").Value = rs.Fields("color").Value.ToString.Trim
                                rs1.Fields("descrip").Value = rs.Fields("descripcion").Value.ToString.Trim
                                rs1.Fields("linkcat").Value = txtLinkcat.Text.ToString.Trim
                                rs1.Fields("exist1a").Value = rs.Fields("existencia").Value
                                rs1.Fields("exist2a").Value = 0
                                rs1.Fields("exist3a").Value = 0
                                rs1.Fields("fisic1").Value = 1
                                rs1.Fields("fisic2").Value = 0
                                rs1.Fields("fisic3").Value = 0
                                rs1.Fields("temporal").Value = 0
                                rs1.Fields("fecha").Value = Format(Now(), "yyyy-MM-dd hh:mm:ss")
                                rs1.Fields("estado").Value = "ACTIVO"
                                rs1.Fields("maquina").Value = SystemInformation.ComputerName
                                rs1.Update()
                                conn.Execute("INSERT INTO bitacora VALUES (" & maxBitacora() & ", '" & cmbBodega.Text & "', '" & rs.Fields("referencia").Value.ToString.Trim & "', '" & rs.Fields("talla").Value.ToString.Trim & "', '" & rs.Fields("color").Value.ToString.Trim & "', '" & txtLinkcat.Text & "', 'NUEVO', " & rs.Fields("existencia").Value & ", 1, 0, '" & Format(Now(), "yyyy-MM-dd hh:mm:ss") & "', '" & SystemInformation.ComputerName & "', '" & txtBloque.Text & "');")
                            Else
                                MsgBox("Producto no existe en bodega selecionada", MsgBoxStyle.Critical, "PRODUCTO")
                                If rs.State = 1 Then rs.Close()
                                If rs1.State = 1 Then rs1.Close()
                                conn.RollbackTrans()
                                Exit Sub
                            End If
                        End If
                    End If
                    If rs1.State = 1 Then rs1.Close()

Fin:
                    conn.CommitTrans()
                End If
            Catch ex As Exception
                conn.RollbackTrans()
                MsgBox("Ocurrió un error.!" & vbCrLf & ex.Message, MsgBoxStyle.Critical, "NUEVO")
                Me.Close()
            Finally
                llenarGrid()
            txtLinkcat.Clear()
            txtLinkcat.Focus()
            End Try
        End If
    End Sub
    Private Sub txtBloque_TextChanged(sender As Object, e As EventArgs) Handles txtBloque.TextChanged

        If Not cmbBodega.Text.Length > 0 Then
            MsgBox("Por favor seleccione una bodega.!", MsgBoxStyle.Information, "BODEGA")
            cmbBodega.Focus()
            Exit Sub
        End If
        If txtBloque.Text.Length > 0 Then
            LEstado.Text = "---"
            DataGridView1.DataSource = Nothing
            llenarGrid()
        Else
            LEstado.Text = "---"
            DataGridView1.DataSource = Nothing
        End If
    End Sub
    Private Sub DataGridView1_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles DataGridView1.MouseDoubleClick
        Try
            If DataGridView1.Rows.Count > 0 Then
                If Not IsDBNull(DataGridView1.CurrentRow.Index) Then
                    If LEstado.Text = "ACTIVO" Then
                        value = False
                        FrmPermiso.TxtUsuario.Clear()
                        FrmPermiso.TxtPass.Clear()
                        FrmPermiso.TxtUsuario.Focus()
                        FrmPermiso.ShowDialog()
                        FrmPermiso.SelectNextControl(FrmPermiso.ActiveControl, True, True, True, True)
                        FrmPermiso.Close()

                        conn.BeginTrans()

                        If value Then
                            Dim rs As New ADODB.Recordset
                            rs.Open("Select * from etiquetas where bloque = " & txtBloque.Text & " and bodega = '" & DataGridView1.Rows(DataGridView1.CurrentRow.Index).Cells(0).Value.ToString.Trim & "' and linkcat = '" & DataGridView1.Rows(DataGridView1.CurrentRow.Index).Cells(1).Value.ToString.Trim & "' and codart = '" & DataGridView1.Rows(DataGridView1.CurrentRow.Index).Cells(2).Value.ToString.Trim & "' and talla = '" & DataGridView1.Rows(DataGridView1.CurrentRow.Index).Cells(3).Value.ToString.Trim & "' and color  = '" & DataGridView1.Rows(DataGridView1.CurrentRow.Index).Cells(4).Value.ToString.Trim & "'", conn, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockOptimistic)
                            If Not rs.EOF Then
                                If rs.Fields("maquina").Value.ToString.Equals(SystemInformation.ComputerName) Then
                                    Dim cadena As String
                                    cadena = DataGridView1.Rows(DataGridView1.CurrentRow.Index).Cells(2).Value.ToString.Trim & " - " & DataGridView1.Rows(DataGridView1.CurrentRow.Index).Cells(3).Value.ToString.Trim & " - " & DataGridView1.Rows(DataGridView1.CurrentRow.Index).Cells(4).Value.ToString.Trim
                                    FrmAux.num = Convert.ToInt32(DataGridView1.Rows(DataGridView1.CurrentRow.Index).Cells(7).Value)
                                    FrmAux.LTitulo.Text = cadena
                                    FrmAux.txtCantidad.Text = "1"
                                    FrmAux.ShowDialog()
                                    FrmAux.txtCantidad.Focus()
                                    If cant > 0 Then
                                        rs.Fields("fisic1").Value -= cant
                                        rs.Update()

                                        If rs.Fields("fisic1").Value = 0 Then
                                            rs.Delete()
                                            conn.Execute("INSERT INTO bitacora VALUES (" & maxBitacora() & ", '" & DataGridView1.Rows(DataGridView1.CurrentRow.Index).Cells(0).Value.ToString.Trim & "', '" & DataGridView1.Rows(DataGridView1.CurrentRow.Index).Cells(2).Value.ToString.Trim & "', '" & DataGridView1.Rows(DataGridView1.CurrentRow.Index).Cells(3).Value.ToString.Trim & "', '" & DataGridView1.Rows(DataGridView1.CurrentRow.Index).Cells(4).Value.ToString.Trim & "', '" & DataGridView1.Rows(DataGridView1.CurrentRow.Index).Cells(1).Value.ToString.Trim & "', 'ELIMINAR', " & DataGridView1.Rows(DataGridView1.CurrentRow.Index).Cells(6).Value.ToString.Trim & ", " & cant & ", " & DataGridView1.Rows(DataGridView1.CurrentRow.Index).Cells(8).Value.ToString.Trim & ", '" & Format(Now(), "yyyy-MM-dd hh:mm:ss") & "', '" & SystemInformation.ComputerName & "', '" & txtBloque.Text & "')")
                                        Else
                                            conn.Execute("INSERT INTO bitacora VALUES (" & maxBitacora() & ", '" & DataGridView1.Rows(DataGridView1.CurrentRow.Index).Cells(0).Value.ToString.Trim & "', '" & DataGridView1.Rows(DataGridView1.CurrentRow.Index).Cells(2).Value.ToString.Trim & "', '" & DataGridView1.Rows(DataGridView1.CurrentRow.Index).Cells(3).Value.ToString.Trim & "', '" & DataGridView1.Rows(DataGridView1.CurrentRow.Index).Cells(4).Value.ToString.Trim & "', '" & DataGridView1.Rows(DataGridView1.CurrentRow.Index).Cells(1).Value.ToString.Trim & "', 'RESTAR', " & DataGridView1.Rows(DataGridView1.CurrentRow.Index).Cells(6).Value.ToString.Trim & ", " & cant & ", " & DataGridView1.Rows(DataGridView1.CurrentRow.Index).Cells(8).Value.ToString.Trim & ", '" & Format(Now(), "yyyy-MM-dd hh:mm:ss") & "', '" & SystemInformation.ComputerName & "', '" & txtBloque.Text & "')")
                                        End If
                                        rs.Update()
                                    End If
                                Else
                                    MsgBox("Linea no agregada con el equipo actual.!" & vbCrLf & "No se puede eliminar.!", MsgBoxStyle.Critical, "ELIMINAR FILA")
                                End If
                            End If
                            If rs.State = 1 Then rs.Close()
                        End If
                        conn.CommitTrans()

                        txtLinkcat.Focus()
                        llenarGrid()
                    End If
                End If
            Else
                DataGridView1.DataSource = Nothing
            End If
        Catch ex As Exception
            conn.RollbackTrans()
            MsgBox("Error al actualizar registro.!" & vbCrLf & ex.Message, MsgBoxStyle.Critical, "ACTUALIZAR")
        End Try
    End Sub
    Private Sub btnFinalizar_Click(sender As Object, e As EventArgs) Handles btnFinalizar.Click
        If txtBloque.Text.Length > 0 Then
            If LEstado.Text = "FINALIZADO" Then
                MsgBox("Este bloque ya a sido finalizado.!", MsgBoxStyle.Information, "BLOQUE")
                Exit Sub
            End If
            Try
                If MsgBox("¿Desea finalizar el bloque " & txtBloque.Text & "?", MsgBoxStyle.Question Or MsgBoxStyle.YesNo, "BLOQUE") = vbYes Then
                    conn.BeginTrans()
                    Dim rs As New ADODB.Recordset
                    rs.Open("Select * from etiquetas where bloque = " & txtBloque.Text & " and maquina = '" & SystemInformation.ComputerName & "'", conn, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockReadOnly)
                    If Not rs.EOF Then
                        Dim i As Integer = 0
                        conn.Execute("UPDATE etiquetas SET estado = 'FINALIZADO' WHERE bloque = " & txtBloque.Text & " and maquina = '" & SystemInformation.ComputerName & "'", i)
                        If i > 0 Then
                            LEstado.Text = "FINALIZADO"
                            LEstado.ForeColor = Color.Red
                            conn.Execute("INSERT INTO bitacora VALUES (" & maxBitacora() & ", '" & cmbBodega.Text & "', '0', '0', '0', '0', 'FINALIZAR', 0, 0, 0, '" & Format(Now(), "yyyy-MM-dd hh:mm:ss") & "', '" & SystemInformation.ComputerName & "', '" & txtBloque.Text & "')")
                            MsgBox("Se finalizaron correctamente " & i & " registros.!", MsgBoxStyle.Information, "FINALIZAR")
                        Else
                            MsgBox("No se actualizó ningún registro.!", MsgBoxStyle.Critical, "ERROR")
                        End If
                    Else
                        MsgBox("Bloque y/o maquina no coinciden.!", MsgBoxStyle.Critical, "FINALIZAR")
                    End If
                    If rs.State = 1 Then rs.Close()
                    conn.CommitTrans()
                    llenarGrid()
                    txtLinkcat.Clear()
                    txtLinkcat.Focus()
                End If
            Catch ex As Exception
                conn.RollbackTrans()
                MsgBox("Error al finalizar bloque.!" & vbCrLf & ex.Message, MsgBoxStyle.Critical, "FINALIZAR")
            End Try
        End If
    End Sub
    Private Sub btnNuevo_Click(sender As Object, e As EventArgs) Handles btnNuevo.Click
        txtBloque.Text = ""
        llenarGrid()
        txtLinkcat.Clear()
        txtLinkcat.Focus()
    End Sub
    Private Sub btnEliminar_Click(sender As Object, e As EventArgs) Handles btnEliminar.Click
        value = False
        FrmPermiso.TxtUsuario.Clear()
        FrmPermiso.TxtPass.Clear()
        FrmPermiso.TxtUsuario.Focus()
        FrmPermiso.ShowDialog()
        FrmPermiso.TxtUsuario.Focus()
        FrmPermiso.SelectNextControl(FrmPermiso.ActiveControl, True, True, True, True)
        FrmPermiso.Close()

        If value Then
            Dim rs As New ADODB.Recordset
            rs.Open("Select * from etiquetas where bodega = '" & cmbBodega.Text & "' and bloque = '" & txtBloque.Text & "'", conn, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockReadOnly)
            If Not rs.EOF Then
                If rs.Fields("maquina").Value.ToString.Equals(SystemInformation.ComputerName) Then
                    If MsgBox("¿Desea eliminar bloque completo?", MsgBoxStyle.Question Or MsgBoxStyle.YesNo, "BORRAR") = vbYes Then
                        Dim i As Integer = 0
                        conn.Execute("Delete from etiquetas where bodega = '" & cmbBodega.Text & "' and bloque = '" & txtBloque.Text & "'", i)
                        If i > 0 Then
                            MsgBox("Se borraron correctamente " & i & " lineas!", MsgBoxStyle.Information)
                        End If

                        txtBloque.Text = ""
                        DataGridView1.DataSource = Nothing
                        txtLinkcat.Focus()
                        llenarGrid()
                    End If
                Else
                    MsgBox("Este bloque no fue creado en esta computadora.!" & vbCrLf & "Por lo tanto no puede ser eliminado.!", MsgBoxStyle.Critical, "ELIMINAR BLOQUE")
                End If
            Else
                MsgBox("No se encontro bloque con bodega seleccionada.!", MsgBoxStyle.Critical, "ELIMINAR BLOQUE")
            End If

        End If
    End Sub
    Private Sub cmbBodega_TextChanged(sender As Object, e As EventArgs) Handles cmbBodega.TextChanged
        If cmbBodega.Text.Length > 0 Then
            If Not cmbBodega.Text = "101" Then
                If Not conn.State = 1 Then
                    If Not conectarLocal() Then
                        errores("Error de conexion.", "No se pudo establecer conexion.")
                        Exit Sub
                    End If
                End If
                Try
                    Dim rs As New ADODB.Recordset
                    Dim da As New System.Data.OleDb.OleDbDataAdapter
                    Dim ds As New DataSet
                    rs.Open("Select nombre from bodega where codigo = '" & cmbBodega.Text & "'", conn, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockReadOnly)
                    If Not rs.EOF Then
                        Lbodega.Text = rs.Fields(0).Value.ToString
                    Else
                        Lbodega.Text = "---"
                    End If
                    If rs.State = 1 Then rs.Close()
                Catch ex As Exception
                    errores(ex.Message, "Buscar bodega.!")
                End Try
            Else
                Lbodega.Text = "---"
            End If
        Else
            Lbodega.Text = "---"
        End If
    End Sub
    Private Sub btnActualizarBase_Click(sender As Object, e As EventArgs) Handles btnActualizarBase.Click
        If Not cnxMysql.State = 1 Then
            If Not conectarPublica() Then
                MsgBox("No se pudo establer conexion con la base de datos.!", MsgBoxStyle.Critical, "ERROR DE CONEXION")
                Exit Sub
            End If
        End If
        If Not conn.State = 1 Then
            If Not conectarLocal() Then
                errores("Error de conexion.", "No se pudo establecer conexion.")
                Exit Sub
            End If
        End If
        If cmbBodega.Text.Length > 0 Then
            If MsgBox("¿Desea actualizar la base de datos de la bodega " & cmbBodega.Text & "?", MsgBoxStyle.YesNo Or MsgBoxStyle.Question, "ACTUALIZAR") = vbYes Then
                Try
                    LEstado.Text = "CARGANDO..."

                    conn.BeginTrans()
                    copiarArticulos()
                    copiarExistencia()
                    copiarBodega()
                    copiarUsuarios()
                    conn.CommitTrans()

                    LEstado.Text = "---"

                    MsgBox("Se completo el proceso.!", MsgBoxStyle.Information, "ACTUALIZACION")
                Catch ex As Exception
                    LEstado.Text = "---"
                    MsgBox("Error al actualizar base de datos.!" & ex.Message, MsgBoxStyle.Critical, "Error")
                    conn.RollbackTrans()
                End Try
            End If
        Else
            MsgBox("Debe seleccionar una bodega para actualizar la base de datos.!", MsgBoxStyle.Information, "BODEGA NO SELECCIONADA")
            cmbBodega.Focus()
        End If
    End Sub
    Private Sub btnReportes_Click(sender As Object, e As EventArgs) Handles btnReportes.Click
        FrmReportes.Show()
        Me.Hide()
        FrmReportes.Lbodega.Text = "---"
        FrmReportes.cmbBodega.Text = cmbBodega.Text
        FrmReportes.Lbodega.Text = Lbodega.Text
    End Sub

    '---------------------------------------------------------------------------------------------------------------------
    'FUNCIONES
    '---------------------------------------------------------------------------------------------------------------------
    Sub llenarGrid()
        Try
            If Not conn.State = 1 Then
                If Not conectarLocal() Then
                    errores("Error de conexion.", "No se pudo establecer conexion.")
                    Exit Sub
                End If
            End If

            Dim rs As New ADODB.Recordset
            Dim da As New System.Data.OleDb.OleDbDataAdapter
            Dim ds As New DataSet
            rs.Open("Select bodega as Bodega, linkcat as Linkcat, codart as Referencia, talla as Talla, color as Color,
            descrip as Descripcion, exist1a as Existencia, fisic1 as Fisico, temporal as Temporal,
            fecha as Fecha, maquina as Equipo, estado from etiquetas where bloque = '" & txtBloque.Text & "' and bodega = '" & cmbBodega.Text & "'", conn, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockReadOnly)
            If Not rs.EOF Then
                DataGridView1.DataSource = Nothing
                da.Fill(ds, rs, "Tabla")
                DataGridView1.DataSource = ds.Tables("Tabla")
                If rs.State = 1 Then rs.Close()

                If DataGridView1.Columns.Count > 0 Then
                    DataGridView1.Columns(0).Visible = False
                    DataGridView1.Columns(1).Visible = False
                    DataGridView1.Columns(11).Visible = False
                End If
            End If

            mostrarCantidades()
            mostrarEstado()
        Catch ex As Exception
            MsgBox("Error al llenar grid.!" & vbCrLf & ex.Message, MsgBoxStyle.Critical, "GRID")
        End Try
    End Sub
    Sub mostrarCantidades()
        If Not conn.State = 1 Then
            If Not conectarLocal() Then
                errores("Error de conexion.", "No se pudo establecer conexion.")
                Exit Sub
            End If
        End If
        Dim rs As New ADODB.Recordset
        rs.Open("SELECT ifnull(sum(fisic1), 0) as tot FROM etiquetas where bloque = '" & txtBloque.Text & "' and bodega = '" & cmbBodega.Text & "'", conn, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockReadOnly)
        If Not rs.EOF Then
            LTotalPiezas.Text = rs.Fields(0).Value
        End If
    End Sub
    Sub mostrarEstado()
        If Not conn.State = 1 Then
            If Not conectarLocal() Then
                errores("Error de conexion.", "No se pudo establecer conexion.")
                Exit Sub
            End If
        End If

        Dim rs As New ADODB.Recordset
        rs.Open("SELECT estado FROM etiquetas where bloque = '" & txtBloque.Text & "' and bodega = '" & cmbBodega.Text & "'", conn, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockReadOnly)
        If Not rs.EOF Then
            LEstado.Text = rs.Fields(0).Value
            LEstado.ForeColor = Color.Blue
            If LEstado.Text = "FINALIZADO" Then
                LEstado.ForeColor = Color.Red
            End If
        Else
            LEstado.Text = "---"
        End If
    End Sub
    Function maxBitacora()
        If Not conn.State = 1 Then
            If Not conectarLocal() Then
                errores("Error de conexion.", "No se pudo establecer conexion.")
                Return 0
                Exit Function
            End If
        End If
        Dim rs As New ADODB.Recordset
        rs.Open("Select ifnull(max(id) + 1, 1) from bitacora", conn, CursorTypeEnum.adOpenStatic, LockTypeEnum.adLockReadOnly)
        If Not rs.EOF Then
            Return rs.Fields(0).Value
        Else
            Return 1
        End If
    End Function
    Function buscarBloque() As Integer
        If Not conn.State = 1 Then
            If Not conectarLocal() Then
                errores("Error de conexion.", "No se pudo establecer conexion.")
                Return 0
                Exit Function
            End If
        End If
        buscarBloque = 0
        Dim rsBlo As New ADODB.Recordset
        rsBlo.Open("Select ifnull(Max(bloque) + 1, 1) as bloque from etiquetas where bodega = '" & cmbBodega.Text & "'", conn, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockReadOnly)
        If Not rsBlo.EOF Then
            buscarBloque = rsBlo.Fields("bloque").Value
        End If
        If rsBlo.State = 1 Then rsBlo.Close()
        If buscarBloque <> 0 Then
            Return buscarBloque
        End If
    End Function

    'FUNCION PARA COPIAR
    Sub copiarBodega()
        If Not conn.State = 1 Then
            If Not conectarLocal() Then
                errores("Error de conexion.", "No se pudo establecer conexion.")
                Exit Sub
            End If
        End If

        Dim rsFuente As New ADODB.Recordset
            rsFuente.Open("Select distinct(codigo), nombre from tienda.bodega group by 1,2", cnxMysql, CursorTypeEnum.adOpenStatic, LockTypeEnum.adLockReadOnly)
            While Not rsFuente.EOF
                Dim rs As New ADODB.Recordset
                rs.Open("Select * from bodega where codigo = " & rsFuente.Fields(0).Value & "", conn, CursorTypeEnum.adOpenStatic, LockTypeEnum.adLockOptimistic)
                If rs.EOF Then
                    rs.AddNew()
                    rs.Fields(0).Value = rsFuente.Fields(0).Value
                    rs.Fields(1).Value = rsFuente.Fields(1).Value
                    rs.Update()
                End If
                If rs.State = 1 Then rs.Close()
                rsFuente.MoveNext()
            End While
        If rsFuente.State = 1 Then rsFuente.Close()
    End Sub
    Sub copiarUsuarios()
        If conn.State = 1 Then

            Dim rsFuente As New ADODB.Recordset
            rsFuente.Open("Select login, nombre, clave from generales.usuarios;", cnxMysql, CursorTypeEnum.adOpenStatic, LockTypeEnum.adLockReadOnly)
            While Not rsFuente.EOF
                Dim rs As New ADODB.Recordset
                rs.Open("Select * from usuarios where login = '" & rsFuente.Fields(0).Value & "'", conn, CursorTypeEnum.adOpenStatic, LockTypeEnum.adLockOptimistic)
                If rs.EOF Then
                    'SI ES NUEVO SE CREA EL USUARIO.
                    rs.AddNew()
                    rs.Fields(0).Value = rsFuente.Fields(0).Value
                    rs.Fields(1).Value = rsFuente.Fields(1).Value
                    rs.Fields(2).Value = rsFuente.Fields(2).Value
                    rs.Update()
                Else
                    'SE ACTUALIZA NOMBRE Y CLAVE
                    rs.Fields(1).Value = rsFuente.Fields(1).Value
                    rs.Fields(2).Value = rsFuente.Fields(2).Value
                    rs.Update()
                End If
                If rs.State = 1 Then rs.Close()
                rsFuente.MoveNext()
            End While
            If rsFuente.State = 1 Then rsFuente.Close()
        Else
            MsgBox("Conexion local no abierta.!", MsgBoxStyle.Critical, "ERROR")
        End If
    End Sub
    Sub copiarExistencia()
        If conn.State = 1 Then
            'SI LA BODEGA ES 0 SE COPIAN TODAS LAS EXISTENCIAS DE LA BASE.
            If cmbBodega.Text = "0" Then
                Dim rsFuente As New ADODB.Recordset
                rsFuente.Open("SELECT bodega, plu, existencia FROM tienda.existencias order by 1;", cnxMysql, CursorTypeEnum.adOpenStatic, LockTypeEnum.adLockReadOnly)
                While Not rsFuente.EOF
                    Dim rs As New ADODB.Recordset
                    rs.Open("Select * from existencias where bodega = " & rsFuente.Fields(0).Value & " and plu = '" & rsFuente.Fields(1).Value & "'", conn, CursorTypeEnum.adOpenStatic, LockTypeEnum.adLockOptimistic)
                    If rs.EOF Then
                        'SI NO EXISTE CREA UNO.
                        rs.AddNew()
                        rs.Fields(0).Value = rsFuente.Fields(0).Value
                        rs.Fields(1).Value = rsFuente.Fields(1).Value
                        rs.Fields(2).Value = rsFuente.Fields(2).Value
                        rs.Update()
                    Else
                        'SI EXISTE LO ACTUALIZA.
                        rs.Fields(2).Value = rs.Fields(2).Value
                        rs.Update()
                    End If
                    If rs.State = 1 Then rs.Close()

                    rsFuente.MoveNext()
                End While
                If rsFuente.State = 1 Then rsFuente.Close()
            Else
                'SI LA BODEGA ES DIFERENTE DE 0 SE ACTUALIZA LA INFORMACION SOLO DE ESA BODEGA EN ESPECIFICO.
                conn.Execute("Delete from existencias where bodega = " & cmbBodega.Text & "")
                Dim rsFuente As New ADODB.Recordset
                rsFuente.Open("SELECT bodega, plu, existencia FROM tienda.existencias where bodega = " & cmbBodega.Text & " order by 2;", cnxMysql, CursorTypeEnum.adOpenStatic, LockTypeEnum.adLockReadOnly)
                While Not rsFuente.EOF
                    Dim rs As New ADODB.Recordset
                    rs.Open("Select * from existencias where bodega = '" & rsFuente.Fields(0).Value & "' and plu = '" & rsFuente.Fields(1).Value & "'", conn, CursorTypeEnum.adOpenStatic, LockTypeEnum.adLockOptimistic)
                    If rs.EOF Then
                        'SI NO EXISTE CREA UNO.
                        rs.AddNew()
                        rs.Fields(0).Value = rsFuente.Fields(0).Value
                        rs.Fields(1).Value = rsFuente.Fields(1).Value
                        rs.Fields(2).Value = rsFuente.Fields(2).Value
                        rs.Update()
                    Else
                        'SI EXISTE LO ACTUALIZA.
                        rs.Fields(2).Value = rs.Fields(2).Value
                        rs.Update()
                    End If
                    If rs.State = 1 Then rs.Close()

                    rsFuente.MoveNext()
                End While
                If rsFuente.State = 1 Then rsFuente.Close()
            End If
        Else
            MsgBox("Conexion local no abierta.!", MsgBoxStyle.Critical, "ERROR")
        End If
    End Sub
    Sub copiarArticulos()
        If conn.State = 1 Then
            Dim rsFuente As New ADODB.Recordset
            'SI LA BODEGA ES 0 ENTONCES SE COPIAN TODOS LOS ARTICULOS INDICANDO QUE DE ESTA MANERA SE GUARDE TODA LA INFORMACION.
            If cmbBodega.Text = "0" Then
                conn.Execute("Delete from articulos;")
                rsFuente.Open("Select articulos.plu, articulos.lincod, articulos.referencia, articulos.talla, 
                articulos.color, articulos.coddge, articulos.descripcion, articulos.barra from tienda.existencias, tienda.articulos 
                where articulos.plu = existencias.plu;", cnxMysql, CursorTypeEnum.adOpenStatic, LockTypeEnum.adLockReadOnly)
                While Not rsFuente.EOF
                    Dim rs As New ADODB.Recordset
                    rs.Open("Select * from articulos where plu = '" & rsFuente.Fields(0).Value & "' and barra = '" & rsFuente.Fields(7).Value & "'", conn, CursorTypeEnum.adOpenStatic, LockTypeEnum.adLockOptimistic)
                    If rs.EOF Then
                        rs.AddNew()
                        rs.Fields(0).Value = rsFuente.Fields(0).Value
                        rs.Fields(1).Value = rsFuente.Fields(1).Value
                        rs.Fields(2).Value = rsFuente.Fields(2).Value
                        rs.Fields(3).Value = rsFuente.Fields(3).Value
                        rs.Fields(4).Value = rsFuente.Fields(4).Value
                        rs.Fields(5).Value = rsFuente.Fields(5).Value
                        rs.Fields(6).Value = rsFuente.Fields(6).Value
                        rs.Fields(7).Value = rsFuente.Fields(7).Value
                        rs.Update()
                    End If
                    If rs.State = 1 Then rs.Close()
                    rsFuente.MoveNext()
                End While
                If rsFuente.State = 1 Then rsFuente.Close()
            Else
                'SI LA BODEGA NO ES 0 ENTONCES SE COPIA LA INFORMACION SOLO DE LA BODEGA SELECCIONADA.
                rsFuente.Open("Select articulos.plu, articulos.lincod, articulos.referencia, articulos.talla, 
                articulos.color, articulos.coddge, articulos.descripcion, articulos.barra from tienda.existencias, tienda.articulos 
                where existencias.bodega = '" & cmbBodega.Text & "' and articulos.plu = existencias.plu;", cnxMysql, CursorTypeEnum.adOpenStatic, LockTypeEnum.adLockReadOnly)
                While Not rsFuente.EOF
                    Dim rs As New ADODB.Recordset
                    rs.Open("Select * from articulos where plu = '" & rsFuente.Fields(0).Value & "'", conn, CursorTypeEnum.adOpenStatic, LockTypeEnum.adLockOptimistic)
                    If rs.EOF Then
                        rs.AddNew()
                        rs.Fields(0).Value = rsFuente.Fields(0).Value
                        rs.Fields(1).Value = rsFuente.Fields(1).Value
                        rs.Fields(2).Value = rsFuente.Fields(2).Value
                        rs.Fields(3).Value = rsFuente.Fields(3).Value
                        rs.Fields(4).Value = rsFuente.Fields(4).Value
                        rs.Fields(5).Value = rsFuente.Fields(5).Value
                        rs.Fields(6).Value = rsFuente.Fields(6).Value
                        rs.Fields(7).Value = rsFuente.Fields(7).Value
                        rs.Update()
                    End If
                    If rs.State = 1 Then rs.Close()
                    rsFuente.MoveNext()
                End While
                If rsFuente.State = 1 Then rsFuente.Close()
            End If
        Else
            MsgBox("Conexion local no abierta.!", MsgBoxStyle.Critical, "ERROR")
        End If
    End Sub
    Private Sub txtLinkcat_TextChanged(sender As Object, e As EventArgs) Handles txtLinkcat.TextChanged
        If txtLinkcat.Text = "CREAR BD" Then
            If crearBase() = False Then
                MsgBox("Error al crear la base de datos.!", MsgBoxStyle.Information, "ERROR")
                Exit Sub
            End If
        End If
        If txtLinkcat.Text = "BORRAR ETIQUETAS" Then
            If Not conectarLocal() Then
                MsgBox("No se pudo conectar a la BD!", MsgBoxStyle.Critical, "ERROR")
                Exit Sub
            End If
            If MsgBox("¿Desea eliminar la tabla de etiquetas?", MsgBoxStyle.Question Or MsgBoxStyle.YesNo, "BORRAR") = vbYes Then
                Dim i As Integer = 0
                conn.Execute("Delete from etiquetas;", i)
                MsgBox("Se eliminaron: " & i & " registros.!", MsgBoxStyle.Information, "ELIMINACION")
                Application.Exit()
            End If
        End If
    End Sub
End Class