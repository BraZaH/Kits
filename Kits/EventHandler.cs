namespace Kits
{
    using Exiled.Events.EventArgs.Player;
    using Exiled.Events.EventArgs.Server;
    using System.Collections.Generic;
    using MEC;
    public class EventHandler
    {
        public static Dictionary<string, List<string>> RoundKitUses = new Dictionary<string, List<string>>();
        public void OnVerifiedPlayer(VerifiedEventArgs ev)
        {
            Timing.CallDelayed(0.8f, () => Database.Extension.CheckPlayer(ev.Player));
        }
        public void OnRoundEnding(EndingRoundEventArgs ev)
        {
            RoundKitUses.Clear();
        }
    }
}
