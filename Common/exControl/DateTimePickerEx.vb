Imports System.Text

''' <summary>
''' DateTimePicker拡張コントロール
''' </summary>
''' <remarks>DateTimePickerでNULLを入力できるようにした
''' <para>作成情報：2008/04/15 kawate
''' <p>改定情報：</p>
''' </para></remarks>
Public Class DateTimePickerEx

    'テキストボックスの値をパブリック変数で保持
    Public puTextBox As String = ""

    'コンボボックスの矢印ボタンのサイズ定数
    Private Const YAJIRUSHI_SIZE As Integer = 16

    ''' <summary>
    ''' テキストボックスのロストフォーカス時の処理（参照元クラス追加用）
    ''' </summary>
    ''' <param name="sender">引数sender</param>
    ''' <param name="e">引数e</param>
    ''' <remarks>テキストボックスのロストフォーカスイベントを参照元クラスで追加可能にする。
    ''' <para>作成情報：2010/5/20 s.ikeda
    ''' <p>改定情報：</p>
    ''' </para></remarks>
    Public Event txtDate_LostFocus_ex(ByVal sender As Object, ByVal e As System.EventArgs)

    ''' <summary>
    ''' カレンダー選択後の処理（参照元クラス追加用）
    ''' </summary>
    ''' <param name="sender">引数sender</param>
    ''' <param name="e">引数e</param>
    ''' <remarks>カレンダーから日付選択後の処理を参照元クラスで追加可能にする。
    ''' <para>作成情報：2010/5/20 s.ikeda
    ''' <p>改定情報：</p>
    ''' </para></remarks>
    Public Event txtDate_TextChanged_ex(ByVal sender As Object, ByVal e As System.EventArgs)


    ''' <summary>
    ''' テキストボックスフォーカス時の処理
    ''' </summary>
    ''' <param name="sender">引数sender</param>
    ''' <param name="e">引数e</param>
    ''' <remarks>テキストボックスの入力値からスラッシュを除去し、値を全選択する
    ''' <para>作成情報：2012/06/11 t.fukuo
    ''' <p>改定情報：</p>
    ''' </para></remarks>
    Private Sub txtDate_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtDate.Enter

        'テキストボックスからスラッシュを除去
        txtDate.Text = txtDate.Text.Replace("/", "")

    End Sub



    ''' <summary>
    ''' テキストボックスのキー押下時の処理
    ''' </summary>
    ''' <param name="sender">引数sender</param>
    ''' <param name="e">引数e</param>
    ''' <remarks>MultiLineのテキストボックスに改行コードを入力できないようにする
    ''' <para>作成情報：2012/06/07 t.fukuo
    ''' <p>改定情報：2013/04/23 r.hoshino 全角・半角スペースを入力不可とする</p>
    ''' </para></remarks>
    Private Sub txtDate_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtDate.KeyPress

        '改行コードの場合は入力不可
        'If e.KeyChar = vbCr  Then
        If e.KeyChar = vbCr Or e.KeyChar = " " Or e.KeyChar = "　" Then
            e.Handled = True
        End If

    End Sub


    ''' <summary>
    ''' テキストボックスのロストフォーカス時の処理
    ''' </summary>
    ''' <param name="sender">引数sender</param>
    ''' <param name="e">引数e</param>
    ''' <remarks>テキストボックス手動入力後に日付チェックを行う
    ''' <para>作成情報：2010/5/20 s.ikeda
    ''' <p>改定情報：2012/06/07 t.fukuo スラッシュ保管するよう修正
    ''' 　　　　　　 2012/07/05 t.fukuo エラーメッセージおよび日付チェック不具合を修正</p>
    ''' </para></remarks>
    Private Sub txtDate_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtDate.LostFocus

        Dim strTextBox As String = Trim(txtDate.Text)

        '日付フラグ 2012/06/07 t.fukuo ADD
        Dim blnIsDate As Boolean = False

        '未入力でない場合、チェックする 2012/06/07 t.fukuo ADD
        If strTextBox <> "" Then
            'YYYY/MM/DDまたはYYYYMMDD書式チェックを行う
            If RegularExpressions.Regex.IsMatch(strTextBox, "(19|20)[0-9][0-9]/(0[1-9]|1[0-2])/(0[1-9]|[1-2][0-9]|3[0-1])") = True Then
                '日付型チェック
                If IsDate(strTextBox) = True Then
                    '日付フラグON
                    blnIsDate = True
                End If
            ElseIf RegularExpressions.Regex.IsMatch(strTextBox, "(19|20)[0-9][0-9](0[1-9]|1[0-2])(0[1-9]|[1-2][0-9]|3[0-1])") = True Then
                '入力値にスラッシュを補完
                strTextBox = strTextBox.Insert(4, "/").Insert(7, "/")
                '日付型チェック
                If IsDate(strTextBox) = True Then
                    '日付フラグON
                    blnIsDate = True
                    'テキストボックスにセット
                    txtDate.Text = strTextBox
                End If          
            End If
            '日付でない場合、エラーメッセージを表示し処理終了
            If blnIsDate = False Then
                '2012/07/05 t.fukuo MOD：START
                'MsgBox("日付形式(YYYY/MM/DD)で入力して下さい")
                MsgBox("日付形式(YYYYMMDD)で入力して下さい", MsgBoxStyle.Critical, "エラー")
                '2012/07/05 t.fukuo MOD：END
                txtDate.Text = puTextBox
                txtDate.Focus()
                txtDate.SelectAll()
                Exit Sub
            End If
        Else
            'スペースのみの場合、エラーメッセージを表示し処理終了
            If txtDate.Text.Length <> 0 Then
                MsgBox("日付形式(YYYYMMDD)で入力して下さい", MsgBoxStyle.Critical, "エラー")
                '2012/07/05 t.fukuo MOD：END
                txtDate.Text = puTextBox
                txtDate.Focus()
                txtDate.SelectAll()
                Exit Sub
            End If
        End If

        '2012/06/07 t.fukuo DEL：START
        'If (IsDate(strTextBox) = False Or RegularExpressions.Regex.IsMatch(strTextBox, "(19|20)[0-9][0-9]/(0[1-9]|1[0-2])/(0[1-9]|[1-2][0-9]|3[0-1])") = False) And strTextBox <> "" Then
        '    MsgBox("日付形式(YYYY/MM/DD)で入力して下さい")
        '    txtDate.Text = puTextBox
        '    txtDate.Focus()
        '    txtDate.SelectAll()
        '    Exit Sub
        'End If

        '参照元クラスのイベントを実行 2010/5/20 s.ikeda ADD
        RaiseEvent txtDate_LostFocus_ex(sender, e)

        'テキストボックスの値をパブリック変数に保管 2010/5/20 s.ikeda ADD
        puTextBox = txtDate.Text

    End Sub


    ''' <summary>
    ''' カレンダーから日付選択後の処理
    ''' </summary>
    ''' <param name="sender">引数sender</param>
    ''' <param name="e">引数e</param>
    ''' <remarks>カレンダーから日付選択後にテキストボックスに値をコピーする    
    ''' <para>作成情報：
    ''' <p>改定情報：2010/5/20 s.ikeda</p>
    ''' </para></remarks>
    Private Sub dtDate_CloseUp(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtDate.CloseUp
        'カレンダーの値をテキストボックスにコピー
        txtDate.Text = dtDate.Value.ToShortDateString

        '参照元クラスのイベントを実行 2010/5/20 s.ikeda ADD
        RaiseEvent txtDate_TextChanged_ex(sender, e)

        'テキストボックスの値をパブリック変数に保管
        '2010/5/20 s.ikeda 元の値をカレンダーからテキストボックスに変更
        puTextBox = txtDate.Text

    End Sub


    ''' <summary>
    ''' カレンダーへのフォーカス移動時の処理
    ''' </summary>
    ''' <param name="sender">引数sender</param>
    ''' <param name="e">引数e</param>
    ''' <remarks>カレンダーへのフォーカス移動時に、テキストボックスからカレンダーに値をコピーする
    ''' <para>作成情報：
    ''' <p>改定情報：</p>
    ''' </para></remarks>
    Private Sub dtDate_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtDate.GotFocus
        Dim strTextBox As String = Trim(txtDate.Text)
        '空白だとエラーになるので現在日付を入力する
        If strTextBox = "" Or IsDate(strTextBox) = False Then
            dtDate.Value = Now
        Else
            dtDate.Value = CDate(strTextBox)
        End If
    End Sub

End Class
