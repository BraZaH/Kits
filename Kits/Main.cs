namespace Kits
{
    using System;
    using Exiled.API.Features;
    using System.Collections.Generic;
    using MEC;
    public class Main : Plugin<Config>
    {
        public override string Name => "Kits";
        public override string Author => "BraZa#5713";
        public override Version Version { get; } = new Version(1, 0, 0);
        public override Version RequiredExiledVersion { get; } = new Version(6, 0, 0);

        public static Main Singleton;
        public EventHandler eventHandler;

        public override void OnEnabled()
        {
            Singleton = this;
            Database.Database.Open();
            eventHandler = new EventHandler();
            Timing.RunCoroutine(CheckHour());

            Exiled.Events.Handlers.Player.Verified += eventHandler.OnVerifiedPlayer;
            Exiled.Events.Handlers.Server.EndingRound += eventHandler.OnRoundEnding;

            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            Singleton = null;
            Database.Database.Close();

            Exiled.Events.Handlers.Player.Verified -= eventHandler.OnVerifiedPlayer;
            Exiled.Events.Handlers.Server.EndingRound -= eventHandler.OnRoundEnding;

            base.OnDisabled();
        }

        private IEnumerator<float> CheckHour()
        {
            if (!Singleton.Config.IsMainServer) yield break;
            Log.Info("Check usos de KITS iniciado");
            for (; ; )
            {
                yield return Timing.WaitForSeconds(3600);
                if(DateTime.Now.Hour == 0)
                {
                    Database.Extension.RestartUses();
                    yield return Timing.WaitForSeconds(3600);
                }
            }
        }
    }
}
