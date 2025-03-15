using Config;
using Sdk;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheEntity;
using UnityEngine;
using UniverseLib.UI;
using ZGScriptTrainer.FrendSet;
using ZGScriptTrainer.Load;
using ZGScriptTrainer.SaveAndLoad;
using ZGScriptTrainer.UI;
using ZGScriptTrainer.UI.Models;
using ZGScriptTrainer.UI.Panels;

namespace ZGScriptTrainer.FriendSet
{
    public static class FriendSet
    {
        public const string DefaultSearchText = "请输入人物名称";
        public static Dictionary<int, List<ForFriend>> BaseFriend { get; private set; } = new Dictionary<int, List<ForFriend>>();

        #region[每个游戏都需要修改]
        //人物种类
        public static readonly List<string> FriendTypeDic = new List<string>()
        {
            "全部"
        };
        //获取所有人物

        public static Sprite FriendSprite;
        public static int FriendIconid;
        public static Dictionary<int, PersonCfg> GetFriend;
        public static Dictionary<int, RelationCfg> GetRelation;
        public static Dictionary<int, PersonCfg> AllCanTalkFriend;
        public static int AddCount = 1;
        public static bool IsAllFriendShowInMap = false;
        public static List<int> AllFriend = new List<int>();
        public static bool Bonce = false;
        public static int i = 0;

        public static bool SaveFriendSuccess= false;
        public static bool SaveFriendSuccessBonce = false;
        private static readonly string SaveFriendPath = "BepInEx\\plugins\\TheSave\\Friend";
        public static Dictionary<int, PersonCfg> LoadFriend;

        public static bool SaveFriendRelationSuccess = false;
        public static bool SaveFriendRelationSuccessBonce = false;
        private static readonly string SaveFriendRelationPath = "BepInEx\\plugins\\TheSave\\FriendRelation";
        public static Dictionary<int, RelationCfg> LoadFriendRelation;
        public static Dictionary<int, string> RelationIDToName = new Dictionary<int, string>();
        public static void GetBaseFriendData()
        {
            BaseFriend.Clear();

            for (int i = 0; i < FriendTypeDic.Count; i++)
            {
                BaseFriend.Add(i, new List<ForFriend>());
            }
            i++;
            SaveAndLoadNeed Save = new SaveAndLoadNeed();
            if (!SaveFriendSuccess &&i>50)
            {
                LoadFile.LoadAsync<PersonCfg>("Cfgs/" + LocalizationMgr.Lang + "/PersonCfg", delegate (Dictionary<int, PersonCfg> _t)
                {
                    GetFriend = _t;
                    foreach (var Friend in GetFriend)
                    {
                        if (GetFriend.ContainsKey(Friend.Key))
                            SaveFriendSuccess = true;
                    }
                });
            }
            if (SaveFriendSuccess && !SaveFriendSuccessBonce)
            {
                Save.SaveAllFriendToFile<PersonCfg>(GetFriend, SaveFriendPath, "Friend");
                SaveFriendSuccessBonce = true;
            }
            if (SaveFriendSuccessBonce && LoadFriend == null)
            LoadFriend = Save.ReadAllFriendFromFile<PersonCfg>(SaveFriendPath);


            if (!SaveFriendRelationSuccess && i > 50)
            {
                LoadFile.LoadAsync<RelationCfg>("Cfgs/" + LocalizationMgr.Lang + "/RelationCfg", delegate (Dictionary<int, RelationCfg> _t)
                {
                    GetRelation = _t;
                    foreach (var Friend in GetRelation)
                    {
                        if (GetRelation.ContainsKey(Friend.Key))
                            SaveFriendRelationSuccess = true;
                        RelationIDToName[Friend.Value.id] = Friend.Value.name;
                    }
                });
            }
            if (SaveFriendRelationSuccess && !SaveFriendRelationSuccessBonce)
            {
                Save.SaveAllFriendToFile<RelationCfg>(GetRelation, SaveFriendRelationPath, "FriendRelation");
                SaveFriendRelationSuccessBonce = true;
            }
            if (SaveFriendRelationSuccessBonce && LoadFriendRelation == null)
                LoadFriendRelation = Save.ReadAllFriendFromFile<RelationCfg>(SaveFriendRelationPath);



            if (LoadFriend != null)
            {
                //获取人物对应数据
                foreach (var kvp in LoadFriend.Values)
                {
                    //空白图片不要
                    if (!kvp.url.IsEmpty<string>() && !kvp.url2.IsEmpty<string>() && LoadSprite.GetSprite(kvp.GetComicIcon()))
                    {
                        BaseFriend[0].Add(new ForFriend() { FriendID = kvp.id, FriendName = kvp.name, FriendIcon = LoadSprite.GetSprite(kvp.GetComicIcon()) });
                        if (IsAllFriendShowInMap && kvp.id != 1 && kvp.id != 2)
                        {
                            Singleton<FuncMgr>.Ins.GetMapData().UpdateNpcMap(kvp.id);
                        }
                        if (kvp.id !=0 && kvp.name != "妈妈" && kvp.name != "爸爸" && !AllFriend.Contains(kvp.id))
                        {
                            AllFriend.Add(kvp.id);
                        }
                    
                        Singleton<RoleMgr>.Ins.GetRelationData(true).CanFocusNPC();
                    }
                };
            }

            if (!Bonce && i>100)
            {
                Bonce = true;
                SaveAndLoad.SaveFriendAndLoadFrined.SaveAllFriendToFile(AllFriend);
            }

            var list = new List<ForFriend>();
            for (int i = 0; i < BaseFriend.Count; i++)
            {
                list.AddRange(BaseFriend[i]);
            }
            BaseFriend[0] = list;
        }
        //获取人物名称
        public static string GetFriendName(this ForFriend Friend)
        {
            var tmp = Friend.FriendName;
            return tmp != null ? Friend.FriendName : "";
        }
        //获取人物图标
        public static Sprite GetFriendIcon(this ForFriend Friend)
        {
            var tmp = Friend.FriendIcon;
            return tmp != null ? tmp : null;
        }
        //获取人物解释
        public static string GetFriendDescription(this ForFriend Friend)
        {
            return Friend.FriendDes;
        }
        //添加好感
        public static void AddFavor(this ForFriend Friend)
        {
                Singleton<RoleMgr>.Ins.GetRole(Friend.FriendID).Relation = 1;
                Singleton<RoleMgr>.Ins.AddRoleFavor(Friend.FriendID,AddCount);
                ZGScriptTrainer.WriteLog($"添加好感人物ID：{Friend.FriendID} 名称：{Friend.FriendName}");
        }
        public static void DelFavor(this ForFriend Friend)
        {
            Singleton<RoleMgr>.Ins.GetRole(Friend.FriendID).Relation = 1;
            Singleton<RoleMgr>.Ins.AddRoleFavor(Friend.FriendID,- AddCount);
            ZGScriptTrainer.WriteLog($"移除好感人物ID：{Friend.FriendID} 名称：{Friend.FriendName}");

        }
        #endregion


        public static List<ForFriend> GetTypeFriends(int FriendType = -1)
        {
            List<ForFriend> result = new List<ForFriend>();
            if (FriendType < 0)
            {
                if (BaseFriend.Count > 0)
                result.AddRange(BaseFriend[0]);
            }
            else
            {
                BaseFriend.TryGetValue(FriendType, out result);
            }
            return result ?? new List<ForFriend>();
        }
        public static List<ForFriend> FilterFriendData(this string text, int type = -1)
        {
            if (!string.IsNullOrEmpty(text) && text != DefaultSearchText)
            {
                List<ForFriend> list = new List<ForFriend>();

                foreach (var Friend in GetTypeFriends(type))
                {
                    string tmp1 = GetFriendName(Friend);
                    string tmp2 = GetFriendDescription(Friend);
                    if (tmp1 != null && tmp1.Contains(text.Replace(" ", "")))
                    {
                        list.Add(Friend);
                    }
                    else if (tmp2 != null && tmp2.Contains(text.Replace(" ", "")))
                    {
                        list.Add(Friend);
                    }
                }
                return list;
            }
            else { return GetTypeFriends(type); }
        }
    }
}
