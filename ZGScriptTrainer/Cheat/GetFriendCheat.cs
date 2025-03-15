using Config;
using HarmonyLib;
using Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheEntity;
using UnityEngine;
using ZGScriptTrainer.FrendSet;
using ZGScriptTrainer.FriendSet;
using ZGScriptTrainer.Load;
using ZGScriptTrainer.MainUI;

namespace ZGScriptTrainer.Cheat
{
    internal class GetFriendCheat
    {
        public static bool 社交无限制 = false;
        [HarmonyPrefix]
        [HarmonyPatch(typeof(BagMgr), nameof(BagMgr.AddGiftCntThisRound))]
        public static bool NoAddGiftCntThisRound(Role __instance)
        {
            if (BasicUI.赠送礼物无限制)
                return false;
            return true;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(RoleMgr), nameof(RoleMgr.GetRole), new[] { typeof(int) })]
        public static bool Prefix(int _roleId, ref Role __result)
        {
            if (社交无限制)
            {
                PersonCfg personCfg;
                if (!Cfg.PersonCfgMap.TryGetValue(_roleId, out personCfg))
                {
                    return false;
                }
                Role role;
                if (Singleton<RoleMgr>.Ins.model.roleDict.ContainsKey(_roleId))
                {
                    role = Singleton<RoleMgr>.Ins.model.roleDict[_roleId];
                }
                else
                {
                    role = new Role();
                    Singleton<RoleMgr>.Ins.model.roleDict.Add(_roleId, role);
                    role.Load(_roleId, false, null, null, true, null, null, null);
                }
                __result = role;
                return false;
            }          
            return true;
        }
    }
}
