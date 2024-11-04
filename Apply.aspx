<%@ Page Title="Form Filling" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="Apply.aspx.cs" Inherits="Apply" MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        .form-container {
            margin-top: 20px;
            background-color: white;
            padding: 20px;
            border-radius: 10px;
            box-shadow: 0px 0px 10px rgba(0,0,0,0.1);
            font-size: 14px;
        }

        .gradient-bar {
            background: linear-gradient(to right, rgba(0, 0, 0, 1), rgba(0, 0, 0, 0));
            text-align: center;
            padding: 13px;
            margin-top: 20px;
        }

            .gradient-bar h1 {
                color: white;
                font-weight: bold;
                margin: 0;
                font-size: 15px;
            }

        .gridview {
            width: 100%;
            margin-top: 10px;
            border-collapse: collapse;
            border-spacing: 5px;
        }

            .gridview th {
                background-color: #4e97ca;
                color: white;
                padding: 10px;
                text-align: center;
                font-size: 15px;
                border: 1px solid #ddd; /* Border for cells */
            }

            .gridview td {
                background-color: rgba(0,0,0,0.02);
                padding: 5px;
                text-align: center;
                font-size: 13px;
                border: 1px solid #ddd; /* Border for cells */
            }

        .input-field,
        .dropdown-field,
        .textbox-time {
            font-size: 13px;
            font-weight: bold;
        }

        .col-form-label {
            font-size: 14px;
        }

        .custom-margin {
            margin-left: 1px;
        }
    </style>

    <script type="text/javascript">
        function validateForm() {
            var allow = true;
            var msg = "Please Check & Fill the Following Fields before Submitting :\n";

            var photo = document.getElementById('<%= photoUpload.ClientID %>').value;
            var signature = document.getElementById('<%= signatureUpload.ClientID %>').value;
            var ageProof = document.getElementById('<%= ageProofUpload.ClientID %>').value;
            var aadhar = document.getElementById('<%= aadharUpload.ClientID %>').value;

            function isPdf(fileName) {
                var extension = fileName.split('.').pop().toLowerCase();
                return extension === "pdf";
            }

            if (photo === "" || signature === "" || ageProof === "" || aadhar === "") {
                allow = false;
                msg += "All the files (Photograph, Aadhar Card, Age-Proof, and Signature) are required to be uploaded before submitting!\n";
            }

            function isJpeg(fileName) {
                var ext = fileName.substring(fileName.lastIndexOf('.') + 1).toLowerCase();
                return ext === "jpeg" || ext === "jpg";
            }

            if (photo !== "" && !isJpeg(photo)) {
                allow = false;
                msg += "Photograph must be in JPEG / JPG format.\n";
            }

            if (signature !== "" && !isJpeg(signature)) {
                allow = false;
                msg += "Signature must be in JPEG / JPG format.\n";
            }
            if (ageProof !== "" && !isPdf(ageProof)) {
                allow = false;
                msg += "Age Proof must be a PDF file.\n";
            }
            if (aadhar !== "" && !isPdf(aadhar)) {
                allow = false;
                msg += "Aadhar Card must be a PDF file.\n";
            }

            var performanceCert1 = document.getElementById('<%= performanceCertUpload1.ClientID %>').value;
            var performanceCert2 = document.getElementById('<%= performanceCertUpload2.ClientID %>').value;
            var rankingCert = document.getElementById('<%= rankingCertUpload.ClientID %>').value;

            if (performanceCert1 !== "" && !isPdf(performanceCert1)) {
                allow = false;
                msg += "Performance Certificate 1 must be a PDF file.\n";
            }
            if (performanceCert2 !== "" && !isPdf(performanceCert2)) {
                allow = false;
                msg += "Performance Certificate 2 must be a PDF file.\n";
            }
            if (rankingCert !== "" && !isPdf(rankingCert)) {
                allow = false;
                msg += "Ranking Certificate must be a PDF file.\n";
            }

            var identifier = document.getElementById('<%= identifier.ClientID %>').value;
            var firstName = document.getElementById('<%= firstName.ClientID %>').value.trim();
            var lastName = document.getElementById('<%= lastName.ClientID %>').value.trim();
            if (identifier === "" || firstName === "" || lastName === "") {
                allow = false;
                msg += "Please Enter Your Full Name !\n";
            }

            var fFirstName = document.getElementById('<%= fFirstName.ClientID %>').value.trim();
            var fLastName = document.getElementById('<%= fLastName.ClientID %>').value.trim();
            if (fFirstName === "" || fLastName === "") {
                allow = false;
                msg += "Please Enter Your Father's Full Name !\n";
            }

            var mFirstName = document.getElementById('<%= mFirstName.ClientID %>').value.trim();
            var mLastName = document.getElementById('<%= mLastName.ClientID %>').value.trim();
            if (mFirstName === "" || mLastName === "") {
                allow = false;
                msg += "Please Enter Your Mother's Full Name !\n";
            }

            var dob = document.getElementById('<%= dob.ClientID %>').value.trim();
            if (dob === "") {
                allow = false;
                msg += "Please Enter Your Date of Birth !\n";
            } else {
                var dobDate = new Date(dob);
                var today = new Date();
                if (dobDate >= today) {
                    allow = false;
                    msg += "Date of Birth must be before Today's Date!\n";
                }
            }

            var mobile = document.getElementById('<%= mobile.ClientID %>').value.trim();
            if (mobile === "") {
                allow = false;
                msg += "Please Enter Your Mobile No. !\n";
            } else if (!/^\d{10}$/.test(mobile)) {  // Checks if mobile is numeric and 10 digits long
                allow = false;
                msg += "Mobile number must be exactly 10 digits long and numeric!\n";
            }

            var email = document.getElementById('<%= email.ClientID %>').value.trim();
            if (email === "") {
                allow = false;
                msg += "Please Enter Your E-Mail ID !\n";
            } else {
                var emailPattern = /^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/;
                if (!emailPattern.test(email)) {
                    allow = false;
                    msg += "Please Enter a Valid E-Mail ID used During Registration !\n";
                }
            }

            var address = document.getElementById('<%= address.ClientID %>').value.trim();
            var city = document.getElementById('<%= city.ClientID %>').value.trim();
            var state = document.getElementById('<%= state.ClientID %>').value.trim();
            var pinCode = document.getElementById('<%= pinCode.ClientID %>').value.trim();
            if (address === "") {
                allow = false;
                msg += "Please Enter Your Permanent Address !\n";
            }
            if (city === "") {
                allow = false;
                msg += "Please Enter Your City !\n";
            }
            if (state === "") {
                allow = false;
                msg += "Please Enter Your State !\n";
            }
            if (pinCode === "") {
                allow = false;
                msg += "Please Enter City PIN Code !\n";
            }

            var aadharNo = document.getElementById('<%= aadharNo.ClientID %>').value.trim();
            var gameList = document.getElementById('<%= gameList.ClientID %>').value;
            var catOfParticipation = document.getElementById('<%= catOfParticipation.ClientID %>').value;
            var otherScholarShip = document.getElementById('<%= otherScholarShip.ClientID %>').value;
            if (aadharNo === "") {
                allow = false;
                msg += "Please Enter Same Aadhar As Used During Registration !\n";
            }
            if (gameList === "") {
                allow = false;
                msg += "Please Select Game for which you're applying Scholarship !\n";
            }
            if (catOfParticipation === "") {
                allow = false;
                msg += "Please Select Your Category of Participation !\n";
            }
            if (otherScholarShip === "") {
                allow = false;
                msg += "Please Select Whether you're applying for any other Scholarship !\n";
            }

            var examPass = document.getElementById('<%= examPass.ClientID %>').value.trim();
            var passYear = document.getElementById('<%= passYear.ClientID %>').value;
            var school_Uni = document.getElementById('<%= school_Uni.ClientID %>').value;
            if (examPass === "") {
                allow = false;
                msg += "Please Select Your Highest Examination Passed !\n";
            }
            if (passYear === "") {
                allow = false;
                msg += "Please Select Year of Passing Highest Examination !\n";
            }
            if (school_Uni === "") {
                allow = false;
                msg += "Please Enter Your School / University Name !\n";
            }

            document.getElementById('<%= achievement.ClientID %>').value = "False";

            var grid = document.getElementById('<%= gvScholarship.ClientID %>'); // Get GridView by ClientID
            var rows = grid.getElementsByTagName('tr'); // Get all rows in the GridView

            for (var i = 1; i < rows.length; i++) { // Start from 1 to skip the header row
                var row = rows[i];
                var inputs = row.querySelectorAll('input[type="text"], input[type="date"], select'); // Get all inputs and dropdowns
                var hasValue = false; // To check if any field in the row has a value
                var allEmpty = true; // To check if all fields in the row are empty

                for (var j = 0; j < inputs.length; j++) {
                    var input = inputs[j];
                    if ((input.type === 'text' || input.type === 'date') && input.value.trim() !== "") {
                        hasValue = true;
                        allEmpty = false; // Row has some value, mark it
                    } else if (input.tagName === 'SELECT' && input.value !== "") {
                        hasValue = true;
                        allEmpty = false;
                    }
                }

                if (allEmpty) {
                    continue;
                }
                else {
                    document.getElementById('<%= achievement.ClientID %>').value = "True";
                }

                if (hasValue) {
                    for (var j = 0; j < inputs.length; j++) {
                        var input = inputs[j];
                        if ((input.type === 'text' || input.type === 'date') && input.value.trim() === "") {
                            document.getElementById('<%= achievement.ClientID %>').value = "False";
                            msg += "Please fill all fields in row " + (i) + " of Sports Achievement(s) before submitting!\n";
                            allow = false;
                            break;
                        } else if (input.tagName === 'SELECT' && input.value === "") {
                            document.getElementById('<%= achievement.ClientID %>').value = "False";
                            msg += "Please select all dropdown values in row " + (i) + " of Sports Achievement(s) before submitting!\n";
                            allow = false;
                            break;
                        }
                        else document.getElementById('<%= achievement.ClientID %>').value = "True";
                    }
                }
                if (!allow) break; // Stop validation if a row is invalid
            }

            var declare = document.getElementById('<%= declare.ClientID %>');
            if (!declare.checked) {  // Check if the checkbox is not checked
                allow = false;
                msg += "Please Tick & Accept Declaration Before Proceeding !\n";
            }

            if (!allow) {
                alert(msg);
                return false;
            }
            return true;
        }
    </script>

    <div class="form-container" style="text-align: center;">
        <h1 style="color: #4e97ca;"><b>BASIC INFORMATION</b></h1>
        <div class="row">
            &nbsp;
        </div>
        <div class="container-fluid row" style="font-size: 14px;">
            <div class="col-2">Name<sup style="color: red;">*</sup> :</div>
            <div class="col-1">
                <asp:DropDownList ID="identifier" runat="server" CssClass="form-control input-field">
                    <asp:ListItem Text="--SELECT--" Value=""></asp:ListItem>
                    <asp:ListItem Text="Mr." Value="MR"></asp:ListItem>
                    <asp:ListItem Text="Mrs." Value="MRS"></asp:ListItem>
                    <asp:ListItem Text="Dr." Value="DR"></asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="col-3">
                <asp:TextBox ID="firstName" runat="server" CssClass="form-control input-field" Placeholder="First Name" MaxLength="50"></asp:TextBox>
            </div>
            <div class="col-3">
                <asp:TextBox ID="midName" runat="server" CssClass="form-control input-field" Placeholder="Middle Name" MaxLength="50"></asp:TextBox>
            </div>
            <div class="col-3">
                <asp:TextBox ID="lastName" runat="server" CssClass="form-control input-field" Placeholder="Last Name" MaxLength="50"></asp:TextBox>
            </div>
        </div>
        <div class="row">
            &nbsp;
        </div>

        <div class="container-fluid row" style="font-size: 14px;">
            <div class="col-3">Father's Name<sup style="color: red;">*</sup> :</div>

            <div class="col-3">
                <asp:TextBox ID="fFirstName" runat="server" CssClass="form-control input-field" Placeholder="Father's First Name" MaxLength="50"></asp:TextBox>
            </div>
            <div class="col-3">
                <asp:TextBox ID="fMidName" runat="server" CssClass="form-control input-field" Placeholder="Father's Middle Name" MaxLength="50"></asp:TextBox>
            </div>
            <div class="col-3">
                <asp:TextBox ID="fLastName" runat="server" CssClass="form-control input-field" Placeholder="Father's Last Name" MaxLength="50"></asp:TextBox>
            </div>
        </div>
        <div class="row">
            &nbsp;
        </div>

        <div class="container-fluid row" style="font-size: 14px;">
            <div class="col-3">Mother's Name<sup style="color: red;">*</sup> :</div>
            <div class="col-3">
                <asp:TextBox ID="mFirstName" runat="server" CssClass="form-control input-field" Placeholder="Mother's First Name" MaxLength="50"></asp:TextBox>
            </div>
            <div class="col-3">
                <asp:TextBox ID="mMidName" runat="server" CssClass="form-control input-field" Placeholder="Mother's Middle Name" MaxLength="50"></asp:TextBox>
            </div>
            <div class="col-3">
                <asp:TextBox ID="mLastName" runat="server" CssClass="form-control input-field" Placeholder="Mother's Last Name" MaxLength="50"></asp:TextBox>
            </div>
        </div>
        <div class="row">
            &nbsp;
        </div>

        <div class="container-fluid row" style="font-size: 14px;">
            <div class="col-3">Date of Birth<sup style="color: red;">*</sup> :</div>
            <div class="col-2">
                <asp:TextBox ID="dob" runat="server" CssClass="form-control input-field" TextMode="Date"></asp:TextBox>
            </div>
            <div class="col-3">Mobile No.<sup style="color: red;">*</sup>: <b>+ 91</b></div>
            <div class="col-2">
                <asp:TextBox ID="mobile" runat="server" CssClass="form-control input-field" Placeholder="Mobile Number" MaxLength="10"></asp:TextBox>
            </div>

        </div>
        <div class="row">
            &nbsp;
        </div>

        <div class="container-fluid row" style="font-size: 14px;">
            <div class="col-3"><b>E-Mail ID</b><sup style="color: red;">*</sup> : <i style="color: red;">(Used for Registration)</i></div>
            <div class="col-2">
                <asp:TextBox ID="email" runat="server" CssClass="form-control input-field" MaxLength="50" Placeholder="Enter E-Mail ID" Enabled="false"></asp:TextBox>
            </div>
            <div class="col-3">Permanent Address<sup style="color: red;">*</sup> :</div>
            <div class="col-4">
                <asp:TextBox ID="address" runat="server" CssClass="form-control input-field" TextMode="MultiLine" MaxLength="300" Placeholder="Enter Permanent Address"></asp:TextBox>
            </div>
        </div>
        <div class="row">
            &nbsp;
        </div>

        <div class="container-fluid row" style="font-size: 14px;">
            <div class="col-1"></div>
            <div class="col-1">City<sup style="color: red;">*</sup> :</div>
            <div class="col-2">
                <asp:TextBox ID="city" runat="server" CssClass="form-control input-field" MaxLength="50" Placeholder="Enter City"></asp:TextBox>
            </div>
            <div class="col-2">State<sup style="color: red;">*</sup> :</div>
            <div class="col-2">
                <asp:TextBox ID="state" runat="server" CssClass="form-control input-field" MaxLength="50" Placeholder="Enter State"></asp:TextBox>
            </div>
            <div class="col-2">PIN Code<sup style="color: red;">*</sup> :</div>
            <div class="col-2">
                <asp:TextBox ID="pinCode" runat="server" CssClass="form-control input-field" MaxLength="6" Placeholder="Enter PIN Code"></asp:TextBox>
            </div>
        </div>
        <div class="row">
            &nbsp;
        </div>

        <div class="container-fluid row" style="font-size: 14px;">
            <div class="col-3"><b>Aadhar No.</b><sup style="color: red;">*</sup>: <i style="color: red;">(Used for Registration)</i></div>
            <div class="col-2">
                <asp:TextBox ID="aadharNo" runat="server" CssClass="form-control input-field" MaxLength="12" Placeholder="Enter Aadhar Number" Enabled="false"></asp:TextBox>
            </div>
            <div class="col-5">Applying Scholarship for Game<sup style="color: red;">*</sup> :</div>
            <div class="col-2">
                <asp:DropDownList ID="gameList" runat="server" CssClass="form-control input-field"></asp:DropDownList>
            </div>
        </div>
        <div class="row">
            &nbsp;
        </div>

        <div class="container-fluid row" style="font-size: 14px;">
            <div class="col-3">Category of Participation<sup style="color: red;">*</sup> :</div>
            <div class="col-2">
                <asp:DropDownList ID="catOfParticipation" runat="server" CssClass="form-control input-field">
                    <asp:ListItem Text="--SELECT CATEGORY--" Value=""></asp:ListItem>
                    <asp:ListItem Text="Junior" Value="Junior"></asp:ListItem>
                    <asp:ListItem Text="Senior" Value="Senior"></asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="col-5">Whether Receiving any Scholarship from any other source<sup style="color: red;">*</sup> :</div>
            <div class="col-2">
                <asp:DropDownList ID="otherScholarShip" runat="server" CssClass="form-control input-field">
                    <asp:ListItem Text="--SELECT--" Value=""></asp:ListItem>
                    <asp:ListItem Text="Yes" Value="Yes"></asp:ListItem>
                    <asp:ListItem Text="No" Value="No"></asp:ListItem>
                </asp:DropDownList>
            </div>
        </div>
        <div class="row">
            &nbsp;
        </div>
        <div class="row">
            &nbsp;
        </div>

        <div class="container-fluid row" style="font-size: 15px;">
            <h1 style="color: #4e97ca;"><b>ACADEMIC ACHIEVEMENTS</b></h1>
        </div>

        <div class="row">
            &nbsp;
        </div>

        <div class="container-fluid row" style="font-size: 14px;">
            <div class="col-2">Exam Passed<sup style="color: red;">*</sup> :</div>
            <div class="col-2">
                <asp:DropDownList ID="examPass" runat="server" CssClass="form-control input-field"></asp:DropDownList>
            </div>
            <div class="col-2">Passing Year<sup style="color: red;">*</sup> :</div>
            <div class="col-1">
                <asp:DropDownList ID="passYear" runat="server" CssClass="form-control input-field">
                    <asp:ListItem Text="--SELECT--" Value=""></asp:ListItem>
                    <asp:ListItem Text="2024" Value="2024"></asp:ListItem>
                    <asp:ListItem Text="2023" Value="2023"></asp:ListItem>
                    <asp:ListItem Text="2022" Value="2022"></asp:ListItem>
                    <asp:ListItem Text="2021" Value="2021"></asp:ListItem>
                    <asp:ListItem Text="2020" Value="2020"></asp:ListItem>
                    <asp:ListItem Text="2019" Value="2019"></asp:ListItem>
                    <asp:ListItem Text="2018" Value="2018"></asp:ListItem>
                    <asp:ListItem Text="2017" Value="2017"></asp:ListItem>
                    <asp:ListItem Text="2016" Value="2016"></asp:ListItem>
                    <asp:ListItem Text="2015" Value="2015"></asp:ListItem>
                    <asp:ListItem Text="2014" Value="2014"></asp:ListItem>
                    <asp:ListItem Text="2013" Value="2013"></asp:ListItem>
                    <asp:ListItem Text="2012" Value="2012"></asp:ListItem>
                    <asp:ListItem Text="2011" Value="2011"></asp:ListItem>
                    <asp:ListItem Text="2010" Value="2010"></asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="col-2">School  / University<sup style="color: red;">*</sup> :</div>
            <div class="col-3">
                <asp:TextBox ID="school_Uni" runat="server" CssClass="form-control input-field" TextMode="MultiLine" Placeholder="Enter School / University"></asp:TextBox>
            </div>
        </div>
    </div>
    <div class="row">
        &nbsp;
    </div>
    <div class="row">
        &nbsp;
    </div>

    <div class="container-fluid row" style="font-size: 15px;">
        <b>SPORTS ACHIEVEMENTS <i style="color: red;">(Maximum of 2 Best Distinct Performances)</i> :</b>
        <asp:GridView ID="gvScholarship" runat="server" AutoGenerateColumns="False" CssClass="gridview" OnRowDataBound="gvScholarship_RowDataBound">
            <Columns>
                <asp:TemplateField HeaderText="Sports">
                    <ItemTemplate>
                        <asp:DropDownList ID="ddlSports" runat="server" CssClass="form-control input-field" AutoPostBack="True" OnSelectedIndexChanged="ddlSports_SelectedIndexChanged"></asp:DropDownList>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Events">
                    <ItemTemplate>
                        <asp:DropDownList ID="ddlEvents" runat="server" CssClass="form-control input-field" AutoPostBack="True"></asp:DropDownList>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Type">
                    <ItemTemplate>
                        <asp:DropDownList ID="ddlType" runat="server" CssClass="form-control input-field" AutoPostBack="True" OnSelectedIndexChanged="ddlType_SelectedIndexChanged">
                            <asp:ListItem Text="--TYPE--" Value=""></asp:ListItem>
                            <asp:ListItem Text="National" Value="1"></asp:ListItem>
                            <asp:ListItem Text="International" Value="2"></asp:ListItem>
                        </asp:DropDownList>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Tournament">
                    <ItemTemplate>
                        <asp:DropDownList ID="ddlTournamentName" runat="server" AutoPostBack="True" CssClass="form-control input-field"></asp:DropDownList>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Venue">
                    <ItemTemplate>
                        <asp:TextBox ID="txtVenue" runat="server" CssClass="form-control input-field"></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Position">
                    <ItemTemplate>
                        <asp:DropDownList ID="ddlPositionLevel" runat="server" AutoPostBack="True" CssClass="form-control input-field"></asp:DropDownList>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Unit">
                    <ItemTemplate>
                        <asp:DropDownList ID="ddlUnit" runat="server" CssClass="form-control input-field">
                            <asp:ListItem Text="--UNIT--" Value=""></asp:ListItem>
                        </asp:DropDownList>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Results">
                    <ItemTemplate>
                        <asp:TextBox ID="txtResults" runat="server" CssClass="form-control input-field"></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="GameDate">
                    <ItemTemplate>
                        <asp:TextBox ID="txtDate" runat="server" TextMode="Date" CssClass="form-control input-field"></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Upload Relevant PDF">
                    <ItemTemplate>
                        <asp:FileUpload ID="fileUpload" runat="server" accept="application/pdf" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <asp:HiddenField ID="achievement" runat="server" />
    </div>

    <div class="row">
        &nbsp;
    </div>

    <div class="row">
        &nbsp;
    </div>
    <div class="container-fluid row" style="font-size: 15px;">
        <b>UPLOAD DOCUMENTS <i style="color: red;">(Size < 2 MB)</i> :</b>
    </div>

    <div class="row">
        &nbsp;
    </div>

    <div class="form-group container-fluid row" style="font-size: 14px;">
        <div class="col-1">Photograph<sup style="color: red;">JPEG</sup> :</div>
        <div class="col-2">
            <asp:FileUpload ID="photoUpload" runat="server" CssClass="form-control" accept="image/jpeg" />
        </div>
        <div class="col-1">Signature<sup style="color: red;">JPEG</sup> :</div>
        <div class="col-2">
            <asp:FileUpload ID="signatureUpload" runat="server" CssClass="form-control" accept="image/jpeg" />
        </div>
        <div class="col-1">Age Proof<sup style="color: red;">PDF</sup> :</div>
        <div class="col-2">
            <asp:FileUpload ID="ageProofUpload" runat="server" CssClass="form-control" accept="application/pdf" />
        </div>
        <div class="col-1">Aadhar Card<sup style="color: red;">PDF</sup> :</div>
        <div class="col-2">
            <asp:FileUpload ID="aadharUpload" runat="server" CssClass="form-control" accept="application/pdf" />
        </div>
    </div>

    <div class="form-group container-fluid row" style="font-size: 14px;">
        <div class="col-3"><b>Performance Certificate(s)</b>:   1.</div>
        <div class="col-2">
            <asp:FileUpload ID="performanceCertUpload1" runat="server" CssClass="form-control" accept="application/pdf" />
        </div>
        <div class="col-1">2.</div>
        <div class="col-2">
            <asp:FileUpload ID="performanceCertUpload2" runat="server" CssClass="form-control" accept="application/pdf" />
        </div>
        <div class="col-2">Ranking Certificate :</div>
        <div class="col-2">
            <asp:FileUpload ID="rankingCertUpload" runat="server" CssClass="form-control" accept="application/pdf" />
        </div>
    </div>
    <div class="row">
        &nbsp;
    </div>

    <div class="container-fluid row d-flex justify-content-center align-items-center" style="font-size: 16px;">
        <div class="col-2" style="font-size: 20px; text-align: center;">
            <b style="color: #4e97ca;">DECLARATION<sup style="color: red;">*</sup> : </b>
        </div>
        <div class="col-1">
            <asp:CheckBox ID="declare" runat="server" />
        </div>
        <div class="col-9">
            I hereby, declare that, to my best knowledge, all information provided for this application is true. In case of any false information, my candidature will be liable to be forfeited !
        </div>
    </div>

    <div class="row mt-4 mb-4">
        <div class="col-12 text-center" style="font-size: 15px;">
            <asp:Button ID="btnDraft" runat="server" Text="💾SAVE DRAFT" OnClick="btnDraft_Click" OnClientClick="return validateForm();" CssClass="btn btn-lg btn-warning rounded-pill mr-3" />
            <asp:Button ID="btnSubmit" runat="server" Text="✔️SUBMIT" OnClick="btnSubmit_Click" OnClientClick="return validateForm();" CssClass="btn btn-lg btn-primary rounded-pill mr-3" />
        </div>
    </div>
    <div class="row">
        &nbsp;
    </div>
    <div class="row">
        &nbsp;
    </div>
    <div class="row">
        &nbsp;
    </div>
    <div class="row">
        &nbsp;
    </div>
</asp:Content>
