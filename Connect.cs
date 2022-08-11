using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmacyAutomation
{
    public static class Connect
    {
        public static SqlConnection connection = new SqlConnection("Data Source=DESKTOP-2H5V0KB\\SQLEXPRESS;Initial Catalog=DbPharmacy;Integrated Security=True");
        public static string AdminID()
        {
            string adminID = null;
            connection.Open();
            SqlCommand command1 = new SqlCommand("Select AuthorityID From TblAuthority Where AuthorityName = 'admin'", connection);
            SqlDataReader dataReader1 = command1.ExecuteReader();
            if (dataReader1.Read())
            {
                adminID = dataReader1[0].ToString();
            }
            connection.Close();
            return adminID;
        }

        public static string IsAdmin(string personID)
        {
            string adminID = null;
            connection.Open();
            SqlCommand command = new SqlCommand("Select AuthorityID From TblPersonAuthority Where PersonID=@p1", connection);
            command.Parameters.AddWithValue("@p1", personID);
            SqlDataReader dataReader = command.ExecuteReader();
            if (dataReader.Read())
            {
                adminID = dataReader[0].ToString();
            }
            connection.Close();
            return adminID;
        }
    }
}
