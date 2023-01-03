namespace Kits
{
    using Exiled.Events.EventArgs.Player;
    using Exiled.Events.EventArgs.Server;
    using Exiled.API.Features;
    using System.Collections.Generic;
    using MEC;
    public class EventHandler
    {
        public static Dictionary<string, List<string>> RoundKitUses = new Dictionary<string, List<string>>();
        CoroutineHandle _coroutineHandle = new CoroutineHandle();
        public void OnVerifiedPlayer(VerifiedEventArgs ev)
        {
            _coroutineHandle = Timing.RunCoroutine(WaitForCheckPlayer(ev.Player));
        }
        public void OnRoundEnding(EndingRoundEventArgs ev)
        {
            RoundKitUses.Clear();
        }
        private IEnumerator<float> WaitForCheckPlayer(Player ply)
        {
            yield return Timing.WaitForSeconds(1);
            Database.Extension.CheckPlayer(ply);
            Timing.KillCoroutines(_coroutineHandle);
        }
    }
}
