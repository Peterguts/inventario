
Imports System.IO
Imports System.Windows.Forms
Imports Microsoft.Office.Interop

Public Class FrmReportes
    Public var As String
    'EVENTO
    Private Sub FrmReportes_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        FrmInventario.Show()
    End Sub
    'BOTONES
    Private Sub btnExportar_Click(sender As Object, e As EventArgs) Handles btnExportar.Click
        If DataGridView1.Rows.Count > 0 Then
            ExportarDataGridViewAExcel(DataGridView1)
        End If
    End Sub
    Private Sub btnFinalizados_Click(sender As Object, e As EventArgs) Handles btnFinalizados.Click
        If Not cmbBodega.Text.Length > 0 Then Exit Sub
        llenarGrid("SELECT bodega,
                    codart AS Referencia,
                    talla AS Talla,
                    color AS Color,
                    linkcat AS Barra,
                    descrip AS Descripcion,
                    exist1a AS Existencia,
                    fisic1 AS Fisico,
                    strftime('%Y-%m-%d', fecha) AS Fecha,
                    maquina,
                    bloque
                    FROM etiquetas
                    WHERE estado = 'FINALIZADO' and bodega = " & cmbBodega.Text & "
                    GROUP BY 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11
                    ORDER BY bloque, bodega;")
        var = "Finalizados"
    End Sub
    Private Sub btnActivos_Click(sender As Object, e As EventArgs) Handles btnActivos.Click
        If Not cmbBodega.Text.Length > 0 Then Exit Sub
        llenarGrid("SELECT bodega,
                    codart AS Referencia,
                    talla AS Talla,
                    color AS Color,
                    linkcat AS Barra,
                    descrip AS Descripcion,
                    exist1a AS Existencia,
                    fisic1 AS Fisico,
                    strftime('%Y-%m-%d', fecha) AS Fecha,
                    maquina,
                    bloque
                    FROM etiquetas
                    WHERE estado = 'ACTIVO' and bodega = " & cmbBodega.Text & "
                    GROUP BY 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11
                    ORDER BY bloque, bodega;")
        var = "Activos"
    End Sub
    Private Sub btnComparativo_Click(sender As Object, e As EventArgs) Handles btnComparativo.Click
        If Not cmbBodega.Text.Length > 0 Then Exit Sub
        llenarGrid("SELECT
                Bodega,
                Plu,
                Referencia,
                Talla,
                Color,
                Barra,
                Descripcion,
                SUM(Existencia) AS Existencia,
                SUM(Fisico) AS Fisico,
                SUM(Fisico) - SUM(Existencia) AS Diferencia
            FROM (
                SELECT
                    bodega,
                    TRIM(existencias.plu) AS Plu,
                    TRIM(articulos.referencia) AS Referencia,
                    TRIM(articulos.talla) AS Talla,
                    TRIM(articulos.color) AS Color,
                    TRIM(barra) AS Barra,
                    TRIM(descripcion) AS Descripcion,
                    existencias.existencia AS Existencia,
                    0 AS Fisico
                FROM
                    existencias
                    INNER JOIN articulos ON articulos.plu = existencias.plu
                WHERE
                    bodega = " & cmbBodega.Text & " and existencia <> 0
                GROUP BY
                    bodega,
                    existencias.plu,
                    articulos.referencia,
                    articulos.talla,
                    articulos.color,
                    barra,
                    descripcion,
                    existencias.existencia

                UNION ALL

                SELECT
                    bodega,
                    TRIM(codart) || TRIM(talla) || TRIM(color) AS Plu,
                    TRIM(codart) AS Referencia,
                    TRIM(talla) AS Talla,
                    TRIM(color) AS Color,
                    TRIM(linkcat) AS Barra,
                    TRIM(descrip) AS Descripcion,
                    0 AS Existencia,
                    SUM(fisic1) AS Fisico
                FROM
                    etiquetas
                WHERE
                    estado = 'FINALIZADO'
                    AND bodega = " & cmbBodega.Text & "
                GROUP BY
                    bodega,
                    codart,
                    talla,
                    color,
                    linkcat,
                    descrip
            ) AS subquery
            GROUP BY
                Bodega,
                Plu,
                Referencia,
                Talla,
                Color,
                Barra,
                Descripcion;")
        var = "Comparativo"
    End Sub
    Private Sub DataGridView1_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles DataGridView1.MouseDoubleClick
        Try
            If DataGridView1.Rows.Count > 0 Then
                If DataGridView1.Columns.Count = 10 Then
                    Exit Sub
                End If
                If Not IsDBNull(DataGridView1.CurrentRow.Index) Then
                    If DataGridView1.Columns.Count > 6 Then
                        If Not conn.State = 1 Then
                            If Not conectarLocal() Then
                                errores("Error de conexion.", "No se pudo establecer conexion.")
                                Exit Sub
                            End If
                        End If

                        Dim rs As New ADODB.Recordset
                        rs.Open("Select * from etiquetas where bodega = '" & DataGridView1.Rows(DataGridView1.CurrentRow.Index).Cells(0).Value.ToString.Trim & "' and bloque  = '" & DataGridView1.Rows(DataGridView1.CurrentRow.Index).Cells(10).Value.ToString.Trim & "'", conn, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockOptimistic)
                        If Not rs.EOF Then
                            FrmInventario.cmbBodega.Text = rs.Fields("bodega").Value
                            FrmInventario.txtBloque.Text = rs.Fields("bloque").Value
                            FrmInventario.llenarGrid()
                            FrmInventario.Show()
                            Me.Close()
                        End If
                        If rs.State = 1 Then rs.Close()
                    End If
                End If
            End If
        Catch ex As Exception
            errores(ex.Message, "BUSCAR BLOQUE")
        End Try
    End Sub

    'FUNCIONES
    '---------------------------------------------------------
    Public Sub ExportarDataGridViewAExcel(dataGridView As DataGridView)
        ' Crear un OpenFileDialog para que el usuario seleccione la ubicación y el nombre del archivo
        Dim saveFileDialog As New SaveFileDialog()
        'saveFileDialog.Filter = "Archivos de Excel 97-2003 (*.xls)|*.xls"
        'saveFileDialog.Title = "Guardar archivo de Excel"
        'saveFileDialog.FileName = var & ".xls"

        saveFileDialog.Filter = "Archivos de Excel (*.xlsx)|*.xlsx"
        saveFileDialog.Title = "Guardar archivo de Excel"
        saveFileDialog.FileName = var & ".xlsx"

        ' Mostrar el diálogo y comprobar si el usuario hizo clic en el botón "Guardar"
        If saveFileDialog.ShowDialog() = DialogResult.OK Then
            ' Obtener la ruta completa del archivo seleccionado
            Dim nombreArchivo As String = saveFileDialog.FileName

            ' Crear una nueva instancia de Excel
            Dim excelApp As New Excel.Application()
            Dim excelWorkbook As Excel.Workbook = excelApp.Workbooks.Add()
            Dim excelWorksheet As Excel.Worksheet = excelWorkbook.Sheets(1)

            ' Copiar los encabezados de las columnas
            For i As Integer = 1 To dataGridView.Columns.Count
                excelWorksheet.Cells(1, i) = dataGridView.Columns(i - 1).HeaderText
            Next

            ' Copiar los datos de las celdas
            For i As Integer = 0 To dataGridView.Rows.Count - 1
                For j As Integer = 0 To dataGridView.Columns.Count - 1
                    excelWorksheet.Cells(i + 2, j + 1) = dataGridView.Rows(i).Cells(j).Value.ToString()
                Next
            Next

            ' Guardar el archivo de Excel
            'excelWorkbook.SaveAs(nombreArchivo)
            If File.Exists(nombreArchivo) Then
                File.Delete(nombreArchivo)
            End If

            excelWorkbook.SaveAs(nombreArchivo, Excel.XlFileFormat.xlOpenXMLWorkbook)

            ' Cerrar Excel
            excelWorkbook.Close()
            excelApp.Quit()

            ' Liberar recursos
            System.Runtime.InteropServices.Marshal.ReleaseComObject(excelWorksheet)
            System.Runtime.InteropServices.Marshal.ReleaseComObject(excelWorkbook)
            System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp)

            excelWorksheet = Nothing
            excelWorkbook = Nothing
            excelApp = Nothing

            ' Mostrar mensaje de finalización
            MsgBox("El exportó correctamente a Excel en la ubicación seleccionada.!", MsgBoxStyle.Information, "EXCEL")
        End If
    End Sub
    Sub llenarGrid(ByVal consulta As String)
        If Not conn.State = 1 Then
            If Not conectarLocal() Then
                MsgBox("No se pudo conectar con la base de datos.!", MsgBoxStyle.Critical, "ERROR")
                Exit Sub
            End If
        End If
        Dim rs As New ADODB.Recordset
        Dim da As New System.Data.OleDb.OleDbDataAdapter
        Dim ds As New DataSet
        rs.Open(consulta, conn, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockReadOnly)
        DataGridView1.DataSource = Nothing
        If Not rs.EOF Then
            da.Fill(ds, rs, "Datos")
            DataGridView1.DataSource = ds.Tables("Datos")
        End If
        If rs.State = 1 Then rs.Close()
    End Sub

    Private Sub FrmReportes_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Lbodega.Text = "---"
        llenarBodega(cmbBodega)
    End Sub

    Private Sub cmbBodega_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbBodega.SelectedIndexChanged
        If cmbBodega.Text.Length > 0 Then
            If Not cmbBodega.Text = "101" Then
                If Not conn.State = 1 Then
                    If Not conectarLocal() Then
                        errores("Error de conexion.", "No se pudo establecer conexion.")
                        Exit Sub
                    End If
                End If
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
            Else
                Lbodega.Text = "---"
            End If
        Else
            Lbodega.Text = "---"
        End If
    End Sub

    Private Sub cmbBodega_TextChanged(sender As Object, e As EventArgs) Handles cmbBodega.TextChanged
        Call cmbBodega_SelectedIndexChanged(sender, e)
    End Sub
End Class