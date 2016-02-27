using Sodium;

namespace UCS.Proxy
{
    public class ClientState : State
    {
        public KeyPair clientKey;
        public byte[] serverKey, nonce;
        public ServerState serverState;
    }
}