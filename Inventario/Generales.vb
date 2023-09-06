Imports System.IO
Imports ADODB
Module Generales
    Public conn As Connection
    Public cnxMysql As New ADODB.Connection
    Public cadenaMysql As String = "Driver={MySQL ODBC 5.3 ANSI Driver};Server=127.0.0.1;Database=prueba;User=pedro;Password=pedroPabloCela202355;Option=3;"
    'CONECTAR A LAS BASES, LOCAL Y LA PUBLICA.
    Function conectarLocal()
        Try
            conn = New Connection
            conn.Open("Driver=SQLite3 ODBC Driver;Database=C:\Inventario\inventario.s3db;")
            If conn.State = 1 Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            Return False
        End Try
    End Function
    Function conectarPublica()
        Try
            cnxMysql.Open(cadenaMysql)
            If cnxMysql.State = 1 Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            Return False
        End Try
    End Function
    'VERIFICA QUE EXISTA LA BASE DE DATOS, DE LO CONTRARIO CREA LA ESTRUCTURA.
    Public cont As Integer = 0
    Function crearBase() As Boolean
        crearBase = False
        'VERIFICAMOS QUE NO EXISTA LA BASE DE DATOS.
        If Not File.Exists("C:\Inventario\inventario.s3db") Then
            'CREAMOS LA CARPETA Y LA BASE DE DATOS.
            If Not Directory.Exists("C:\Inventario") Then
                Directory.CreateDirectory("C:\Inventario")

                If Directory.Exists("C:\Inventario") Then
                    File.Create("C:\Inventario\inventario.s3db")
                End If
            End If
        End If

        If File.Exists("C:\Inventario\inventario.s3db") Then
            Dim conector As Connection
            conector = New Connection
            Try
                conector.Open("Driver=SQLite3 ODBC Driver;Database=C:\Inventario\inventario.s3db;")
            Catch ex As Exception
                Application.Restart()
            End Try

            If conector.State = 1 Then
                Dim i As Integer = 0
                Dim sql As String
                sql = "CREATE TABLE IF NOT EXISTS articulos (
                           plu TEXT NOT NULL,
                           lincod TEXT DEFAULT NULL,
                           referencia TEXT DEFAULT NULL,
                           talla TEXT DEFAULT NULL,
                           color INTEGER DEFAULT NULL,
                           coddge INTEGER DEFAULT NULL,
                           descripcion TEXT DEFAULT NULL,
                           barra TEXT NOT NULL,
                           PRIMARY KEY (plu, barra)
                        )"
                conector.Execute(sql, i)

                sql = "CREATE TABLE IF NOT EXISTS bitacora (
                       id INTEGER PRIMARY KEY AUTOINCREMENT,
                       bodega INTEGER NOT NULL,
                       codart TEXT NOT NULL,
                       talla TEXT NOT NULL,
                       color TEXT NOT NULL,
                       linkcat TEXT NOT NULL,
                       accion TEXT DEFAULT NULL,
                       existencia INTEGER NOT NULL,
                       fisico INTEGER NOT NULL,
                       temporal INTEGER NOT NULL,
                       fecha DATETIME NOT NULL,
                       maquina TEXT NOT NULL,
                       bloque TEXT NOT NULL
                    )"
                conector.Execute(sql)


                sql = "CREATE TABLE IF NOT EXISTS bodega (
                       codigo INTEGER DEFAULT NULL,
                       nombre TEXT DEFAULT NULL,
                       PRIMARY KEY (codigo)
                    )"
                conector.Execute(sql)


                sql = "CREATE TABLE IF NOT EXISTS etiquetas (
                       bodega INTEGER NOT NULL,
                       codart TEXT NOT NULL,
                       talla TEXT NOT NULL,
                       color TEXT NOT NULL,
                       linkcat TEXT NOT NULL,
                       descrip TEXT DEFAULT '',
                       exist1a INTEGER DEFAULT 0,
                       exist2a INTEGER DEFAULT 0,
                       exist3a INTEGER DEFAULT 0,
                       fisic1 INTEGER DEFAULT 0,
                       fisic2 INTEGER DEFAULT 0,
                       fisic3 INTEGER DEFAULT 0,
                       temporal INTEGER DEFAULT NULL,
                       fecha DATETIME NOT NULL,
                       estado TEXT NOT NULL,
                       maquina TEXT NOT NULL,
                       bloque INTEGER NOT NULL,
                       PRIMARY KEY (bloque, codart, talla, color, linkcat, bodega, maquina)
                    )"
                conector.Execute(sql)


                sql = "CREATE TABLE IF NOT EXISTS existencias (
                       bodega INTEGER NOT NULL,
                       plu TEXT NOT NULL,
                       existencia INTEGER DEFAULT NULL,
                       PRIMARY KEY (bodega, plu)
                    )"
                conector.Execute(sql)


                sql = "CREATE TABLE IF NOT EXISTS usuarios (
                       login TEXT NOT NULL,
                       nombre TEXT NOT NULL,
                       clave TEXT NOT NULL,
                       PRIMARY KEY (login)
                    )"
                conector.Execute(sql)

                crearBase = True
                    FrmInventario.cmbBodega.Text = "0"
                FrmInventario.btnActualizarBase.PerformClick()

                If conector.State = 1 Then conector.Close()
            End If
        End If
        Return crearBase
    End Function
    Sub errores(mensaje As String, procedimiento As String)
        MsgBox("Ocurrio un error en el procedimiento: " & procedimiento & vbCrLf & mensaje, MsgBoxStyle.Critical, "ERROR")
    End Sub
    Sub llenarBodega(control As ComboBox)
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
            rs.Open("SELECT DISTINCT codigo AS codigo FROM bodega", conn, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockReadOnly)
            If Not rs.EOF Then
                da.Fill(ds, rs, "Datos")
                control.DataSource = ds.Tables("Datos")
                control.DisplayMember = "codigo"
                control.ValueMember = "codigo"
            Else
                control.DataSource = Nothing
            End If
            If rs.State = 1 Then rs.Close()
        Catch ex As Exception
            errores("Llenar bodega", ex.Message)
        End Try
    End Sub

End Module
