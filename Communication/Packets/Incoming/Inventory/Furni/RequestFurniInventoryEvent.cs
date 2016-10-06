using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using Plus.HabboHotel.Items;
using Plus.Communication.Packets.Outgoing.Inventory.Furni;



namespace Plus.Communication.Packets.Incoming.Inventory.Furni
{
    class RequestFurniInventoryEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            ICollection<Item> FloorItems = Session.GetHabbo().GetInventoryComponent().GetFloorItems();
            ICollection<Item> WallItems = Session.GetHabbo().GetInventoryComponent().GetWallItems();

            if (Session.GetHabbo().InventoryAlert == false)
            {
                Session.GetHabbo().InventoryAlert = true;
                int TotalCount = FloorItems.Count + WallItems.Count;
                if (TotalCount >= 5000)
                {
                    Session.SendNotification("Hey! Our system has detected that you have a very large inventory!\n\n" +
                        "The maximum an inventory can load is 8000 items, you have " + TotalCount + " items loaded now.\n\n" +
                        "If you have 8000 loaded now then you're probably over the limit and some items will be hidden until you free up space.\n\n" +
                        "Please note that we are not responsible for you crashing because of too large inventorys!");
                }
            }

           
            Session.SendMessage(new FurniListComposer(FloorItems.ToList(), WallItems));
        }
    }
}
