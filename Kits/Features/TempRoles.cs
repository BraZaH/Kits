using System.Collections.Generic;

namespace Kits.Features
{
    public static class TempRoles
    {
        public static Dictionary<string, string> _TempRoles { get; } = new Dictionary<string, string>()
        {
            {"Miembro","user" },
            {"V.I.P Safe", "VipSafe" },
            {"V.I.P Euclid", "VipEuclid" },
            {"V.I.P Keter", "VipKeter" },
            {"V.I.P Eventos", "VipEventos" }
        };
    }
}
