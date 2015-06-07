using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace Group3_MVC4.Models
{
    public class WatchSystem
    {
        // SMS configuration
        const string ApiKey = "638201221EF8E80A419725A4A881A0";
        const string SecretKey = "6B2C19DA5488792C6C352E750A26ED";
        public static string SMSServiceUrl = "http://api.esms.vn/MainService.svc/xml/SendMultipleMessage_V2/";

        public static void SendNotification(string phone)
        {
            ASCIIEncoding encoding = new ASCIIEncoding();
            string XmlString = @"<RQST>" 
                               +"<APIKEY>" + ApiKey +"</APIKEY>" 
                               +"<SECRETKEY>"+ SecretKey +"</SECRETKEY>"
                               +"<ISFLASH>0</ISFLASH>"
                               + "<SMSTYPE>7</SMSTYPE>"
                               +"<CONTENT>He thong tim thay dong ho phu hop voi yeu cau cua ban. "
                               +"Xem chi tiet tai: ....</CONTENT>"
                               +"<CONTACTS>" 
                               + "<CUSTOMER>" 
                               +"<PHONE>"+ phone +"</PHONE>" 
                               +"</CUSTOMER>" 
                               +"</CONTACTS>" 
                               +"</RQST >";
            // Covert xml to bytes
            byte[] data = encoding.GetBytes(XmlString);

            // Get request
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(SMSServiceUrl);
            // Set method as post
            request.Method = "POST";
            request.Timeout = 50000;
            // Set content type
            request.ContentType = "application/x-www-form-urlencoded";
            // Set content length
            request.ContentLength = data.Length;

            // Get stream data out of webrequest object
            Stream newStream = request.GetRequestStream();
            newStream.Write(data, 0, data.Length);
            newStream.Close();
            // Declare & read response from service
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            // Set utf8 encoding
            Encoding enc = System.Text.Encoding.GetEncoding("utf-8");
            // Read response stream from response object
            StreamReader loResponseStream = new StreamReader(response.GetResponseStream(), enc);
            // Read string from stream data
            string strResult = loResponseStream.ReadToEnd();
            Console.WriteLine(strResult);
            // close the stream object
            loResponseStream.Close();
            // close the response object
            response.Close();
        }

        public static string GenerateCode()
        {
            Random generator = new Random();
            String code = generator.Next(0, 1000000).ToString("D5");
            return code;
        }
    }

}