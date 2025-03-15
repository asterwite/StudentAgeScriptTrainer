using System;
using UnityEngine.UI;
using UnityEngine;
using UniverseLib.UI;
using ZGScriptTrainer.UI.Models;
using HarmonyLib;
using Sdk;
using Config;
using BepInEx.Configuration;
using System.Security.Cryptography;
using ZGScriptTrainer.MainUI;
using System.Collections.Generic;
using ZGScriptTrainer.Load;
using ZGScriptTrainer.MyManagerSet;

namespace ZGScriptTrainer.UI.Panels
{
    public class MainWindow : ZGPanel
    {
        public static MainWindow Instance { get; private set; }
        public override UIManager.Panels PanelType => UIManager.Panels.MainWindow;
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

        public static ConfigEntry<int> itemid { get; set; }
        public MainWindow(UIBase owner) : base(owner)
        {
            Instance = this;
        }
        public override void Update()
        {
            base.Update();
            LoadFile.LoadAsync<PersonAttrCfg>("Cfgs/" + LocalizationMgr.Lang + "/PersonAttrCfg", delegate (Dictionary<int, PersonAttrCfg> _t)
            {
                BasicUI.AttrSet = _t;
            });
            foreach (var kvp in BasicUI.AttrSet.Values)
            {
                foreach (var Newkvp in kvp.name)
                {
                    if (Newkvp.ToString() == "精力")
                        BasicUI.is精力 = kvp.id;
                    if (Newkvp.ToString() == "动力")
                        BasicUI.is动力 = kvp.id;
                    if (Newkvp.ToString() == "热情")
                        BasicUI.is热情 = kvp.id;
                    if (Newkvp.ToString() == "体魄")
                        BasicUI.is体魄 = kvp.id;
                    if (Newkvp.ToString() == "智力")
                        BasicUI.is智力 = kvp.id;
                    if (Newkvp.ToString() == "情商")
                        BasicUI.is情商 = kvp.id;
                    if (Newkvp.ToString() == "阅历")
                        BasicUI.is阅历 = kvp.id;
                    if (Newkvp.ToString() == "成就")
                        BasicUI.is成就 = kvp.id;
                    if (Newkvp.ToString() == "信任")
                        BasicUI.is信任 = kvp.id;
                }
            }
        }


        protected override void ConstructPanelContent()
        {
            ScrollPoolUI ScrollPoolUI = new ScrollPoolUI();
            var Object = ScrollPoolUI.DrawscrollPool(ContentRoot);
            GameObject tabGroup = UIFactory.CreateVerticalGroup(Object, "MainWindowVerticalGroup", true, false, true, true, 5,
                new Vector4(4, 4, 4, 4), new Color(0.065f, 0.065f, 0.065f));
            UIFactory.SetLayoutElement(tabGroup, minHeight: 40, flexibleHeight: 0);

            BasicUI.DrawBasicUI(tabGroup);



            // default active state: Active
            this.SetActive(true);
        }

    } 
}
