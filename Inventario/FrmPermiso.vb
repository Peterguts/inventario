Imports System.Runtime.InteropServices

Public Class FrmPermiso
    Private Sub TxtUsuario_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TxtUsuario.KeyPress
        If e.KeyChar = Convert.ToChar(Keys.Enter) And TxtUsuario.Text.Length > 0 Then
            TxtPass.Focus()
        End If
    End Sub

    Private Sub TxtPass_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TxtPass.KeyPress
        If TxtUsuario.Text.Length > 0 Then
            If e.KeyChar = Convert.ToChar(Keys.Enter) And TxtPass.Text.Length > 0 Then

                If Not conn.State = 1 Then
                    If Not conectarLocal() Then
                        errores("Error de conexion.", "No se pudo establecer conexion.")
                        Exit Sub
                    End If
                End If

                Dim rs As New ADODB.Recordset
                    rs.Open("Select * from usuarios where login = '" & TxtUsuario.Text & "' and clave = '" & TxtPass.Text & "'", conn, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockReadOnly)
                If Not rs.EOF Then
                    FrmInventario.value = True
                    Me.Close()
                    FrmInventario.BringToFront()
                Else
                    MsgBox("Usuario o contraseña incorrecta.!", MsgBoxStyle.Critical, "CONTRASEÑA")
                    TxtPass.Focus()
                End If
            End If
        End If
    End Sub

    Private Sub FrmPermiso_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        TxtUsuario.Focus()
    End Sub
End Class