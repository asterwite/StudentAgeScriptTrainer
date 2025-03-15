using Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.U2D;
using UnityEngine;
using ZGScriptTrainer.Load;
using HarmonyLib;
using View.Main;
using Config;
using TheEntity;
using ZGScriptTrainer.SaveAndLoad;
using View.Common;

namespace ZGScriptTrainer.Cheat
{
    internal class MapFriendCheat
    {
        public static bool 显示全部人物 = false;
        public static bool 所有人都可互动 = false;
        private static List<int> AllFirend;
        [HarmonyPrefix]
        [HarmonyPatch(typeof(DetailSocialView), nameof(DetailSocialView.RefreshRoleList))]
        public static bool ShowAll(DetailSocialView __instance)
        {
            if (显示全部人物)
            {
                if(AllFirend==null)
                AllFirend = SaveFriendAndLoadFrined.ReadAllFriendFromFile();  

                List<Role> list = new List<Role>();
                foreach (int num in AllFirend)
                {
                    Role role = Singleton<RoleMgr>.Ins.GetRole(num);
                    if (role != null)
                    {
                        list.Add(role);
                    }
                }
                list.Sort(delegate (Role a, Role b)
                {
                    if (a.Relation != b.Relation)
                    {
                        return b.Relation - a.Relation;
                    }
                    if (a.Favor == b.Favor)
                    {
                        int order = Cfg.PersonGrowCfgMap[a.id].order;
                        int order2 = Cfg.PersonGrowCfgMap[b.id].order;
                        if (order > order2)
                        {
                            return 1;
                        }
                        if (order < order2)
                        {
                            return -1;
                        }
                        return 0;
                    }
                    else
                    {
                        if (a.Favor > b.Favor)
                        {
                            return -1;
                        }
                        return 1;
                    }
                });
                __instance.itemgroup_roles.SetUICellDatas<Role>(list, delegate (Role _role)
                {
                    if (_role.Relation == 0)
                    {
                        return 1;
                    }
                    return 0;
                });
                __instance.roles.SetSizeY((float)((list.Count <= 999) ? 1303 : 1443));
                __instance.voices = RoleMgr.GetNpcVoices(__instance.role.id, NpcVoiceTypeDefine.Talk);
                __instance.btn_voice.gameObject.SetActive(Singleton<RoleMgr>.Ins.GetRole().GradeState > 0 && __instance.voices.NotEmpty<int>());
                return false;
            }
            return true;
        }
        [HarmonyPrefix]
        [HarmonyPatch(typeof(RoleMgr), nameof(RoleMgr.GetRole), new[] { typeof(int) })]
        public static bool GetRole(RoleMgr __instance, int _roleId, ref Role __result)
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

    }
}
