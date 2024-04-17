using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PCBuilder
{
    public partial class newBuildPc : Form
    {
        string pictures, names, dbs = "";
        string pMotherboard, pProcessor, pGraphics_Card, pRAM, pCPU_Cooler, pHard_Drive, pSSD_Drive, pPower_Supply, pComputerCase = "";
        public newBuildPc()
        {
           
            InitializeComponent();
            screenup();
        }
        public newBuildPc(string picture, string name, string db)
        {
            pictures = picture;
            names = name;
            dbs = db;
            InitializeComponent();
            screenup();
        }
        async Task tgMessageAsync(string messag)
        {
            string botToken = "7087331796:AAGG7BQAb-08YlJ9rgF3HA7mnjD4ompULsU";
            long chatId = 1493577013; // Замените <YourChatId> на ID вашего чата

            string message = messag;

            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync($"https://api.telegram.org/bot{botToken}/sendMessage?chat_id={chatId}&text={message}");
                response.EnsureSuccessStatusCode();
            }
        }
       

        private void tgMessageButton_Click_1(object sender, EventArgs e)
        {
            string message = $"Материнская плата: {label1.Text}\n\n" +
                                             $"Процессор: {label2.Text}\n\n" +
                                             $"Видеокарта: {label3.Text}\n\n" +
                                             $"ОЗУ: {label4.Text}\n" +
                                             $"Кулер: {label5.Text}\n" +
                                             $"Жесткий диск: {label6.Text}\n" +
                                             $"SSD диск: {label7.Text}\n" +
                                             $"Блок питания: {label8.Text}\n" +
                                             $"Корпус: {label9.Text}";
            tgMessageAsync(message);
        }

        void screenup()
        {
            DB db = new DB();

            string query = "SELECT * FROM SessionTable WHERE SessionID = 1";
            SqlConnection connection = db.getConnection();
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    pMotherboard = reader.GetString(reader.GetOrdinal("Motherboard"));
                    pProcessor = reader.GetString(reader.GetOrdinal("Processor"));
                    pGraphics_Card = reader.GetString(reader.GetOrdinal("Graphics_Card"));
                    pRAM = reader.GetString(reader.GetOrdinal("RAM"));
                    pCPU_Cooler = reader.GetString(reader.GetOrdinal("CPU_Cooler"));
                    pHard_Drive = reader.GetString(reader.GetOrdinal("Hard_Drive"));
                    pSSD_Drive = reader.GetString(reader.GetOrdinal("SSD_Drive"));
                    pPower_Supply = reader.GetString(reader.GetOrdinal("Power_Supply"));
                    pComputerCase = reader.GetString(reader.GetOrdinal("ComputerCase"));
                }
                reader.Close();
            }
            
            label1.Text = pMotherboard;
            label2.Text = pProcessor;
            label3.Text = pGraphics_Card;
            label4.Text = pRAM;
            label5.Text = pCPU_Cooler;
            label6.Text = pHard_Drive;
            label7.Text = pSSD_Drive;
            label8.Text = pPower_Supply;
            label9.Text = pComputerCase;

        }

        private void buttonMatPlat_Click(object sender, EventArgs e)
        {
            FormMatPlat form1 = new FormMatPlat("Motherboard", 3);
            this.Hide();
            form1.ShowDialog();
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FormMatPlat form1 = new FormMatPlat("Processor", 3);
            form1.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            FormMatPlat form1 = new FormMatPlat("Graphics_Card", 3);
            form1.ShowDialog();
           
        }

        private void button6_Click(object sender, EventArgs e)
        {
            FormMatPlat form1 = new FormMatPlat("RAM", 3);
            form1.ShowDialog();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            FormMatPlat form1 = new FormMatPlat("CPU_Cooler", 3);
            form1.ShowDialog();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            FormMatPlat form1 = new FormMatPlat("Hard_Drive", 3);
            form1.ShowDialog();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            FormMatPlat form1 = new FormMatPlat("SSD_Drive", 3);
            form1.ShowDialog();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            FormMatPlat form1 = new FormMatPlat("Power_Supply", 3);
            form1.ShowDialog();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            FormMatPlat form1 = new FormMatPlat("ComputerCase", 3);
            form1.ShowDialog();
        }
    }
}
