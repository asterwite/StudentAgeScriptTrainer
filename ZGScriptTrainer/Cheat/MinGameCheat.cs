using Config;
using HarmonyLib;
using MiniGame.CardMatch;
using MiniGame.Negotiation;
using MiniGame.Piano;
using Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.PlayerLoop;
using ZGScriptTrainer.MainUI;
using ZGScriptTrainer.UI.Panels;

namespace ZGScriptTrainer.Cheat
{
    public class MinGameCheat
    {
        public static bool 添加了策略点 = false;
        public static int 添加策略点;
        public static bool 补充血量 = false;
        public static bool 快速说服对手 = false;
        public static bool 跳过对话 = false;

        public static bool 跳过方块匹配游戏复习 = false;
        public static bool 跳过方块匹配游戏英语 = false;
        public static bool 跳过计算游戏 = false;
        public static bool 跳过卡牌顺序游戏 = false;
        public static bool 跳过闭合电路游戏 = false;

        public static bool 跳过篮球小游戏 = false;
        public static bool 跳过填词小游戏 = false;
        public static bool 跳过期末考试 = false;
        public static bool 跳过高考 = false;
        public static bool 跳过讨价还价 = false;
        public static bool 跳过拼图游戏 = false;
        public static bool 跳过钢琴游戏 = false;
        public static bool 跳过数独游戏 = false;
        public static bool 跳过打架游戏 = false;
        public static bool 跳过灵感游戏 = false;
        public static bool 跳过戳指缝游戏 = false;
        public static bool 跳过羽毛球游戏 = false;
        public static bool 跳过修下水管道游戏 = false;

        public static bool 跳过抢红包游戏 = false;
        public static bool 跳过打砖块游戏 = false;
        public static bool 跳过造句游戏 = false;
        public static bool 跳过手工游戏 = false;


        public static bool 百人答题下一题 = false;

     

        [HarmonyPrefix]
        [HarmonyPatch(typeof(MiniGame.Exam.Exam2MiniGameView), nameof(MiniGame.Exam.Exam2MiniGameView.Update))]
        public static bool 高考Cheat(MiniGame.Exam.Exam2MiniGameView __instance)
        {
            if (跳过高考)
            {
                Vector3 vec = new Vector3();
                __instance.AddScore(4, 1000, vec);
                __instance.FinishExam();
                跳过高考 = false;
            }
            return true;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(MiniGame.Exam.ExamMiniGameView), nameof(MiniGame.Exam.ExamMiniGameView.RefreshDesc))]
        public static bool 考试Cheat(MiniGame.Exam.ExamMiniGameView __instance)
        {
            if (跳过期末考试)
            {
                Vector3 vec = new Vector3();
                __instance.AddScore(4, 1000, vec);
                __instance.FinishExam();
                跳过期末考试 = false;
            }
            return true;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(MiniGame.Hongbao.HongbaoMiniGameView), nameof(MiniGame.Hongbao.HongbaoMiniGameView.Update))]
        public static bool 抢红包Cheat(MiniGame.Hongbao.HongbaoMiniGameView __instance)
        {
            if (跳过抢红包游戏)
            {
                __instance.Skip();
                跳过抢红包游戏 = false;
            }
            return true;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(MiniGame.Sudoku.SudokuMiniGameView), nameof(MiniGame.Sudoku.SudokuMiniGameView.Update))]
        public static bool 玩数独Cheat(MiniGame.Sudoku.SudokuMiniGameView __instance)
        {
            if (跳过数独游戏)
            {
                __instance.OnClickSkip();
                跳过数独游戏 = false;
            }
            return true;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(MiniGame.Negotiation.NegotiationMiniGameView), nameof(MiniGame.Negotiation.NegotiationMiniGameView.FixedUpdate))]
        public static bool 话术Talk(MiniGame.Negotiation.NegotiationMiniGameView __instance)
        {
            if (跳过对话)
            {
                __instance.OnClickSkip();
                跳过对话 = false;
            }
            if (添加了策略点)
            {
                __instance.ctrl.datas[PlayerType.Me].skillMP += 添加策略点;
                __instance.RefreshSkill();
                string LeftName = "你已添加" + 添加策略点.ToString() + "策略点";
                string RightName = "准备好了吗，我要开了！！";
                BasicUI.SendMessageToGame(LeftName, RightName);
                添加了策略点 = false;
            }
            if (补充血量)
            {
                __instance.ctrl.datas[PlayerType.AI].DelHP(1, -__instance.ctrl.datas[PlayerType.AI].initHP);
                __instance.RefreshSkill();
                string LeftName = "你已补满血量";
                string RightName = "你想说服我？不，我的钱，我的时间！！！";
                BasicUI.SendMessageToGame(LeftName, RightName);
                补充血量 = false;
            }
            if (快速说服对手)
            {
                __instance.ctrl.datas[PlayerType.AI].DelHP(1, __instance.ctrl.datas[PlayerType.AI].initHP);
                __instance.RefreshHP();
                string LeftName = "你现在可以快速说服对方，无论是谁出牌";
                string RightName = "不好意思，我秒杀你！";
                BasicUI.SendMessageToGame(LeftName, RightName);
                快速说服对手 = false;
            }
            return true;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(MiniGame.QuickCalc.QuickCalcMiniGameView), nameof(MiniGame.QuickCalc.QuickCalcMiniGameView.Update))]
        public static bool 速算Cheat(MiniGame.QuickCalc.QuickCalcMiniGameView __instance)
        {
            if (跳过计算游戏)
            {
                __instance.OnClickSkip();
                跳过计算游戏 = false;
            }
            return true;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(MiniGame.Crossword.CrosswordMiniGameView), nameof(MiniGame.Crossword.CrosswordMiniGameView.Update))]
        public static bool 成语Cheat(MiniGame.Crossword.CrosswordMiniGameView __instance)
        {
            if (跳过填词小游戏)
            {
                __instance.OnClickSkip();
                跳过填词小游戏 = false;
            }
            return true;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(MiniGame.Puzzle.PuzzleMiniGameView), nameof(MiniGame.Puzzle.PuzzleMiniGameView.Update))]
        public static bool 玩拼图Cheat(MiniGame.Puzzle.PuzzleMiniGameView __instance)
        {
            if (跳过拼图游戏)
            {
                __instance.OnClickSkip();
                跳过拼图游戏 = false;
            }
            return true;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(MiniGame.Other.BrickGameView), nameof(MiniGame.Other.BrickGameView.Update))]
        public static bool 打砖块Cheat(MiniGame.Other.BrickGameView __instance)
        {
            if (跳过打砖块游戏)
            {
                __instance.OnClickSkip();
                跳过打砖块游戏 = false;
            }
            return true;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(MiniGame.Basketball.Basketball1On1View), nameof(MiniGame.Basketball.Basketball1On1View.FixedUpdate))]
        public static bool 篮球Cheat(MiniGame.Basketball.Basketball1On1View __instance)
        {
            if (跳过篮球小游戏)
            {
                __instance.OnClickSkip();
                跳过篮球小游戏 = false;
            }
            return true;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(MiniGame.Sentence.SentenceMiniGameView), nameof(MiniGame.Sentence.SentenceMiniGameView.Update))]
        public static bool 造句Cheat(MiniGame.Sentence.SentenceMiniGameView __instance)
        {
            if (跳过造句游戏)
            {
                __instance.OnClickSkip();
                跳过造句游戏 = false;
            }
            return true;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(MiniGame.CardMatch.CardMatchMiniGameView), nameof(MiniGame.CardMatch.CardMatchMiniGameView.FixedUpdate))]
        public static bool 翻牌Cheat(MiniGame.CardMatch.CardMatchMiniGameView __instance)
        {
            if (跳过方块匹配游戏英语)
            {
                __instance.OnClickSkip();
                跳过方块匹配游戏英语 = false;
            }
            return true;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(MiniGame.CardMatch.CardMatch2MiniGameView), nameof(MiniGame.CardMatch.CardMatch2MiniGameView.Update))]
        public static bool 太阁版复习Cheat(MiniGame.CardMatch.CardMatch2MiniGameView __instance)
        {
            if (跳过方块匹配游戏复习)
            {
                __instance.OnClickSkip();
                跳过方块匹配游戏复习 = false;
            }
            return true;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(MiniGame.Qte.Qte2MiniGameView), nameof(MiniGame.Qte.Qte2MiniGameView.FixedUpdate))]
        public static bool 讲价Cheat(MiniGame.Qte.Qte2MiniGameView __instance)
        {
            if (跳过讨价还价)
            {
               __instance.successCnt = 999;
               __instance.SetState(MiniGame.Qte.State.End);
                跳过讨价还价 = false;
            }
            return true;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(MiniGame.Quiz.QuizMiniGameView), nameof(MiniGame.Quiz.QuizMiniGameView.FixedUpdate))]
        public static bool 知识竞赛Cheat(MiniGame.Quiz.QuizMiniGameView __instance)
        {
            
            if (百人答题下一题)
            {
                if (__instance.sp == 0)
                {
                    __instance.ShowResult(true);
                    __instance.NextQuestion();
                }
                __instance.myScore = 99999;
                __instance.Continue();
            }
            return true;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(MiniGame.Hurdling.HurdlingMiniGameView), nameof(MiniGame.Hurdling.HurdlingMiniGameView.PlayerUpdate))]
        public static bool 跨栏Cheat(MiniGame.Hurdling.HurdlingMiniGameView __instance)
        {
            //if (跳过)
            //{
            //    __instance.
            // }
            return true;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(MiniGame.FingerKnife.FingerKnifeMiniGameView), nameof(MiniGame.FingerKnife.FingerKnifeMiniGameView.Update))]
        public static bool 戳指缝Cheat(MiniGame.FingerKnife.FingerKnifeMiniGameView __instance)
        {
            if (跳过戳指缝游戏)
            {
                __instance.OnClickSkip();
                跳过戳指缝游戏 = false;
            }
            return true;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(MiniGame.Fight.FightMiniGameView), nameof(MiniGame.Fight.FightMiniGameView.Update))]
        public static bool 打架Cheat(MiniGame.Fight.FightMiniGameView __instance)
        {
            if (跳过打架游戏)
            {
                __instance.OnClickSkip();
                跳过打架游戏 = false;
            }
            return true;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(MiniGame.Handicraft.HandicraftView), nameof(MiniGame.Handicraft.HandicraftView.Update))]
        public static bool 手工Cheat(MiniGame.Handicraft.HandicraftView __instance)
        {
            if (跳过手工游戏)
            {
                __instance.OnClickSkip();
                跳过手工游戏 = false;
            }
            return true;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(MiniGame.MagicCube.MagicCubeMiniGameView), nameof(MiniGame.MagicCube.MagicCubeMiniGameView.Update))]
        public static bool 灵感Cheat(MiniGame.MagicCube.MagicCubeMiniGameView __instance)
        {
            if (跳过灵感游戏)
            {
                __instance.OnClickSkip();
                跳过灵感游戏 = false;
            }
            return true;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(MiniGame.Piano.PianoMiniGameView), nameof(MiniGame.Piano.PianoMiniGameView.Update))]
        public static bool 弹钢琴Cheat(MiniGame.Piano.PianoMiniGameView __instance)
        {
            if (跳过钢琴游戏)
            {
                __instance.OnClickSkip();
                跳过钢琴游戏 = false;
            }
            return true;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(MiniGame.Badminton.BadmintonMiniGameView), nameof(MiniGame.Badminton.BadmintonMiniGameView.Update))]
        public static bool 羽毛球游戏Cheat(MiniGame.Badminton.BadmintonMiniGameView __instance)
        {
            if (跳过羽毛球游戏)
            {
                __instance.OnClickSkip();
                跳过羽毛球游戏 = false;
            }
            return true;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(MiniGame.StudyCard.StudyCardMiniGameView), nameof(MiniGame.StudyCard.StudyCardMiniGameView.FixedUpdate))]
        public static bool 学习卡牌游戏Cheat(MiniGame.StudyCard.StudyCardMiniGameView __instance)
        {
            if (跳过卡牌顺序游戏)
            {
                __instance.OnClickSkip();
                跳过卡牌顺序游戏 = false;
            }
            return true;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(MiniGame.Qte.Qte3MiniGameView), nameof(MiniGame.Qte.Qte3MiniGameView.FixedUpdate))]
        public static bool 狂点蓄力游戏Cheat(MiniGame.Qte.Qte3MiniGameView __instance)
        {
            if (跳过修下水管道游戏)
            {
                __instance.OnClickSkip();
                跳过修下水管道游戏 = false;
            }
            return true;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(MiniGame.Lizong.LizongMiniGameView), nameof(MiniGame.Lizong.LizongMiniGameView.Update))]
        public static bool 理综连线游戏Cheat(MiniGame.Lizong.LizongMiniGameView __instance)
        {
            if (跳过闭合电路游戏)
            {
                __instance.OnClickSkip();
                跳过闭合电路游戏 = false;
            }
            return true;
        }
        
        //[HarmonyPrefix]
        //[HarmonyPatch(typeof(MiniGame.Fight.FightMiniGameView), nameof(MiniGame.Fight.FightMiniGameView.Update))]
        //public static bool FightFastFinish(MiniGame.Fight.FightMiniGameView __instance)
        //{
        //    if (跳过打架游戏)
        //    {
        //        __instance.OnClickSkip();
        //        string LeftName = "你已跳过游戏";
        //        string RightName = "为什么要跳过打架？是不会打架吗";
        //        BasicUI.SendMessageToGame(LeftName, RightName);
        //        跳过打架游戏 = false;
        //    }
        //    return true;
        //}

        //[HarmonyPrefix]
        //[HarmonyPatch(typeof(MiniGame.MagicCube.MagicCubeMiniGameView), nameof(MiniGame.MagicCube.MagicCubeMiniGameView.Update))]
        //public static bool MagicCubeFastFinish(MiniGame.MagicCube.MagicCubeMiniGameView __instance)
        //{
        //    if (跳过魔方游戏)
        //    {
        //        __instance.OnClickSkip();
        //        string LeftName = "你已跳过游戏";
        //        string RightName = "为什么要跳过魔方？是不会魔方吗";
        //        BasicUI.SendMessageToGame(LeftName, RightName);
        //        跳过魔方游戏 = false;
        //    }
        //    return true;
        //}


    }
}
