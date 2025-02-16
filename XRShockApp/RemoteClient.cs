using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ShockCollar

{
    internal class RemoteClient
    {
        public string ServerAdress;
        public int ServerPort;
        public string PeerID;
        public string PeerPublicKey;
        public string PeerPrivateKey;
        public bool IsConnected;

        HttpListener listener;
        
      public RemoteClient()
        {

        }
        public void ConnectToServer()
        {
            listener = new HttpListener();
            listener.Start();

        }
    }
}
