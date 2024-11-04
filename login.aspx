<%@ Page Language="VB" AutoEventWireup="false" EnableSessionState="True" EnableViewState="true" CodeFile="Login.aspx.vb" Inherits="Login" MaintainScrollPositionOnPostback="true" %>

<!DOCTYPE html>

<html>

<head>
    <title>EIL Sports Login Page</title>
    <meta content="public" http-equiv="Cache-control" />
    <meta charset="utf-8" />
    <meta content="IE=edge" http-equiv="X-UA-Compatible" />
    <meta content="width=device-width, initial-scale=1" name="viewport" />


    <link href="LoginContent/edms.css" media="all" rel="stylesheet" type="text/css" />
    <link href="LoginContent/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" type="text/css" href="/bootstrap.min.css" />
    <script type="text/javascript" src="<%= Page.ResolveUrl("~/jquery-3.6.0.min.js")%>"></script>
    <script type="text/javascript" src="Scripts/aes.js"></script>
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.3.1/jquery.min.js"></script>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.1/dist/css/bootstrap.min.css" rel="stylesheet" />
    <style>
        table.form-table {
            display: table;
            border-collapse: separate;
            border-spacing: 2px;
        }

        .btn:not(.footer) {
            margin-bottom: 20px;
            margin-top: 20px;
        }

        .btn {
            margin-right: 16px;
        }
    </style>
    <style>
        #header-fixed {
            position: fixed;
            top: 0px;
            display: none;
            background-color: white;
        }
    </style>
    <style>
        .breadcrumb {
            margin-bottom: 0px !important;
        }

        div.FixedHeader_Cloned th,
        div.FixedHeader_Cloned td {
            background-color: #337AB7 !important;
        }

        div.FixedHeader_Cloned th,
        div.FixedHeader_Cloned td {
            color: #fff !important;
        }

        .bigdrop {
            max-width: 600px;
        }

        body {
            margin: 0px;
        }

        table.fixed {
            table-layout: fixed;
        }

            table.fixed td {
                overflow: hidden;
            }

        .fontsizse {
            font-size: 1.1rem;
        }

        .fontsizselabel {
            font-size: 1rem;
        }
    </style>

    <script>
        function showNewUserRegister() {
            var newUser = document.getElementById("newUserRegister");
            newUser.style.display = "block"; // Show the div
        }

        function checkForgot() {
            var emailInput = document.getElementById('<%= txtEmailID.ClientID %>').value;
            var aadhar = document.getElementById("<%= txtAadharNo.ClientID%>").value.trim();

            var emailPattern = /^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/;

            if (emailInput === "" || aadhar === "" || aadhar.length !== 12 || isNaN(aadhar)) {
                alert("Please enter your Email ID & valid 12-digit Aadhar Card No. !");
                return false; // Prevent the server-side function from being called
            } else if (!emailPattern.test(emailInput)) {
                alert("Please enter a valid Email ID.");
                return false; // Prevent the server-side function from being called
            }
            return true; // If validation passes, allow server-side function to be called
        }

        function validatePassword() {
            var password = document.getElementById("<%= txtPass.ClientID %>").value.trim();

            var passwordPattern = /^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{8,}$/;

            if (!passwordPattern.test(password)) {
                alert("Password must be at least 8 characters long and contain at least 1 numeric digit and 1 alphabetic character.");
                return false;
            }

            return true;
        }
        function validateRegisterForm() {

            var fullName = document.getElementById("<%= txtFullName.ClientID %>").value.trim();
            var aadhar = document.getElementById("<%= txtAadhar.ClientID %>").value.trim();
            var email = document.getElementById("<%= txtEmail.ClientID %>").value.trim();

            var emailPattern = /^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,6}$/;

            if (fullName === "") {
                alert("Please enter your Full Name.");
                return false;
            }

            if (aadhar === "" || aadhar.length !== 12 || isNaN(aadhar)) {
                alert("Please enter a valid 12-digit Aadhar Card No.");
                return false;
            }

            if (email === "" || !emailPattern.test(email)) {
                alert("Please enter a valid Email ID.");
                return false;
            }

            return true;
        }
        function RefreshCaptcha() {

            var img = document.getElementById("imgCaptcha");

            img.src = "Captcha.ashx?query=" + Math.random();
        }
        function SubmitsEncry() {

            var txtpassword = document.getElementById("<%=txtPwd.ClientID%>").value.trim();
            if (txtpassword == "") {
                alert('Please enter Password');
                return false;
            }
            else {
                var aesValue = document.getElementById("<%=aesKeyField.ClientID%>").value.trim()
                var key = CryptoJS.enc.Utf8.parse(aesValue);
                var iv = CryptoJS.enc.Utf8.parse(aesValue);
                var encryptedpassword = CryptoJS.AES.encrypt(CryptoJS.enc.Utf8.parse(txtpassword), key,
                {
                    keySize: 128 / 8,
                    iv: iv,
                    mode: CryptoJS.mode.CBC,
                    padding: CryptoJS.pad.Pkcs7
                });



                document.getElementById("<%=txtPwd.ClientID%>").value = encryptedpassword;
                document.getElementById("<%=aesKeyField.ClientID%>").value = '';
            }
        }

    </script>
</head>

<body style="overflow-x: hidden;">

    <div>
        <script>
            var msg = null;
            var error = null;

            if (msg !== null && msg.length > 0)
                alertify.log(msg, "success", 0);
            if (error !== null && error.length > 0)
                alertify.log(error, "error", 0);
        </script>
    </div>

    <div style="background-color: #fff; border: 15px solid #337ab7; height: 90px">
        <div id="centeredmenu" style="height: 60px; padding-top: 1px;">
            <span class="logo" style="outline: none; top: -16px;" tabindex="1">
                <img style="height: 100%; width: 20%; float: left; margin-left: 20px;" src="./eilLogo.png" />
            </span>
            <ul class="nav nav-pills" style="float: left;">
                <li style="color: #000; font-size: 35px; color: #337ab7; font-weight: bold; top: 5px; left: 20px; padding-left: 50px;">EIL SPORTS SCHOLARSHIP PORTAL</li>
            </ul>
        </div>
    </div>

    <form autocomplete="on" runat="server" class="form-horizontal">
        <div class="container-fluid row">

            <div id="oldUserRegister" class="col-6">
                <div class="panel panel-primary" style="width: 90%; max-width: 600px; margin: auto;">
                    <div class="panel-heading">
                        <h3 class="panel-title">
                            <span aria-hidden="true" class="glyphicon glyphicon-log-in"></span>&nbsp;&nbsp;&nbsp;<b>SIGN IN</b>
                        </h3>
                    </div>
                    <div class="row">&nbsp;</div>
                    <div class="panel-body">

                        <asp:HiddenField ID="aesKeyField" runat="server" Value="" />
                        <asp:ScriptManager runat="server"></asp:ScriptManager>
                        <div class="form-group fontsizselabel row">
                            <label class="col-3 control-label" for="txtEmailID">Email ID<sup style="color: red;">*</sup> :</label>
                            <div class="col-9" style="display: flex;">
                                <div class="input-group">
                                    <asp:TextBox runat="server" ID="txtEmailID" placeholder="Email Address" CssClass="form-control fontsizse" autocomplete="on" MaxLength="50" />
                                </div>
                            </div>
                        </div>
                        <div class="row">&nbsp;</div>
                        <div class="form-group fontsizselabel row">
                            <label class="col-3 control-label" for="txtAadharNo">Aadhar No<sup style="color: red;">*</sup>:</label>
                            <div class="col-9" style="display: flex;">
                                <div class="input-group">
                                    <asp:TextBox runat="server" ID="txtAadharNo" placeholder="Aadhar Card Number" CssClass="form-control fontsizse" autocomplete="on" MaxLength="12" />
                                </div>
                            </div>
                        </div>
                        <div class="row">&nbsp;</div>
                        <div class="form-group fontsizselabel row">
                            <label class="col-3 control-label" for="txtPwd">Password<sup style="color: red;">*</sup> :</label>
                            <div class="col-9" style="display: flex;">
                                <div class="input-group" style="font-size: 1rem;">
                                    <asp:TextBox runat="server" ID="txtPwd" placeholder="Password" TextMode="Password" CssClass="form-control fontsizse" MaxLength="32" />

                                </div>
                            </div>
                        </div>
                        <div class="row">&nbsp;</div>
                        <div class="form-group fontsizselabel row">
                            <asp:Label runat="server" CssClass="col-4 control-label"><a href="#" onclick="javascript:RefreshCaptcha();"><b>Refresh</b></a></asp:Label>
                            <div class="col-6" style="align-content: flex-start;">
                                <img src="Captcha.ashx" id="imgCaptcha" />
                            </div>
                        </div>
                        <div class="row">&nbsp;</div>
                        <div class="form-group fontsizselabel row">
                            <label class="col-3 control-label">Captcha<sup style="color: red;">*</sup> :</label>
                            <div class="col-7">
                                <div class="input-group">
                                    <asp:TextBox ID="txtCaptcha" runat="server" CssClass="form-control fontsizse" placeholder="Enter Captcha" autocomplete="off" MaxLength="6"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12 fontsizselabel">
                                <asp:Label ID="lblError" runat="server" Font-Bold="true" ForeColor="Firebrick"></asp:Label>
                            </div>
                        </div>
                        <div class="row mt-4 mb-4">
                            <div class="col-12 text-center" style="font-size: 15px;">
                                <asp:Button ID="btnLogin" runat="server" Text="Sign In" CssClass="btn btn-primary fontsizse" OnClientClick="return SubmitsEncry();" />
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row mt-4 mb-4">
                    <div class="col-12 text-center" style="font-size: 15px;">
                        <asp:Button ID="newUser" runat="server" Text="New User" CssClass="btn btn-warning fontsizse" OnClick="btnNew_Click" />
                        <asp:Button ID="forgotPass" runat="server" Text="Forgot Password" CssClass="btn btn-secondary fontsizse" OnClick="btnForgot_Click" OnClientClick="return checkForgot();" />
                    </div>
                </div>
            </div>

            <div id="newUserRegister" class="col-6" style="display: none;" runat="server">
                <div class="panel panel-primary" style="width: 90%; max-width: 600px; margin: auto;">
                    <div class="panel-heading">
                        <h3 class="panel-title">
                            <span aria-hidden="true" class="glyphicon glyphicon-user"></span>&nbsp;&nbsp;&nbsp;<b>NEW USER REGISTRATION</b>
                        </h3>
                    </div>
                    <div class="row">&nbsp;</div>
                    <div class="panel-body">

                        <asp:HiddenField ID="HiddenField1" runat="server" Value="" />
                        <div class="form-group fontsizselabel row">
                            <label class="col-4 control-label" for="txtFullName">Full Name<sup style="color: red;">*</sup> :</label>
                            <div class="col-8" style="display: flex;">
                                <div class="input-group">
                                    <asp:TextBox runat="server" ID="txtFullName" placeholder="Enter Full Name" CssClass="form-control fontsizse" autocomplete="off" MaxLength="100" />
                                </div>
                            </div>
                        </div>
                        <div class="row">&nbsp;</div>


                        <div class="form-group fontsizselabel row">
                            <label class="col-4 control-label" for="txtAadhar">Aadhar Card No<sup style="color: red;">*</sup>:</label>
                            <div class="col-8" style="display: flex;">
                                <div class="input-group">
                                    <asp:TextBox runat="server" ID="txtAadhar" placeholder="Enter Aadhar No" CssClass="form-control fontsizse" MaxLength="12" />
                                </div>
                            </div>
                        </div>
                        <div class="row">&nbsp;</div>
                        <div class="form-group fontsizselabel row">
                            <label class="col-4 control-label" for="txtEmail">Email-ID<sup style="color: red;">*</sup> :</label>
                            <div class="col-8" style="display: flex;">
                                <div class="input-group" style="font-size: 1rem;">
                                    <asp:TextBox runat="server" ID="txtEmail" placeholder="Enter Email ID" MaxLength="50" CssClass="form-control fontsizse" />
                                </div>
                            </div>
                        </div>
                        <%--<div class="row">&nbsp;</div>
                        <div class="form-group fontsizselabel row">
                            <label class="col-4 control-label" for="txtEmail">Advertisement No.:</label>
                            <div class="col-8" style="display: flex;">
                                <div class="input-group" style="font-size: 1rem;">
                                    <asp:TextBox runat="server" ID="txtAdv" placeholder="Adv ID" MaxLength="50" CssClass="form-control fontsizse" />
                                </div>
                            </div>
                        </div>--%>

                        <asp:Button ID="getOTP" runat="server" Text="Get OTP" CssClass="btn btn-secondary" OnClick="btnGetOtp_Click" OnClientClick="return validateRegisterForm();" />

                        <div class="form-group fontsizselabel row">
                            <label class="col-4 control-label" for="txtOtp">OTP<sup style="color: red;">*</sup> :</label>
                            <div class="col-8" style="display: flex;">
                                <div class="input-group">
                                    <asp:TextBox runat="server" ID="txtOtp" placeholder="Enter OTP" CssClass="form-control fontsizse" MaxLength="4" Enabled="false" />
                                </div>
                            </div>
                        </div>

                        <div class="row">&nbsp;</div>
                        <div class="form-group fontsizselabel row">
                            <label class="col-4 control-label" for="txtPass">New Password<sup style="color: red;">*</sup> :</label>
                            <div class="col-8" style="display: flex;">
                                <div class="input-group">
                                    <asp:TextBox runat="server" ID="txtPass" placeholder="Create Strong Password" TextMode="Password" MaxLength="32" CssClass="form-control fontsizse" Enabled="false" />
                                </div>
                            </div>
                        </div>
                        <div class="row">&nbsp;</div>

                        <div class="row">
                            <div class="col-md-12 fontsizselabel">
                                <asp:Label ID="lblRegisterError" runat="server" Font-Bold="true" ForeColor="Firebrick"></asp:Label>
                            </div>
                        </div>
                        <asp:Button ID="btnRegister" runat="server" Text="Verify & Register" CssClass="btn btn-success" OnClick="btnRegister_Click" OnClientClick="return validatePassword();" Enabled="false" />
                    </div>
                </div>
            </div>
        </div>
    </form>

    <div class="row">&nbsp;</div>
    <div class="row">&nbsp;</div>
    <div class="container-fluid well" style="margin-bottom: 0; position: fixed; left: 0; bottom: 0; width: 100%; background-color: #e6f9ff; font-size: 1.4rem;">
        <div class="container-fluid row">
            <div class="col-1"></div>
            <div class="col-10" style="text-align: center;">
                <b>Developed By: ENGINEERS INDIA LIMITED (EIL)</b>
            </div>
            <div class="col-1"></div>
        </div>
    </div>
</body>
</html>
