Imports System
Imports System.Data
Imports System.IO
Imports System.Text

''' <summary>
''' CommonValidation
''' </summary>
''' <remarks>CommonValidation</remarks>
''' 

Public Class CommonValidation

    ''' <summary>
    ''' 日付範囲チェック
    ''' </summary>
    ''' <param name="s">yyyy/mm/dd形式</param>
    ''' <param name="t">yyyy/mm/dd形式</param>
    ''' <returns>指定した日付のFROM～TOが正しければ true。それ以外は false</returns>
    ''' <remarks>日付範囲チェックを行う</remarks>
    Public Function IsDateFromTo(ByVal s As String, ByVal t As String) As Boolean

        Dim iDateS As Integer
        Dim iDateT As Integer

        'ブランクの場合、チェックしない
        If Trim(s) = "" Or Trim(t) = "" Then
            Return True
        End If

        '日付のFROM,TOが正しいかチェック
        iDateS = CInt(Mid(s, 1, 4) & Mid(s, 6, 2) & Mid(s, 9, 2))
        iDateT = CInt(Mid(t, 1, 4) & Mid(t, 6, 2) & Mid(t, 9, 2))

        If iDateS > iDateT Then
            Return False
        End If

        Return True

    End Function

    ''' <summary>
    ''' 年月範囲チェック
    ''' </summary>
    ''' <param name="s">yyyymm形式</param>
    ''' <param name="t">yyyymm形式</param>
    ''' <returns>指定した年月のFROM～TOが正しければ true。それ以外は false</returns>
    ''' <remarks>年月範囲チェックを行う</remarks>
    Public Function IsYMFromTo(ByVal s As String, ByVal t As String) As Boolean

        'ブランクの場合、チェックしない
        If s = "" Or t = "" Then
            Return True
        End If

        '年月のFROM,TOが正しいかチェック
        If CInt(s) > CInt(t) Then
            Return False
        End If

        Return True

    End Function


    ''' <summary>
    ''' 半角英数字チェック
    ''' </summary>
    ''' <param name="argTxt">[IN]チェック対象文字列</param>
    ''' <returns>Boolean  チェック結果    true  ＯＫ  false  ＮＧ</returns>
    ''' <remarks>対象文字列が半角文字のみかチェックする
    ''' <para>作成情報：2009/09/04 okamura
    ''' <p>改定情報：2009/09/14 okamura</p>
    ''' </para></remarks>
    Public Function IsHalfChar(ByVal argTxt As String) As Boolean

        Dim utfEnc As Encoding
        Dim length As Integer
        Dim strTmp As String

        utfEnc = Encoding.GetEncoding("Shift-JIS")

        length = Len(argTxt)

        For i = 1 To length
            strTmp = Mid(argTxt, i, 1)
            If (Not strTmp Like "[\!-~]") Then
                Return False
            End If
        Next i

        Return True

    End Function



    ''' <summary>
    ''' 半角数字チェック
    ''' </summary>
    ''' <param name="argTxt">[IN]チェック対象文字列</param>
    ''' <returns>Boolean  チェック結果    true  ＯＫ  false  ＮＧ</returns>
    ''' <remarks>対象文字列が半角数字のみかチェックする
    ''' <para>作成情報：2009/09/04 okamura
    ''' <p>改定情報：</p>
    ''' </para></remarks>
    Public Function IsHalfNmb(ByVal argTxt As String) As Boolean

        Dim i As Integer
        Dim length As Integer
        Dim strTmp As String

        length = Len(argTxt)

        If length = 0 Then
            Return True
        End If

        For i = 1 To length
            strTmp = Mid(argTxt, i, 1)
            If (Not strTmp Like "[0-9]") Or (strTmp = " ") Then
                Return False
            End If
        Next i

        Return True

    End Function



    ''' <summary>
    ''' 半角符号付き数字チェック
    ''' </summary>
    ''' <param name="argTxt">[IN]チェック対象文字列</param>
    ''' <returns>Boolean  チェック結果    true  ＯＫ  false  ＮＧ</returns>
    ''' <remarks>対象文字列が半角数字のみかチェックする（マイナス含む）
    ''' <para>作成情報：2009/11/18 okamura
    ''' <p>改定情報：</p>
    ''' </para></remarks>
    Public Function IsHalfMNmb(ByVal argTxt As String) As Boolean

        Dim i As Integer
        Dim length As Integer
        Dim strTmp As String

        length = Len(argTxt)

        If length = 0 Then
            Return True
        End If

        For i = 1 To length
            strTmp = Mid(argTxt, i, 1)

            If i = 1 Then
                If ((strTmp <> "-") And (Not strTmp Like "[0-9]")) Or (strTmp = " ") Then
                    Return False
                End If
            Else
                If (Not strTmp Like "[0-9]") Or (strTmp = " ") Then
                    Return False
                End If
            End If
        Next i

        Return True

    End Function



    ''' <summary>
    ''' 半角カナチェック
    ''' </summary>
    ''' <param name="argTxt">[IN]チェック対象文字列</param>
    ''' <returns>Boolean  チェック結果    true  ＯＫ  false  ＮＧ</returns>
    ''' <remarks>対象文字列が半角カナ文字のみかチェックする
    ''' <para>作成情報：2009/09/04 okamura
    ''' <p>改定情報：</p>
    ''' </para></remarks>
    Public Function IsHalfKana(ByVal argTxt As String) As Boolean

        Dim i As Integer
        Dim length As Integer

        length = Len(argTxt)

        If length = 0 Then
            Return True
        End If

        For i = 1 To length
            If Not Mid(argTxt, i, 1) Like "[ｦ-ﾟ]" Then
                Return False
            End If
        Next i

        Return True

    End Function



    ''' <summary>
    ''' 全角チェック
    ''' </summary>
    ''' <param name="argTxt">[IN]チェック対象文字列</param>
    ''' <returns>Boolean  チェック結果    true  ＯＫ  false  ＮＧ</returns>
    ''' <remarks>対象文字列が全角文字のみかチェックする
    ''' <para>作成情報：2009/09/04 okamura
    ''' <p>改定情報：</p>
    ''' </para></remarks>
    Public Function IsFullChar(ByVal argTxt As String) As Boolean

        Dim utfEnc As Encoding

        If Len(argTxt) = 0 Then
            Return True
        End If

        utfEnc = Encoding.GetEncoding("Shift-JIS")

        If utfEnc.GetByteCount(argTxt) <> (argTxt.Length * 2) Then
            Return False
        End If

        Return True

    End Function



    ''' <summary>
    ''' 半角英数字カナチェック
    ''' </summary>
    ''' <param name="argTxt">[IN]チェック対象文字列</param>
    ''' <returns>Boolean  チェック結果    true  ＯＫ  false  ＮＧ</returns>
    ''' <remarks>対象文字列が半角英数字カナ文字のみかチェックする
    ''' <para>作成情報：2009/09/14 okamura
    ''' <p>改定情報：</p>
    ''' </para></remarks>
    Public Function IsHalfCharKana(ByVal argTxt As String) As Boolean

        Dim i As Integer
        Dim strTmp As String
        Dim length As Integer

        length = Len(argTxt)

        If length = 0 Then
            Return True
        End If

        For i = 1 To length
            strTmp = Mid(argTxt, i, 1)
            If (Not strTmp Like "[\!-~]") And (Not strTmp Like "[ｦ-ﾟ]") Then
                Return False
            End If
        Next i

        Return True

    End Function

End Class
