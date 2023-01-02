using System;
using CommandSystem;
using Exiled.API.Enums;
using Exiled.API.Features.Items;
using Kits.Features;

namespace Kits.Features
{
    public class AmmoStruct
    {
        public AmmoType Type { get; set; }
        public ushort Amount { get; set; }
        public void Deconstruct(out AmmoType type, out ushort limit)
        {
            type = Type;
            limit = Amount;
        }
    }
}
