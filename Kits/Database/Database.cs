namespace Kits.Database
{
    using System.IO;
    using LiteDB;

    class Database
    {
        public static LiteDatabase LiteDatabase { get; set; }
        public static string Folder = Main.Singleton.Config.DatabaseFolder;
        public static string FilePath = Path.Combine(Folder, "UserData.db");
        public static void Open()
        {
            if (!Directory.Exists(Folder))
                Directory.CreateDirectory(Folder);
            LiteDatabase = new LiteDatabase(new ConnectionString(FilePath)
            {
                Connection = ConnectionType.Shared
            });

            LiteDatabase.GetCollection<PlayerData>().EnsureIndex(x => x.UserID, true);
            LiteDatabase.GetCollection<PlayerData>().EnsureIndex(x => x.GroupName);
            LiteDatabase.GetCollection<PlayerData>().EnsureIndex(x => x.KitsUses);
        }

        public static void Close()
        {
            LiteDatabase.Checkpoint();
            LiteDatabase.Dispose();
            LiteDatabase = null;
        }
    }
}
