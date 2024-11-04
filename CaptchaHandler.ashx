<%@ WebHandler Language="C#" Class="CaptchaHandler" %>
using System;
using System.Web;
using System.Drawing;
using System.Drawing.Text;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.IO;
using System.Web.SessionState;
public class CaptchaHandler : IHttpHandler, IReadOnlySessionState
{
    public void ProcessRequest(HttpContext context)
    {
        Bitmap objBmp = new Bitmap(75, 25);
        Graphics objGraphics = Graphics.FromImage(objBmp);
        objGraphics.SmoothingMode = SmoothingMode.AntiAlias;
        objGraphics.Clear(Color.Gray);
        objGraphics.TextRenderingHint = TextRenderingHint.AntiAlias;
        Font objFont = new Font("Arial", 12, System.Drawing.FontStyle.Bold);
        string strCaptcha = string.Empty;
        if (context.Session["Captcha"].ToString() != null)
        {
            strCaptcha = context.Session["Captcha"].ToString();
        }
        objGraphics.DrawString(strCaptcha, objFont, Brushes.White, 3, 3);
        MemoryStream ms = new MemoryStream();
        objBmp.Save(ms, ImageFormat.Png);
        byte[] bmpBytes = ms.GetBuffer();
        context.Response.ContentType = "image/png";
        context.Response.BinaryWrite(bmpBytes);
        objBmp.Dispose();
        objFont.Dispose();
        objGraphics.Dispose();
        ms.Close();
        context.Response.End();
    }
    public bool IsReusable
    {
        get
        {
            return false;
        }
    }
}