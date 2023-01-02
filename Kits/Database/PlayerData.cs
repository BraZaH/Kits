namespace Kits.Database
{
    using LiteDB;
    using System.Collections.Generic;

    class PlayerData
    {
        [BsonId]
        public string UserID { get; set; }
        public string GroupName { get; set; }
        public Dictionary<string, int> KitsUses { get; set; }
    }
}
