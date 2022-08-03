using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.IO;        //추가


namespace project
{
    
    public partial class Form1 : Form
    {
        private string stFolderPath1;
        private System.IO.DirectoryInfo di;

        //그래프
        int Min = Int32.MaxValue;
        int Max = Int32.MinValue;
        int Min2 = Int32.MaxValue;
        int Max2 = Int32.MinValue;
        double[] ecg = new double[100000];
        double[] ppg = new double[100000];
        private int ecgLength;
        private int ppgLength;
        private int Tick = 0;
        private bool isTimerOn = true;
        string[] time_table= new string [10000];
        int[] avg = new int[10000];
        int index = 0;

        public Form1()
        {
            InitializeComponent();
        }

        private void chart1_Click(object sender, EventArgs e)
        {
            if(isTimerOn)
            {
                timer1.Stop(); isTimerOn = false;
            }
            else
            {
                timer1.Start();
                isTimerOn = true;
            }
        }
        private void ecgppgRead()
        {
            string filename = @"C:\\Users\\dydrk\\source\\repos\\WindowsFormsApp3\\WindowsFormsApp3\\a101.txt";
            string[] lines = System.IO.File.ReadAllLines(filename);
            string filename2 = @"C:\\Users\\dydrk\\source\\repos\\WindowsFormsApp3\\WindowsFormsApp3\\PPG.txt";
            string[] lines2 = System.IO.File.ReadAllLines(filename2);
            for (int i=0;i<index;i++)
            {
                if (avg[i] < Min)
                    Min = avg[i];
                if (avg[i] > Max)
                    Max = avg[i]; 
            }
            ecgLength = index;
            Console.WriteLine("Min = {0}, Max ={1}", Min, Max);
            i = 0;
            foreach (string line in lines2)
            {
                ppg[i] = Convert.ToDouble(line);
                if (ppg[i] < Min2) Min2 = Convert.ToInt32(ppg[i]);
                if (ppg[i] > Max2) Max2 = Convert.ToInt32(ppg[i]);
                i++;
            }
            ppgLength = i;
            Console.WriteLine("Min2 = {0}, Max2 = {1}", Min2, Max2);
        }
      
        private void timer1_Tick_1(object sender, EventArgs e)
        {
            ecgChartSetting(Tick * 10);
            Tick += 1;
        }
        private void ecgChartSetting(int minX)
        {
            chart1.ChartAreas["ChartArea1"].AxisX.Minimum = minX;
            chart1.ChartAreas["ChartArea1"].AxisX.Maximum = minX + 1500;
            chart1.ChartAreas["ChartArea1"].AxisY.Minimum = Min / 100 * 100;
            chart1.ChartAreas["ChartArea1"].AxisY.Maximum = Max / 100 * 100;
            chart1.ChartAreas["ChartArea2"].AxisX.Minimum = minX;
            chart1.ChartAreas["ChartArea2"].AxisX.Maximum = minX + 1500;
            chart1.ChartAreas["ChartArea2"].AxisY.Minimum = Min2;
            chart1.ChartAreas["ChartArea2"].AxisY.Maximum = Max2; ;
        }




        private void button1_Click(object sender, EventArgs e)
        {
            StreamReader SR = new StreamReader("C:\\Users\\dydrk\\source\\repos\\project\\project\\dumpState.txt");    //읽어들일 TXT 파일의 경로를 
                                                                                                                                        //매개변수로 StreamReader 생성

            string line="";                                            //한 줄씩 읽은 후, 그 값을 저장시킬 변수
            bool check = false;
            string result = "";                                     //전체 라인을 저장시킬 변수
            
            while ((line = SR.ReadLine()) != null)                  //line변수에 SR에서 한줄을 읽은 걸 저장, 읽은 줄이 null이 아닐때까지 반복
            {
                if (check == false && line.StartsWith("network statistics:") == true)
                {
                    check = true;
                }
                else if (check == true && line.StartsWith("packet wakeup events:") != true)
                {
                    //result += line;                                     //전체 라인 변수에 그 값을 저장
                    //result += "\r\n";

                    
                    //time_table[index] = line.Split(' ')[0];
                    //string avg_str = line.Split(' ')[3];
                    int avg_index = line.IndexOf("avg"); int avg_ms_index = line.IndexOf("ms"); int avg_count = avg_ms_index - (avg_index + 4);
                    //avg[index] = Int32.Parse(avg_str.Substring(avg_index + 3, avg_count));
                    if (avg_index > 0)
                    {
                        //listBox1.Items.Add(line.Substring(avg_index + 4, avg_count));
                        avg[index] = Int32.Parse(line.Substring(avg_index + 4, avg_count));
                        index++;

                    }
                }//표출을 위해서 추가
                else if (line.StartsWith("packet wakeup events:") == true)
                {
                    break;
                }
            }

                                    
            SR.Close();
        }


        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
