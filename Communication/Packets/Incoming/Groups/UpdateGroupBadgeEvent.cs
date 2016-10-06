using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using Plus.HabboHotel.Groups;
using Plus.Communication.Packets.Outgoing.Groups;
using Plus.Database.Interfaces;


namespace Plus.Communication.Packets.Incoming.Groups
{
    class UpdateGroupBadgeEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            int GroupId = Packet.PopInt();

            Group Group = null;
            if (!PlusEnvironment.GetGame().GetGroupManager().TryGetGroup(GroupId, out Group))
                return;

             if (Group.CreatorId != Session.GetHabbo().Id)
                return;

            int Count = Packet.PopInt();
            int Current = 1;
        
            string x;
            string newBadge = "";
            while (Current <= Count)
            {
                int Id = Packet.PopInt();
                int Colour = Packet.PopInt();
                int Pos = Packet.PopInt();
                if (Current == 1)
                    x = "b" + ((Id < 10) ? "0" + Id.ToString() : Id.ToString()) +     ((Colour < 10) ? "0" + Colour.ToString() : Colour.ToString()) + Pos;
                else
                    x = "s" + ((Id < 10) ? "0" + Id.ToString() : Id.ToString()) +   ((Colour < 10) ? "0" + Colour.ToString() : Colour.ToString()) + Pos;
                newBadge += PlusEnvironment.GetGame().GetGroupManager().CheckActiveSymbol(x);
                Current++;
            }

            using (IQueryAdapter dbClient = PlusEnvironment.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.SetQuery("UPDATE groups SET badge = @badge WHERE id=" + Group.Id + " LIMIT 1");
                dbClient.AddParameter("badge", newBadge);
                dbClient.RunQuery();
            }

            Group.Badge = (string.IsNullOrWhiteSpace(newBadge) ? "b05114s06114" : newBadge);
            Session.SendMessage(new GroupInfoComposer(Group, Session));
        }
    }
}
