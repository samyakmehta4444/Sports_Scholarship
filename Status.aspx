<%@ Page Title="Application Status" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="Status.aspx.cs" Inherits="Status" %>

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

    <div class="form-container" style="text-align: center;">
        <h1 style="color: #4e97ca;"><b>APPLICATION STATUS</b></h1>

        <div class="container-fluid row" style="font-size: 14px;">
            <asp:GridView ID="gvApplications" runat="server" AutoGenerateColumns="False" CssClass="gridview" OnRowDataBound="gvApplications_RowDataBound">
                <EmptyDataTemplate>
                    <div class="no-data">No Existing Applications Found, Click on ✏️APPLY Tab to Fill Details!</div>
                </EmptyDataTemplate>
                <Columns>
                    <asp:BoundField DataField="REGCODE" HeaderText="Registration Code" />
                    <asp:BoundField DataField="GAME_DESC" HeaderText="Game Applied" />
                    <asp:BoundField DataField="GAME_CAT" HeaderText="Game Category" />
                    <asp:BoundField DataField="UPD_DATE" HeaderText="Upload Date" DataFormatString="{0:dd MMMM yyyy}" />
                    <asp:BoundField DataField="APP_STATUS" HeaderText="Status" />
                    <asp:TemplateField HeaderText="Application">
                        <ItemTemplate>
                            <asp:LinkButton ID="btnAction" runat="server" CommandArgument='<%# Eval("REGCODE") %>' OnClick="btnAction_Click"></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>
</asp:Content>
