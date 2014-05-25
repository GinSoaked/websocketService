using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;


namespace playerHandler
{
    enum EntityType
    {
        player,
        enemy
    }
    class Entity
    {
        public GameManager m_Parent = null;
        public bool m_updated = true; // this ensures that only the entities that have been updated need to be synced with the client
        public Vector2 m_position;
        public int m_entityID;
        public EntityType m_type;
        public Entity(int id, EntityType type, Vector2 position )
        {
            m_entityID = id;
            m_type = type;
            Trace.WriteLine(m_type.ToString());
            m_position = new Vector2(position.x, position.y);
        }

        public virtual void OnAttach(GameManager g)
        {
            m_Parent = g;
        }

        public virtual void OnRemove()
        {

        }

        public virtual void Update(double dt)
        {
            if(m_type == EntityType.enemy)
            {
                m_position.y += (float)(20.0f * (float)dt);// (int)(0.01f * dt);
                m_updated = true;
                
            }
        }

        public override string ToString()
        {
            return
                m_entityID + " " +
                Math.Round(m_position.x, 1) + " " +
                Math.Round(m_position.y, 1);
        }
    }
}
