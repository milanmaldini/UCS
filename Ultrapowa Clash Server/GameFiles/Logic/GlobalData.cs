namespace UCS.GameFiles
{
    class GlobalData : Data
    {
        public GlobalData(CSVRow row, DataTable dt)
            : base(row, dt)
        {
            LoadData(this, GetType(), row);
        }

        public int NumberValue { get; set; }
        public bool BooleanValue { get; set; }
        public string TextValue { get; set; }
        public int NumberArray { get; set; }
        public string StringArray { get; set; }
        public string AltStringArray { get; set; }
    }
}