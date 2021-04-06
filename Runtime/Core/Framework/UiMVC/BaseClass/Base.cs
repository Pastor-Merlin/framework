using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Framework
{
    public class Base : MonoBehaviour
    {
        private AppFacade m_Facade;
        private ResourceManager m_ResMgr;
        private SoundManager m_SoundMgr;
        private TimerManager m_TimerMgr;
        private ObjectPoolManager m_ObjectPoolMgr;
        private ScenesManager m_ScenesMgr;
        //private InputManager m_InputMgr;
        private UpdateManager m_UpdateMgr;
        private NetWorkManager m_NetWorkMgr;

        /// <summary>
        /// 注册消息
        /// </summary>
        /// <param name="view"></param>
        /// <param name="messages"></param>
        protected void RegisterMessage(IBaseView view, List<string> messages)
        {
            if (messages == null || messages.Count == 0) return;
            MessageCtl.Instance.RegisterViewCommand(view, messages.ToArray());
        }

        /// <summary>
        /// 移除消息
        /// </summary>
        /// <param name="view"></param>
        /// <param name="messages"></param>
        protected void RemoveMessage(IBaseView view, List<string> messages)
        {
            if (messages == null || messages.Count == 0) return;
            MessageCtl.Instance.RemoveViewCommand(view, messages.ToArray());
        }

        protected AppFacade facade
        {
            get
            {
                if (m_Facade == null)
                {
                    m_Facade = AppFacade.Instance;
                }
                return m_Facade;
            }
        }

        protected ResourceManager ResManager
        {
            get
            {
                if (m_ResMgr == null)
                {
                    m_ResMgr = facade.GetManager<ResourceManager>(ManagerName.Resource);
                }
                return m_ResMgr;
            }
        }

        protected SoundManager SoundManager
        {
            get
            {
                if (m_SoundMgr == null)
                {
                    m_SoundMgr = facade.GetManager<SoundManager>(ManagerName.Sound);
                }
                return m_SoundMgr;
            }
        }

        protected TimerManager TimerManager
        {
            get
            {
                if (m_TimerMgr == null)
                {
                    m_TimerMgr = facade.GetManager<TimerManager>(ManagerName.Timer);
                }
                return m_TimerMgr;
            }
        }

        protected ObjectPoolManager ObjPoolManager
        {
            get
            {
                if (m_ObjectPoolMgr == null)
                {
                    m_ObjectPoolMgr = facade.GetManager<ObjectPoolManager>(ManagerName.ObjectPool);
                }
                return m_ObjectPoolMgr;
            }
        }

        //protected InputManager InputManager
        //{
        //    get
        //    {
        //        if (m_InputMgr == null)
        //        {
        //            m_InputMgr = facade.GetManager<InputManager>(ManagerName.Input);
        //        }
        //        return m_InputMgr;
        //    }
        //}

        protected NetWorkManager NetWorkManager
        {
            get
            {
                if (m_NetWorkMgr == null)
                {
                    m_NetWorkMgr = facade.GetManager<NetWorkManager>(ManagerName.NetWork);
                }
                return m_NetWorkMgr;
            }
        }

        protected UpdateManager UpdateManager
        {
            get
            {
                if (m_UpdateMgr == null)
                {
                    m_UpdateMgr = facade.GetManager<UpdateManager>(ManagerName.Update);
                }
                return m_UpdateMgr;
            }
        }

        protected ScenesManager ScenesManager
        {
            get
            {
                if(m_ScenesMgr == null)
                {
                    m_ScenesMgr = facade.GetManager<ScenesManager>(ManagerName.Scenes);
                }
                return m_ScenesMgr;
            }
        }
    }
}