using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using ZGScriptTrainer.FriendSet;
using ZGScriptTrainer.MyManagerSet;

namespace ZGScriptTrainer.TheFriend
{
    internal class FriendDropDownSet
    {
        public static List<int> FriendRelationidList = new List<int>();
        public static int FriendRelationID;
        public static string FriendRelationName;

        public static int FriendGiftTypeID;
        public static string FriendGiftTypeName;
        public static List<int> FriendGiftTypeList = new List<int>();
        public static List<GameObject> CurrentFrinedRelation = new List<GameObject>();
        public static void DropDownFriendRelationChange(int index, GameObject tabGroup, Dropdown FriendRelationTypeDown)
        {
            // 获取当前选中的选项文本
            if (index >= 0 && index < FriendRelationidList.Count)
            {
                FriendRelationID = FriendRelationidList[index];
                FriendRelationName = FriendRelationTypeDown.options[index].text;
                // 在这里处理选中的 id
                Debug.Log("选中的关系 ID: " + FriendRelationID + "\n关系名称：" + FriendRelationName);
            }
        }
    }
}
