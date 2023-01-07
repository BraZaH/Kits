namespace Kits.Features
{
    using System.Collections.Generic;
    using InventorySystem.Items.Usables.Scp330;

    public class ItemKit
    {
        
        public List<ItemType> Items { get; set; } = new List<ItemType>();
        public List<AmmoStruct> Ammo { get; set; } = new List<AmmoStruct>();
        public List<CandyKindID> Candies { get; set; } = new List<CandyKindID>();

        public Dictionary<string, int> UserGroup { get; set; } = new Dictionary<string, int>();
    }
}
