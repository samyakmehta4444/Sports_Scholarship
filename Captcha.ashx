﻿<%@ WebHandler Language="C#" Class="Captcha" %>

using System;
using System.Web;
using System.Drawing;
using System.Drawing.Imaging;

public class Captcha : IHttpHandler, System.Web.SessionState.IRequiresSessionState
{

    public void ProcessRequest(HttpContext context)
    {
        using (Bitmap b = new Bitmap(150, 40, PixelFormat.Format32bppArgb))
        {
            using (Graphics g = Graphics.FromImage(b))
            {
                Rectangle rect = new Rectangle(0, 0, 149, 39);
                g.FillRectangle(Brushes.White, rect);

                // Create string to draw.
                Random r = new Random();
                int startIndex = r.Next(1, 5);
                //int length = r.Next(5, 10);
                int length = 6;
                String drawString = Guid.NewGuid().ToString().Replace("-", "0").Substring(startIndex, length);

                // Create font and brush.
                //Font drawFont = new Font("Arial", 16, FontStyle.Italic | FontStyle.Strikeout);
                Font drawFont = new Font("Arial", 16, FontStyle.Italic );
                using (SolidBrush drawBrush = new SolidBrush(Color.Black))
                {
                    // Create point for upper-left corner of drawing.
                    PointF drawPoint = new PointF(15, 10);

                    // Draw string to screen.
                    g.DrawRectangle(new Pen(Color.Red, 0), rect);
                    g.DrawString(drawString, drawFont, drawBrush, drawPoint);
                    context.Session.Add("captcha", drawString);
                }
                b.Save(context.Response.OutputStream, ImageFormat.Jpeg);
                context.Response.ContentType = "image/jpeg";
                context.Response.End();
            }
        }
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}