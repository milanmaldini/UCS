namespace UCS.GameFiles
{
    class WarData : Data
    {
        public WarData(CSVRow row, DataTable dt)
            : base(row, dt)
        {
            LoadData(this, GetType(), row);
        }

        public int TeamSize { get; set; }
        public int PreparationMinutes { get; set; }
        public int WarMinutes { get; set; }
        public int BonusPercentWin { get; set; }
        public int BonusPercentLose { get; set; }
        public int BonusPercentDraw { get; set; }
        public bool DisableProduction { get; set; }
    }
}