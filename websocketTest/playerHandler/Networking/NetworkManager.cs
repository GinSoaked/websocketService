using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Threading;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Diagnostics;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.Storage;
using System.Security.Cryptography;
using Fleck;

namespace playerHandler
{
    class NetworkManager
    {
        List<Client> m_players = new List<Client>();
        public WebSocketServer m_server = null;
        public GameManager m_gameManager = null;
        int m_nextIDtoUse = 0;
        public NetworkManager(GameManager game)
        {
            m_gameManager = game;
            var server = new WebSocketServer("ws://" +
                RoleEnvironment.CurrentRoleInstance.InstanceEndpoints["access"].IPEndpoint.Address.ToString() + ":" +
                RoleEnvironment.CurrentRoleInstance.InstanceEndpoints["access"].IPEndpoint.Port.ToString());
            server.Start(socket =>
            {
                socket.OnOpen = () =>
                {

                    m_players.Add(new Client(socket, m_nextIDtoUse, m_gameManager));
                    //Trace.WriteLine("Opened connection to client " + m_nextIDtoUse);
                    m_nextIDtoUse++;

                };

                socket.OnClose = () =>
                {

                    //Trace.WriteLine("Close!");
                };

            });

            m_server = server;
        }

        public void Update()
        {
            int length = m_players.Count();
            for (int i = 0; i < length; i++)
            {
                if (m_players[i].m_disconnected)
                {
                    closeConnection(m_players[i]);

                    break;
                }
            }

            length = m_players.Count();
            string output = m_gameManager.ToString();
            //bytesSent = 0;
            if (output != "") // only send data if there is something to update
            {
                for (int i = 0; i < length; i++)
                {
                    //   thisId.numberOfClients.idn typen posx posy,idn+1 typen+1 posx posy
                    string output2 = m_players[i].m_myEntity.m_entityID + "." + output;
                    //Trace.WriteLine(output2);
                    m_players[i].Send(output2);
                   // bytesSent += output2.Length;
                }
            }
        }

        private void closeConnection(Client p)
        {
            p.CloseConnection();
            m_gameManager.RemoveEntity(p.m_myEntity);
            m_players.Remove(p);
        }
    }
}
