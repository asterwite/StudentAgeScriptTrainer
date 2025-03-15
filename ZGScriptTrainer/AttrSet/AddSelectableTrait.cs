using Config;
using Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.UI;
using UnityEngine;
using UniverseLib.UI;
using ZGScriptTrainer.UI;

namespace ZGScriptTrainer.AttrSet
{
    internal class AddState
    {
        private static Dropdown ItemTypeDown;
        private static List<int> ItemidList = new List<int>();
        private static List<string> itemDes1Name = new List<string>();
        public static int ItemID;
        private static int lastSearchIndex = -1;
        private static string itemDes1;

        private static int lastSelectedIndex = 0;
        private static MethodInfo showMethod;
        private static bool isDropdownOpen = false;
        public static void OnDropdownItemValueChanged(int index, GameObject tabGroup)
        {
            AttrUI.RemoveCurrentItemDesControls();
            // 获取当前选中的选项文本
            if (index >= 0 && index < ItemidList.Count)
            {
                ItemID = ItemidList[index];
                string StateName = ItemTypeDown.options[index].text;
                // 在这里处理选中的 id
                //itemDes1 = itemDes1Name[index];
                Debug.Log("选中的特征 ID: " + ItemID + "\n特征名称：" + StateName + "\n特征介绍：" + itemDes1);
            }
            IntroduceState(tabGroup);
        }
        public static void IntroduceState(GameObject tabGroup)
        {

            GameObject Item2 = UIFactory.CreateHorizontalGroup(tabGroup, "SpawnitemWindow2", false, false, true, true, 5, default,
            new Color(0.065f, 0.065f, 0.065f), TextAnchor.MiddleLeft);
            UIFactory.SetLayoutElement(Item2, minHeight: 40, flexibleWidth: 9999);
            UIFactory.CreateLabel(Item2, "SpawnitemLabel", "简介：" + itemDes1, TextAnchor.MiddleLeft, default, true, ZGScriptTrainer.FontSize.Value);
            AttrUI.CurrentItemDesControls.Add(Item2);
        }
        public static Dictionary<int, PersonStateCfg> TraitsSet;
        public static void DrawAddStateUI(GameObject tabGroup)
        {

            GameObject SpawnItem = UIFactory.CreateHorizontalGroup(tabGroup, "MainWindowItem1", false, false, true, true, 5, default,
            new Color(0.065f, 0.065f, 0.065f), TextAnchor.MiddleLeft);
            UIFactory.SetLayoutElement(SpawnItem, minHeight: 40, flexibleWidth: 9999);
            SpawnItem.CreateInputEditSpawnFindItemButton("搜索特征", "输入名称", "搜索", new Action<string>((text) =>
            {
                OnSearchInputChanged(text);
            }));
            AttrUI.currentPageControls.Add(SpawnItem);


            GameObject Item = UIFactory.CreateHorizontalGroup(tabGroup, "MainWindowItem", false, false, true, true, 5, default,
            new Color(0.065f, 0.065f, 0.065f), TextAnchor.MiddleLeft);
            UIFactory.SetLayoutElement(Item, minHeight: 40, flexibleWidth: 9999);
            GameObject gameObject = UIFactory.CreateDropdown(Item, "MainTypeDown", out ItemTypeDown, "选择特征", ZGScriptTrainer.FontSize.Value, null, null);
            foreach (var kvp in TraitsSet.Values)
            {
                ItemTypeDown.options.Add(new Dropdown.OptionData(kvp.name));
                ItemidList.Add(kvp.id);
            }
            UIFactory.SetLayoutElement(gameObject, minHeight: 40, minWidth: 550);
            ItemTypeDown.captionText.fontSize = ZGScriptTrainer.FontSize.Value;
            ItemID = ItemidList[0];
            //设置下拉栏滚动速度
            SetDropdownScrollSpeed(ItemTypeDown, 10f);
            Item.CreateButton("添加", () =>
            {
                if (Singleton<RoleMgr>.Ins.GetRole() != null)
                {
                    Singleton<RoleMgr>.Ins.GetRole().AddState(ItemID);
                }
            }).SetLayoutElement(40, 30);

            Item.CreateButton("移除", () =>
            {
                if (Singleton<RoleMgr>.Ins.GetRole() != null)
                {
                    Singleton<RoleMgr>.Ins.GetRole().DelState(ItemID);
                }
            }).SetLayoutElement(40, 30);
            ItemTypeDown.onValueChanged.AddListener((index) => OnDropdownItemValueChanged(index, tabGroup));

            AttrUI.currentPageControls.Add(Item);

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
