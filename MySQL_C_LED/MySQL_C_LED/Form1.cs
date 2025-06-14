using System;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace MySQL_C_LED
{
    public partial class Form1 : Form
    {
        MySqlConnection databaseConnection;

        public Form1()
        {
            InitializeComponent();
            timer1.Start();
            InitializeDatabaseConnection();

        }

        private void InitializeDatabaseConnection()
        {
            tabPage1.Enabled = false;
            tabPage2.Enabled = false;
            tabPage3.Enabled = false;
            string MySQLConnectionString = "server=localhost;port=3306;user=root;password=;database=esp32_mc_db";
            databaseConnection = new MySqlConnection(MySQLConnectionString);
        }
        private void button11_Click(object sender, EventArgs e)
        {
            if ((textBox7.Text == "admin") && (textBox8.Text == "1234"))
            {
                tabControl1.SelectedTab = tabPage1;

                tabPage1.Enabled = true;
                tabPage2.Enabled = true;
                tabPage3.Enabled = true;
            }
            else
            {
                MessageBox.Show("Invalid Username or Password!");
                textBox7.Text = "";
                textBox8.Text = "";
            }
        }
        private void UpdateDataLED(string senddata1)
        {
            try
            {
                databaseConnection.Open();
                MySqlCommand commandDatabase;

                // Check if there is a record with id = 1
                commandDatabase = new MySqlCommand("SELECT COUNT(*) FROM esp32_table_test WHERE id = 1", databaseConnection);
                int count = Convert.ToInt32(commandDatabase.ExecuteScalar());

                if (count == 0)
                {
                    // If there is no record with id = 1, insert new record
                    commandDatabase = new MySqlCommand($"INSERT INTO esp32_table_test (id, LED_01, Temparature, Servo) VALUES (1,0,0,0)", databaseConnection);

                }
                else
                {
                    // If there is a record with id = 1, update existing record
                    commandDatabase = new MySqlCommand($"UPDATE esp32_table_test SET LED_01 = '{senddata1}' WHERE id = 1", databaseConnection);
                }

                int rowsAffected = commandDatabase.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Query error: " + ex.Message);
            }
            finally
            {
                databaseConnection.Close();
            }
        }
        private void UpdateDataServo(string senddata2)
        {
            try
            {
                databaseConnection.Open();
                MySqlCommand commandDatabase;

                // Check if there is a record with id = 1
                commandDatabase = new MySqlCommand("SELECT COUNT(*) FROM esp32_table_test WHERE id = 1", databaseConnection);
                int count = Convert.ToInt32(commandDatabase.ExecuteScalar());

                if (count == 0)
                {
                    // If there is no record with id = 1, insert new record
                    commandDatabase = new MySqlCommand($"INSERT INTO esp32_table_test (id, LED_01, Temparature, Servo) VALUES (1,0,0,0)", databaseConnection);

                }
                else
                {
                    // If there is a record with id = 1, update existing record
                    commandDatabase = new MySqlCommand($"UPDATE esp32_table_test SET Servo = '{senddata2}' WHERE id = 1", databaseConnection);
                }

                int rowsAffected = commandDatabase.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Query error: " + ex.Message);
            }
            finally
            {
                databaseConnection.Close();
            }
        }
        private void hScrollBar1servo_Scroll(object sender, ScrollEventArgs e)
        {
            int currentPosition = hScrollBar1servo.Value;
            int position180 = currentPosition * 180 / 100;
            string senddata2 = position180.ToString();
            UpdateDataServo(senddata2);
            RetrieveData();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            string senddata1 = "1";
            UpdateDataLED(senddata1);
            Console.WriteLine("Gönderilen veri: " + senddata1);
            RetrieveData(); // Veriyi çek ve textbox'lara bastır
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string senddata1 = "0";
            UpdateDataLED(senddata1);
            Console.WriteLine("Gönderilen veri: " + senddata1);
            RetrieveData(); // Veriyi çek ve textbox'lara bastır
        }

        private void button3_Click(object sender, EventArgs e)
        {
            RetrieveData(); // Veriyi çek ve textbox'lara bastır
        }

        private void RetrieveData()
        {
            try
            {
                databaseConnection.Open();
                Console.WriteLine("\nRetrieving data from MySQL database\n");

                MySqlCommand cmd = databaseConnection.CreateCommand();
                cmd.CommandText = "SELECT * FROM esp32_table_test";
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        textBox1.Text = reader.GetString(0); // id
                        textBox2.Text = reader.GetString(1); // LED_01
                        textBox3.Text = reader.GetString(2); // Temp
                        textBox5.Text = reader.GetString(1); // LED_01
                        textBox4.Text = reader.GetString(3); // Servo
                        textBox6.Text = reader.GetString(3); // Servo
                        textBox9.Text = reader.GetString(4); // Nem
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.ToString());
            }
            finally
            {
                if (databaseConnection != null && databaseConnection.State == System.Data.ConnectionState.Open)
                {
                    databaseConnection.Close();
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabPage2;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabPage3;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            RetrieveData();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabPage1;
        }

        private void button9_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabPage1;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabPage4;
            tabPage1.Enabled = false;
            tabPage2.Enabled = false;
            tabPage3.Enabled = false;
            textBox7.Text = "";
            textBox8.Text = "";
        }

        private void button10_Click(object sender, EventArgs e)
        {
            RetrieveData();
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            label13.Text = DateTime.Now.ToLongDateString();
            label12.Text = DateTime.Now.ToLongTimeString();
        }
    }
}
