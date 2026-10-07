using System;
using System.Diagnostics;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace RamInfo.Logic
{
    // Erfasst Prozessarbeitssätze im Hintergrund und speichert sie sekundenweise
    public class RamRecorder
    {
        private readonly RamDataModel _model;

        public RamRecorder(RamDataModel model)
        {
            _model = model ?? throw new ArgumentNullException(nameof(model));
        }

        // Übernimmt eine vollständige Messung auf dem aufrufenden UI-Kontext
        public async Task TickAsync(int second)
        {
            if (second < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(second));
            }

            var snapshot = await Task.Run(CaptureSnapshot);

            foreach (var entry in snapshot)
            {
                int pid = entry.Key;
                if (!_model.RamData.TryGetValue(pid, out var values))
                {
                    values = new List<long>();
                    _model.RamData[pid] = values;
                    _model.ProcessNames[pid] = entry.Value.Name;
                    _model.ProcessStartTimes[pid] = second;
                }
                int index = second - _model.ProcessStartTimes[pid];
                // Nicht erfasste Sekunden erhalten einen Platzhalter, damit die Zeitachse stimmt.
                while (values.Count <= index)
                {
                    values.Add(0);
                }

                values[index] = entry.Value.WorkingSetBytes;
            }
            _model.SecondsCounter = Math.Max(_model.SecondsCounter, second + 1);
        }

        private static Dictionary<int, (string Name, long WorkingSetBytes)> CaptureSnapshot()
        {
            var snapshot = new Dictionary<int, (string Name, long WorkingSetBytes)>();
            var processes = Process.GetProcesses();
            try
            {
                foreach (var process in processes)
                {
                    try
                    {
                        snapshot[process.Id] = (process.ProcessName, process.WorkingSet64);
                    }
                    catch (Win32Exception)
                    {
                        // Geschützte Prozesse sind nicht immer lesbar.
                    }
                    catch (InvalidOperationException)
                    {
                        // Ein Prozess kann während der Abfrage bereits beendet sein.
                    }
                }
            }
            finally
            {
                foreach (var process in processes)
                {
                    process.Dispose();
                }
            }

            return snapshot;
        }
    }
}
