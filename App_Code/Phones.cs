using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace Phones
{
    public class Phone
    {
        public static string LogonUserIdentity()
        {
            string lui = "";
            lui = HttpContext.Current.Request.LogonUserIdentity.Name.Split(new char[] { '\\' })[1];
            //lui = "1132337280A";        //test iAm string for Jeffry Burnside
            //lui = "1020280260E";        //test iAm string for Lee Cone
            return lui;
        }

        public Phone() { }

        public Phone(
            int _Ident,
            string _EDIPI,
            string _Name,
            string _Location,
            string _Description,
            string _Phone
            ) { }

        public int _Ident { get; set; }
        public string _EDIPI { get; set; }
        public string _Name { get; set; }
        public string _Location { get; set; }
        public string _Description { get; set; }
        public string _Phone { get; set; }

        public static Phone GetPhone(SqlDataReader reader)
        {
            Phone phone = new Phone();
            if (reader.IsClosed)
                reader.Read();

            phone._Ident = (reader["ident"] != DBNull.Value ? Convert.ToInt32(reader["ident"]) : 0);
            phone._EDIPI = (reader["EDIPI"] != DBNull.Value ? reader["EDIPI"].ToString() : "");
            phone._Name = (reader["Name"] != DBNull.Value ? reader["Name"].ToString() : "");
            phone._Location = (reader["Location"] != DBNull.Value ? reader["Location"].ToString() : "");
            phone._Description = (reader["Description"] != DBNull.Value ? reader["Description"].ToString() : "");
            phone._Phone = (reader["Phone"] != DBNull.Value ? reader["Phone"].ToString() : "");

            return phone;
        }
    }

    public class PhoneCollection : List<Phone>
    {
    }

    public class Phones
    {
        public static PhoneCollection GetMyPhones(string edipi)
        {
            PhoneCollection ac = new PhoneCollection();
            using (SqlConnection conn = new SqlConnection(PhoneSQL.csPhone))
            {
                SqlCommand cmd = new SqlCommand(PhoneSQL.cGetMyPhones, conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@psEDIPI", edipi);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                FillPhoneList(ac, reader);
                reader.Close();
                conn.Close();

                return ac;
            }
        }

        public static PhoneCollection GetAllPhones()
        {
            PhoneCollection ac = new PhoneCollection();
            using (SqlConnection conn = new SqlConnection(PhoneSQL.csPhone))
            {
                SqlCommand cmd = new SqlCommand(PhoneSQL.cGetAllPhones, conn);
                cmd.CommandType = CommandType.StoredProcedure;
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                FillPhoneList(ac, reader);
                reader.Close();
                conn.Close();

                return ac;
            }
        }

        public static Int32 GetResponderCount()
        {
            using (SqlConnection conn = new SqlConnection(PhoneSQL.csPhone))
            {
                int ResponderCount;
                SqlCommand cmd = new SqlCommand(PhoneSQL.cGetResponderCount, conn);
                cmd.CommandType = CommandType.StoredProcedure;
                conn.Open();
                ResponderCount = Convert.ToInt32(cmd.ExecuteScalar());
                conn.Close();
                return ResponderCount;
            }
        }

        public static void FillPhoneList(PhoneCollection coll, SqlDataReader reader)
        {
            FillPhoneList(coll, reader, -1, 0);
        }

        public static void FillPhoneList(PhoneCollection coll, SqlDataReader reader, int totalRows, int firstRow)
        {
            int index = 0;
            bool readMore = true;

            while (reader.Read())
            {
                if (index >= firstRow && readMore)
                {
                    if (coll.Count >= totalRows && totalRows > 0)
                        readMore = false;
                    else
                    {
                        Phone trx = Phone.GetPhone(reader);
                        coll.Add(trx);
                    }
                }
                index++;
            }
        }

        public static Int32 InsertPhone(Phone p)
        {
            int NewPhoneId;
            NewPhoneId = InsertPhone(
                p._EDIPI
                , p._Name
                , p._Location
                , p._Description
                , p._Phone
                );
            return NewPhoneId;
        }

        public static Int32 InsertPhone(
            string _EDIPI
            , string _Name
            , string _Location
            , string _Description
            , string _Phone
            )
        {
            using (SqlConnection conn = new SqlConnection(PhoneSQL.csPhone))
            {
                int NewPhoneId;
                SqlCommand cmd = new SqlCommand(PhoneSQL.cInsert, conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@psEDIPI", _EDIPI);
                cmd.Parameters.AddWithValue("@psName", _Name);
                cmd.Parameters.AddWithValue("@psLocation", _Location);
                cmd.Parameters.AddWithValue("@psDescription", _Description);
                cmd.Parameters.AddWithValue("@psPhone", _Phone);
                conn.Open();
                NewPhoneId = Convert.ToInt32(cmd.ExecuteScalar());
                conn.Close();
                return NewPhoneId;
            }
        }

        public static Phone GetPhone(int pid)
        {
            using (SqlConnection conn = new SqlConnection(PhoneSQL.csPhone))
            {
                SqlCommand cmd = new SqlCommand(PhoneSQL.cGetPhone, conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ident", pid);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                reader.Read();
                Phone px = Phone.GetPhone(reader);
                reader.Close();
                conn.Close();
                return px;
            }
        }

        public static bool ExistingPhoneNumber(string p, string e)
        {
            using (SqlConnection conn = new SqlConnection(PhoneSQL.csPhone))
            {
                bool exists = false;
                SqlCommand cmd = new SqlCommand(PhoneSQL.cGetPhoneNumber, conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@psphone", p);
                cmd.Parameters.AddWithValue("@psEDIPI", e);
                conn.Open();
                if (Convert.ToInt32(cmd.ExecuteScalar()) > 0) { exists = true; }
                conn.Close();
                return exists;
            }
        }

        public static void UpdatePhone(Phone p)
        {
            UpdatePhone(
                p._Ident
                , p._EDIPI
                , p._Name
                , p._Location
                , p._Description
                , p._Phone
                );
        }

        public static void UpdatePhone(
            int _Ident
            , string _EDIPI
            , string _Name
            , string _Location
            , string _Description
            , string _Phone
            )
        {
            using (SqlConnection conn = new SqlConnection(PhoneSQL.csPhone))
            {
                SqlCommand cmd = new SqlCommand(PhoneSQL.cUpdate, conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@piident", _Ident);
                cmd.Parameters.AddWithValue("@psEDIPI", _EDIPI);
                cmd.Parameters.AddWithValue("@psName", _Name);
                cmd.Parameters.AddWithValue("@psLocation", _Location);
                cmd.Parameters.AddWithValue("@psDescription", _Description);
                cmd.Parameters.AddWithValue("@psPhone", _Phone);
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
                return;
            }
        }

        public static void DeletePhone(Phone p)
        {
            DeletePhone(p._Ident);
        }

        public static void DeletePhone(int pid)
        {
            using (SqlConnection conn = new SqlConnection(PhoneSQL.csPhone))
            {
                SqlCommand cmd = new SqlCommand(PhoneSQL.cDelete, conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@piident", pid);
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
                return;
            }
        }

    }
    public class PhoneSQL
    {
        public static string csPhone = System.Configuration.ConfigurationManager.ConnectionStrings["PhoneAudit"].ToString();
        public static string cInsert = "dbo.InsertPhone";
        public static string cGetPhone = "dbo.GetPhone";
        public static string cGetPhoneNumber = "dbo.GetPhoneNumber";
        public static string cGetMyPhones = "dbo.GetMyPhones";
        public static string cGetAllPhones = "dbo.GetAllPhones";
        public static string cGetResponderCount = "dbo.GetResponderCount";
        public static string cUpdate = "dbo.UpdatePhone";
        public static string cDelete = "dbo.DeletePhone";
    }
}