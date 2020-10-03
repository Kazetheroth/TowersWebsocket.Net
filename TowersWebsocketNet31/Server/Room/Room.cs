namespace TowersWebsocketNet31.Server.Room
{
    public class Room
    {
        private int id;
        private string name;
        private string password;
        private int roomOwner;
        private int maxPlayers;
        private string mode;
        private bool isRanking;
        private bool isPublic;
        private bool isLaunched;
        private bool hasEnded;

        public Room(int id, string name, string password, int roomOwner, int maxPlayers, string mode, bool isRanking, bool isPublic, bool isLaunched, bool hasEnded)
        {
            this.id = id;
            this.name = name;
            this.password = password;
            this.roomOwner = roomOwner;
            this.maxPlayers = maxPlayers;
            this.mode = mode;
            this.isRanking = isRanking;
            this.isPublic = isPublic;
            this.isLaunched = isLaunched;
            this.hasEnded = hasEnded;
        }
    }
}