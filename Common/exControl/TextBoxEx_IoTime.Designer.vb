<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class TextBoxEx_IoTime
    Inherits System.Windows.Forms.UserControl

    'UserControl はコンポーネント一覧をクリーンアップするために dispose をオーバーライドします。
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

    'Windows フォーム デザイナで必要です。
    Private components As System.ComponentModel.IContainer

    'メモ: 以下のプロシージャは Windows フォーム デザイナで必要です。
    'Windows フォーム デザイナを使用して変更できます。  
    'コード エディタを使って変更しないでください。
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.txtTime = New System.Windows.Forms.TextBox
        Me.SuspendLayout()
        '
        'txtTime
        '
        Me.txtTime.Cursor = System.Windows.Forms.Cursors.Default
        Me.txtTime.ImeMode = System.Windows.Forms.ImeMode.Alpha
        Me.txtTime.Location = New System.Drawing.Point(0, 0)
        Me.txtTime.MaxLength = 5
        Me.txtTime.Name = "txtTime"
        Me.txtTime.Size = New System.Drawing.Size(50, 19)
        Me.txtTime.TabIndex = 0
        '
        'TextBoxEx_IoTime
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.txtTime)
        Me.Name = "TextBoxEx_IoTime"
        Me.Size = New System.Drawing.Size(51, 21)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents txtTime As System.Windows.Forms.TextBox

End Class
