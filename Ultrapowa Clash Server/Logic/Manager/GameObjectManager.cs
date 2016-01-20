using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using UCS.Core;
using UCS.GameFiles;

namespace UCS.Logic
{
    class GameObjectManager
    {
        private ComponentManager m_vComponentManager;
        private List<List<GameObject>> m_vGameObjects;
        private List<int> m_vGameObjectsIndex;
        private Level m_vLevel;

        public GameObjectManager(Level l)
        {
            m_vLevel = l;
            m_vGameObjects = new List<List<GameObject>>();
            m_vGameObjectsIndex = new List<int>();
            for (var i = 0; i < 7; i++)
            {
                m_vGameObjects.Add(new List<GameObject>());
                m_vGameObjectsIndex.Add(0);
            }
            m_vComponentManager = new ComponentManager(m_vLevel);
        }

        public ComponentManager GetComponentManager()
        {
            return m_vComponentManager;
        }

        public GameObject GetGameObjectByID(int id)
        {
            var classId = GlobalID.GetClassID(id) - 500;
            return m_vGameObjects[classId].Find(g => g.GlobalId == id);
        }

        public List<GameObject> GetGameObjects(int id)
        {
            return m_vGameObjects[id];
        }

        public void AddGameObject(GameObject go)
        {
            go.GlobalId = GenerateGameObjectGlobalId(go);
            if (go.ClassId == 0)
            {
                var b = (Building) go;
                var bd = b.GetBuildingData();
                if (bd.IsWorkerBuilding())
                {
                    m_vLevel.WorkerManager.IncreaseWorkerCount();
                }
            }
            m_vGameObjects[go.ClassId].Add(go);
        }

        public void RemoveGameObject(GameObject go)
        {
            m_vGameObjects[go.ClassId].Remove(go);
            if (go.ClassId == 0)
            {
                var b = (Building) go;
                var bd = b.GetBuildingData();
                if (bd.IsWorkerBuilding())
                {
                    m_vLevel.WorkerManager.DecreaseWorkerCount();
                }
            }
            RemoveGameObjectReferences(go);
        }

        public void RemoveGameObjectReferences(GameObject go)
        {
            m_vComponentManager.RemoveGameObjectReferences(go);
        }

        private int GenerateGameObjectGlobalId(GameObject go)
        {
            var index = m_vGameObjectsIndex[go.ClassId];
            m_vGameObjectsIndex[go.ClassId]++;
            return GlobalID.CreateGlobalID(go.ClassId + 500, index);
        }

        public void Tick()
        {
            m_vComponentManager.Tick();
            foreach (var l in m_vGameObjects)
            {
                foreach (var go in l)
                    go.Tick();
            }
        }

        public JObject Save()
        {
            var jsonData = new JObject();

            //Buildings
            var jsonBuildingsArray = new JArray();
            foreach (var go in new List<GameObject>(m_vGameObjects[0]))
            {
                var b = (Building) go;
                var jsonObject = new JObject();
                jsonObject.Add("data", b.GetBuildingData().GetGlobalID());
                b.Save(jsonObject);
                jsonBuildingsArray.Add(jsonObject);
            }
            jsonData.Add("buildings", jsonBuildingsArray);

            //Traps
            var jsonTrapsArray = new JArray();
            foreach (var go in new List<GameObject>(m_vGameObjects[4]))
            {
                var t = (Trap) go;
                var jsonObject = new JObject();
                jsonObject.Add("data", t.GetTrapData().GetGlobalID());
                t.Save(jsonObject);
                jsonTrapsArray.Add(jsonObject);
            }
            jsonData.Add("traps", jsonTrapsArray);

            //Decos
            var jsonDecosArray = new JArray();
            foreach (var go in new List<GameObject>(m_vGameObjects[6]))
            {
                var d = (Deco) go;
                var jsonObject = new JObject();
                jsonObject.Add("data", d.GetDecoData().GetGlobalID());
                d.Save(jsonObject);
                jsonDecosArray.Add(jsonObject);
            }
            jsonData.Add("decos", jsonDecosArray);

            return jsonData;
        }

        public void Load(JObject jsonObject)
        {
            var jsonBuildings = (JArray) jsonObject["buildings"];
            foreach (JObject jsonBuilding in jsonBuildings)
            {
                var bd = (BuildingData) ObjectManager.DataTables.GetDataById(jsonBuilding["data"].ToObject<int>());
                var b = new Building(bd, m_vLevel);
                AddGameObject(b);
                b.Load(jsonBuilding);
            }

            var jsonTraps = (JArray) jsonObject["traps"];
            foreach (JObject jsonTrap in jsonTraps)
            {
                var td = (TrapData) ObjectManager.DataTables.GetDataById(jsonTrap["data"].ToObject<int>());
                var t = new Trap(td, m_vLevel);
                AddGameObject(t);
                t.Load(jsonTrap);
            }

            var jsonDecos = (JArray) jsonObject["decos"];
            foreach (JObject jsonDeco in jsonDecos)
            {
                var dd = (DecoData) ObjectManager.DataTables.GetDataById(jsonDeco["data"].ToObject<int>());
                var d = new Deco(dd, m_vLevel);
                AddGameObject(d);
                d.Load(jsonDeco);
            }
        }
    }
}