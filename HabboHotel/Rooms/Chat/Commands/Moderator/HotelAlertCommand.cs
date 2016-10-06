﻿using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using Plus.Communication.Packets.Outgoing.Moderation;

namespace Plus.HabboHotel.Rooms.Chat.Commands.Moderator
{
    class HotelAlertCommand : IChatCommand
    {
        public string PermissionRequired
        {
            get { return "command_hotel_alert"; }
        }

        public string Parameters
        {
            get { return "%message%"; }
        }

        public string Description
        {
            get { return "Send a message to the entire hotel."; }
        }

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {
            if (Params.Length == 1)
            {
                Session.SendWhisper("Please enter a message to send.");
                return;
            }

            string Message = CommandManager.MergeParams(Params, 1);
            PlusEnvironment.GetGame().GetClientManager().SendMessage(new BroadcastMessageAlertComposer(Message + "\r\n" + "- " + Session.GetHabbo().Username));
                            
            return;
        }
    }
}
