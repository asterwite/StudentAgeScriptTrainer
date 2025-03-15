using UnityEngine;
using UnityEngine.UI;
using UniverseLib.Input;
using UniverseLib.UI;
using ZGScriptTrainer.UI.Panels;
namespace ZGScriptTrainer.UI.Models
{
    public class TooltipGUI : MonoBehaviour
    {
#if CPP
        public TooltipGUI(System.IntPtr ptr) : base(ptr) { }
#endif
        public bool EnableTooltip
        {
            get
            {
                return Background?.activeSelf ?? false;
            }
            set
            {
                if (Background != null && Background.activeSelf != value)
                    Background.SetActive(value);
            }
        }
        public Vector2 Size
        {
            get
            {
                if (Background != null)
                    return Background.GetComponent<RectTransform>().sizeDelta;
                else return Vector2.zero;
            }
        }
        public static readonly Color DefaultBackgroundColor = new Color(0.45f, 0.45f, 0.45f, 1f);
        public static readonly Color DefaultTextColor = new Color(1f, 1f, 1f, 1f);
        public string Tooltip = string.Empty;
        public Text TooltipText = null;
        public GameObject Background = null;

        private RectTransform ParentPanel = null;
        public void Initialize(string text,  GameObject Parent = null, GameObject ToolRoot = null, Color backgroundColor = default, Color textColor = default)
        {
            if(Parent != null)
                ParentPanel = Parent.GetComponent<RectTransform>();
            Tooltip = text;
            if(ToolRoot == null)
                Background = UIFactory.CreateUIObject("GridUIRoot", UIManager.UIToolTip, new Vector2(200, 60));
            else
                Background = UIFactory.CreateUIObject("GridUIRoot", base.gameObject, new Vector2(200, 60));
            Background.AddComponent<Image>().color = backgroundColor == default ? DefaultBackgroundColor : backgroundColor;
            TooltipText = UIFactory.CreateLabel(Background, "TooltipText", Tooltip, TextAnchor.MiddleLeft, textColor, true, ZGScriptTrainer.FontSize.Value - 1);
            int size = 40;
            TooltipText.color = textColor == default ? DefaultTextColor : textColor;
            Tooltip = text;
            TooltipText.text = text;
            TooltipText.GetComponent<RectTransform>().sizeDelta = new Vector2(150, size);
            size = (int)(TooltipText.preferredHeight + 10);
            TooltipText.GetComponent<RectTransform>().sizeDelta = new Vector2(150, size);
            TooltipText.SetLayoutElement(minHeight: size, minWidth: 160, flexibleHeight: 0, flexibleWidth: 0);
            Background.GetComponent<RectTransform>().sizeDelta = new Vector2(160, TooltipText.preferredHeight + 20);
            UIFactory.SetLayoutElement(Background, minHeight: (int?)(TooltipText.GetComponent<RectTransform>().sizeDelta.x + 10), minWidth: 160, flexibleHeight: 0, flexibleWidth: 0);
            EnableTooltip = false;
        }
        public void SetText(string text)
        {
            Tooltip = text;
            ZGScriptTrainer.WriteLog(text);
            TooltipText.text = text;
            int size = 40;
            TooltipText.GetComponent<RectTransform>().sizeDelta = new Vector2(150, size);
            size = (int)(TooltipText.preferredHeight + 10);
            TooltipText.GetComponent<RectTransform>().sizeDelta = new Vector2(150, size);
            TooltipText.SetLayoutElement(minHeight: size, minWidth: 160, flexibleHeight: 0, flexibleWidth: 0);
            Background.GetComponent<RectTransform>().sizeDelta = new Vector2(160, TooltipText.preferredHeight + 20);
            UIFactory.SetLayoutElement(Background, minHeight: (int?)(TooltipText.GetComponent<RectTransform>().sizeDelta.x + 10), minWidth: 160, flexibleHeight: 0, flexibleWidth: 0);
        }
        public void Update()
        {
            var mousepos = InputManager.MousePosition;
            if (Background != null && !string.IsNullOrEmpty(Tooltip))
            {
                if (ParentPanel == null)
                {
                    Background.transform.position = new Vector3(mousepos.x + Size.x / 2 + 10, mousepos.y - Size.y / 2 - 10, 0);
                    return;
                }
                if (RectTransformUtility.RectangleContainsScreenPoint(gameObject.GetComponent<RectTransform>(), mousepos) && ContainParent(mousepos))
                {
                    if (!EnableTooltip)
                    {
                        EnableTooltip = true;
                    }
                    Background.transform.position = new Vector3(mousepos.x + Size.x / 2 + 10, mousepos.y - Size.y / 2 - 10, 0);
                    return;
                }
            }
            EnableTooltip = false;
        }
        public void OnDestroy()
        {
            Destroy(Background);
            Destroy(gameObject);
        }
        public bool ContainParent(Vector3 mousepos)
        {
            Vector2 size = ParentPanel.sizeDelta;
            if (mousepos.x < ParentPanel.position.x + size.x / 2 && mousepos.x > ParentPanel.position.x - size.x / 2)
            {
                if (mousepos.y < ParentPanel.position.y + size.y / 2 && mousepos.y > ParentPanel.position.y - size.y / 2)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
