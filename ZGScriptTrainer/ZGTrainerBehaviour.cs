using System.Reflection;
using UnityEngine;
using UniverseLib.Input;
using UniverseLib;
using ZGScriptTrainer.UI.Panels;
using ZGScriptTrainer.UI;
using System.Collections;
using System;
using UnityEngine.Networking;
using System.Collections.Generic;
using ZGScriptTrainer.UI.Models;
using Config;
using Sdk;
using ZGScriptTrainer.Hook;
using ZGScriptTrainer.Cheat;
using UnityEngine.UI;
using System.Security.Policy;
using ZGScriptTrainer.Load;
using TheEntity;
using ZGScriptTrainer.ItemSpawn;
using System.IO;
using ZGScriptTrainer.FrendSet;

#if IL2CPP_6E
using Il2CppInterop.Runtime.Injection;
#elif IL2CPP_6
using UnhollowerRuntimeLib;
#endif

namespace ZGScriptTrainer
{
    public class ZGTrainerBehaviour : MonoBehaviour
    {
        internal static ZGTrainerBehaviour Instance { get; private set; }
        public static Dictionary<string, Action> Actions { get; set; } = new Dictionary<string, Action>();

#if CPP
        public ZGTrainerBehaviour(System.IntPtr ptr) : base(ptr) { }
#endif

        internal static void Setup()
        {
#if CPP
            ClassInjector.RegisterTypeInIl2Cpp<ZGTrainerBehaviour>();
#endif
            var BaseGameObject = new GameObject("ZGTrainerBehaviour");
            DontDestroyOnLoad(BaseGameObject);
            BaseGameObject.hideFlags = HideFlags.HideAndDontSave;
            Instance = BaseGameObject.AddComponent<ZGTrainerBehaviour>();
        }
        public static Dictionary<int, ItemCfg> Getkvp;

        private static string i = "";
        public static Dictionary<int, Sprite> imageDic;
        internal void Update()
        {
            if (InputManager.GetKeyDown(KeyCode.BackQuote) && Singleton<RoleMgr>.Ins.GetRole()!=null)
            DebugMgr.Show();
            //LoadFile.LoadAsync<ItemCfg>("Cfgs/" + LocalizationMgr.Lang + "/ItemCfg", delegate (Dictionary<int, ItemCfg> _t)
            //{
            //    Getkvp = _t;
            //});
            //foreach (var kvp in Getkvp.Values)
            //{
            //    if (Singleton<RoleMgr>.Ins.GetRole() != null)
            //    {
            //        UISprite Sprite = new UISprite();
            //        Sprite.SetAtlasUrl("item/djj");
            //        Debug.Log("ID：" + kvp.id.ToString() + "\t名字" + kvp.GetItemIcon);
            //    }

            //}
            //+"\t介绍：" + kvp. + "\t其他：" + kvp.icon

            if (Singleton<RoleMgr>.Ins.GetRole() != null)
                HookCheat.AllCheatForUICheat();


            if (OptionWindow.ShowKey == ZGScriptTrainer.ShowCounter.Value || OptionWindow.RestShowWindow && !OptionWindow.Bonce)
            {
                UIManager.OnShowCounterValueChanged(UIManager.MycloseBtn, UIManager.navbarPanelObject);
                OptionWindow.Bonce = true;
                OptionWindow.RestShowWindow = false;
            }
            if (InputManager.GetKeyDown(ZGScriptTrainer.ShowCounter.Value))
            {
                UIManager.ShowMenu = !UIManager.ShowMenu;
            }
            foreach (var action in Actions)
            {
                action.Value.Invoke();
            }      
        }
        internal void OnDestroy()
        {
            OnApplicationQuit();
        }

        internal void DelUpdateAction(string actionName)
        {
            if(Actions.ContainsKey(actionName)) 
            {
                Actions.Remove(actionName);
            }
        }
        internal void AddUpdateAction(string actionName, Action action)
        {
            if (!Actions.ContainsKey(actionName))
            {
                Actions.Add(actionName, action);
            }
        }
        internal bool quitting;
        internal void OnApplicationQuit()
        {
            if (quitting) return;
            quitting = true;

            TryDestroy(UIManager.UIRoot?.transform.root.gameObject);

            TryDestroy((typeof(Universe).Assembly.GetType("UniverseLib.UniversalBehaviour")
                .GetProperty("Instance", BindingFlags.Static | BindingFlags.NonPublic)
                .GetValue(null, null)
                as Component).gameObject);

            TryDestroy(this.gameObject);
        }
        internal void TryDestroy(GameObject obj)
        {
            try
            {
                if (obj)
                    Destroy(obj);
            }
            catch { }
        }      
    }
}
