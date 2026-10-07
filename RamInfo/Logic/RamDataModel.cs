using System;
using System.Collections.Generic;

namespace RamInfo.Logic
{
    // Hält Rohmesswerte und den Zustand der aktuellen Aufzeichnung
    public class RamDataModel
    {
        // Arbeitssatz in Bytes je PID, relativ zur ersten Erfassung gespeichert
        public Dictionary<int, List<long>> RamData { get; } = new Dictionary<int, List<long>>();

        // Prozessnamen bleiben auch nach dem Beenden eines Prozesses verfügbar
        public Dictionary<int, string> ProcessNames { get; } = new Dictionary<int, string>();

        // Sekunde der ersten Erfassung je PID, relativ zum Aufzeichnungsbeginn
        public Dictionary<int, int> ProcessStartTimes { get; } = new Dictionary<int, int>();
        public HashSet<int> ActiveChartPIDs { get; } = new HashSet<int>();
        public bool IsRecording { get; set; }
        public bool ShowOnlyTag1 { get; set; }

        // Ende des erfassten Zeitbereichs: letzte Messsekunde plus eins
        public int SecondsCounter { get; set; }
        public DateTime StartTime { get; set; } = DateTime.Now;
        public Random Rand { get; } = new Random();

        // Löscht die Aufzeichnung und behält die Filterauswahl bei
        public void Reset()
        {
            RamData.Clear();
            ProcessNames.Clear();
            ProcessStartTimes.Clear();
            ActiveChartPIDs.Clear();
            SecondsCounter = 0;
            StartTime = DateTime.Now;
            IsRecording = false;
        }
    }
}

