using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


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
        public bool m_updated = false; // this ensures that only the entities that have been updated need to be synced with the client
        public Vector2 m_position;
        public int m_entityID;
        public EntityType m_type;
        public Entity(int id, EntityType type, Vector2 position )
        {
            m_entityID = id;
            m_type = type;
            m_position = new Vector2(position.x, position.y);
        }

        public virtual void OnAttach(GameManager g)
        {
            m_Parent = g;
        }

        public virtual void OnRemove()
        {

        }

        public virtual void Update(float dt)
        {

        }

        public override string ToString()
        {
            return
                m_entityID + " " +
                //(int)m_type + " " +
                m_position.x + " " +
                m_position.y;
        }
    }
}
