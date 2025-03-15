using Config;
using Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UniverseLib.UI;
using ZGScriptTrainer.ItemSpawn;
using ZGScriptTrainer.Load;
using ZGScriptTrainer.SaveAndLoad;
using ZGScriptTrainer.UI;
using ZGScriptTrainer.UI.Models;
using ZGScriptTrainer.UI.Panels;

namespace ZGScriptTrainer.ItemSpawn
{
    public static class ItemSet
    {
        public const string DefaultSearchText = "请输入物品名称";
        public static Dictionary<int, List<ForItem>> BaseItems { get; private set; } = new Dictionary<int, List<ForItem>>();

        #region[每个游戏都需要修改]
        //物品种类
        public static readonly List<string> ItemTypeDic = new List<string>()
        {
            "物品",
            "书籍",
            "收藏",
        };
        //获取所有物品

        public static Sprite ItemSprite;
        public static int ItemIconid;
        public static Dictionary<int, Sprite> itemSpriteMap = new Dictionary<int, Sprite>();
        public static HashSet<int> ItemIDAnother = new HashSet<int>();
        public static int Page = 0;
        public static Dictionary<int, ItemCfg> GetItem;
        public static Dictionary<int, BookCfg> GetBook;
        public static Dictionary<int, ShellCfg> GetShell;
        public static int SpawnCount = 1;
        public static int i= 0;

        public static bool SaveItemSuccess = false;
        public static bool SaveItemSuccessBonce = false;
        private static readonly string SaveItemPath = "BepInEx\\plugins\\TheSave\\Item";
        public static Dictionary<int, ItemCfg> LoadItem;

        public static bool SaveBookSuccess = false;
        public static bool SaveBookSuccessBonce = false;
        private static readonly string SaveBookPath = "BepInEx\\plugins\\TheSave\\Book";
        public static Dictionary<int, BookCfg> LoadBook;

        public static bool SaveShellSuccess = false;
        public static bool SaveShellSuccessBonce = false;
        private static readonly string SaveShellPath = "BepInEx\\plugins\\TheSave\\Shell";
        public static Dictionary<int, ShellCfg> LoadShell;
        public static void GetBaseItemData()
        {
            BaseItems.Clear();

            for (int i = 0; i < ItemTypeDic.Count; i++)
            {
                BaseItems.Add(i, new List<ForItem>());
            }
            i++;

            SaveAndLoadNeed Save = new SaveAndLoadNeed();
            //物品
            if (!SaveItemSuccess && i > 50)
            {
                LoadFile.LoadAsync("Cfgs/" + LocalizationMgr.Lang + "/ItemCfg", delegate (Dictionary<int, ItemCfg> _t)
                {
                    GetItem = _t;
                    foreach (var Friend in GetItem)
                    {
                        if (GetItem.ContainsKey(Friend.Key))
                            SaveItemSuccess = true;
                    }
                });
              
            }
            if (SaveItemSuccess && !SaveItemSuccessBonce)
            {
                Save.SaveAllFriendToFile<ItemCfg>(GetItem, SaveItemPath, "Item");
                SaveItemSuccessBonce = true;
            }
            if (SaveItemSuccessBonce && LoadItem == null)
                LoadItem = Save.ReadAllFriendFromFile<ItemCfg>(SaveItemPath);

            //书籍
            if (!SaveBookSuccess && i > 50)
            {
                LoadFile.LoadAsync<BookCfg>("Cfgs/" + LocalizationMgr.Lang + "/BookCfg", delegate (Dictionary<int, BookCfg> _t)
                {
                    GetBook = _t;
                    foreach (var Friend in GetBook)
                    {
                        if (GetBook.ContainsKey(Friend.Key))
                            SaveBookSuccess = true;
                    }
                });

            }
            if (SaveBookSuccess && !SaveBookSuccessBonce)
            {
                Save.SaveAllFriendToFile<BookCfg>(GetBook, SaveBookPath, "Book");
                SaveBookSuccessBonce = true;
            }
            if (SaveBookSuccessBonce && LoadBook == null)
                LoadBook = Save.ReadAllFriendFromFile<BookCfg>(SaveBookPath);
            //收藏
            if (!SaveShellSuccess && i > 50)
            {
                LoadFile.LoadAsync<ShellCfg>("Cfgs/" + LocalizationMgr.Lang + "/ShellCfg", delegate (Dictionary<int, ShellCfg> _t)
                {
                    GetShell = _t;
                    foreach (var Friend in GetShell)
                    {
                        if (GetShell.ContainsKey(Friend.Key))
                            SaveShellSuccess = true;
                    }
                });

            }
            if (SaveShellSuccess && !SaveShellSuccessBonce)
            {
                Save.SaveAllFriendToFile<ShellCfg>(GetShell, SaveShellPath, "Shell");
                SaveShellSuccessBonce = true;
            }
            if (SaveShellSuccessBonce && LoadShell == null)
            {
                LoadShell = Save.ReadAllFriendFromFile<ShellCfg>(SaveShellPath);
                Console.WriteLine("物品数据加载完成，你现在可以物品添加的修改内容了");
            }
              
            ////获取物品对应数据
            if (LoadItem != null)
            {
                foreach (var kvp in LoadItem.Values)
                {
                     BaseItems[0].Add(new ForItem() { ItemID = kvp.id, ItemName = kvp.name, ItemDes = kvp.desc, ItemIcon = LoadSprite.GetSprite(kvp.GetItemIcon()) });             
                };
            }
            if (LoadBook != null)
            {
                foreach (var kvp in GetBook.Values)
                {
                        BaseItems[1].Add(new ForItem() { ItemID = kvp.id, ItemName = kvp.name, ItemDes = kvp.desc, ItemIcon = LoadSprite.GetSprite(kvp.GetBookIcon()) });
                };
            }          
            if (LoadShell != null)
            {
                foreach (var kvp in GetShell.Values)
                {
                      BaseItems[2].Add(new ForItem() { ItemID = kvp.id, ItemName = kvp.name, ItemDes = kvp.desc, ItemIcon = LoadSprite.GetSprite(kvp.icon) });
                };
            }
           
            var list = new List<ForItem>();
            for (int i = 0; i < BaseItems.Count; i++)
            {
                list.AddRange(BaseItems[i]);
            }
            //BaseItems[0] = list;
        }
        //获取物品名称
        public static string GetItemName(this ForItem item)
        {
            var tmp = item.ItemName;
            return tmp != null ? item.ItemName : "";
        }
        //获取物品图标
        public static Sprite GetItemIcon(this ForItem item)
        {
            var tmp = item.ItemIcon;
            return tmp != null ? tmp : null;
        }
        //获取物品解释
        public static string GetItemDescription(this ForItem item)
        {
            return item.ItemDes;
        }
        //添加物品
        public static void SpwanItem(this ForItem Item)
        {

            if (Page == 0)
            {
                Singleton<BagMgr>.ins.AddItem(Item.ItemID, SpawnCount);
                var count = Singleton<BagMgr>.ins.GetItemCnt(Item.ItemID);
                ZGScriptTrainer.WriteLog($"添加物品ID：{Item.ItemID} 名称：{Item.ItemName} 当前物品数量：{count}");
            }
            if (Page == 1)
            {
                Singleton<BagMgr>.ins.AddBook(Item.ItemID);
                var count = Singleton<BagMgr>.ins.GetItemCnt(Item.ItemID);
                ZGScriptTrainer.WriteLog($"添加书本ID：{Item.ItemID} 名称：{Item.ItemName} 当前书本数量：{count}");
            }
            if (Page == 2)
            {
                Singleton<BagMgr>.ins.AddShell(Item.ItemID, SpawnCount);
                HintHelper.ShowShell(Item.ItemID, DescCtrl.GetTxt<string>(726, new string[]
                {
                    Utils.Number2Chinese("1"),
                    Cfg.ShellCfgMap[Item.ItemID].name
                }), null);
                var count = Singleton<BagMgr>.ins.GetShellCnt(Item.ItemID);
                ZGScriptTrainer.WriteLog($"添加收藏ID：{Item.ItemID} 名称：{Item.ItemName} 当前收藏数量：{count}");
            }

        }
        //移除物品
        public static void DelItem(this ForItem Item)
        {
            if (Page == 0)
            {
                Singleton<BagMgr>.ins.DelItem(Item.ItemID, SpawnCount);
                var count = Singleton<BagMgr>.ins.GetItemCnt(Item.ItemID);
                ZGScriptTrainer.WriteLog($"移除物品ID：{Item.ItemID} 名称：{Item.ItemName} 当前物品剩余数量：{count}");
            }

            if (Page == 1)
            {
                Singleton<BagMgr>.ins.DelBook(Item.ItemID, SpawnCount);
                var count = Singleton<BagMgr>.ins.GetItemCnt(Item.ItemID);
                ZGScriptTrainer.WriteLog($"移除书本ID：{Item.ItemID} 名称：{Item.ItemName} 当前书本剩余数量：{count}");
            }
            //if (Page == 2)
            //{
            //    for (int i = 0; i < SpawnCount; i++)
            //    {
            //        Singleton<BagMgr>.ins.RemoveShells(Item.ItemID);
            //    }
            //    var count = Singleton<BagMgr>.ins.GetShellCnt(Item.ItemID);
            //    ZGScriptTrainer.WriteLog($"移除收藏ID：{Item.ItemID} 名称：{Item.ItemName} 当前收藏剩余数量：{count}");
            //}
        }
        #endregion


        public static List<ForItem> GetTypeItems(int ItemType = -1)
        {
            List<ForItem> result = new List<ForItem>();
            if (ItemType < 0)
            {
                if (BaseItems.Count > 0)
                result.AddRange(BaseItems[0]);
            }
            else
            {
                BaseItems.TryGetValue(ItemType, out result);
            }
            return result ?? new List<ForItem>();
        }
        public static List<ForItem> FilterItemData(this string text, int type = -1)
        {
            if (!string.IsNullOrEmpty(text) && text != DefaultSearchText)
            {
                List<ForItem> list = new List<ForItem>();

                foreach (var item in GetTypeItems(type))
                {
                    string tmp1 = GetItemName(item);
                    string tmp2 = GetItemDescription(item);
                    if (tmp1 != null && tmp1.Contains(text.Replace(" ", "")))
                    {
                        list.Add(item);
                    }
                    else if (tmp2 != null && tmp2.Contains(text.Replace(" ", "")))
                    {
                        list.Add(item);
                    }
                }
                return list;
            }
            else { return GetTypeItems(type); }
        }
    }
}
