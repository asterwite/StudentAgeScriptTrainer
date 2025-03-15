using System;
using UnityEngine.UI;
using UnityEngine;
using UniverseLib.UI;
using ZGScriptTrainer.UI.Models;
using HarmonyLib;
using ZGScriptTrainer.Cheat;
using System.Collections.Generic;
using BepInEx.Configuration;
using UniverseLib.UI.Widgets.ScrollView;
using UnityEngine;
using ZGScriptTrainer.MyManagerSet;
namespace ZGScriptTrainer.UI.Panels
{
    public class OptionWindow : ZGPanel
    {
        public static OptionWindow Instance { get; private set; }
        public override UIManager.Panels PanelType => UIManager.Panels.OptionWindow;
        public override string Name => UIManager.PanelNames[PanelType];
        public override int MinWidth => 350;
        public override int MinHeight => 200;
        public override Vector2 DefaultAnchorMin => new Vector2(0.35f, 0.175f);
        public override Vector2 DefaultAnchorMax => new Vector2(0.8f, 0.925f);

        public GameObject NavbarHolder;
        public Dropdown MouseInspectDropdown;
        public GameObject ContentHolder;
        public RectTransform ContentRect;
        public static float CurrentPanelWidth => Instance.Rect.rect.width;
        public static float CurrentPanelHeight => Instance.Rect.rect.height;

        public OptionWindow(UIBase owner) : base(owner)
        {
            Instance = this;
        }
        public override void Update()
        {
            base.Update();
        }

        public static Dropdown TypeDown;
        public static KeyCode ShowKey;
        public static bool Bonce = true;
        public static bool ShowWindow = false;
        public static bool ShowStartDelay = false;
        public static bool ShowFont = false;
        public static List<GameObject> CurrentStartDelayListPag = new List<GameObject>();
        public static List<GameObject> CurrenShowWindowListPag = new List<GameObject>();
        public static List<GameObject> CurrentFontListPag = new List<GameObject>();
        private static float SetStartDelay;
        private static int SetFont;
        public static bool  RestShowWindow =false;
        protected override void ConstructPanelContent()
        {
            ScrollPoolUI ScrollPoolUI = new ScrollPoolUI();
            var Object = ScrollPoolUI.DrawscrollPool(ContentRoot);

            GameObject tabGroup = UIFactory.CreateVerticalGroup(Object, "MainWindowVerticalGroup", true, false, true, true, 5,
                new Vector4(4, 4, 4, 4), new Color(0.065f, 0.065f, 0.065f));
            UIFactory.SetLayoutElement(tabGroup, minHeight: 40, flexibleHeight: 0);

            var saveBtn = UIFactory.CreateButton(tabGroup, "Save", "保存所有设置", new Color(0.2f, 0.3f, 0.2f));
            UIFactory.SetLayoutElement(saveBtn.Component.gameObject, flexibleWidth: 9999, minHeight: 30, flexibleHeight: 0);
            saveBtn.OnClick = SaveOption;

            var restoreBtn = UIFactory.CreateButton(tabGroup, "restore", "还原所有设置", new Color(0.2f, 0.3f, 0.2f));
            UIFactory.SetLayoutElement(restoreBtn.Component.gameObject, flexibleWidth: 9999, minHeight: 30, flexibleHeight: 0);
            restoreBtn.OnClick = RestOption;

            GameObject ShowMain = UIFactory.CreateVerticalGroup(Object, "MainWindowShowMain", true, false, true, true, 5,
            new Vector4(4, 4, 4, 4), new Color(0.065f, 0.065f, 0.065f));
            UIFactory.SetLayoutElement(ShowMain, minHeight: 40, flexibleHeight: 0);


            Text title2 = UIFactory.CreateLabel(ShowMain, "Title", "呼出快捷键：", TextAnchor.MiddleLeft, default, true, ZGScriptTrainer.FontSize.Value);
            UIFactory.SetLayoutElement(title2.gameObject, minWidth: 75, flexibleWidth: 0);
            ButtonSet buttonSet = new ButtonSet();
            var button = ShowMain.CreateOptionButton("▲", buttonSet.lightGreen, () =>
            {
                CleanControls clean = new CleanControls();
                ShowWindow = !ShowWindow;
                if (ShowWindow)
                    ShowWindowKeyUI(ShowMain);
                else
                    clean.RemoveNeedSetControls(CurrenShowWindowListPag);
            });
            button.SetLayoutElement(5, 20);
            buttonSet.EnableButtonCheckSet(button, buttonSet.lightGreen, buttonSet.lightBule, "▲", "▼");


            GameObject StartDelay = UIFactory.CreateVerticalGroup(Object, "MainWindowStartDelay", true, false, true, true, 5,
            new Vector4(4, 4, 4, 4), new Color(0.065f, 0.065f, 0.065f));
            UIFactory.SetLayoutElement(StartDelay, minHeight: 40, flexibleHeight: 0);

            Text title3 = UIFactory.CreateLabel(StartDelay, "Title", "启动延迟时间：", TextAnchor.MiddleLeft, default, true, ZGScriptTrainer.FontSize.Value);
            UIFactory.SetLayoutElement(title3.gameObject, minWidth: 75, flexibleWidth: 0);
            ButtonSet buttonSet1 = new ButtonSet();
            var buttonStartDelay = StartDelay.CreateOptionButton("▲", buttonSet1.lightGreen, () =>
            {
                CleanControls clean = new CleanControls();
                ShowStartDelay = !ShowStartDelay;
                if (ShowStartDelay)
                    ShowStartDelayUI(StartDelay);
                else
                clean.RemoveNeedSetControls(CurrentStartDelayListPag);
            });
            buttonStartDelay.SetLayoutElement(5, 20);
            buttonSet1.EnableButtonCheckSet(buttonStartDelay, buttonSet1.lightGreen, buttonSet1.lightBule, "▲", "▼");


            GameObject Font = UIFactory.CreateVerticalGroup(Object, "MainWindowFont", true, false, true, true, 5,
            new Vector4(4, 4, 4, 4), new Color(0.065f, 0.065f, 0.065f));
            UIFactory.SetLayoutElement(Font, minHeight: 40, flexibleHeight: 0);
            Text title4 = UIFactory.CreateLabel(Font, "Title", "菜单字体大小：", TextAnchor.MiddleLeft, default, true, ZGScriptTrainer.FontSize.Value);
            UIFactory.SetLayoutElement(title3.gameObject, minWidth: 75, flexibleWidth: 0);
            ButtonSet buttonFontSet = new ButtonSet();
            var buttonFont = Font.CreateOptionButton("▲", buttonFontSet.lightGreen, () =>
            {
                CleanControls clean = new CleanControls();
                ShowFont = !ShowFont;
                if (ShowFont)
                    ShowFontUI(Font);
                else
                    clean.RemoveNeedSetControls(CurrentFontListPag);
            });
            buttonFont.SetLayoutElement(5, 20);
            buttonFontSet.EnableButtonCheckSet(buttonFont, buttonFontSet.lightGreen, buttonFontSet.lightBule, "▲", "▼");
            this.SetActive(true);
        }
        public static void SaveOption()
        {
            if (ShowKey != KeyCode.None )
            ZGScriptTrainer.ShowCounter.Value = ShowKey;  Bonce = false;
            if (ShowStartDelay != null)
            ZGScriptTrainer.StartDelay.Value = SetStartDelay;
            if ( ShowFont!= null)
                ZGScriptTrainer.FontSize.Value = SetFont;
        }
        public static void RestOption()
        {
            ZGScriptTrainer.ShowCounter.Value = KeyCode.Insert; Bonce = false; RestShowWindow = true;
            ZGScriptTrainer.StartDelay.Value = 1f;
            ZGScriptTrainer.FontSize.Value = 17;
        }


        #region[设置下拉栏滚动速度]
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
        #endregion


        #region[菜单呼出快捷键]
        private static void ShowWindowKeyUI(GameObject tabGroup)
        {
            GameObject showWindow = UIFactory.CreateVerticalGroup(tabGroup, "MainWindowshowWindow", true, false, true, true, 5,
           new Vector4(4, 4, 4, 4), new Color(0.065f, 0.065f, 0.065f));
            UIFactory.SetLayoutElement(showWindow, minHeight: 40, flexibleHeight: 0);

            GameObject gameObject = UIFactory.CreateDropdown(showWindow, "MainTypeDown", out TypeDown, "快捷键", 14, null, null);
            // 初始化下拉栏选项
            List<string> keyCodeNames = new List<string>();
            Array keyCodes = Enum.GetValues(typeof(KeyCode));
            foreach (KeyCode keyCode in keyCodes)
            {
                keyCodeNames.Add(keyCode.ToString());
            }
            UIFactory.SetLayoutElement(gameObject, minHeight: 40, minWidth: 200);
            TypeDown.captionText.fontSize = ZGScriptTrainer.FontSize.Value;

            showWindow.CreateButton("设置：", () =>
            {
                ZGScriptTrainer.ShowCounter.Value = ShowKey;
                Bonce = false;
            }).SetLayoutElement(20, 40);
            TypeDown.AddOptions(keyCodeNames);

            // 设置下拉栏的初始选中项
            TypeDown.value = Array.IndexOf(keyCodes, ZGScriptTrainer.ShowCounter.Value);

            SetDropdownScrollSpeed(TypeDown, 10f);

            // 处理下拉栏的选中事件
            TypeDown.onValueChanged.AddListener((index) =>
            {
                ShowKey = (KeyCode)keyCodes.GetValue(index);
            });
            CurrenShowWindowListPag.Add(showWindow);
        }
        #endregion


        #region[菜单延迟]
        public static void ShowStartDelayUI(GameObject tabGroup)
        {
            GameObject StartDelay = UIFactory.CreateVerticalGroup(tabGroup, "MainWindowStartDelay", true, false, true, true, 5,
            new Vector4(4, 4, 4, 4), new Color(0.065f, 0.065f, 0.065f));
            UIFactory.SetLayoutElement(StartDelay, minHeight: 40, flexibleHeight: 0);

            StartDelay.CreateInputEditButton("延迟时间", "10", "设置", new Action<string>((text) =>
            {
                var MoveSpeedMul = text.ConvertToFloatDef(10f);
                SetStartDelay = MoveSpeedMul;
                ZGScriptTrainer.StartDelay.Value = SetStartDelay;
                //Manager<MainPlayerData>.i.speed = MoveSpeed;
            }));
            CurrentStartDelayListPag.Add(StartDelay);
        }
        #endregion


        #region[字体大小]
        private static void ShowFontUI(GameObject tabGroup)
        {
            GameObject Font = UIFactory.CreateVerticalGroup(tabGroup, "MainWindowStartDelay", true, false, true, true, 5,
           new Vector4(4, 4, 4, 4), new Color(0.065f, 0.065f, 0.065f));
            UIFactory.SetLayoutElement(Font, minHeight: 40, flexibleHeight: 0);

            Font.CreateInputEditButton("字体大小", "17", "设置", new Action<string>((text) =>
            {
                var FontSize = text.ConvertToIntDef(17);
                SetFont = FontSize;
                ZGScriptTrainer.FontSize.Value = SetFont;
                //Manager<MainPlayerData>.i.speed = MoveSpeed;
            }));
            CurrentFontListPag.Add(Font);
        }
        #endregion



    }
}
