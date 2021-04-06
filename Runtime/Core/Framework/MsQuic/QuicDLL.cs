using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Framework
{
    public enum C_ERRNO
    {
        EPERM           = 1,
        ENOENT          = 2,
        ENOMEM          = 12,
        EINVAL          = 22,
        EOVERFLOW       = 75,
        EOPNOTSUPP      = 95,
        EADDRINUSE      = 98,
        ETIMEDOUT       = 110,
        EHOSTUNREACH    = 113,
    }
    public enum QUIC_ERROR
    {
        NO_ERROR                    = 0,
        ERROR_SUCCESS               = 0,
        ERROR_CONTINUE              = -1,
        ERROR_NOT_READY             = -2,
        ERROR_BASE                  =       200000000,
        ERROR_NOT_ENOUGH_MEMORY     = 1 +   200000000,
        ERROR_INVALID_STATE         = 2 +   200000000,
        ERROR_INVALID_PARAMETER     = 3 +   200000000,
        ERROR_NOT_SUPPORTED         = 4 +   200000000,
        ERROR_NOT_FOUND             = 5 +   200000000,
        ERROR_BUFFER_OVERFLOW       = 6 +   200000000,
        ERROR_CONNECTION_REFUSED    = 7 +   200000000,
        ERROR_OPERATION_ABORTED     = 8 +   200000000,
        ERROR_HANDSHAKE_FAILURE     = 9 +   200000000,
        ERROR_NETWORK_UNREACHABLE   = 10 +  200000000,
        ERROR_CONNECTION_IDLE       = 11 +  200000000,
        ERROR_INTERNAL_ERROR        = 12 +  200000000,
        ERROR_PROTOCOL_ERROR        = 13 +  200000000,
        ERROR_VER_NEG_ERROR         = 14 +  200000000,
        ERROR_EPOLL_ERROR           = 15 +  200000000,
        ERROR_DNS_RESOLUTION_ERROR  = 16 +  200000000,
        ERROR_SOCKET_ERROR          = 17 +  200000000,
        ERROR_SSL_ERROR             = 18 +  200000000,
        ERROR_USER_CANCELED         = 19 +  200000000,
        ERROR_ALPN_NEG_FAILURE      = 20 +  200000000,
    }

    public enum QUIC_STATUS
    {
        QUIC_STATUS_SUCCESS             = QUIC_ERROR.ERROR_SUCCESS,
        QUIC_STATUS_PENDING             = QUIC_ERROR.ERROR_NOT_READY,
        QUIC_STATUS_CONTINUE            = QUIC_ERROR.ERROR_CONTINUE,
        QUIC_STATUS_OUT_OF_MEMORY       = C_ERRNO.ENOMEM,
        QUIC_STATUS_INVALID_PARAMETER   = C_ERRNO.EINVAL,
        QUIC_STATUS_INVALID_STATE       = QUIC_ERROR.ERROR_INVALID_STATE,
        QUIC_STATUS_NOT_SUPPORTED       = C_ERRNO.EOPNOTSUPP,
        QUIC_STATUS_NOT_FOUND           = C_ERRNO.ENOENT,
        QUIC_STATUS_BUFFER_TOO_SMALL    = C_ERRNO.EOVERFLOW,
        QUIC_STATUS_HANDSHAKE_FAILURE   = QUIC_ERROR.ERROR_HANDSHAKE_FAILURE,
        QUIC_STATUS_ABORTED             = QUIC_ERROR.ERROR_OPERATION_ABORTED,
        QUIC_STATUS_ADDRESS_IN_USE      = C_ERRNO.EADDRINUSE,
        QUIC_STATUS_CONNECTION_TIMEOUT  = C_ERRNO.ETIMEDOUT,
        QUIC_STATUS_CONNECTION_IDLE     = QUIC_ERROR.ERROR_CONNECTION_IDLE,
        QUIC_STATUS_INTERNAL_ERROR      = QUIC_ERROR.ERROR_INTERNAL_ERROR,
        QUIC_STATUS_CONNECTION_REFUSED  = QUIC_ERROR.ERROR_CONNECTION_REFUSED,
        QUIC_STATUS_PROTOCOL_ERROR      = QUIC_ERROR.ERROR_PROTOCOL_ERROR,
        QUIC_STATUS_VER_NEG_ERROR       = QUIC_ERROR.ERROR_VER_NEG_ERROR,
        QUIC_STATUS_UNREACHABLE         = C_ERRNO.EHOSTUNREACH,
        QUIC_STATUS_PERMISSION_DENIED   = C_ERRNO.EPERM,
        QUIC_STATUS_EPOLL_ERROR         = QUIC_ERROR.ERROR_EPOLL_ERROR,
        QUIC_STATUS_DNS_RESOLUTION_ERROR = QUIC_ERROR.ERROR_DNS_RESOLUTION_ERROR,
        QUIC_STATUS_SOCKET_ERROR         = QUIC_ERROR.ERROR_SOCKET_ERROR,
        QUIC_STATUS_TLS_ERROR            = QUIC_ERROR.ERROR_SSL_ERROR,
        QUIC_STATUS_USER_CANCELED        = QUIC_ERROR.ERROR_USER_CANCELED,
        QUIC_STATUS_ALPN_NEG_FAILURE     = QUIC_ERROR.ERROR_ALPN_NEG_FAILURE,
    }

    public enum QUIC_EXECUTION_PROFILE
    {
        QUIC_EXECUTION_PROFILE_LOW_LATENCY,         // Default
        QUIC_EXECUTION_PROFILE_TYPE_MAX_THROUGHPUT,
        QUIC_EXECUTION_PROFILE_TYPE_SCAVENGER,
        QUIC_EXECUTION_PROFILE_TYPE_REAL_TIME,
    }

    public enum QUIC_CONNECTION_EVENT_TYPE
    {
        QUIC_CONNECTION_EVENT_CONNECTED                         = 0,
        QUIC_CONNECTION_EVENT_SHUTDOWN_INITIATED_BY_TRANSPORT   = 1,    // The transport started the shutdown process.
        QUIC_CONNECTION_EVENT_SHUTDOWN_INITIATED_BY_PEER        = 2,    // The peer application started the shutdown process.
        QUIC_CONNECTION_EVENT_SHUTDOWN_COMPLETE                 = 3,    // Ready for the handle to be closed.
        QUIC_CONNECTION_EVENT_LOCAL_ADDRESS_CHANGED             = 4,
        QUIC_CONNECTION_EVENT_PEER_ADDRESS_CHANGED              = 5,
        QUIC_CONNECTION_EVENT_PEER_STREAM_STARTED               = 6,
        QUIC_CONNECTION_EVENT_STREAMS_AVAILABLE                 = 7,
        QUIC_CONNECTION_EVENT_PEER_NEEDS_STREAMS                = 8,
        QUIC_CONNECTION_EVENT_IDEAL_PROCESSOR_CHANGED           = 9,
        QUIC_CONNECTION_EVENT_DATAGRAM_STATE_CHANGED            = 10,
        QUIC_CONNECTION_EVENT_DATAGRAM_RECEIVED                 = 11,
        QUIC_CONNECTION_EVENT_DATAGRAM_SEND_STATE_CHANGED       = 12,
        QUIC_CONNECTION_EVENT_RESUMED                           = 13,   // Server-only; provides resumption data, if any.
        QUIC_CONNECTION_EVENT_RESUMPTION_TICKET_RECEIVED        = 14,   // Client-only; provides ticket to persist, if any.
        QUIC_CONNECTION_EVENT_PEER_CERTIFICATE_RECEIVED         = 15,   // Only with QUIC_CREDENTIAL_FLAG_INDICATE_CERTIFICATE_RECEIVED set
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct QUIC_BUFFER
    {
        public Int32 Length;
        public IntPtr Buffer;
    }

    //connect call back
    public delegate void QUIC_CONNECTION_CONNECTED_HANDLER(QUIC_STATUS Status);
    public delegate void QUIC_CONNECTION_EVENT_SHUTDOWN_INITIATED_BY_TRANSPORT_HANDLER();
    public delegate void QUIC_CONNECTION_EVENT_SHUTDOWN_INITIATED_BY_PEER_HANDLER();
    public delegate void QUIC_CONNECTION_EVENT_SHUTDOWN_COMPLETE_HANDLER();
    public delegate void QUIC_CONNECTION_EVENT_RESUMPTION_TICKET_RECEIVED_HANDLER();

    //stream call back
    public delegate void QUIC_STREAM_EVENT_SEND_COMPLETE_HANDLER();
    public delegate void QUIC_STREAM_EVENT_RECEIVE_HANDLER(ref QUIC_BUFFER quicBuffer);
    public delegate void QUIC_STREAM_EVENT_PEER_SEND_ABORTED_HANDLER();
    public delegate void QUIC_STREAM_EVENT_PEER_SEND_SHUTDOWN_HANDLER();
    public delegate void QUIC_STREAM_EVENT_SEND_SHUTDOWN_COMPLETE_HANDLER();
    public delegate void QUIC_STREAM_EVENT_SHUTDOWN_COMPLETE_HANDLER();

    public delegate void QUIC_TEST(Int64 value);

    public class QuicDLL
    {
        const string MSQUICDLL = "MsQuic";

        [DllImport(MSQUICDLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void MsQuicInitSet([MarshalAs(UnmanagedType.LPStr)] string RegName, [MarshalAs(UnmanagedType.LPStr)] string AlpnName, UInt64 IdleTimeOutMs, QUIC_TEST tetFunc);

        [DllImport(MSQUICDLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern QUIC_STATUS MsQuicOpen(out IntPtr L);

        [DllImport(MSQUICDLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern QUIC_STATUS MsQuicConnectServer([MarshalAs(UnmanagedType.LPStr)] string host, int port);
        //public static extern QUIC_STATUS MsQuicConnectServer(QUIC_CONNECTION_CALLBACK_HANDLER Handler);

        [DllImport(MSQUICDLL, CallingConvention = CallingConvention.Cdecl)]

        public static extern void MsQuicCloseConnect(bool destory);

        [DllImport(MSQUICDLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void MsQuicRegisterConnectCallBack(QUIC_CONNECTION_CONNECTED_HANDLER Connected,
                                                                        QUIC_CONNECTION_EVENT_SHUTDOWN_INITIATED_BY_TRANSPORT_HANDLER Transport,
                                                                        QUIC_CONNECTION_EVENT_SHUTDOWN_INITIATED_BY_PEER_HANDLER Peer,
                                                                        QUIC_CONNECTION_EVENT_SHUTDOWN_COMPLETE_HANDLER Complete,
                                                                        QUIC_CONNECTION_EVENT_RESUMPTION_TICKET_RECEIVED_HANDLER Received);

        [DllImport(MSQUICDLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void MsQuicClearConnectCallBack();

        [DllImport(MSQUICDLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void MsQuicRegisterStreamCallBack(QUIC_STREAM_EVENT_SEND_COMPLETE_HANDLER Complete,
                                                                QUIC_STREAM_EVENT_RECEIVE_HANDLER Receive,
                                                                QUIC_STREAM_EVENT_PEER_SEND_ABORTED_HANDLER Aborted,
                                                                QUIC_STREAM_EVENT_PEER_SEND_SHUTDOWN_HANDLER Shutdown,
                                                                QUIC_STREAM_EVENT_SEND_SHUTDOWN_COMPLETE_HANDLER eventShutDownComplete,
                                                                QUIC_STREAM_EVENT_SHUTDOWN_COMPLETE_HANDLER ShutdownComplte
                                                               );
        [DllImport(MSQUICDLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void MsQuicClearStreamCallBack();

        [DllImport(MSQUICDLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern QUIC_STATUS ClientSend(UInt32 SendDataLength, byte[] SendData);

        [DllImport(MSQUICDLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void ClientSendDatagram();
    }
}