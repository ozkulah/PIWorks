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
        
        //Equals methods are overrided so object contain/compare methods can be used in hashset and other lists
        public override bool Equals(object newplayedSongsModel)
        {
            if (newplayedSongsModel == null)
                return false;

            PlayedSongsModel playedSongsModel = newplayedSongsModel as PlayedSongsModel;
            if (playedSongsModel == null)
                return false;
            else
                return Equals(playedSongsModel);
        }

        //Create a unique and fast hashcode for our task
        public override int GetHashCode()
        {
            // 269 and 47 are primes
            int hash = 269;
            hash = (hash * 47) + CLIENT_ID.GetHashCode();
            hash = (hash * 47) + SONG_ID.GetHashCode();
            return hash;
        }

        //Only CLIENT_ID & SONG_ID used to compare because other variables always different and unimportant in our task
        public bool Equals(PlayedSongsModel other)
        {
            if (other == null)
                return false;

            return (this.CLIENT_ID.Equals(other.CLIENT_ID) && this.SONG_ID.Equals(other.SONG_ID));
        }
    }
}
