namespace Kits.Features
{
    using System.Collections.Generic;
    public class ItemKit
    {
        
        public List<ItemType> Items { get; set; } = new List<ItemType>();
        public List<AmmoStruct> Ammo { get; set; } = new List<AmmoStruct>();

        public Dictionary<string, int> UserGroup { get; set; } = new Dictionary<string, int>();
    }
}
