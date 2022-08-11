using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PharmacyAutomation
{
    public partial class FrmAuthorize : Form
    {
        public FrmAuthorize()
        {
            InitializeComponent();
        }
        public string personID;
        public string username;
        SqlConnection connection = new SqlConnection("Data Source=DESKTOP-2H5V0KB\\SQLEXPRESS;Initial Catalog=DbPharmacy;Integrated Security=True");

        public void GoAdminOrSellerDashboard()
        {

            if (Connect.IsAdmin(personID) == Connect.AdminID())
            {
                FrmAdminDashboard frmAdminDashboard = new FrmAdminDashboard();
                frmAdminDashboard.username = username;
                frmAdminDashboard.personID = personID;
                frmAdminDashboard.Show();
            }
            else
            {
                FrmSellerDashboard frmSellerDashboard = new FrmSellerDashboard();
                frmSellerDashboard.username = username;
                frmSellerDashboard.personID = personID;
                frmSellerDashboard.Show();
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            GoAdminOrSellerDashboard();
            Close();
        }
        public void Notification()
        {
            connection.Open();
            SqlCommand command = new SqlCommand("Select * from TblPerson where PersonID not in (select PersonID from TblPersonAuthority)", connection);
            SqlDataReader dataReader = command.ExecuteReader();
            if (dataReader.Read())
            {
                lblPersonelID.Text = dataReader[0].ToString();
                lblPersonName.Text = dataReader[1].ToString();
                lblPersonelSurname.Text = dataReader[2].ToString();
                LblUserName.Text = dataReader[3].ToString();
            }
            connection.Close();
        }

        public void Authorities()
        {
            SqlCommand command = new SqlCommand("select * from TblAuthority", connection);
            SqlDataAdapter adapter1 = new SqlDataAdapter(command);
            DataTable dataTable = new DataTable();
            adapter1.Fill(dataTable);
            cmbAuthority.ValueMember = "AuthorityID";
            cmbAuthority.DisplayMember = "AuthorityName";
            cmbAuthority.DataSource = dataTable;
        }
        private void FrmAuthorize_Load(object sender, EventArgs e)
        {
            Notification();
            Authorities();
        }
    }
}
