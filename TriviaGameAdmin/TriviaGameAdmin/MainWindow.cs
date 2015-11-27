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
            client.Connect(30);
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

        /*
        *METHOD		    :	btn_ExportExcel_Click
        *
        *DESCRIPTION	:	Method called when the 'Export Excel' button is clicked
        *                   Calls a method to create an Excel sheet and chart with data
        *
        *PARAMETERS		:	object sender:  Object relaying information on where the event call came from
        *                   EventArgs e:    Object that contains data about the event
        *  
        *RETURNS		:	void
        *
        */
        private void btn_ExportExcel_Click(object sender, EventArgs e)
        {
            this.CreateExcelSheet();
        }

        /*
        *METHOD		    :	CreateExcelSheet
        *
        *DESCRIPTION	:	Method creates a workbook with a sheet, and adds the question number, question 
        *                   text, average correct answer time and percentage of users who guessed correctly 
        *                   to the sheet
        *                   This method calls another method to create a chart
        *                   This method is a modified version of the button1_Click method from
        *                   https://support.microsoft.com/en-us/kb/302084
        *
        *PARAMETERS		:	
        *  
        *RETURNS		:	void
        *
        */
        public void CreateExcelSheet()
        {
            Excel.Application oXL;
            Excel._Workbook oWB;
            Excel._Worksheet oSheet;
            Excel.Range oRng;

            try
            {
                //start Excel and get Application object
                oXL = new Excel.Application();
                oXL.Visible = true;
                //get a new workbook
                oWB = (Excel._Workbook)(oXL.Workbooks.Add(Missing.Value));
                oSheet = (Excel._Worksheet)oWB.ActiveSheet;

                //add table headers going cell by cell
                oSheet.Cells[1, 1] = "Question Number";
                oSheet.Cells[1, 2] = "Question Text";
                oSheet.Cells[1, 3] = "Average Correct Answer Time (s)";
                oSheet.Cells[1, 4] = "% Who Answered Correctly";
                //format A1:D1 as bold, vertical and horizontal alignment is center, and text wrapping is true
                oSheet.get_Range("A1", "D1").Font.Bold = true;
                oSheet.get_Range("A1", "D1").VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
                oSheet.get_Range("A1", "D1").HorizontalAlignment = Excel.XlVAlign.xlVAlignCenter;
                oSheet.get_Range("A1", "D1").WrapText = true;

                //send a user command to the service
                output.WriteLine("GetExcel.");
                output.Flush();

                //get the question number and question text
                Object[,] questionArray = new Object[11, 2];
                //read the question text from the service
                for (int counter = 0; counter < 10; counter++)
                {
                    String questionText = input.ReadLine();
                    //fill an array with the question number and text
                    questionArray[counter, 0] = counter + 1;
                    questionArray[counter, 1] = questionText;
                }
                //fill A2:B11 with the question number and question text
                oSheet.get_Range("A2", "B11").Value2 = questionArray;
                oRng = oSheet.get_Range("B2", "B11");
                oRng.EntireColumn.AutoFit();

                //get the average time to answer correctly
                float[,] averageTimes = new float[10, 10];
                //read the average times from the service
                for (int counter = 0; counter < 10; counter++)
                {
                    float currentAverageTime = float.Parse(input.ReadLine());
                    //fill an array with the average times
                    averageTimes[counter, 0] = currentAverageTime;
                }
                //fill C2:C11 with the average time to answer questions correctly
                oSheet.get_Range("C2", "C11").Value2 = averageTimes;
                oRng = oSheet.get_Range("C2", "C11");
                oRng.EntireColumn.AutoFit();

                //get the percentage of users who answered the question correctly
                float[,] percentCorrect = new float[10, 10];
                //read the percentages from the service
                for (int counter = 0; counter < 10; counter++)
                {
                    float currentQuestionPercent = float.Parse(input.ReadLine());
                    //fill an array with the percentages
                    percentCorrect[counter, 0] = currentQuestionPercent;
                }
                //fill D2:D11 with the percentage of users who answered correctly
                oSheet.get_Range("D2", "D11").Value2 = percentCorrect;
                oRng = oSheet.get_Range("D2", "D11");
                oRng.EntireColumn.AutoFit();

                ///create a chart
                DisplayHistogram(oSheet);

                //make sure Excel is visible and give the user control of Microsoft Excel's lifetime
                oXL.Visible = true;
                oXL.UserControl = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception: " + ex.Message);
            }
        }

        /*
        *METHOD		    :	DisplayHistogram
        *
        *DESCRIPTION	:	Method creates an Excel chart that displays the average length of time to answer 
        *                   questions correctly
        *                   This method is a modified version of the DisplayQuarterlySales method from
        *                   https://support.microsoft.com/en-us/kb/302084
        *
        *PARAMETERS		:	Excel._Worksheet oWS    A worksheet interface  
        *  
        *RETURNS		:	void
        *
        */
        public void DisplayHistogram(Excel._Worksheet oWS)
        {
            Excel._Workbook oWB;
            Excel.Range oRange;
            Excel._Chart oChart;
            Excel.Series oSeries;

            try
            {
                //add a chart for the selected data
                oWB = (Excel._Workbook)oWS.Parent;
                oChart = (Excel._Chart)oWB.Charts.Add(Missing.Value, Missing.Value, Missing.Value, Missing.Value);

                //use the ChartWizard to create a new chart from the selected data
                oRange = oWS.get_Range("B1", "C11");
                oChart.SetSourceData(oRange, Missing.Value);
                oChart.ChartWizard(oRange, Excel.XlChartType.xlBarClustered, Missing.Value, Excel.XlRowCol.xlColumns, 
                Missing.Value, Missing.Value, Missing.Value, "Average Length of Time to Answer Questions Correctly", 
                "Questions", "Time (seconds)", Missing.Value);
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
