// using System.Collections;
// using System.Collections.Generic;

// using FlatBuffers;

// using MyFlatBuffer;

// using Toolkits;

// using UnityEngine;
// using UnityEngine.Events;

// namespace Framework
// {
//     public class BaseProxy<T> where T : class, new()
//     {
//         private static T _instance;
//         private static readonly Object _lock = new Object();

//         public static T Instance()
//         {
//             if (_instance == null)
//             {
//                 lock (_lock)
//                 {
//                     if (_instance == null)
//                         _instance = new T();
//                 }
//             }

//             return _instance;
//         }

//         public void Invoke(string eventName, object eventParams = null)
//         {
//             EventManager.Invoke(eventName, eventParams);
//         }

//         public void Invoke(object obj, string eventName, object eventParams = null)
//         {
//             EventManager.Invoke(obj, eventName, eventParams);
//         }

//         /// <summary>
//         /// 处理FlatBuffer句柄
//         /// </summary>
//         public MyFlatBufferHandler FBHandler { get { return MyFlatBufferHandler.Instance(); } }

//         /// <summary>
//         /// 注册监听函数
//         /// </summary>
//         /// <param name="action"></param>
//         /// <param name="listenCallBack"></param>
//         public void RegisterListenCallBack(Actions action, UnityAction<Actions, object> listenCallBack)
//         {
//             FBHandler.RegisterListenCallBack(action, listenCallBack);
//         }

//         /// <summary>
//         /// 发送FlatBufferBuilder
//         /// </summary>
//         /// <param name="builder"></param>
//         public void SendFlatBufffer(FlatBufferBuilder builder)
//         {
//             FBHandler.SendFlatBufffer(builder);
//         }

//         /// <summary>
//         /// 创建Event 的 Message
//         /// </summary>
//         /// <param name="builder"></param>
//         public FlatBufferBuilder CreateEventMessage(FlatBufferBuilder sendbuilder, Offset<MyFlatBuffer.Publish> publish)
//         {
//             if (sendbuilder == null)
//                 return null;

//             //EVENT
//             MyFlatBuffer.Event.StartEvent(sendbuilder);
//             MyFlatBuffer.Event.AddData(sendbuilder, publish);
//             var eventoffset = MyFlatBuffer.Event.EndEvent(sendbuilder);
//             //MESSAGE
//             MyFlatBuffer.Message.StartMessage(sendbuilder);
//             MyFlatBuffer.Message.AddCode(sendbuilder, Error.OK);
//             MyFlatBuffer.Message.AddContentType(sendbuilder, Messages.Event);
//             MyFlatBuffer.Message.AddContent(sendbuilder, eventoffset.Value);
//             var offset3 = MyFlatBuffer.Message.EndMessage(sendbuilder);
//             sendbuilder.FinishSizePrefixed(offset3.Value);

//             return sendbuilder;
//         }

//         /// <summary>
//         /// 创建Event 的 Message
//         /// </summary>
//         /// <param name="builder"></param>
//         public FlatBufferBuilder CreateEventMessage(FlatBufferBuilder sendbuilder, Offset<MyFlatBuffer.Entity> entityOffset)
//         {
//             if (sendbuilder == null)
//                 return null;

//             //EVENT
//             MyFlatBuffer.Event.StartEvent(sendbuilder);
//             MyFlatBuffer.Event.AddEntity(sendbuilder, entityOffset);
//             var eventoffset = MyFlatBuffer.Event.EndEvent(sendbuilder);

//             //MESSAGE
//             MyFlatBuffer.Message.StartMessage(sendbuilder);
//             MyFlatBuffer.Message.AddCode(sendbuilder, Error.OK);
//             MyFlatBuffer.Message.AddContentType(sendbuilder, Messages.Event);
//             MyFlatBuffer.Message.AddContent(sendbuilder, eventoffset.Value);
//             var offset3 = MyFlatBuffer.Message.EndMessage(sendbuilder);
//             sendbuilder.FinishSizePrefixed(offset3.Value);

//             return sendbuilder;
//         }

//         /// <summary>
//         /// 创建Event 的 Message
//         /// </summary>
//         /// <param name="builder"></param>
//         public FlatBufferBuilder CreateEventMessage(FlatBufferBuilder sendbuilder, Offset<MyFlatBuffer.Entity> entityOffset, Offset<MyFlatBuffer.Publish> publish)
//         {
//             if (sendbuilder == null)
//                 return null;

//             //EVENT
//             MyFlatBuffer.Event.StartEvent(sendbuilder);
//             MyFlatBuffer.Event.AddEntity(sendbuilder, entityOffset);
//             MyFlatBuffer.Event.AddData(sendbuilder, publish);
//             var eventoffset = MyFlatBuffer.Event.EndEvent(sendbuilder);
//             //MESSAGE
//             MyFlatBuffer.Message.StartMessage(sendbuilder);
//             MyFlatBuffer.Message.AddCode(sendbuilder, Error.OK);
//             MyFlatBuffer.Message.AddContentType(sendbuilder, Messages.Event);
//             MyFlatBuffer.Message.AddContent(sendbuilder, eventoffset.Value);
//             var offset3 = MyFlatBuffer.Message.EndMessage(sendbuilder);
//             sendbuilder.FinishSizePrefixed(offset3.Value);

//             return sendbuilder;
//         }

//         /// <summary>
//         /// 创建Publish 的 Message
//         /// </summary>
//         /// <param name="sendbuilder"></param>
//         /// <param name="stateType"></param>
//         /// <param name="stateOffset"></param>
//         /// <param name="actionType"></param>
//         /// <param name="actionOffset"></param>
//         /// <returns></returns>
//         public FlatBufferBuilder CreatePublishMessage(FlatBufferBuilder sendbuilder, MyFlatBuffer.States stateType = MyFlatBuffer.States.NONE, int stateOffset = 0, MyFlatBuffer.Actions actionType = MyFlatBuffer.Actions.NONE, int actionOffset = 0)
//         {
//             if (sendbuilder == null)
//                 return null;

//             //PUBLISH
//             var publishoffset = MyFlatBuffer.Publish.CreatePublish(sendbuilder, stateType, stateOffset, actionType, actionOffset);
//             //MESSAGE
//             MyFlatBuffer.Message.StartMessage(sendbuilder);
//             MyFlatBuffer.Message.AddCode(sendbuilder, Error.OK);
//             MyFlatBuffer.Message.AddContentType(sendbuilder, Messages.Publish);
//             MyFlatBuffer.Message.AddContent(sendbuilder, publishoffset.Value);
//             var offset3 = MyFlatBuffer.Message.EndMessage(sendbuilder);
//             sendbuilder.FinishSizePrefixed(offset3.Value);

//             return sendbuilder;
//         }

//         /// <summary>
//         /// 创建command 的 Mesage
//         /// </summary>
//         /// <param name="sendbuilder"></param>
//         /// <param name="action_type"></param>
//         /// <param name="actionOffset"></param>
//         /// <param name="targetsOffset"></param>
//         /// <returns></returns>
//         public FlatBufferBuilder CreateCommandMessage(FlatBufferBuilder sendbuilder, MyFlatBuffer.Actions action_type = MyFlatBuffer.Actions.NONE, int actionOffset = 0, VectorOffset targetsOffset = default(VectorOffset))
//         {
//             if (sendbuilder == null)
//                 return null;

//             //COMMAND
//             var commndoffset = MyFlatBuffer.Command.CreateCommand(sendbuilder, action_type, actionOffset, targetsOffset);

//             //MESSAGE
//             MyFlatBuffer.Message.StartMessage(sendbuilder);
//             MyFlatBuffer.Message.AddCode(sendbuilder, Error.OK);
//             MyFlatBuffer.Message.AddContentType(sendbuilder, Messages.Command);
//             MyFlatBuffer.Message.AddContent(sendbuilder, commndoffset.Value);
//             var offset3 = MyFlatBuffer.Message.EndMessage(sendbuilder);
//             sendbuilder.FinishSizePrefixed(offset3.Value);

//             return sendbuilder;
//         }
//     }
// }