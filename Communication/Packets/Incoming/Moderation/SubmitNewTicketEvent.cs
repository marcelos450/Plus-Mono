using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using Plus.HabboHotel.Support;
using Plus.HabboHotel.Rooms.Chat.Moderation;
using Plus.Communication.Packets.Outgoing.Moderation;

namespace Plus.Communication.Packets.Incoming.Moderation
{
    class SubmitNewTicketEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            if (Session == null || Session.GetHabbo() == null)
                return;

            if (PlusEnvironment.GetGame().GetModerationTool().UsersHasPendingTicket(Session.GetHabbo().Id))
            {
                Session.SendMessage(new BroadcastMessageAlertComposer("You currently already have a pending ticket, please wait for a response from a moderator."));
                return;
            }

            string Message = Packet.PopString();
            int Type = Packet.PopInt();
            int ReportedUser = Packet.PopInt();
            int Room = Packet.PopInt();

            int Messagecount = Packet.PopInt();
            List<string> Chats = new List<string>();
            for (int i = 0; i < Messagecount; i++)
            {
                Packet.PopInt();
                Chats.Add(Packet.PopString());
            }

            ModerationRoomChatLog Chat = new ModerationRoomChatLog(Packet.PopInt(), Chats);

            PlusEnvironment.GetGame().GetModerationTool().SendNewTicket(Session, Type, ReportedUser, Message, Chats);
            PlusEnvironment.GetGame().GetClientManager().ModAlert("A new support ticket has been submitted!");

        }
    }
}
