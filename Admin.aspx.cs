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
using System.Text;

public partial class Admin : Page
{
    BindControls bc = new BindControls();
    protected void Page_Load(object sender, EventArgs e) {
        if (!IsPostBack) {
            BindApplicantsData();
        }
    }

    private void BindApplicantsData() {
        string sql = "SELECT (STITLE || ' ' || SFNAME || ' ' || SMNAME || ' ' || SLNAME) AS FullName," +
                     "(SELECT GAME_DESC FROM EILSPORT_GAMEDIR gm WHERE gm.GAME_CODE = GAME_APPLIED) AS APPLY_GAME," +
                     "TO_CHAR(DOB, 'DD-MM-YYYY') AS DOB, GAME_CAT," +
                     "TO_CHAR(MOBILENO, '9999999999') AS MOBILENO," +
                     "EMAIL, REGCODE, AADHARNO " +
                     "FROM EILSPORT_APPMAIN WHERE APP_STATUS = 'SUBMIT'";
        ApplicantsGridView.DataSource = bc.getDetailsInDataTable(sql);
        ApplicantsGridView.DataBind();
    }

    protected void DownloadPDF_Click(object sender, EventArgs e) {
        LinkButton btn = (LinkButton)(sender);
        string applicationId = btn.CommandArgument;
        string aadhar = "SELECT AADHARNO FROM EILSPORT_APPMAIN WHERE REGCODE = '" + applicationId + "'";
        ReportDocument crystalReport = new ReportDocument();
        RunReport rn = new RunReport();
        rn.Param1_name = "AADHAR";
        rn.Param1_value = bc.returnString(aadhar);
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

    protected void DownloadAadhar_Click(object sender, EventArgs e) {
        LinkButton linkButton = (LinkButton)sender;
        string regCode = linkButton.CommandArgument;

        string filePath = Server.MapPath("~/App_Data/" + regCode + "/AadharCard.pdf");

        if (System.IO.File.Exists(filePath)) {
            Response.Clear();
            Response.ContentType = "application/pdf";
            Response.AddHeader("Content-Disposition", "attachment; filename=AadharCard_" + regCode + ".pdf");
            Response.WriteFile(filePath);
            Response.Flush();
            Response.End();
        }
        else {
            Response.Write("<script>alert('Aadhar Card NOT found !');</script>");
        }
    }

    protected void DownloadPhoto_Click(object sender, EventArgs e)
    {
        LinkButton linkButton = (LinkButton)sender;
        string regCode = linkButton.CommandArgument;

        string filePath = Server.MapPath("~/App_Data/" + regCode + "/Photograph.jpg");

        if (!System.IO.File.Exists(filePath))
        {
            filePath = Server.MapPath("~/App_Data/" + regCode + "/Photograph.jpeg");
        }

        if (System.IO.File.Exists(filePath))
        {
            Response.Clear();
            Response.ContentType = "image/jpeg"; // Content type for jpg/jpeg images
            Response.AddHeader("Content-Disposition", "attachment; filename=Photograph_" + regCode + ".jpg");
            Response.WriteFile(filePath);
            Response.Flush();
            Response.End();
        }
        else
        {
            Response.Write("<script>alert('Photograph NOT found !');</script>");
        }
    }

    protected void DownloadSign_Click(object sender, EventArgs e)
    {
        LinkButton linkButton = (LinkButton)sender;
        string regCode = linkButton.CommandArgument;

        string filePath = Server.MapPath("~/App_Data/" + regCode + "/Signature.jpg");

        if (!System.IO.File.Exists(filePath))
        {
            filePath = Server.MapPath("~/App_Data/" + regCode + "/Signature.jpeg");
        }

        if (System.IO.File.Exists(filePath))
        {
            Response.Clear();
            Response.ContentType = "image/jpeg"; // Content type for jpg/jpeg images
            Response.AddHeader("Content-Disposition", "attachment; filename=Signature_" + regCode + ".jpg");
            Response.WriteFile(filePath);
            Response.Flush();
            Response.End();
        }
        else
        {
            Response.Write("<script>alert('Signature NOT found !');</script>");
        }
    }

    protected void DownloadProof_Click(object sender, EventArgs e)
    {
        LinkButton linkButton = (LinkButton)sender;
        string regCode = linkButton.CommandArgument;

        string filePath = Server.MapPath("~/App_Data/" + regCode + "/ProofOfAge.pdf");

        if (System.IO.File.Exists(filePath))
        {
            Response.Clear();
            Response.ContentType = "application/pdf";
            Response.AddHeader("Content-Disposition", "attachment; filename=AgeProof_" + regCode + ".pdf");
            Response.WriteFile(filePath);
            Response.Flush();
            Response.End();
        }
        else
        {
            Response.Write("<script>alert('Age Proof NOT Found !');</script>");
        }
    }

    protected void DownloadCerti1_Click(object sender, EventArgs e)
    {
        LinkButton linkButton = (LinkButton)sender;
        string regCode = linkButton.CommandArgument;

        string filePath = Server.MapPath("~/App_Data/" + regCode + "/PerformanceCert1.pdf");

        if (System.IO.File.Exists(filePath))
        {
            Response.Clear();
            Response.ContentType = "application/pdf";
            Response.AddHeader("Content-Disposition", "attachment; filename=Performance_Certificate_1_" + regCode + ".pdf");
            Response.WriteFile(filePath);
            Response.Flush();
            Response.End();
        }
        else
        {
            Response.Write("<script>alert('Performance Certificate 1 NOT Uploaded !');</script>");
        }
    }

    protected void DownloadCerti2_Click(object sender, EventArgs e)
    {
        LinkButton linkButton = (LinkButton)sender;
        string regCode = linkButton.CommandArgument;

        string filePath = Server.MapPath("~/App_Data/" + regCode + "/PerformanceCert2.pdf");

        if (System.IO.File.Exists(filePath))
        {
            Response.Clear();
            Response.ContentType = "application/pdf";
            Response.AddHeader("Content-Disposition", "attachment; filename=Performance_Certificate_2_" + regCode + ".pdf");
            Response.WriteFile(filePath);
            Response.Flush();
            Response.End();
        }
        else
        {
            Response.Write("<script>alert('Performance Certificate 2 NOT Uploaded !');</script>");
        }
    }

    protected void DownloadRank_Click(object sender, EventArgs e)
    {
        LinkButton linkButton = (LinkButton)sender;
        string regCode = linkButton.CommandArgument;

        string filePath = Server.MapPath("~/App_Data/" + regCode + "/RankingCertificate.pdf");

        if (System.IO.File.Exists(filePath))
        {
            Response.Clear();
            Response.ContentType = "application/pdf";
            Response.AddHeader("Content-Disposition", "attachment; filename=Ranking_Certificate_" + regCode + ".pdf");
            Response.WriteFile(filePath);
            Response.Flush();
            Response.End();
        }
        else
        {
            Response.Write("<script>alert('Ranking Certificate NOT Uploaded !');</script>");
        }
    }

    protected void DownloadAchieve1_Click(object sender, EventArgs e)
    {
        LinkButton linkButton = (LinkButton)sender;
        string regCode = linkButton.CommandArgument;

        string filePath = Server.MapPath("~/App_Data/" + regCode + "/Achievement_1.pdf");

        if (System.IO.File.Exists(filePath))
        {
            Response.Clear();
            Response.ContentType = "application/pdf";
            Response.AddHeader("Content-Disposition", "attachment; filename=Achievement_1_" + regCode + ".pdf");
            Response.WriteFile(filePath);
            Response.Flush();
            Response.End();
        }
        else
        {
            Response.Write("<script>alert('Achievement 1 NOT Uploaded !');</script>");
        }
    }

    protected void DownloadAchieve2_Click(object sender, EventArgs e)
    {
        LinkButton linkButton = (LinkButton)sender;
        string regCode = linkButton.CommandArgument;

        string filePath = Server.MapPath("~/App_Data/" + regCode + "/Achievement_2.pdf");

        if (System.IO.File.Exists(filePath))
        {
            Response.Clear();
            Response.ContentType = "application/pdf";
            Response.AddHeader("Content-Disposition", "attachment; filename=Achievement_2_" + regCode + ".pdf");
            Response.WriteFile(filePath);
            Response.Flush();
            Response.End();
        }
        else
        {
            Response.Write("<script>alert('Achievement 2 NOT Uploaded !');</script>");
        }
    }

    protected void ExportToExcel_Click(object sender, EventArgs e) {
        string sql = "SELECT (STITLE || ' ' || SFNAME || ' ' || SMNAME || ' ' || SLNAME) AS FullName," +
                     "(SELECT GAME_DESC FROM EILSPORT_GAMEDIR gm WHERE gm.GAME_CODE = GAME_APPLIED) AS APPLY_GAME," +
                     "TO_CHAR(DOB, 'DD-MM-YYYY') AS DOB, GAME_CAT," +
                     "TO_CHAR(MOBILENO, '9999999999') AS MOBILENO," +
                     "EMAIL, REGCODE, AADHARNO " +
                     "FROM EILSPORT_APPMAIN WHERE APP_STATUS = 'SUBMIT'";
        DataTable applicantsData = bc.getDetailsInDataTable(sql);  // Get the data from the database
        ExportToExcel(applicantsData);
    }

    private void ExportToExcel(DataTable data)
    {
        Response.Clear();
        Response.Buffer = true;
        Response.AddHeader("content-disposition", "attachment;filename=ApplicantsData.xls");
        Response.Charset = "";
        Response.ContentType = "application/vnd.ms-excel";

        using (System.IO.StringWriter sw = new System.IO.StringWriter()) {
            using (HtmlTextWriter hw = new HtmlTextWriter(sw)) {
                GridView gridView = new GridView();
                gridView.AllowPaging = false;

                DataTable modifiedData = new DataTable();

                foreach (DataColumn column in data.Columns) {
                    if (column.ColumnName == "RegCode" || column.ColumnName == "AadharNo") {
                        modifiedData.Columns.Add(column.ColumnName, typeof(string));
                    }
                    else {
                        modifiedData.Columns.Add(column.ColumnName, column.DataType);
                    }
                }

                foreach (DataRow row in data.Rows) {
                    DataRow newRow = modifiedData.NewRow();
                    foreach (DataColumn column in data.Columns)
                    {
                        if (column.ColumnName == "RegCode" || column.ColumnName == "AadharNo")
                        {
                            newRow[column.ColumnName] = "'" + row[column.ColumnName].ToString();
                        }
                        else
                        {
                            newRow[column.ColumnName] = row[column.ColumnName];
                        }
                    }
                    modifiedData.Rows.Add(newRow);
                }

                gridView.DataSource = modifiedData;
                gridView.DataBind();

                gridView.RenderControl(hw);

                Response.Output.Write(sw.ToString());
                Response.Flush();
                Response.End();
            }
        }
    }

    public override void VerifyRenderingInServerForm(Control control) {
        // This method is needed to prevent ASP.NET from throwing an error when rendering a control to HTML
    }

    protected void ExportToCSV_Click(object sender, EventArgs e) {
        string sql = "SELECT (STITLE || ' ' || SFNAME || ' ' || SMNAME || ' ' || SLNAME) AS FullName," +
                     "(SELECT GAME_DESC FROM EILSPORT_GAMEDIR gm WHERE gm.GAME_CODE = GAME_APPLIED) AS APPLY_GAME," +
                     "TO_CHAR(DOB, 'DD-MM-YYYY') AS DOB, GAME_CAT," +
                     "TO_CHAR(MOBILENO, '9999999999') AS MOBILENO," +
                     "EMAIL, REGCODE, AADHARNO " +
                     "FROM EILSPORT_APPMAIN WHERE APP_STATUS = 'SUBMIT'";
        var applicantsData = bc.getDetailsInDataTable(sql);
        ExportToCSV(applicantsData);
    }

    private void ExportToCSV(DataTable data) {
        Response.Clear();
        Response.Buffer = true;
        Response.AddHeader("content-disposition", "attachment;filename=Applicants.csv");
        Response.Charset = "";
        Response.ContentType = "application/text";

        StringBuilder sb = new StringBuilder();

        for (int i = 0; i < data.Columns.Count; i++) {
            sb.Append(data.Columns[i].ColumnName + ',');
        }
        sb.Append("\r\n");

        foreach (DataRow dr in data.Rows) {
            for (int i = 0; i < data.Columns.Count; i++) {
                string columnName = data.Columns[i].ColumnName;
                if (columnName == "RegCode" || columnName == "AadharNo") {
                    sb.Append("'" + dr[i].ToString() + ',');
                }
                else {
                    sb.Append(dr[i].ToString() + ',');
                }
            }
            sb.Append("\r\n");
        }

        Response.Output.Write(sb.ToString());
        Response.Flush();
        Response.End();
    }
}