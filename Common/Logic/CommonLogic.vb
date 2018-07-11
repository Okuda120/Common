Imports System.Runtime.InteropServices.Marshal
Imports System.Reflection
Imports System.Text
Imports System.IO
Imports System.Windows.Forms
Imports System.Net
Imports System.Net.Mail
Imports System.Diagnostics
Imports System.Configuration
Imports System.Security
Imports Npgsql
Imports Common

''' <summary>CommonLogic
''' </summary>
''' <remarks>解説を書く</remarks>

Public Class CommonLogic

    '共通変数
    Private vec As String = "0123456789ABCDEF"  '暗号化用初期化ベクトルIV


    ''' <summary>
    ''' COMオブジェクトの解放
    ''' </summary>
    ''' <param name="objCom">COMオブジェクト</param>
    ''' <remarks>COMオブジェクトの解放を行う
    ''' <para>作成情報：2008/04/01 kawate
    ''' <p>改定情報：</p>
    ''' </para></remarks> 
    Public Shared Sub MRComObject(ByRef objCom As Object)
        'COMオブジェクトの使用後、明示的にCOMオブジェクトへの参照を解放する

        Try
            '提供されたランタイム呼び出し可能ラッパーの参照カウントをデクリメントする
            If Not objCom Is Nothing AndAlso IsComObject(objCom) Then
                Dim i As Integer
                Do
                    i = ReleaseComObject(objCom)
                Loop Until i <= 0
            End If

        Catch ex As Exception

        Finally
            '参照を解放する
            objCom = Nothing
        End Try
    End Sub


    ''' <summary>
    ''' アプリケーションパスの取得
    ''' </summary>
    ''' <returns>String アプリケーションパス</returns>
    ''' <remarks>アプリケーションパスの取得を行う
    ''' <para>作成情報：2008/04/01 kawate
    ''' <p>改定情報：</p>
    ''' </para></remarks> 
    Public Shared Function GetAppPath() As String
        Return Path.GetDirectoryName( _
            Assembly.GetExecutingAssembly().Location)
    End Function


    ''' <summary>
    ''' 全角英数字を半角へ変換　英語・日本語・Eメール入力欄 共通
    ''' </summary>
    ''' <param name="s">変換したい文字列</param>
    ''' <returns>boolean エラーコード
    ''' true	正常終了
    ''' false	異常終了
    ''' </returns>
    ''' <remarks>全角英数字を半角へ変換する
    ''' <para>作成情報：2008/04/01 kawate
    ''' <p>改定情報：</p>
    ''' </para></remarks> 
    Public Shared Function SingleChar(ByVal s As String) As Boolean

        Dim i As Integer
        Dim s1 As String
        Dim LS As String
        Dim SS As String

        LS = "ＡＢＣＤＥＦＧＨＩＪＫＬＭＮＯＰＱＲＳＴＵＶＷＸＹＺａｂｃｄｅｆｇｈｉｊｋｌｍｎｏｐｑｒｓｔｕｖｗｘｙｚ１２３４５６７８９０〇－＋＊／＝！゛＃＄％＆￥’（）｜［］｛｝＜＞；：，．＿　？＠"
        SS = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz12345678900-+*/=!""#$%&\'()|[]{}<>;:,._ ?@"

        s1 = ""
        For i = 1 To Len(s)
            If InStr(LS, Mid(s, i, 1)) <> 0 Then
                Return False
            End If
        Next

        Return True

    End Function


    ''' <summary>
    '''  ログ出力処理
    ''' </summary>
    ''' <param name="LogType">ログ種別</param>
    ''' <param name="LogMessage">出力内容</param>
    ''' <param name="argEx">エクセプションクラス</param>
    ''' <param name="Command">SQLコマンド</param>
    ''' <remarks>ファイル保存単位の指定が可能　ログ種別毎のログ出力可否の指定が可能
    ''' <para>作成情報：2010/03/17 kawate
    ''' <p>改定情報：</p>
    ''' </para></remarks> 
    Public Shared Sub WriteLog(ByVal LogType As String, ByVal LogMessage As String, _
                               ByVal argEx As Exception, ByVal Command As NpgsqlCommand)

        Dim info As New StackFrame(1, True)
        Dim strFileName As String = ""
        Dim strFileDate As String = ""
        Dim strDate As String = ""
        Dim strTime As String = ""
        Dim strPcName As String = ""
        Dim strLevel As String = ""
        Dim strPreFuncName As String = ""
        Dim sql As String = ""
        Dim sw As StreamWriter = Nothing

        Try
            '設定されたログ出力レベル以上のとき、出力処理を行う
            If LogType >= LogOutputLevel Then

                '出力情報取得
                strFileDate = Format(Now, "yyyyMMdd").ToString
                strDate = Format(Now, "yyyy/MM/dd").ToString
                strTime = Format(Now, "HH:mm:ss")
                strPcName = SystemInformation.ComputerName
                strPreFuncName = info.GetMethod().DeclaringType.Name & "." & info.GetMethod().Name

                '出力フォルダチェック
                If Directory.Exists(SystemLogPath) = False Then
                    Directory.CreateDirectory(SystemLogPath)
                End If

                'ログファイル名の取得
                Select Case SystemLogFileTerm
                    Case 1    '日単位
                        strFileName = SystemLogFileName & strFileDate & ".log"
                    Case 2    '月単位
                        strFileName = SystemLogFileName & Left(strFileDate, 6) & ".log"
                    Case 3    '年単位
                        strFileName = SystemLogFileName & Left(strFileDate, 4) & ".log"
                    Case Else '期間なし
                        strFileName = SystemLogFileName & ".log"
                End Select

                'ログ出力時のレベル表示設定
                Select Case LogType
                    Case LogLevel.DEBUG_Lv
                        strLevel = "DEBUG>"
                    Case LogLevel.TRACE_Lv
                        strLevel = "TRACE>"
                    Case LogLevel.ERROR_Lv
                        strLevel = "ERROR>"
                End Select

                'ファイルオープン
                sw = New StreamWriter(SystemLogPath & "\" & strFileName, True, Encoding.Default)

                'ログを書き込む
                'メッセージ出力
                sw.WriteLine(strDate & " " & strTime & " " & puUserID & " " & _
                             strLevel & " " & strPreFuncName & "() " & LogMessage)

                'エクセプション出力
                If argEx IsNot Nothing Then
                    sw.WriteLine(argEx.ToString)
                End If

                'SQL出力
                If Command IsNot Nothing Then
                    sw.WriteLine("■SQL Data :")

                    'SQLコマンド出力
                    sql = If(IsNothing(Command.CommandText), "", Command.CommandText)
                    sw.WriteLine("   Sql Command > " & sql)

                    'SQLパラメータ出力
                    sw.WriteLine("   Parameters Data > ")
                    For Each prm As NpgsqlParameter In Command.Parameters
                        sw.WriteLine("      [" & prm.ParameterName & "] " & prm.Value.ToString)
                    Next
                End If

                'フラッシュ（出力）
                sw.Flush()

                'ファイルクローズ
                sw.Close()

            End If

        Catch ex As Exception
            puErrMsg = ex.Message
        Finally
            If Not IsNothing(sw) Then
                sw.Close()
            End If
        End Try

    End Sub


    ''' <summary>
    ''' メール送信処理
    ''' </summary>
    ''' <param name="MailTo">宛先（To）※カンマ区切りで複数宛先（同報）可能</param>
    ''' <param name="MailCc">宛先（Cc）※カンマ区切りで複数宛先（同報）可能</param>
    ''' <param name="MailBcc">宛先（Bcc）※カンマ区切りで複数宛先（同報）可能</param>
    ''' <param name="Subject">件名</param>
    ''' <param name="Body">本文</param>
    ''' <param name="File">添付ファイル ※カンマ区切りで複数添付可能</param>
    ''' <param name="AsyncFlg">1:非同期メール送信</param>
    ''' <returns>boolean エラーコード
    ''' true 正常終了
    ''' false 異常終了</returns>
    ''' <remarks>メール送信処理を行う/同報メールの送信も可能（To,Cc,Bcc）/非同期か同期か選択可能
    ''' <para>作成情報：2008/09/01 kawate
    ''' <p>改定情報：</p>
    ''' </para></remarks> 
    Public Shared Function SendMail(ByVal MailTo As String, ByVal MailCc As String, ByVal MailBcc As String, ByVal Subject As String, ByVal Body As String, ByVal File As String, ByVal AsyncFlg As String) As Boolean

        Dim st = New System.Diagnostics.StackTrace(True)

        Dim smtp As New SmtpClient()
        Dim msg As New MailMessage()
        Dim enc As Encoding = Encoding.GetEncoding("iso-2022-jp")
        Dim aryMailTo As String()
        Dim aryMailCc As String()
        Dim aryMailBcc As String()
        Dim aryFile As String()

        Try
            CommonLogic.WriteLog(LogLevel.TRACE_Lv, "Start", Nothing, Nothing)

            '差出人
            msg.From = New MailAddress(My.Settings.MailFrom, My.Settings.MailFromName, enc)

            '宛先(To)
            If MailTo <> "" Then
                aryMailTo = Split(MailTo, ",")
                For i As Integer = 0 To aryMailTo.Length - 1
                    msg.To.Add(New MailAddress(aryMailTo(i)))
                Next
            End If

            '宛先(Cc)
            If MailCc <> "" Then
                aryMailCc = Split(MailCc, ",")
                For i As Integer = 0 To aryMailCc.Length - 1
                    msg.CC.Add(New MailAddress(aryMailCc(i)))
                Next
            End If

            '宛先(Bcc)
            If MailBcc <> "" Then
                aryMailBcc = Split(MailBcc, ",")
                For i As Integer = 0 To aryMailBcc.Length - 1
                    msg.Bcc.Add(New MailAddress(aryMailBcc(i)))
                Next
            End If

            '添付ファイル
            If File <> "" Then
                aryFile = Split(File, ",")
                For i As Integer = 0 To aryFile.Length - 1
                    msg.Attachments.Add(New Attachment(aryFile(i)))
                Next
            End If

            '件名
            msg.Subject = Subject
            msg.SubjectEncoding = enc

            '本文
            msg.Body = Body
            msg.BodyEncoding = enc

            'ＳＭＴＰサーバー
            smtp.Host = My.Settings.MailSmtp

            'SMTP認証
            If My.Settings.SmtpAuth = 1 Then
                smtp.Credentials = New NetworkCredential(My.Settings.SmtpUserId, My.Settings.SmtpPass)
            End If

            'メール送信
            If AsyncFlg = "1" Then
                smtp.SendAsync(msg, Nothing)
            Else
                smtp.Send(msg)
            End If

            CommonLogic.WriteLog(LogLevel.TRACE_Lv, "End", Nothing, Nothing)

            Return True

        Catch ex As Exception
            '例外処理
            CommonLogic.WriteLog(LogLevel.ERROR_Lv, ex.Message, ex, Nothing)
            Return False
        Finally
            '終了処理
        End Try

    End Function


    ''' <summary>
    ''' 共通設定取得処理
    ''' </summary>
    ''' <param name="CryptoPath">暗号キーのレジストリパス</param>
    ''' <param name="LogFileId">ログファイル名に付与するID</param>
    ''' <returns>Boolean エラーコード
    ''' true 正常終了
    ''' false 異常終了
    ''' </returns>
    ''' <remarks>各種共通設定を取得する</remarks>
    Public Function InitCommonSetting(ByVal CryptoPath As String, Optional ByVal LogFileId As String = Nothing) As Boolean

        'コンフィグ設定を取得
        If GetConfigSetting(LogFileId) = False Then
            Return False
        End If

        '暗号キーを取得
        If CryptoPath IsNot Nothing Then
            If GetCryptoKey(CryptoPath) = False Then
                Return False
            End If
        End If

        Return True

    End Function

    ''' <summary>
    ''' コンフィグ設定取得処理
    ''' </summary>
    ''' <returns>Boolean エラーコード
    ''' true 正常終了
    ''' false 異常終了</returns>
    ''' <remarks>コンフィグファイルから各種設定を取得する
    ''' <para>作成情報：2009/10/26 okamura
    ''' <p>改定情報：</p>
    ''' </para></remarks> 
    Private Function GetConfigSetting(ByVal LogFileId As String) As Boolean

        Try
            SystemLogPath = System.Configuration.ConfigurationManager.AppSettings("SystemLogPath")
            If LogFileId Is Nothing Then
                SystemLogFileName = System.Configuration.ConfigurationManager.AppSettings("SystemLogFileName")
            Else
                SystemLogFileName = LogFileId & "_" & System.Configuration.ConfigurationManager.AppSettings("SystemLogFileName")
            End If
            SystemLogFileTerm = System.Configuration.ConfigurationManager.AppSettings("SystemLogFileTerm")
            MailSmtp = System.Configuration.ConfigurationManager.AppSettings("MailSmtp")
            MailFrom = System.Configuration.ConfigurationManager.AppSettings("MailFrom")
            MailFromName = System.Configuration.ConfigurationManager.AppSettings("MailFromName")
            SmtpAuth = System.Configuration.ConfigurationManager.AppSettings("SmtpAuth")
            SmtpUserId = System.Configuration.ConfigurationManager.AppSettings("SmtpUserId")
            SmtpPass = System.Configuration.ConfigurationManager.AppSettings("SmtpPass")
            DbString = System.Configuration.ConfigurationManager.AppSettings("DbString")
            LogOutputLevel = System.Configuration.ConfigurationManager.AppSettings("LogOutputLevel")

            Return True
        Catch ex As Exception
            puErrMsg = ex.Message
            CommonLogic.WriteLog(LogLevel.ERROR_Lv, ex.Message, ex, Nothing)
            Return False
        End Try

    End Function


    ''' <summary>
    ''' コンボボックス設定処理
    ''' </summary>
    ''' <param name="strSetAry">設定するリストのIDと表示項目</param>
    ''' <param name="cmbSetBox">コンボボックス</param>
    ''' <returns>Boolean エラーコード
    ''' true 正常終了
    ''' false 異常終了
    ''' </returns>
    ''' <remarks>対象のコンボボックスにIDとリスト項目を設定する
    ''' <para>作成情報：2009/09/28 okamura kawate
    ''' <p>改定情報：</p>
    ''' </para></remarks> 
    Public Function SetCmbBox(ByVal strSetAry(,) As String, ByRef cmbSetBox As Object) As Boolean

        '変数宣言
        Dim dt As New DataTable()   'コンボボックスに格納するDataTable

        Try
            'DataTableに列を追加
            dt.Columns.Add("ID", GetType(String))
            dt.Columns.Add("Text", GetType(String))

            '行を追加
            For i As Integer = 0 To strSetAry.GetLength(0) - 1
                '新しい行を作成
                Dim row As DataRow = dt.NewRow()

                '各列に値をセット
                row("ID") = strSetAry(i, 0)    'IDをセット
                row("Text") = strSetAry(i, 1)  '表示項目をセット

                'DataTableに行を追加
                dt.Rows.Add(row)
            Next

            'DataTableの変更を確定
            dt.AcceptChanges()

            'コンボボックスのDataSourceにDataTableを割り当てる
            cmbSetBox.DataSource = dt

            '対応する値はDataTableのID列
            cmbSetBox.ValueMember = "ID"

            '表示される値はDataTableのText列
            cmbSetBox.DisplayMember = "Text"

            Return True

        Catch ex As Exception
            puErrMsg = ex.Message
            CommonLogic.WriteLog(LogLevel.ERROR_Lv, ex.Message, ex, Nothing)
            Return False
        End Try

    End Function


    ''' <summary>
    ''' コンボボックス設定処理（データテーブル型用）
    ''' </summary>
    ''' <param name="dtSetTable">設定するリストのIDと表示項目</param>
    ''' <param name="cmbSetBox">コンボボックス</param>
    ''' <param name="blnAddTopRow">先頭行追加フラグ（省略可）</param>
    ''' <param name="strTopID">先頭行セットID値（省略可）</param>
    ''' <param name="strTopText">先頭行セットText値（省略可）</param>
    ''' <returns>Boolean エラーコード
    ''' true 正常終了
    ''' false 異常終了
    ''' </returns>
    ''' <remarks>対象のコンボボックスにIDとリスト項目を設定する
    ''' <para>作成情報：2010/05/31 s.ikeda
    ''' <p>改定情報：</p>
    ''' </para></remarks> 
    Public Function SetCmbBox(ByVal dtSetTable As DataTable, _
                              ByRef cmbSetBox As Object, _
                              Optional ByVal blnAddTopRow As Boolean = True, _
                              Optional ByVal strTopID As String = "0", _
                              Optional ByVal strTopText As String = "") As Boolean

        '変数宣言
        Dim Addrow As DataRow   'コンボボックス先頭行に追加する行オブジェクト

        Try
            'カラム名称変更
            dtSetTable.Columns(0).ColumnName = "ID"
            dtSetTable.Columns(1).ColumnName = "Text"


            '先頭行追加フラグ(strAddTopRow)がTrueの場合のみ先頭行を追加する。
            If blnAddTopRow = True Then

                '先頭に追加する行を作成
                Addrow = dtSetTable.NewRow

                '指定されたID,Textをセット
                Addrow("ID") = strTopID
                Addrow("Text") = strTopText

                '先頭に行を追加
                dtSetTable.Rows.InsertAt(Addrow, 0)
            End If

            'コンボボックスのDataSourceにDataTableを割り当てる
            cmbSetBox.DataSource = dtSetTable

            '対応する値はDataTableのID列
            cmbSetBox.ValueMember = "ID"

            '表示される値はDataTableのText列
            cmbSetBox.DisplayMember = "Text"

            Return True
        Catch ex As Exception
            puErrMsg = ex.Message
            CommonLogic.WriteLog(LogLevel.ERROR_Lv, ex.Message, ex, Nothing)
            Return False
        End Try

    End Function


    ''' <summary>
    ''' コンボボックスEx設定処理（データテーブル型用）
    ''' </summary>
    ''' <param name="dtSetTable">設定するリストのIDと表示項目</param>
    ''' <param name="cmbSetBox">コンボボックス</param>
    ''' <param name="strValueMember">値項目名</param>
    ''' <param name="strDisplayMember">表示項目名</param>
    ''' <param name="blnAddTopRow">先頭行追加フラグ（省略可）</param>
    ''' <param name="strTopID">先頭行セットID値（省略可）</param>
    ''' <param name="strTopText">先頭行セットText値（省略可）</param>
    ''' <returns>Boolean エラーコード
    ''' true 正常終了
    ''' false 異常終了
    ''' </returns>
    ''' <remarks>対象のコンボボックスExにIDとリスト項目を設定する
    ''' <para>作成情報：2012/06/06 t.fukuo
    ''' <p>改定情報：</p>
    ''' </para></remarks> 
    Public Function SetCmbBoxEx(ByVal dtSetTable As DataTable, _
                                ByRef cmbSetBox As ComboBoxEx, _
                                ByVal strValueMember As String, _
                                ByVal strDisplayMember As String, _
                                Optional ByVal blnAddTopRow As Boolean = True, _
                                Optional ByVal strTopID As String = "0", _
                                Optional ByVal strTopText As String = "") As Boolean

        '変数宣言
        Dim Addrow As DataRow   'コンボボックス先頭行に追加する行オブジェクト

        Try
            '先頭行追加フラグ(strAddTopRow)がTrueの場合のみ先頭行を追加する。
            If blnAddTopRow = True Then

                '先頭に追加する行を作成
                Addrow = dtSetTable.NewRow

                For i As Integer = 0 To dtSetTable.Columns.Count - 1
                    If dtSetTable.Columns(i).ColumnName = strValueMember Then
                        '指定された先頭行IDをセット
                        Addrow(strValueMember) = strTopID
                    ElseIf dtSetTable.Columns(i).ColumnName = strDisplayMember Then
                        '指定された先頭行Textをセット
                        Addrow(strDisplayMember) = strTopText
                    Else
                        'その他の列にはNULLをセット
                        Addrow(i) = DBNull.Value
                    End If
                Next

                Addrow(strValueMember) = strTopID
                Addrow(strDisplayMember) = strTopText

                '先頭に行を追加
                dtSetTable.Rows.InsertAt(Addrow, 0)
            End If

            'コンボボックスのDataSourceにDataTableを割り当てる
            cmbSetBox.cmbColumns.DataSource = dtSetTable

            '対応する値をセット
            cmbSetBox.cmbColumns.ValueMember = strValueMember

            '表示される値をセット
            cmbSetBox.cmbColumns.DisplayMember = strDisplayMember

            Return True
        Catch ex As Exception
            puErrMsg = ex.Message
            CommonLogic.WriteLog(LogLevel.ERROR_Lv, ex.Message, ex, Nothing)
            Return False
        End Try

    End Function



    ''' <summary>
    ''' 暗号キー取得処理
    ''' </summary>
    ''' <param name="RegPath">登録時のパス</param>
    ''' <returns>Boolean エラーコード
    '''  true 正常終了
    '''  false 異常終了
    ''' </returns>
    ''' <remarks>暗号キーをレジストリから取得する
    ''' <para>作成情報：2010/03/26 okamura
    ''' <p>改定情報：</p>
    ''' </para></remarks> 
    Private Function GetCryptoKey(ByVal RegPath As String) As Boolean

        '暗号キーをレジストリから取得
        Dim regkey As Microsoft.Win32.RegistryKey = _
            Microsoft.Win32.Registry.CurrentUser.OpenSubKey(RegPath, False)

        If regkey Is Nothing Then
            puErrMsg = STTEC002
            Return False
        End If

        puCryptoKey = CType(regkey.GetValue("MyKey"), String)

        Return True

    End Function

    ''' <summary>
    ''' 暗号化処理
    ''' </summary>
    ''' <param name="argPlainStr">平文</param>
    ''' <param name="outCryptStr">暗号文</param>
    ''' <returns> Boolean エラーコード
    ''' true 正常終了
    ''' false 異常終了</returns>
    ''' <remarks>入力された平文をAES暗号化方式で暗号文に変換する
    ''' <para>作成情報：2010/03/26 okamura
    ''' <p>改定情報：</p>
    ''' </para></remarks> 
    Public Function EncodeAESC(ByVal argPlainStr As String, ByRef outCryptStr As String) As Boolean

        Try
            '対象を16進バイト列化
            Dim data As Byte() = Encoding.UTF8.GetBytes(argPlainStr)
            Dim key As String = CType(puCryptoKey, String)

            '暗号用のキー情報をセットする
            Dim aesKey As Byte() = Encoding.UTF8.GetBytes(key)
            Dim aesIV As Byte() = Encoding.UTF8.GetBytes(vec)

            '暗号化オブジェクトとストリームを作成する
            Dim aes As New Cryptography.RijndaelManaged
            Dim ms As New MemoryStream
            Dim cs As New Cryptography.CryptoStream( _
                            ms, _
                            aes.CreateEncryptor(aesKey, aesIV), _
                            Cryptography.CryptoStreamMode.Write)

            'ストリームに暗号化するデータを出力
            cs.Write(data, 0, data.Length)
            cs.Close()

            '暗号化されたデータを取得
            Dim code As String = byte2HexString(ms.ToArray())

            If code.Equals("") Then
                Return False
            End If

            outCryptStr = code

            ms.Close()

        Catch ex As Exception
            CommonLogic.WriteLog(LogLevel.ERROR_Lv, ex.Message, ex, Nothing)
            Return False
        End Try

        Return True

    End Function


    ''' <summary>
    ''' 複合化処理
    ''' </summary>
    ''' <param name="argCryptStr">暗号文</param>
    ''' <param name="outDecStr">平文</param>
    ''' <returns>Boolean エラーコード
    ''' true 正常終了
    ''' false 異常終了
    ''' </returns>
    ''' <remarks>入力された平文をAES暗号化方式で暗号文に変換する
    ''' <para>作成情報:2010/03/19 okamura
    ''' <p>改定情報：</p>
    ''' </para></remarks> 
    Public Function DecodeAES(ByVal argCryptStr As String, ByRef outDecStr As String) As Boolean
        Try
            '対象を16進バイト列化
            Dim data As Byte() = hexString2Byte(argCryptStr)

            If data Is Nothing Then
                Return False
            End If

            Dim key As String = CType(puCryptoKey, String)

            '暗号用のキー情報をセットする
            Dim aesKey As Byte() = Encoding.UTF8.GetBytes(key)
            Dim aesIV As Byte() = Encoding.UTF8.GetBytes(vec)

            '暗号化オブジェクトとストリームを作成する
            Dim aes As New Cryptography.RijndaelManaged()
            Dim ms As New MemoryStream()
            Dim cs As New Cryptography.CryptoStream( _
                            ms, _
                            aes.CreateDecryptor(aesKey, aesIV), _
                            Cryptography.CryptoStreamMode.Write)

            'ストリームに複合化するデータを出力
            cs.Write(data, 0, data.Length)
            cs.Close()

            '複合化されたデータを取得
            Dim code As String = Encoding.UTF8.GetString(ms.ToArray())
            outDecStr = code

            ms.Close()

        Catch ex As Exception
            CommonLogic.WriteLog(LogLevel.ERROR_Lv, ex.Message, ex, Nothing)
            Return False
        End Try

        Return True

    End Function


    ''' <summary>
    ''' バイト列16進変換処理
    ''' </summary>
    ''' <param name="data">変換するバイト列</param>
    ''' <returns>String 変換した16進数の文字列</returns>
    ''' <remarks>バイト列を16進数の文字列に変換する
    ''' <para>作成情報：2010/03/19 okamura
    ''' <p>改定情報：</p>
    ''' </para></remarks> 
    Private Function byte2HexString(ByVal data As Byte()) As String

        Dim result As String = ""

        Try
            '00-11-22形式を001122に変換
            result = BitConverter.ToString(data).Replace("-", "")
        Catch ex As Exception
            CommonLogic.WriteLog(LogLevel.ERROR_Lv, ex.Message, ex, Nothing)
        End Try

        Return result

    End Function


    ''' <summary>
    ''' 16進文字列バイト変換処理
    ''' </summary>
    ''' <param name="data"> 変換する16進文字列</param>
    ''' <returns>String 変換したバイト列</returns>
    ''' <remarks>16進数の文字列をバイト列に変換する
    ''' <para>作成情報：2010/03/19 okamura
    ''' <p>改定情報：</p>
    ''' </para></remarks> 
    Private Function hexString2Byte(ByVal data As String) As Byte()

        Dim size As Integer = data.Length / 2
        Dim result As Byte() = New Byte(size - 1) {}

        Try

            '2文字ずつ進み、バイトに変換
            For i As Integer = 0 To size - 1
                Dim target As String = data.Substring(i * 2, 2)
                result(i) = Convert.ToByte(target, 16)
            Next

        Catch ex As Exception
            CommonLogic.WriteLog(LogLevel.ERROR_Lv, ex.Message, ex, Nothing)
            result = Nothing
        End Try

        Return result

    End Function


    ''' <summary>
    ''' バイト数取得
    ''' </summary>
    ''' <param name="strTarget">[IN]バイト数取得対象文字列</param>
    ''' <returns>[OUT]Integer バイト数</returns>
    ''' <remarks>半角 1 バイト、全角 2 バイトとして、指定された文字列のバイト数を返す
    ''' <para>作成情報：2012/06/06 t.fukuo
    ''' <p>改訂情報:</p>
    ''' </para>
    ''' </remarks>
    Public Shared Function LenB(ByVal strTarget As String) As Integer
        Return System.Text.Encoding.GetEncoding("Shift_JIS").GetByteCount(strTarget)
    End Function

End Class
