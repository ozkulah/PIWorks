using MvvmDialogs;
using MvvmDialogs.FrameworkDialogs.OpenFile;
using MvvmDialogs.FrameworkDialogs.SaveFile;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Input;
using PIWorks_Assignment.Utils;
using PIWorks_Assignment.Models;

namespace PIWorks_Assignment.ViewModels
{
    class MainViewModel : ViewModelBase
    {
        #region Parameters
        private readonly IDialogService DialogService;

        /// <summary>
        /// Title of the application, as displayed in the top bar of the window
        /// </summary>
        public string Title
        {
            get { return "PIWorks Assignment"; }
        }

        //Struct for Hashset usage
        public struct ClientSongPair
        {
            public int clientID ;
            public int songID;
        }

        Dictionary<int, int> countOfDistinctPlayedByClientsDic = new Dictionary<int, int>();
        Dictionary<int, int> countOfPlayedClientSongsDic = new Dictionary<int, int>();
        HashSet<ClientSongPair> clientHashSet = new HashSet<ClientSongPair>();
        PlayedSongsModel pSong;

        DateTime August10 = new DateTime(2016, 8, 10); //Spesific date for the question

        List<PlayedSongsModel> playedSongsList = new List<PlayedSongsModel>();
        #endregion

        #region Constructors
        public MainViewModel()
        {
            // DialogService is used to handle dialogs
            this.DialogService = new MvvmDialogs.DialogService();
        }




        #endregion

        #region Methods

        #region Upload and Analyze All Methods Lambda Expressions
        //Uploading data to an object list
        public void UploadFileToList(string line)
        {
            pSong = new PlayedSongsModel(line);
            playedSongsList.Add(pSong);
        }

        //To see the operation time upload file function is seperated
        //Analyzing data in lambda experessions
        //I seperated lambda expressions to three part for readability 
        //(One lambda expression method is in region Unused methods)
        public Dictionary<int, int> CountAllDistictPlayedByClients()
        {
            //Eliminating the same song repeats and selecting songs on specific date
            var clientSongMatches = playedSongsList
                            .Where(z => z.PLAY_TS.Date == August10.Date)  // Choose specific date
                            .GroupBy(x => new { x.CLIENT_ID, x.SONG_ID })
                            .Select(x => x.First()); // Take only first of repeates
           
            //Finding song count played by clients
            var countOfPlayedClientSongs = clientSongMatches.GroupBy(x => x.CLIENT_ID)
                        .ToDictionary(y => y.Key, y => y.Count());

            //Finding distinct client and played song counts 
            var countOfDistinctPlayedByClients = countOfPlayedClientSongs.GroupBy(x => x.Value)
                        .ToDictionary(y => y.Key, y => y.Count());

            return countOfDistinctPlayedByClients;
        }

        #region Unused methods

        //Reading and analyzing data in one lambda experssion
        public Dictionary<int, int> UploadAllAndCountDistictValuesAll(string csvFileName)
        {
            var playedSongs = File.ReadLines(csvFileName).Skip(1) //Read file and skip header line
                            .Select(line => new PlayedSongsModel(line)) //Convert lines to objects
                            .Where(x => x.PLAY_TS.Date == August10.Date) //Check spesific date
                            .GroupBy(x => new { x.CLIENT_ID, x.SONG_ID })
                            .Select(x => x.First()) //Eliminate repeated songs
                            .GroupBy(x => x.CLIENT_ID)
                            .ToDictionary(y => y.Key, y => y.Count()) // Count songs of clients
                            .GroupBy(x => x.Value)
                            .ToDictionary(z => z.Key, z => z.Count()); // Count distinct played songs

            //Console.WriteLine("Client Played Count " + playedSongs.Count());

            return playedSongs;
        }

        //Analyzing data in one lambda experssion
        public Dictionary<int, int> UploadAndCountDistinctSongCountThirdWay()
        {
            var playedSongs = playedSongsList
                            .Where(x => x.PLAY_TS.Date == August10.Date) //Check spesific date
                            .GroupBy(x => new { x.CLIENT_ID, x.SONG_ID })
                            .Select(x => x.First()) //Eliminate repeated songs
                            .GroupBy(x => x.CLIENT_ID)
                            .ToDictionary(y => y.Key, y => y.Count()) // Count songs of clients
                            .GroupBy(x => x.Value)
                            .ToDictionary(z => z.Key, z => z.Count()); // Count distinct played songs

            return playedSongs;
        }

        #endregion Unused methods

        #endregion Upload and Analyze All using Lambda Expressions

        #region Upload and Analyze Line by Line using Dictionary and Hashset

        //Uploading and analyzing data line by line
        public void UploadAndCountDistinctSongCountSingular(string line)
        {
            try
            {
                //Create and object from line
                pSong = new PlayedSongsModel(line);
                //Check play time stamp for specific date
                if (pSong.PLAY_TS.Date == August10.Date)
                {
                    //Create a struct for hash set to eliminate repeated songs
                    ClientSongPair pair;
                    pair.clientID = pSong.CLIENT_ID;
                    pair.songID = pSong.SONG_ID;

                    //Check for repeated songs
                    if (!clientHashSet.Contains(pair))
                    {
                        //Add unique matches to hashset
                        clientHashSet.Add(pair);

                        //Check the dictionary for client
                        if (countOfPlayedClientSongsDic.TryGetValue(pSong.CLIENT_ID, out int countOfSong))
                        {
                            //If it is in dictionary increase counter
                            countOfSong++;
                            countOfPlayedClientSongsDic.Remove(pSong.CLIENT_ID);
                            countOfPlayedClientSongsDic.Add(pSong.CLIENT_ID, countOfSong);
                        }
                        else
                        {
                            //If it is not in dictionary, add it
                            countOfPlayedClientSongsDic.Add(pSong.CLIENT_ID, 1);
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        //Calculate client count with same played song count
        public Dictionary<int, int> CountOfDistinctPlayedByClientsCalculation()
        {
            //Check every member of dictionary and sum client counts if they played same count of song
            foreach (var item in countOfPlayedClientSongsDic)
            {
                //Check the distinct played dictionary for client counts
                if (countOfDistinctPlayedByClientsDic.TryGetValue(item.Value, out int count))
                {
                    //If it is in dictionary increase counter
                    count++;
                    countOfDistinctPlayedByClientsDic.Remove(item.Value);
                    countOfDistinctPlayedByClientsDic.Add(item.Value, count);
                }
                else
                {
                    //If it is not in dictionary, add it
                    countOfDistinctPlayedByClientsDic.Add(item.Value, 1);
                }
            }

            //Console.WriteLine("Played Songs " + clientHashSet.Count);
            //Console.WriteLine("Client Played Count " + countOfDistinctPlayedByClientsDic.Count);

            return countOfDistinctPlayedByClientsDic;
        }

        #endregion Upload and Analyze Line by Line using Dictionary and Hashset

       


        #endregion

        #region Commands
        public RelayCommand<object> SampleCmdWithArgument { get { return new RelayCommand<object>(OnSampleCmdWithArgument); } }

        public ICommand SaveAsCmd { get { return new RelayCommand(OnSaveAsTest, AlwaysFalse); } }
        public ICommand SaveCmd { get { return new RelayCommand(OnSaveTest, AlwaysFalse); } }
        public ICommand NewCmd { get { return new RelayCommand(OnNewTest, AlwaysFalse); } }
        public ICommand OpenCmd { get { return new RelayCommand(OnOpenTest, AlwaysFalse); } }
        public ICommand ExitCmd { get { return new RelayCommand(OnExitApp, AlwaysTrue); } }

        private bool AlwaysTrue() { return true; }
        private bool AlwaysFalse() { return false; }

        private void OnSampleCmdWithArgument(object obj)
        {
            // TODO
        }

        private void OnSaveAsTest()
        {
            var settings = new SaveFileDialogSettings
            {
                Title = "Save As",
                Filter = "Sample (.xml)|*.xml",
                CheckFileExists = false,
                OverwritePrompt = true
            };

            bool? success = DialogService.ShowSaveFileDialog(this, settings);
            if (success == true)
            {
                // Do something
            }
        }
        private void OnSaveTest()
        {
            // TODO
        }
        private void OnNewTest()
        {
            // TODO
        }
        private void OnOpenTest()
        {
            var settings = new OpenFileDialogSettings
            {
                Title = "Open",
                Filter = "Sample (.xml)|*.xml",
                CheckFileExists = false
            };

            bool? success = DialogService.ShowOpenFileDialog(this, settings);
            if (success == true)
            {
                // Do something
            }
        }

        private void OnExitApp()
        {
            System.Windows.Application.Current.MainWindow.Close();
        }
        #endregion

        #region Events

        #endregion
    }
}
