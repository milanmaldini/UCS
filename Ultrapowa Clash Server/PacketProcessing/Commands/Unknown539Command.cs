﻿using System;
using System.IO;
using System.Text;
using UCS.Helpers;
using UCS.Logic;

namespace UCS.PacketProcessing
{
    //Commande 0x21B
    internal class Unknown539Command : Command
    {
        public byte[] packet;
        public Unknown539Command(BinaryReader br)
        {
            //packet = br.ReadAllBytes();
            Unknown1 = br.ReadUInt32WithEndian();
            Unknown2 = br.ReadUInt32WithEndian();
        }

        //00 00 00 02 00 00 02 1B 00 00 00 0C 00 00 00 00 00 00 02 1B 00 00 00 0D 00 00 00 00

        public uint Unknown1 { get; set; } //00 00 00 0C
        public uint Unknown2 { get; set; } //00 00 00 00

        public override void Execute(Level level)
        {

        }
    }
}