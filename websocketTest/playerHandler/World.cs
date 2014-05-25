using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
    class World
    {
        GameManager m_gameManager;
        double m_spawntimer = 1.0f;
        public static Random random = new Random();
        public World(GameManager gameManager)
        {
            m_gameManager = gameManager;
        }

        public void Update(double dt)
        {
            m_spawntimer -= dt;
            if (m_spawntimer <= 0)
            {
                m_gameManager.AddEntity(new Entity(m_gameManager.GetUniqueID(), EntityType.enemy, new Vector2(random.Next(10, 790), 100)));
                m_spawntimer = 1.0f;
                Trace.WriteLine("spawned");
            }
        }
    }
}
