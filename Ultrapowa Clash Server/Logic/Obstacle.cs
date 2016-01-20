using Newtonsoft.Json.Linq;
using UCS.GameFiles;
using UCS.Helpers;

namespace UCS.Logic
{
    class Obstacle : GameObject
    {
        private Level m_vLevel;
        private Timer m_vTimer;

        public Obstacle(Data data, Level l) : base(data, l)
        {
            m_vLevel = l;
        }

        public override int ClassId
        {
            get { return 3; }
        }

        public ObstacleData GetObstacleData()
        {
            return (ObstacleData) GetData();
        }

        public void StartClearing()
        {
            var constructionTime = GetObstacleData().ClearTimeSeconds;
            if (constructionTime < 1)
            {
                ClearingFinished();
            }
            else
            {
                m_vTimer = new Timer();
                m_vTimer.StartTimer(constructionTime, m_vLevel.GetTime());
                m_vLevel.WorkerManager.AllocateWorker(this);
            }
        }

        public void CancelClearing()
        {
            m_vLevel.WorkerManager.DeallocateWorker(this);
            m_vTimer = null;
            var od = GetObstacleData();
            var rd = od.GetClearingResource();
            var cost = od.ClearCost;
            GetLevel().GetPlayerAvatar().CommodityCountChangeHelper(0, rd, cost);
        }

        public int GetRemainingClearingTime()
        {
            return m_vTimer.GetRemainingSeconds(m_vLevel.GetTime());
        }

        public override void Tick()
        {
            if (IsClearingOnGoing())
            {
                if (m_vTimer.GetRemainingSeconds(m_vLevel.GetTime()) <= 0)
                    ClearingFinished();
            }
        }

        public void ClearingFinished()
        {
            //gérer achievement
            //gérer xp reward
            //gérer obstacleclearcounter
            //gérer diamond reward
            m_vLevel.WorkerManager.DeallocateWorker(this);
            m_vTimer = null;
        }

        public void SpeedUpClearing()
        {
            var remainingSeconds = 0;
            if (IsClearingOnGoing())
            {
                remainingSeconds = m_vTimer.GetRemainingSeconds(m_vLevel.GetTime());
            }
            var cost = GamePlayUtil.GetSpeedUpCost(remainingSeconds);
            var ca = GetLevel().GetPlayerAvatar();
            if (ca.HasEnoughDiamonds(cost))
            {
                ca.UseDiamonds(cost);
                ClearingFinished();
            }
        }

        public bool IsClearingOnGoing()
        {
            return m_vTimer != null;
        }

        public JObject ToJson()
        {
            var jsonObject = new JObject();
            jsonObject.Add("data", GetObstacleData().GetGlobalID());
            //const_t à vérifier pour un obstacle
            if (IsClearingOnGoing())
                jsonObject.Add("const_t", m_vTimer.GetRemainingSeconds(m_vLevel.GetTime()));
            jsonObject.Add("x", X);
            jsonObject.Add("y", Y);
            return jsonObject;
        }
    }
}