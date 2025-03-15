using BepInEx.Configuration;
using Config;
using Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.UI;
using UnityEngine;
using UniverseLib.UI;
using MiniGame.Negotiation;
using HarmonyLib;
using ZGScriptTrainer.Cheat;
using ZGScriptTrainer.MyManagerSet;
using ZGScriptTrainer.MainUI;
using System.Reflection.Emit;
using ZGScriptTrainer.Load;

namespace ZGScriptTrainer.UI.Panels
{
    public class MinGameUI : ZGPanel
    {
        public static MinGameUI Instance { get; private set; }
        public override UIManager.Panels PanelType => UIManager.Panels.MinGameWindow;
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
        public MinGameUI(UIBase owner) : base(owner)
        {
            Instance = this;
        }
        public override void Update()
        {
            base.Update();
         
        }
        public static int ID = 0;
        protected override void ConstructPanelContent()
        {
            ScrollPoolUI ScrollPoolUI = new ScrollPoolUI();
            var Object = ScrollPoolUI.DrawscrollPool(ContentRoot);
            // Tab bar
            GameObject tabGroup = UIFactory.CreateVerticalGroup(Object, "MainWindowVerticalGroup", true, false, true, true, 5,
                new Vector4(4, 4, 4, 4), new Color(0.065f, 0.065f, 0.065f));
            UIFactory.SetLayoutElement(tabGroup, minHeight: 40, flexibleHeight: 0);

            Text title = UIFactory.CreateLabel(tabGroup, "Title", "交涉小游戏设置：", TextAnchor.MiddleLeft, default, true, ZGScriptTrainer.FontSize.Value);
            UIFactory.SetLayoutElement(title.gameObject, minWidth: 75, flexibleWidth: 0);
            tabGroup.CreateSplitPanel(Color.white);

            GameObject Talk = UIFactory.CreateHorizontalGroup(tabGroup, "MainWindowTime", false, false, true, true, 5, default,
              new Color(0.065f, 0.065f, 0.065f), TextAnchor.MiddleLeft);
            UIFactory.SetLayoutElement(Talk, minHeight: 40, flexibleWidth: 9999);


            Talk.CreateInputEditButton("添加策略点", "100", "添加", new Action<string>((string text) =>
            {
                MinGameCheat.添加了策略点 = true;
                int MP = text.ConvertToIntDef(100);
                MinGameCheat.添加策略点 = MP;
            }));

            Talk.CreateButton("补满血量", () =>
            {
                MinGameCheat.补充血量 = true;
            }).SetLayoutElement(40, 30);

            Talk.CreateButton("快速说服", () =>
            {
               MinGameCheat.快速说服对手 = true;
            }).SetLayoutElement(40, 30);

            Talk.CreateButton("跳过对话", () =>
            {
                MinGameCheat.跳过对话 = true;
            }).SetLayoutElement(40, 30);

            Text title1 = UIFactory.CreateLabel(tabGroup, "Title", "学习小游戏设置：", TextAnchor.MiddleLeft, default, true, ZGScriptTrainer.FontSize.Value);
            UIFactory.SetLayoutElement(title.gameObject, minWidth: 75, flexibleWidth: 0);
            tabGroup.CreateSplitPanel(Color.white);

            GameObject CardMatch = UIFactory.CreateHorizontalGroup(tabGroup, "MainWindowconnection", false, false, true, true, 5, default,
              new Color(0.065f, 0.065f, 0.065f), TextAnchor.MiddleLeft);
            UIFactory.SetLayoutElement(CardMatch, minHeight: 40, flexibleWidth: 9999);

            CardMatch.CreateButton("跳过游戏（复习）", () =>
            {
                MinGameCheat.跳过方块匹配游戏复习 = true;
            }).SetLayoutElement(40, 30);

            CardMatch.CreateButton("跳过游戏（英语）", () =>
            {
                MinGameCheat.跳过方块匹配游戏英语 = true;
            }).SetLayoutElement(40, 30);

            CardMatch.CreateButton("跳过计算游戏（数学）", () =>
            {
                MinGameCheat.跳过计算游戏 = true;
            }).SetLayoutElement(40, 30);


            CardMatch.CreateButton("跳过游戏（语文）", () =>
            {
                MinGameCheat.跳过填词小游戏 = true;
            }).SetLayoutElement(40, 30);

            GameObject CardMatch1 = UIFactory.CreateHorizontalGroup(tabGroup, "MainWindowCardMatch1", false, false, true, true, 5, default,
            new Color(0.065f, 0.065f, 0.065f), TextAnchor.MiddleLeft);
            UIFactory.SetLayoutElement(CardMatch1, minHeight: 40, flexibleWidth: 9999);

            CardMatch1.CreateButton("跳过卡牌顺序游戏（文综）", () =>
            {
                MinGameCheat.跳过卡牌顺序游戏 = true;
            }).SetLayoutElement(40, 30);

            CardMatch1.CreateButton("跳过闭合电路游戏（理综）", () =>
            {
                MinGameCheat.跳过闭合电路游戏 = true;
            }).SetLayoutElement(40, 30);

            CardMatch1.CreateButton("跳过期末考试", () =>
            {
                
                MinGameCheat.跳过期末考试 = true;
                if (Singleton<RoleMgr>.Ins.GetRole() != null)
                BasicUI.SendMessageToGame("你已跳过游戏", "但是还需要你随便点一个方块");

            }).SetLayoutElement(40, 30);

            CardMatch1.CreateButton("跳过高考", () =>
            {
                MinGameCheat.跳过高考 = true;
            }).SetLayoutElement(40, 30);

            Text title4 = UIFactory.CreateLabel(tabGroup, "Title", "其他小游戏设置：不是对战情况下的不能跳过", TextAnchor.MiddleLeft, default, true, ZGScriptTrainer.FontSize.Value);
            UIFactory.SetLayoutElement(title.gameObject, minWidth: 75, flexibleWidth: 0);
            tabGroup.CreateSplitPanel(Color.white);

            GameObject AnotherGame = UIFactory.CreateHorizontalGroup(tabGroup, "MainWindowconnection", false, false, true, true, 5, default,
              new Color(0.065f, 0.065f, 0.065f), TextAnchor.MiddleLeft);
            UIFactory.SetLayoutElement(AnotherGame, minHeight: 40, flexibleWidth: 9999);

            AnotherGame.CreateButton("跳过篮球游戏", () =>
            {
                MinGameCheat.跳过篮球小游戏 = true;
            }).SetLayoutElement(40, 30);

            AnotherGame.CreateButton("跳过讨价还价（在开始游戏后再跳过）", () =>
            {
                MinGameCheat.跳过讨价还价 = true;
            }).SetLayoutElement(40, 30);

            AnotherGame.CreateButton("跳过拼图游戏", () =>
            {
                MinGameCheat.跳过拼图游戏 = true;
            }).SetLayoutElement(40, 30);

            GameObject AnotherGame1 = UIFactory.CreateHorizontalGroup(tabGroup, "MainWindowAnotherGame1", false, false, true, true, 5, default,
            new Color(0.065f, 0.065f, 0.065f), TextAnchor.MiddleLeft);
            UIFactory.SetLayoutElement(AnotherGame1, minHeight: 40, flexibleWidth: 9999);
            AnotherGame1.CreateButton("跳过钢琴游戏", () =>
            {
                MinGameCheat.跳过钢琴游戏 = true;
            }).SetLayoutElement(40, 30);

            AnotherGame1.CreateButton("跳过数独游戏", () =>
            {
                MinGameCheat.跳过数独游戏 = true;
            }).SetLayoutElement(40, 30);

            AnotherGame1.CreateButton("跳过打架游戏", () =>
            {
                MinGameCheat.跳过打架游戏 = true;
            }).SetLayoutElement(40, 30);

            AnotherGame1.CreateButton("跳过灵感游戏", () =>
            {
                MinGameCheat.跳过灵感游戏 = true;
            }).SetLayoutElement(40, 30);

            AnotherGame1.CreateButton("跳过戳指缝游戏", () =>
            {
                MinGameCheat.跳过戳指缝游戏 = true;
            }).SetLayoutElement(40, 30);

            GameObject AnotherGame2 = UIFactory.CreateHorizontalGroup(tabGroup, "MainWindowAnotherGame2", false, false, true, true, 5, default,
            new Color(0.065f, 0.065f, 0.065f), TextAnchor.MiddleLeft);
            UIFactory.SetLayoutElement(AnotherGame2, minHeight: 40, flexibleWidth: 9999);
            AnotherGame2.CreateButton("跳过羽毛球游戏", () =>
            {
                MinGameCheat.跳过羽毛球游戏 = true;
            }).SetLayoutElement(40, 30);
            AnotherGame2.CreateButton("速点小游戏（除害虫，堵水管，长跑简陋版）", () =>
            {
                MinGameCheat.跳过修下水管道游戏 = true;
            }).SetLayoutElement(40, 30);
            
            GameObject AnotherGame3 = UIFactory.CreateHorizontalGroup(tabGroup, "MainWindowAnotherGame2", false, false, true, true, 5, default,
            new Color(0.065f, 0.065f, 0.065f), TextAnchor.MiddleLeft);
            UIFactory.SetLayoutElement(AnotherGame3, minHeight: 40, flexibleWidth: 9999);


            AnotherGame3.CreateButton("跳过抢红包游戏", () =>
            {
                MinGameCheat.跳过抢红包游戏 = true;
            }).SetLayoutElement(40, 30);

            AnotherGame3.CreateButton("跳过打砖块游戏", () =>
            {
                MinGameCheat.跳过打砖块游戏 = true;
            }).SetLayoutElement(40, 30);

            AnotherGame3.CreateButton("跳过造句游戏", () =>
            {
                MinGameCheat.跳过造句游戏 = true;
            }).SetLayoutElement(40, 30);

            AnotherGame3.CreateButton("跳过手工游戏", () =>
            {
                MinGameCheat.跳过手工游戏 = true;
            }).SetLayoutElement(40, 30);

            UIFactory.CreateLabel(tabGroup, "Label", "百人答题秒完成看视频来打"
            , fontSize: ZGScriptTrainer.FontSize.Value).SetLayoutElement(20, 40);
            GameObject AnotherGame4 = UIFactory.CreateHorizontalGroup(tabGroup, "MainWindowAnotherGame4", false, false, true, true, 5, default,
            new Color(0.065f, 0.065f, 0.065f), TextAnchor.MiddleLeft);
            UIFactory.SetLayoutElement(AnotherGame4, minHeight: 40, flexibleWidth: 9999);

            AnotherGame4.CreateToggle("百人答题秒完成", (state) => { MinGameCheat.百人答题下一题 = state; });
            // default active state: Active

            this.SetActive(true);
        }

    }
}
