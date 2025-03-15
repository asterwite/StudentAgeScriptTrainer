using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
#if IL2CPP_6E
using BepInEx.Unity.IL2CPP;
using Il2CppInterop.Runtime.Injection;
#elif IL2CPP_6
using BepInEx.IL2CPP;
using UnhollowerRuntimeLib;
#endif

using System;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using UniverseLib;
using ZGScriptTrainer.UI;
using ZGScriptTrainer.UI.Models;

namespace ZGScriptTrainer
{
    public static class ZGBepInExInfo
    {
        public const string PLUGIN_GUID = "ScriptTrainer.SchoolDays";

        public const string PLUGIN_NAME = "学生时代内置修改器";

        public const string For_Name = "By：无云五蕴";

        public const string PLUGIN_VERSION = "1.4";     
    }


    [BepInPlugin(ZGBepInExInfo.PLUGIN_GUID, ZGBepInExInfo.PLUGIN_NAME, ZGBepInExInfo.PLUGIN_VERSION)]
    public class ZGScriptTrainer :
#if MONO
        BaseUnityPlugin
#else
        BasePlugin
#endif
    {
        public static ZGScriptTrainer Instance;
        public ManualLogSource LogSource
#if MONO
            => Logger;
#else
            => Log;
#endif
        const string IL2CPP_LIBS_FOLDER =
#if UNHOLLOWER
            "unhollowed";
#else
            "interop";
#endif
        public string UnhollowedModulesFolder => Path.Combine(Paths.BepInExRootPath, IL2CPP_LIBS_FOLDER);
        public static Harmony Harmony { get; } = new Harmony(ZGBepInExInfo.PLUGIN_GUID);
        #region[配置]
        public static ConfigEntry<KeyCode> ShowCounter { get; set; }
        public static ConfigEntry<float> StartDelay { get; set; }
        public static ConfigEntry<int> FontSize { get; set; }
        public static ConfigEntry<string> PanelPosition { get; set; }
        #endregion
        public static void WriteLog(object message, LogType logType = LogType.Log)
        {
            ZGScriptTrainer.Instance.ILog(message, logType);
        }
        public void ILog(object message, LogType logType = LogType.Log)
        {
            string text = (message?.ToString()) ?? "";
            switch (logType)
            {
                case LogType.Error:
                case LogType.Exception:
                    LogSource.LogError(text);
                    break;
                case LogType.Assert:
                case LogType.Log:
                    LogSource.LogMessage(text);
                    break;
                case LogType.Warning:
                    LogSource.LogWarning(text);
                    break;
            }
        }

        private void Init()
        {
            Instance = this;

            #region[注入游戏补丁]
            Hook.Hook.HarmonyHook();
            #endregion

            #region 读取游戏配置
            ShowCounter = Config.Bind("修改器初始设置", "快捷键", KeyCode.Insert);
            StartDelay = Config.Bind("修改器初始设置", "启动延迟时间", 1f);
            FontSize = Config.Bind("修改器初始设置", "字体大小", 17);
            PanelPosition = Config.Bind("界面保存配置", "界面位置", "");
            #endregion
            Universe.Init(ZGScriptTrainer.StartDelay.Value, new Action(LateInit), new Action<string, LogType>(ILog), new UniverseLib.Config.UniverseLibConfig()
            {
                Force_Unlock_Mouse = true,
                Unhollowed_Modules_Folder = UnhollowedModulesFolder,
                Disable_EventSystem_Override = false,
                Allow_UI_Selection_Outside_UIBase = true,
            });

            ILog($"正在注入修改器...");
            ZGTrainerBehaviour.Setup();
        }
        private static int i = 0;
        private void LateInit()
        {
            //ILog($"正在获取物品数据...");
            //ZGItemUtil.GetBaseItemData();

            ILog($"正在生成UI界面...");
            UIManager.InitUI();

            ILog($"修改器初始化完成！");
        }

#if MONO // Mono
        internal void Awake()
        {
            Init();
        }

#else   // Il2Cpp
        public override void Load()
        {
            ClassInjector.RegisterTypeInIl2Cpp<TooltipGUI>();
            Init();
        }
#endif
    }
}
