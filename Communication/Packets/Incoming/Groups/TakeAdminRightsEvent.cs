﻿using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using Plus.HabboHotel.Users;
using Plus.HabboHotel.Rooms;
using Plus.HabboHotel.Groups;
using Plus.Communication.Packets.Outgoing.Groups;
using Plus.Communication.Packets.Outgoing.Rooms.Permissions;
using Plus.HabboHotel.Cache;



namespace Plus.Communication.Packets.Incoming.Groups
{
    class TakeAdminRightsEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            int GroupId = Packet.PopInt();
            int UserId = Packet.PopInt();

            Group Group = null;
            if (!PlusEnvironment.GetGame().GetGroupManager().TryGetGroup(GroupId, out Group))
                return;

            if (Session.GetHabbo().Id != Group.CreatorId || !Group.IsMember(UserId))
                return;

            Habbo Habbo = PlusEnvironment.GetHabboById(UserId);
            if (Habbo == null)
            {
                Session.SendNotification("Oops, an error occurred whilst finding this user.");
                return;
            }

            Group.TakeAdmin(UserId);

            Room Room = null;
            if (PlusEnvironment.GetGame().GetRoomManager().TryGetRoom(Group.RoomId, out Room))
            {
                RoomUser User = Room.GetRoomUserManager().GetRoomUserByHabbo(UserId);
                if (User != null)
                {
                    if (User.Statusses.ContainsKey("flatctrl 3"))
                        User.RemoveStatus("flatctrl 3");
                    User.UpdateNeeded = true;
                    if (User.GetClient() != null)
                        User.GetClient().SendMessage(new YouAreControllerComposer(0));
                }
            }

            Session.SendMessage(new GroupMemberUpdatedComposer(GroupId, Habbo, 2));
        }
    }
}
