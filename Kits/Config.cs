using System.ComponentModel;
using System.Collections.Generic;
using Exiled.API.Enums;
using Exiled.API.Interfaces;
using Exiled.API.Features;
using Kits.Features;
using System.IO;

namespace Kits
{
    public class Config : IConfig
    {
        public bool IsEnabled { get; set; } = true;
        public bool Debug { get; set; } = true;
        public string DatabaseFolder { get; set; } = Path.Combine(Paths.Plugins, "KitsDatabase");
        [Description("Se necesita para el reinicio de usos autoaticamente de forma diaria")]
        public bool IsMainServer { get; set; }
        [Description("Nombre del kit: objeto - cantidad de veces que se va a dar (si UserGroup contiene 'none' el acceso del KIT sera para todos)")]
        public Dictionary<string, ItemKit> Kits { get; set; } = new Dictionary<string, ItemKit>()
        {
            {
                "Feca", new ItemKit
                {
                    Items = new List<ItemType>()
                    {
                        ItemType.Coin,
                        ItemType.Coal
                    },
                    Ammo = new List<AmmoStruct>()
                    {
                        new AmmoStruct()
                        {
                            Type = AmmoType.Nato556,
                            Amount = 200
                        }
                    },
                    UserGroup = new Dictionary<string, int>()
                    {
                        {"VipKeter",5 },
                        {"none", 2 }
                    }
                }
            }
        };
    }
}
