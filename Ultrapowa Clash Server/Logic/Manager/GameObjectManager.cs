using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using UCS.Core;
using UCS.GameFiles;

namespace UCS.Logic
{
    internal class GameObjectManager
    {
        private readonly ComponentManager m_vComponentManager;
        private readonly List<List<GameObject>> m_vGameObjects;
        private readonly List<int> m_vGameObjectsIndex;
        private readonly Level m_vLevel;

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

        public JObject Save()
        {
            var jsonData = new JObject();
            jsonData.Add("android_client", true);
            jsonData.Add("active_layout",  0);
            jsonData.Add("layout_state", new JArray() { 0, 0, 0, 0, 0, 0 });

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

            // Obstacles
            jsonData.Add("obstacles", new JArray() { });

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

            // respawnVars
            var jsonRespawnObject = new JObject();
            jsonRespawnObject.Add("secondsFromLastRespawn", 195);
            jsonRespawnObject.Add("respawnSeed", -212853765);
            jsonRespawnObject.Add("obstacleClearCounter", 0);
            jsonRespawnObject.Add("time_to_gembox_drop", 359805);
            jsonRespawnObject.Add("time_in_gembox_period", 244679);
            jsonRespawnObject.Add("time_to_special_drop", 248205);
            jsonRespawnObject.Add("time_to_special_period", 97079);
            jsonData.Add("respawnVars", jsonRespawnObject);

            var cooldowns = new JArray();
            jsonData.Add("cooldowns", cooldowns);
            var newShopBuildings = new JArray() { 1, 0, 1, 1, 1, 1, 1, 0, 2, 0, 0, 0, 0, 0, 1, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            jsonData.Add("newShopBuildings", newShopBuildings);
            var newShopTraps = new JArray() { 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            jsonData.Add("newShopTraps", newShopTraps);
            var newShopDecos = new JArray() { 1, 4, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            jsonData.Add("newShopDecos", newShopDecos);

            jsonData.Add("last_league_rank", 0);
            jsonData.Add("last_alliance_level", 1);
            jsonData.Add("last_league_shuffle", 0);
            jsonData.Add("last_season_seen", -1);
            jsonData.Add("last_news_seen", 13);
            jsonData.Add("edit_mode_shown", false);
            jsonData.Add("war_tutorials_seen", 0);
            jsonData.Add("war_base", false);
            jsonData.Add("help_opened", false);
            jsonData.Add("bool_layout_edit_shown_erase", false);

            System.IO.File.WriteAllText(System.IO.Directory.GetCurrentDirectory() + "/base.json", jsonData.ToString());
            return jsonData;
        }

        public void Tick()
        {
            ComponentManager.Tick();
            foreach (var l in m_vGameObjects)
            {
                foreach (var go in l)
                    go.Tick();
            }
        }

        private int GenerateGameObjectGlobalId(GameObject go)
        {
            var index = m_vGameObjectsIndex[go.ClassId];
            m_vGameObjectsIndex[go.ClassId]++;
            return GlobalID.CreateGlobalID(go.ClassId + 500, index);
        }
    }
}