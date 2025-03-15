using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UniverseLib.UI;
using ZGScriptTrainer.UI;
using UnityEngine.UI;
using Sdk;
using Config;
using ZGScriptTrainer.Cheat;
using ZGScriptTrainer.UI.Panels;
using ZGScriptTrainer.FriendSet;
namespace ZGScriptTrainer.MainUI
{

    internal class BasicUI
    {
        public static Dictionary<int, PersonAttrCfg> AttrSet;
        public static Dictionary<int, PersonCfg> ForTheFriendSet;
        public static int is精力;
        public static int is动力;
        public static int is心情;
        public static int is热情;
        public static int is体魄;
        public static int is智力;
        public static int is情商;
        public static int is成就;
        public static int is阅历;
        public static int is信任;
        public static bool 无限精力 = false;
        public static bool 无限动力 = false;
        public static bool 无限心情 = false;
        public static bool 无限热情 = false;
        public static bool 无限精力Bonce = false;
        public static bool 无限动力Bonce = false;
        public static bool 无限心情Bonce = false;
        public static bool 无限热情Bonce = false;

        public static bool 赠送礼物无限制 = false;
        public static void SendMessageToGame(string LeftName, string RightName)
        {
            ToastHelper.Toast<string>(215, new string[]
              {
                     LeftName,
                     RightName
                  //HtmlTxtUtil.ToStr(realAddFavor, "{0:0.#}", 0, false)
              });
        }
        public static void BasicCheat()
        {

                #region[精力]
                if (无限精力)
                {
                    float MaxAttr = Singleton<RoleMgr>.Ins.GetRole().GetAttrMax(is精力);
                    Singleton<RoleMgr>.Ins.GetRole().SetAttr(is精力, MaxAttr);
                    if (!无限精力Bonce)
                    {
                        SendMessageToGame("启动了无限精力", "启动无限模式");
                        无限精力Bonce = true;
                    }
                }
                if(!无限精力&& 无限精力Bonce)
                {
                    无限精力Bonce = false;
                    SendMessageToGame("关闭了无限精力", "关闭无限模式");
                }
                #endregion

                #region[动力]
                if (无限动力)
                {
                    Singleton<RoleMgr>.Ins.GetRole().SetAttr(is动力, 99);
                    if (!无限动力Bonce)
                    {
                        SendMessageToGame("启动了无限动力", "启动无限模式");
                        无限动力Bonce = true;
                    }
                }
                if(!无限动力&& 无限动力Bonce)
                {
                    无限动力Bonce = false;
                    SendMessageToGame("关闭了无限动力", "关闭无限模式");
                }
            #endregion

                #region[心情]
                 if (无限心情)
                 {
                    float MaxAttr = Singleton<RoleMgr>.Ins.GetRole().GetAttrMax(is心情);
                    Singleton<RoleMgr>.Ins.GetRole().SetAttr(is心情, MaxAttr);
                    if (!无限心情Bonce)
                    {
                        SendMessageToGame("启动了无限心情", "启动无限模式");
                        无限心情Bonce = true;
                    }
                }
                if (!无限心情 && 无限心情Bonce)
                {
                    无限心情Bonce = false;
                    SendMessageToGame("关闭了无限心情", "关闭无限模式");
                }
            #endregion

                #region[热情]
                if (无限热情)
                {
                    float MaxAttr = Singleton<RoleMgr>.Ins.GetRole().GetAttrMax(is心情);
                    Singleton<RoleMgr>.Ins.GetRole().SetAttr(is热情, MaxAttr);
                    if (!无限热情Bonce)
                    {
                        SendMessageToGame("启动了无限热情", "启动无限模式");
                        无限热情Bonce = true;
                    }
                }
                if (!无限热情 && 无限热情Bonce)
                {
                    无限热情Bonce = false;
                    SendMessageToGame("关闭了无限热情", "关闭无限模式");
                }
            #endregion

        }


        public static void DrawBasicUI(GameObject tabGroup)
        {
            Text title = UIFactory.CreateLabel(tabGroup, "Title", "基础功能：如果要精力上限提升，你开了无限系列，你需要关闭全部的无限系列，如果不关闭升级调用会冲突，导致无法升级\n" +
                "呼出游戏内置控制台菜单快捷键~", TextAnchor.MiddleLeft, default, true, ZGScriptTrainer.FontSize.Value);
            UIFactory.SetLayoutElement(title.gameObject, minWidth: 75, flexibleWidth: 0);
            tabGroup.CreateSplitPanel(Color.white);

            GameObject Time = UIFactory.CreateHorizontalGroup(tabGroup, "MainWindowTime", false, false, true, true, 5, default,
              new Color(0.065f, 0.065f, 0.065f), TextAnchor.MiddleLeft);
            UIFactory.SetLayoutElement(Time, minHeight: 40, flexibleWidth: 9999);


            Time.CreateInputLeftEditButtonHasTwo("2001", "年", "1", "月", "设置", new Action<string, string>((string text, string text1) =>
            {
                int Year = text.ConvertToIntDef(2001);
                int Month = text1.ConvertToIntDef(1);
                Singleton<RoundMgr>.Ins.SetTime(Year, Month);
                string LeftName = "已经将时间设置为" + Year.ToString() + "年" + Month.ToString() + "月";
                string RightName = "时间是逆转的公式吗？";
                SendMessageToGame(LeftName, RightName);
            }));

            Time.CreateInputEditButton("添加多少金钱", "100", "添加", new Action<string>((string text) =>
            {
                float AddMoney = text.ConvertToFloatDef(100);
                Singleton<RoleMgr>.Ins.GetRole().UpdateAttr(30, AddMoney);
                string LeftName = "已经添加了" + AddMoney.ToString() + "元";
                string RightName = "我刷钱了孩子们";
                SendMessageToGame(LeftName, RightName);
            }));

            GameObject AttrUISet = UIFactory.CreateHorizontalGroup(tabGroup, "MainWindowAttribute", false, false, true, true, 5, default,
            new Color(0.065f, 0.065f, 0.065f), TextAnchor.MiddleLeft);
            UIFactory.SetLayoutElement(AttrUISet, minHeight: 40, flexibleWidth: 9999);

            AttrUISet.CreateToggle("无限精力", (state) => { 无限精力 = state; });
            AttrUISet.CreateToggle("无限心情", (state) => { 无限心情 = state; });
            AttrUISet.CreateToggle("无限动力", (state) => { 无限动力 = state; });
            AttrUISet.CreateToggle("无限热情", (state) => { 无限热情 = state; });
            AttrUISet.CreateToggle("赠送礼物无限制", (state) => { 赠送礼物无限制 = state; });
            GameObject Attribute1 = UIFactory.CreateHorizontalGroup(tabGroup, "MainWindowAttribute1", false, false, true, true, 5, default,
            new Color(0.065f, 0.065f, 0.065f), TextAnchor.MiddleLeft);
            UIFactory.SetLayoutElement(Attribute1, minHeight: 40, flexibleWidth: 9999);

            Attribute1.CreateInputEditButton("添加信任", "10", "添加", new Action<string>((string text) =>
            {
                float Addtrust = text.ConvertToFloatDef(10);
                Singleton<RoleMgr>.Ins.GetRole().UpdateAttr(is信任, Addtrust);
                string LeftName = "添加了" + Addtrust.ToString() + "信任";
                string RightName = "你对我的不信任，是对我最大的惩罚";
                SendMessageToGame(LeftName, RightName);
            }));

            Attribute1.CreateInputEditButton("添加阅历", "10", "添加", new Action<string>((string text) =>
            {
                float Addexperience = text.ConvertToFloatDef(10);
                Singleton<RoleMgr>.Ins.GetRole().UpdateAttr(is阅历, Addexperience);
                string LeftName = "添加了" + Addexperience.ToString() + "阅历";
                string RightName = "我的人生阅历还不够，我要继续向前冲";
                SendMessageToGame(LeftName, RightName);
            }));

            Attribute1.CreateInputEditButton("添加成就", "10", "添加", new Action<string>((string text) =>
            {
                float Addachievement = text.ConvertToFloatDef(10);
                Singleton<RoleMgr>.Ins.GetRole().UpdateAttr(is成就, Addachievement);
                string LeftName = "添加了" + Addachievement.ToString() + "成就";
                string RightName = "我的成就并不突出，但每一次点击都在让我突出";
                SendMessageToGame(LeftName, RightName);
            }));

            GameObject capacity = UIFactory.CreateHorizontalGroup(tabGroup, "MainWindowcapacity", false, false, true, true, 5, default,
              new Color(0.065f, 0.065f, 0.065f), TextAnchor.MiddleLeft);
            UIFactory.SetLayoutElement(capacity, minHeight: 40, flexibleWidth: 9999);

            capacity.CreateInputEditButton("添加智力", "10", "添加", new Action<string>((string text) =>
            {
                float Addintelligence = text.ConvertToFloatDef(10);
                Singleton<RoleMgr>.Ins.GetRole().UpdateAttr(is智力, Addintelligence);
                string LeftName = "添加了" + Addintelligence.ToString() + "智力";
                string RightName = "我很蠢吗？不！其实我很聪明";
                SendMessageToGame(LeftName, RightName);
            }));

            capacity.CreateInputEditButton("添加情商", "10", "添加", new Action<string>((string text) =>
            {
                float AddEQ = text.ConvertToFloatDef(10);
                Singleton<RoleMgr>.Ins.GetRole().UpdateAttr(is情商, AddEQ);
                string LeftName = "添加了" + AddEQ.ToString() + "情商";
                string RightName = "情商是个令人操心的问题，但今天不是问题了";
                SendMessageToGame(LeftName, RightName);
            }));

            capacity.CreateInputEditButton("添加体魄", "10", "添加", new Action<string>((string text) =>
            {
                float Addphysique = text.ConvertToFloatDef(10);
                Singleton<RoleMgr>.Ins.GetRole().UpdateAttr(is体魄, Addphysique);
                string LeftName = "添加了" + Addphysique.ToString() + "体魄";
                string RightName = "玩游戏怎么能没有一个强健的体魄呢，力来！！！";
                SendMessageToGame(LeftName, RightName);
            }));

            GameObject capacity1 = UIFactory.CreateHorizontalGroup(tabGroup, "MainWindowcapacity1", false, false, true, true, 5, default,
              new Color(0.065f, 0.065f, 0.065f), TextAnchor.MiddleLeft);
            UIFactory.SetLayoutElement(capacity1, minHeight: 40, flexibleWidth: 9999);

            capacity1.CreateInputEditButton("添加跑步经验", "8", "添加", new Action<string>((string text) =>
            {
                float AddRun = text.ConvertToFloatDef(8);
                Singleton<SkillData>.Ins.AddExp(1, AddRun);
                string LeftName = "添加了" + AddRun.ToString() + "体能经验";
                string RightName = "每次的跑步之所以跑不动，一定是没有体能原因";
                SendMessageToGame(LeftName, RightName);
            }));

            capacity1.CreateInputEditButton("添加篮球经验", "8", "添加", new Action<string>((string text) =>
            {
                float AddBasket = text.ConvertToFloatDef(8);
                Singleton<SkillData>.Ins.AddExp(2, AddBasket);
                string LeftName = "添加了" + AddBasket.ToString() + "篮球经验";
                string RightName = "我就是球王在世！！！";
                SendMessageToGame(LeftName, RightName);
            }));

            capacity1.CreateInputEditButton("添加羽毛球经验", "8", "添加", new Action<string>((string text) =>
            {
                float Addbadminton = text.ConvertToFloatDef(8);
                Singleton<SkillData>.Ins.AddExp(3, Addbadminton);
                string LeftName = "添加了" + Addbadminton.ToString() + "羽毛球经验";
                string RightName = "我就是林丹！！！";
                SendMessageToGame(LeftName, RightName);
            }));
            capacity1.CreateInputEditButton("添加武术经验", "8", "添加", new Action<string>((string text) =>
            {
                float AddWuShu = text.ConvertToFloatDef(8);
                Singleton<SkillData>.Ins.AddExp(4, AddWuShu);
                string LeftName = "添加了" + AddWuShu.ToString() + "武术经验";
                string RightName = "什么马师父？叫我一代宗师";
                SendMessageToGame(LeftName, RightName);
            }));
            GameObject NPCSpecialData = UIFactory.CreateHorizontalGroup(tabGroup, "MainWindowcapacity1", false, false, true, true, 5, default,
            new Color(0.065f, 0.065f, 0.065f), TextAnchor.MiddleLeft);
            UIFactory.SetLayoutElement(NPCSpecialData, minHeight: 40, flexibleWidth: 9999);
            NPCSpecialData.CreateInputEditAddAndDelSetButton("添加谭梓君躁动值", "100", "添加","减少", new Action<string>((string text) =>
            {
                float AddJunChao = text.ConvertToFloatDef(100);
                if (Singleton<RoleMgr>.Ins.GetRole() != null)
                Singleton<RoleMgr>.Ins.GetNpcSpecialData().UpdateJunChaos(AddJunChao);

            }), new Action<string>((string text1)=>
            {
                float AddJunChao = text1.ConvertToFloatDef(100);
                if (Singleton<RoleMgr>.Ins.GetRole() != null)
                    Singleton<RoleMgr>.Ins.GetNpcSpecialData().UpdateJunChaos(-AddJunChao);
            }));

            NPCSpecialData.CreateInputEditAddAndDelSetButton("添加罗晓纯支持度", "100", "添加", "减少", new Action<string>((string text) =>
            {
                float AddChunSupport = text.ConvertToFloatDef(100);
                if (Singleton<RoleMgr>.Ins.GetRole() != null)
                    Singleton<RoleMgr>.Ins.GetNpcSpecialData().UpdateChunSupport(AddChunSupport);

            }), new Action<string>((string text1) =>
            {
                float AddChunSupport = text1.ConvertToFloatDef(100);
                if (Singleton<RoleMgr>.Ins.GetRole() != null)
                    Singleton<RoleMgr>.Ins.GetNpcSpecialData().UpdateChunSupport(-AddChunSupport);
            }));

            NPCSpecialData.CreateInputEditAddAndDelSetButton("给程良添加分数", "100", "添加", "减少", new Action<string>((string text) =>
            {
                float AddPont = text.ConvertToFloatDef(100);
                if (Singleton<RoleMgr>.Ins.GetRole() != null)
                    Singleton<RoleMgr>.Ins.GetNpcSpecialData().UpdateChengProgress(AddPont, 0);

            }), new Action<string>((string text1) =>
            {
                float AddPont = text1.ConvertToFloatDef(100);
                if (Singleton<RoleMgr>.Ins.GetRole() != null)
                    Singleton<RoleMgr>.Ins.GetNpcSpecialData().UpdateChengProgress(-AddPont, 0);
            }));
        }
    }
}
