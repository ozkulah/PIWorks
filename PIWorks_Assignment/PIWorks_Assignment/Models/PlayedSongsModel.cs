using System;

namespace PIWorks_Assignment.Models
{
    class PlayedSongsModel
    {
        public string PLAY_ID { get; set; }

        public int SONG_ID { get; set; }

        public int CLIENT_ID { get; set; }

        public DateTime PLAY_TS { get; set; }

        //Play_ID  can be used for other operations
        public PlayedSongsModel(string line)
        {
            var split = line.Split('\t');
            PLAY_ID = split[0];
            if (Int32.TryParse(split[1], out int Song_Id))
                SONG_ID = Song_Id;
            if (Int32.TryParse(split[2], out int Client_Id))
                CLIENT_ID = Client_Id;
            if (DateTime.TryParse(split[3], out DateTime Play_ts))
                PLAY_TS = Play_ts;
        }
    }
}
