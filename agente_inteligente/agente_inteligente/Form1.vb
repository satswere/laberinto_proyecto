Public Class Form1
    Dim columna, fila As Byte
    Dim cantidad_columnas, cantidad_filas As Integer
    Dim valor As Integer
    Dim opcion_moviemiento As Integer

    Dim arriba_usado As Integer
    Dim abajo_usado As Integer
    Dim izquierda_usado As Integer
    Dim derecha_usado As Integer


    Dim inicial_columna As Integer
    Dim inicial_fila As Integer


    Dim pila_columna As New Stack()
    Dim pila_fila As New Stack()
    Sub delay(ByVal dblSecs As Double)
        'codigo empleado para simular un segundo / se llama escribiendo delay()
        Const Onesec As Double = 1.0# / (1440.0# * 60.0#)
        Dim dblwaitil As Date
        Now.AddSeconds(Onesec)
        dblwaitil = Now.AddSeconds(Onesec).AddSeconds(dblSecs)
        Do Until Now > dblwaitil
            Application.DoEvents()
        Loop

    End Sub

    Private Sub DgvMatriz_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs) Handles DgvMatriz.CellFormatting
        'codigo para condicionar el color de las celdas de la tabla

        If (Convert.ToInt32(e.Value) = 0) Then 'pared
            e.CellStyle.ForeColor = Color.Black
            e.CellStyle.BackColor = Color.Black
        End If

        If (Convert.ToInt32(e.Value) = 1) Then 'camino
            e.CellStyle.ForeColor = Color.White
            e.CellStyle.BackColor = Color.White
        End If

        If (Convert.ToInt32(e.Value) = 2) Then 'recorrido del robot
            e.CellStyle.ForeColor = Color.Blue
            e.CellStyle.BackColor = Color.Blue
        End If
        If (Convert.ToInt32(e.Value) = 3) Then 'meta
            e.CellStyle.ForeColor = Color.Red
            e.CellStyle.BackColor = Color.Red
        End If
        If (Convert.ToInt32(e.Value) = 4) Then 'meta
            e.CellStyle.ForeColor = Color.Aqua
            e.CellStyle.BackColor = Color.Aqua
        End If

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        'limpieza de datos
        pila_columna.Clear()
        pila_fila.Clear()
        valor = 0

        'Creacion de la tabla 
        columna = 13
        fila = 13

        cantidad_columnas = columna 'total creado
        cantidad_filas = fila

        DgvMatriz.ColumnCount = cantidad_columnas
        DgvMatriz.RowCount = cantidad_filas
        'establece tamaño de celdas

        'iniciacion de la matriz
        For x = 0 To cantidad_columnas - 1
            DgvMatriz.Columns(x).HeaderText = x + 1
            'establece tamaño de celdas x2
            DgvMatriz.Columns(x).Width = 20
        Next

        'llenado de la matriz
        For x = 0 To cantidad_columnas - 1
            For y = 0 To fila - 1
                DgvMatriz.Rows(y).Cells(x).Value = 0
            Next
        Next
        'esto hace que se cambie el valor de las columnas
        ' DgvMatriz.Rows(yy).Cells(xx - 2).Value = 1

        'Pausa de un segundo y llamada del laberinto
        delay(1)
        campo_inicial()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        pila_columna.Clear()
        pila_fila.Clear()

        pila_columna.Push(inicial_columna)
        pila_fila.Push(inicial_fila)

        movientos_agente(inicial_columna, inicial_fila)
    End Sub

    Private Sub movientos_agente(columna_recibda As Integer, fila_recibida As Integer)
        If ((pila_columna.Count > 0) And (valor <> 3)) Then
            delay(0.1)
            columna = columna_recibda
            fila = fila_recibida

            If (arriba_usado = 1) And (abajo_usado = 1) And (izquierda_usado = 1) And (derecha_usado = 1) Then



                columna = pila_columna.Pop
                fila = pila_fila.Pop
                DgvMatriz.Rows(columna_recibda).Cells(fila_recibida).Value = 4

                limpiar_direcciones_usadas()
            End If
            Randomize()

            Dim nueva_columna As Integer
            Dim nueva_fila As Integer

            opcion_moviemiento = CInt(Int((4 * Rnd())))

            Select Case opcion_moviemiento
                Case 0 'arriba'
                    ' MsgBox("arriba")
                    nueva_columna = (columna_recibda - 1)
                    nueva_fila = (fila_recibida)
                    arriba_usado = 1
                    comprobar_si_es_muro_agente(nueva_columna, nueva_fila)

                Case 1 'abajo'
                    'MsgBox("abajo")
                    nueva_columna = (columna_recibda + 1)
                    nueva_fila = (fila_recibida)
                    abajo_usado = 1
                    comprobar_si_es_muro_agente(nueva_columna, nueva_fila)

                Case 2 'izquierda
                    '   MsgBox("izquierda")
                    nueva_columna = (columna_recibda)
                    nueva_fila = (fila_recibida - 1)
                    izquierda_usado = 1
                    comprobar_si_es_muro_agente(nueva_columna, nueva_fila)

                Case 3 'derecha'
                    '  MsgBox("derecha")
                    nueva_columna = (columna_recibda)
                    nueva_fila = (fila_recibida + 1)
                    derecha_usado = 1
                    comprobar_si_es_muro_agente(nueva_columna, nueva_fila)
            End Select

        Else
            MsgBox("se ha terminado de encontrar el camino")
        End If

        Return
    End Sub

    Private Sub comprobar_limite_del_array_agente(posible_columna As Integer, posible_fila As Integer)
        If ((posible_columna < cantidad_columnas) And (posible_columna >= 0)) And ((posible_fila < cantidad_filas) And (posible_fila >= 0)) Then
            comprobar_si_es_muro_agente(posible_columna, posible_fila)
        Else
            movientos_agente(columna, fila)
        End If
    End Sub
    Private Sub comprobar_si_es_muro_agente(columna_moviemiento As Integer, fila_movimiento As Integer)
        Dim posible_pintado As Integer

        posible_pintado = DgvMatriz.Rows(columna_moviemiento).Cells(fila_movimiento).Value.ToString()
        If (posible_pintado = 1 Or posible_pintado = 3) Then


            Select Case opcion_moviemiento
                Case 0 'arriba'
                    valor = DgvMatriz.Rows(columna_moviemiento).Cells(fila_movimiento).Value

                    DgvMatriz.Rows(columna_moviemiento).Cells(fila_movimiento).Value = 2

                    limpiar_direcciones_usadas()

                    pila_columna.Push(columna_moviemiento)
                    pila_fila.Push(fila_movimiento)

                    movientos_agente(columna_moviemiento, fila_movimiento)

                Case 1 'abajo'
                    valor = DgvMatriz.Rows(columna_moviemiento).Cells(fila_movimiento).Value

                    DgvMatriz.Rows(columna_moviemiento).Cells(fila_movimiento).Value = 2
                    limpiar_direcciones_usadas()

                    pila_columna.Push(columna_moviemiento)
                    pila_fila.Push(fila_movimiento)

                    movientos_agente(columna_moviemiento, fila_movimiento)

                Case 2 'izquierda'
                    valor = DgvMatriz.Rows(columna_moviemiento).Cells(fila_movimiento).Value

                    DgvMatriz.Rows(columna_moviemiento).Cells(fila_movimiento).Value = 2
                    limpiar_direcciones_usadas()

                    pila_columna.Push(columna_moviemiento)
                    pila_fila.Push(fila_movimiento)

                    movientos_agente(columna_moviemiento, fila_movimiento)

                Case 3 'derecha'
                    valor = DgvMatriz.Rows(columna_moviemiento).Cells(fila_movimiento).Value

                    DgvMatriz.Rows(columna_moviemiento).Cells(fila_movimiento).Value = 2
                    limpiar_direcciones_usadas()


                    pila_columna.Push(columna_moviemiento)
                    pila_fila.Push(fila_movimiento)
                    movientos_agente(columna_moviemiento, fila_movimiento)

            End Select

        Else

            movientos_agente(columna, fila)
        End If
    End Sub
    Private Sub campo_inicial()

        Randomize() 'iniciar la semilla random

        Dim columna As Integer = CInt(Int((cantidad_columnas * Rnd())))
        Dim fila As Integer = CInt(Int((cantidad_filas * Rnd())))
        Dim contador As Integer
        contador = 0

        Dim Res = columna Mod 2
        Dim Res2 = fila Mod 2
        '  Dim valor
        If Res <> 0 And Res2 <> 0 Then
            DgvMatriz.Rows(columna).Cells(fila).Value = 1

            ' valor = DgvMatriz.Rows(columna).Cells(fila).Value.ToString()
            pila_columna.Push(columna)
            pila_fila.Push(fila)

            inicial_columna = columna
            inicial_fila = fila
            movimiento(contador, columna, fila)
            ' pregunta(valor As Integer, xx As Integer, yy As Integer)
        Else
            campo_inicial()
        End If
    End Sub

    Private Sub movimiento(cont As Integer, columna_recibda As Integer, fila_recibida As Integer)
        If (pila_columna.Count > 0) Then

            delay(0.1)
            columna = columna_recibda
            fila = fila_recibida

            If (arriba_usado = 0) And (abajo_usado = 0) And (izquierda_usado = 0) And (derecha_usado = 0) Then
                '  MsgBox("nuevo elemento")
                ' MsgBox(columna)
                'MsgBox(fila)

            End If

            If (arriba_usado = 1) And (abajo_usado = 1) And (izquierda_usado = 1) And (derecha_usado = 1) Then
                'MsgBox("sin opciones")
                columna = pila_columna.Pop
                fila = pila_fila.Pop
                'MsgBox(columna)
                'MsgBox(fila)
                limpiar_direcciones_usadas()
            End If
            Randomize()

            Dim nueva_columna As Integer
            Dim nueva_fila As Integer

            opcion_moviemiento = CInt(Int((4 * Rnd())))

            Select Case opcion_moviemiento
                Case 0 'arriba'
                    nueva_columna = (columna_recibda - 2)
                    nueva_fila = (fila_recibida)
                    arriba_usado = 1
                    comprobar_limite_del_array(nueva_columna, nueva_fila)


                Case 1 'abajo'
                    nueva_columna = (columna_recibda + 2)
                    nueva_fila = (fila_recibida)
                    abajo_usado = 1
                    comprobar_limite_del_array(nueva_columna, nueva_fila)



                Case 2 'izquierda'
                    nueva_columna = (columna_recibda)
                    nueva_fila = (fila_recibida - 2)
                    izquierda_usado = 1
                    comprobar_limite_del_array(nueva_columna, nueva_fila)



                Case 3 'derecha'
                    nueva_columna = (columna_recibda)
                    nueva_fila = (fila_recibida + 2)
                    derecha_usado = 1

                    comprobar_limite_del_array(nueva_columna, nueva_fila)


            End Select

        Else
            DgvMatriz.Rows(inicial_columna).Cells(inicial_fila).Value = 2
            DgvMatriz.Rows(cantidad_columnas - 1).Cells(cantidad_filas - 2).Value = 3

            MsgBox("se ha terminado de crear")
        End If
    End Sub

    Private Sub comprobar_limite_del_array(posible_columna As Integer, posible_fila As Integer)
        Dim cont As Integer = 0

        If ((posible_columna < cantidad_columnas) And (posible_columna >= 0)) And ((posible_fila < cantidad_filas) And (posible_fila >= 0)) Then
            comprobar_si_es_muro(cont, posible_columna, posible_fila)
        Else
            movimiento(cont, columna, fila)
        End If
    End Sub

    Private Sub DgvMatriz_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles DgvMatriz.CellContentClick

    End Sub

    Private Sub comprobar_si_es_muro(valor As Integer, columna_moviemiento As Integer, fila_movimiento As Integer)
        Dim cont As Integer
        Dim posible_pintado As Integer
        cont = 0
        posible_pintado = DgvMatriz.Rows(columna_moviemiento).Cells(fila_movimiento).Value.ToString()
        If (posible_pintado = 0) Then
            cont = 0

            '   columna = columna_moviemiento
            '  fila = fila_movimiento


            Select Case opcion_moviemiento
                Case 0 'arriba'
                    DgvMatriz.Rows(columna - 2).Cells(fila).Value = 1
                    DgvMatriz.Rows(columna - 1).Cells(fila).Value = 1
                    limpiar_direcciones_usadas()

                    pila_columna.Push(columna_moviemiento)
                    pila_fila.Push(fila_movimiento)

                    movimiento(cont, columna_moviemiento, fila_movimiento)

                Case 1 'abajo'
                    DgvMatriz.Rows(columna + 2).Cells(fila).Value = 1
                    DgvMatriz.Rows(columna + 1).Cells(fila).Value = 1
                    limpiar_direcciones_usadas()

                    pila_columna.Push(columna_moviemiento)
                    pila_fila.Push(fila_movimiento)

                    movimiento(cont, columna_moviemiento, fila_movimiento)

                Case 2 'izquierda'
                    DgvMatriz.Rows(columna).Cells(fila - 2).Value = 1
                    DgvMatriz.Rows(columna).Cells(fila - 1).Value = 1
                    limpiar_direcciones_usadas()

                    pila_columna.Push(columna_moviemiento)
                    pila_fila.Push(fila_movimiento)

                    movimiento(cont, columna_moviemiento, fila_movimiento)

                Case 3 'derecha'
                    DgvMatriz.Rows(columna).Cells(fila + 2).Value = 1
                    DgvMatriz.Rows(columna).Cells(fila + 1).Value = 1
                    limpiar_direcciones_usadas()


                    pila_columna.Push(columna_moviemiento)
                    pila_fila.Push(fila_movimiento)
                    movimiento(cont, columna_moviemiento, fila_movimiento)

            End Select

        Else
            cont = cont + 1
            movimiento(cont, columna, fila)
        End If
    End Sub
    Private Sub limpiar_direcciones_usadas()
        arriba_usado = 0
        abajo_usado = 0
        izquierda_usado = 0
        derecha_usado = 0
    End Sub

End Class
