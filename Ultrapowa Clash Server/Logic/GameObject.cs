using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Windows;
using UCS.GameFiles;

namespace UCS.Logic
{
    class GameObject
    {
        private List<Component> m_vComponents;
        private Data m_vData;
        private Level m_vLevel; //a1 + 8

        public GameObject(Data data, Level level)
        {
            m_vLevel = level;
            m_vData = data;
            m_vComponents = new List<Component>();
            for (var i = 0; i < 11; i++)
                m_vComponents.Add(new Component());
        }

        public int X { get; set; }
        public int Y { get; set; }
        public int GlobalId { get; set; } //a1 + 4

        public virtual int ClassId
        {
            get { return -1; }
        }

        public void AddComponent(Component c)
        {
            if (m_vComponents[c.Type].Type != -1)
            {
                //ignore, component already set
            }
            else
            {
                m_vLevel.GetComponentManager().AddComponent(c);
                m_vComponents[c.Type] = c;
            }
        }

        public Component GetComponent(int index, bool test)
        {
            Component result = null;
            if (!test || m_vComponents[index].IsEnabled())
                result = m_vComponents[index];
            return result;
        }

        public Data GetData()
        {
            return m_vData;
        }

        public Level GetLevel()
        {
            return m_vLevel;
        }

        public Vector GetPosition()
        {
            return new Vector(X, Y);
        }

        public virtual bool IsHero()
        {
            return false;
        }

        public void SetPositionXY(int newX, int newY)
        {
            X = newX;
            Y = newY;
        }

        public virtual void Tick()
        {
        }

        public JObject Save(JObject jsonObject)
        {
            jsonObject.Add("x", X);
            jsonObject.Add("y", Y);
            foreach (var c in m_vComponents)
                c.Save(jsonObject);
            return jsonObject;
        }

        public void Load(JObject jsonObject)
        {
            X = jsonObject["x"].ToObject<int>();
            Y = jsonObject["y"].ToObject<int>();
            foreach (var c in m_vComponents)
                c.Load(jsonObject);
        }
    }
}