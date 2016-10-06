﻿using System;
using Plus.Communication.Packets.Incoming;

using Plus.HabboHotel.GameClients;
using Plus.Communication.Packets.Outgoing.Catalog;
using Plus.Communication.Packets.Outgoing.BuildersClub;

namespace Plus.Communication.Packets.Incoming.Catalog
{
    public class GetCatalogIndexEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            /*int Sub = 0;

            if (Session.GetHabbo().GetSubscriptionManager().HasSubscription)
            {
                Sub = Session.GetHabbo().GetSubscriptionManager().GetSubscription().SubscriptionId;
            }*/

            Session.SendMessage(new CatalogIndexComposer(Session, PlusEnvironment.GetGame().GetCatalog().GetPages()));//, Sub));
            Session.SendMessage(new CatalogItemDiscountComposer());
            Session.SendMessage(new BCBorrowedItemsComposer());
        }
    }
}