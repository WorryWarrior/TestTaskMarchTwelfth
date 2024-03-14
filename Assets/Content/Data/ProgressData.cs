using System;
using System.Collections.Generic;

namespace Content.Data
{
    [Serializable]
    public class ProgressData
    {
        public List<ProgressEntryData> GameSessions { get; set; }
    }
}