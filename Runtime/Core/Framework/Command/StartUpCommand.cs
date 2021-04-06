using Framework;

public class StartUpCommand : ControllerCommand 
{
    public override void Execute(IMessage message)
    {
        //-----------------初始化管理器-----------------------
        AppFacade.Instance.AddManager<SoundManager>(ManagerName.Sound);
        AppFacade.Instance.AddManager<TimerManager>(ManagerName.Timer);
        AppFacade.Instance.AddManager<ResourceManager>(ManagerName.Resource);
        AppFacade.Instance.AddManager<ObjectPoolManager>(ManagerName.ObjectPool);
        //AppFacade.Instance.AddManager<InputManager>(ManagerName.Input);
        AppFacade.Instance.AddManager<UpdateManager>(ManagerName.Update);
        AppFacade.Instance.AddManager<ScenesManager>(ManagerName.Scenes);
        AppFacade.Instance.AddManager<GameManager>(ManagerName.Game);
        AppFacade.Instance.AddManager<NetWorkManager>(ManagerName.NetWork);
    }
}