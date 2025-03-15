using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.UI;
using UnityEngine;
using UniverseLib.UI.Models;
using UniverseLib.UI.Widgets.ScrollView;
using UniverseLib.UI;

namespace ZGScriptTrainer.UI.Cells
{
    public class FriendCell : ICell
    {
        public float DefaultHeight => 50f;
        public GameObject UIRoot { get; set; }
        public bool Enabled => m_enabled;
        private bool m_enabled;
        public RectTransform Rect { get; set; }

        public void Disable()
        {
            m_enabled = false;
            UIRoot.SetActive(false);
        }

        public void Enable()
        {
            m_enabled = true;
            UIRoot.SetActive(true);
        }

        public Text NameLabel;

        public LayoutElement NameLayout;

        public Text DescriptionLabel;

        public LayoutElement DescriptionLayout;

        public Image ItemImage;

        public Color PanelBackColor;

        public ButtonRef SubmitButton;

        public ButtonRef DelButton;

        public Dropdown TypeDropdown;
        public virtual GameObject CreateContent(GameObject parent)
        {
            //主界面

            UIRoot = UIFactory.CreateUIObject(this.GetType().Name, parent, new Vector2(190, 60));
            Rect = UIRoot.GetComponent<RectTransform>();
            UIFactory.SetLayoutGroup<HorizontalLayoutGroup>(UIRoot, false, false, true, true, spacing: 5, padTop: 5, padBottom: 5, padLeft: 5, padRight: 5, childAlignment: TextAnchor.MiddleLeft);
            UIFactory.SetLayoutElement(UIRoot, minWidth: 190, flexibleWidth: 9999, minHeight: 60, flexibleHeight: 600);
            UIRoot.AddComponent<ContentSizeFitter>().verticalFit = ContentSizeFitter.FitMode.PreferredSize;
            //PanelBackColor = UIRoot.AddComponent<Image>().color;
            //物品图片
            var ItemUI = UIFactory.CreateUIObject("ItemUI", UIRoot, new Vector2(50, 50));
            ItemImage = ItemUI.AddComponent<Image>();
            ItemImage.preserveAspect = true;
            UIFactory.SetLayoutElement(ItemUI, minHeight: 50, minWidth: 50, flexibleWidth:0,flexibleHeight: 0);

            NameLabel = UIFactory.CreateLabel(UIRoot, "NameLabel", "<notset>", TextAnchor.MiddleLeft, fontSize: ZGScriptTrainer.FontSize.Value);
            NameLabel.horizontalOverflow = HorizontalWrapMode.Wrap;
            NameLayout = ZGUIUtility.SetLayoutElement(NameLabel, minHeight: 25, minWidth: 40, flexibleHeight: 300, flexibleWidth: 2000);

            var space1 = UIFactory.CreateUIObject("space1", UIRoot, new Vector2(4, 50));
            space1.AddComponent<Image>().color = "#7D7D7D".HexToColor();
            UIFactory.SetLayoutElement(space1, minHeight: 25, minWidth: 4, flexibleHeight: 300, flexibleWidth: 0);

            DescriptionLabel = UIFactory.CreateLabel(UIRoot, "DescriptionLabel", "<notset>", TextAnchor.MiddleLeft, fontSize: 12);
            DescriptionLabel.horizontalOverflow = HorizontalWrapMode.Wrap;
            DescriptionLayout = ZGUIUtility.SetLayoutElement(DescriptionLabel, minHeight: 25, minWidth: 20, flexibleHeight: 300, flexibleWidth: 9999);

            SubmitButton = UIFactory.CreateButton(UIRoot, "SubmitButton", "添加", new Color(0.15f, 0.19f, 0.15f));
            ZGUIUtility.SetLayoutElement(SubmitButton, minWidth: 70, minHeight: 25, flexibleWidth: 0, flexibleHeight: 0);

            DelButton = UIFactory.CreateButton(UIRoot, "DelButton", "减少", new Color(0.15f, 0.19f, 0.15f));
            ZGUIUtility.SetLayoutElement(DelButton, minWidth: 70, minHeight: 25, flexibleWidth: 0, flexibleHeight: 0);

            var Drop = UIFactory.CreateUIObject("space1", UIRoot, new Vector2(4, 50));
            Drop.AddComponent<Image>().color = "#7D7D7D".HexToColor();
            UIFactory.SetLayoutElement(Drop, minHeight: 25, minWidth: 4, flexibleHeight: 300, flexibleWidth: 0);

            GameObject gameObject2 = UIFactory.CreateDropdown(Drop, "TypeDropdown", out TypeDropdown, "选择类型", ZGScriptTrainer.FontSize.Value, null, null);
            ZGUIUtility.SetLayoutElement(gameObject2, minWidth: 90, minHeight: 25, flexibleWidth: 0, flexibleHeight: 0);

            return UIRoot;
        }
    }
}
