namespace Kits.Commands
{
    using System;
    using CommandSystem;
    using Kits.Database;

    [CommandHandler(typeof(GameConsoleCommandHandler))]
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    class KitRestartUses : ICommand
    {
        public string Command => "RestartKitUses";
        public string[] Aliases => new[] { "RKU" };
        public string Description => "Reinicia los usos de los vip.";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            Extension.RestartUses();
            response = "Comando ejecutado con exito";
            return true;
        }
    }
}
