using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fleck;

namespace playerHandler
{
    class Client
    {

        public IWebSocketConnection m_socket;
        public bool m_disconnected = false;
        public Entity m_myEntity;

        public Client(IWebSocketConnection socket, int id, GameManager manager)
        {
            m_socket = socket;
            m_socket.OnMessage = message => parseMessage(message);
            m_myEntity = new Entity(id, EntityType.player, new Vector2(200, 200));
            manager.AddEntity(m_myEntity);
        }


        private void parseMessage(string message)
        {
            if (message == "left")
            {
                m_myEntity.m_position.x -= 4;
            }
            else if (message == "right")
            {
                m_myEntity.m_position.x += 4;
            }
            else if (message == "up")
            {
                m_myEntity.m_position.y -= 4;
            }
            else if (message == "down")
            {
                m_myEntity.m_position.y += 4;
            }
            m_myEntity.m_updated = true;
        }

        public void Send(string output)
        {
            if (m_socket.IsAvailable)
                m_socket.Send(output);
            else m_disconnected = true;
        }

        public void CloseConnection()
        {
            m_socket.Close();

        }
    }
}
