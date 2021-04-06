using FlatBuffers;
using Framework;
using MyFlatBuffer;
using System;
using System.Collections;
using System.Collections.Generic;
using Toolkits;
using UnityEngine;
using UnityEngine.Events;

public class MyFlatBufferHandler : Single<MyFlatBufferHandler>
{
    private NetWorkManager NetWorkMgr = null;
    // private Dictionary<Actions, UnityAction<Actions, object> > RegisterListenFunctions = new Dictionary<Actions, UnityAction<Actions, object> >();

    // /// <summary>
    // /// 发送数据
    // /// </summary>
    // /// <param name="builder">由FlatBufferBuilder创建的数据</param>
    // public void SendFlatBufffer(FlatBufferBuilder builder)
    // {
    //     if(builder == null)
    //     {
    //         Debug.LogError("【SendFlatBufffer】创建FlatBufferBuilder 不能为空");
    //         return;
    //     }

    //     byte[] sendData = builder.SizedByteArray();
    //     if (sendData.Length <= 0)
    //     {
    //         Debug.LogError("【SendFlatBufffer】发送数据 不能为空");
    //         return;
    //     }

    //     if (NetWorkMgr == null)
    //         NetWorkMgr = AppFacade.Instance.GetManager<NetWorkManager>(ManagerName.NetWork);

    //     NetWorkMgr.Client.SendServerBytes(sendData);
    // }

    // public void RegisterListenCallBack(Actions action, UnityAction<Actions, object> listenCallBack)
    // {
    //     if(RegisterListenFunctions.ContainsKey(action))
    //     {
    //         Debug.LogError("网络监听回调已经存在" + action);
    //         return;
    //     }

    //     RegisterListenFunctions.Add(action, listenCallBack);
    // }

    // public void DelListenCallBack(Actions action)
    // {
    //     if (RegisterListenFunctions.ContainsKey(action))
    //     {
    //         RegisterListenFunctions.Remove(action);
    //     }
    // }

    // public void DelAllListenCallBack()
    // {
    //     RegisterListenFunctions.Clear();
    // }

    // public void HandlerFlatterBufferData(ByteBuffer byteBuffer)
    // {
    //     MyFlatBuffer.Message message = MyFlatBuffer.Message.GetRootAsMessage(byteBuffer);

    //     if(AppConst.UseNetDebug)
    //         Debug.Log("服务器数据解析：" + message.Code + " type = " + message.ContentType);

    //     if (message.Code != Error.OK)
    //     {
    //         Debug.LogError("服务器返回消息错误：" + message.Code);
    //     }

    //     //处理数据
    //     switch (message.ContentType)
    //     {
    //         case MyFlatBuffer.Messages.NONE:
    //             break;
    //         case MyFlatBuffer.Messages.ConnectReq:
    //             ConnectReq connectReq = message.ContentAsConnectReq();
    //             Debug.Log(connectReq.SenceId);
    //             Debug.Log(connectReq.Token);
    //             break;
    //         case MyFlatBuffer.Messages.ConnectResp:
    //             ConnectResp connectResp = message.ContentAsConnectResp();
    //             Debug.Log(connectResp.Result);
    //             Debug.Log(connectResp.Snapshot);
    //             break;
    //         case MyFlatBuffer.Messages.Event:
    //             MyFlatBuffer.Event e = message.ContentAsEvent();
    //             HanleEventBuffer(e);
    //             break;
    //         case MyFlatBuffer.Messages.Publish:
    //             MyFlatBuffer.Publish p = message.ContentAsPublish();
    //             HandlePublishBuffer(p);
    //             break;
    //         case MyFlatBuffer.Messages.Command:
    //             MyFlatBuffer.Command c = message.ContentAsCommand();
    //             HandleCommandBuffer(c);
    //             break;
    //         default:
    //             Debug.LogError("服务器返回消息类型未定义");
    //             break;
    //     }
    // }

    // public void HanleEventBuffer(MyFlatBuffer.Event e)
    // {
    //     MyFlatBuffer.Entity? entity = e.Entity;
    //     MyFlatBuffer.Publish? publish = e.Data;

    //     //Debug.Log("event publish enter");
    //     if(publish != null)
    //     {
    //         MyFlatBuffer.Publish p = (MyFlatBuffer.Publish)publish;
    //         Actions action = p.ActionType;
    //         //Debug.Log("MyFlatBuffer 处理函数 action = " + action);
    //         if (action == Actions.UITestAction)
    //         {
    //             UITestAction value = p.ActionAsUITestAction();
    //             HandlerFlatterBufferData(new ByteBuffer(value.GetRawArray()));
    //             return;
    //         }

    //         UnityAction<Actions, object> callFunc;
    //         if (RegisterListenFunctions.TryGetValue(action, out callFunc))
    //         {
    //             callFunc(action, e);
    //         }
    //         else
    //         {
    //             Debug.LogError("MyFlatBuffer 未定义处理函数 action = " + action);
    //         }
    //     }
    // }

    // private void HandlePublishBuffer(MyFlatBuffer.Publish p)
    // {
    //     Actions action = p.ActionType;
    //     UnityAction<Actions, object> callFunc;
    //     if (RegisterListenFunctions.TryGetValue(action, out callFunc))
    //     {
    //         callFunc(action, p);
    //     }
    // }

    // private void HandleCommandBuffer(MyFlatBuffer.Command c)
    // {
    //     Actions action = c.ActionType;
    //     UnityAction<Actions, object> callFunc;
    //     if(RegisterListenFunctions.TryGetValue(action,out callFunc))
    //     {
    //         callFunc(action, c);
    //     }
    // }
}
