using System;
using CommandSystem;
using Exiled.API.Features;
using Exiled.API.Features.Items;
using Kits.Features;

namespace Kits.Commands
{
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    [CommandHandler(typeof(ClientCommandHandler))]
    class KitCommand : ICommand
    {
        public string Command => "Kit";
        public string[] Aliases => new[] {"SelectKit"};
        public string Description  => "Elige uno de los kit existentes.";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if(Round.IsEnded || !Round.IsStarted)
            {
                response = "<color=red>No puedes usar este comando cuando la partida no haya comenzado</color>";
                return false;
            }
            Player _player = Player.Get(sender);
            if(_player.Role.Side == Exiled.API.Enums.Side.Scp || _player.Role.Side == Exiled.API.Enums.Side.None || _player.Role.Side == Exiled.API.Enums.Side.Tutorial)
            {
                response = "<color=red>No puedes usar kits siendo SCP / Espectador o Tutorial</color>";
                return false;
            }
            string _kit = string.Empty;
            string _dispoKits = string.Empty;
            foreach (string kit in Main.Singleton.Config.Kits.Keys)
            {
                string _ItemsList = string.Empty;
                string _AmmoList = string.Empty;
                string _GroupList = string.Empty;
                Main.Singleton.Config.Kits.TryGetValue(kit, out ItemKit _ik);
                foreach (ItemType items in _ik.Items)
                {
                    _ItemsList += $" {items} ";
                }
                foreach (AmmoStruct ammo in _ik.Ammo)
                {
                    _AmmoList += $" {ammo.Type} x{ammo.Amount} ";
                }
                foreach (var group in _ik.UserGroup)
                {
                    _GroupList += $" {group.Key} ";
                }
                _dispoKits += $"➤ {kit} |{_ItemsList}| {_AmmoList.Trim()} |({_GroupList.Trim().Replace("none", "Miembros")})\n";
            }
            string ErrResponse = $" <color=red>Kit inexistente o falta de argumentos, por favor eliga uno de los siguientes kits disponibles:</color>\n<color=green>{_dispoKits}</color>";
            try
            {
                _kit = arguments.At(0);
                string str = _kit;
                _kit = char.ToUpper(str[0]) + str.Substring(1).ToLower();
                if (!Main.Singleton.Config.Kits.ContainsKey(_kit))
                {
                    response = ErrResponse;
                    return false;
                }
            }
            catch (Exception)
            {
                response = ErrResponse;
                return false;
            }
            Main.Singleton.Config.Kits.TryGetValue(_kit, out ItemKit _itemKit);
            if(_itemKit.UserGroup.ContainsKey("none") || _itemKit.UserGroup.ContainsKey(_player.GroupName))
            {
                foreach (var _item in _itemKit.Items)
                {
                    if (!_player.IsInventoryFull)
                    {
                        _player.AddItem(_item);
                    }
                    else
                    {
                        Item.Create(_item).CreatePickup(_player.Position);
                    }
                }
                foreach (var _ammo in _itemKit.Ammo)
                {
                    _player.AddAmmo(_ammo.Type, _ammo.Amount);
                }
                response = $"<color=green>Kit </color><color=lime>{_kit}</color><color=green> reclamado con exito.</color>";
                return true;
            }
            else
            {
                response = "<color=red>No tienes permisos para usar este KIT</color>";
                return false;
            }
        }
    }
}
