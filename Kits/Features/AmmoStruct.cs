namespace Kits.Features
{
    using Exiled.API.Enums;
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
