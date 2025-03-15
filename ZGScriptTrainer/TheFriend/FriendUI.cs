using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ZGScriptTrainer.TheFriend
{
    internal class FriendUI
    {
        public static List<GameObject> currentPageControls = new List<GameObject>();
        public static List<GameObject> CurrentItemDesControls = new List<GameObject>();
        public static void RemoveCurrentPageControls()
        {
            foreach (GameObject control in currentPageControls)
            {
                if (control != null)
                {
                    UnityEngine.Object.Destroy(control);
                }
            }
            currentPageControls.Clear();
        }
        public static void RemoveCurrentItemDesControls()
        {
            foreach (GameObject control in CurrentItemDesControls)
            {
                if (control != null)
                {
                    UnityEngine.Object.Destroy(control);
                }
            }
            CurrentItemDesControls.Clear();
        }
        public static void SwitchPage(int selectedIndex, GameObject tabGroup)
        {
            // 删除旧的控件
            RemoveCurrentPageControls();
            RemoveCurrentItemDesControls();
            if (selectedIndex == 1)
                FriendCheat.DrawFriendCheatUI(tabGroup);
            //if (selectedIndex == 1)
            //SpawnItem.SpawnSet.SpawnEquip.DrawSpawnEquipUI(tabGroup);
        }
    }
}
