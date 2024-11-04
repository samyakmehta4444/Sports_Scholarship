<%@ Page Title="Sports Admin Window" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="Admin.aspx.cs" Inherits="Admin" %>

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
                background-color: rgba(0,0,0,0.01);
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

    <script>

    </script>

    <div class="row mt-4 mb-4">
        <div class="col-12 text-center" style="font-size: 15px;">
            <asp:Button ID="ExportExcelButton" runat="server" Text="Export to Excel" OnClick="ExportToExcel_Click" CssClass="btn btn-success btn-lg rounded-pill mr-3" />
            <%--<asp:Button ID="ExportCSVButton" runat="server" Text="Export to CSV" OnClick="ExportToCSV_Click" CssClass="btn btn-primary btn-lg rounded-pill mr-3" />--%>
        </div>
    </div>

    <asp:GridView ID="ApplicantsGridView" runat="server" AutoGenerateColumns="False" AllowPaging="True" PageSize="15" CssClass="gridview">
        <EmptyDataTemplate>
            <div class="no-data">No Existing Applications Found !</div>
        </EmptyDataTemplate>
        <Columns>
            <asp:BoundField DataField="FullName" HeaderText="Applicant Name" />
            <asp:BoundField DataField="APPLY_GAME" HeaderText="Scholarship For" />
            <asp:BoundField DataField="DOB" HeaderText="Date of Birth" />
            <asp:BoundField DataField="GAME_CAT" HeaderText="Participation Category" />
            <asp:BoundField DataField="MOBILENO" HeaderText="Mobile No." />
            <asp:BoundField DataField="Email" HeaderText="Email-ID" />
            <asp:BoundField DataField="RegCode" HeaderText="Registration No." />
            <asp:BoundField DataField="AadharNo" HeaderText="Aadhaar No." />
            <asp:TemplateField HeaderText="Documents">
                <ItemTemplate>
                    <asp:LinkButton ID="DownloadPDF" runat="server" Text="|Application|" CommandArgument='<%# Eval("RegCode") %>' OnClick="DownloadPDF_Click"></asp:LinkButton>
                    <asp:LinkButton ID="aadhar" runat="server" Text="|Aadhar|" CommandArgument='<%# Eval("RegCode") %>' OnClick="DownloadAadhar_Click"></asp:LinkButton>
                    <asp:LinkButton ID="photo" runat="server" Text="|Photo|" CommandArgument='<%# Eval("RegCode") %>' OnClick="DownloadPhoto_Click"></asp:LinkButton>
                    <asp:LinkButton ID="sign" runat="server" Text="|Sign|" CommandArgument='<%# Eval("RegCode") %>' OnClick="DownloadSign_Click"></asp:LinkButton>
                    <asp:LinkButton ID="ageProof" runat="server" Text="|AgeProof|" CommandArgument='<%# Eval("RegCode") %>' OnClick="DownloadProof_Click"></asp:LinkButton>
                    <asp:LinkButton ID="perform1" runat="server" Text="|Certificate_1|" CommandArgument='<%# Eval("RegCode") %>' OnClick="DownloadCerti1_Click"></asp:LinkButton>
                    <asp:LinkButton ID="perform2" runat="server" Text="|Certificate_2|" CommandArgument='<%# Eval("RegCode") %>' OnClick="DownloadCerti2_Click"></asp:LinkButton>
                    <asp:LinkButton ID="rank" runat="server" Text="|Ranking|" CommandArgument='<%# Eval("RegCode") %>' OnClick="DownloadRank_Click"></asp:LinkButton>
                    <asp:LinkButton ID="achieve1" runat="server" Text="|Achievement_1|" CommandArgument='<%# Eval("RegCode") %>' OnClick="DownloadAchieve1_Click"></asp:LinkButton>
                    <asp:LinkButton ID="achieve2" runat="server" Text="|Achievement_2|" CommandArgument='<%# Eval("RegCode") %>' OnClick="DownloadAchieve2_Click"></asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>

</asp:Content>
