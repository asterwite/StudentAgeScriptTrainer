using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.UI;
using UnityEngine;
using Sdk;
using ZGScriptTrainer.TheFriend;
using Mono.CSharp;
using ZGScriptTrainer.FriendSet;
using ZGScriptTrainer.Cheat;

namespace ZGScriptTrainer.FrendSet
{
    internal class FriendRelation
    {
        public  int FriendRelationID;
        public string RoleName = null;
        public  void DropDownFriendRelationChange(int index, Dropdown FriendRelationTypeDown,List<int> FriendRelationidList,ForFriend Friend)
        {
            // 获取当前选中的选项文本
            if (index >= 0 && index < FriendRelationidList.Count)
            {
                FriendRelationID = FriendRelationidList[index];
                string Name = FriendRelationTypeDown.options[index].text;
                Singleton<RoleMgr>.Ins.GetLoveData().breakfastId = 1;
                Singleton<RoleMgr>.Ins.GetRelationData().ChangeRelation(Friend.FriendID, FriendRelationID);
                // 在这里处理选中的 id
                Debug.Log("人物ID："+ Friend.FriendID + "\t人物名称："+Friend.FriendName + "\n当前关系 ID: " + FriendRelationID + "\t关系名称：" + Name);
            }
        }
        private  int lastSearchIndex = -1;
        public  void OnSearchInputChanged(string searchText, Dropdown FriendRelationTypeDown)
        {
            int startIndex = lastSearchIndex + 1; // 从上次匹配的下一个索引开始查找
            int nextMatchIndex = -1;

            for (int i = startIndex; i < FriendRelationTypeDown.options.Count; i++)
            {
                if (FriendRelationTypeDown.options[i].text.Contains(searchText))
                {
                    nextMatchIndex = i;
                    break;
                }
            }

            if (nextMatchIndex != -1)
            {
                FriendRelationTypeDown.value = nextMatchIndex;
                FriendRelationTypeDown.RefreshShownValue();
                lastSearchIndex = nextMatchIndex; // 更新上次匹配的索引
            }
            else
            {
                // 如果没有找到，从开头重新开始查找
                for (int i = 0; i < startIndex; i++)
                {
                    if (FriendRelationTypeDown.options[i].text.Contains(searchText))
                    {
                        nextMatchIndex = i;
                        FriendRelationTypeDown.value = nextMatchIndex;
                        FriendRelationTypeDown.RefreshShownValue();
                        lastSearchIndex = nextMatchIndex;
                        break;
                    }
                }
            }
        }
    }
}
