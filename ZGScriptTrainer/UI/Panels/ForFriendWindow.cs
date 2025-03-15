using System;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UniverseLib.UI.Models;
using UniverseLib.UI;
using ZGScriptTrainer.UI.Models;
using ZGScriptTrainer.UI.Cells;
using Sdk;
using ZGScriptTrainer.FriendSet;
using ZGScriptTrainer.TheFriend;
using ZGScriptTrainer.FrendSet;
using TheEntity;
using System.Linq;
using ZGScriptTrainer.Cheat;
using ZGScriptTrainer.ItemSpawn;
using Newtonsoft.Json.Linq;

namespace ZGScriptTrainer.UI.Panels
{
    public class ForTheFriendWindow : ZGPanel/*, ICellPoolDataSource<FriendCell>*/
    {
        public static ForTheFriendWindow Instance { get; private set; }
        public override UIManager.Panels PanelType => UIManager.Panels.ForTheFriendWindow;
        public override string Name => UIManager.PanelNames[PanelType];
        public override int MinWidth => 350;
        public override int MinHeight => 200;
        public override Vector2 DefaultAnchorMin => new Vector2(0.35f, 0.175f);
        public override Vector2 DefaultAnchorMax => new Vector2(0.8f, 0.925f);
        public override Vector2 DefaultPosition => new Vector2(0f, 0f);

        private readonly List<ForFriend> ZGFriend = new List<ForFriend>();
        public int FriendCount => ZGFriend.Count;

        //public ScrollPool<FriendCell> ScrollPool { get; private set; }
        public bool Initialized { get; private set; } = false;

        public Dropdown FriendTypeDropdown;
        public Dropdown FriendRelationTypeDropdown;

        public static InputFieldRef SearchInput;

        public GameObject FriendGrid;
        public UniverseLib.UI.Widgets.AutoSliderScrollbar autoSliderScrollbar;
        public List<GameObject> TheFriend = new List<GameObject>();

        public GameObject FriendGridScrollView;
        private bool CanAddListener = true;
        private int FriendTypeDropdownIndex = 0;
        public List<int> FriendRelationidList = new List<int>();
        public override void OnFinishResize()
        {
            base.OnFinishResize();
        }
        public ForTheFriendWindow(UIBase owner) : base(owner)
        {
            Instance = this;
            Initialize();

            //foreach (KeyValuePair<string, IConfigElement> entry in ConfigManager.ConfigElements)
            //{
            //    configEntries.Add(cache);
            //}

            //foreach (CacheConfigEntry config in configEntries)
            //    config.UpdateValueFromSource();
        }
        public void Initialize()
        {
            if (FriendSet.FriendSet.BaseFriend.Count != 0)
            {
                ZGFriend.AddRange(FriendSet.FriendSet.GetTypeFriends());
                Initialized = true;
                PrintFriend();
            }
        }
        public void SetCell(FriendCell cell, int index)
        {

            if (index < 0 || index >= ZGFriend.Count)
            {
                cell.Disable();
                return;
            }

            ForFriend entry = ZGFriend[index];
            cell.NameLabel.text = entry.GetFriendName();
            cell.ItemImage.sprite = entry.GetFriendIcon();
            //cell.DescriptionLabel.text = entry.GetFriendDescription();
            cell.SubmitButton.OnClick = new Action(() => {
                FriendSet.FriendSet.AddFavor(entry);
            });
            cell.SubmitButton.OnClick = new Action(() => {
                FriendSet.FriendSet.DelFavor(entry);
            });
        }
        public void OnCellBorrowed(FriendCell cell)
        {

        }
        public void SearchFriend(string text)
        {
            ZGFriend.Clear();
            ZGFriend.AddRange(text.FilterFriendData(FriendTypeDropdown.value));
            //ScrollPool.Refresh(true, true);
            PrintFriend();
        }
        public void ChangeFriendCount(string text)
        {
            if (int.TryParse(text, out int result))
            {
                FriendSet.FriendSet.AddCount = result;
            }
        }
        private void FriendTypeDropdownValueChange(int value)
        {

            Debug.Log(value.ToString());
            ZGFriend.Clear();
            ZGFriend.AddRange(SearchInput.Text.FilterFriendData(value));
            //ScrollPool.Refresh(true, true);
            PrintFriend();
        }
        public void PrintFriend()
        {
            foreach (var Friend in TheFriend)
            {
                UnityEngine.Object.Destroy(Friend);
            }
            TheFriend.Clear();

            foreach (var Friend in ZGFriend)
            {
                var UIRoot = UIFactory.CreateUIObject("GridUIRoot", FriendGrid, new Vector2(230, 110));
                var Rect = UIRoot.GetComponent<RectTransform>();

                // 物品图片 
                var FriendUI = UIFactory.CreateUIObject("FriendUI", UIRoot, new Vector2(50, 50));
                var FriendImage = FriendUI.AddComponent<Image>();
                FriendImage.preserveAspect = true;
                FriendImage.sprite = Friend.GetFriendIcon();
                UIFactory.SetLayoutElement(FriendUI, minHeight: 50, minWidth: 50, flexibleWidth: 0, flexibleHeight: 0);
                FriendUI.GetComponent<RectTransform>().localPosition = new Vector2(-70, 0);

                // 添加按钮 
                var SubmitButton = UIFactory.CreateButton(UIRoot, "SubmitButton", "添加", new Color(0.15f, 0.19f, 0.15f));
                SubmitButton.OnClick = new Action(() =>
                {
                    FriendSet.FriendSet.AddFavor(Friend);     
                });
                SubmitButton.ButtonText.fontSize = ZGScriptTrainer.FontSize.Value;
                float addButtonWidth = 40f;
                SubmitButton.ButtonText.GetComponent<RectTransform>().sizeDelta = new Vector2(addButtonWidth, 45);
                addButtonWidth = SubmitButton.ButtonText.preferredWidth;
                SubmitButton.ButtonText.GetComponent<RectTransform>().sizeDelta = new Vector2(addButtonWidth, 45);
                SubmitButton.Component.GetComponent<RectTransform>().sizeDelta = new Vector2(addButtonWidth + 5, 50);

                // 移除按钮 
                var RemoveButton = UIFactory.CreateButton(UIRoot, "RemoveButton", "减少", new Color(0.15f, 0.19f, 0.15f));
                RemoveButton.OnClick = new Action(() =>
                {
                    FriendSet.FriendSet.DelFavor(Friend);
                });
                RemoveButton.ButtonText.fontSize = ZGScriptTrainer.FontSize.Value;
                float removeButtonWidth = 40f;
                RemoveButton.ButtonText.GetComponent<RectTransform>().sizeDelta = new Vector2(removeButtonWidth, 45);
                removeButtonWidth = RemoveButton.ButtonText.preferredWidth;
                RemoveButton.ButtonText.GetComponent<RectTransform>().sizeDelta = new Vector2(removeButtonWidth, 45);
                RemoveButton.Component.GetComponent<RectTransform>().sizeDelta = new Vector2(removeButtonWidth + 5, 50);

                // 计算按钮总宽度 
                float totalButtonWidth = addButtonWidth + removeButtonWidth + 10;

                // 设置添加按钮位置 
                SubmitButton.Component.GetComponent<RectTransform>().localPosition = new Vector2(95 - totalButtonWidth / 2, 0);
                // 设置移除按钮位置 
                RemoveButton.Component.GetComponent<RectTransform>().localPosition = new Vector2(95 - totalButtonWidth / 2 + addButtonWidth + 20, 0);

                var RelationLabel = UIFactory.CreateLabel(UIRoot, "RelationLabel", "修改关系：", TextAnchor.MiddleLeft, fontSize: ZGScriptTrainer.FontSize.Value);
                RelationLabel.horizontalOverflow = HorizontalWrapMode.Wrap;
                RelationLabel.GetComponent<RectTransform>().sizeDelta = new Vector2(80, 50);
                RelationLabel.GetComponent<RectTransform>().localPosition = new Vector2(-50, -60);
                RelationLabel.resizeTextForBestFit = true;

                Dictionary<int, string> RelationIDToName = new Dictionary<int, string>();
                GameObject gameObject2 = UIFactory.CreateDropdown(UIRoot, "FriendTypeDropdown", out FriendRelationTypeDropdown, "选择类型", ZGScriptTrainer.FontSize.Value, null, null);
                foreach (var kvp in FriendSet.FriendSet.GetRelation.Values)
                {
                    FriendRelationTypeDropdown.options.Add(new Dropdown.OptionData(kvp.name));
                    FriendRelationidList.Add(kvp.id);
                    RelationIDToName[kvp.id] = kvp.name;
                }
                UIFactory.SetLayoutElement(gameObject2, minHeight: 40, minWidth: 180);
                FriendRelationTypeDropdown.captionText.fontSize = ZGScriptTrainer.FontSize.Value;
                FriendRelation FriendRelation = new FriendRelation();
                FriendRelationTypeDropdown.onValueChanged.AddListener((index) => FriendRelation.DropDownFriendRelationChange(index, FriendRelationTypeDropdown, FriendRelationidList,Friend));
                Role role = Singleton<RoleMgr>.Ins.GetRole(Friend.FriendID);
                string RoleName = null;
                if ( FriendRelationidList.Contains(role.GetRelationForUI()))
                {
                    int RelationID = role.GetRelationForUI();
                    if (RelationIDToName.TryGetValue(RelationID, out RoleName))
                    FriendRelation.OnSearchInputChanged(RoleName, FriendRelationTypeDropdown);
                }

                // 手动排版，将 gameObject2 的垂直位置设置为 -50
                RectTransform rectTransform = gameObject2.GetComponent<RectTransform>();
                if (rectTransform != null)
                {
                    Vector2 anchoredPosition = rectTransform.anchoredPosition;
                    anchoredPosition.y = -60;
                    anchoredPosition.x = 50;
                    rectTransform.anchoredPosition = anchoredPosition;
                }

                // 物品名称标签 
                var NameLabel = UIFactory.CreateLabel(UIRoot, "NameLabel", Friend.GetFriendName(), TextAnchor.MiddleLeft, fontSize: ZGScriptTrainer.FontSize.Value);
                NameLabel.horizontalOverflow = HorizontalWrapMode.Wrap;

                float labelWidth = 210 - 60 - totalButtonWidth;
                NameLabel.GetComponent<RectTransform>().sizeDelta = new Vector2(labelWidth, 50);
                NameLabel.GetComponent<RectTransform>().localPosition = new Vector2(-40 + labelWidth / 2, 0);

                NameLabel.resizeTextForBestFit = true;
                NameLabel.resizeTextMinSize = 10;
                NameLabel.resizeTextMaxSize = ZGScriptTrainer.FontSize.Value;

              

                var tool = UIRoot.AddComponent<TooltipGUI>();
                tool.Initialize(Friend.GetFriendDescription(), FriendGridScrollView);
                TheFriend.Add(UIRoot);
            }

            //for(int i = 0; i < ZGFriend.Count; i++)
            //{
            //    var tool = Friend[i].AddComponent<TooltipGUI>();
            //    tool.Initialize(ZGFriend[i].GetFriendDescription(), UIManager.UIToolTip/*Friend.Last()*/);
            //}
        }

        public override void Update()
        {
            base.Update();
            if (!CanAddListener)
            {
                if (FriendTypeDropdownIndex != FriendTypeDropdown.value)
                {
                    FriendTypeDropdownIndex = FriendTypeDropdown.value;
                    FriendTypeDropdownValueChange(FriendTypeDropdownIndex);
                }
            }
            if (!Initialized)
            {
                Initialize();
            }
            if (Singleton<RoleMgr>.Ins.GetRole() != null)
                FriendSet.FriendSet.GetBaseFriendData();
            else
                FriendSet.FriendSet.i = 0;
        }
        protected override void ConstructPanelContent()
        {
            GameObject tabGroup = UIFactory.CreateVerticalGroup(ContentRoot, "FriendWindowVerticalGroup", true, false, true, true, 5,
                new Vector4(4, 4, 4, 4), new Color(0.065f, 0.065f, 0.065f));
            UIFactory.SetLayoutElement(tabGroup, minHeight: 40, flexibleHeight: 9999);

            var lable = UIFactory.CreateLabel(tabGroup, "Lable", "注意：请先进入存档，才能加载人物，打开页面后等待朋友数据加载完毕后再点击刷新朋友页面，" +
                "空白头像的人物我全部排除了，这种也是不能互动的，加了也没必要，所有朋友显示在地图上，你显示过一次后就会一直显示在地图上来", fontSize: ZGScriptTrainer.FontSize.Value);
            ZGUIUtility.SetLayoutElement(lable, minHeight: 40, flexibleWidth: 0);
            // Save button

            GameObject Row = UIFactory.CreateHorizontalGroup(tabGroup, "Row", false, false, true, true, 5, default,
            new Color(0.065f, 0.065f, 0.065f), TextAnchor.MiddleLeft);
            UIFactory.SetLayoutElement(Row, minHeight: 40, flexibleWidth: 9999);

            Row.CreateToggle("所有朋友显示在地图上", (state) => { FriendSet.FriendSet.IsAllFriendShowInMap = state; });
            Row.CreateToggle("社交列表显示全部可互动人物", (state) => { MapFriendCheat.显示全部人物 = state; });
            Row.CreateToggle("社交无限制", (state) => { GetFriendCheat.社交无限制 = state; });

            //GameObject Row1 = UIFactory.CreateHorizontalGroup(tabGroup, "Row1", false, false, true, true, 5, default,
            //    new Color(0.065f, 0.065f, 0.065f), TextAnchor.MiddleLeft);
            //UIFactory.SetLayoutElement(Row1, minHeight: 40, flexibleWidth: 9999);


            //var lable1 = UIFactory.CreateLabel(Row1, "Lable", "类别:", fontSize: ZGScriptTrainer.FontSize.Value);
            //ZGUIUtility.SetLayoutElement(lable1, minHeight: 40, flexibleWidth: 0);

            //GameObject gameObject2 = UIFactory.CreateDropdown(Row1, "FriendTypeDropdown", out FriendTypeDropdown, "选择类型", ZGScriptTrainer.FontSize.Value, null, null);
            //for (int i = 0; i < FriendSet.FriendSet.FriendTypeDic.Count; i++)
            //{
            //    FriendTypeDropdown.options.Add(new Dropdown.OptionData(FriendSet.FriendSet.FriendTypeDic[i]));
            //}
            //UIFactory.SetLayoutElement(gameObject2, minHeight: 40, minWidth: 180);
            //FriendTypeDropdown.captionText.fontSize = ZGScriptTrainer.FontSize.Value;
            //try
            //{
            //    FriendTypeDropdown.onValueChanged.AddListener((UnityEngine.Events.UnityAction<int>)FriendTypeDropdownValueChange);
            //}
            //catch (Exception ex)
            //{
            //    ZGScriptTrainer.WriteLog($"AddListener错误：\n{ex}", LogType.Error);
            //    CanAddListener = false;
            //    //if (FriendTypeDropdown != null)
            //    //{
            //    //    UnityEngine.Object.Destroy(FriendTypeDropdown);
            //    //}
            //    //for (int i = 0; i < ZGFriendUtil.FriendTypeDic.Count; i++)
            //    //{
            //    //    var button = UIFactory.CreateButton(Row1, "Row1Button", ZGFriendUtil.FriendTypeDic[i]);
            //    //    button.SetLayoutElement(minHeight: 40, flexibleWidth: 0);
            //    //    int tmpi = i;
            //    //    button.OnClick = () => { FriendTypeDropdownValueChange(tmpi); ZGScriptTrainer.WriteLog($"ZG Button:{tmpi}"); };
            //    //}
            //}

            GameObject Row2 = UIFactory.CreateHorizontalGroup(tabGroup, "Row2", false, false, true, true, 5, default,
                new Color(0.065f, 0.065f, 0.065f), TextAnchor.MiddleLeft);
            UIFactory.SetLayoutElement(Row2, minHeight: 40, flexibleWidth: 0);

            var lable2 = UIFactory.CreateLabel(Row2, "Lable", "搜索:", fontSize: ZGScriptTrainer.FontSize.Value);
            ZGUIUtility.SetLayoutElement(lable2, minHeight: 40, flexibleWidth: 0);

            SearchInput = UIFactory.CreateInputField(Row2, "FriendearchInput", FriendSet.FriendSet.DefaultSearchText);
            ZGUIUtility.SetLayoutElement(SearchInput, minWidth: 180, minHeight: 40);
            SearchInput.OnValueChanged += new Action<string>(SearchFriend);

            Row2.CreateButton("刷新朋友页面（没有显示朋友的时候）", () =>
            {
                ZGFriend.Clear();
                ZGFriend.AddRange(SearchInput.Text.FilterFriendData(0));
                PrintFriend();
            }).SetLayoutElement(20, 40);

            ////添加数量
            GameObject Row3 = UIFactory.CreateHorizontalGroup(tabGroup, "Row3", false, false, true, true, 5, default,
                new Color(0.065f, 0.065f, 0.065f), TextAnchor.MiddleLeft);
            UIFactory.SetLayoutElement(Row3, minHeight: 40, flexibleWidth: 0);

            var lable3 = UIFactory.CreateLabel(Row3, "Lable", "好感添加数量:", fontSize: ZGScriptTrainer.FontSize.Value);
            ZGUIUtility.SetLayoutElement(lable3, minHeight: 40, flexibleWidth: 0);

            var CountInput = UIFactory.CreateInputField(Row3, "FriendCountInput", "1");
            ZGUIUtility.SetLayoutElement(CountInput, minWidth: 180, minHeight: 40);
            CountInput.OnValueChanged += new Action<string>(ChangeFriendCount);



            // Config entries

            //ScrollPool = UIFactory.CreateScrollPool<FriendCell>(
            //    this.ContentRoot,
            //    "ZGFriend",
            //    out GameObject scrollObj,
            //    out GameObject scrollContent);

            //ScrollPool.Initialize(this);

            FriendGridScrollView = tabGroup.CreateGridScrollView("FriendGridScrollView", new Vector2(230, 130), new Vector2(10, 5), out FriendGrid, out autoSliderScrollbar);
        }
    }
}
