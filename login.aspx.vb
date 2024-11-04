Imports Microsoft.VisualBasic
Imports System.Data.OleDb
Imports System.Data
Imports System.DirectoryServices
Imports System.IO
Imports System.Security.Cryptography
Imports System.Net.Mail
Imports System.Net
Imports System.Text

Public Class Login
    Inherits System.Web.UI.Page

    Dim bc As New BindControls
    Dim bytes As Byte() = ASCIIEncoding.ASCII.GetBytes("ZeroConn")
    Private Const AES_KEY As String = "AES_KEY"

    Public Function Decrypt(ByVal cryptedString As String) As String
        If [String].IsNullOrEmpty(cryptedString) Then
            Throw New ArgumentNullException("The string which needs to be decrypted can not be null.")
        End If
        Dim cryptoProvider As New DESCryptoServiceProvider()
        Dim memoryStream As New MemoryStream(Convert.FromBase64String(cryptedString))
        Dim cryptoStream As New CryptoStream(memoryStream, cryptoProvider.CreateDecryptor(bytes, bytes), CryptoStreamMode.Read)
        Dim reader As New StreamReader(cryptoStream)
        Return reader.ReadToEnd()
    End Function

    Public Shared Function EncryptStringAES(ByVal plainText As String, ByVal key As String) As String
        Dim keyBytes As Byte() = Encoding.UTF8.GetBytes(key.PadRight(16).Substring(0, 16))
        Dim iv As Byte() = Encoding.UTF8.GetBytes(key.PadRight(16).Substring(0, 16))

        Using aesAlg As Aes = Aes.Create()
            aesAlg.Key = keyBytes
            aesAlg.IV = iv
            aesAlg.Padding = PaddingMode.PKCS7

            Using msEncrypt As New MemoryStream()
                Using encryptor As ICryptoTransform = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV)
                    Using csEncrypt As New CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write)
                        Using swEncrypt As New StreamWriter(csEncrypt)
                            swEncrypt.Write(plainText)
                        End Using
                    End Using
                End Using
                Return Convert.ToBase64String(msEncrypt.ToArray())
            End Using
        End Using
    End Function

    Public Shared Function getRandomKey() As String
        Dim length As Integer = 16
        Dim random As Random = New Random()
        Const chars As String = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789"
        Return New String(Enumerable.Repeat(chars, length).[Select](Function(s) s(random.[Next](s.Length))).ToArray())
    End Function

    Public Shared Function DecryptStringAES(ByVal cipherText As String, ByVal key As String) As String
        Dim keyBytes As Byte() = Encoding.UTF8.GetBytes(key.PadRight(16).Substring(0, 16))
        Dim iv As Byte() = Encoding.UTF8.GetBytes(key.PadRight(16).Substring(0, 16))

        Try
            Dim encrypted As Byte() = Convert.FromBase64String(cipherText)

            Using aesAlg As Aes = Aes.Create()
                aesAlg.Key = keyBytes
                aesAlg.IV = iv
                aesAlg.Padding = PaddingMode.PKCS7

                Using msDecrypt As New MemoryStream(encrypted)
                    Using decryptor As ICryptoTransform = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV)
                        Using csDecrypt As New CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read)
                            Using srDecrypt As New StreamReader(csDecrypt)
                                Return srDecrypt.ReadToEnd()
                            End Using
                        End Using
                    End Using
                End Using
            End Using
        Catch ex As CryptographicException
            Return "Decryption failed: " & ex.Message
        End Try
    End Function

    Public Function IsAuthenticated(ByVal domain As String, ByVal userId As String, ByVal pwd As String) As Boolean
        Return True
    End Function

    Private Sub resetAESKey()
        Dim randmmAESKey As String = getRandomKey()
        aesKeyField.Value = randmmAESKey
        Session(AES_KEY) = randmmAESKey
    End Sub

    Protected Sub btnLogin_Click(sender As Object, e As EventArgs) Handles btnLogin.Click
        If Session("captcha") IsNot Nothing Then
            If (Not txtCaptcha.Text.Equals(Session("captcha").ToString())) Then
                resetAESKey()
                lblError.Text = "Captcha was Invalid!"
            Else
                Dim email As String = txtEmailID.Text.Trim()
                Dim aadhar As String = txtAadharNo.Text.Trim()

                If aadhar.Length = 12 And email.Length >= 4 Then
                    Dim key As String = aadhar & email.Substring(0, 4)

                    Dim encryptedPwd As String = EncryptStringAES(txtPwd.Text.Trim(), key)

                    Dim query As String = "SELECT * FROM EILSPORT_USER WHERE Email = '" & email & "' AND PWD = '" & encryptedPwd & "' AND AADHAR = '" & aadhar & "'"
                    Dim dt As DataTable = bc.getDetailsInDataTable(query)

                    If dt.Rows.Count > 0 Then
                        Session("AadharNo") = dt.Rows(0)("AADHAR").ToString()
                        Session("EmailID") = dt.Rows(0)("EMAIL").ToString()
                        Response.Redirect("Status.aspx")
                        Response.End()
                    Else
                        lblRegisterError.Text = "User DOES NOT exist with this Email ID or Aadhar No. Kindly Register first!"
                        lblRegisterError.ForeColor = System.Drawing.Color.Red
                    End If
                Else
                    lblError.Text = "Invalid Aadhaar or Email length."
                    lblError.ForeColor = System.Drawing.Color.Red
                End If
            End If
        Else
            Response.Redirect("login.aspx")
            lblError.Text = "Captcha was Invalid!"
        End If
    End Sub

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If IsPostBack = False Then
            Session.Clear()
            Session.Abandon()
            resetAESKey()
        End If
    End Sub

    Private Function GenerateOtp() As String
        Dim random As New Random()
        Return random.Next(1000, 9999).ToString()
    End Function

    Private Function HashPassword(ByVal password As String) As String
        Dim sha256 As SHA256 = SHA256.Create()
        Dim bytes As Byte() = Encoding.UTF8.GetBytes(password)
        Dim hash As Byte() = sha256.ComputeHash(bytes)

        Dim stringBuilder As New StringBuilder()
        For Each t As Byte In hash
            stringBuilder.Append(t.ToString("x2"))
        Next
        Return stringBuilder.ToString()
    End Function

    Protected Sub btnRegister_Click(sender As Object, e As EventArgs)
        txtOtp.Enabled = False
        txtPass.Enabled = False
        getOTP.Enabled = False

        Dim fullName As String = txtFullName.Text
        Dim email As String = txtEmail.Text
        Dim aadhar As String = txtAadhar.Text
        Dim userOtp As String = txtOtp.Text
        Dim generatedOtp As String = CType(Session("GeneratedOtp"), String)

        If userOtp = generatedOtp Then
            If aadhar.Length = 12 And email.Length >= 4 Then
                Dim key As String = aadhar & email.Substring(0, 4)

                Dim encryptedPassword As String = EncryptStringAES(txtPass.Text, key)

                Dim query As String = "INSERT INTO EILSPORT_USER (APPNAME, Email, Aadhar, PWD, ADVNO, REGDATE) VALUES ('" & fullName & "', '" & email & "', '" & aadhar & "', '" & encryptedPassword & "', NULL, TRUNC(SYSDATE))"
                bc.executeSQLCommand(query)

                lblRegisterError.Text = "Email ID Verified! Registration successful, Now you may Sign In!"
                lblRegisterError.ForeColor = System.Drawing.Color.Green

                txtEmailID.Enabled = True
                txtAadharNo.Enabled = True
                txtPwd.Enabled = True
                txtCaptcha.Enabled = True

                btnLogin.Enabled = True
                newUser.Enabled = True
                forgotPass.Enabled = True

                btnRegister.Enabled = False
            Else
                lblRegisterError.Text = "Invalid Aadhaar or Email length."
                txtFullName.Enabled = True
                txtEmail.Enabled = True
                txtAadhar.Enabled = True
                txtOtp.Enabled = False
                txtPass.Enabled = False
            End If
        Else
            lblRegisterError.Text = "Invalid OTP or Invalid Email ID. Please Try Again!"
            txtFullName.Enabled = True
            txtEmail.Enabled = True
            txtAadhar.Enabled = True
            txtOtp.Enabled = False
            txtPass.Enabled = False
        End If

        txtFullName.Text = String.Empty
        txtEmail.Text = String.Empty
        txtAadhar.Text = String.Empty
        txtOtp.Text = String.Empty
        txtPass.Text = String.Empty
    End Sub


    Protected Sub btnGetOtp_Click(sender As Object, e As EventArgs)

        Dim query As String = "SELECT * FROM EILSPORT_USER WHERE Email = '" & txtEmail.Text & "' OR Aadhar = '" & txtAadhar.Text & "'"
        Dim dt As DataTable = bc.getDetailsInDataTable(query)
        If dt.Rows.Count > 0 Then
            lblRegisterError.Text = "User already exists with this Email or Aadhar number."
            lblRegisterError.ForeColor = System.Drawing.Color.Red
        Else
            txtEmailID.Text = String.Empty
            txtPwd.Text = String.Empty
            txtAadharNo.Text = String.Empty
            txtCaptcha.Text = String.Empty

            txtEmailID.Enabled = False
            txtPwd.Enabled = False
            txtAadharNo.Enabled = False
            txtCaptcha.Enabled = False

            txtFullName.Enabled = False
            txtEmail.Enabled = False
            txtAadhar.Enabled = False

            btnLogin.Enabled = False
            newUser.Enabled = False
            forgotPass.Enabled = False
            btnRegister.Enabled = True
            Dim otp As String = GenerateOtp()
            Try
                Dim email As String = txtEmail.Text
                Dim body As String = "EIL Sports Scholarship Portal. Dear Sir/Ma'am, Your Registration OTP code is: " & otp & "<br>" & _
                                     "<i>This is an autogenerated mail. Please do not reply.</i>"
                SendMail(email, "itsapp@eil.co.in", "", "Verification Code for EIL Sports Scholarship", body, "")
                Session("GeneratedOtp") = otp ' Store OTP in session for verification
                lblRegisterError.Text = "An OTP has been sent to given email ID. Please enter that OTP Pin !"
                lblRegisterError.ForeColor = System.Drawing.Color.Blue
                txtOtp.Enabled = True
                txtPass.Enabled = True
            Catch ex As Exception
                lblRegisterError.Text = "Error sending OTP : " & ex.Message
            End Try
        End If
    End Sub

    Public Function SendMail(ByVal toList As String, ByVal fromm As String, ByVal ccList As String, ByVal subject As String, ByVal body As String, ByVal msg As String) As String

        Dim message As New MailMessage()
        Dim smtpClient As New SmtpClient()

        Try
            Dim mfrom As MailAddress = New MailAddress(fromm)
            Dim mto As MailAddress = New MailAddress(toList)
            Dim objEmail As MailMessage = New MailMessage(mfrom, mto)
            'objEmail.Bcc.Add(ccList)
            objEmail.Subject = subject
            objEmail.Body = body
            objEmail.Priority = MailPriority.High
            objEmail.IsBodyHtml = True
            Dim smtpmail As SmtpClient = New SmtpClient("appsmtp.eil.co.in")
            If toList <> "" Then
                smtpmail.Send(objEmail)
            End If

            objEmail.Dispose()
            msg = "Successful"
        Catch ex As Exception
            msg = ex.Message
        End Try
        Return msg
    End Function

    Protected Sub btnForgot_Click(sender As Object, e As EventArgs)
        txtEmailID.Enabled = False
        txtAadharNo.Enabled = False

        Dim email As String = txtEmailID.Text
        Dim aadhar As String = txtAadharNo.Text
        Dim query As String = "SELECT * FROM EILSPORT_USER WHERE Email = '" & email & "' AND AADHAR = '" & aadhar & "'"
        Dim dt As DataTable = bc.getDetailsInDataTable(query)

        If dt.Rows.Count > 0 Then
            Dim encryptedPwd As String = dt.Rows(0)("pwd").ToString()

            If aadhar.Length = 12 And email.Length >= 4 Then
                Dim key As String = aadhar & email.Substring(0, 4)

                Dim decryptedPwd As String = DecryptStringAES(encryptedPwd, key)

                Dim body As String = "EIL Sports Scholarship Portal.<br><br> Dear " + dt.Rows(0)("APPNAME").ToString() + ", Your Password is: " & decryptedPwd & "<br>Please DO NOT Share Your Password with Anyone !<br>" & _
                                     "<i>This is an autogenerated mail. Please do not reply.</i>"
                SendMail(email, "itsapp@eil.co.in", "", "User Password for EIL Sports Scholarship", body, "")
                lblError.Text = "User Password has been sent to the provided Email ID."
                lblError.ForeColor = System.Drawing.Color.Blue
            Else
                lblError.Text = "Invalid Aadhaar or Email length."
                lblError.ForeColor = System.Drawing.Color.Red
            End If
        Else
            lblError.Text = "NO User exists with this Email ID & Aadhar No. !"
            lblError.ForeColor = System.Drawing.Color.Red
        End If
    End Sub

    Protected Sub btnNew_Click(sender As Object, e As EventArgs)
        newUserRegister.Style("display") = "block"
    End Sub
End Class
