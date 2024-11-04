using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;

public partial class Status : Page {
    BindControls bc = new BindControls();
    protected void Page_Load(object sender, EventArgs e) {
        if (!IsPostBack) {
            if (Session["AadharNo"] != null) {
                BindGrid();
            } 
        }
    }

    private void BindGrid() {
        string query = "SELECT EA.REGCODE, G.GAME_DESC, EA.GAME_CAT, EA.UPD_DATE, EA.APP_STATUS FROM EILSPORT_APPMAIN EA JOIN EILSPORT_GAMEDIR G ON EA.GAME_APPLIED = G.GAME_CODE WHERE EA.AADHARNO = '" + Session["AadharNo"] + "' AND EA.EMAIL = '" + Session["EmailID"] + "'";
        DataTable dt = bc.getDetailsInDataTable(query);
        gvApplications.DataSource = dt;
        gvApplications.DataBind();
    }

    protected void btnAction_Click(object sender, EventArgs e) {
        LinkButton btnAction = (LinkButton)sender;
        string regCode = btnAction.CommandArgument;
        string commandName = btnAction.CommandName;

        if (commandName == "Edit") {
            Response.Redirect("Apply.aspx?regCode=" + regCode);
        }
        else if (commandName == "Download") {
            GeneratePDF(regCode);
            Response.Redirect("Apply.aspx?regCode=" + regCode);
        }
    }

    protected void gvApplications_RowDataBound(object sender, GridViewRowEventArgs e) {
        if (e.Row.RowType == DataControlRowType.DataRow)  {

            LinkButton btnAction = (LinkButton)e.Row.FindControl("btnAction");

            string status = DataBinder.Eval(e.Row.DataItem, "APP_STATUS").ToString();

            if (status == "DRAFT") {
                btnAction.Text = "Edit";
                btnAction.CommandName = "Edit";
            }
            else if (status == "SUBMIT") {
                btnAction.Text = "Download";
                btnAction.CommandName = "Download";
            }
        }
    }

    private void GeneratePDF(string applicationId) {

        ReportDocument crystalReport = new ReportDocument();
        RunReport rn = new RunReport();
        rn.Param1_name = "AADHAR";
        rn.Param1_value = Session["AadharNo"].ToString();
        rn.Param2_name = "REGCODE";
        rn.Param2_value = applicationId;

        string imagePath = Server.MapPath("~/App_Data/" + applicationId + "/Photograph.jpg");  // or .jpeg if applicable
        rn.Param3_name = "Photograph";
        rn.Param3_value = imagePath;

        string signPath = Server.MapPath("~/App_Data/" + applicationId + "/Signature.jpg");  // or .jpeg if applicable
        rn.Param4_name = "Signature";
        rn.Param4_value = signPath;

        string repPth = Server.MapPath("~/CrystalReport.rpt");
        rn.Report(repPth, 1, true, "EIL_" + applicationId, Response);
    }

    public static byte[] ReadFully(Stream input) {
        using (MemoryStream ms = new MemoryStream()) {
            input.CopyTo(ms);
            return ms.ToArray();
        }
    }
}