﻿using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

namespace Plus.Communication.Packets.Outgoing.Inventory.Trading
{
    class TradingAcceptComposer : ServerPacket
    {
        public TradingAcceptComposer(int UserId, bool Accept)
            : base(ServerPacketHeader.TradingAcceptMessageComposer)
        {
            base.WriteInteger(UserId);
            base.WriteInteger(Accept ? 1 : 0);
        }
    }
}
