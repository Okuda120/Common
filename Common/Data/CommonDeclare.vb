''' <summary>
''' CommonDeclare
''' </summary>
''' <remarks>定数、変数の設定</remarks>
''' 
Public Module CommonDeclare
    ''' <summary>
    ''' コンフィグ取得
    ''' </summary>
    ''' <remarks>コンフィグ取得</remarks>
    Public SystemLogPath As String

    ''' <summary>
    ''' コンフィグ取得
    ''' </summary>
    ''' <remarks>コンフィグ取得</remarks>
    Public SystemLogFileName As String

    ''' <summary>
    ''' コンフィグ取得
    ''' </summary>
    ''' <remarks>コンフィグ取得</remarks>
    Public SystemLogFileTerm As String

    ''' <summary>
    ''' コンフィグ取得
    ''' </summary>
    ''' <remarks>コンフィグ取得</remarks>
    Public ErrorLogFlg As String

    ''' <summary>
    ''' コンフィグ取得
    ''' </summary>
    ''' <remarks>コンフィグ取得</remarks>
    Public EventLogFlg As String

    ''' <summary>
    ''' コンフィグ取得
    ''' </summary>
    ''' <remarks>コンフィグ取得</remarks>
    Public TraceLogFlg As String

    ''' <summary>
    ''' コンフィグ取得
    ''' </summary>
    ''' <remarks>コンフィグ取得</remarks>
    Public DebugLogFlg As String

    ''' <summary>
    ''' コンフィグ取得
    ''' </summary>
    ''' <remarks>コンフィグ取得</remarks>
    Public MailSmtp As String

    ''' <summary>
    ''' コンフィグ取得
    ''' </summary>
    ''' <remarks>コンフィグ取得</remarks>
    Public MailFrom As String

    ''' <summary>
    ''' コンフィグ取得
    ''' </summary>
    ''' <remarks>コンフィグ取得</remarks>
    Public MailFromName As String

    ''' <summary>
    ''' コンフィグ取得
    ''' </summary>
    ''' <remarks>コンフィグ取得</remarks>
    Public SmtpAuth As String

    ''' <summary>
    ''' コンフィグ取得
    ''' </summary>
    ''' <remarks>コンフィグ取得</remarks>
    Public SmtpUserId As String

    ''' <summary>
    ''' コンフィグ取得
    ''' </summary>
    ''' <remarks>コンフィグ取得</remarks>
    Public SmtpPass As String

    ''' <summary>
    ''' コンフィグ取得
    ''' </summary>
    ''' <remarks>コンフィグ取得</remarks>
    Public DbString As String

    ''' <summary>
    ''' コンフィグ取得
    ''' </summary>
    ''' <remarks>コンフィグ取得</remarks>
    Public SqlLogFlg As String

    ''' <summary>
    ''' コンフィグ取得
    ''' </summary>
    ''' <remarks>コンフィグ取得</remarks>
    Public LogOutputLevel As String

    ''' <summary>
    ''' 定数LogLevel
    ''' </summary>
    ''' <remarks>定数LogLevel</remarks>
    Public Enum LogLevel

        ''' <summary>
        ''' デバッグモード
        ''' </summary>
        ''' <remarks></remarks>
        DEBUG_Lv = 1

        ''' <summary>
        ''' トレースモード
        ''' </summary>
        ''' <remarks></remarks>
        TRACE_Lv = 2

        ''' <summary>
        ''' エラーモード
        ''' </summary>
        ''' <remarks></remarks>
        ERROR_Lv = 3

    End Enum

    ''' <summary>
    ''' 共通エラーメッセージ
    ''' </summary>
    ''' <remarks>表示内容　システムエラーが発生しました。</remarks> 
    Public Const STTEC001 As String = "システムエラーが発生しました。"

    ''' <summary>
    ''' 共通エラーメッセージ
    ''' </summary>
    ''' <remarks>表示内容　レジストリ情報を取得できませんでした。</remarks>
    Public Const STTEC002 As String = "レジストリ情報を取得できませんでした。"

    ''' <summary>
    ''' コモン内部変数
    ''' </summary>
    ''' <remarks>コモン内部変数</remarks>
    Friend SecretKey As String

    ''' <summary>
    ''' 共通変数
    ''' </summary>
    ''' <remarks>puErrMsg</remarks>
    Public puErrMsg As String

    ''' <summary>
    ''' 共通変数
    ''' </summary>
    ''' <remarks>puUserID</remarks>
    Public puUserID As String

    ''' <summary>
    ''' 共通変数
    ''' </summary>
    ''' <remarks>puCryptoKey</remarks>
    Public puCryptoKey As String

End Module
