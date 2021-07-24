using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Work2
{
    public partial class Form1 : Form
    {
        bool f_open, f_save; // определяют выбор файла и его сохранение
        

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            f_open = false;
            f_save = false;
            Txt.Text = "Ничего не открыто";
            MainTextBox.Text = "";
        }

        private void OpenBtn_Click(object sender, EventArgs e)
        {
            if(openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                Txt.Text = openFileDialog1.FileName;

                f_open = true;
                f_save = false;

                MainTextBox.Clear();

                StreamReader streamReader = File.OpenText(openFileDialog1.FileName);

                string line = null;
                line = streamReader.ReadLine();

                while(line!=null)
                {
                    MainTextBox.AppendText(line);
                    MainTextBox.AppendText("\r\n");
                    line = streamReader.ReadLine();
                }

                streamReader.Close();
            }
        }

        private void SortBtn_Click(object sender, EventArgs e)
        {
            if (!f_open) return;

            if (f_save) return;

            
            List<string> name = new List<string>();
            List<int> inHour = new List<int>();
            List<int> kolvHour = new List<int>();
            List<string> resultLine = new List<string>();
            StreamReader streamReader;


           using (streamReader = File.OpenText(Txt.Text))
           {
                string tempLine = streamReader.ReadLine();

                while (tempLine != null)
                {
                    string[] line = tempLine.Split(" ");

                    name.Add(line[0]);
                    inHour.Add(Convert.ToInt32(line[1]));
                    kolvHour.Add(Convert.ToInt32(line[2]));

                    tempLine = streamReader.ReadLine();
                }
            }

            MainTextBox.Clear();


            for (int i = 0; i < name.Count(); i++)
            {
                string currentName = name[i];
                int tempInHour = inHour[i];
                int tempKolvHour = kolvHour[i];
                int sumHour = 0;
                bool replace = false;

                for (int j = 0; j < name.Count(); j++)
                {
                    if (currentName == name[j] && i != j)
                    {
                        tempInHour += inHour[j];
                        tempKolvHour += kolvHour[j];
                    }

                    if (currentName == name[j] && i > j)
                        replace = true;
                }
                sumHour = tempInHour * tempKolvHour;
                if (!replace)
                {
                    resultLine.Add(currentName + " " + tempInHour + " " + tempKolvHour);
                }
            }


            for (int i = 0; i < resultLine.Count(); i++)
            {
                for (int k = 0; k < resultLine.Count()-1; k++)
                {
                    if(needToReOrder(resultLine[k], resultLine[k+1]))
                    {
                        string tempStr = resultLine[k];
                        resultLine[k] = resultLine[k + 1];
                        resultLine[k + 1] = tempStr;
                    }
                }
                MainTextBox.AppendText(resultLine[i]);
                MainTextBox.AppendText("\r\n");
            }

            streamReader.Close();
        }

        private static bool needToReOrder(string s1, string s2)
        {
            for (int i = 0; i < (s1.Length > s2.Length ? s2.Length : s1.Length); i++)
            {
                if (s1.ToCharArray()[i] < s2.ToCharArray()[i]) return false;
                if (s1.ToCharArray()[i] > s2.ToCharArray()[i]) return true;
            }
            return false;
        }

        private void MainTextBox_TextChanged(object sender, EventArgs e)
        {
            f_save = false;
        }

        private void SaveBtn_Click(object sender, EventArgs e)
        {
            if (!f_open) return;

            if (f_save) return;

            StreamWriter streamWriter = File.CreateText(openFileDialog1.FileName);
            string line;
            for (int i = 0; i < MainTextBox.Lines.Length; i++)
            {
                line = MainTextBox.Lines[i].ToString();

                streamWriter.WriteLine(line);
            }

            streamWriter.Close();

            MessageBox.Show("Файл сохранен","", MessageBoxButtons.OK);
        }
    }
}
