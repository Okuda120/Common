Imports System.Windows.Forms
Imports System.Text
Imports Common

''' <summary>
''' TextBox拡張コントロール
''' </summary>
''' <remarks>時分入出力用に拡張
''' <para>作成情報：2012/06/06 t.fukuo
''' <p>改定情報：</p>
''' </para></remarks>
Public Class TextBoxEx_IoTime

    '共通ロジッククラスインスタンス作成
    Private commonLogic As New CommonLogic

    'テキストボックスの値をパブリック変数で保持
    Public puTextBox As String = ""

    ''' <summary>
    ''' プロパティセット【テキストボックス】
    ''' </summary>
    ''' <value></value>
    ''' <returns>txtTime</returns>
    ''' <remarks><para>作成情報：2012/06/06 t.fukuo
    ''' <p>改訂情報:</p>
    ''' </para></remarks>
    Public ReadOnly Property PropTxtTime() As TextBox
        Get
            Return txtTime
        End Get
    End Property

    ''' <summary>
    ''' テキストボックスのロストフォーカス時の処理（参照元クラス追加用）
    ''' </summary>
    ''' <param name="sender">引数sender</param>
    ''' <param name="e">引数e</param>
    ''' <remarks>テキストボックスのロストフォーカスイベントを参照元クラスで追加可能にする。
    ''' <para>作成情報：2012/06/06 t.fukuo
    ''' <p>改定情報：</p>
    ''' </para></remarks>
    Public Event txtTime_LostFocus_ex(ByVal sender As Object, ByVal e As System.EventArgs)


    ''' <summary>
    ''' テキストボックスのフォーカス時の処理
    ''' </summary>
    ''' <param name="sender">引数sender</param>
    ''' <param name="e">引数e</param>
    ''' <remarks>テキストボックスの入力値からコロンを除去し、値を全選択する
    ''' <para>作成情報：
    ''' <p>改定情報：2012/06/08 t.fukuo</p>
    ''' </para></remarks>
    Private Sub txtTime_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtTime.Enter

        'テキストボックスからコロンを除去
        txtTime.Text = txtTime.Text.Replace(":", "")

        '入力値を全選択
        txtTime.SelectAll()

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
    Private Sub txtTime_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtTime.KeyPress

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
    ''' <para>作成情報：
    ''' <p>改定情報：2012/06/08 t.fukuo
    ''' :2013/04/23 r.hoshino スペース入力エラーとする</p>
    ''' </para></remarks>
    Private Sub txtDate_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtTime.LostFocus

        Const ERR_MSG As String = "時刻形式(HHMM)で入力して下さい"

        '入力文字列を取得
        Dim strTextBox As String = Trim(txtTime.Text)

        'Edit 2013/04/23 start
        ''未入力の場合は処理を抜ける
        'If strTextBox = "" Then
        '    Exit Sub
        'End If
        If txtTime.Text = "" Then
            Exit Sub
        End If
        '入力がスペースのみの場合エラーとする
        If strTextBox = "" Then
            MsgBox(ERR_MSG, MsgBoxStyle.Critical, "エラー")
            txtTime.Text = puTextBox
            txtTime.Focus()
            txtTime.SelectAll()
            Exit Sub
        End If
        'edit 2013/04/23 end

        '半角変換チェックを行い、変換できない場合はエラーメッセージを表示
        If commonLogic.SingleChar(strTextBox) = False Then
            MsgBox(ERR_MSG, MsgBoxStyle.Critical, "エラー")
            txtTime.Text = puTextBox
            txtTime.Focus()
            txtTime.SelectAll()
            Exit Sub
        End If

        ':を除去
        strTextBox = strTextBox.Replace(":", "")

        '時分チェックを行い、時分出ない場合はエラーメッセージを表示
        If IsHHMI(strTextBox) = False Then
            MsgBox(ERR_MSG, MsgBoxStyle.Critical, "エラー")
            txtTime.Text = puTextBox
            txtTime.Focus()
            txtTime.SelectAll()
            Exit Sub
        Else
            '時分の場合は、HH:MI形式に変換し、テキストボックスにセット
            txtTime.Text = strTextBox.Substring(0, 2) & ":" & strTextBox.Substring(2, 2)
        End If

        '参照元クラスのイベントを実行
        RaiseEvent txtTime_LostFocus_ex(sender, e)

        'テキストボックスの値をパブリック変数に保管
        puTextBox = txtTime.Text

    End Sub

    ''' <summary>
    ''' 時分チェック
    ''' </summary>
    ''' <param name="s">[IN]チェック対象文字列</param>
    ''' <returns>[OUT]時分形式の場合：True、時分形式でない場合：False</returns>
    ''' <remarks>文字列が時分形式（HH:MI、HHMI）がチェックする
    ''' <para>作成情報：
    ''' <p>改定情報：2012/06/06 t.fukuo</p>
    ''' </para></remarks>
    Private Function IsHHMI(ByVal s As String) As Boolean

        Dim chkText As String = s
        Dim chkStr As String

        '桁数チェック
        If chkText.Length <> 4 Then
            '4桁でない場合はFalse
            Return False
        End If

        '数値チェック
        If IsNumeric(chkText) = False Then
            '数値でない場合はFalse
            Return False
        End If

        '上2桁を取得し、00～23の間かチェック
        chkStr = chkText.Substring(0, 2)
        If chkStr < "00" Or chkStr > "23" Then
            Return False
        End If

        '下2桁を取得し、00～59の間かチェック
        chkStr = s.Substring(2, 2)
        If chkStr < "00" Or chkStr > "59" Then
            Return False
        End If

        '正常処理
        Return True

    End Function





End Class
