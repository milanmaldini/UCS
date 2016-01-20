namespace UCS.GameFiles
{
    class AchievementData : Data
    {
        public AchievementData(CSVRow row, DataTable dt) : base(row, dt)
        {
            LoadData(this, GetType(), row);
        }

        public int Level { get; set; }
        public string TID { get; set; }
        public string InfoTID { get; set; }
        public string Action { get; set; }
        public int ActionCount { get; set; }
        public string ActionData { get; set; }
        public int ExpReward { get; set; }
        public int DiamondReward { get; set; }
        public string IconSWF { get; set; }
        public string IconExportName { get; set; }
        public string CompletedTID { get; set; }
        public bool ShowValue { get; set; }
        public string AndroidID { get; set; }
    }
}