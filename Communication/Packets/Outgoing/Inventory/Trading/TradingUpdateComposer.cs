using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using Plus.HabboHotel.Rooms.Trading;
using Plus.HabboHotel.Items;

namespace Plus.Communication.Packets.Outgoing.Inventory.Trading
{
    class TradingUpdateComposer : ServerPacket
    {
        public TradingUpdateComposer(Trade Trade)
            : base(ServerPacketHeader.TradingUpdateMessageComposer)
        {
            foreach (TradeUser User in Trade.Users.ToList())
            {
                base.WriteInteger(User.GetClient().GetHabbo().Id);
                base.WriteInteger(User.OfferedItems.Count);

                foreach (Item Item in User.OfferedItems.ToList())
                {
                    base.WriteInteger(Item.Id);
                   base.WriteString(Item.GetBaseItem().Type.ToString().ToLower());
                    base.WriteInteger(Item.Id);
                    base.WriteInteger(Item.Data.SpriteId);
                    base.WriteInteger(0);//Not sure.
                    if (Item.LimitedNo > 0)
                    {
                        base.WriteBoolean(false);//Stackable
                        base.WriteInteger(256);
                       base.WriteString("");
                        base.WriteInteger(Item.LimitedNo);
                        base.WriteInteger(Item.LimitedTot);
                    }
                    else
                    {
                        base.WriteBoolean(true);//Stackable
                        base.WriteInteger(0);
                       base.WriteString("");
                    }

                    base.WriteInteger(0);
                    base.WriteInteger(0);
                    base.WriteInteger(0);

                    if (Item.GetBaseItem().Type == 's')
                        base.WriteInteger(0);
                }
            }
        }
    }
}