using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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

        public Form1()
        {
            InitializeComponent();
            serialPort.DataReceived += new SerialDataReceivedEventHandler(SerialPort_DataReceived);
            serialPort.Open();
            InitializeChart();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

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
            string data = serialPort.ReadLine();
            this.Invoke(new Action(() => listBox1.Items.Add(data)));
            UpdateChart(data);
        }
        private void UpdateChart(string data)
        {
            if (chart1.InvokeRequired)
            {
                chart1.Invoke(new Action(() =>
                {
                    // Parse the data to a double (assuming data is in correct format)
                    if (double.TryParse(data, out double luminosityValue))
                    {
                        // Add the luminosity value to the chart
                        chart1.Series["Luminosity"].Points.AddY(luminosityValue);
                    }
                }));
            }
        }
    }
}

   
