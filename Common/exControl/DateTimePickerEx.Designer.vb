<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class DateTimePickerEx
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
        Me.txtDate = New System.Windows.Forms.TextBox()
        Me.dtDate = New System.Windows.Forms.DateTimePicker()
        Me.SuspendLayout()
        '
        'txtDate
        '
        Me.txtDate.Font = New System.Drawing.Font("MS UI Gothic", 9.0!)
        Me.txtDate.ImeMode = System.Windows.Forms.ImeMode.Alpha
        Me.txtDate.Location = New System.Drawing.Point(1, 0)
        Me.txtDate.MaxLength = 10
        Me.txtDate.Multiline = True
        Me.txtDate.Name = "txtDate"
        Me.txtDate.Size = New System.Drawing.Size(96, 19)
        Me.txtDate.TabIndex = 0
        Me.txtDate.Text = "1999/01/01"
        '
        'dtDate
        '
        Me.dtDate.CalendarFont = New System.Drawing.Font("MS UI Gothic", 10.0!)
        Me.dtDate.CustomFormat = " "
        Me.dtDate.Font = New System.Drawing.Font("MS UI Gothic", 9.0!)
        Me.dtDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtDate.Location = New System.Drawing.Point(1, 0)
        Me.dtDate.Name = "dtDate"
        Me.dtDate.Size = New System.Drawing.Size(110, 19)
        Me.dtDate.TabIndex = 1
        Me.dtDate.TabStop = False
        Me.dtDate.Value = New Date(2008, 4, 16, 0, 0, 0, 0)
        '
        'DateTimePickerEx
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.txtDate)
        Me.Controls.Add(Me.dtDate)
        Me.Name = "DateTimePickerEx"
        Me.Size = New System.Drawing.Size(111, 20)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Public WithEvents txtDate As System.Windows.Forms.TextBox
    Friend WithEvents dtDate As System.Windows.Forms.DateTimePicker

End Class
