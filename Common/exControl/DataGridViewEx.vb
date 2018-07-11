Imports System
Imports System.Windows.Forms

''' <summary>
''' DataGridView拡張コントロール
''' </summary>
''' <remarks>DataGridView内でEnterキーを押下時に、Tabキーを押下時の動きと同様にする
''' <para>作成情報：2008/04/11 kawate
''' <p>改定情報：</p>
''' </para></remarks>
Public Class DataGridViewEx
    Inherits DataGridView

    ''' <summary>
    ''' Enterキーが押された時の処理
    ''' </summary>
    ''' <param name="keyData">引数keyData</param>
    ''' <returns>boolean エラーコード
    ''' true Tabキーを押時の動き
    ''' false Dialogキーを押時の動き</returns>
    ''' <remarks>'Enterキーが押された時は、Tabキーが押されたようにする</remarks>
    Protected Overrides Function ProcessDialogKey( _
            ByVal keyData As Keys) As Boolean
        'Enterキーが押された時は、Tabキーが押されたようにする
        If (keyData And Keys.KeyCode) = Keys.Enter Then
            Return Me.ProcessTabKey(keyData)
        End If
        Return MyBase.ProcessDialogKey(keyData)
    End Function


    ''' <summary>
    ''' Enterキーが押された時の処理
    ''' </summary>
    ''' <param name="e">引数e</param>
    ''' <returns>boolean エラーコード
    ''' true Tabキーを押時の動き
    ''' false DataGridViewキーを押時の動き</returns>
    ''' <remarks>'Enterキーが押された時は、Tabキーが押されたようにする</remarks>
    Protected Overrides Function ProcessDataGridViewKey( _
            ByVal e As KeyEventArgs) As Boolean
        'Enterキーが押された時は、Tabキーが押されたようにする
        If e.KeyCode = Keys.Enter Then
            Return Me.ProcessTabKey(e.KeyCode)
        End If
        Return MyBase.ProcessDataGridViewKey(e)
    End Function
End Class
