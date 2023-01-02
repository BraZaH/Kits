namespace Kits.Database
{
    using System;
    using System.Collections.Generic;
    using Exiled.API.Features;
    using Kits.Features;
    public static class Extension
    {
        public static void AddPlayer(Player ply)
        {
            Dictionary<string, int> _avalibleKits = new Dictionary<string, int>();
            foreach (var _kits in Main.Singleton.Config.Kits)
            {
                if (_kits.Value.UserGroup.ContainsKey(ply.GroupName) || _kits.Value.UserGroup.ContainsKey("none"))
                {
                    string _referenceUserGroup = string.Empty;
                    if (_kits.Value.UserGroup.ContainsKey(ply.GroupName))
                        _referenceUserGroup = ply.GroupName;
                    else
                        _referenceUserGroup = "none";
                    _kits.Value.UserGroup.TryGetValue(_referenceUserGroup, out int _kitUses);
                    _avalibleKits.Add(_kits.Key,_kitUses);
                }
            }
            try
            {
                PlayerData pd = new PlayerData()
                {
                    UserID = ply.UserId,
                    GroupName = ply.GroupName,
                    KitsUses = _avalibleKits
                };
                Database.LiteDatabase.GetCollection<PlayerData>().Insert(pd);
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }

        public static void CheckPlayer(Player ply)
        {
            if (!Database.LiteDatabase.GetCollection<PlayerData>().Exists(e => e.UserID == ply.UserId))
            {
                AddPlayer(ply);
                return;
            }
            PlayerData _player = Database.LiteDatabase.GetCollection<PlayerData>().FindOne(e => e.UserID == ply.UserId);
            if(_player.GroupName != ply.GroupName)
            {
                Database.LiteDatabase.GetCollection<PlayerData>().Delete(_player.UserID);
                AddPlayer(ply);
                return;
            }
            bool HasChanges = false;
            Dictionary<string, int> _kitsUses = _player.KitsUses;
            foreach (var _kits in Main.Singleton.Config.Kits)
            {
                if (!_player.KitsUses.ContainsKey(_kits.Key))
                {
                    if(_kits.Value.UserGroup.ContainsKey(ply.GroupName) || _kits.Value.UserGroup.ContainsKey("none"))
                    {
                        string _referenceGroupName = ply.GroupName;
                        if (!_kits.Value.UserGroup.ContainsKey(_referenceGroupName))
                            _referenceGroupName = "none";
                        _kits.Value.UserGroup.TryGetValue(_referenceGroupName, out int _uses);
                        _kitsUses.Add(_kits.Key, _uses);
                        HasChanges = true;
                    }
                }
            }
            if (HasChanges)
            {
                PlayerData pd = new PlayerData()
                {
                    UserID = _player.UserID,
                    GroupName = _player.GroupName,
                    KitsUses = _kitsUses
                };
                Database.LiteDatabase.GetCollection<PlayerData>().Update(pd);
            }

        }

        public static void RestartUses()
        {
            var _players = Database.LiteDatabase.GetCollection<PlayerData>().FindAll();
            foreach (PlayerData player in _players)
            {
                Dictionary<string, int> _freshKitUses = new Dictionary<string, int>();
                foreach (var _kits in player.KitsUses)
                {
                    Main.Singleton.Config.Kits.TryGetValue(_kits.Key, out ItemKit _kitValue);

                    string _referenceUserGroup = string.Empty;
                    if (_kitValue.UserGroup.ContainsKey(player.GroupName))
                        _referenceUserGroup = player.GroupName;
                    else
                        _referenceUserGroup = "none";

                    _kitValue.UserGroup.TryGetValue(_referenceUserGroup, out int _kitFreshValue);
                    _freshKitUses.Add(_kits.Key, _kitFreshValue);
                }
                PlayerData pd = new PlayerData()
                {
                    UserID = player.UserID,
                    GroupName = player.GroupName,
                    KitsUses = _freshKitUses
                };
                Database.LiteDatabase.GetCollection<PlayerData>().Update(pd);
            }
        }

        public static void SubstractUses(Player ply, string Kit)
        {
            PlayerData _player = Database.LiteDatabase.GetCollection<PlayerData>().FindOne(e => e.UserID == ply.UserId);
            Dictionary<string, int> _kitUses = new Dictionary<string, int>();
            foreach (var item in _player.KitsUses)
            {
                if(item.Key == Kit)
                {
                    _kitUses.Add(item.Key, item.Value - 1);
                }
                else
                {
                    _kitUses.Add(item.Key, item.Value);
                }
            }
            PlayerData pd = new PlayerData()
            {
                UserID = _player.UserID,
                GroupName = _player.GroupName,
                KitsUses = _kitUses
            };
            Database.LiteDatabase.GetCollection<PlayerData>().Update(pd);
        }
    }
}
