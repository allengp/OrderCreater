using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.IO;
using System.Text;

/// <summary>
/// Summary description for WebRequestManager
/// </summary>
public class JSONRequestManager
{
    private string url;
    public JSONRequestManager(string url)
	{
        this.url = url;
	}

    public string POST(string data)
    {
        string result  = null;
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(this.url);
        request.Method = "POST";

        System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
        Byte[] byteArray = encoding.GetBytes(data);

        request.ContentLength = byteArray.Length;
        request.ContentType = @"application/json";

        using (Stream dataStream = request.GetRequestStream())
        {
            dataStream.Write(byteArray, 0, byteArray.Length);
        }
       
        try
        {
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {

                using (Stream responseStream = response.GetResponseStream())
                using (StreamReader responseStreamReader = new StreamReader(responseStream))
                {
                    result = responseStreamReader.ReadToEnd();
                }
            }
        }
        catch (WebException ex)
        {
            result = null;
        }
        return result;
    }

    public string Get()
    {
        string result = null;
        try
        {
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.Method = "GET";
            using (HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse())
            using (Stream responseStream = httpWebResponse.GetResponseStream())
            using (StreamReader responseStreamReader = new StreamReader(responseStream))
            {
                result = responseStreamReader.ReadToEnd();

            }
        }
        catch (Exception exp)
        {
            result = null;
        }
        return result;
    }
}