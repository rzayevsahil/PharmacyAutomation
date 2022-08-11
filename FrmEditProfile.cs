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
    public partial class FrmEditProfile : Form
    {
        public FrmEditProfile()
        {
            InitializeComponent();
        }
        public string personID;
        SqlConnection connection = new SqlConnection("Data Source=DESKTOP-2H5V0KB\\SQLEXPRESS;Initial Catalog=DbPharmacy;Integrated Security=True");

        public string AdminID()
        {
            string adminID=null;
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

        public string IsAdmin(string personID)
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

        public void GoAdminOrSellerDashboard()
        {
            if (IsAdmin(personID) == AdminID())
            {
                FrmAdminDashboard frmAdminDashboard = new FrmAdminDashboard();
                frmAdminDashboard.username = txtUserName.Text;
                frmAdminDashboard.personID = personID;
                frmAdminDashboard.Show();
            }
            else
            {
                FrmSellerDashboard frmSellerDashboard = new FrmSellerDashboard();
                frmSellerDashboard.username = txtUserName.Text;
                frmSellerDashboard.personID = personID;
                frmSellerDashboard.Show();
            }
        }
        private void btnBack_Click(object sender, EventArgs e)
        {
            GoAdminOrSellerDashboard();
            Close();
        }

        private void FrmEditProfile_Load(object sender, EventArgs e)
        {
            connection.Open();
            SqlCommand command1 = new SqlCommand("Select PersonID,PersonName,PersonSurname,UserName,Password From TblPerson Where PersonID=@p1", connection);
            command1.Parameters.AddWithValue("@p1", personID);
            SqlDataReader dataReader1 = command1.ExecuteReader();
            if (dataReader1.Read())
            {
                txtName.Text = dataReader1["PersonName"].ToString();
                txtSurname.Text = dataReader1["PersonSurname"].ToString();
                txtUserName.Text = dataReader1["UserName"].ToString();
                txtOldPassword.Text = dataReader1["Password"].ToString();
                txtOldPassword.UseSystemPasswordChar = false;
                txtOldPassword.Enabled = false;
            }
            connection.Close();
        }

        private void eyePassword_Click(object sender, EventArgs e)
        {
            eyePassword.Visible = false;
            txtPassword.UseSystemPasswordChar = false;
            eyeClosedPassword.Visible = true;
        }

        private void eyeClosedPassword_Click(object sender, EventArgs e)
        {
            eyePassword.Visible = true;
            txtPassword.UseSystemPasswordChar = true;
            eyeClosedPassword.Visible = false;
        }

        private void eyePasswordRepeat_Click(object sender, EventArgs e)
        {
            eyePasswordRepeat.Visible = false;
            txtPasswordRepeat.UseSystemPasswordChar = false;
            eyeClosedPasswordRepeat.Visible = true;
        }

        private void eyeClosedPasswordRepeat_Click(object sender, EventArgs e)
        {
            eyePasswordRepeat.Visible = true;
            txtPasswordRepeat.UseSystemPasswordChar = true;
            eyeClosedPasswordRepeat.Visible = false;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (txtPassword.Text != txtOldPassword.Text)
            {
                if (txtPassword.Text == txtPasswordRepeat.Text)
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("Update TblPerson Set PersonName=@p1,PersonSurname=@p2,UserName=@p3,Password=@p4 Where PersonID=@p5", connection);
                    command.Parameters.AddWithValue("@p1", txtName.Text);
                    command.Parameters.AddWithValue("@p2", txtSurname.Text);
                    command.Parameters.AddWithValue("@p3", txtUserName.Text);
                    command.Parameters.AddWithValue("@p4", txtPassword.Text);
                    command.Parameters.AddWithValue("@p5", personID);
                    command.ExecuteNonQuery();
                    connection.Close();
                    MessageBox.Show("Hesabınız başarıyla güncellendi", "Bilgi", MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                    GoAdminOrSellerDashboard();
                    //FrmAdminDashboard frmAdminDashboard = new FrmAdminDashboard();
                    //frmAdminDashboard.username = txtUserName.Text;
                    //frmAdminDashboard.personID = personID;
                    //frmAdminDashboard.Show();
                    Close();
                }
                else
                {
                    MessageBox.Show("Şifreler birbiriyle eşleşmiyor", "Uyarı", MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                }
            }
            else
            {
                MessageBox.Show("Lütfen eski şifrenizden farklı bir şifre giriniz", "Uyarı", MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

            }
        }
    }
}
