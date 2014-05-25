using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace playerHandler
{
    class GameManager
    {
        List<Entity> m_allEntities = new List<Entity>();
        Stack<Entity> m_toAdd = new Stack<Entity>();
        Stack<Entity> m_toRemove = new Stack<Entity>();

        public GameManager()
        {

        }

        public void AddEntity(Entity e)
        {
            m_toAdd.Push(e);
            e.OnAttach(this);
        }

        public void RemoveEntity(Entity e)
        {
            m_toRemove.Push(e);
            e.OnRemove();
        }

        public virtual void Update()
        {
            // remove
            for (int i = m_toRemove.Count - 1; i >= 0; i--)
            {
                m_allEntities.Remove(m_toRemove.Pop());
            }
            // add
            for (int i = m_toAdd.Count - 1; i >= 0; i--)
            {
                m_allEntities.Add(m_toAdd.Pop());
            }
            // update
            for (int i = 0; i < m_allEntities.Count; i++)
            {
                m_allEntities[i].Update(0.1f); // needs actual time
            }
        }

        public override string ToString()
        {
            int length = m_allEntities.Count();
            string output = ".";
            int numbertoupdate = 0;
            for (int i = 0; i < length; i++)
            {
                if (m_allEntities[i].m_updated)
                {
                    numbertoupdate++;
                    output += m_allEntities[i].ToString();
                    if (i != length - 1) output += ",";
                    m_allEntities[i].m_updated = false;
                }
            }
            if (numbertoupdate == 0)
                return ""; // no need to send out updates
            else
                return numbertoupdate + output; // put the number to update at the front of the string.
        }
    }

    public struct Vector2
    {
        public int x;
        public int y;
        public Vector2(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }
}
