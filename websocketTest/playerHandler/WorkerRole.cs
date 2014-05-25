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

        public override bool OnStart()
        {
            // Set the maximum number of concurrent connections 
            ServicePointManager.DefaultConnectionLimit = 1;

            // For information on handling configuration changes
            // see the MSDN topic at http://go.microsoft.com/fwlink/?LinkId=166357.

            m_gameManager = new GameManager();
            m_netManager = new NetworkManager(m_gameManager);

            return base.OnStart();
        }

        public override void Run()
        {
            Trace.TraceInformation("playerHandler entry point called");

            while (true)
            {        
                m_gameManager.Update();
                m_netManager.Update();
                //Trace.WriteLine(length);
                Thread.Sleep(1000 / 30);
            }
        }







    }

    
}
