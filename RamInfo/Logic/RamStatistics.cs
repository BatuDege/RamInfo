using System;
using System.Linq;
using System.Collections.Generic;

namespace RamInfo.Logic
{
    // Berechnet Kennzahlen und zeitlich ausgerichtete Summen aus den Rohdaten 
    public class RamStatistics
    {
        private readonly RamDataModel _model;

        public RamStatistics(RamDataModel model)
        {
            _model = model ?? throw new ArgumentNullException(nameof(model));
        }

        // Summiert gleichnamige Prozesse ab dem Beginn der Aufzeichnung
        public Dictionary<string, List<long>> KombiniereRamVerläufeNachName()
        {
            var combined = new Dictionary<string, List<long>>();

            foreach (var entry in _model.RamData)
            {
                int pid = entry.Key;
                if (pid < 0)
                {
                    continue;
                }

                string name = GetProcessName(pid);
                int start = GetStartSecond(pid);
                if (!combined.TryGetValue(name, out var values))
                {
                    values = new List<long>();
                    combined[name] = values;
                }

                AddToTimeline(values, entry.Value, start);
            }

            return combined;
        }

        // Liefert je Prozessname die erste Erfassung in Aufzeichnungssekunden
        public Dictionary<string, int> BerechneFrühesteStartzeiten()
        {
            var frühesteStarts = new Dictionary<string, int>();

            foreach (var kvp in _model.ProcessStartTimes)
            {
                int pid = kvp.Key;
                if (pid < 0)
                {
                    continue;
                }

                int start = kvp.Value;
                string name = GetProcessName(pid);

                if (!frühesteStarts.TryGetValue(name, out int earliest) || start < earliest)
                {
                    frühesteStarts[name] = start;
                }
            }

            return frühesteStarts;
        }

        // Summiert die Prozessarbeitssätze; abgeleitete Zeilen werden ignoriert
        public List<long> BerechneGesamtverlauf()
        {
            var total = new List<long>();
            foreach (var entry in _model.RamData)
            {
                if (entry.Key < 0)
                {
                    continue;
                }

                AddToTimeline(total, entry.Value, GetStartSecond(entry.Key));
            }
            return total;
        }

        // Berechnet Mittelwert sowie Mittelwerte der höchsten und niedrigsten 1%
        public (double avg, double high, double low) BerechneStatistik(List<long> werte)
        {
            if (werte == null || werte.Count == 0)
            {
                return (0, 0, 0);
            }

            // Einmal sortieren; die Reihenfolge der gespeicherten Messwerte bleibt erhalten.
            var sorted = werte.OrderBy(value => value).ToArray();
            int percentileCount = Math.Max(1, sorted.Length / 100);
            double avg = werte.Average();
            double high = sorted.Skip(sorted.Length - percentileCount).Average();
            double low = sorted.Take(percentileCount).Average();

            return (avg, high, low);
        }

        private string GetProcessName(int pid)
        {
            return _model.ProcessNames.TryGetValue(pid, out var name) ? name : "Unbekannt";
        }

        private int GetStartSecond(int pid)
        {
            return _model.ProcessStartTimes.TryGetValue(pid, out int start) ? start : 0;
        }

        // Addiert Messwerte an ihrer tatsächlichen Zeitposition in den Zielverlauf
        private static void AddToTimeline(List<long> target, List<long> samples, int start)
        {
            while (target.Count < start + samples.Count)
            {
                target.Add(0);
            }

            for (int i = 0; i < samples.Count; i++)
            {
                target[start + i] += samples[i];
            }
        }
    }
}

