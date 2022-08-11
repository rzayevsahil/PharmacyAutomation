using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace PharmacyAutomation
{
    public partial class FrmLogin : Form
    {
        public FrmLogin()
        {
            InitializeComponent();
        }
        public string personID;

        SqlConnection connection = new SqlConnection("Data Source=DESKTOP-2H5V0KB\\SQLEXPRESS;Initial Catalog=DbPharmacy;Integrated Security=True");

        //public string PersonID(string adminID)
        //{
        //    connection.Open();
        //    SqlCommand command3 = new SqlCommand("Select PersonID From TblAdmin Where PersonID=@p1", connection);
        //    command3.Parameters.AddWithValue("@p1", adminID.ToString());
        //    SqlDataReader dataReader3 = command3.ExecuteReader();
        //    if (dataReader3.Read())
        //    {
        //        personID = dataReader3[0].ToString();
        //    }
        //    connection.Close();
        //    return personID;
        //}
        //public bool IsSuccessLogin()
        //{
        //    bool isSuccess = false;
        //    connection.Open();
        //    SqlCommand command3 = new SqlCommand("Execute isLogin @username=@p1, @password=@p2", connection);
        //    command3.Parameters.AddWithValue("@p1", txtUsername.Text);
        //    command3.Parameters.AddWithValue("@p2", txtPassword.Text);
        //    SqlDataReader dataReader3 = command3.ExecuteReader();
        //    if (dataReader3.Read())
        //    {
        //        isSuccess = true;
        //    }
        //    connection.Close();
        //    return isSuccess;
        //}


        public string AdminID()
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

        public string SellerID()
        {
            string sellerID = null;
            connection.Open();
            SqlCommand command = new SqlCommand("Select AuthorityID From TblAuthority Where AuthorityName = 'seller'", connection);
            SqlDataReader dataReader = command.ExecuteReader();
            if (dataReader.Read())
            {
                sellerID = dataReader[0].ToString();
            }
            connection.Close();
            return sellerID;
        }
        public string pID()
        {
            string personID = null;
            connection.Open();
            SqlCommand command3 = new SqlCommand("Select PersonID From TblPerson Where UserName=@p1 and Password=@p2", connection);
            command3.Parameters.AddWithValue("@p1", txtUsername.Text);
            command3.Parameters.AddWithValue("@p2", txtPassword.Text);
            SqlDataReader dataReader3 = command3.ExecuteReader();
            if (dataReader3.Read())
            {
                personID = dataReader3["PersonID"].ToString();
            }
            connection.Close();
            return personID;
        }

        
        public string IsHavingAuthorityID()
        {
            string authorityID = null;
            connection.Open();
            SqlCommand command2 = new SqlCommand("Execute AuthorityName @username=@p1, @password=@p2", connection);
            command2.Parameters.AddWithValue("@p1", txtUsername.Text);
            command2.Parameters.AddWithValue("@p2", txtPassword.Text);
            SqlDataReader dataReader2 = command2.ExecuteReader();
            if (dataReader2.Read())
            {
                authorityID = dataReader2["AuthorityID"].ToString();
            }
            connection.Close();
            return authorityID;
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (pID() != null)
            {
                if (IsHavingAuthorityID() != null) 
                {
                    if (IsHavingAuthorityID()==AdminID())
                    {
                        FrmAdminDashboard frmAdminDashboard = new FrmAdminDashboard();
                        frmAdminDashboard.username = txtUsername.Text;
                        frmAdminDashboard.personID = pID();
                        frmAdminDashboard.Show();
                    }
                    else
                    {
                        FrmSellerDashboard frmSellerDashboard = new FrmSellerDashboard();
                        frmSellerDashboard.username = txtUsername.Text;
                        frmSellerDashboard.personID = pID();
                        frmSellerDashboard.Show();
                    }
                    txtPassword.Text = null;
                    txtUsername.Text = null;
                }
                else
                {
                    MessageBox.Show("Sisteme girmek için hiçbir yetkiniz yok!", "Dikkat", MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                }
            }
            else
            {
                MessageBox.Show("Hatalı kullanıcı adı veya şifre", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


            
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            FrmRegister frmRegister = new FrmRegister();
            frmRegister.Show();
        }

        private void eyePassword_Click(object sender, EventArgs e)
        {
            eyePassword.Visible = false;
            eyeClosedPassword.Visible = true;
            txtPassword.UseSystemPasswordChar = false;
        }

        private void eyeClosedPassword_Click(object sender, EventArgs e)
        {
            eyeClosedPassword.Visible = false;
            eyePassword.Visible = true;
            txtPassword.UseSystemPasswordChar = true;
        }
    }
}
