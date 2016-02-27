using Sodium;

namespace UCS.Proxy
{
    public class ServerState : State
    {
        public byte[] clientKey, nonce, sessionKey, sharedKey;
        public ClientState clientState;

        public KeyPair serverKey;
    }
}