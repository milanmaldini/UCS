using System;
using System.Collections.Generic;

namespace UCS.GameFiles
{
    class LeagueData : Data
    {

        public LeagueData(CSVRow row, DataTable dt)
            : base(row, dt)
        {
            LoadData(this, this.GetType(), row);
        }

        public String TID { get; set; }
        public String TIDShort { get; set; }
        public String IconSWF { get; set; }
        public String IconExportName { get; set; }
        public String LeagueBannerIcon { get; set; }
        public String LeagueBannerIconNum { get; set; }
        public int GoldReward { get; set; }
        public int ElixirReward { get; set; }
        public int DarkElixirReward { get; set; }
        public int PlacementLimitLow { get; set; }
        public int PlacementLimitHigh { get; set; }
        public int DemoteLimit { get; set; }
        public int PromoteLimit { get; set; }
        public List<int> BucketPlacementRangeLow { get; set; }
        public List<int> BucketPlacementRangeHigh { get; set; }
        public List<int> BucketPlacementSoftLimit { get; set; }
        public List<int> BucketPlacementHardLimit { get; set; }
        public bool IgnoredByServer { get; set; }
        public bool DemoteEnabled { get; set; }
        public bool PromoteEnabled { get; set; }
    }
}
