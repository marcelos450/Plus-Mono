﻿using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using Plus.HabboHotel.Global;
using Plus.HabboHotel.Rooms;
using Plus.HabboHotel.Quests;

using Plus.Communication.Packets.Outgoing.Rooms.Engine;
using Plus.Database.Interfaces;
using Plus.Communication.Packets.Outgoing.Moderation;

namespace Plus.Communication.Packets.Incoming.Users
{
    class UpdateFigureDataEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            if (Session == null || Session.GetHabbo() == null)
                return;

            string Gender = Packet.PopString().ToUpper();
            string Look = PlusEnvironment.GetGame().GetAntiMutant().RunLook(Packet.PopString());

            if (Look == Session.GetHabbo().Look)
                return;

            if ((DateTime.Now - Session.GetHabbo().LastClothingUpdateTime).TotalSeconds <= 2.0)
            {
                Session.GetHabbo().ClothingUpdateWarnings += 1;
                if (Session.GetHabbo().ClothingUpdateWarnings >= 25)
                    Session.GetHabbo().SessionClothingBlocked = true;
                return;
            }

            if (Session.GetHabbo().SessionClothingBlocked)
                return;

            Session.GetHabbo().LastClothingUpdateTime = DateTime.Now;

            string[] AllowedGenders = { "M", "F" };
            if (!AllowedGenders.Contains(Gender))
            {
                Session.SendMessage(new BroadcastMessageAlertComposer("Sorry, you chose an invalid gender."));
                return;
            }

            PlusEnvironment.GetGame().GetQuestManager().ProgressUserQuest(Session, QuestType.PROFILE_CHANGE_LOOK);

            Session.GetHabbo().Look = PlusEnvironment.FilterFigure(Look);
            Session.GetHabbo().Gender = Gender.ToLower();

            using (IQueryAdapter dbClient = PlusEnvironment.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.SetQuery("UPDATE users SET look = @look, gender = @gender WHERE `id` = '" + Session.GetHabbo().Id + "' LIMIT 1");
                dbClient.AddParameter("look", Look);
                dbClient.AddParameter("gender", Gender);
                dbClient.RunQuery();
            }

            PlusEnvironment.GetGame().GetAchievementManager().ProgressAchievement(Session, "ACH_AvatarLooks", 1);
            if (Session.GetHabbo().Look.Contains("ha-1006"))
                PlusEnvironment.GetGame().GetQuestManager().ProgressUserQuest(Session, QuestType.WEAR_HAT);

            if (Session.GetHabbo().InRoom)
            {
                RoomUser RoomUser = Session.GetHabbo().CurrentRoom.GetRoomUserManager().GetRoomUserByHabbo(Session.GetHabbo().Id);
                if (RoomUser != null)
                {
                    Session.SendMessage(new UserChangeComposer(RoomUser, true));
                    Session.GetHabbo().CurrentRoom.SendMessage(new UserChangeComposer(RoomUser, false));
                }
            }
        }
    }
}
