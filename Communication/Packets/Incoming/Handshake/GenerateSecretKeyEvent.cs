﻿using System;

using Plus.Communication.Packets.Incoming;
using Plus.Utilities;
using Plus.HabboHotel.GameClients;

using Plus.Communication.Encryption;
using Plus.Communication.Encryption.Crypto.Prng;
using Plus.Communication.Packets.Outgoing.Handshake;

namespace Plus.Communication.Packets.Incoming.Handshake
{
    public class GenerateSecretKeyEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            string CipherPublickey = Packet.PopString();
           
            BigInteger SharedKey = HabboEncryptionV2.CalculateDiffieHellmanSharedKey(CipherPublickey);
            if (SharedKey != 0)
            {
                Session.RC4Client = new ARC4(SharedKey.getBytes());
                Session.SendMessage(new SecretKeyComposer(HabboEncryptionV2.GetRsaDiffieHellmanPublicKey()));
            }
            else 
            {
                Session.SendNotification("There was an error logging you in, please try again!");
                return;
            }
        }
    }
}