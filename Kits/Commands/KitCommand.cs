﻿namespace Kits.Commands
{
    using System;
    using System.Collections.Generic;
    using CommandSystem;
    using Exiled.API.Features;
    using Exiled.API.Features.Items;
    using Kits.Features;
    using Kits.Database;

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
            PlayerData _playerData = Database.LiteDatabase.GetCollection<PlayerData>().FindOne(e => e.UserID == _player.UserId);

            if(_player.Role.Side == Exiled.API.Enums.Side.Scp || _player.Role.Side == Exiled.API.Enums.Side.None || _player.Role.Side == Exiled.API.Enums.Side.Tutorial)
            {
                response = "<color=red>No puedes usar kits siendo SCP / Espectador o Tutorial</color>";
                return false;
            }

            string _kit = string.Empty;
            string _dispoKits = string.Empty;
            foreach (var kit in _playerData.KitsUses)
            {
                int _leftUses = kit.Value;
                string _color = string.Empty;

                if (_leftUses > 0)
                    _color = "green";
                else
                    _color = "silver";
                _dispoKits += $"<color={_color}>➤ {kit.Key} - Usos restantes del día: {_leftUses}</color>\n";
            }
            if (_dispoKits.IsEmpty())
                _dispoKits = "- No hay KITS disponibles -";
            string ErrResponse = $" <color=red>Kit inexistente o falta de argumentos, por favor eliga uno de los siguientes kits disponibles:</color>\n<color=green>{_dispoKits}</color>";

            try
            {
                _kit = arguments.At(0);
                string str = _kit;
                _kit = char.ToUpper(str[0]) + str.Substring(1).ToLower();
                if (!_playerData.KitsUses.ContainsKey(_kit))
                {
                    response = ErrResponse;
                    return false;
                }
                _playerData.KitsUses.TryGetValue(_kit, out int _kitLeftUses);
                if(_kitLeftUses <= 0)
                {
                    response = $"<color=red>No tienes usos restantes para el Kit </color><color=orange>{_kit}</color><color=red> por favor use el comando '.kit' y revise cuales tiene disponible.</color>";
                    return false;
                }
                if(Kits.EventHandler.RoundKitUses.ContainsKey(_player.UserId) && Kits.EventHandler.RoundKitUses[_player.UserId].Contains(_kit))
                {
                    response = $"<color=red>Ya has usado el kit </color><color=orange>{_kit}</color><color=red> en esta ronda, por favor use el comando '.kit' y revise cuales tiene disponible.</color>";
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
                if (Kits.EventHandler.RoundKitUses.ContainsKey(_player.UserId))
                {
                    Kits.EventHandler.RoundKitUses[_player.UserId].Add(_kit);
                }
                else
                {
                    Kits.EventHandler.RoundKitUses.Add(_player.UserId, new List<string>() { { _kit} });
                }
                Extension.SubstractUses(_player, _kit);
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
