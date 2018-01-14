using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.Security.Principal;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace Personnel
{
    public class Person
    {
        public static string LogonUserIdentity()
        {
            string lui = "";
            lui = HttpContext.Current.Request.LogonUserIdentity.Name.Split(new char[] { '\\' })[1];
            //lui = "1132337280A";        //test iAm string for Jeffry Burnside
            //lui = "1020280260E";        //test iAm string for Lee Cone
            return lui;
        }

        /// <summary>
        /// Returns A Person.
        /// Person IAM = Person.GetCachedPerson(EDI-PI);
        /// </summary>
        /// <param name="EDI-PI">1212161825E</param>

        public Person() { }

        public Person(
            string EmployeeId,
            string DisplayName,
            string WindowsLogonUserName,
            int QID,
            string EmailAddress,
            string PhoneNumber,
            string DsnNumber,
            string FaxNumber,
            string Rank,
            string Organization,
            string EmployeeType,
            string Location,
            int Enabled
            ) { }

        public string EmployeeID { get; set; }
        public string DisplayName { get; set; }
        public string WindowsLogonUserName { get; set; }
        public int QID { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }
        public string DsnNumber { get; set; }
        public string FaxNumber { get; set; }
        public string Rank { get; set; }
        public string Organization { get; set; }
        public string EmployeeType { get; set; }
        public string Location { get; set; }
        public int Enabled { get; set; }
        public string LastLogin { get; set; }

        public bool IsUserAccountEnabled
        {
            get
            {
                if (Convert.ToBoolean(false)) //Convert.ToBoolean(UserAccountControlFlag & AD_UserProperties.ADS_UF_ACCOUNTDISABLE))
                    return false;
                else
                    return true;
            }
        }

        public static Person GetPerson(Principal principal, string fullDomain)
        {
            Person person = new Person();
            string filter = string.Format("(&(ObjectClass={0})(sAMAccountName={1}))", "person", principal);
            string[] properties = new string[] { "fullname" };
            DirectoryEntry adRoot = new DirectoryEntry(fullDomain, null, null, AuthenticationTypes.Secure);
            DirectorySearcher searcher = new DirectorySearcher(adRoot);
            searcher.SearchScope = SearchScope.Subtree;
            searcher.ReferralChasing = ReferralChasingOption.All;
            searcher.PropertiesToLoad.AddRange(properties);
            searcher.Filter = filter;
            DirectoryEntry directoryEntry = principal.GetUnderlyingObject() as DirectoryEntry;
            person.EmployeeID = (directoryEntry.Properties["employeeId"].Value != null ? directoryEntry.Properties["employeeId"][0].ToString() : "");
            person.DisplayName = (directoryEntry.Properties["displayName"].Value != null ? directoryEntry.Properties["displayName"][0].ToString() : "");
            person.WindowsLogonUserName = (directoryEntry.Properties["sAMAccountName"].Value != null ? directoryEntry.Properties["sAMAccountName"][0].ToString() : "");
            person.EmailAddress = (directoryEntry.Properties["mail"].Value != null ? directoryEntry.Properties["mail"][0].ToString() : "");
            person.PhoneNumber = (directoryEntry.Properties["otherTelephone"].Value != null ? directoryEntry.Properties["otherTelephone"][0].ToString() : "");
            person.DsnNumber = (directoryEntry.Properties["telephoneNumber"].Value != null ? directoryEntry.Properties["telephoneNumber"][0].ToString() : "");
            person.FaxNumber = (directoryEntry.Properties["facsimileTelephoneNumber"].Value != null ? directoryEntry.Properties["facsimileTelephoneNumber"][0].ToString() : "");
            person.Rank = (directoryEntry.Properties["personalTitle"].Value != null ? directoryEntry.Properties["personalTitle"][0].ToString() : "");
            person.Organization = (directoryEntry.Properties["physicalDeliveryOfficeName"].Value != null ? directoryEntry.Properties["physicalDeliveryOfficeName"][0].ToString() : "");
            person.EmployeeType = (directoryEntry.Properties["employeeType"].Value != null ? directoryEntry.Properties["employeeType"][0].ToString() : "");
            person.Enabled = (directoryEntry.Properties["userAccountControl"].Value != null ? Convert.ToInt32(directoryEntry.Properties["userAccountControl"][0]) : 0);

            string[] localInfo = GetLocalInfo(person.WindowsLogonUserName);
            person.QID = Convert.ToInt32(localInfo[0]);
            person.Location = localInfo[1];

            return person;
        }

        public static Person GetPersonFromADbyEDIPI(Principal principal, string fullDomain)
        {
            Person person = new Person();
            string filter = string.Format("(&(ObjectClass={0})(sAMAccountName={1}))", "person", principal);
            string[] properties = new string[] { "SamAccountName" };
            DirectoryEntry adRoot = new DirectoryEntry(fullDomain, null, null, AuthenticationTypes.Secure);
            DirectorySearcher searcher = new DirectorySearcher(adRoot);
            searcher.SearchScope = SearchScope.Subtree;
            searcher.ReferralChasing = ReferralChasingOption.All;
            searcher.PropertiesToLoad.AddRange(properties);
            searcher.Filter = filter;
            DirectoryEntry directoryEntry = principal.GetUnderlyingObject() as DirectoryEntry;
            person.EmployeeID = (directoryEntry.Properties["employeeId"].Value != null ? directoryEntry.Properties["employeeId"][0].ToString() : "");
            person.DisplayName = (directoryEntry.Properties["displayName"].Value != null ? directoryEntry.Properties["displayName"][0].ToString() : "");
            person.WindowsLogonUserName = (directoryEntry.Properties["sAMAccountName"].Value != null ? directoryEntry.Properties["sAMAccountName"][0].ToString() : "");
            person.EmailAddress = (directoryEntry.Properties["mail"].Value != null ? directoryEntry.Properties["mail"][0].ToString() : "");
            person.PhoneNumber = (directoryEntry.Properties["otherTelephone"].Value != null ? directoryEntry.Properties["otherTelephone"][0].ToString() : "");
            person.DsnNumber = (directoryEntry.Properties["telephoneNumber"].Value != null ? directoryEntry.Properties["telephoneNumber"][0].ToString() : "");
            person.FaxNumber = (directoryEntry.Properties["facsimileTelephoneNumber"].Value != null ? directoryEntry.Properties["facsimileTelephoneNumber"][0].ToString() : "");
            person.Rank = (directoryEntry.Properties["personalTitle"].Value != null ? directoryEntry.Properties["personalTitle"][0].ToString() : "");
            person.Organization = (directoryEntry.Properties["physicalDeliveryOfficeName"].Value != null ? directoryEntry.Properties["physicalDeliveryOfficeName"][0].ToString() : "");
            person.EmployeeType = (directoryEntry.Properties["employeeType"].Value != null ? directoryEntry.Properties["employeeType"][0].ToString() : "");
            person.Enabled = (directoryEntry.Properties["userAccountControl"].Value != null ? Convert.ToInt32(directoryEntry.Properties["userAccountControl"][0]) : 0);

            string[] localInfo = GetLocalInfo(person.WindowsLogonUserName);
            person.QID = Convert.ToInt32(localInfo[0]);
            person.Location = localInfo[1];

            return person;
        }

        public static string[] GetLocalInfo(string edipi)
        {
            StringBuilder sb = new StringBuilder();
            SqlCommand sqlCmd = new SqlCommand();
            try
            {
                sqlCmd.Connection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Queue"].ToString());
                sqlCmd.CommandType = CommandType.Text;
                sqlCmd.CommandText = "select [Queue].[dbo].[CISFUser].[ident] as qid, [Queue].[dbo].[CISFUser].[Location_Hierarchy_String] as Location from [Queue].[dbo].[CISFUser] where [Queue].[dbo].[CISFUser].[User_Name] = @edipi";
                //sqlCmd.CommandText = "select [CISF_People].[dbo].[People].[qid] as qid, [CISF_Locations].[dbo].[Full_Locations].[HierarchyString] as Location from [CISF_People].[dbo].[People] JOIN [CISF_Locations].[dbo].[Full_Locations] ON ([CISF_People].[dbo].[People].[location_ident] = [CISF_Locations].[dbo].[Full_Locations].[ident]) where [CISF_People].[dbo].[People].[EDI-PI] = @edipi";
                sqlCmd.Parameters.AddWithValue("@edipi", edipi);
                sqlCmd.Connection.Open();
                SqlDataReader reader = sqlCmd.ExecuteReader();
                int rowCount = 0;
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        rowCount++;
                        sb.Append(reader.IsDBNull(0) ? "0" : reader.GetInt32(0).ToString() + ", ");
                        sb.Append(reader.IsDBNull(1) ? "Unknown Location" : reader.GetString(1));
                    }
                    reader.Close();
                }
                else
                {
                    sb.Append("0, No Location");
                }
                if (rowCount > 1)
                {
                    sb.Clear();
                    sb.Append("0, Multiple Records");
                }
            }
            catch (Exception ex_GetLocalInfo)
            {
                string xxx = ex_GetLocalInfo.ToString();
                throw ex_GetLocalInfo;
            }
            finally
            {
                sqlCmd.Connection.Close();
            }

            return Regex.Split(sb.ToString(), ", ");
        }


        /// <summary>
        /// Returns A Person from AD.
        /// Usage:
        /// Person IAM = Person.GetPersonFromActiveDirectory(edipi);
        /// </summary>
        /// <param name="edipi">1212161825E</param>

        public static Person GetPersonFromActiveDirectory(string edipi)
        {
            Person thisPerson = new Person();
            string ADDomain = System.Configuration.ConfigurationManager.AppSettings["ADDomain"];
            string LDAPPath = System.Configuration.ConfigurationManager.AppSettings["LDAPPath"];

            PrincipalContext ctx = new PrincipalContext(ContextType.Domain, ADDomain);
            Principal principal = Principal.FindByIdentity(ctx, IdentityType.SamAccountName, edipi);
            if (principal != null)
            {
                thisPerson = (Person)Person.GetPersonFromADbyEDIPI(principal, LDAPPath);
            }
            ctx.Dispose();
            return thisPerson;
        }


        public static string GetInformation(Person person)
        {
            StringBuilder sb = new StringBuilder();
            string displayname = (person.DisplayName == null ? "" : person.DisplayName);
            string email = (person.EmailAddress == null ? "" : person.EmailAddress);
            string phone = (person.PhoneNumber == null ? "" : person.PhoneNumber);
            string dsn = (person.DsnNumber == null ? "" : person.DsnNumber);
            string fax = (person.FaxNumber == null ? "" : person.FaxNumber);
            string edipi = (person.WindowsLogonUserName == null ? "" : person.WindowsLogonUserName);
            int qid = (person.QID == 0 ? 0 : person.QID);
            sb.Append(@"<p class='bold'>" + displayname + @"</p>");
            sb.Append(@"<p class='bold'>Email: <a href='mailto:" + email + @"'>" + email + @"</a></p>");
            sb.Append(@"<p class='bold'>Phone: " + phone + @",   ");
            sb.Append(@"DSN: " + dsn + @",   ");
            sb.Append(@"FAX: " + fax + @"</p>");
            sb.Append(@"<p class='bold'>EDI-PI: " + edipi + @"</p>");
            sb.Append(@"<p class='bold'>QID: <a href='https://cisf13.peterson.af.mil/nexusqueue/index.aspx?page=4&state=3&userident=" + qid.ToString() + @"' Target='_blank'>" + qid.ToString() + @"</a></p>");

            return sb.ToString();
        }
    }
}
