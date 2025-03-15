using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.UI;
using UnityEngine;
using UniverseLib.UI;
using ZGScriptTrainer.MyManagerSet;
using Sdk;
using ZGScriptTrainer.AttrSet;
using Config;
using ZGScriptTrainer.Load;

namespace ZGScriptTrainer.UI.Panels
{
    public class AttrWindow : ZGPanel
    {
        public static AttrWindow Instance { get; private set; }
        public override UIManager.Panels PanelType => UIManager.Panels.AttrWindow;
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

        public AttrWindow(UIBase owner) : base(owner)
        {
            Instance = this;
        }
        public override void Update()
        {
            base.Update();
            LoadFile.LoadAsync<PersonAttrCfg>("Cfgs/" + LocalizationMgr.Lang + "/PersonAttrCfg", delegate (Dictionary<int, PersonAttrCfg> _t)
            {
               AttrCheat.ItemSet = _t;
            });
            LoadFile.LoadAsync<PersonStateCfg>("Cfgs/" + LocalizationMgr.Lang + "/PersonStateCfg", delegate (Dictionary<int, PersonStateCfg> _t)
            {
                AddState.TraitsSet = _t;
            });
        }
        public Dropdown TypeDown;

        private static List<string> PagSet = new List<string>()
            {
            "空页面",
            "属性",
            "状态",         
            };
        private static bool Bonce = true;

        private static List<GameObject> NoInGameNoSpanw;
        protected override void ConstructPanelContent()
        {
            ScrollPoolUI ScrollPoolUI = new ScrollPoolUI();
            var Object = ScrollPoolUI.DrawscrollPool(ContentRoot);
            // Tab bar
            GameObject label = UIFactory.CreateVerticalGroup(Object, "MainWindowlabel", true, false, true, true, 5,
            new Vector4(4, 4, 4, 4), new Color(0.065f, 0.065f, 0.065f));
            UIFactory.CreateLabel(label, "Lable", "使用说明：先进入主菜单，再从空页面转到属性页面"
                            , fontSize: ZGScriptTrainer.FontSize.Value);

            GameObject tabGroup = UIFactory.CreateVerticalGroup(Object, "MainWindowVerticalGroup", true, false, true, true, 5,
                new Vector4(4, 4, 4, 4), new Color(0.065f, 0.065f, 0.065f));
            UIFactory.SetLayoutElement(tabGroup, minHeight: 40, flexibleHeight: 0);
            UIFactory.CreateLabel(tabGroup, "Lable", "属性:", fontSize: ZGScriptTrainer.FontSize.Value);
            GameObject gameObject2 = UIFactory.CreateDropdown(tabGroup, "MainTypeDown", out TypeDown, "选择属性", ZGScriptTrainer.FontSize.Value, null, null);

            foreach (var kvp in PagSet)
            {
                TypeDown.options.Add(new Dropdown.OptionData(kvp));
            }
            UIFactory.SetLayoutElement(gameObject2, minHeight: 40, minWidth: 550);
            TypeDown.captionText.fontSize = ZGScriptTrainer.FontSize.Value;
            // 添加下拉框值改变的监听事件
            TypeDown.onValueChanged.AddListener((index) => AttrUI.SwitchPage(index, tabGroup));

            this.SetActive(true);
        }
    }
}
