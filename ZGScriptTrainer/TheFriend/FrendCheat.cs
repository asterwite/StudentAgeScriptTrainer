using Config;
using Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UniverseLib.UI;
using ZGScriptTrainer.Cheat;
using ZGScriptTrainer.Load;
using ZGScriptTrainer.MainUI;
using ZGScriptTrainer.UI;
namespace ZGScriptTrainer.TheFriend
{
    internal class FriendCheat
    {
        private static Dropdown ItemTypeDown;
        private static Dropdown FriendRelationTypeDown;
        private static Dropdown FriendGiftTypeDown;
        private static List<int> FriendIDList = new List<int>();

        private static List<string> itemDes1Name = new List<string>();
        public static int FriendID;

        private static int lastSearchIndex = -1;
        private static string itemDes1;

        private static int lastSelectedIndex = 0;
        private static MethodInfo showMethod;
        private static bool isDropdownOpen = false;
        public static string itemName;
        public static void OnDropdownItemValueChanged(int index, GameObject tabGroup)
        {
            FriendUI.RemoveCurrentItemDesControls();
            // 获取当前选中的选项文本
            if (index >= 0 && index < FriendIDList.Count)
            {
                FriendID = FriendIDList[index];
                itemName = ItemTypeDown.options[index].text;
                // 在这里处理选中的 id
                Debug.Log("选中的人物 ID: " + FriendID + "\n人物名称：" + itemName);
            }
            IntroduceFriend(tabGroup);
        }
        public static void IntroduceFriend(GameObject tabGroup)
        {
            GameObject FriendCheat = UIFactory.CreateHorizontalGroup(tabGroup, "SpawnitemFriend", false, false, true, true, 5, default,
            new Color(0.065f, 0.065f, 0.065f), TextAnchor.MiddleLeft);
            UIFactory.SetLayoutElement(FriendCheat, minHeight: 40, flexibleWidth: 9999);

            UIFactory.CreateLabel(FriendCheat, "Lable", "选择人物关系：", fontSize: ZGScriptTrainer.FontSize.Value).SetLayoutElement(20, 40);
            GameObject gameObject = UIFactory.CreateDropdown(FriendCheat, "MainTypeDown", out FriendRelationTypeDown, "选择关系", ZGScriptTrainer.FontSize.Value, null, null);
            foreach (var kvp in RelationSet.Values)
            {
                FriendRelationTypeDown.options.Add(new Dropdown.OptionData(kvp.name));
                FriendDropDownSet.FriendRelationidList.Add(kvp.id);
            }
            UIFactory.SetLayoutElement(gameObject, minHeight: 40, minWidth: 550);
            FriendRelationTypeDown.captionText.fontSize = ZGScriptTrainer.FontSize.Value;
            FriendCheat.CreateButton("设置", () =>
            {
                if (Singleton<RoleMgr>.Ins.GetRole(FriendID) != null)
                {
                    Singleton<RoleMgr>.Ins.GetRelationData().ChangeRelation(FriendID, FriendDropDownSet.FriendRelationID);
                    Singleton<RoleMgr>.Ins.GetLoveData().breakfastId = 1;
                }
            }).SetLayoutElement(20, 40);

            FriendRelationTypeDown.onValueChanged.AddListener((index) => FriendDropDownSet.DropDownFriendRelationChange(index, tabGroup, FriendRelationTypeDown));
            FriendUI.CurrentItemDesControls.Add(FriendCheat);
        }

        public static Dictionary<int, PersonCfg> NpcSet;
        public static Dictionary<int, RelationCfg> RelationSet;
        public static void DrawFriendCheatUI(GameObject tabGroup)
        {

            GameObject SpawnItem = UIFactory.CreateHorizontalGroup(tabGroup, "MainWindowItem1", false, false, true, true, 5, default,
            new Color(0.065f, 0.065f, 0.065f), TextAnchor.MiddleLeft);
            UIFactory.SetLayoutElement(SpawnItem, minHeight: 40, flexibleWidth: 9999);
            SpawnItem.CreateInputEditSpawnFindItemButton("搜索", "输入人物名称", "搜索", new Action<string>((text) =>
            {
                OnSearchInputChanged(text);
            }));
            FriendUI.currentPageControls.Add(SpawnItem);

            GameObject Item = UIFactory.CreateHorizontalGroup(tabGroup, "MainWindowItem", false, false, true, true, 5, default,
            new Color(0.065f, 0.065f, 0.065f), TextAnchor.MiddleLeft);
            UIFactory.SetLayoutElement(Item, minHeight: 40, flexibleWidth: 9999);
            GameObject gameObject = UIFactory.CreateDropdown(Item, "MainTypeDown", out ItemTypeDown, "选择人物", ZGScriptTrainer.FontSize.Value, null, null);
            foreach (var kvp in NpcSet.Values)
            {
                ItemTypeDown.options.Add(new Dropdown.OptionData(kvp.name));
                FriendIDList.Add(kvp.id);
            }
            UIFactory.SetLayoutElement(gameObject, minHeight: 40, minWidth: 550);
            ItemTypeDown.captionText.fontSize = ZGScriptTrainer.FontSize.Value;

            //设置下拉栏滚动速度
            SetDropdownScrollSpeed(ItemTypeDown, 10f);
            Item.CreateInputEditButton("好感：", "10", "添加", new Action<string>((text) =>
            {
                var Add = text.ConvertToIntDef(10);
                if (Singleton<RoleMgr>.Ins.GetRole() != null)
                {
                    Singleton<RoleMgr>.Ins.GetRole(FriendID).Relation = 1;
                    Singleton<RoleMgr>.Ins.GetRole(FriendID).UpdateFavor(Add);
                }

            }));
            IntroduceFriend(tabGroup);
            ItemTypeDown.onValueChanged.AddListener((index) => OnDropdownItemValueChanged(index, tabGroup));

            FriendUI.currentPageControls.Add(Item);
            FriendID = FriendIDList[0];
            FriendDropDownSet.FriendRelationID = FriendDropDownSet.FriendRelationidList[0];
        }
        private static void OnSearchInputChanged(string searchText)
        {
            int startIndex = lastSearchIndex + 1; // 从上次匹配的下一个索引开始查找
            int nextMatchIndex = -1;

            for (int i = startIndex; i < ItemTypeDown.options.Count; i++)
            {
                if (ItemTypeDown.options[i].text.Contains(searchText))
                {
                    nextMatchIndex = i;
                    break;
                }
            }

            if (nextMatchIndex != -1)
            {
                ItemTypeDown.value = nextMatchIndex;
                ItemTypeDown.RefreshShownValue();
                lastSearchIndex = nextMatchIndex; // 更新上次匹配的索引
            }
            else
            {
                // 如果没有找到，从开头重新开始查找
                for (int i = 0; i < startIndex; i++)
                {
                    if (ItemTypeDown.options[i].text.Contains(searchText))
                    {
                        nextMatchIndex = i;
                        ItemTypeDown.value = nextMatchIndex;
                        ItemTypeDown.RefreshShownValue();
                        lastSearchIndex = nextMatchIndex;
                        break;
                    }
                }
            }
        }
        public static void SetDropdownScrollSpeed(Dropdown dropdown, float scrollSpeed)
        {
            if (dropdown == null)
            {
                Debug.LogError("Dropdown is null.");
                return;
            }

            // 获取Dropdown的模板对象，模板对象包含ScrollRect组件
            Transform dropdownTemplate = dropdown.template;
            if (dropdownTemplate == null)
            {
                Debug.LogError("Dropdown template is null.");
                return;
            }
            // 获取ScrollRect组件
            ScrollRect scrollRect = dropdownTemplate.GetComponent<ScrollRect>();
            if (scrollRect == null)
            {
                Debug.LogError("ScrollRect component not found on dropdown template.");
                return;
            }
            // 设置滚动速度
            scrollRect.scrollSensitivity = scrollSpeed;
        }
    }
}
