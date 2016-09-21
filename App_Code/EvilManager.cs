using System;
using System.Collections.Generic;

using System.Web;
using System.Net;
using System.IO;
using System.Web.Script.Serialization;
using System.Text;
/// <summary>
/// Summary description for EvilManager
/// </summary>
public class EvilAPIManager
{
    private string url;

    public EvilAPIManager(string url)
	{
        this.url = url;
	}

    public Status MakeOrder(Customer newOrder)
    {
        var jsonString = new JavaScriptSerializer().Serialize(newOrder);
        JSONRequestManager jsonManager = new JSONRequestManager(url + "/upload");
        string result = jsonManager.POST(jsonString);
        return new JavaScriptSerializer().Deserialize<Status>(result);
    }
    public Customer Check(string hash)
    {
        Customer customer = null;
        try
        {
            JSONRequestManager jsonManager = new JSONRequestManager(url + "/check?hash=" + hash);
            string result = jsonManager.Get();
            if (!string.IsNullOrEmpty(result))
            {
                customer = new JavaScriptSerializer().Deserialize<Customer>(result);
            }
        }
        catch (Exception exp)
        {
            customer = null;
        }
        return customer;
    }
    
    
}