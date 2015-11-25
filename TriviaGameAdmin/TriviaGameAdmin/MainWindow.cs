using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.IO.Pipes;
using Excel = Microsoft.Office.Interop.Excel;
using System.Reflection;

namespace TriviaGameAdmin
{
    public partial class MainWindow : Form
    {
        NamedPipeClientStream client;
        NamedPipeServerStream server;
        StreamReader input;
        StreamWriter output;

        public MainWindow(string userName, string serverName, string pipeName)
        {
            InitializeComponent();

            //set up the named pipe security
            PipeSecurity ps = new PipeSecurity();
            System.Security.Principal.SecurityIdentifier sid = new System.Security.Principal.SecurityIdentifier(System.Security.Principal.WellKnownSidType.WorldSid, null);
            PipeAccessRule par = new PipeAccessRule(sid, PipeAccessRights.ReadWrite, System.Security.AccessControl.AccessControlType.Allow);
            ps.AddAccessRule(par);

            //connect to service
            client = new NamedPipeClientStream(pipeName + "service");//naming convention for pipe is given name(pipeName) and who has the server (service or user)
            client.Connect();
            output = new StreamWriter(client);

            //tell service the name of the computer to connect back to
            output.WriteLine(Environment.MachineName);
            output.Flush();

            server = new NamedPipeServerStream(pipeName + "User");//naming convention for pipe is given name(pipeName) and who has the server (service or user)
            server.WaitForConnection();
            input = new StreamReader(server);
        }

        private void btn_EditQuestion_Click(object sender, EventArgs e)
        {
            EditQuestions edit = new EditQuestions(input, output);
            edit.Show();
        }

        private void btn_CurrentStatus_Click(object sender, EventArgs e)
        {
            CurrentStatus status = new CurrentStatus(input, output);
            status.Show();
        }

        private void btn_Leaderboard_Click(object sender, EventArgs e)
        {
            Leaderboard leaders = new Leaderboard(input, output);
            leaders.Show();
        }

        private void btn_ExportExcel_Click(object sender, EventArgs e)
        {
            Excel.Application oXL;
            Excel._Workbook oWB;
            Excel._Worksheet oSheet;
            Excel.Range oRng;

            try
            {
                //Start Excel and get Application object.
                oXL = new Excel.Application();
                oXL.Visible = true;

                //Get a new workbook.
                oWB = (Excel._Workbook)(oXL.Workbooks.Add(Missing.Value));
                oSheet = (Excel._Worksheet)oWB.ActiveSheet;

                //Add table headers going cell by cell.
                oSheet.Cells[1, 1] = "Question Number";
                oSheet.Cells[1, 2] = "Question Text";
                oSheet.Cells[1, 3] = "Average Correct Answer Time (s)";
                oSheet.Cells[1, 4] = "% Who Answered Correctly";

                //Format A1:K1 as bold, vertical alignment = center, and wrapping = true.
                oSheet.get_Range("A1", "D1").Font.Bold = true;
                oSheet.get_Range("A1", "D1").VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
                oSheet.get_Range("A1", "D1").HorizontalAlignment = Excel.XlVAlign.xlVAlignCenter;
                oSheet.get_Range("A1", "D1").WrapText = true;

                output.WriteLine("GetExcel.");
                output.Flush();

                // Made this array of type object to store both strings and ints
                Object[,] questionArray = new Object[11, 2];

                for (int counter = 0; counter < 10; counter++)
                {
                    String questionText = input.ReadLine();
                    questionArray[counter, 0] = counter + 1;
                    questionArray[counter, 1] = questionText;
                }

                //Fill A2:B6 with an array of values (Question # and Question Text).
                oSheet.get_Range("A2", "B11").Value2 = questionArray;
                oRng = oSheet.get_Range("B2", "B11");
                oRng.EntireColumn.AutoFit();

                // Get the average time to answer correctly
                float[,] averageTimes = new float[10, 10];
                for (int counter = 0; counter < 10; counter++)
                {
                    float currentAverageTime = float.Parse(input.ReadLine());
                    averageTimes[counter, 0] = currentAverageTime;
                }

                // Fill J2:J11 with an array of average times, and autofit the columns
                oSheet.get_Range("C2", "C11").Value2 = averageTimes;
                oRng = oSheet.get_Range("C2", "C11");
                oRng.EntireColumn.AutoFit();

                // Get the percentage of users who answered the question correctly
                float[,] percentCorrect = new float[10, 10];
                for (int counter = 0; counter < 10; counter++)
                {
                    float currentQuestionPercent = float.Parse(input.ReadLine());
                    percentCorrect[counter, 0] = currentQuestionPercent;
                }

                // Fill K2:K11 with an array of percents, and autofit the column
                oSheet.get_Range("D2", "D11").Value2 = percentCorrect;
                oRng = oSheet.get_Range("D2", "D11");
                oRng.EntireColumn.AutoFit();

                //Manipulate a variable number of columns for Quarterly Sales Data.
                DisplayHistogram(oSheet);

                //Make sure Excel is visible and give the user control
                //of Microsoft Excel's lifetime.
                oXL.Visible = true;
                oXL.UserControl = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception: " + ex.Message);
            }
        }

        // In addition, a histogram should be created to show the average length of time needed to answer each question correctly
        // this method is a modified version of the code from https://support.microsoft.com/en-us/kb/302084
        public void DisplayHistogram(Excel._Worksheet oWS)
        {
            Excel._Workbook oWB;
            Excel.Range oRange;
            Excel._Chart oChart;
            Excel.Series oSeries;

            try
            {
                //Add a Chart for the selected data.
                oWB = (Excel._Workbook)oWS.Parent;
                oChart = (Excel._Chart)oWB.Charts.Add(Missing.Value, Missing.Value, Missing.Value, Missing.Value);

                //Use the ChartWizard to create a new chart from the selected data.
                oRange = oWS.get_Range("B1", "C11");
                oChart.SetSourceData(oRange, Missing.Value);
                oChart.ChartWizard(oRange, Excel.XlChartType.xlBarClustered, Missing.Value,
                    Excel.XlRowCol.xlColumns, Missing.Value, Missing.Value, Missing.Value,
                    "Average Length of Time to Answer Questions Correctly", "Questions", "Time (seconds)", Missing.Value);
                oSeries = (Excel.Series)oChart.SeriesCollection(1);
                oSeries.XValues = oWS.get_Range("B2", "B11");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception: " + ex.Message);
            }
        }
    }
}
