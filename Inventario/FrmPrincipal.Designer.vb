<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmPrincipal
    Inherits System.Windows.Forms.Form

    'Form reemplaza a Dispose para limpiar la lista de componentes.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Requerido por el Diseñador de Windows Forms
    Private components As System.ComponentModel.IContainer

    'NOTA: el Diseñador de Windows Forms necesita el siguiente procedimiento
    'Se puede modificar usando el Diseñador de Windows Forms.  
    'No lo modifique con el editor de código.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.txtUbicacion = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.txtProducto = New System.Windows.Forms.TextBox()
        Me.btnFinalizar = New System.Windows.Forms.Button()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.cmbBodega = New System.Windows.Forms.ComboBox()
        Me.Lpaquetes = New System.Windows.Forms.ListBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.txtPiezas = New System.Windows.Forms.TextBox()
        Me.Lcolores = New System.Windows.Forms.ListBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Txtpiezaspaquete = New System.Windows.Forms.TextBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.TxtExistencia = New System.Windows.Forms.TextBox()
        Me.TxtPaquete = New System.Windows.Forms.TextBox()
        Me.txtReferencia = New System.Windows.Forms.TextBox()
        Me.txtTalla = New System.Windows.Forms.TextBox()
        Me.txtColor = New System.Windows.Forms.TextBox()
        Me.txtDescripcion = New System.Windows.Forms.TextBox()
        Me.SuspendLayout()
        '
        'txtUbicacion
        '
        Me.txtUbicacion.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtUbicacion.Location = New System.Drawing.Point(27, 49)
        Me.txtUbicacion.Name = "txtUbicacion"
        Me.txtUbicacion.Size = New System.Drawing.Size(214, 29)
        Me.txtUbicacion.TabIndex = 0
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(23, 25)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(81, 21)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "Ubicación:"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(23, 84)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(76, 21)
        Me.Label2.TabIndex = 3
        Me.Label2.Text = "Producto:"
        '
        'txtProducto
        '
        Me.txtProducto.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtProducto.Location = New System.Drawing.Point(27, 108)
        Me.txtProducto.Name = "txtProducto"
        Me.txtProducto.Size = New System.Drawing.Size(214, 29)
        Me.txtProducto.TabIndex = 10
        '
        'btnFinalizar
        '
        Me.btnFinalizar.Font = New System.Drawing.Font("Segoe UI", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnFinalizar.Location = New System.Drawing.Point(507, 49)
        Me.btnFinalizar.Name = "btnFinalizar"
        Me.btnFinalizar.Size = New System.Drawing.Size(340, 88)
        Me.btnFinalizar.TabIndex = 50
        Me.btnFinalizar.Text = "Finalizar Inventario"
        Me.btnFinalizar.UseVisualStyleBackColor = True
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(303, 56)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(65, 21)
        Me.Label3.TabIndex = 5
        Me.Label3.Text = "Bodega:"
        '
        'cmbBodega
        '
        Me.cmbBodega.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbBodega.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbBodega.FormattingEnabled = True
        Me.cmbBodega.Items.AddRange(New Object() {"101"})
        Me.cmbBodega.Location = New System.Drawing.Point(307, 80)
        Me.cmbBodega.Name = "cmbBodega"
        Me.cmbBodega.Size = New System.Drawing.Size(139, 29)
        Me.cmbBodega.TabIndex = 15
        '
        'Lpaquetes
        '
        Me.Lpaquetes.Font = New System.Drawing.Font("Segoe UI Semibold", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Lpaquetes.FormattingEnabled = True
        Me.Lpaquetes.ItemHeight = 21
        Me.Lpaquetes.Location = New System.Drawing.Point(17, 293)
        Me.Lpaquetes.Name = "Lpaquetes"
        Me.Lpaquetes.Size = New System.Drawing.Size(309, 214)
        Me.Lpaquetes.TabIndex = 35
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(691, 300)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(140, 21)
        Me.Label4.TabIndex = 9
        Me.Label4.Text = "Piezas sin paquete:"
        '
        'txtPiezas
        '
        Me.txtPiezas.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPiezas.Location = New System.Drawing.Point(661, 324)
        Me.txtPiezas.Name = "txtPiezas"
        Me.txtPiezas.Size = New System.Drawing.Size(214, 29)
        Me.txtPiezas.TabIndex = 20
        '
        'Lcolores
        '
        Me.Lcolores.Font = New System.Drawing.Font("Segoe UI Semibold", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Lcolores.FormattingEnabled = True
        Me.Lcolores.ItemHeight = 21
        Me.Lcolores.Location = New System.Drawing.Point(332, 293)
        Me.Lcolores.Name = "Lcolores"
        Me.Lcolores.Size = New System.Drawing.Size(309, 256)
        Me.Lcolores.TabIndex = 45
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(691, 375)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(152, 21)
        Me.Label5.TabIndex = 12
        Me.Label5.Text = "Total piezas paquete:"
        '
        'Txtpiezaspaquete
        '
        Me.Txtpiezaspaquete.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Txtpiezaspaquete.Location = New System.Drawing.Point(661, 399)
        Me.Txtpiezaspaquete.Name = "Txtpiezaspaquete"
        Me.Txtpiezaspaquete.Size = New System.Drawing.Size(214, 29)
        Me.Txtpiezaspaquete.TabIndex = 25
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(727, 454)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(80, 21)
        Me.Label6.TabIndex = 14
        Me.Label6.Text = "Existencia:"
        '
        'TxtExistencia
        '
        Me.TxtExistencia.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtExistencia.Location = New System.Drawing.Point(661, 478)
        Me.TxtExistencia.Name = "TxtExistencia"
        Me.TxtExistencia.Size = New System.Drawing.Size(214, 29)
        Me.TxtExistencia.TabIndex = 30
        '
        'TxtPaquete
        '
        Me.TxtPaquete.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtPaquete.Location = New System.Drawing.Point(17, 520)
        Me.TxtPaquete.Name = "TxtPaquete"
        Me.TxtPaquete.Size = New System.Drawing.Size(309, 29)
        Me.TxtPaquete.TabIndex = 40
        '
        'txtReferencia
        '
        Me.txtReferencia.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtReferencia.Location = New System.Drawing.Point(27, 169)
        Me.txtReferencia.Name = "txtReferencia"
        Me.txtReferencia.Size = New System.Drawing.Size(214, 29)
        Me.txtReferencia.TabIndex = 51
        '
        'txtTalla
        '
        Me.txtTalla.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtTalla.Location = New System.Drawing.Point(27, 204)
        Me.txtTalla.Name = "txtTalla"
        Me.txtTalla.Size = New System.Drawing.Size(214, 29)
        Me.txtTalla.TabIndex = 52
        '
        'txtColor
        '
        Me.txtColor.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtColor.Location = New System.Drawing.Point(27, 239)
        Me.txtColor.Name = "txtColor"
        Me.txtColor.Size = New System.Drawing.Size(214, 29)
        Me.txtColor.TabIndex = 53
        '
        'txtDescripcion
        '
        Me.txtDescripcion.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDescripcion.Location = New System.Drawing.Point(247, 204)
        Me.txtDescripcion.Name = "txtDescripcion"
        Me.txtDescripcion.Size = New System.Drawing.Size(628, 29)
        Me.txtDescripcion.TabIndex = 54
        '
        'FrmPrincipal
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(898, 561)
        Me.Controls.Add(Me.txtDescripcion)
        Me.Controls.Add(Me.txtColor)
        Me.Controls.Add(Me.txtTalla)
        Me.Controls.Add(Me.txtReferencia)
        Me.Controls.Add(Me.TxtPaquete)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.TxtExistencia)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.Txtpiezaspaquete)
        Me.Controls.Add(Me.Lcolores)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.txtPiezas)
        Me.Controls.Add(Me.Lpaquetes)
        Me.Controls.Add(Me.cmbBodega)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.btnFinalizar)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.txtProducto)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.txtUbicacion)
        Me.Name = "FrmPrincipal"
        Me.Text = "Inventario"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents txtUbicacion As TextBox
    Friend WithEvents Label1 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents txtProducto As TextBox
    Friend WithEvents btnFinalizar As Button
    Friend WithEvents Label3 As Label
    Friend WithEvents cmbBodega As ComboBox
    Friend WithEvents Lpaquetes As ListBox
    Friend WithEvents Label4 As Label
    Friend WithEvents txtPiezas As TextBox
    Friend WithEvents Lcolores As ListBox
    Friend WithEvents Label5 As Label
    Friend WithEvents Txtpiezaspaquete As TextBox
    Friend WithEvents Label6 As Label
    Friend WithEvents TxtExistencia As TextBox
    Friend WithEvents TxtPaquete As TextBox
    Friend WithEvents txtReferencia As TextBox
    Friend WithEvents txtTalla As TextBox
    Friend WithEvents txtColor As TextBox
    Friend WithEvents txtDescripcion As TextBox
End Class
