using System;
using System.Collections.Generic;
using System.Windows;
using Microsoft.Win32;
using System.IO;
using PIWorks_Assignment.ViewModels;

namespace PIWorks_Assignment.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string csvFileName;
        public Dictionary<int, int> itemSourceForClienSong;
        MainViewModel mv = new MainViewModel();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void OrganizeButtonsStatus(bool isEnabled)
        {
            btnSingular.IsEnabled = isEnabled;
            btnUploadFile.IsEnabled = isEnabled;
            //btnAnalyzeAll.IsEnabled = isEnabled;
        }

        private void btnOpenFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "CSV files (*.csv)|*.csv"; //Only csv files can be selected
            if (openFileDialog.ShowDialog() == true)
            {
                txtEditor.Text = openFileDialog.FileName;
            }
            if (txtEditor.Text != null)
            {
                csvFileName = txtEditor.Text;
                OrganizeButtonsStatus(true);
            }
        }

        private void btnUploadFile_Click(object sender, RoutedEventArgs e)
        {

            DateTime difference = DateTime.Now; // Operation time begin

            if (csvFileName == null)
                return;

            string line;
            StreamReader file = new StreamReader(csvFileName);
            line = file.ReadLine(); //First line has header of file
            while ((line = file.ReadLine()) != null)
            {
                mv.UploadFileToList(line);  //Upload File line by line
            }

            file.Close();

            TimeSpan dif = -difference.Subtract(DateTime.Now); //Calculate operation time

            lblUpload.Content = "Time of Upload File : " + dif.TotalSeconds.ToString("0.00") + " sec";

            btnAnalyzeAll.IsEnabled = true;
        }

        private void btnAnalyzeAll_Click(object sender, RoutedEventArgs e)
        {
            DateTime difference = DateTime.Now; // Operation time begin

            Dictionary<int, int> result = mv.UploadAndCountDistinctSongCountThirdWay();

            TimeSpan dif = -difference.Subtract(DateTime.Now); //Calculate operation time

            lblAnalyzeAll.Content = "Time of Analyze : " + dif.TotalSeconds.ToString("0.00") + " sec";
            
            DataGridForAll.ItemsSource = result;  //Bind to datagrid
        }

        private void btnSingular_Click(object sender, RoutedEventArgs e)
        {
            if (csvFileName == null)
                return;

            DateTime difference = DateTime.Now; // Operation time begin

            string line;
            StreamReader file = new StreamReader(csvFileName);
            line = file.ReadLine();
            while ((line = file.ReadLine()) != null)
            {
                mv.UploadAndCountDistinctSongCountSingular(line);
            }

            file.Close();

            Dictionary<int, int> result = mv.CountOfDistinctPlayedByClientsCalculation();
            
            TimeSpan dif = -difference.Subtract(DateTime.Now); //Calculate operation time

            lblSingular.Content = "Time of Upload and Analyze : " + dif.TotalSeconds.ToString("0.00") + " sec"; 

            DataGridForSingular.ItemsSource = result; //Bind to datagrid
        }
    }
}
