namespace Kits
{
    using Exiled.Events.EventArgs.Player;
    using Exiled.Events.EventArgs.Server;
    using Exiled.API.Features;
    using System.Collections.Generic;
    using Exiled.API.Features.Items;
    using MEC;
    public class EventHandler
    {
        public static Dictionary<string, List<string>> RoundKitUses = new Dictionary<string, List<string>>();
        CoroutineHandle _coroutine = new CoroutineHandle();
        public void OnVerifiedPlayer(VerifiedEventArgs ev)
        {
            Timing.CallDelayed(4f,() => Database.Extension.CheckPlayer(ev.Player));
            //_coroutine = Timing.RunCoroutine(PlayerCheck(ev.Player));
        }
        public void OnRoundEnding(EndingRoundEventArgs ev)
        {
            RoundKitUses.Clear();
        }

        public void OnLeave(LeftEventArgs ev)
        {

        }

        IEnumerator<float> PlayerCheck(Player ply)
        {
            yield return Timing.WaitForSeconds(2);
            Database.Extension.CheckPlayer(ply);
            Timing.KillCoroutines(_coroutine);
        }
    }
}
