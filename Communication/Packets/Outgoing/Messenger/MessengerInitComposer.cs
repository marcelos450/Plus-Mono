﻿using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using Plus.HabboHotel.Users;
using Plus.HabboHotel.Users.Messenger;
using Plus.HabboHotel.Users.Relationships;

namespace Plus.Communication.Packets.Outgoing.Messenger
{
    class MessengerInitComposer : ServerPacket
    {
        public MessengerInitComposer()
            : base(ServerPacketHeader.MessengerInitMessageComposer)
        {
            base.WriteInteger(PlusStaticGameSettings.MessengerFriendLimit);//Friends max.
            base.WriteInteger(300);
            base.WriteInteger(800);
            base.WriteInteger(1100);
            base.WriteInteger(0); // category count
        }
    }
}
