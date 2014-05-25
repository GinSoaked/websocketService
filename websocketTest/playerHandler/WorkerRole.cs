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
    // One worker role = one game server
    // OnStart is the entry point
    public class WorkerRole : RoleEntryPoint
    {
        GameManager m_gameManager; // holds all entities
        NetworkManager m_netManager; // handles client interaction
        World m_world; // game logic

        double m_networkPollTimer = 0; // update clients 25 times a second
        double m_networkPollFreq = (double)1 / (double)25;

        DateTime m_currentTime, m_previousTime;
        public override bool OnStart()
        {
            // Set the maximum number of concurrent connections 
            ServicePointManager.DefaultConnectionLimit = 1;

            // For information on handling configuration changes
            // see the MSDN topic at http://go.microsoft.com/fwlink/?LinkId=166357.

            m_gameManager = new GameManager();
            m_netManager = new NetworkManager(m_gameManager);
            m_world = new World(m_gameManager);
            return base.OnStart();
        }

        public override void Run()
        {
            Trace.TraceInformation("playerHandler entry point called");
            TimeSpan ts;

            ////Thread worldThread = new Thread(new ThreadStart(RunWorld));
            //Thread networkingThread = new Thread(new ThreadStart(RunNetworking));
            double dt = 0;
            ////worldThread.Start();
            //networkingThread.Start();

            while (true)
            {
                m_currentTime = DateTime.Now;
                ts = m_currentTime - m_previousTime;
                dt = ts.TotalMilliseconds / 1000;
                m_world.Update(dt);
                m_gameManager.Update(dt);

                m_networkPollTimer -= dt;
                if (m_networkPollTimer <= 0)
                {
                    m_netManager.Update();
                    m_networkPollTimer = m_networkPollFreq;
                }


                m_previousTime = m_currentTime;
            }
        }

        //private void RunWorld()
        //{
        //    while (true)
        //    {
        //        //if (m_gameManager.AquireLock())
        //        {
        //            m_world.Update(dt);
        //            //m_gameManager.ReleaseLock();
        //        }
        //    }     
        //}

        private void RunNetworking()
        {
            TimeSpan ts;
            double dt = 0;
            while (true)
            {
                m_currentTime = DateTime.Now;
                ts = m_currentTime - m_previousTime;
                dt = ts.TotalMilliseconds / 1000;
                m_networkPollTimer -= dt;
                if (m_networkPollTimer <= 0)
                {
                    if (m_gameManager.AquireLock())
                    {

                        m_netManager.Update();
                        m_networkPollTimer = m_networkPollFreq;
                        m_gameManager.ReleaseLock();
                    }
                }
                
                m_previousTime = m_currentTime;
            }
        }

    }
}
