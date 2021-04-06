using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Framework
{
    public class QuicClientPtr
    {
        protected IntPtr L = new IntPtr();

        public void MsQuicInitSet(string RegName, string AlpnName, UInt64 IdleTimeOutMs, QUIC_TEST tetFunc)
        {
            QuicDLL.MsQuicInitSet(RegName, AlpnName, IdleTimeOutMs, tetFunc);
        }

        public QUIC_STATUS MsQuicOpen(out IntPtr L)
        {
            return QuicDLL.MsQuicOpen(out L);
        }

        public QUIC_STATUS MsQuicConnectServer(string host, int port)
        {
            return QuicDLL.MsQuicConnectServer(host, port);
            //return QuicDLL.MsQuicConnectServer(Handler);
        }

        public void MsQuicRegisterConnectCallBack(QUIC_CONNECTION_CONNECTED_HANDLER Connected,
                                                                        QUIC_CONNECTION_EVENT_SHUTDOWN_INITIATED_BY_TRANSPORT_HANDLER Transport,
                                                                        QUIC_CONNECTION_EVENT_SHUTDOWN_INITIATED_BY_PEER_HANDLER Peer,
                                                                        QUIC_CONNECTION_EVENT_SHUTDOWN_COMPLETE_HANDLER Complete,
                                                                        QUIC_CONNECTION_EVENT_RESUMPTION_TICKET_RECEIVED_HANDLER Received)
        {
            QuicDLL.MsQuicRegisterConnectCallBack(Connected, Transport, Peer, Complete, Received);
        }

        public void MsQuicClearConnectCallBack()
        {
            QuicDLL.MsQuicClearConnectCallBack();
        }

        public void MsQuicRegisterStreamCallBack(QUIC_STREAM_EVENT_SEND_COMPLETE_HANDLER Complete,
                                                                QUIC_STREAM_EVENT_RECEIVE_HANDLER Receive,
                                                                QUIC_STREAM_EVENT_PEER_SEND_ABORTED_HANDLER Aborted,
                                                                QUIC_STREAM_EVENT_PEER_SEND_SHUTDOWN_HANDLER Shutdown,
                                                                QUIC_STREAM_EVENT_SEND_SHUTDOWN_COMPLETE_HANDLER eventShutDownComplete,
                                                                QUIC_STREAM_EVENT_SHUTDOWN_COMPLETE_HANDLER ShutdownComplte
                                                               )
        {
            QuicDLL.MsQuicRegisterStreamCallBack(Complete, Receive, Aborted, Shutdown, eventShutDownComplete, ShutdownComplte);
        }

        public void MsQuicClearStreamCallBack()
        {
            QuicDLL.MsQuicClearStreamCallBack();
        }

        public QUIC_STATUS ClientSend(UInt32 SendDataLength, byte[] SendData)
        {
            return QuicDLL.ClientSend(SendDataLength, SendData);
        }

        public void MsQuicCloseConnect(bool isDestory)
        {
            QuicDLL.MsQuicCloseConnect(isDestory);
        }

        public void ClientSendDatagram()
        {
            QuicDLL.ClientSendDatagram();
        }
    }
}