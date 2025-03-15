using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.UI;
using UnityEngine;
using UniverseLib.UI.Widgets.ScrollView;
using UniverseLib.UI;
using UniverseLib.UI.Panels;
using ZGScriptTrainer.UI.Panels;
namespace ZGScriptTrainer.MyManagerSet
{
    internal class ScrollPoolUI
    {
        public GameObject DrawscrollPool(GameObject ContentRoot)
        {
            // 创建滚动池并获取ScrollRect组件
            var scrollPool = UIFactory.CreateScrollPool<ICell>(
                parent: ContentRoot,
                name: "MainScrollPool",
                uiRoot: out GameObject scrollRoot,
                content: out GameObject scrollContent,
                bgColor: new Color(0.12f, 0.12f, 0.12f)
            );

            // 设置滚动池布局
            UIFactory.SetLayoutElement(scrollRoot,
                flexibleWidth: 9999,
                flexibleHeight: 1000);

            // 获取ScrollRect组件
            var scrollRect = scrollRoot.GetComponent<ScrollRect>();
            if (scrollRect == null)
            {
                Debug.LogError("ScrollRect component not found on scrollRoot.");
                return null;
            }

            // 设置滚动方向为垂直
            scrollRect.vertical = true;
            scrollRect.horizontal = false;

            // 创建并配置滚动条
            var scrollbarObj = UIFactory.CreateScrollbar(scrollRoot, "Scrollbar", out Scrollbar scrollbar);
            if (scrollbar == null)
            {
                Debug.LogError("Scrollbar component not found on scrollbarObj.");
                return null;
            }

            // 设置滚动条方向为从上到下
            scrollbar.direction = Scrollbar.Direction.TopToBottom;

            // 滚动条定位配置
            RectTransform scrollbarRect = scrollbarObj.GetComponent<RectTransform>();
            if (scrollbarRect == null)
            {
                Debug.LogError("RectTransform component not found on scrollbarObj.");
                return null;
            }

            scrollbarRect.anchorMin = new Vector2(1, 1);
            scrollbarRect.anchorMax = new Vector2(1, 1);
            scrollbarRect.pivot = new Vector2(1, 0.5f);
            scrollbarRect.sizeDelta = new Vector2(20, 0); // 固定宽度20像素
            scrollbarRect.anchoredPosition = Vector2.zero;

            // 关联滚动条到ScrollRect
            scrollRect.verticalScrollbar = scrollbar;
            scrollRect.verticalScrollbarVisibility = ScrollRect.ScrollbarVisibility.Permanent;

            // 设置滚动条样式，模拟Dropdown滚动条样式
            // 设置滚动条背景颜色
            Image scrollbarBackground = scrollbarObj.GetComponentInChildren<Image>();
            if (scrollbarBackground != null)
            {
                scrollbarBackground.color = new Color(0.2f, 0.2f, 0.2f); // 类似Dropdown滚动条背景颜色
            }

            // 设置滚动条手柄颜色
            if (scrollbar.handleRect != null)
            {
                Image scrollbarHandle = scrollbar.handleRect.GetComponent<Image>();
                if (scrollbarHandle != null)
                {
                    scrollbarHandle.color = new Color(0.5f, 0.5f, 0.5f); // 类似Dropdown滚动条手柄颜色
                }
            }

            // 设置 Scrollbar 的布局元素
            UIFactory.SetLayoutElement(scrollbar.gameObject, minWidth: 20, flexibleWidth: 0);

            // 设置滚动条交互效果，模拟Dropdown滚动条交互
            scrollRect.inertia = true; // 启用惯性滚动，类似Dropdown滚动条的效果
            scrollRect.decelerationRate = 0.135f; // 设置减速速率，类似Dropdown滚动条的减速效果

            // 确保ScrollRect的content设置正确
            scrollRect.content = scrollContent.GetComponent<RectTransform>();

            return scrollContent;
        }

    }
}
