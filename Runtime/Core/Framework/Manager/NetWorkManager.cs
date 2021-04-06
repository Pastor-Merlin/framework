using Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text;
// using Toolkits;
using System.Threading;

namespace Framework
{
    public class NetWorkManager : Manager
    {
        private QuicClient client;
        public QuicClient Client { get { return client; } }

        private bool checkConnected = false;
        private string connectType = "";
        private float curTicketTime = 0f;
        private Thread thread;

        private byte[] nullBytes = new byte[1];

        public void Start()
        {
        }

        public void Update()
        {
            // if (checkConnected)
            // {
            //     checkConnected = false;

            //     EventManager.Invoke(NotiConst.LOADINGDESTORY, "");

            //     if (connectType == "connect")
            //         EventManager.Invoke(NotiConst.SCENELOAD, "Menu");
            // }
        }

        public void ConnectedToServer(string connectType)
        {
            this.connectType = connectType;

            if (client != null)
            {
                client.QuicClose();
                client = null;
            }

            QuicClient.SetConnectedCallBack(OnConnected);

            client = new QuicClient();

            //设置IP Port
            if (AppConst.IsOuterNet)
                client.SetHostPort(AppConst.GameSocketAddress, (ushort)AppConst.GameSocketPort);
            else
                client.SetHostPort(AppConst.LocalSocketAddress, (ushort)AppConst.LocalSocketPort);
            //初始化DLL
            client.InitQuicDLL();

            //DLL链接
            client.ConnectQuicDLL();
        }

        public void Close()
        {
            if (client != null)
            {
                if (thread != null)
                    thread.Abort();

                thread = null;
                client.Dispose();
                client = null;
            }
        }


        public void OnConnected(QUIC_STATUS Status)
        {
            if (Status > QUIC_STATUS.QUIC_STATUS_SUCCESS && 459749 != (int)Status)
            {
                return;
            }
            else
            {
                client.IsConnected = true;
                checkConnected = true;

                thread = new Thread(new ThreadStart(FixedSendDataGram));
                thread.Start();
            }
        }

        private void FixedSendDataGram()
        {
            while (client != null && client.IsConnected)
            {
                client.SendDatagram();
                Thread.Sleep(2000);
            }
        }

    }
}