using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Personnel;
using Phones;
using System.Text.RegularExpressions;

public partial class _Default : System.Web.UI.Page
{
    string iAm;
    Person IAM;        

    protected void Page_Load(object sender, EventArgs e)
    {
        iAm = Person.LogonUserIdentity();
        IAM = Person.GetPersonFromActiveDirectory(iAm);
        if (!Page.IsPostBack)
        {
            GetMyInformation(IAM);
            InsertFromAD();
            GetMyPhones(IAM);
        }
    }


    private void GetMyInformation(Person IAM)
    {
        Label lblIAM = new Label();
        lblIAM.Text = IAM.DisplayName.ToString();
        phForm.Controls.Add(lblIAM);

        if (IAM.EmailAddress.Length >1)
        {
            phForm.Controls.Add(new LiteralControl(@"</br>"));
            Label lblEmail = new Label();
            lblEmail.Text = @"<a href='mailto:" + IAM.EmailAddress.ToString() + "'>" + IAM.EmailAddress.ToString() + @"</a>";
            phForm.Controls.Add(lblEmail); 
        }
        if (IAM.Location.Length > 1)
        {
            phForm.Controls.Add(new LiteralControl(@"</br>"));
            Label lblLocation = new Label();
            lblLocation.Text = IAM.Location.ToString();
            phForm.Controls.Add(lblLocation);
        }

        phForm.Controls.Add(new LiteralControl(@"</br>"));
        phForm.Controls.Add(new LiteralControl(@"<p>I am using these telephone extensions:</p>"));
    }

    private void GetMyPhones(Person IAM)
    {
        int myphonescount;
        myphonescount = Phones.Phones.GetMyPhones(IAM.WindowsLogonUserName).Count;
        dlMyPhones.DataSource = Phones.Phones.GetMyPhones(IAM.WindowsLogonUserName);
        dlMyPhones.DataBind();
        //if (myphonescount > 0) { dlMyPhones.Visible = true; }
    }

    private void InsertFromAD()
    {
        if ((IAM.DsnNumber.Length > 4) && (FormatPhone(IAM.DsnNumber) != ""))
        {
            if (!Phones.Phones.ExistingPhoneNumber(FormatPhone(IAM.DsnNumber), IAM.WindowsLogonUserName))
            {
                Phone q = new Phone();
                q._Description = "Desk";
                q._EDIPI = IAM.WindowsLogonUserName;
                q._Ident = 0;
                q._Location = IAM.Location;
                q._Name = IAM.DisplayName;
                q._Phone = FormatPhone(IAM.DsnNumber);
                int d = Phones.Phones.InsertPhone(q);
            }
        }
        if ((IAM.PhoneNumber.Length > 4) && (FormatPhone(IAM.PhoneNumber) != ""))
        {
            if (!Phones.Phones.ExistingPhoneNumber(FormatPhone(IAM.PhoneNumber), IAM.WindowsLogonUserName))
            {
                Phone q = new Phone();
                q._Description = "Desk";
                q._EDIPI = IAM.WindowsLogonUserName;
                q._Ident = 0;
                q._Location = IAM.Location;
                q._Name = IAM.DisplayName;
                q._Phone = FormatPhone(IAM.PhoneNumber);
                int p = Phones.Phones.InsertPhone(q);
            }
        }
        if ((IAM.FaxNumber.Length > 4) && (FormatPhone(IAM.FaxNumber) != ""))
        {
            if (!Phones.Phones.ExistingPhoneNumber(FormatPhone(IAM.FaxNumber), IAM.WindowsLogonUserName))
            {
                Phone q = new Phone();
                q._Description = "Fax";
                q._EDIPI = IAM.WindowsLogonUserName;
                q._Ident = 0;
                q._Location = IAM.Location;
                q._Name = IAM.DisplayName;
                q._Phone = FormatPhone(IAM.FaxNumber);
                int f = Phones.Phones.InsertPhone(q);
            }
        }
    }

    protected string FormatPhone(string n)
    {
        string test = "";
        string phone = "";
        if (n.Length > 4)
        {
            switch (n.Substring(0, 3))
            {
                case "834":
                    test = n.Length > 3 ? n.Substring(3, n.Length - 3) : "";
                    test = n.Substring(0, n.Length >= 3 ? 3 : n.Length).Replace("834", "556") + test;
                    break;
                case "692":
                    test = n.Length > 3 ? n.Substring(3, n.Length - 3) : "";
                    test = n.Substring(0, n.Length >= 3 ? 3 : n.Length).Replace("692", "554") + test;
                    break;
                default:
                    test = n;
                    break;
            }
            Regex regexObj = new Regex(@"^([0-9]{3})?[-. ]?([0-9]{4})$");    //Regex regexObj = new Regex(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$");
            if (regexObj.IsMatch(test))
            {
                phone = regexObj.Replace(test, "$1-$2");
            }
            else
            {
                phone = "";
            }
        }
        return phone;
    }


    protected void Add_Click(object sender, EventArgs e)
    {
        TextBox txtPhone = (TextBox)dlMyPhones.Controls[0].Controls[0].FindControl("txtPhone");
        TextBox txtDescription = (TextBox)dlMyPhones.Controls[0].Controls[0].FindControl("txtDescription");
        Label lblError = (Label)dlMyPhones.Controls[0].Controls[0].FindControl("lblError");
        lblError.Text = "";
        string phone = FormatPhone(txtPhone.Text);
        if (phone == "")
        {
            lblError.Text = "Invalid phone number.  I'm looking for a 7 digit phone number like 5541234 or 556-1234.";
            GetMyInformation(IAM);
            return;
        }
        if (Phones.Phones.ExistingPhoneNumber(phone, IAM.WindowsLogonUserName))
        {
            lblError.Text = "You have already confirmed that phone number.";
            GetMyInformation(IAM);
            return;
        }
        Phone q = new Phone();
        q._Description = txtDescription.Text;
        q._EDIPI = IAM.WindowsLogonUserName;
        q._Ident = 0;
        q._Location = IAM.Location;
        q._Name = IAM.DisplayName;
        q._Phone = phone;
        int i = Phones.Phones.InsertPhone(q);

        GetMyInformation(IAM);
        GetMyPhones(IAM);
    }
    protected void Delete_Click(Object sender, DataListCommandEventArgs e) 

    {
        Label lblIdent = (Label)e.Item.FindControl("lblIdent");
        Phones.Phones.DeletePhone(Convert.ToInt32(lblIdent.Text));
        GetMyInformation(IAM);
        GetMyPhones(IAM);
    }
}

