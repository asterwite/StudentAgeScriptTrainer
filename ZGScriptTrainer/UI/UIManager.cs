using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UniverseLib.UI.Models;
using UniverseLib.UI;
using UniverseLib;
using ZGScriptTrainer.UI.Panels;
using ZGScriptTrainer.UI.Models;
namespace ZGScriptTrainer.UI
{
    public static class UIManager
    {
        public enum Panels
        {
            MainWindow,
            ItemWindow,
            OptionWindow,
            ForTheFriendWindow,
            FriendWindow,
            FreeCamWindow,
            AttrWindow,
            MinGameWindow,
        }
        public enum VerticalAnchor
        {
            Top,
            Bottom
        }
        public static readonly Dictionary<Panels, string> PanelNames = new Dictionary<Panels, string>() 
        { 
            { Panels.MainWindow, "基础功能" }, 
            { Panels.AttrWindow, "属性设置" },
            { Panels.MinGameWindow, "小游戏修改" },
            { Panels.FriendWindow, "朋友修改" },
            { Panels.ForTheFriendWindow, "朋友设置" },
            
            { Panels.OptionWindow, "配置设置" },
            { Panels.ItemWindow, "物品添加" },
        };
        public static VerticalAnchor NavbarAnchor = VerticalAnchor.Top;
        public static bool Initializing { get; internal set; } = true;
        internal static UIBase UiBase { get; private set; }

        public const string UiBaseGUID = "ScriptTrainer.UiBase";
        public static GameObject UIRoot => UiBase?.RootObject;
        public static GameObject UIToolTip { get; private set; }
        public static GameObject WorldToolTip { get; private set; }
        public static RectTransform UIRootRect { get; private set; }
        public static Canvas UICanvas { get; private set; }

        internal static readonly Dictionary<Panels, ZGPanel> UIPanels = new Dictionary<Panels, ZGPanel>();

        public static RectTransform NavBarRect;

        public static GameObject NavbarTabButtonHolder;

        private static readonly Vector2 NAVBAR_DIMENSIONS = new Vector2(1020f, 40f);

        private static ButtonRef closeBtn;
        private static ButtonRef UpdateBtn;
        private static Text Title;

        public static bool ShowMenu
        {
            get => UiBase != null && UiBase.Enabled;
            set
            {
                if (UiBase == null || !UIRoot || UiBase.Enabled == value)
                    return;

                UniversalUI.SetUIActive(UiBaseGUID, value);
            }
        }

        internal static void InitUI()
        {


            UiBase = UniversalUI.RegisterUI(UiBaseGUID, null);

            UIRootRect = UIRoot.GetComponent<RectTransform>();
            UICanvas = UIRoot.GetComponent<Canvas>();
            UIToolTip = UIFactory.CreateUIObject("UIToolTip", UIRoot);

            WorldToolTip = UniversalUI.RegisterUI("ScriptTrainer.WorldToolTip", null).RootObject;
            UniversalUI.SetUIActive("ScriptTrainer.WorldToolTip", true);
            WorldToolTip.AddComponent<TooltipGUI>().Initialize("内置修改器", null, WorldToolTip);
            // Create UI.
            CreateTopNavBar();
            // This could be automated with Assembly.GetTypes(),
            // but the order is important and I'd have to write something to handle the order.
            UIPanels.Add(Panels.MainWindow, new MainWindow(UiBase));
            UIPanels.Add(Panels.MinGameWindow, new MinGameUI(UiBase));
            UIPanels.Add(Panels.ForTheFriendWindow, new ForTheFriendWindow(UiBase));
            UIPanels.Add(Panels.ItemWindow, new ItemWindow(UiBase));
            UIPanels.Add(Panels.AttrWindow, new AttrWindow(UiBase));
            UIPanels.Add(Panels.OptionWindow, new OptionWindow(UiBase));
            //UIPanels.Add(Panels.AutoCompleter, new (UiBase));

            //UIPanels.Add(Panels.ItemWindow, new ItemWindow(UiBase));

            //UIPanels.Add(Panels.ConsoleLog, new LogPanel(UiBase));


            // Failsafe fix, in some games all dropdowns displayed values are blank on startup for some reason.
            foreach (Dropdown dropdown in UIRoot.GetComponentsInChildren<Dropdown>(true))
                dropdown.RefreshShownValue();

            Initializing = false;

            ShowMenu = false;
        }

        #region[NavPanel]
        public static ZGPanel GetPanel(Panels panel) => UIPanels[panel];
        public static T GetPanel<T>(Panels panel) where T : ZGPanel => (T)UIPanels[panel];
        public static void TogglePanel(Panels panel)
        {
            ZGPanel uiPanel = GetPanel(panel);
            SetPanelActive(panel, !uiPanel.Enabled);
        }
        public static void SetPanelActive(Panels panelType, bool active)
        {
            GetPanel(panelType).SetActive(active);
        }
        public static void SetPanelActive(ZGPanel panel, bool active)
        {
            panel.SetActive(active);
        }
        private static void CreateTopNavBar()
        {
            GameObject navbarPanel = UIFactory.CreateUIObject("MainNavbar", UIRoot);
            UIFactory.SetLayoutGroup<HorizontalLayoutGroup>(navbarPanel, false, false, true, true, 5, 4, 4, 4, 4, TextAnchor.MiddleCenter);
            navbarPanel.AddComponent<Image>().color = new Color(0.1f, 0.1f, 0.1f);
            NavBarRect = navbarPanel.GetComponent<RectTransform>();
            NavBarRect.pivot = new Vector2(0.5f, 1f);

            switch (NavbarAnchor)
            {
                case VerticalAnchor.Top:
                    NavBarRect.anchorMin = new Vector2(0.5f, 1f);
                    NavBarRect.anchorMax = new Vector2(0.5f, 1f);
                    NavBarRect.anchoredPosition = new Vector2(NavBarRect.anchoredPosition.x, 0);
                    NavBarRect.sizeDelta = NAVBAR_DIMENSIONS;
                    break;

                case VerticalAnchor.Bottom:
                    NavBarRect.anchorMin = new Vector2(0.5f, 0f);
                    NavBarRect.anchorMax = new Vector2(0.5f, 0f);
                    NavBarRect.anchoredPosition = new Vector2(NavBarRect.anchoredPosition.x, 35);
                    NavBarRect.sizeDelta = NAVBAR_DIMENSIONS;
                    break;
            }

            //标题

            string titleTxt = $"{ZGBepInExInfo.PLUGIN_NAME} <i><color=grey>{ZGBepInExInfo.PLUGIN_VERSION}</color></i> {ZGBepInExInfo.For_Name}";
            Title = UIFactory.CreateLabel(navbarPanel, "Title", titleTxt, TextAnchor.MiddleCenter, default, true, ZGScriptTrainer.FontSize.Value);
            Title.SetLayoutElement(minWidth: 75, flexibleWidth: 0);

            //界面

            NavbarTabButtonHolder = UIFactory.CreateUIObject("NavTabButtonHolder", navbarPanel);
            UIFactory.SetLayoutElement(NavbarTabButtonHolder, minHeight: 25, flexibleHeight: 999, flexibleWidth: 999);
            UIFactory.SetLayoutGroup<HorizontalLayoutGroup>(NavbarTabButtonHolder, false, true, true, true, 4, 2, 2, 2, 2);


            //间隔
            GameObject spacer = UIFactory.CreateUIObject("Spacer", navbarPanel);
            UIFactory.SetLayoutElement(spacer, minWidth: 15);

            // Hide menu button
            UpdateBtn = UIFactory.CreateButton(navbarPanel, "UpdateButton", "更新");
            ZGUIUtility.SetLayoutElement(UpdateBtn, minHeight: 25, minWidth: 60, flexibleWidth: 0);
            RuntimeHelper.SetColorBlock(UpdateBtn.Component, Color.red,
                new Color(0.81f, 0.25f, 0.2f), new Color(0.6f, 0.18f, 0.16f));
            UpdateBtn.GameObject.SetActive(false);

            closeBtn = UIFactory.CreateButton(navbarPanel, "CloseButton", ZGScriptTrainer.ShowCounter.Value.ToString());
            ZGUIUtility.SetLayoutElement(closeBtn, minHeight: 25, minWidth: 60, flexibleWidth: 0);
            RuntimeHelper.SetColorBlock(closeBtn.Component, new Color(0.63f, 0.32f, 0.31f),
                new Color(0.81f, 0.25f, 0.2f), new Color(0.6f, 0.18f, 0.16f));

            navbarPanelObject = navbarPanel;
            MycloseBtn = closeBtn;

            closeBtn.OnClick += OnCloseButtonClicked;
        }
        private static void OnCloseButtonClicked()
        {
            ShowMenu = false;
        }
    
        public static void CheckUpdate()
        {
            Title.text = $"{ZGBepInExInfo.PLUGIN_NAME} <i><color=grey>{ZGBepInExInfo.PLUGIN_VERSION}</color></i> <i><color=red>需要更新</color></i>";
            Title.SetLayoutElement(minWidth: 75, flexibleWidth: 0);
            UpdateBtn.GameObject.SetActive(true);
        }
        #endregion
        public static GameObject navbarPanelObject;
        public static ButtonRef MycloseBtn;
        private static void CreateCloseButton(GameObject navbarPanel)
        {
            // 创建新的 closeBtn
            var closeBtn = UIFactory.CreateButton(navbarPanel, "CloseButton", ZGScriptTrainer.ShowCounter.Value.ToString());
            ZGUIUtility.SetLayoutElement(closeBtn, minHeight: 25, minWidth: 60, flexibleWidth: 0);
            RuntimeHelper.SetColorBlock(closeBtn.Component, new Color(0.63f, 0.32f, 0.31f),
            new Color(0.81f, 0.25f, 0.2f), new Color(0.6f, 0.18f, 0.16f));
            navbarPanelObject = navbarPanel;
            MycloseBtn = closeBtn;
        }
        public static void OnShowCounterValueChanged(ButtonRef closeBtn, GameObject navbarPanel)
        {
            UnityEngine.Object.Destroy(closeBtn.GameObject);
            // 创建新的 closeBtn
            CreateCloseButton(navbarPanel);
        }
    }
}
