Public Class FrmAux
    Public num As Integer = 0
    Private Sub Label1_Click(sender As Object, e As EventArgs) Handles Label1.Click
        FrmInventario.cant = 0
        Me.Close()
    End Sub

    Private Sub FrmAux_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        FrmInventario.BringToFront()
    End Sub

    Private Sub btnAceptar_Click(sender As Object, e As EventArgs) Handles btnAceptar.Click
        If Not txtCantidad.Text.Length > 0 Then Exit Sub

        If IsNumeric(txtCantidad.Text) Then
            If txtCantidad.Text > 0 Then
                If Convert.ToInt32(txtCantidad.Text) > num Then
                    MsgBox("Cantidad no puede ser mayor a fisico.!", MsgBoxStyle.Information, "CANTIDAD")
                    txtCantidad.Focus()
                    Exit Sub
                End If
                FrmInventario.cant = Convert.ToInt16(txtCantidad.Text)
                Me.Close()
            Else
                MsgBox("El numero debe ser mayor a 0.!", MsgBoxStyle.Critical, "NUMEROS")
            End If
        Else
                MsgBox("Solo puede ingresar numeros.!", MsgBoxStyle.Critical, "NUMEROS")
        End If
    End Sub

    Private Sub FrmAux_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

    Private Sub txtCantidad_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtCantidad.KeyPress
        If e.KeyChar = Convert.ToChar(Keys.Enter) And txtCantidad.Text.Length > 0 Then
            btnAceptar.PerformClick()
        End If
    End Sub
End Class