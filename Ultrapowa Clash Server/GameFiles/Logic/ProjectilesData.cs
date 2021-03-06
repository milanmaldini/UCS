﻿namespace UCS.GameFiles
{
    internal class ProjectilesData : Data
    {
        public ProjectilesData(CSVRow row, DataTable dt) : base(row, dt)
        {
            LoadData(this, GetType(), row);
        }
        public string Name { get; set; }
        public string SWF { get; set; }
        public string ExportName { get; set; }
        public string ParticleEmitter { get; set; }
        public string Effect { get; set; }
        public int Speed { get; set; }
        public int StartHeight { get; set; }
        public int StartOffset { get; set; }
        public bool IsBallistic { get; set; }
        public string ShadowSWF { get; set; }
        public string ShadowExportName { get; set; }
        public bool RandomHitPosition { get; set; }
        public bool UseRotate { get; set; }
        public bool PlayOnce { get; set; }
        public bool UseTopLayer { get; set; }
        public int Scale { get; set; }
    }
}