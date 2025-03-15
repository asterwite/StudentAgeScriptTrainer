using System;
using UnityEngine.UI;
using UnityEngine;
using UniverseLib.UI.Models;
using UniverseLib.UI.Panels;
using UniverseLib.UI;
using UniverseLib;

namespace ZGScriptTrainer.UI.Panels
{
    public abstract class ZGPanel : PanelBase
    {
        protected ZGPanel(UIBase owner) : base(owner) { }
        public abstract UIManager.Panels PanelType { get; }
        public virtual bool ShowByDefault => false;
        public virtual bool NavButtonWanted => true;
        public ButtonRef NavButton { get; internal set; }
        public bool ApplyingSaveData { get; set; } = true;
        public override void SetActive(bool active)
        {
            if (this.Enabled != active)
            {
                base.SetActive(active);

                if (NavButtonWanted && NavButton != null)
                {
                    Color color = active ? UniversalUI.EnabledButtonColor : UniversalUI.DisabledButtonColor;
                    RuntimeHelper.SetColorBlock(NavButton.Component, color, color * 1.2f);
                }
            }

            if (!active)
            {
                if (Dragger != null)
                    this.Dragger.WasDragging = false;
            }
            else
            {
                this.UIRoot.transform.SetAsLastSibling();
                //this.Owner.Panels.InvokeOnPanelsReordered();
            }
        }
        public override void ConstructUI()
        {
            base.ConstructUI();

            if (NavButtonWanted)
            {
                // create navbar button

                NavButton = UIFactory.CreateButton(UIManager.NavbarTabButtonHolder, $"Button_{PanelType}", Name);
                GameObject navBtn = NavButton.Component.gameObject;
                navBtn.AddComponent<ContentSizeFitter>().horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
                UIFactory.SetLayoutGroup<HorizontalLayoutGroup>(navBtn, false, true, true, true, 0, 0, 0, 5, 5, TextAnchor.MiddleCenter);
                ZGUIUtility.SetLayoutElement(NavButton, minWidth: 80);

                RuntimeHelper.SetColorBlock(NavButton.Component, UniversalUI.DisabledButtonColor, UniversalUI.DisabledButtonColor * 1.2f);
                NavButton.OnClick += () => { UIManager.TogglePanel(PanelType); };

                GameObject txtObj = navBtn.transform.Find("Text").gameObject;
                txtObj.AddComponent<ContentSizeFitter>().horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
            }

            base.TitleBar.GetComponentInChildren<Text>().fontSize = ZGScriptTrainer.FontSize.Value;
            this.SetActive(true);
            this.SetActive(false);
            this.SetActive(ShowByDefault);
        }
        protected override void LateConstructUI()
        {
            ApplyingSaveData = true;
            base.SetDefaultSizeAndPosition();
            try
            {
                ZGPanelData data = ZGScriptTrainer.PanelPosition.Value.ConvertString2ZGPanelData(Enum.GetValues(typeof(UIManager.Panels)).Length)[(int)PanelType];
                if(data.Position != Vector2.zero)
                    Rect.localPosition = data.Position;
                Rect.pivot = new Vector2(0f, 1f);
                if (data.AnchorMin != Vector2.zero)
                    Rect.anchorMin = data.AnchorMin;
                if (data.AnchorMax != Vector2.zero)
                    Rect.anchorMax = data.AnchorMax;
                this.EnsureValidSize();
                this.EnsureValidPosition();
            }
            catch
            {

            }
            Dragger.OnEndResize();
            ApplyingSaveData = false;
        }
        public override void OnFinishDrag()
        {
            base.OnFinishDrag();
            SaveData();
        }
        public override void OnFinishResize()
        {
            base.OnFinishResize();
            SaveData();
        }
        public void SaveData()
        {
            if (UIManager.Initializing || ApplyingSaveData)
                return;
            var data = ZGScriptTrainer.PanelPosition.Value.ConvertString2ZGPanelData(Enum.GetValues(typeof(UIManager.Panels)).Length);
            data[(int)PanelType].Position = Rect.localPosition;
            data[(int)PanelType].AnchorMin = Rect.anchorMin;
            data[(int)PanelType].AnchorMax = Rect.anchorMax;
            ZGScriptTrainer.PanelPosition.Value = data.ConvertZGPanelData2String();
        }
    }
}
