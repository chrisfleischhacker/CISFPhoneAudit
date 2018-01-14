using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Personnel;
using Phones;

public partial class _PhoneList : System.Web.UI.Page
{
    string iAm;
    Person IAM;        

    protected void Page_Load(object sender, EventArgs e)
    {
        iAm = Person.LogonUserIdentity();
        IAM = Person.GetPersonFromActiveDirectory(iAm);
        if (!Page.IsPostBack)
        {
            GetAllPhones();
        }
    }

    private void GetAllPhones()
    {
        bool iAmAuditor = false;
        string[] arrauditors = System.Configuration.ConfigurationManager.AppSettings["Auditors"].Split(',');
        foreach (string auditor in arrauditors)
        {
            if (IAM.WindowsLogonUserName == auditor)
            {
                iAmAuditor = true;
            }
        }
        if (iAmAuditor)
        {
            int allphonescount, respondercount;
            allphonescount = Phones.Phones.GetAllPhones().Count;
            dlAllPhones.DataSource = Phones.Phones.GetAllPhones();
            dlAllPhones.DataBind();
            if (allphonescount > 0) { dlAllPhones.Visible = true; }
            respondercount = Phones.Phones.GetResponderCount();
            lblError.Text += "List of " + respondercount.ToString() + " users confirming " + allphonescount.ToString() + " phones.";
        }
        else
        {
            lblError.Text = "You are not an auditor.";
        }
    }

    protected void Delete_Click(Object sender, DataListCommandEventArgs e)
    {
        Label lblIdent = (Label)e.Item.FindControl("lblIdent");
        Phones.Phones.DeletePhone(Convert.ToInt32(lblIdent.Text));
        GetAllPhones();
    }

}

