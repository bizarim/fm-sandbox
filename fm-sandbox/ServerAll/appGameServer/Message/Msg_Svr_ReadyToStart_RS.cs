﻿using fmCommon;
using fmLibrary;
using fmServerCommon;

namespace appGameServer
{
    public class Msg_Svr_ReadyToStart_RS : IMessage
    {
        Session m_session = null;
        Packet m_recvPacket = null;

        public Msg_Svr_ReadyToStart_RS(appServer server, SessionBase session, Packet packet)
        {
            m_server = server;
            m_recvPacket = packet;
            m_session = session as Session;
        }

        public override void Process()
        {
            using (var recvfmProtocol = new PT_Server_ReadyToStart_RS())
            {
                recvfmProtocol.Deserialize(m_recvPacket);

                if (eErrorCode.Success != recvfmProtocol.m_eErrorCode)
                {
                    Logger.Error("Fail PT_Server_ReadyToStart_RS");
                    return;
                }

                m_server.Start();
            }
        }
        protected override void Release()
        {
            m_server = null;
            m_session = null;
            m_recvPacket = null;
        }
        public override void Exclude()
        {
            if (null != m_session)
                m_session.ForceDisconnect(CloseReason.ThreadExclude);
        }
    }
}
