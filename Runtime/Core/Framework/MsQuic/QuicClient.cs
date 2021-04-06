using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using FlatBuffers;

namespace Framework
{
    public class QuicClient : QuicClientPtr, IDisposable
    {
        private const int offset = sizeof(int);

        private string host = "10.234.61.130";//"192.168.102.55";//46
                                              //private string host = "192.168.102.55";
        public string Host { set { host = value; } get { return host; } }

        private ushort port = 4433;
        public ushort Port { set { port = value; } get { return port; } }

        private string protocolname = "hq-29";
        public string ProtocolName { set { protocolname = value; } get { return protocolname; } }

        private string appname = "quichq-29";
        public string AppName { set { appname = value; } get { return appname; } }

        public delegate void ConnectedCallBack(QUIC_STATUS s);
        private static ConnectedCallBack ConnectCallBackHandler;

        public bool IsConnected = false;

        private static byte[] dataArray;

        private static List<byte[]> sendBytesList = new List<byte[]>();
        private static List<ByteBuffer> reciveFlatBuffers = new List<ByteBuffer>();
        private static List<byte[]> reciveBytesList = new List<byte[]>();
        private static List<byte[]> handleReciveBytesList = new List<byte[]>();

        public QuicClient()
        {
            Loom.Initialize();
            //AppFacade.Instance.GetManager<UpdateManager>(ManagerName.Update).AddEvent(Update);
        }

        public void SetHostPort(string ip, ushort port)
        {
            Host = ip;
            Port = port;
        }

        public static void SetConnectedCallBack(ConnectedCallBack callBack)
        {
            ConnectCallBackHandler += callBack;
        }

        public void InitQuicDLL()
        {
            IsConnected = false;

            MsQuicClearConnectCallBack();
            MsQuicClearStreamCallBack();

            QUIC_TEST testFunc = new QUIC_TEST(QUIC_Test);
            MsQuicInitSet(appname, protocolname, 1000 * 60, testFunc);

            QUIC_CONNECTION_CONNECTED_HANDLER callbackFunc1 = new QUIC_CONNECTION_CONNECTED_HANDLER(QUIC_Connection_Connected_Handler);
            QUIC_CONNECTION_EVENT_SHUTDOWN_INITIATED_BY_TRANSPORT_HANDLER callbackFunc2 = new QUIC_CONNECTION_EVENT_SHUTDOWN_INITIATED_BY_TRANSPORT_HANDLER(QUIC_Connection_Event_Shutdown_Initiated_By_Transport_Handler);
            QUIC_CONNECTION_EVENT_SHUTDOWN_INITIATED_BY_PEER_HANDLER callbackFunc3 = new QUIC_CONNECTION_EVENT_SHUTDOWN_INITIATED_BY_PEER_HANDLER(QUIC_Connection_Event_Shutdown_Initiated_By_Peer_Handler);
            QUIC_CONNECTION_EVENT_SHUTDOWN_COMPLETE_HANDLER callbackFunc4 = new QUIC_CONNECTION_EVENT_SHUTDOWN_COMPLETE_HANDLER(QUIC_Connection_Event_Shutdown_Complete_Handler);
            QUIC_CONNECTION_EVENT_RESUMPTION_TICKET_RECEIVED_HANDLER callbackFunc5 = new QUIC_CONNECTION_EVENT_RESUMPTION_TICKET_RECEIVED_HANDLER(QUIC_Connection_Event_Resumption_Ticket_Received_Handler);
            MsQuicRegisterConnectCallBack(callbackFunc1, callbackFunc2, callbackFunc3, callbackFunc4, callbackFunc5);

            QUIC_STREAM_EVENT_SEND_COMPLETE_HANDLER Complete = new QUIC_STREAM_EVENT_SEND_COMPLETE_HANDLER(QUIC_Stream_Event_Send_Complete_Handler);
            QUIC_STREAM_EVENT_RECEIVE_HANDLER Receive = new QUIC_STREAM_EVENT_RECEIVE_HANDLER(QUIC_Stream_Event_Receive_Handler);
            QUIC_STREAM_EVENT_PEER_SEND_ABORTED_HANDLER Aborted = new QUIC_STREAM_EVENT_PEER_SEND_ABORTED_HANDLER(QUIC_Stream_Event_Peer_Send_Aborted_Handler);
            QUIC_STREAM_EVENT_PEER_SEND_SHUTDOWN_HANDLER Shutdown = new QUIC_STREAM_EVENT_PEER_SEND_SHUTDOWN_HANDLER(QUIC_Stream_Event_Peer_Send_Shutdown_Handler);
            QUIC_STREAM_EVENT_SEND_SHUTDOWN_COMPLETE_HANDLER EventShutdownComplete = new QUIC_STREAM_EVENT_SEND_SHUTDOWN_COMPLETE_HANDLER(QUIC_Stream_Event_Send_Shutdown_Complete_Handler);
            QUIC_STREAM_EVENT_SHUTDOWN_COMPLETE_HANDLER ShutdownComplte = new QUIC_STREAM_EVENT_SHUTDOWN_COMPLETE_HANDLER(QUIC_Stream_Event_Shutdown_Complete_Handler);
            MsQuicRegisterStreamCallBack(Complete, Receive, Aborted, Shutdown, EventShutdownComplete, ShutdownComplte);
        }

        public static void QUIC_Test(Int64 value)
        {
            Debug.Log("测试数据返回" + value);
        }

        public void ConnectQuicDLL()
        {
            QUIC_STATUS Status;
            Status = MsQuicConnectServer(host, port);
            if (Status > QUIC_STATUS.QUIC_STATUS_SUCCESS && 459749 != (int)Status)
            {
                //QuicClose();
                Debug.LogError("MsQuicOpen failed Status =" + Status);
                return;
            }
        }

        public static void QUIC_Connection_Connected_Handler(QUIC_STATUS Status)
        {
            Debug.Log("QUIC_Connection_Connected_Handler");
            if (Status > QUIC_STATUS.QUIC_STATUS_SUCCESS && 459749 != (int)Status)
            {
                Debug.LogError("QUIC_Connection_Connected_Handler ERROR!!!!!!" + Status);
                return;
            }

            if (ConnectCallBackHandler != null)
            {
                ConnectCallBackHandler(Status);
            }
        }

        public void SendServerBytes(byte[] sendData)
        {
            if (AppConst.UseNetDebug)
            {
                Debug.Log("发送总长度" + sendData.Length + "发送byte[]===" + BitConverter.ToString(sendData));
            }

            SendMsg(sendData);
        }

        public static void SendMsg(byte[] sendData)
        {
            QUIC_STATUS StatusSend = QuicDLL.ClientSend((UInt32)sendData.Length, sendData);
            if (StatusSend > QUIC_STATUS.QUIC_STATUS_SUCCESS && 459749 != (int)StatusSend)
            {
                Debug.Log("QUIC Send ERROR!!!!!!" + StatusSend);
            }
        }

        public static void QUIC_Connection_Event_Shutdown_Initiated_By_Transport_Handler()
        {
            Debug.LogError("QUIC_Connection_Event_Shutdown_Initiated_By_Transport_Handler");
        }

        public static void QUIC_Connection_Event_Shutdown_Initiated_By_Peer_Handler()
        {
            Debug.LogError("QUIC_Connection_Event_Shutdown_Initiated_By_Peer_Handler");
        }

        public static void QUIC_Connection_Event_Shutdown_Complete_Handler()
        {
            Debug.LogError("QUIC_Connection_Event_Shutdown_Complete_Handler");
        }

        public static void QUIC_Connection_Event_Resumption_Ticket_Received_Handler()
        {
            Debug.Log("QUIC_Connection_Event_Resumption_Ticket_Received_Handler");
        }

        //----------------------------stream about-------------------------------------------------
        public static void QUIC_Stream_Event_Send_Complete_Handler()
        {
            //sendBytesList.RemoveAt(0);

            ////Debug.Log("QUIC_Stream_Event_Send_Complete_Handler");
            //if(sendBytesList.Count > 0)
            //{
            //    SendMsg(sendBytesList[0]);
            //}
        }

        
        private void Update()
        {
            if(reciveBytesList.Count > 0)
            {
                //Debug.LogFormat("<color=#00ffff>{0},{1}</color> ", "reciveBytesList handled after count ", reciveBytesList.Count);

                lock (reciveBytesList)
                {
                    handleReciveBytesList.Clear();
                    handleReciveBytesList.AddRange(reciveBytesList);
                    reciveBytesList.Clear();
                }
                
                int totalLength = 0;
                for(int i = 0; i < handleReciveBytesList.Count;i++)
                {
                    totalLength += handleReciveBytesList[i].Length;
                }
                byte[] allbytes = new byte[totalLength];
                int cpyAllByteStart = 0;
                for (int i = 0; i < handleReciveBytesList.Count; i++)
                {
                    byte[] bytes = handleReciveBytesList[i];
                    Array.Copy(bytes, 0, allbytes, cpyAllByteStart, bytes.Length);
                    cpyAllByteStart += bytes.Length;
                }
                DecodeServerData(allbytes);

                //Debug.LogFormat("<color=#00ff00>{0},{1}</color> ", "reciveBytesList handled after count ", reciveBytesList.Count);
            }

            if(reciveFlatBuffers.Count > 0)
            {
                for(int i = 0; i < reciveFlatBuffers.Count;i++)
                {
                    ByteBuffer byteBuffer = reciveFlatBuffers[i];
                    //MyFlatBufferHandler.Instance().HandlerFlatterBufferData(byteBuffer);
                }
                reciveFlatBuffers.Clear();
            }
        }

        public static void QUIC_Stream_Event_Receive_Handler(ref QUIC_BUFFER quicBuffer)
        {
            byte[] serverArray = new byte[quicBuffer.Length];
            Marshal.Copy(quicBuffer.Buffer, serverArray, 0, quicBuffer.Length);

            if (AppConst.UseNetDebug)
            {
                Debug.Log("收到（数据）length = "+ serverArray.Length + " byte[]===" + BitConverter.ToString(serverArray));
            }

            lock (reciveBytesList)
            {
                reciveBytesList.Add(serverArray);
            }

            //Debug.LogFormat("<color=#ff0000>{0},{1}</color> ", "reciveBytesList recive count " , reciveBytesList.Count);
        }

        private static void DecodeServerData(byte[] serverArray)
        {
            byte[] allDataArry;
            if (dataArray != null)
            {
                allDataArry = new byte[dataArray.Length + serverArray.Length];
                Array.Copy(dataArray, allDataArry, dataArray.Length);
                Array.Copy(serverArray, 0, allDataArry, dataArray.Length, serverArray.Length);
            }
            else
            {
                allDataArry = serverArray;
            }

            byte[] lenArray = new byte[offset];
            for (int i = 0; i < offset; i++)
                lenArray[i] = allDataArry[i];

            int length = BitConverter.ToInt32(lenArray, 0);
            int totalLength = length + offset;

            if (allDataArry.Length == totalLength)
            {
                ByteBuffer byteBuffer = new ByteBuffer(allDataArry, offset);
                dataArray = null;
                reciveFlatBuffers.Add(byteBuffer);

                //Loom.QueueOnMainThread((object obj) =>
                //{
                //    MyFlatBufferHandler.Instance().HandlerFlatterBufferData(byteBuffer);
                //});
            }
            else
            {
                if (allDataArry.Length < totalLength)
                {
                    dataArray = allDataArry;
                }
                else  // allDataArry.Length > totalLength
                {
                    byte[] handleArray = new byte[totalLength];
                    Array.Copy(allDataArry, handleArray, totalLength);

                    ByteBuffer byteBuffer = new ByteBuffer(handleArray, offset);
                    reciveFlatBuffers.Add(byteBuffer);

                    //Loom.QueueOnMainThread((object obj) =>
                    //{
                    //    MyFlatBufferHandler.Instance().HandlerFlatterBufferData(byteBuffer);
                    //});

                    byte[] reminDataArray = new byte[allDataArry.Length - totalLength];
                    Array.Copy(allDataArry, totalLength, reminDataArray, 0, allDataArry.Length - totalLength);

                    dataArray = null;
                    DecodeServerData(reminDataArray);
                }
            }
        }

        public static void QUIC_Stream_Event_Peer_Send_Aborted_Handler()
        {
            Debug.Log("QUIC_Stream_Event_Peer_Send_Aborted_Handler");
        }

        public static void QUIC_Stream_Event_Peer_Send_Shutdown_Handler()
        {
            Debug.Log("QUIC_Stream_Event_Peer_Send_Shutdown_Handler");
        }

        public static void QUIC_Stream_Event_Send_Shutdown_Complete_Handler()
        {
            Debug.LogError("QUIC_Stream_Event_Send_Shutdown_Complete_Handler");
        }

        public static void QUIC_Stream_Event_Shutdown_Complete_Handler()
        {
            Debug.LogError("QUIC_Stream_Event_Shutdown_Complete_Handler");
        }

        public void SendDatagram()
        {
            ClientSendDatagram();
        }

        /// <summary>
        /// 关闭连接
        /// </summary>
        public void QuicClose()
        {
            AppFacade.Instance.GetManager<UpdateManager>(ManagerName.Update).RemoveEvent(Update);
            MsQuicCloseConnect(true);
        }

        public void Dispose()
        {
            QuicClose();
            System.GC.SuppressFinalize(this);
        }
    }
}
