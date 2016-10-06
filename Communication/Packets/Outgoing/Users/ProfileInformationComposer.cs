using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using Plus.Utilities;
using Plus.HabboHotel.Users;
using Plus.HabboHotel.Groups;
using Plus.HabboHotel.GameClients;

namespace Plus.Communication.Packets.Outgoing.Users
{
    class ProfileInformationComposer : ServerPacket
    {
        public ProfileInformationComposer(Habbo Data, GameClient Session, List<Group> Groups, int friendCount)
            : base(ServerPacketHeader.ProfileInformationMessageComposer)
        {
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(Data.AccountCreated);

            base.WriteInteger(Data.Id);
            base.WriteString(Data.Username);
            base.WriteString(Data.Look);
            base.WriteString(Data.Motto);
            base.WriteString(origin.ToString("dd/MM/yyyy"));
            base.WriteInteger(Data.GetStats().AchievementPoints);
            base.WriteInteger(friendCount); // Friend Count
            base.WriteBoolean(Data.Id != Session.GetHabbo().Id && Session.GetHabbo().GetMessenger().FriendshipExists(Data.Id)); //  Is friend
            base.WriteBoolean(Data.Id != Session.GetHabbo().Id && !Session.GetHabbo().GetMessenger().FriendshipExists(Data.Id) && Session.GetHabbo().GetMessenger().RequestExists(Data.Id)); // Sent friend request
            base.WriteBoolean((PlusEnvironment.GetGame().GetClientManager().GetClientByUserID(Data.Id)) != null);

            base.WriteInteger(Groups.Count);
            foreach (Group Group in Groups)
            {
                base.WriteInteger(Group.Id);
                base.WriteString(Group.Name);
                base.WriteString(Group.Badge);
                base.WriteString(PlusEnvironment.GetGame().GetGroupManager().GetGroupColour(Group.Colour1, true));
                base.WriteString(PlusEnvironment.GetGame().GetGroupManager().GetGroupColour(Group.Colour2, false));
                base.WriteBoolean(Data.GetStats().FavouriteGroupId == Group.Id); // todo favs
                base.WriteInteger(0);//what the fuck
                base.WriteBoolean(Group != null ? Group.ForumEnabled : true);//HabboTalk
            }

            base.WriteInteger(Convert.ToInt32(PlusEnvironment.GetUnixTimestamp() - Data.LastOnline)); // Last online
            base.WriteBoolean(true); // Show the profile
        }
    }
}