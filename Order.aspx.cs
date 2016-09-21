using System;
using System.Collections.Generic;

using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Threading;

public partial class _Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            if (Session["ProcessingFileId"] != null)
            {
                /*The process alreday in progress.*/
                DisableFields();
            }
        }
    }

    private void DisableFields()
    {
        this.lblStatus.Text = "Order Creation Is in Progress";
        this.fleCustomerData.Enabled = false;
        this.btnUpload.Enabled = false;
        this.btnUpload.Text = "Processing..";
        this.divStatus.Style.Remove("display");
        this.Timer1.Enabled = true;
    }

    private void EnableFields()
    {
        this.lblStatus.Text = "";
        this.fleCustomerData.Enabled = true;
        this.btnUpload.Enabled = true;
        this.btnUpload.Text = "Upload";

        this.Timer1.Enabled = false;
        Session.Remove("ProcessingFileId");
    }
    protected void btnUpload_Click(object sender, EventArgs e)
    {
        try
        {
            if (Session["ProcessingFileId"] != null)
            {
                throw new Exception("Order processing alreday in Progress...s");
            }

            string uniqueFileName = Guid.NewGuid().ToString();
            string filePath = Server.MapPath("~\\Uploads\\" + uniqueFileName + ".csv");
            /*Save the file in temporary location*/
            this.fleCustomerData.SaveAs(filePath);

            OMRequest request = new OMRequest();
            request.fileName = this.fleCustomerData.FileName;
            request.filePath = filePath;
            request.property = System.Configuration.ConfigurationManager.AppSettings["Property"];
            request.url = System.Configuration.ConfigurationManager.AppSettings["API URL"];
            Session["ProcessingFileId"] = uniqueFileName;

            /*ABout to start processing so, disable fields to prevent subsequent request*/
            DisableFields();

            Thread newThread = new Thread(MakeOredr);

            newThread.Start(request);
        }
        catch (Exception exp)
        {
            this.lblStatus.Text = exp.Message;
        }
    }

    #region MakeOredr
    private void MakeOredr(Object request)
    {
        OMRequest omRequest = (OMRequest)request;
        StreamReader reader = new StreamReader(omRequest.filePath);
        string currentLine;
        StreamWriter logFileStream = File.AppendText(omRequest.filePath.Replace(".csv", ".log"));
        logFileStream.Write( "PROCESS STARTED<br/>");
        int recordCount = 0;
        while ((currentLine = reader.ReadLine()) != null)
        {
            recordCount++;
            string[] sections = currentLine.Split(',');
            if (sections == null || sections.Length != 2)
            {
                continue;
            }
            logFileStream.Write(DateTime.Now.ToString() + " Processing Record " + recordCount.ToString() + " - " + sections[0] + "</br>");
            Customer newOrder = new Customer();
            newOrder.action = "order created";
            newOrder.customer = sections[0];
            newOrder.file = omRequest.fileName;
            newOrder.property = omRequest.property;
            newOrder.value = sections[1];

            EvilAPIManager manager = new EvilAPIManager(omRequest.url);
            Status status = manager.MakeOrder(newOrder);
            if (status.added == "true")
            {
                Customer customer = manager.Check(status.hash);
                if (customer != null)
                {
                    logFileStream.Write(DateTime.Now.ToString() + "  Completed successfully</br>");
                }
                else
                {
                    logFileStream.Write(DateTime.Now.ToString() + "  Failed to chek the details.</br>");
                }
            }
            else
            {
                logFileStream.Write(DateTime.Now.ToString() + "  Order creation failed</br>");
            }

            logFileStream.Flush();

        }
        logFileStream.Write("PROCESS COMPLETED");

        logFileStream.Close();
    }

    public static void DoWork(Object request)
    {
        for (int i = 0; i < 1000; i++)
        {
            Thread.Sleep(1000);
            StreamWriter writter = File.AppendText(@"D:\Research\OrderMaker\Uploads\Log.csv");
            writter.WriteLine("Thank God");
            writter.Close();
        }

    }
    #endregion

    protected void Timer1_Tick(object sender, EventArgs e)
    {
        if (Session["ProcessingFileId"] != null)
        {
            string logFilePath = Server.MapPath("~\\Uploads\\" + Session["ProcessingFileId"] + ".log");

            var fs = new FileStream(logFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            using (var sr = new StreamReader(fs))
            {
                string log = sr.ReadToEnd();
                this.lblLog.Text = log;
                if (log.EndsWith("PROCESS COMPLETED"))
                {
                    EnableFields();
                }
            }
        }
        else
        {
            EnableFields();
        }

    }
}
