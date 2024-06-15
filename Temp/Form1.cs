using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace Temp
{
    public partial class Form1 : Form
    {
        SerialPort serialPort = new SerialPort("COM3", 9600); // Adjust the COM port as needed
        private int elapsedTime = 0;
        public Form1()
        {
            try
            {
                InitializeComponent();
                serialPort.DataReceived += new SerialDataReceivedEventHandler(SerialPort_DataReceived);
                serialPort.Open();
                InitializeChart();
            }
            catch (Exception)
            {
                MessageBox.Show( "It was not possible to keep the serial communication", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            chart1.ChartAreas[0].AxisX.Title = "Tempo (ms)";

            // Configura o eixo Y para luminosidade
            chart1.ChartAreas[0].AxisY.Title = "Luminosidade";
        }
        private void InitializeChart()
        {
            // Configure the Chart control (chart1 is the name of your chart control)
            chart1.Series.Clear();
            var series = chart1.Series.Add("Luminosity");
            series.ChartType = SeriesChartType.Line;
        }
        private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            /*
            string data = serialPort.ReadLine();
            this.Invoke(new Action(() => listBox1.Items.Add(data)));
            UpdateChart(data);
            */
            string data = serialPort.ReadLine();
            this.Invoke(new Action(() =>
            {
                listBox1.Items.Add(data);
                listBox1.SelectedIndex = listBox1.Items.Count - 1;
                listBox1.ClearSelected();
            }));
            UpdateChart(data);
        }
        private void UpdateChart(string data)
        {
            if (chart1.InvokeRequired)
            {
                /*
                chart1.Invoke(new Action(() =>
                {
                    // Parse the data to a double (assuming data is in correct format)
                    if (double.TryParse(data, out double luminosityValue))
                    {
                        // Add the luminosity value to the chart
                        chart1.Series["Luminosity"].Points.AddY(luminosityValue);
                    }
                }));
                */
                chart1.Invoke(new Action(() =>
                {
                    // Parse the data to a double (assuming data is in correct format)
                    if (double.TryParse(data, out double luminosityValue))
                    {
                        // Add the luminosity value to the chart with the elapsed time as X value
                        chart1.Series["Luminosity"].Points.AddXY(elapsedTime, luminosityValue);

                        // Incrementa o tempo decorrido
                        elapsedTime += 500;
                    }
                }));
            }
        }

        private void Print_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "PNG Image|*.png|JPEG Image|*.jpg|Bitmap Image|*.bmp";
                saveFileDialog.Title = "Salvar gráfico como imagem";
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    chart1.SaveImage(saveFileDialog.FileName, System.Windows.Forms.DataVisualization.Charting.ChartImageFormat.Png);
                    MessageBox.Show("Saved suecfully", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void Save_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "Text File|*.txt";
                saveFileDialog.Title = "Salvar dados do ListBox";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    using (StreamWriter sw = new StreamWriter(saveFileDialog.FileName))
                    {
                        foreach (var item in listBox1.Items)
                        {
                            sw.WriteLine(item.ToString());
                        }
                        MessageBox.Show("Saved suecfully","Information" , MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
        }
    }
}




