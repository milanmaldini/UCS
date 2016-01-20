using System.Collections.Generic;
using System.Linq;
using UCS.Helpers;
using UCS.Logic;

namespace UCS.PacketProcessing
{
    //Packet 24311
    class AllianceStreamMessage : Message
    {
        private Alliance m_vAlliance;

        public AllianceStreamMessage(Client client, Alliance alliance)
            : base(client)
        {
            SetMessageType(24311);
            m_vAlliance = alliance;
        }

        public override void Encode()
        {
            var pack = new List<byte>();

            var chatMessages = m_vAlliance.GetChatMessages().ToList(); //avoid concurrent access issues

            pack.AddInt32(chatMessages.Count);
            foreach (var chatMessage in chatMessages)
            {
                pack.AddRange(chatMessage.Encode());
            }

            SetData(pack.ToArray());
        }
    }
}