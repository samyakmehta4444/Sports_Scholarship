using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Data;
using System.Globalization;
using System.Net.Mail;

public partial class Apply : Page {
    BindControls bc = new BindControls();
    protected void Page_Load(object sender, EventArgs e) {
        if (!IsPostBack) {
            if (Session["AadharNo"] != null && Session["EmailID"] != null) {
                aadharNo.Text = Session["AadharNo"].ToString();
                email.Text = Session["EmailID"].ToString();

                BindGrid();
                BindGameList();
                BindExamList();

                string regCode = Request.QueryString["regCode"];

                if (!string.IsNullOrEmpty(regCode)) {
                    Session["regCode"] = regCode;
                    string isSubmittedQuery = "SELECT APP_STATUS FROM EILSPORT_APPMAIN WHERE AADHARNO = '" + Session["AadharNo"] + "' AND EMAIL = '" + Session["EmailID"] + "' AND REGCODE = '" + regCode + "'";
                    string isSubmitted = bc.returnString(isSubmittedQuery);
                    if (isSubmitted == "DRAFT") {
                        openDraft(regCode);
                    }
                    else if (isSubmitted == "SUBMIT") {
                        openSubmit(regCode);
                    }
                }
            } 
        }
    }

    private void BindExamList() {
        string sql = "SELECT * FROM EILSPORT_EXAMDIR";
        bc.bindDropDownList(examPass, sql, "EXAM_DESC", "EXAM_CODE", "--HIGHEST QUALIFICATION--", "");
    }

    private void BindGameList() {
        string sql = "SELECT * FROM EILSPORT_GAMEDIR";
        bc.bindDropDownList(gameList, sql, "GAME_DESC", "GAME_CODE", "--SPORT--", "");
    }

    private bool ValidateFileExtension(string extension) {
        string[] allowedExtensions = { ".pdf" };
        return Array.Exists(allowedExtensions, ext => ext.Equals(extension, StringComparison.OrdinalIgnoreCase));
    }

    bool IsJpegOrJpg(string fileExtension) {
        return fileExtension.Equals(".jpeg", StringComparison.OrdinalIgnoreCase) || fileExtension.Equals(".jpg", StringComparison.OrdinalIgnoreCase);
    }

    protected void btnDraft_Click(object sender, EventArgs e) {
        string currentYear = DateTime.Now.Year.ToString(); // First 4 digits
        string registrationNumber = "" ;
        string sql = "SELECT REGCODE FROM EILSPORT_APPGAMEDTL WHERE AADHARNO = '" + aadharNo.Text + "'";
        DataTable dt = new DataTable();
        dt = bc.getDetailsInDataTable(sql);
        if (dt.Rows.Count > 0) {
            registrationNumber = dt.Rows[0]["REGCODE"].ToString();
            string del1 = "DELETE FROM EILSPORT_APPMAIN WHERE REGCODE = '" + registrationNumber + "' AND AADHARNO = '" + aadharNo.Text + "'";
            bc.returnString(del1);
            string del2 = "DELETE FROM EILSPORT_APPGAMEDTL WHERE REGCODE = '" + registrationNumber + "' AND AADHARNO = '" + aadharNo.Text + "'";
            bc.returnString(del2);
        }
        else {
            if (Session["AadharNo"] != null && Session["EmailID"] != null)
            {
                string incrementingDigits = GetNextIncrementingNumber(); // Last 8 digits
                registrationNumber = currentYear + incrementingDigits; // Full 12-digit registration number
            }
        }

        string uploadDir = Server.MapPath("~/App_Data/" + registrationNumber + "/");
        if (!Directory.Exists(uploadDir)) {
            Directory.CreateDirectory(uploadDir);
        }
        

        if (photoUpload.HasFile) {
            string fileExtension = Path.GetExtension(photoUpload.FileName);
            if (IsJpegOrJpg(fileExtension) && photoUpload.PostedFile.ContentLength <= 2000000) {
                photoUpload.SaveAs(uploadDir + "Photograph" + fileExtension);
            } else {
                throw new Exception("Photograph must be in JPEG or JPG format and less than 2 MB.");
            }
        }

        if (signatureUpload.HasFile) {
            string fileExtension = Path.GetExtension(signatureUpload.FileName);
            if (IsJpegOrJpg(fileExtension) && signatureUpload.PostedFile.ContentLength <= 2000000) {
                signatureUpload.SaveAs(uploadDir + "Signature" + fileExtension);
            } else {
                throw new Exception("Signature must be in JPEG or JPG format and less than 2 MB.");
            }
        }
        if (ageProofUpload.HasFile) {
            string fileExtension = Path.GetExtension(ageProofUpload.FileName);
            if (ValidateFileExtension(fileExtension) && ageProofUpload.PostedFile.ContentLength <= 2000000) {
                ageProofUpload.SaveAs(uploadDir + "ProofOfAge" + fileExtension);
            }
        }
        if (performanceCertUpload1.HasFile) {
            string fileExtension = Path.GetExtension(performanceCertUpload1.FileName);
            if (ValidateFileExtension(fileExtension) && performanceCertUpload1.PostedFile.ContentLength <= 2000000) {
                performanceCertUpload1.SaveAs(uploadDir + "PerformanceCert1" + fileExtension);
            }
        }
        if (performanceCertUpload2.HasFile) {
            string fileExtension = Path.GetExtension(performanceCertUpload2.FileName);
            if (ValidateFileExtension(fileExtension) && performanceCertUpload2.PostedFile.ContentLength <= 2000000) {
                performanceCertUpload2.SaveAs(uploadDir + "PerformanceCert2" + fileExtension);
            }
        }
        if (rankingCertUpload.HasFile) {
            string fileExtension = Path.GetExtension(rankingCertUpload.FileName);
            if (ValidateFileExtension(fileExtension) && rankingCertUpload.PostedFile.ContentLength <= 2000000) {
                rankingCertUpload.SaveAs(uploadDir + "RankingCertificate" + fileExtension);
            }
        }
        if (aadharUpload.HasFile) {
            string fileExtension = Path.GetExtension(aadharUpload.FileName);
            if (ValidateFileExtension(fileExtension) && aadharUpload.PostedFile.ContentLength <= 2000000) // Max size 2 MB
            {
                aadharUpload.SaveAs(uploadDir + "AadharCard" + fileExtension);
            }
        }
        
        string regdate = "SELECT TO_CHAR(REGDATE, 'DD-MM-YYYY') REGDATE FROM EILSPORT_USER WHERE AADHAR = '" + aadharNo.Text + "'";
        regdate = bc.returnString(regdate);
        if (!string.IsNullOrEmpty(regdate)) {
            regdate = "TO_DATE('" + regdate + "', 'DD-MM-YYYY')"; // Format to Oracle date
        }
        else {
            regdate = "NULL"; // Handle case where regdate is null
        }
        string ldate = dob.Text;
        if (ldate.Trim() == "") {
            ldate += "NULL";
        }
        else {
            string[] dateParts = ldate.Split('-');
            string day = dateParts[2];
            string month = dateParts[1];
            string year = dateParts[0];
            ldate = "TO_DATE('" + day + "-" + month + "-" + year + "', 'DD-MM-YYYY')"; // Format DOB for Oracle insertion
        }

        string query1 = "INSERT INTO EILSPORT_APPMAIN (REGCODE, REGDATE, ADVNO, STITLE, SFNAME, SMNAME, SLNAME, FTITLE, FFNAME, FMNAME, FLNAME,MTITLE, MFNAME, MMNAME, MLNAME," +
                       "DOB, MOBILENO, EMAIL, PADD, PCITY, PSTATE, PZIP, AADHARNO, GAME_APPLIED, GAME_CAT, GAME_SCHOLARSHIP, EXAM_CODE, YEAR_EXAM," +
                       "SCHOOL_UNIVERSITY, GAME_ACHIEVE, PHOTO, SIGNATURE, AGEPROOF, PERFORMANCE1, PERFORMANCE2, RANKCERTIFICATE, AADHARUPLOAD, UPD_DATE, APP_STATUS, APP_DECLARE) VALUES (" +
                       "'" + registrationNumber + "'," + regdate + ", null, '" + identifier.SelectedValue + "','" + firstName.Text + "','" + midName.Text + "','" + lastName.Text + "'," +
                       "'MR','" + fFirstName.Text + "','" + fMidName.Text + "','" + fLastName.Text + "','MRS','" + mFirstName.Text + "','" + mMidName.Text + "','" + mLastName.Text + "'," +
                       ldate + ",'" + mobile.Text + "','" + email.Text + "','" + address.Text + "','" + city.Text + "','" + state.Text + "','" + pinCode.Text + "'," + 
                       "'" + aadharNo.Text + "','" + gameList.SelectedValue + "','" + catOfParticipation.SelectedValue + "','" + otherScholarShip.SelectedValue + "'," +
                       "'" + examPass.SelectedValue + "','" + passYear.SelectedValue + "','" + school_Uni.Text + "','" + achievement.Value + "','" + photoUpload.HasFile + "'," +
                       "'" + signatureUpload.HasFile + "','" + ageProofUpload.HasFile + "','" + performanceCertUpload1.HasFile + "','" + performanceCertUpload2.HasFile + "'," +
                       "'" + rankingCertUpload.HasFile + "','" + aadharUpload.HasFile + "', TRUNC(SYSDATE), 'DRAFT', '" + declare.Checked + "'" +
                       ")";
        if (registrationNumber != "")
        {
            bc.executeSQLCommand(query1);
        }

        foreach (GridViewRow row in gvScholarship.Rows) {
            int rowIndex = row.RowIndex + 1;  // Serial number starts from 1

            DropDownList ddlSports = (DropDownList)row.FindControl("ddlSports");
            DropDownList ddlEvents = (DropDownList)row.FindControl("ddlEvents");
            DropDownList ddlType = (DropDownList)row.FindControl("ddlType");
            DropDownList ddlTournamentName = (DropDownList)row.FindControl("ddlTournamentName");
            TextBox txtVenue = (TextBox)row.FindControl("txtVenue");
            DropDownList ddlPositionLevel = (DropDownList)row.FindControl("ddlPositionLevel");
            DropDownList ddlUnit = (DropDownList)row.FindControl("ddlUnit");
            TextBox txtResults = (TextBox)row.FindControl("txtResults");
            TextBox txtDate = (TextBox)row.FindControl("txtDate");
            FileUpload fileUpload = (FileUpload)row.FindControl("fileUpload");

            string gameCode = ddlSports.SelectedValue;    // Extract the selected game code
            string eventCode = ddlEvents.SelectedValue;   // Extract the selected event code
            string tmntmCode = ddlType.SelectedValue;     // Tournament Type code
            string tmntdCode = ddlTournamentName.SelectedValue; // Tournament name code
            string venue = txtVenue.Text.Trim();          // Venue
            string levelCode = ddlPositionLevel.SelectedValue;  // Level code
            string unit = ddlUnit.SelectedValue;          // Unit
            string result = txtResults.Text.Trim();       // Result
            string playDate = txtDate.Text.Trim();        // Play date (in string format)

            if (fileUpload.HasFile) {
                string fileExtension = Path.GetExtension(fileUpload.FileName);
                if (ValidateFileExtension(fileExtension) && fileUpload.PostedFile.ContentLength <= 2000000) {
                    fileUpload.SaveAs(uploadDir + "Achievement_" + rowIndex + fileExtension);  // Save the renamed file
                }
            }

            DateTime parsedPlayDate;
            if (DateTime.TryParseExact(playDate, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out parsedPlayDate)) {
                playDate = parsedPlayDate.ToString("dd-MM-yyyy");  // Convert to DD-MM-YYYY format
            }

            string insertQuery = "INSERT INTO EILSPORT_APPGAMEDTL (REGCODE, AADHARNO, SLNO, GAME_CODE, EVENT_CODE, TMNTM_CODE, TMNTD_CODE, " +
                                 "VENUE, LEVEL_CODE, UNIT, RESULT, PLAY_DATE, RELEVANT_DOC, UPD_DATE) VALUES (" +
                                 "'" + registrationNumber + "','" + aadharNo.Text + "'," + rowIndex + "," +
                                 "'" + gameCode + "', '" + eventCode + "', '" + tmntmCode + "', '" + tmntdCode + "', '" + venue + "', " +
                                 "'" + levelCode + "', '" + unit + "', '" + result + "',TO_DATE('" + playDate + "', 'DD-MM-YYYY'), " +  // Insert playDate in DD-MM-YYYY format
                                 "'" + fileUpload.HasFile + "', TRUNC(SYSDATE))";  // UPD_DATE is current date in Oracle format
            if (registrationNumber != "")
            {
                bc.returnString(insertQuery);
            }
        }
        ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Details Saved as Draft !');", true);
        Response.Redirect("Status.aspx");
    }

    protected void btnSubmit_Click(object sender, EventArgs e) {
        string currentYear = DateTime.Now.Year.ToString(); // First 4 digits
        string registrationNumber = "";
        string sql = "SELECT REGCODE FROM EILSPORT_APPGAMEDTL WHERE AADHARNO = '" + aadharNo.Text + "'";
        DataTable dt = new DataTable();
        dt = bc.getDetailsInDataTable(sql);
        if (dt.Rows.Count > 0) {
            registrationNumber = dt.Rows[0]["REGCODE"].ToString();
            string del1 = "DELETE FROM EILSPORT_APPMAIN WHERE REGCODE = '" + registrationNumber + "' AND AADHARNO = '" + aadharNo.Text + "'";
            bc.returnString(del1);
            string del2 = "DELETE FROM EILSPORT_APPGAMEDTL WHERE REGCODE = '" + registrationNumber + "' AND AADHARNO = '" + aadharNo.Text + "'";
            bc.returnString(del2);
        }
        else {
            if (Session["AadharNo"] != null && Session["EmailID"] != null)
            {
                string incrementingDigits = GetNextIncrementingNumber(); // Last 8 digits
                registrationNumber = currentYear + incrementingDigits; // Full 12-digit registration number
            }
        }

        string uploadDir = Server.MapPath("~/App_Data/" + registrationNumber + "/");
        if (!Directory.Exists(uploadDir)) {
            Directory.CreateDirectory(uploadDir);
        }
        if (photoUpload.HasFile) {
            string fileExtension = Path.GetExtension(photoUpload.FileName);
            if (ValidateFileExtension(fileExtension) && photoUpload.PostedFile.ContentLength <= 2000000)
            {
                photoUpload.SaveAs(uploadDir + "Photograph" + fileExtension);
            }
        }
        if (signatureUpload.HasFile) {
            string fileExtension = Path.GetExtension(signatureUpload.FileName);
            if (ValidateFileExtension(fileExtension) && signatureUpload.PostedFile.ContentLength <= 2000000)
            {
                signatureUpload.SaveAs(uploadDir + "Signature" + fileExtension);
            }
        }
        if (ageProofUpload.HasFile) {
            string fileExtension = Path.GetExtension(ageProofUpload.FileName);
            if (ValidateFileExtension(fileExtension) && ageProofUpload.PostedFile.ContentLength <= 2000000)
            {
                ageProofUpload.SaveAs(uploadDir + "ProofOfAge" + fileExtension);
            }
        }
        if (performanceCertUpload1.HasFile) {
            string fileExtension = Path.GetExtension(performanceCertUpload1.FileName);
            if (ValidateFileExtension(fileExtension) && performanceCertUpload1.PostedFile.ContentLength <= 2000000)
            {
                performanceCertUpload1.SaveAs(uploadDir + "PerformanceCert1" + fileExtension);
            }
        }
        if (performanceCertUpload2.HasFile) {
            string fileExtension = Path.GetExtension(performanceCertUpload2.FileName);
            if (ValidateFileExtension(fileExtension) && performanceCertUpload2.PostedFile.ContentLength <= 2000000)
            {
                performanceCertUpload2.SaveAs(uploadDir + "PerformanceCert2" + fileExtension);
            }
        }
        if (rankingCertUpload.HasFile) {
            string fileExtension = Path.GetExtension(rankingCertUpload.FileName);
            if (ValidateFileExtension(fileExtension) && rankingCertUpload.PostedFile.ContentLength <= 2000000)
            {
                rankingCertUpload.SaveAs(uploadDir + "RankingCertificate" + fileExtension);
            }
        }
        if (aadharUpload.HasFile) {
            string fileExtension = Path.GetExtension(aadharUpload.FileName);
            if (ValidateFileExtension(fileExtension) && aadharUpload.PostedFile.ContentLength <= 2000000) // Max size 2 MB
            {
                aadharUpload.SaveAs(uploadDir + "AadharCard" + fileExtension);
            }
        }

        string regdate = "SELECT TO_CHAR(REGDATE, 'DD-MM-YYYY') REGDATE FROM EILSPORT_USER WHERE AADHAR = '" + aadharNo.Text + "'";
        regdate = bc.returnString(regdate);
        if (!string.IsNullOrEmpty(regdate))  {
            regdate = "TO_DATE('" + regdate + "', 'DD-MM-YYYY')"; // Format to Oracle date
        }
        else {
            regdate = "NULL"; // Handle case where regdate is null
        }
        string ldate = dob.Text;
        if (ldate.Trim() == "") {
            ldate += "NULL";
        }
        else {
            string[] dateParts = ldate.Split('-');
            string day = dateParts[2];
            string month = dateParts[1];
            string year = dateParts[0];
            ldate = "TO_DATE('" + day + "-" + month + "-" + year + "', 'DD-MM-YYYY')"; // Format DOB for Oracle insertion
        }

        string query1 = "INSERT INTO EILSPORT_APPMAIN (REGCODE, REGDATE, ADVNO, STITLE, SFNAME, SMNAME, SLNAME, FTITLE, FFNAME, FMNAME, FLNAME,MTITLE, MFNAME, MMNAME, MLNAME," +
                       "DOB, MOBILENO, EMAIL, PADD, PCITY, PSTATE, PZIP, AADHARNO, GAME_APPLIED, GAME_CAT, GAME_SCHOLARSHIP, EXAM_CODE, YEAR_EXAM," +
                       "SCHOOL_UNIVERSITY, GAME_ACHIEVE, PHOTO, SIGNATURE, AGEPROOF, PERFORMANCE1, PERFORMANCE2, RANKCERTIFICATE, AADHARUPLOAD, UPD_DATE, APP_STATUS, APP_DECLARE) VALUES (" +
                       "'" + registrationNumber + "'," + regdate + ", null, '" + identifier.SelectedValue + "','" + firstName.Text + "','" + midName.Text + "','" + lastName.Text + "'," +
                       "'MR','" + fFirstName.Text + "','" + fMidName.Text + "','" + fLastName.Text + "','MRS','" + mFirstName.Text + "','" + mMidName.Text + "','" + mLastName.Text + "'," +
                       ldate + ",'" + mobile.Text + "','" + email.Text + "','" + address.Text + "','" + city.Text + "','" + state.Text + "','" + pinCode.Text + "'," +
                       "'" + aadharNo.Text + "','" + gameList.SelectedValue + "','" + catOfParticipation.SelectedValue + "','" + otherScholarShip.SelectedValue + "'," +
                       "'" + examPass.SelectedValue + "','" + passYear.SelectedValue + "','" + school_Uni.Text + "','" + achievement.Value + "','" + photoUpload.HasFile + "'," +
                       "'" + signatureUpload.HasFile + "','" + ageProofUpload.HasFile + "','" + performanceCertUpload1.HasFile + "','" + performanceCertUpload2.HasFile + "'," +
                       "'" + rankingCertUpload.HasFile + "','" + aadharUpload.HasFile + "', TRUNC(SYSDATE), 'SUBMIT', '" + declare.Checked + "'" +
                       ")";
        if (registrationNumber != "")
        {
            bc.executeSQLCommand(query1);
        }

        foreach (GridViewRow row in gvScholarship.Rows) {
            int rowIndex = row.RowIndex + 1;  // Serial number starts from 1

            DropDownList ddlSports = (DropDownList)row.FindControl("ddlSports");
            DropDownList ddlEvents = (DropDownList)row.FindControl("ddlEvents");
            DropDownList ddlType = (DropDownList)row.FindControl("ddlType");
            DropDownList ddlTournamentName = (DropDownList)row.FindControl("ddlTournamentName");
            TextBox txtVenue = (TextBox)row.FindControl("txtVenue");
            DropDownList ddlPositionLevel = (DropDownList)row.FindControl("ddlPositionLevel");
            DropDownList ddlUnit = (DropDownList)row.FindControl("ddlUnit");
            TextBox txtResults = (TextBox)row.FindControl("txtResults");
            TextBox txtDate = (TextBox)row.FindControl("txtDate");
            FileUpload fileUpload = (FileUpload)row.FindControl("fileUpload");

            string gameCode = ddlSports.SelectedValue;    // Extract the selected game code
            string eventCode = ddlEvents.SelectedValue;   // Extract the selected event code
            string tmntmCode = ddlType.SelectedValue;     // Tournament Type code
            string tmntdCode = ddlTournamentName.SelectedValue; // Tournament name code
            string venue = txtVenue.Text.Trim();          // Venue
            string levelCode = ddlPositionLevel.SelectedValue;  // Level code
            string unit = ddlUnit.SelectedValue;          // Unit
            string result = txtResults.Text.Trim();       // Result
            string playDate = txtDate.Text.Trim();        // Play date (in string format)

            if (fileUpload.HasFile)
            {
                string fileExtension = Path.GetExtension(fileUpload.FileName);
                if (ValidateFileExtension(fileExtension) && fileUpload.PostedFile.ContentLength <= 2000000)
                {
                    fileUpload.SaveAs(uploadDir + "Achievement_" + rowIndex + fileExtension);  // Save the renamed file
                }
            }

            DateTime parsedPlayDate;
            if (DateTime.TryParseExact(playDate, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out parsedPlayDate))
            {
                playDate = parsedPlayDate.ToString("dd-MM-yyyy");  // Convert to DD-MM-YYYY format
            }

            string insertQuery = "INSERT INTO EILSPORT_APPGAMEDTL (REGCODE, AADHARNO, SLNO, GAME_CODE, EVENT_CODE, TMNTM_CODE, TMNTD_CODE, " +
                                 "VENUE, LEVEL_CODE, UNIT, RESULT, PLAY_DATE, RELEVANT_DOC, UPD_DATE) VALUES (" +
                                 "'" + registrationNumber + "','" + aadharNo.Text + "'," + rowIndex + "," +
                                 "'" + gameCode + "', '" + eventCode + "', '" + tmntmCode + "', '" + tmntdCode + "', '" + venue + "', " +
                                 "'" + levelCode + "', '" + unit + "', '" + result + "',TO_DATE('" + playDate + "', 'DD-MM-YYYY'), " +  // Insert playDate in DD-MM-YYYY format
                                 "'" + fileUpload.HasFile + "', TRUNC(SYSDATE))";  // UPD_DATE is current date in Oracle format
            if (registrationNumber != "")
            {
                bc.returnString(insertQuery);
            }
        }
        string body = @"
        <p>Dear " + firstName.Text + " " + midName.Text + " " + lastName.Text + @",</p>
        <p>Thank You for Submitting your Application for the EIL Sports Scholarship.</p>
        <p>Your Registration Number is : <strong>" + registrationNumber + @"</strong>.</p>
        <p>Please retain a printout of your online application form for future reference. Selected candidates will be informed through the registered email ID or mobile phone. Applicants may kindly note that any form of canvassing will lead to disqualification from candidature.</p>
        <br />
        <p>Regards,<br />
        EIL Sports Team</p>
        <br />
        <p><em>This is an autogenerated mail. Please do not reply.</em></p>";

        SendMail(email.Text, "itsapp@eil.co.in", "", "Application Submitted EIL Sports Scholarship", body, "");

        ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Application Submitted. Please check your Registered Email-ID Inbox Folder !');", true);
        Response.Redirect("Status.aspx");
    }

    public string SendMail(string toList, string fromm, string ccList, string subject, string body, string msg) {
        MailMessage message = new MailMessage();
        SmtpClient smtpClient = new SmtpClient();
        try {
            MailAddress mfrom = new MailAddress(fromm);
            MailAddress mto = new MailAddress(toList);
            MailMessage objEmail = new MailMessage(mfrom, mto);

            // objEmail.Bcc.Add(ccList); // Uncomment if needed
            objEmail.Subject = subject;
            objEmail.Body = body;
            objEmail.Priority = MailPriority.High;
            objEmail.IsBodyHtml = true;

            SmtpClient smtpmail = new SmtpClient("appsmtp.eil.co.in");

            if (!string.IsNullOrEmpty(toList)) {
                smtpmail.Send(objEmail);
            }

            objEmail.Dispose();
            msg = "Successful";
        }
        catch (Exception ex) {
            msg = ex.Message;
        }

        return msg;
    }

    private string GetNextIncrementingNumber() {
        string filePath = Server.MapPath("~/App_Data/LastRegistrationNumber.txt");
        long lastNumber = 0;

        if (File.Exists(filePath)) {
            string lastNumberStr = File.ReadAllText(filePath);
            long.TryParse(lastNumberStr, out lastNumber);
        }

        lastNumber++;

        File.WriteAllText(filePath, lastNumber.ToString());

        return lastNumber.ToString().PadLeft(8, '0');
    }

    private void BindGrid() {
        DataTable dt = new DataTable();
        dt.Columns.Add("Venue");
        dt.Columns.Add("Position");
        dt.Columns.Add("Unit");
        dt.Columns.Add("Results");
        dt.Columns.Add("Date");

        for (int i = 0; i < 2; i++)  // Add 2 rows for demonstration
        {
            dt.Rows.Add("", "", "", "", "");
        }
        gvScholarship.DataSource = dt;
        gvScholarship.DataBind();
    }

    protected void ddlSports_SelectedIndexChanged(object sender, EventArgs e) {
        DropDownList ddlSports = (DropDownList)sender;
        GridViewRow row = (GridViewRow)ddlSports.NamingContainer;
        DropDownList ddlEvents = (DropDownList)row.FindControl("ddlEvents");
        DropDownList ddlType = (DropDownList)row.FindControl("ddlType");
        DropDownList ddlTournamentName = (DropDownList)row.FindControl("ddlTournamentName");
        DropDownList ddlUnit = (DropDownList)row.FindControl("ddlUnit");
        DropDownList ddlPositionLevel = (DropDownList)row.FindControl("ddlPositionLevel");

        string selectedSport = ddlSports.SelectedValue;
        ddlEvents.Items.Clear();
        ddlTournamentName.Items.Clear();
        ddlUnit.Items.Clear();
        ddlPositionLevel.Items.Clear();
        ddlType.SelectedValue = "";

        string sql = "SELECT * FROM EILSPORT_EVENTDIR WHERE GAME_CODE = '" + selectedSport + "'";
        bc.bindDropDownList(ddlEvents, sql, "EVENT_DESC", "EVENT_CODE", "--EVENT--", "");

        PopulateUnitDropdown(ddlSports.SelectedValue, ddlUnit);
        PopulatePositionLevelDropdown(ddlSports.SelectedValue, ddlPositionLevel);
    }

    private void PopulateUnitDropdown(string selectedSport, DropDownList ddlUnit) {
        if (selectedSport == "1") {
            ddlUnit.Items.Add(new ListItem("NA", "NA"));
        }
        else if (selectedSport == "2") {
            ddlUnit.Items.Add(new ListItem("MM:SS:MS", "MM:SS:MS"));
        }
        else if (selectedSport == "3")  {
            ddlUnit.Items.Add(new ListItem("Meters", "Meters"));
            ddlUnit.Items.Add(new ListItem("MM:SS:MS", "MM:SS:MS"));
        }
    }

    protected void gvScholarship_RowDataBound(object sender, GridViewRowEventArgs e) {
        if (e.Row.RowType == DataControlRowType.DataRow) {
            DropDownList ddlSports = (DropDownList)e.Row.FindControl("ddlSports");
            DropDownList ddlEvents = (DropDownList)e.Row.FindControl("ddlEvents");
            DropDownList ddlPositionLevel = (DropDownList)e.Row.FindControl("ddlPositionLevel");
            DropDownList ddlType = (DropDownList)e.Row.FindControl("ddlType");
            DropDownList ddlTournamentName = (DropDownList)e.Row.FindControl("ddlTournamentName");
            DropDownList ddlUnit = (DropDownList)e.Row.FindControl("ddlUnit");
            TextBox txtVenue = (TextBox)e.Row.FindControl("txtVenue");
            TextBox txtResults = (TextBox)e.Row.FindControl("txtResults");
            TextBox txtDate = (TextBox)e.Row.FindControl("txtDate");

            string sql = "SELECT * FROM EILSPORT_GAMEDIR";
            bc.bindDropDownList(ddlSports, sql, "GAME_DESC", "GAME_CODE", "--SPORT--", "");

            if (Session["regCode"] != null && !string.IsNullOrEmpty(Session["regCode"].ToString())) {
                string achieve = "SELECT * FROM EILSPORT_APPGAMEDTL WHERE AADHARNO = '" + Session["AadharNo"] + "' AND REGCODE = '" + Session["regCode"] + "'";
                DataTable dt = bc.getDetailsInDataTable(achieve);

                if (dt.Rows.Count > 0) {
                    int rowIndex = e.Row.RowIndex; // This will get the row index of the GridView

                    if (rowIndex < dt.Rows.Count) // Ensure the rowIndex does not exceed dt.Rows.Count
                    {
                        txtVenue.Text = dt.Rows[rowIndex]["VENUE"] != DBNull.Value ? dt.Rows[rowIndex]["VENUE"].ToString() : string.Empty;
                        txtResults.Text = dt.Rows[rowIndex]["RESULT"] != DBNull.Value ? dt.Rows[rowIndex]["RESULT"].ToString() : string.Empty;
                        txtDate.Text = dt.Rows[rowIndex]["PLAY_DATE"] != DBNull.Value ? Convert.ToDateTime(dt.Rows[rowIndex]["PLAY_DATE"]).ToString("yyyy-MM-dd") : string.Empty;

                        ddlSports.SelectedValue = dt.Rows[rowIndex]["GAME_CODE"] != DBNull.Value ? dt.Rows[rowIndex]["GAME_CODE"].ToString() : string.Empty;
                        ddlType.SelectedValue = dt.Rows[rowIndex]["TMNTM_CODE"] != DBNull.Value ? dt.Rows[rowIndex]["TMNTM_CODE"].ToString() : string.Empty;

                        if (ddlSports.SelectedIndex > 0)
                        {
                            string eventsQuery = "SELECT EVENT_CODE, EVENT_DESC FROM EILSPORT_EVENTDIR WHERE GAME_CODE = '" + ddlSports.SelectedValue + "'";
                            DataTable eventsData = bc.getDetailsInDataTable(eventsQuery);
                            ddlEvents.DataSource = eventsData;
                            ddlEvents.DataTextField = "EVENT_DESC";
                            ddlEvents.DataValueField = "EVENT_CODE";
                            ddlEvents.DataBind();
                            ddlEvents.Items.Insert(0, new ListItem("--EVENT--", ""));
                            ddlEvents.SelectedValue = dt.Rows[rowIndex]["EVENT_CODE"] != DBNull.Value ? dt.Rows[rowIndex]["EVENT_CODE"].ToString() : string.Empty;

                            string positionsQuery = "SELECT LEVEL_CODE, LEVEL_DESC FROM EILSPORT_LEVELDIR WHERE GAME_CODE = '" + ddlSports.SelectedValue + "'";
                            DataTable positionsData = bc.getDetailsInDataTable(positionsQuery);
                            ddlPositionLevel.DataSource = positionsData;
                            ddlPositionLevel.DataTextField = "LEVEL_DESC";
                            ddlPositionLevel.DataValueField = "LEVEL_CODE";
                            ddlPositionLevel.DataBind();
                            ddlPositionLevel.Items.Insert(0, new ListItem("--POSITION--", ""));
                            ddlPositionLevel.SelectedValue = dt.Rows[rowIndex]["LEVEL_CODE"] != DBNull.Value ? dt.Rows[rowIndex]["LEVEL_CODE"].ToString() : string.Empty;
                        }

                        if (ddlType.SelectedIndex > 0)
                        {
                            string tournamentsQuery = "SELECT TMNTD_CODE, TMNT_DESC FROM EILSPORT_TMNTDIR WHERE GAME_CODE = '" + ddlSports.SelectedValue + "' AND EVENT_CODE = '" + ddlEvents.SelectedValue + "' AND TMNTM_CODE = '" + ddlType.SelectedValue + "'";
                            DataTable tournaments = bc.getDetailsInDataTable(tournamentsQuery);
                            ddlTournamentName.DataSource = tournaments;
                            ddlTournamentName.DataTextField = "TMNT_DESC";
                            ddlTournamentName.DataValueField = "TMNTD_CODE";
                            ddlTournamentName.DataBind();
                            ddlTournamentName.Items.Insert(0, new ListItem("--TOURNAMENT--", ""));
                            ddlTournamentName.SelectedValue = dt.Rows[rowIndex]["TMNTD_CODE"] != DBNull.Value ? dt.Rows[rowIndex]["TMNTD_CODE"].ToString() : string.Empty;
                        }
                    }
                }
            }
        }
    }

    protected void ddlType_SelectedIndexChanged(object sender, EventArgs e) {
        try {
            DropDownList ddlTournamentType = (DropDownList)sender;
            GridViewRow row = (GridViewRow)ddlTournamentType.NamingContainer;
            DropDownList ddlTournamentName = (DropDownList)row.FindControl("ddlTournamentName");
            DropDownList ddlSports = (DropDownList)row.FindControl("ddlSports");
            DropDownList ddlPositionLevel = (DropDownList)row.FindControl("ddlPositionLevel");
            DropDownList ddlEvents = (DropDownList)row.FindControl("ddlEvents");

            ddlTournamentName.Items.Clear();
            PopulateTournamentNameDropdown(ddlSports.SelectedValue, ddlEvents.SelectedValue, ddlTournamentType.SelectedValue, ddlTournamentName);
        }
        catch (Exception ex) {
            Console.WriteLine(ex.Message);
        }
    }

    private void PopulateTournamentNameDropdown(string selectedSport, string selectedEvent, string selectedTournamentType, DropDownList ddlTournamentName) {
        string sql = "SELECT * FROM EILSPORT_TMNTDIR WHERE GAME_CODE = '" + selectedSport + "' AND EVENT_CODE = '" + selectedEvent + "' AND TMNTM_CODE = '" + selectedTournamentType + "'" ;
        bc.bindDropDownList(ddlTournamentName, sql, "TMNT_DESC", "TMNTD_CODE", "--TOURNAMENT--", "");
    }

    private void PopulatePositionLevelDropdown(string selectedSport, DropDownList ddlPositionLevel) {
        string sql = "SELECT * FROM EILSPORT_LEVELDIR WHERE GAME_CODE = '" + selectedSport + "'";
        bc.bindDropDownList(ddlPositionLevel, sql, "LEVEL_DESC", "LEVEL_CODE", "--POSITION--", "");
    }

    private void openDraft(string regCode) {
        string data = "SELECT * FROM EILSPORT_APPMAIN WHERE AADHARNO = '" + Session["AadharNo"] + "' AND EMAIL = '" + Session["EmailID"] + "' AND REGCODE = '" + regCode + "'";
        DataTable dt = bc.getDetailsInDataTable(data);
        if (dt.Rows.Count > 0) {
            identifier.SelectedValue = dt.Rows[0]["STITLE"].ToString();
            firstName.Text = dt.Rows[0]["SFNAME"].ToString();
            midName.Text = dt.Rows[0]["SMNAME"].ToString();
            lastName.Text = dt.Rows[0]["SLNAME"].ToString();
            fFirstName.Text = dt.Rows[0]["FFNAME"].ToString();
            fMidName.Text = dt.Rows[0]["FMNAME"].ToString();
            fLastName.Text = dt.Rows[0]["FLNAME"].ToString();
            mFirstName.Text = dt.Rows[0]["MFNAME"].ToString();
            mMidName.Text = dt.Rows[0]["MMNAME"].ToString();
            mLastName.Text = dt.Rows[0]["MLNAME"].ToString();

            DateTime dateOfBirth;
            if (DateTime.TryParse(dt.Rows[0]["DOB"].ToString(), out dateOfBirth)) {
                dob.Text = dateOfBirth.ToString("yyyy-MM-dd");  // Setting the date in the correct format
            }
            mobile.Text = dt.Rows[0]["MOBILENO"].ToString();
            address.Text = dt.Rows[0]["PADD"].ToString();
            city.Text = dt.Rows[0]["PCITY"].ToString();
            state.Text = dt.Rows[0]["PSTATE"].ToString();
            pinCode.Text = dt.Rows[0]["PZIP"].ToString();
            gameList.SelectedValue = dt.Rows[0]["GAME_APPLIED"].ToString();
            catOfParticipation.SelectedValue = dt.Rows[0]["GAME_CAT"].ToString();
            otherScholarShip.SelectedValue = dt.Rows[0]["GAME_SCHOLARSHIP"].ToString();
            examPass.SelectedValue = dt.Rows[0]["EXAM_CODE"].ToString();
            passYear.SelectedValue = dt.Rows[0]["YEAR_EXAM"].ToString();
            school_Uni.Text = dt.Rows[0]["SCHOOL_UNIVERSITY"].ToString();

            DataTable gridData = new DataTable();
            gridData.Columns.Add("Sports");
            gridData.Columns.Add("Events");
            gridData.Columns.Add("Type");
            gridData.Columns.Add("Tournament");
            gridData.Columns.Add("Venue");
            gridData.Columns.Add("Position");
            gridData.Columns.Add("Unit");
            gridData.Columns.Add("Results");
            gridData.Columns.Add("GameDate");
            gridData.Rows.Add(gridData.NewRow());
            gridData.Rows.Add(gridData.NewRow());

            gvScholarship.DataSource = gridData;
            gvScholarship.DataBind();
        }
    }

    private void openSubmit(string regCode) {
        openDraft(regCode);
        btnDraft.Enabled = false;
        btnSubmit.Enabled = false;
        declare.Checked = true;
        declare.Enabled = false;

        identifier.Enabled = false;
        firstName.Enabled = false;
        midName.Enabled = false;
        lastName.Enabled = false;
        fFirstName.Enabled = false;
        fMidName.Enabled = false;
        fLastName.Enabled = false;
        mFirstName.Enabled = false;
        mMidName.Enabled = false;
        mLastName.Enabled = false;
        dob.Enabled = false;
        mobile.Enabled = false;
        address.Enabled = false;
        city.Enabled = false;
        state.Enabled = false;
        pinCode.Enabled = false;
        gameList.Enabled = false;
        catOfParticipation.Enabled = false;
        otherScholarShip.Enabled = false;
        examPass.Enabled = false;
        passYear.Enabled = false;
        school_Uni.Enabled = false;

        DataTable gridData = new DataTable();
        gridData.Columns.Add("Sports");
        gridData.Columns.Add("Events");
        gridData.Columns.Add("Type");
        gridData.Columns.Add("Tournament");
        gridData.Columns.Add("Venue");
        gridData.Columns.Add("Position");
        gridData.Columns.Add("Unit");
        gridData.Columns.Add("Results");
        gridData.Columns.Add("GameDate");
        gridData.Rows.Add(gridData.NewRow());
        gridData.Rows.Add(gridData.NewRow());

        gvScholarship.DataSource = gridData;
        gvScholarship.DataBind();
        gvScholarship.Enabled = false;
    }
}