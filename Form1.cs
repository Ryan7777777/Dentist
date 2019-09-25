using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data;
using MySql.Data.MySqlClient;




namespace hhl26_Homework1
{

    public partial class frmDentist : Form
    {
        public Boolean dayofbirth = false;
        public int age;
        public string clinet_id;
        public string hy_dentist_id = "";
        public string carc_dentist_id= "";
        public string check_up_dentist_id="";
        public string root_dentist_id="";
        public string client_name;
        public ArrayList record = new ArrayList();
        public ArrayList appointment_id = new ArrayList();
        public string select_id = "";
        public frmDentist()
        {
            InitializeComponent();


        }


        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click_1(object sender, EventArgs e)
        {

        }

        private void cbChild_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            //remove all string on label display and false all check box

            chkHygiebistTreatment.Checked = false;
            chkCheckUpExam.Checked = false;
            chkCercFilling.Checked = false;
            chkRootCannal.Checked = false;
            lblDisplayTotalFree.Text = "";
            lblDisplayDiscount.Text = "";
            lblDisplayDueDate.Text = "";

        }

        private void btnDisplayFree_Click(object sender, EventArgs e)
        {   //declear variables
            decimal decServiceFree = 0m;
            decimal decTotalAfterDiscount;
            bool bolServiceChecked = false;
            bool IsChild = false;
            DateTime today = DateTime.Now;
            DateTime dueday = today.AddDays(5) + new TimeSpan(12, 00, 00);
            if (age < 12)
            {
                IsChild = true;
            }
            //look up which service have been clicked
            if (chkHygiebistTreatment.Checked == true && IsChild == false)
            {
                bolServiceChecked = true;
                if (IsChild == false)
                {
                    decServiceFree += 119.5m;
                }

            }
            if (chkCheckUpExam.Checked == true)
            {
                decServiceFree += 100m;
                bolServiceChecked = true;
            }
            if (chkCercFilling.Checked == true)
            {
                bolServiceChecked = true;
                if (IsChild == false)
                {
                    decServiceFree += 126.3m;
                }
            }
            if (chkRootCannal.Checked == true)
            {
                bolServiceChecked = true;
                if (IsChild == false)
                {
                    decServiceFree += 465.9m;
                }
            }
            // show total free and calculate + show the discount  + due date 
            if (bolServiceChecked == true)
            {
                lblDisplayTotalFree.Text = decServiceFree.ToString("C");
                decTotalAfterDiscount = ((decServiceFree) * 0.9m);
                lblDisplayDiscount.Text = decTotalAfterDiscount.ToString("C");
                lblDisplayDueDate.Text = dueday.ToShortDateString();
            }
            else
            {   //remain user to choose some service
                MessageBox.Show("Please select some sevice");
            }

        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            if (chkRootCannal.Checked == true)
            {
                if (age < 18)
                {
                    chkRootCannal.Checked = false;
                    cbxRoot.Visible = false;
                    dtpRoot.Visible = false;
                }
                else
                {
                    cbxRoot.Visible = true;
                    dtpRoot.Visible = true;
                    DataBaseConnection DB = new DataBaseConnection();
                    ArrayList dentist = new ArrayList();
                    DataTable reader = DB.Distist();
                    DataBaseConnection DB2 = new DataBaseConnection();
                    int index = 1;
                    foreach (DataRow row in reader.Rows)
                    {
                        DataTable ability = DB2.Ability(index.ToString());
                        foreach (DataRow row2 in ability.Rows)
                        {
                            if (row2[4].ToString() == "1")
                            {
                                cbxRoot.Items.Add(row[0]);
                            }
                        }
                        index += 1;
                    }
                }
            }
            else
            {
                dtpRoot.Value = DateTime.Now;
                cbxRoot.Items.Clear();
                root_dentist_id = "";
                cbxRoot.Visible = false;
                dtpRoot.Visible = false;
            }
        }

        private void PanWelcome_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            PanWelcome.Visible = false;
            PanMenu.Visible = true;

        }

        private void btnFindClient_Click(object sender, EventArgs e)
        {
            String UserId = txtClientid.Text;
            txtClientid.Text = "";
            DataBaseConnection DB = new DataBaseConnection();
            if (String.IsNullOrEmpty(UserId) == true)
            {
                MessageBox.Show("Please type-in clinet id");
            }
            else if (!(UserId.All(char.IsDigit))) {
                MessageBox.Show("Digit only!!");
            }
            else
            {
                DB.openConection();
                MySqlDataReader reader = DB.LookClient(UserId);
                if (reader.HasRows)
                {
                    PanClient.Visible = true;
                    PanMenu.Visible = false;
                    clinet_id = UserId;
                    while (reader.Read())
                    {
                        lblDisplayName.Text = reader.GetString("first_name") + " " + reader.GetString("last_name");
                        lblDisplayGneder.Text = reader.GetString("gender");
                        lblDIaplayDayOfBirth.Text = Convert.ToDateTime(reader.GetString("day_of_birth")).ToString("dd-MM-yyyy");
                        lblDispplayPhoneNumber.Text = reader.GetString("phone_number");
                        lblDisplayAddress1.Text = reader.GetString("house_no");
                        lblDisplayAdrress2.Text = reader.GetString("street");
                        lblDisplayCity.Text = reader.GetString("city");
                        lblDisplayPostCode.Text = reader.GetString("postcode");
                        int now = int.Parse(DateTime.Now.ToString("yyyyMMdd"));
                        int DateOfBirth = int.Parse(Convert.ToDateTime(reader.GetString("day_of_birth")).ToString("yyyMMdd"));
                        age = (now - DateOfBirth) / 10000;
                        client_name = reader.GetString("first_name") + " " + reader.GetString("last_name");
                        lblDisplayAge.Text = age.ToString();
                    }
                    DB.closeConnection();

                }
                else
                {
                    MessageBox.Show("Not such client!");
                    DB.closeConnection();
                }

            }

        }

        private void PanMenu_Paint(object sender, PaintEventArgs e)
        {

        }
        class DataBaseConnection
        {
            MySqlConnection connectionstring = new MySqlConnection("server=localhost;user id=ryan;password=Nemo0422003;database=client_db");
            public void openConection()
            {
                connectionstring.Open();
            }
            public void closeConnection()
            {
                connectionstring.Close();
            }
            public MySqlDataReader LookClient(String clinet_id)
            {

                string sql = ("select * from client where client_id =" + clinet_id);

                MySqlCommand cmd = new MySqlCommand(sql, connectionstring);
                MySqlDataReader reader = cmd.ExecuteReader();

                return reader;

            }
            public void ClinetInsert(String NewFirstName, String NewLastName, String NewGender, String NewDayOfBirth, String NewPhoneNumber, String NewAddress1, String NewAddress2, String NewCity, String NewPostCode)
            {

                connectionstring.Open();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = connectionstring;
                cmd.CommandText = "INSERT INTO Client (phone_number,day_of_birth,first_name,last_name,house_no,street,city,postcode,gender) VALUES (@NewPhoneNumber,@NewDayOfBirth,@NewFirstName,@NewLastName,@NewAddress1,@NewAddress2,@NewCity,@NewPostCode,@NewGender)";
                cmd.Prepare();
                cmd.Parameters.AddWithValue("@NewPhoneNumber", NewPhoneNumber);
                cmd.Parameters.AddWithValue("@NewDayOfBirth", NewDayOfBirth);
                cmd.Parameters.AddWithValue("@NewFirstName", NewFirstName);
                cmd.Parameters.AddWithValue("@NewLastName", NewLastName);
                cmd.Parameters.AddWithValue("@NewAddress1", NewAddress1);
                cmd.Parameters.AddWithValue("@NewAddress2", NewAddress2);
                cmd.Parameters.AddWithValue("@NewCity", NewCity);
                cmd.Parameters.AddWithValue("@NewPostCode", NewPostCode);
                cmd.Parameters.AddWithValue("@NewGender", NewGender);
                cmd.ExecuteNonQuery();
                connectionstring.Close();

            }
            public DataTable Distist(String name = null)
            {
                connectionstring.Open();
                if (name == null)
                {
                    MySqlDataAdapter adapter = new MySqlDataAdapter("Select dentist_name from dentist", connectionstring);
                    DataTable data = new DataTable();
                    adapter.Fill(data);
                    connectionstring.Close();
                    return data;
                }
                else
                {
                    MySqlDataAdapter adapter = new MySqlDataAdapter("Select * from dentist WHERE dentist_name = '" + name + "'", connectionstring);
                    DataTable data = new DataTable();
                    adapter.Fill(data);
                    connectionstring.Close();
                    return data;
                }
            }
            public DataTable Ability(String id)
            {

                MySqlDataAdapter adapter = new MySqlDataAdapter("Select * from ability Where dentist ='" + id + "'", connectionstring);
                DataTable data = new DataTable();
                adapter.Fill(data);

                return data;

            }
            public String DentistInsert(String Name, String Phone_Number)
            {
                try
                {
                    connectionstring.Open();
                    MySqlCommand cmd = new MySqlCommand();
                    cmd.Connection = connectionstring;
                    cmd.CommandText = "INSERT INTO dentist (dentist_name,phone_number) VALUES (@NewDentistName,@NewPhoneNumber)";
                    cmd.Prepare();
                    cmd.Parameters.AddWithValue("@NewDentistName", Name);
                    cmd.Parameters.AddWithValue("@NewPhoneNumber", Phone_Number);
                    cmd.ExecuteNonQuery();
                    connectionstring.Close();

                    return ("ok");
                }
                catch (Exception e)
                {
                    return (e.ToString());
                }
            }
            public String DentistAbilityInsert(Boolean Hygienist, Boolean Check_Up, Boolean Cerec, Boolean Root, Boolean Froendly)
            {
                try
                {
                    connectionstring.Open();
                    MySqlCommand cmd2 = new MySqlCommand();
                    cmd2.Connection = connectionstring;
                    cmd2.CommandText = "INSERT INTO ability (hygienist_treatment, check_up_exam, caerec_filling, root_cannal,child_friendly) VALUES (@hyg,@cue,@cfg,@rc,@cfy)";
                    cmd2.Prepare();
                    cmd2.Parameters.AddWithValue("@hyg", Hygienist);
                    cmd2.Parameters.AddWithValue("@cue", Check_Up);
                    cmd2.Parameters.AddWithValue("@cfg", Cerec);
                    cmd2.Parameters.AddWithValue("@rc", Root);
                    cmd2.Parameters.AddWithValue("@cfy", Froendly);
                    cmd2.ExecuteNonQuery();
                    connectionstring.Close();

                    return ("ok");
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString());
                    return (e.ToString());
                }
            }


            public MySqlDataReader lookappionmentstart(String dentist_id, DateTime datetime, double dutrion)
            {
                DateTime finishtime = datetime.AddHours(dutrion);
                String sql = "Select * from appointment Where dentist  ='" + dentist_id + "' and StartTime between'" + datetime.ToString("yyyy-MM-dd HH:mm:ss") + "' and '" + finishtime.ToString("yyyy-MM-dd HH:mm:ss") + "'";
                MySqlCommand cmd = new MySqlCommand(sql, connectionstring);
                MySqlDataReader start = cmd.ExecuteReader();
                Console.WriteLine(sql);
                return start;
            }
            public MySqlDataReader lookappionmentend(String dentist_id, DateTime datetime, double dutrion)
            {
                DateTime finishtime = datetime.AddHours(dutrion);
                String sql = "Select * from appointment Where dentist  ='" + dentist_id + "' and FinishTime between '" + datetime.ToString("yyyy-MM-dd HH:mm:ss") + "' and'" + finishtime.ToString("yyyy-MM-dd HH:mm:ss") + "'";
                MySqlCommand cmd = new MySqlCommand(sql, connectionstring);
                MySqlDataReader finish = cmd.ExecuteReader();
                return finish;
            }
            public MySqlDataReader lookappionmentclientstart(String customer_id, DateTime datetime, double dutrion)
            {
                DateTime finishtime = datetime.AddHours(dutrion);
                String sql = "Select * from appointment Where client ='" + customer_id + "' and StartTime between'" + datetime.ToString("yyyy-MM-dd HH:mm:ss") + "' and  '" + finishtime.ToString("yyyy-MM-dd HH:mm:ss") + "'";
                MySqlCommand cmd = new MySqlCommand(sql, connectionstring);
                MySqlDataReader start = cmd.ExecuteReader();
                return start;
            }
            public MySqlDataReader lookappionmentclientend(String customer_id, DateTime datetime, double dutrion)
            {

                DateTime finishtime = datetime.AddHours(dutrion);
                String sql = "Select * from appointment Where client ='" + customer_id + "' and  FinishTime between'" + datetime.ToString("yyyy-MM-dd HH:mm:ss") + "' and '" + finishtime.ToString("yyyy-MM-dd HH:mm:ss") + "'";
                MySqlCommand cmd = new MySqlCommand(sql, connectionstring);
                MySqlDataReader finish = cmd.ExecuteReader();
                return finish;
            }
            public DataTable servicerecord(String client_id)
            {
                MySqlDataAdapter adapter = new MySqlDataAdapter("Select  * from  appointment where client = '" + client_id + "'", connectionstring);
                DataTable record = new DataTable();
                adapter.Fill(record);
                connectionstring.Close();
                return record;
            }
            public String appiontment(String client_id, String dentist_id, DateTime StartTime, DateTime FinishTime, String service)
            {
                try
                {

                    MySqlCommand cmd2 = new MySqlCommand();
                    cmd2.Connection = connectionstring;
                    cmd2.CommandText = "INSERT INTO appointment (client, dentist, StartTime, FinishTime,Service,description) VALUES (@client,@dentist,@start,@finish,@service,@description)";
                    cmd2.Prepare();
                    cmd2.Parameters.AddWithValue("@client", client_id);
                    cmd2.Parameters.AddWithValue("@dentist", dentist_id);
                    cmd2.Parameters.AddWithValue("@start", StartTime);
                    cmd2.Parameters.AddWithValue("@finish", FinishTime);
                    cmd2.Parameters.AddWithValue("@service", service);
                    cmd2.Parameters.AddWithValue("@description", "");
                    cmd2.ExecuteNonQuery();
                    return ("ok");
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString());
                    return (e.ToString());
                }
            }
            public String record(String appointment_id, String record, DateTime date)
            {
                try
                {
                    MessageBox.Show(appointment_id);
                    MySqlCommand cmd = new MySqlCommand();
                    cmd.Connection = connectionstring;
                    cmd.CommandText = "Update appointment SET Description = @desc, lastupdate = @date where appointment_id = @id ";
                    cmd.Prepare();
                    cmd.Parameters.AddWithValue("@id", appointment_id);
                    cmd.Parameters.AddWithValue("@desc", record);
                    cmd.Parameters.AddWithValue("@date", date);
                    cmd.ExecuteNonQuery();
                    return ("ok");
                } catch (Exception e)
                {
                    MessageBox.Show(e.ToString());
                    return (e.ToString());
                }
            }
        }
        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void lblDisplayPhoneNumber_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void lblNewGender_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnNewClietnt_Click(object sender, EventArgs e)
        {
            PanMenu.Visible = false;
            PanNewClient.Visible = true;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtNewFirstName_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtNewLastName_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtNewAddress_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnNewClietnt_Click_1(object sender, EventArgs e)
        {

            PanNewClient.Visible = true;
            PanMenu.Visible = false;
        }

        private void PanNewClient_Paint(object sender, PaintEventArgs e)
        {

        }

        private void txtNewFirstName_Enter(object sender, EventArgs e)
        {
            if (txtNewFirstName.Text == "First Name")
            {
                txtNewFirstName.Text = "";

                txtNewFirstName.ForeColor = Color.Black;
            }
        }

        private void txtNewFirstName_Leave(object sender, EventArgs e)
        {
            if (txtNewFirstName.Text == "")
            {
                txtNewFirstName.Text = "First Name";

                txtNewFirstName.ForeColor = Color.Gray;
            }
        }

        private void txtNewLastName_Leave(object sender, EventArgs e)
        {
            if (txtNewLastName.Text == "")
            {
                txtNewLastName.Text = "Last Name";

                txtNewLastName.ForeColor = Color.Gray;
            }
        }

        private void txtNewLastName_Enter(object sender, EventArgs e)
        {
            if (txtNewLastName.Text == "Last Name")
            {
                txtNewLastName.Text = "";

                txtNewLastName.ForeColor = Color.Black;
            }
        }

        private void txtNewPhoneNumber_Leave(object sender, EventArgs e)
        {
            if (txtNewPhoneNumber.Text == "")
            {
                txtNewPhoneNumber.Text = "Phone Number";

                txtNewPhoneNumber.ForeColor = Color.Gray;
            }
        }

        private void txtNewPhoneNumber_Enter(object sender, EventArgs e)
        {
            if (txtNewPhoneNumber.Text == "Phone Number")
            {
                txtNewPhoneNumber.Text = "";

                txtNewPhoneNumber.ForeColor = Color.Black;
            }
        }

        private void txtNewAddress1_Enter(object sender, EventArgs e)
        {
            if (txtNewAddress1.Text == "House Numbering")
            {
                txtNewAddress1.Text = "";

                txtNewAddress1.ForeColor = Color.Black;
            }
        }

        private void txtNewAddress1_Leave(object sender, EventArgs e)
        {
            if (txtNewAddress1.Text == "")
            {
                txtNewAddress1.Text = "House Numbering";

                txtNewAddress1.ForeColor = Color.Gray;
            }
        }

        private void txtNewAddress2_Leave(object sender, EventArgs e)
        {
            if (txtNewAddress2.Text == "")
            {
                txtNewAddress2.Text = "Street";

                txtNewAddress2.ForeColor = Color.Gray;
            }
        }

        private void txtNewAddress2_Enter(object sender, EventArgs e)
        {
            if (txtNewAddress2.Text == "Street")
            {
                txtNewAddress2.Text = "";

                txtNewAddress2.ForeColor = Color.Black;
            }
        }

        private void txtNewCity_Leave(object sender, EventArgs e)
        {
            if (txtNewCity.Text == "")
            {
                txtNewCity.Text = "City";

                txtNewCity.ForeColor = Color.Gray;
            }
        }

        private void txtNewCity_Enter(object sender, EventArgs e)
        {
            if (txtNewCity.Text == "City")
            {
                txtNewCity.Text = "";

                txtNewCity.ForeColor = Color.Black;
            }
        }

        private void txtNewPostCode_Leave(object sender, EventArgs e)
        {
            if (txtNewPostCode.Text == "")
            {
                txtNewPostCode.Text = "Post Code";

                txtNewPostCode.ForeColor = Color.Gray;
            }
        }

        private void txtNewPostCode_Enter(object sender, EventArgs e)
        {
            if (txtNewPostCode.Text == "Post Code")
            {
                txtNewPostCode.Text = "";

                txtNewPostCode.ForeColor = Color.Black;
            }
        }

        private void btnNewCleintMainMenu_Click(object sender, EventArgs e)
        {
            PanNewClient.Visible = false;
            PanMenu.Visible = true;
            txtNewFirstName.Text = "";
            txtNewLastName.Text = "";
            txtNewPhoneNumber.Text = "";
            txtNewPostCode.Text = "";
            txtNewAddress1.Text = "";
            txtNewAddress2.Text = "";
            txtNewCity.Text = "";
            cbxNewGender.Text = "";
            dateTimePicker1.ResetText();
        }

        private void btnNewClient_Click(object sender, EventArgs e)
        {
            String NewFirstName = null, NewLastName = null, NewGender = null, NewDayOfBirth = null, NewPhoneNumber = null, NewAddress1 = null, NewAddress2 = null, NewCity = null, NewPostCode = null;
            Boolean FirstName = false, LastName = false, Gender = false, DayOfBrith = false, PhoneNumber = false, Address1 = false, Address2 = false, City = false, PostCode = false;
            //Checking FirstName
            if (txtNewFirstName.Text != "First Name" && txtNewFirstName.Text.Length <= 20)
            {
                NewFirstName = char.ToUpper(txtNewFirstName.Text[0]) + txtNewFirstName.Text.Substring(1);
                FirstName = true;
            }
            else
            {
                MessageBox.Show("Invalid First Name! First Name could not be more than 20 charater.");
            }
            //Checking LastName
            if (txtNewLastName.Text != "Last Name" && txtNewLastName.Text.Length <= 20)
            {
                NewLastName = char.ToUpper(txtNewLastName.Text[0]) + txtNewLastName.Text.Substring(1);
                LastName = true;
            }
            else
            {
                MessageBox.Show("Invalid Last Name! Last Name could not be more than 20 charater.");
            }
            //Checking Gender
            if (cbxNewGender.Text != "")
            {
                NewGender = cbxNewGender.Text;
                Gender = true;
            }
            //Checking Day Of Birth
            if (dayofbirth == true)
            {
                NewDayOfBirth = Convert.ToDateTime(dateTimePicker1.Text).ToString("yyyy-MM-dd");
                DayOfBrith = true;
            }
            //Checking Phone Number
            if (txtNewPhoneNumber.Text != "Phone Number")
            {
                int ph;
                var isNumeric = int.TryParse(txtNewPhoneNumber.Text, out ph);
                if (isNumeric == true && (txtNewPhoneNumber.Text.Length >= 8 && txtNewPhoneNumber.Text.Length <= 10)) {
                    PhoneNumber = true;
                    NewPhoneNumber = txtNewPhoneNumber.Text;
                }
                else
                {
                    MessageBox.Show("Invalid Phone Number! Phone Numebr should be 8 - 10 digit.");
                }
            }
            //Checking Address1
            if (txtNewAddress1.Text != "House Numbering" && txtNewAddress1.Text.Length <= 20)
            {
                NewAddress1 = txtNewAddress1.Text;
                Address1 = true;
            }
            else
            {
                MessageBox.Show("Invalid House Number! House Numebr could not be more than 20 charater.");
            }
            //Checking Address2
            if (txtNewAddress2.Text != "Street" && txtNewAddress2.Text.Length <= 20)
            {
                NewAddress2 = txtNewAddress2.Text;
                Address2 = true;
            }
            else
            {
                MessageBox.Show("Invalid Street Name! Street Name could not be more than 20 charater.");
            }
            //Checking City
            if (txtNewCity.Text != "City" && txtNewCity.Text.Length <= 20)
            {
                NewCity = txtNewCity.Text;
                City = true;
            }
            else
            {
                MessageBox.Show("Invalid City! City could not be more than 20 charater.");
            }
            //Checking Post Code
            if (txtNewPostCode.Text != "Post Code" && txtNewCity.Text.Length == 4)
            {
                int pc;
                var isNumeric = int.TryParse(txtNewPostCode.Text, out pc);
                if (isNumeric == true && txtNewPostCode.Text.Length == 4)
                {
                    NewPostCode = txtNewPostCode.Text;
                    PostCode = true;
                } else
                {
                    MessageBox.Show("Invalid Post Code! Post Code should be exactly 4 digit.");
                }
            }
            if (FirstName == false || LastName == false || Gender == false || DayOfBrith == false || PhoneNumber == false || Address1 == false || Address2 == false || City == false || PostCode == false) {
                MessageBox.Show("Please make sure choose a gender as well as the date of birth!");
            } else
            {
                DataBaseConnection DB = new DataBaseConnection();
                DB.ClinetInsert(NewFirstName, NewLastName, NewGender, NewDayOfBirth, NewPhoneNumber, NewAddress1, NewAddress2, NewCity, NewPostCode);
                MessageBox.Show("Add successful!");
                PanClient.Visible = false;
                PanMenu.Visible = true;
            }
            txtNewFirstName.Text = "";
            txtNewLastName.Text = "";
            txtNewPhoneNumber.Text = "";
            txtNewPostCode.Text = "";
            txtNewAddress1.Text = "";
            txtNewAddress2.Text = "";
            txtNewCity.Text = "";
            cbxNewGender.Text = "";
            dateTimePicker1.ResetText();
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            dayofbirth = true;
        }

        private void lblDIaplayDayOfBirth_Click(object sender, EventArgs e)
        {

        }

        private void lblDispplayPhoneNumber_Click(object sender, EventArgs e)
        {

        }

        private void btnMainMenu_Click(object sender, EventArgs e)
        {
            PanClient.Visible = false;
            PanMenu.Visible = true;
            age = 0;
            lblDisplayName.Text = "";
            lblDisplayGneder.Text = "";
            lblDisplayAge.Text = "";
            lblDisplayAddress1.Text = "";
            lblDisplayAdrress2.Text = "";
            lblDisplayCity.Text = "";
            lblDisplayPostCode.Text = "";
            lblDIaplayDayOfBirth.Text = "";
            lblDispplayPhoneNumber.Text = "";
        }

        private void btnService_Click(object sender, EventArgs e)
        {
            PanClient.Visible = false;
            PanService.Visible = true;

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnMain_Click(object sender, EventArgs e)
        {
            PanDentist.Visible = false;
            PanMenu.Visible = true;
        }

        private void BtnDistist_Click(object sender, EventArgs e)
        {
            PanMenu.Visible = false;
            PanDentist.Visible = true;
            lblPhoneNuber.Text = "";
            DataBaseConnection DB = new DataBaseConnection();
            ArrayList dentist = new ArrayList();
            DataTable reader = DB.Distist();
            foreach (DataRow row in reader.Rows)
            {
                cbxDentist.Items.Add(row[0]);
            }
        }

        private void cbxDentist_SelectedIndexChanged(object sender, EventArgs e)
        {
            String distinct_name = cbxDentist.Text;
            int distenct_number = -1;
            DataBaseConnection DB = new DataBaseConnection();
            ArrayList dentist = new ArrayList();
            DataTable reader = DB.Distist(distinct_name);
            foreach (DataRow row in reader.Rows)
            {
                lblPhoneNuber.Text = (row[2].ToString());
                distenct_number = (Int32.Parse(row[0].ToString()));
            }
            if (distenct_number != -1) {
                DataTable ability = DB.Ability(distenct_number.ToString());
                foreach (DataRow row in ability.Rows)
                {
                    if (row[1].ToString() == "0")
                    {
                        chkHyTreatment.Checked = false;
                    }
                    else
                    {
                        chkHyTreatment.Checked = true;
                    }
                    if (row[2].ToString() == "0")
                    {
                        chkExam.Checked = false;
                    }
                    else
                    {
                        chkExam.Checked = true;
                    }
                    if (row[3].ToString() == "0")
                    {
                        chkCerc.Checked = false;
                    }
                    else
                    {
                        chkCerc.Checked = true;
                    }
                    if (row[4].ToString() == "0")
                    {
                        chkRoot.Checked = false;
                    }
                    else
                    {
                        chkRoot.Checked = true;
                    }
                    if (row[5].ToString() == "0")
                    {
                        chkChildFirend.Checked = false;
                    }
                    else
                    {
                        chkChildFirend.Checked = true;
                    }
                }
            }
        }

        private void gbxAbilitySetUp_Enter(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click_1(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void btnAddDentist_Click(object sender, EventArgs e)
        {
            String Name = "", Phone_Number = "";
            Boolean Hygienist = false, Check_Up = false, Cerec = false, Root = false, Friendly = false;
            Name = txtDentistName.Text;
            Phone_Number = txtPhoeneNumber.Text;
            Hygienist = chkHy.Checked;
            Check_Up = chkChExam.Checked;
            Root = chkRootC.Checked;
            Cerec = chkCercF.Checked;
            Friendly = chkChildF.Checked;
            txtDentistName.Text = "";
            txtPhoeneNumber.Text = "";
            chkHy.Checked = false;
            chkChExam.Checked = false;
            chkRootC.Checked = false;
            chkCercF.Checked = false;
            chkChildF.Checked = false;
            if (Name != "" && Phone_Number != "")
            {
                if (Phone_Number.Length < 8 || Phone_Number.Length > 10)
                {
                    MessageBox.Show("The length of phone number can be only between 8 - 11");
                }
                else
                if (Friendly == true && Check_Up == false)
                {
                    MessageBox.Show("A child friendly dentist must aviliable with check up exam!");
                } else
                {
                    DataBaseConnection DB = new DataBaseConnection();
                    String error = DB.DentistInsert(Name, Phone_Number);
                    if (error != "ok")
                    {
                        MessageBox.Show(error);
                    }
                    else
                    {
                        DataBaseConnection DB2 = new DataBaseConnection();
                        String error2 = DB2.DentistAbilityInsert(Hygienist, Check_Up, Cerec, Root, Friendly);
                        if (error2 != "ok")
                        {
                            MessageBox.Show(error);
                        } else
                        {
                            MessageBox.Show("Add successful!");

                            PanNewDentist.Visible = false;
                            PanMenu.Visible = true;
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Please type in dintist name and phone number !");
            }
        }

        private void PanNewDentist_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            PanMenu.Visible = false;
            PanNewDentist.Visible = true;
        }

        private void chkHy_CheckedChanged(object sender, EventArgs e)
        {
        }

        private void chkHygiebistTreatment_CheckedChanged(object sender, EventArgs e)
        {
            if (chkHygiebistTreatment.Checked == true)
            {
                if (age < 18)
                {
                    chkHygiebistTreatment.Checked = false;
                }
                else
                {
                    dtpHy.Visible = true;
                    cbxHy.Visible = true;
                    dtpHy.Value = DateTime.Now;
                    DataBaseConnection DB = new DataBaseConnection();
                    ArrayList dentist = new ArrayList();
                    DataTable reader = DB.Distist();
                    DataBaseConnection DB2 = new DataBaseConnection();
                    int index = 1;
                    foreach (DataRow row in reader.Rows)
                    {
                        DataTable ability = DB2.Ability(index.ToString());
                        foreach (DataRow row2 in ability.Rows)
                        {
                            if (row2[1].ToString() == "1")
                            {
                                cbxHy.Items.Add(row[0]);
                            }
                        }
                        index += 1;
                    }
                }
            } else {
                hy_dentist_id = "";
                dtpHy.Value = DateTime.Now;
                cbxHy.Items.Clear();
                cbxHy.Visible = false;
                dtpHy.Visible = false;
            }
        }

        private void chkCheckUpExam_CheckedChanged(object sender, EventArgs e)
        {
            if (chkCheckUpExam.Checked == true)
            {
                    cbxExam.Visible = true;
                    dtpExam.Visible = true;
                    DataBaseConnection DB = new DataBaseConnection();
                    ArrayList dentist = new ArrayList();
                    DataTable reader = DB.Distist();
                    DataBaseConnection DB2 = new DataBaseConnection();
                    int index = 1;
                    foreach (DataRow row in reader.Rows)
                    {
                        DataTable ability = DB2.Ability(index.ToString());
                        foreach (DataRow row2 in ability.Rows)
                        {
                            if (row2[2].ToString() == "1")
                            {
                                cbxExam.Items.Add(row[0]);
                            }
                        }

                        index += 1;
                    }
                }
            else
            {
                dtpExam.Value = DateTime.Now;
                cbxExam.Items.Clear();
                check_up_dentist_id = "";
                cbxExam.Visible = false;
                dtpExam.Visible = false;
            }
        }

        private void chkCercFilling_CheckedChanged(object sender, EventArgs e)
        {
            if (chkCercFilling.Checked == true)
            {
                if (age < 18)
                {
                    chkCercFilling.Checked = false;
                }
                else
                {
                    cbxCerec.Visible = true;
                    dtpCerec.Visible = true;
                    DataBaseConnection DB = new DataBaseConnection();
                    ArrayList dentist = new ArrayList();
                    DataTable reader = DB.Distist();
                    DataBaseConnection DB2 = new DataBaseConnection();
                    int index = 1;
                    foreach (DataRow row in reader.Rows)
                    {
                        DataTable ability = DB2.Ability(index.ToString());
                        foreach (DataRow row2 in ability.Rows)
                        {
                            if (row2[3].ToString() == "1")
                            {
                                cbxCerec.Items.Add(row[0]);
                            }
                        }
                        index += 1;
                    }
                }
            }
            else
            {
                dtpCerec.Value = DateTime.Now;
                cbxCerec.Items.Clear();
                carc_dentist_id = "";
                cbxCerec.Visible = false;
                dtpCerec.Visible = false;
            }
        }

        private void dtpHy_ValueChanged(object sender, EventArgs e)
        {

        }

        private void PanService_Paint(object sender, PaintEventArgs e)
        {

        }

        private void gbxService_Enter(object sender, EventArgs e)
        {

        }

        private void lblDisplayDueDate_Click(object sender, EventArgs e)
        {

        }

        private void lblTotalFree_Click(object sender, EventArgs e)
        {

        }

        private void lblDisplayDiscount_Click(object sender, EventArgs e)
        {

        }

        private void lblDiscount_Click(object sender, EventArgs e)
        {

        }

        private void lblDisplayTotalFree_Click(object sender, EventArgs e)
        {

        }

        private void lblDuedate_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            PanService.Visible = false;
            PanMenu.Visible = true;
            chkHygiebistTreatment.Checked = false;
            chkRootCannal.Checked = false;
            chkCheckUpExam.Checked = false;
            chkCercFilling.Checked = false;
            dtpCerec.Value = DateTime.Now;
            dtpHy.Value = DateTime.Now;
            dtpExam.Value = DateTime.Now;
            dtpRoot.Value = DateTime.Now;
        }

        private void dtpCerec_ValueChanged(object sender, EventArgs e)
        {

        }

        private void cbxHy_SelectedIndexChanged(object sender, EventArgs e)
        {
            int i = cbxHy.SelectedIndex;
            hy_dentist_id = i.ToString();
        }
        private Boolean checktime(DateTime time) {
            if (time < DateTime.Today.AddDays(3) + new TimeSpan(00, 00, 00))
            {
                MessageBox.Show("Can not make a apppoiment with in three working day");
                return false;
            }
            else if (time.DayOfWeek == DayOfWeek.Saturday || time.DayOfWeek == DayOfWeek.Sunday)
            {
                MessageBox.Show("Only work on weekdays!");
                return false;
            }
            else if (!(time.TimeOfDay >= new TimeSpan(09, 00, 00) && time.TimeOfDay <= new TimeSpan(12, 00, 00) || time.TimeOfDay >= new TimeSpan(13, 00, 00) && time.TimeOfDay <= new TimeSpan(15, 00, 00)))
            {
                MessageBox.Show("An apportment can only make between 9am to 12pm and 1pm to 3pm");
                return false;
            }
            else
            {
                return true;
            }
        }
        private Boolean checkappointment(String dentist_id, String clinet_id, DateTime start_time, double dutrion)
        {
            if (dentist_id == "")
            {
                MessageBox.Show("Please select a dintist !");
                return false;
            }
            else
            {
                DataBaseConnection DB = new DataBaseConnection();
                DB.openConection();
                MySqlDataReader dentiststart = DB.lookappionmentstart(hy_dentist_id, start_time, dutrion);
                if (!dentiststart.HasRows)
                {
                    DB.closeConnection();
                    DB.openConection();
                    MySqlDataReader dentistfinish = DB.lookappionmentend(hy_dentist_id, start_time, dutrion);
                    if (!dentistfinish.HasRows)
                    {
                        DB.closeConnection();
                        DB.openConection();
                        MySqlDataReader clientstart = DB.lookappionmentclientstart(clinet_id, start_time, dutrion);
                        if (!clientstart.HasRows)
                        {
                            DB.closeConnection();
                            DB.openConection();
                            MySqlDataReader clientend = DB.lookappionmentclientend(clinet_id, start_time, dutrion);
                            if (!clientend.HasRows)
                            {
                                DB.closeConnection();
                                return true;
                            }
                            else
                            {
                                DB.closeConnection();
                                MessageBox.Show("Clinet no avaliable");
                                return false;
                            }
                        }
                        else
                        {
                            DB.closeConnection();
                            MessageBox.Show("Clinet no avaliable");
                            return false;
                        }
                    }
                    else
                    {
                        DB.closeConnection();
                        MessageBox.Show("Dintist no avaliable");
                        return false;
                    }
                }
                else
                {
                    DB.closeConnection();
                    MessageBox.Show("Dintist no avaliable");
                    return false;
                }
            }
        }
        private void btnSerSub_Click(object sender, EventArgs e)
        {
            DataBaseConnection DB = new DataBaseConnection();
            if (chkHygiebistTreatment.Checked == true)
            {
                Boolean hy_time = checktime(dtpHy.Value);
                if (hy_time == true)
                {
                    Boolean hy_appointment = checkappointment(hy_dentist_id, clinet_id, dtpHy.Value, 1);
                    if (hy_appointment == true)
                    {
                        DB.openConection();
                        string result = DB.appiontment(clinet_id, hy_dentist_id, dtpHy.Value, dtpHy.Value.AddHours(1), "hy");
                        DB.closeConnection();
                        if (result == "ok")
                        {
                            MessageBox.Show("Added Hygiebist Treatment - " + dtpHy.Value);
                            chkHygiebistTreatment.Checked = false;
                        }
                    }
                }
            }
            if (chkCheckUpExam.Checked == true)
            {
                Boolean exam_time = checktime(dtpExam.Value);
                if (exam_time == true)
                {
                    Boolean exam_appointment = checkappointment(check_up_dentist_id, clinet_id, dtpExam.Value, 0.5);
                    if (exam_appointment == true)
                    {
                        DB.openConection();
                        string result = DB.appiontment(clinet_id, check_up_dentist_id, dtpExam.Value, dtpExam.Value.AddHours(0.5), "exam");
                        DB.closeConnection();
                        if (result == "ok")
                        {
                            MessageBox.Show("Added Checkup Exam - " + dtpExam.Value);
                            chkCheckUpExam.Checked = false;
                        }
                    }
                }
            }
            if (chkCercFilling.Checked == true)
            {
                Boolean cerc_time = checktime(dtpExam.Value);
                if (cerc_time == true)
                {
                    Boolean cerc_appointment = checkappointment(carc_dentist_id, clinet_id, dtpCerec.Value, 1);
                    if (cerc_appointment == true)
                    {
                        DB.openConection();
                        string result = DB.appiontment(clinet_id, carc_dentist_id, dtpCerec.Value, dtpCerec.Value.AddHours(1), "cerc");
                        DB.closeConnection();
                        if (result == "ok")
                        {
                            MessageBox.Show("Added Cerec Filling - " + dtpExam.Value);
                            chkCercFilling.Checked = false;
                        }
                    }
                }
            }
            if (chkRootCannal.Checked == true)
            {
                Boolean root_time = checktime(dtpRoot.Value);
                if (root_time == true)
                {
                    Boolean cerc_appointment = checkappointment(root_dentist_id, clinet_id, dtpRoot.Value, 1);
                    if (cerc_appointment == true)
                    {
                        DB.openConection();
                        string result = DB.appiontment(clinet_id, root_dentist_id, dtpRoot.Value, dtpRoot.Value.AddHours(1), "cerc");
                        DB.closeConnection();
                        if (result == "ok")
                        {
                            MessageBox.Show("Added Root Cannal - " + dtpExam.Value);
                            chkRootCannal.Checked = false;
                        }
                    }
                }
            }
        }




        private void cbxExam_SelectedIndexChanged(object sender, EventArgs e)
        {
            int i = cbxExam.SelectedIndex + 1;
            check_up_dentist_id = i.ToString();
        }

        private void cbxCerec_SelectedIndexChanged(object sender, EventArgs e)
        {
            int i = cbxCerec.SelectedIndex+1;
            carc_dentist_id = i.ToString();
        }

        private void cbxRoot_SelectedIndexChanged(object sender, EventArgs e)
        {
            int i = cbxRoot.SelectedIndex + 1;
            root_dentist_id = i.ToString();
        }

        private void label11_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            txtClientName.Text = "";
            cbxType.Text = "";
            cbxType.Items.Clear();
            tbxReview.Clear();
            appointment_id.Clear();
            record.Clear();
            PanReview.Visible = false;
            PanMenu.Visible = true;
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            PanClient.Visible = false;
            PanReview.Visible = true;
            txtClientName.Text = client_name;
            DataBaseConnection DB = new DataBaseConnection();
            DB.openConection();
            DataTable timetype = DB.servicerecord(clinet_id);
            DB.closeConnection();
            int index = 1;
            foreach (DataRow row in timetype.Rows)
            {
                cbxType.Items.Add(row[3] + " - " + row[5]);
                record.Add(row[6]);
                appointment_id.Add(row[0]);
            }
        }

        private void dtpReview_ValueChanged(object sender, EventArgs e)
        {
        }

        private void cbxType_SelectedIndexChanged(object sender, EventArgs e)
        {
           tbxReview.Text = record[cbxType.SelectedIndex].ToString();
            select_id = appointment_id[cbxType.SelectedIndex].ToString();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            DataBaseConnection DB = new DataBaseConnection();
            DB.openConection();
            string result = DB.record(select_id, tbxReview.Text, DateTime.Now);
            DB.closeConnection();
            if (result == "ok")
            {
                MessageBox.Show("Update successful!");
                PanReview.Visible = false;
                PanMenu.Visible = true;
                select_id = "";
                appointment_id.Clear();
                record.Clear();
                txtClientName.Text = "";
                cbxType.Items.Clear();
                cbxType.Text = "";
                tbxReview.Clear();
            }
        }
    }
    }
    

