<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ComboBoxEx
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
        Me.cmbColumns = New System.Windows.Forms.ComboBox()
        Me.txtDisplay = New System.Windows.Forms.TextBox()
        Me.SuspendLayout()
        '
        'cmbColumns
        '
        Me.cmbColumns.BackColor = System.Drawing.SystemColors.Window
        Me.cmbColumns.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.cmbColumns.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbColumns.FormattingEnabled = True
        Me.cmbColumns.IntegralHeight = False
        Me.cmbColumns.Location = New System.Drawing.Point(0, 0)
        Me.cmbColumns.MaxDropDownItems = 1
        Me.cmbColumns.Name = "cmbColumns"
        Me.cmbColumns.Size = New System.Drawing.Size(264, 20)
        Me.cmbColumns.TabIndex = 0
        '
        'txtDisplay
        '
        Me.txtDisplay.BackColor = System.Drawing.SystemColors.Window
        Me.txtDisplay.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtDisplay.Font = New System.Drawing.Font("MS UI Gothic", 9.0!)
        Me.txtDisplay.ForeColor = System.Drawing.Color.Black
        Me.txtDisplay.Location = New System.Drawing.Point(3, 3)
        Me.txtDisplay.Multiline = True
        Me.txtDisplay.Name = "txtDisplay"
        Me.txtDisplay.ReadOnly = True
        Me.txtDisplay.Size = New System.Drawing.Size(245, 14)
        Me.txtDisplay.TabIndex = 0
        Me.txtDisplay.TabStop = False
        '
        'ComboBoxEx
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.txtDisplay)
        Me.Controls.Add(Me.cmbColumns)
        Me.Name = "ComboBoxEx"
        Me.Size = New System.Drawing.Size(266, 20)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Public WithEvents cmbColumns As System.Windows.Forms.ComboBox
    Public WithEvents txtDisplay As System.Windows.Forms.TextBox

End Class
