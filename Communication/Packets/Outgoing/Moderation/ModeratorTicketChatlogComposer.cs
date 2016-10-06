using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using Plus.HabboHotel.Rooms;
using Plus.HabboHotel.Users;
using Plus.HabboHotel.Support;

namespace Plus.Communication.Packets.Outgoing.Moderation
{
    class ModeratorTicketChatlogComposer : ServerPacket
    {
        public ModeratorTicketChatlogComposer(SupportTicket Ticket, RoomData RoomData, double Timestamp)
            : base(ServerPacketHeader.ModeratorTicketChatlogMessageComposer)
        {
            base.WriteInteger(Ticket.TicketId);
            base.WriteInteger(Ticket.SenderId);
            base.WriteInteger(Ticket.ReportedId);
            base.WriteInteger(RoomData.Id);

            base.WriteByte(1);
            base.WriteShort(2);//Count
           base.WriteString("roomName");
            base.WriteByte(2);
           base.WriteString(RoomData.Name);
           base.WriteString("roomId");
            base.WriteByte(1);
            base.WriteInteger(RoomData.Id);

            base.WriteShort(Ticket.ReportedChats.Count);
            foreach (string Chat in Ticket.ReportedChats)
            {
                Habbo Habbo = PlusEnvironment.GetHabboById(Ticket.ReportedId);

                base.WriteInteger(((int)PlusEnvironment.GetUnixTimestamp() - Convert.ToInt32(Timestamp)) * 1000);
                base.WriteInteger(Ticket.ReportedId);
               base.WriteString(Habbo != null ? Habbo.Username : "No username");
               base.WriteString(Chat);
                base.WriteBoolean(false);
            }
        }
    }
}
