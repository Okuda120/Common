Imports System.Windows.Forms
Imports System.Drawing
Imports System.Linq
Imports System.ComponentModel
Imports Common

''' <summary>
''' ComboBox拡張コントロール
''' </summary>
''' <remarks>階層表示（複数列表示）用に拡張
''' <para>作成情報：2012/06/06 t.fukuo
''' <p>改定情報：</p>
''' </para></remarks>
Public Class ComboBoxEx

    '共通ロジッククラスインスタンス作成
    Private commonLogic As New CommonLogic

    'コンボボックスの矢印ボタンのサイズ定数
    Private Const YAJIRUSHI_SIZE As Integer = 20

    'GETTER/SETTER
    Private ppIntStartCol As Integer        '表示開始列

    'MaxDropDwonItems
    Private ppMaxDrop As Integer = 12

    'IntegralHeight
    Private ppIntegralHeight As Boolean = True

    ''' <summary>
    ''' プロパティセット【表示開始列】
    ''' </summary>
    ''' <value></value>
    ''' <returns>ppIntStartCol</returns>
    ''' <remarks><para>作成情報：2012/06/06 t.fukuo
    ''' <p>改訂情報:</p>
    ''' </para></remarks>
    Public Property PropIntStartCol() As Integer
        Get
            Return ppIntStartCol
        End Get
        Set(ByVal value As Integer)
            ppIntStartCol = value
        End Set
    End Property
    ''' <summary>
    ''' プロパティセット【コンボボックス】
    ''' </summary>
    ''' <value></value>
    ''' <returns>cmbColumns</returns>
    ''' <remarks><para>作成情報：2012/06/06 t.fukuo
    ''' <p>改訂情報:</p>
    ''' </para></remarks>
    Public ReadOnly Property PropCmbColumns() As ComboBox
        Get
            Return cmbColumns
        End Get
    End Property
    ''' <summary>
    ''' プロパティセット【テキストボックス】
    ''' </summary>
    ''' <value></value>
    ''' <returns>txtDisplay</returns>
    ''' <remarks><para>作成情報：2012/06/06 t.fukuo
    ''' <p>改訂情報:</p>
    ''' </para></remarks>
    Public ReadOnly Property PropTxtDisplay() As TextBox
        Get
            Return txtDisplay
        End Get
    End Property
    ''' <summary>
    ''' プロパティセット【MaxDorp】
    ''' </summary>
    ''' <value></value>
    ''' <returns>ppMaxDrop</returns>
    ''' <remarks><para>作成情報：2012/10/09 r.hoshino
    ''' <p>改訂情報:</p>
    ''' </para></remarks>
    <Category("拡張")> _
    <Description("ComboBoxのMaxDropDownItems")> _
    <DefaultValue(12)> _
    Public Property PropMaxDrop() As Integer
        Get
            Return ppMaxDrop
        End Get
        Set(value As Integer)
            ppMaxDrop = value
        End Set
    End Property
    ''' <summary>
    ''' プロパティセット【PropIntegralHeight】
    ''' </summary>
    ''' <value></value>
    ''' <returns>ppIntegralHeight</returns>
    ''' <remarks><para>作成情報：2012/10/10 r.hoshino
    ''' <p>改訂情報:</p>
    ''' </para></remarks>
    <Category("拡張")> _
    <Description("ComboBoxのPropIntegralHeight")> _
    <DefaultValue(True)> _
    Public Property PropIntegralHeight() As Boolean
        Get
            Return ppIntegralHeight
        End Get
        Set(value As Boolean)
            ppIntegralHeight = value
        End Set
    End Property

    Public Event ComboBox_Load_Ex(ByVal sender As System.Object, ByVal e As System.EventArgs)
    Public Event ComboBox_DrawItem_Ex(ByVal sender As System.Object, ByVal e As System.EventArgs)
    Public Event ComboBox_SelectedIndexChanged_Ex(ByVal sender As System.Object, ByVal e As System.EventArgs)


    ''' <summary>
    ''' フォームロード時の処理
    ''' </summary>
    ''' <param name="sender">引数sender</param>
    ''' <param name="e">引数e</param>
    ''' <remarks>当ユーザコントロールの初期設定を行う
    ''' <para>作成情報：2010/06/06 t.fukuo
    ''' <p>改定情報：</p>
    ''' </para></remarks>
    Private Sub ComboBoxEx_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        RaiseEvent ComboBox_Load_Ex(sender, e)

        If cmbColumns IsNot Nothing Then
            'テキストボックスのサイズとコンボボックスの長さを合わせる
            txtDisplay.Width = cmbColumns.Width - YAJIRUSHI_SIZE
        End If

    End Sub

    ''' <summary>
    ''' データソース変更時の処理
    ''' </summary>
    ''' <param name="sender">引数sender</param>
    ''' <param name="e">引数e</param>
    ''' <remarks>ドロップダウンリストのサイズを計算し、設定する
    ''' <para>作成情報：
    ''' <p>改定情報：2010/06/06 t.fukuo</p>
    ''' </para></remarks>
    Private Sub cmbColumns_DataSourceChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbColumns.DataSourceChanged

        '変数宣言
        Dim cb As ComboBox = DirectCast(sender, ComboBox)
        Dim dt As DataTable
        Dim bLineX As Single

        'コンボボックスにデータソースが設定されている場合はデータソースをデータテーブルに変換
        If cmbColumns.DataSource IsNot Nothing Then
            dt = DirectCast(cmbColumns.DataSource, DataTable)
        Else
            'データソース未設定時は処理を抜ける
            Exit Sub
        End If

        '項目数分繰り返し、ドロップダウンリストのサイズを計算する
        For i As Integer = ppIntStartCol To dt.Columns.Count - 1

            '項目毎の最大バイト数を取得
            Dim intTargetCol As Integer = i
            Dim maxLenB = Aggregate row As DataRow In dt.Rows Where IsDBNull(row.Item(intTargetCol)) = False Select commonLogic.LenB(row.Item(intTargetCol)) Into Max()

            '次の描画位置計算
            Dim g As Graphics = cb.CreateGraphics()
            Dim sf As SizeF = g.MeasureString(New String("0"c, maxLenB), cb.Font)
            bLineX += sf.Width

            '最終項目の場合、ドロップダウンリストのサイズを設定
            If i = dt.Columns.Count - 1 Then
                cmbColumns.DropDownWidth = bLineX
            End If

            'メモリ解放
            g.Dispose()

        Next

        'IntegralHeightの設定
        cmbColumns.IntegralHeight = PropIntegralHeight

        'MaxDropItemsの設定
        cmbColumns.MaxDropDownItems = PropMaxDrop

    End Sub

    ''' <summary>
    ''' コンボボックスのリストアイテム出力時の処理
    ''' </summary>
    ''' <param name="sender">引数sender</param>
    ''' <param name="e">引数e</param>
    ''' <remarks>出力項目毎に境界線を表示する
    ''' <para>作成情報：
    ''' <p>改定情報：2010/06/06 t.fukuo</p>
    ''' </para></remarks>
    Private Sub cmbColumns_DrawItem(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DrawItemEventArgs) Handles cmbColumns.DrawItem

        '変数宣言
        Dim cb As ComboBox = DirectCast(sender, ComboBox)
        Dim dt As DataTable
        Dim bLineX As Single
        Dim p As Pen
        Dim b As Brush

        'コンボボックスにデータソースが設定されている場合はデータソースをデータテーブルに変換
        If cmbColumns.DataSource IsNot Nothing Then
            dt = DirectCast(cmbColumns.DataSource, DataTable)
        Else
            'データソース未設定時は処理を抜ける
            Exit Sub
        End If

        '******************************
        '*   階層表示処理開始
        '******************************

        '境界線描画用のインスタンス作成
        p = New Pen(Color.Gray)
        b = New SolidBrush(e.ForeColor)

        '背景の描画
        e.DrawBackground()

        If dt.Columns.Count > 0 And e.Index > -1 Then

            '項目数分繰り返し、値の表示と境界線の描画を行う
            For i As Integer = ppIntStartCol To dt.Columns.Count - 1

                '項目毎の最大バイト数を取得
                Dim intTargetCol As Integer = i
                Dim maxLenB = Aggregate row As DataRow In dt.Rows Where IsDBNull(row.Item(intTargetCol)) = False Select commonLogic.LenB(row.Item(intTargetCol)) Into Max()

                '値の表示
                e.Graphics.DrawString(Convert.ToString(dt.Rows(e.Index)(i)), e.Font, b, e.Bounds.X + bLineX, e.Bounds.Y)

                '次の描画位置計算
                Dim g As Graphics = cb.CreateGraphics()
                Dim sf As SizeF = g.MeasureString(New String("0"c, maxLenB), cb.Font)
                bLineX += sf.Width

                '最終項目でない場合、境界線描画
                If i < dt.Columns.Count - 1 Then
                    e.Graphics.DrawLine(p, bLineX, e.Bounds.Top, bLineX, e.Bounds.Bottom)
                End If

                'メモリ解放
                g.Dispose()

            Next

            'フォーカスの四角形を描画
            If CBool(e.State And DrawItemState.Selected) Then
                ControlPaint.DrawFocusRectangle(e.Graphics, e.Bounds)
            End If

        End If

    End Sub

    ''' <summary>
    ''' コンボボックスの選択項目変更時の処理
    ''' </summary>
    ''' <param name="sender">引数sender</param>
    ''' <param name="e">引数e</param>
    ''' <remarks>リストで選択された項目を表示用テキストボックスにセットする
    ''' <para>作成情報：
    ''' <p>改定情報：2010/06/06 t.fukuo</p>
    ''' </para></remarks>
    Private Sub cmbColumns_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbColumns.SelectedIndexChanged

        'リストで選択された値をテキストボックスにセットする
        Dim dtvSelected As DataRowView = cmbColumns.SelectedItem
        If cmbColumns.DisplayMember <> "" Then
            txtDisplay.Text = dtvSelected.Item(cmbColumns.DisplayMember)
            txtDisplay.SelectAll()
        End If

        RaiseEvent ComboBox_SelectedIndexChanged_Ex(sender, e)

    End Sub
End Class
