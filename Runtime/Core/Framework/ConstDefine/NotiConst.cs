using UnityEngine;
using System.Collections;

namespace Framework
{
    public class NotiConst
    {
        /// <summary>
        /// Controller层消息通知
        /// </summary>
        public const string START_UP = "StartUp";                       //启动框架
        public const string UI_INIT = "UiInit";                         //UI 初始化
        public const string DISPATCH_MESSAGE = "DispatchMessage";       //派发信息

        /// <summary>
        /// View层消息通知
        /// </summary>

        //scene load about
        public const string SCENELOAD = "SceneLoad";
        public const string SCENELOADED = "SceneLoaded";

        public const string VIEWSHOW = "ViewShow";
        public const string VIEWDEL = "ViewDel";

        //Loading 
        public const string LOADINGSHOW = "LoadingShow";
        public const string LOADINGDEL = "LoadingDel";
        public const string LOADSTARTSHOW = "LoadStartShow";
        public const string LOADSTARTDEL = "LoadStartDel";
        public const string LOADINGDESTORY = "LoadingDestory";      //Loading View detory
        public const string LOADINGINFO = "LoadingInfo";         //Loading View info update
        public const string LOADINGPROGRESS = "LoadingProgress";     //Loading View Progress update
        //Fight                                                             
        public const string FIGHTSHOW = "FIGHTSHOW";
        public const string FIGHTDEL = "FIGHTDEL";
        public const string FIGHTUPDATE = "FIGHTUPDATE";
        //Score
        public const string SCORESHOW = "SCORESHOW";
        public const string SCOREDEL = "SCOREDEL";
        public const string SCOREUPDATE = "SCOREUPDATE";
        public const string SCORE_SHOWGENERAL = "SCORE_SHOWGENERAL";//跳转综合评分
        public const string SCORE_BACKTOLOBBY = "SCORE_BACKTOLOBBY";//返回游戏大厅
        public const string SCORE_BACKTOHISTORY = "SCORE_BACKTOHISTORY";//返回历史回放



        //Login
        public const string UILOGINSHOW = "UILoginShow";
        public const string UILOGINDEL = "UILoginDel";
        public const string UILOGINUPDATE = "UILoginUpdate";


        //Lobby
        public const string UILOBBYSHOW = "UILobbyShow";
        public const string UILOBBYDEL = "UILobbyDel";
        public const string UILOBBYUPDATE = "UILobbyUpdate";


        //CreatTask
        public const string UICREATTASKSHOW = "UICreatTaskShow";
        public const string UICREATTASKDEL = "UICreatTaskDel";
        public const string UICREATTASKUPDATE = "UICreatTaskUpdate";


        //Task
        public const string UITASKSHOW = "UITaskShow";
        public const string UITASKDEL = "UITaskDel";
        public const string UITASKUPDATE = "UITaskUpdate";

        //MapConfig
        public const string UIMAPCONFIGSHOW = "UIMapConfigShow";
        public const string UIMAPCONFIGDEL = "UIMapConfigDel";
        public const string UIMAPCONFIGUPDATE = "UIMapConfigUpdate";


        //TaskWaiting
        public const string UITASkWAITINGSHOW = "UITaskWaitingShow";
        public const string UITASKWAITINGDEL = "UITaskWaitingDel";
        public const string UITASKWAITINGUPDATE = "UITaskWaitingUpdate";


        //tips
        public const string UITIPSSHOW = "UITipsShow";
        public const string UITIPSDEL = "UITipsDel";
        public const string UITIPSUPDATE = "UITipsUpdate";



        //装备界面
        public const string UIEQUIPSHOW = "UIEquipShow";
        public const string UIEQUIPDEL = "UIEquipDel";
        public const string UIEQUIPUPDATE = "UIEquipUpdate";


    }
}