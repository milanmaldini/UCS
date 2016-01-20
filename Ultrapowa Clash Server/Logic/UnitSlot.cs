using System.IO;
using UCS.GameFiles;
using UCS.Helpers;

namespace UCS.Logic
{
    class UnitSlot
    {
        public int Count; //a1 + 12
        public int Level; //a1 + 8
        public CombatItemData UnitData; //a1 + 4

        public UnitSlot(CombatItemData cd, int level, int count)
        {
            UnitData = cd;
            Level = level;
            Count = count;
        }

        public void Decode(BinaryReader br)
        {
            UnitData = (CombatItemData) br.ReadDataReference();
            Level = br.ReadInt32WithEndian();
            Count = br.ReadInt32WithEndian();
        }
    }
}