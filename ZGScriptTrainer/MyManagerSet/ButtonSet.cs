using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.EventSystems;
using UniverseLib.UI.Models;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ZGScriptTrainer.MyManagerSet
{
    internal class ButtonSet
    {
        public ButtonRef button;
        public Color Color1;
        public Color Color2;
        public Color lightGreen = new Color(0.6f, 1f, 0.6f);
        public Color lightBule = new Color(0.6f, 0.8f, 1f);
        public bool isButtonColor1 = true;
        public string Text1;
        public string Text2;

        public void EnableButtonCheckSet(ButtonRef setButton, Color FirstColor, Color NextColor, string FirstText, string NextText)
        {
            button = setButton;
            Color1 = FirstColor;
            Color2 = NextColor;
            Text1 = FirstText;
            Text2 = NextText;

            // 添加鼠标进入、离开和点击事件监听器
            EventTrigger trigger = button.GameObject.AddComponent<EventTrigger>();

            // 鼠标进入事件
            EventTrigger.Entry enterEntry = new EventTrigger.Entry();
            enterEntry.eventID = EventTriggerType.PointerEnter;
            enterEntry.callback.AddListener((eventData) => OnPointerEnter());
            trigger.triggers.Add(enterEntry);

            // 鼠标离开事件
            EventTrigger.Entry exitEntry = new EventTrigger.Entry();
            exitEntry.eventID = EventTriggerType.PointerExit;
            exitEntry.callback.AddListener((eventData) => OnPointerExit());
            trigger.triggers.Add(exitEntry);

            // 按钮点击事件
            button.Component.onClick.AddListener(OnButtonClick);
        }
        public  void OnPointerEnter()
        {
            // 鼠标进入时切换颜色
            SetButtonColor(isButtonColor1 ? Color2 : Color1);
        }

        public  void OnPointerExit()
        {
            // 鼠标离开时恢复原来颜色
            SetButtonColor(isButtonColor1 ? Color1 : Color2);
        }

        public  void OnButtonClick()
        {
            // 点击按钮时切换颜色
            isButtonColor1 = !isButtonColor1;
            SetButtonColor(isButtonColor1 ? Color1 : Color2);
            button.ButtonText.text = isButtonColor1 ? Text1 : Text2;
        }

        public  void SetButtonColor(Color color)
        {
            ColorBlock colorBlock = button.Component.colors;
            colorBlock.normalColor = color;
            colorBlock.highlightedColor = color;
            colorBlock.pressedColor = color;
            colorBlock.disabledColor = color;
            button.Component.colors = colorBlock;
        }
    }
}
