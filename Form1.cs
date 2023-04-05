using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.ComponentModel.Design.ObjectSelectorEditor;

namespace lab1MoghiroiuBogdan
{
    public partial class Form1 : Form
    {
        SqlConnection cs = new SqlConnection("Server=DESKTOP-BD1G86A\\SQLEXPRESS;Database=my_database1;Integrated Security =true;" +
                "TrustServerCertificate=true;");
        SqlDataAdapter daDepartment = new SqlDataAdapter();
        SqlDataAdapter daEmployees = new SqlDataAdapter();
        DataSet dataSet = new DataSet();
        BindingSource DepartmentBS = new BindingSource();
        BindingSource EmployeesBS = new BindingSource();
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            daDepartment.SelectCommand = new SqlCommand("SELECT * from departments", cs);
            daDepartment.Fill(dataSet, "departments");

            daEmployees.SelectCommand = new SqlCommand("SELECT * from employees", cs);
            daEmployees.Fill(dataSet, "employees");


            DepartmentBS.DataSource = dataSet.Tables["departments"];
            dataGridView1.DataSource = DepartmentBS;

            DataColumn parentPK = dataSet.Tables["departments"].Columns["id"];
            DataColumn childFK = dataSet.Tables["employees"].Columns["department_id"];
            DataRelation relation = new DataRelation("fk_parent_child", parentPK, childFK);
            dataSet.Relations.Add(relation);

            EmployeesBS.DataSource = DepartmentBS;

            EmployeesBS.DataMember = "fk_parent_child";
            dataGridView2.DataSource = EmployeesBS;
        }





        private void button2_Click(object sender, EventArgs e)
        {
            daEmployees.InsertCommand = new SqlCommand("INSERT INTO employees VALUES (@name, " +
                "@email, @phone, @hire_date, @job_title, @department_id)", cs);
            daEmployees.InsertCommand.Parameters.Add("@name", SqlDbType.VarChar).Value = textBox2.Text;
            daEmployees.InsertCommand.Parameters.Add("@email", SqlDbType.VarChar).Value = textBox3.Text;
            daEmployees.InsertCommand.Parameters.Add("@phone", SqlDbType.Int).Value = Int32.Parse(textBox4.Text);
            daEmployees.InsertCommand.Parameters.Add("@hire_date", SqlDbType.Date).Value = DateTime.Parse(textBox5.Text);
            daEmployees.InsertCommand.Parameters.Add("@job_title", SqlDbType.VarChar).Value = textBox6.Text;
            daEmployees.InsertCommand.Parameters.Add("@department_id", SqlDbType.Int).Value = Int32.Parse(textBox7.Text);
            cs.Open();
            daEmployees.InsertCommand.ExecuteNonQuery();
            cs.Close();

        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                daEmployees.InsertCommand = new SqlCommand("DELETE FROM employees WHERE id= @id", cs);
                DataGridViewRow selectedRow = dataGridView2.SelectedRows[0];
                int foreignKeyValue = (int)selectedRow.Cells["id"].Value;
                daEmployees.InsertCommand.Parameters.Add("@id", SqlDbType.Int).Value = foreignKeyValue;
                cs.Open();
                daEmployees.InsertCommand.ExecuteNonQuery();
                MessageBox.Show("Deleted Succesfully from Database");
                cs.Close();
                daEmployees.Fill(dataSet, "employees");
                dataGridView2.DataSource = dataSet.Tables["employees"];
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                cs.Close();
            }

        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                daEmployees.InsertCommand = new SqlCommand("UPDATE employees SET name=@n, email=@e, phone=@p, hire_date=@hd, job_title=@jt WHERE id=@id", cs);
                DataGridViewRow selectedRow = dataGridView2.SelectedRows[0];
                int foreignKeyValue = (int)selectedRow.Cells["id"].Value;
                daEmployees.InsertCommand.Parameters.Add("@n", SqlDbType.VarChar).Value = textBox2.Text;
                daEmployees.InsertCommand.Parameters.Add("@e", SqlDbType.VarChar).Value = textBox3.Text;
                daEmployees.InsertCommand.Parameters.Add("@p", SqlDbType.Int).Value = Int32.Parse(textBox4.Text);
                daEmployees.InsertCommand.Parameters.Add("@hd", SqlDbType.Date).Value = DateTime.Parse(textBox5.Text);
                daEmployees.InsertCommand.Parameters.Add("@jt", SqlDbType.VarChar).Value = textBox6.Text;
                daEmployees.InsertCommand.Parameters.Add("@id", SqlDbType.Int).Value = foreignKeyValue;
                cs.Open();
                daEmployees.InsertCommand.ExecuteNonQuery();
                MessageBox.Show("Updated Succesfully");
                cs.Close();
                daEmployees.Fill(dataSet, "employees");
                dataGridView2.DataSource = dataSet.Tables["employees"];

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                cs.Close();
            }
        }




    }
}