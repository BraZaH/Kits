using System;
using Exiled.API.Features;

namespace Kits
{
    public class Main : Plugin<Config>
    {
        public override string Name => "Kits";
        public override string Author => "BraZa#5713";
        public override Version Version { get; } = new Version(1, 0, 0);
        public override Version RequiredExiledVersion { get; } = new Version(6, 0, 0);

        public static Main Singleton;

        public override void OnEnabled()
        {
            Singleton = this;

            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            Singleton = null;

            base.OnDisabled();
        }
    }
}
