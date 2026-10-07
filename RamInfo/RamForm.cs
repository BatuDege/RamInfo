using System;
using System.Linq;
using RamInfo.Logic;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace RamInfo
{
    //´Verbindet Aufzeichnung, Auswertung, Diagramm und Tabellenfilter
    public partial class RamForm : Form
    {
        private const double BytesPerMebibyte = 1024.0 * 1024.0;
        private const int RecordingIntervalMilliseconds = 1000;
        private const int TotalSeriesPid = -1;
        private const string CombinedProcessSuffix = " (kombiniert)";

        private readonly RamDataModel _model;
        private readonly RamRecorder _recorder;
        private readonly RamStatistics _stats;
        private readonly RamChartManager _chartManager;
        private readonly Timer _timer;
        private readonly Stopwatch _stopwatch = new Stopwatch();
        private Task _pendingTick = Task.CompletedTask;
        private bool _stopping;
        private bool _isRamFilterActive;
        private double _warnThreshold;
        private readonly NotifyIcon _trayIcon;
        private readonly ContextMenuStrip _trayMenu;

        public RamForm()
        {
            InitializeComponent();
            if (components == null)
            {
                components = new System.ComponentModel.Container();
            }

            _trayMenu = new ContextMenuStrip(components);
            _trayMenu.Items.Add("Wiederherstellen", null, (s, e) => RestoreFromTray());
            _trayMenu.Items.Add("Beenden", null, (s, e) => Application.Exit());
            _trayIcon = new NotifyIcon(components)
            {
                Text = "RamInfo - läuft im Hintergrund",
                Icon = SystemIcons.Application,
                ContextMenuStrip = _trayMenu,
                Visible = false
            };
            _trayIcon.DoubleClick += (s, e) => RestoreFromTray();
            _model = new RamDataModel();
            _recorder = new RamRecorder(_model);
            _stats = new RamStatistics(_model);
            _chartManager = new RamChartManager(_model, Ram_Chart);
            _timer = new Timer(components) { Interval = RecordingIntervalMilliseconds };
            _timer.Tick += Ram_Timer_Tick;
            Ram_SearchText.Clear();
            Ram_BtnFilterChart.Text = "Nur ausgewählte Prozesse anzeigen";
        }

        private void RamForm_Load(object sender, EventArgs e)
        {
            Ram_SetupChart();
        }

        private async void Ram_Timer_Tick(object sender, EventArgs e)
        {
            if (!_model.IsRecording || _stopping || !_pendingTick.IsCompleted)
            {
                return;
            }
            _pendingTick = RecordTickAsync();
            await _pendingTick;
        }

        // Erfasst eine Messung und aktualisiert Zeitachse und Laufzeit
        private async Task RecordTickAsync()
        {
            try
            {
                await _recorder.TickAsync((int)_stopwatch.Elapsed.TotalSeconds);
                if (IsDisposed || Disposing)
                {
                    return;
                }
                Ram_LabelTimer.Text = _stopwatch.Elapsed.ToString(@"hh\:mm\:ss");
                var area = Ram_Chart.ChartAreas[0];
                area.AxisX.Maximum = Math.Max(1, _model.SecondsCounter);
                area.AxisX.Interval = _model.SecondsCounter <= 60 ? 10 : 60;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                if (IsDisposed || Disposing)
                {
                    return;
                }
                _timer.Stop();
                _stopwatch.Stop();
                _model.IsRecording = false;
                Ram_BtnAufzeichnen.Text = "Aufzeichnen";
                Ram_BtnDuplikateAddieren.Enabled = true;
                Ram_LabelStatus.Text = "Messung fehlgeschlagen: " + ex.Message;
            }
        }

        private void Ram_SetupChart()
        {
            var area = Ram_Chart.ChartAreas[0];
            area.AxisX.Minimum = 0;
            area.AxisX.Maximum = 1;
            area.AxisX.Interval = 1;
            area.AxisX.MajorGrid.LineColor = Color.LightGray;
            area.AxisY.MajorGrid.LineColor = Color.LightGray;
            area.AxisY.Title = "Ram (MB)";
        }

        private async void Ram_BtnAufzeichnen_Click(object sender, EventArgs e)
        {
            if (_stopping)
            {
                return;
            }
            if (_model.IsRecording)
            {
                await StopRecordingAsync();
            }
            else if (_pendingTick.IsCompleted)
            {
                // Eine noch laufende Messung darf nicht in die neue Aufzeichnung gelangen.
                StartRecording();
            }
        }

        // Setzt die bisherige Auswertung zurück und beginnt eine neue Aufzeichnung
        private void StartRecording()
        {
            _model.Reset();
            _chartManager.EntferneAlleSerien();
            Ram_DGV.Rows.Clear();
            Ram_SetupChart();
            Ram_LabelTimer.Text = "00:00:00";
            _model.IsRecording = true;
            Ram_BtnDuplikateAddieren.Enabled = false;
            _stopwatch.Restart();
            _timer.Start();
            Ram_LabelStatus.Text = "Aufzeichnung läuft...";
            Ram_BtnAufzeichnen.Text = "Aufzeichnung Stoppen";
        }

        // Wartet auf die letzte Messung, bevor die Statistik angezeigt wird
        private async Task StopRecordingAsync()
        {
            _stopping = true;
            Ram_BtnAufzeichnen.Enabled = false;
            _model.IsRecording = false;
            _timer.Stop();
            _stopwatch.Stop();
            try
            {
                await _pendingTick;
                if (IsDisposed || Disposing)
                {
                    return;
                }

                Ram_ShowStatistics();
                Ram_LabelStatus.Text = "Aufzeichnung gestoppt.";
                Ram_BtnAufzeichnen.Text = "Aufzeichnung Starten";
                Ram_BtnDuplikateAddieren.Enabled = true;
            }
            finally
            {
                _stopping = false;
                if (!IsDisposed && !Disposing)
                {
                    Ram_BtnAufzeichnen.Enabled = true;
                }
            }
        }

        // Fügt Kennzahlen in MiB und die erste Erfassung als Tabellenzeile ein
        private void AddStatisticsRow(string name, int pid, int start, List<long> samples)
        {
            var (avg, high, low) = _stats.BerechneStatistik(samples);
            int row = Ram_DGV.Rows.Add(name, pid, TimeSpan.FromSeconds(start).ToString(@"hh\:mm\:ss"),
                Math.Round(low / BytesPerMebibyte, 2), Math.Round(avg / BytesPerMebibyte, 2),
                Math.Round(high / BytesPerMebibyte, 2));
            Ram_DGV.Rows[row].Cells["ToggleChart"].Value = "Zum Chart";
            Ram_DGV.Rows[row].Cells["ToggleChart"].Tag = 0;
        }

        // Baut die Tabelle aus den ursprünglichen Prozessmessungen auf
        private void Ram_ShowStatistics()
        {
            Ram_DGV.Rows.Clear();
            foreach (var entry in _model.RamData)
            {
                if (entry.Key < 0 || entry.Value.Count == 0)
                {
                    continue;
                }
                string name = _model.ProcessNames.TryGetValue(entry.Key, out var cached) ? cached : "Unbekannt";
                int start = _model.ProcessStartTimes.TryGetValue(entry.Key, out var value) ? value : 0;
                AddStatisticsRow(name, entry.Key, start, entry.Value);
            }
            ApplyFilters();
        }

        private void Ram_DGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (_model.IsRecording || _stopping || e.RowIndex < 0 || e.ColumnIndex < 0 ||
                e.RowIndex >= Ram_DGV.Rows.Count || e.ColumnIndex >= Ram_DGV.Columns.Count ||
                Ram_DGV.Columns[e.ColumnIndex].Name != "ToggleChart")
            {
                return;
            }

            var row = Ram_DGV.Rows[e.RowIndex];
            string name = Convert.ToString(row.Cells["ProzessName"].Value);
            if (!int.TryParse(Convert.ToString(row.Cells["PID"].Value), out int pid))
            {
                return;
            }
            string seriesName = pid < 0 ? name + "_kombiniert" : $"{name}_{pid}";
            if (_chartManager.IstSerieAktiv(seriesName))
            {
                _chartManager.EntferneSerie(seriesName);
                Ram_UpdateButtonText(e.RowIndex, false);
            }
            else
            {
                List<long> values;
                int start = 0;
                if (pid == TotalSeriesPid)
                {
                    values = _stats.BerechneGesamtverlauf();
                }
                else if (pid < 0)
                {
                    string baseName = name.Substring(0, name.Length - CombinedProcessSuffix.Length);
                    if (!_stats.KombiniereRamVerläufeNachName().TryGetValue(baseName, out values))
                    {
                        return;
                    }
                }
                else
                {
                    if (!_model.RamData.TryGetValue(pid, out values))
                    {
                        return;
                    }
                    _model.ProcessStartTimes.TryGetValue(pid, out start);
                }
                row.Tag = _chartManager.ZeigeSerie(pid, seriesName, values, start);
                Ram_UpdateButtonText(e.RowIndex, true);
            }
            ApplyFilters();
        }

        private void Ram_UpdateButtonText(int rowIndex, bool active)
        {
            var cell = Ram_DGV.Rows[rowIndex].Cells["ToggleChart"];
            cell.Value = active ? "Im Chart" : "Zum Chart";
            cell.Tag = active ? 1 : 0;
        }

        private void Ram_BtnDuplikateAddieren_Click(object sender, EventArgs e)
        {
            if (_model.IsRecording || _stopping || !_pendingTick.IsCompleted)
            {
                return;
            }
            if (_model.RamData.Count == 0)
            {
                Ram_LabelStatus.Text = "Keine Messdaten vorhanden.";
                return;
            }
            // Summen nur als Ergebniszeilen anlegen, damit die Rohdaten unverändert bleiben.
            _chartManager.EntferneAlleSerien();
            Ram_ShowStatistics();
            var starts = _stats.BerechneFrühesteStartzeiten();
            int syntheticPid = -2;
            foreach (var pair in _stats.KombiniereRamVerläufeNachName())
            {
                int start = starts.TryGetValue(pair.Key, out var value) ? value : 0;
                AddStatisticsRow(pair.Key + CombinedProcessSuffix, syntheticPid--, start, pair.Value.Skip(start).ToList());
            }
            AddStatisticsRow("RAM Gesamt" + CombinedProcessSuffix, TotalSeriesPid, 0, _stats.BerechneGesamtverlauf());
            ApplyFilters();
            Ram_LabelStatus.Text = "RAM-Verläufe wurden kombiniert und angezeigt.";
        }

        // Wendet Suche, Diagrammauswahl und RAM-Schwelle gemeinsam auf jede Zeile an
        private void ApplyFilters()
        {
            if (_model == null)
            {
                return;
            }
            string query = Ram_SearchText.Text;
            Ram_DGV.CurrentCell = null;
            foreach (DataGridViewRow row in Ram_DGV.Rows)
            {
                if (row.IsNewRow)
                {
                    continue;
                }
                bool active = Equals(row.Cells["ToggleChart"].Tag, 1);
                string name = Convert.ToString(row.Cells["ProzessName"].Value);
                bool above = double.TryParse(Convert.ToString(row.Cells["AvgRam"].Value), out var avg) && avg >= _warnThreshold;
                row.Visible = name.IndexOf(query, StringComparison.OrdinalIgnoreCase) >= 0 &&
                    (!_model.ShowOnlyTag1 || active) && (!_isRamFilterActive || above);
                row.DefaultCellStyle.BackColor = active && row.Tag is Color color ? color :
                    _isRamFilterActive && above ? Color.OrangeRed : Color.White;
                row.DefaultCellStyle.ForeColor = active || (_isRamFilterActive && above) ? Color.White : Color.Black;
            }
        }

        private void Ram_BtnFilterChart_Click(object sender, EventArgs e)
        {
            _model.ShowOnlyTag1 = !_model.ShowOnlyTag1;
            Ram_BtnFilterChart.Text = _model.ShowOnlyTag1 ? "Alle Prozesse anzeigen" : "Nur ausgewählte Prozesse anzeigen";
            ApplyFilters();
        }

        private void Ram_SearchText_TextChanged(object sender, EventArgs e)
        {
            ApplyFilters();
        }

        private void Ram_SearchText_Click(object sender, EventArgs e)
        {
            Ram_SearchText.Clear();
        }

        private void BtnWarnFilter_Click(object sender, EventArgs e)
        {
            _isRamFilterActive = !_isRamFilterActive;
            _warnThreshold = (double)numericUpDownWarnMB.Value;
            btnWarnFilter.Text = _isRamFilterActive ? "Ausschalten" : "MB Filter";
            ApplyFilters();
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            if (_trayIcon != null && WindowState == FormWindowState.Minimized)
            {
                Hide();
                _trayIcon.Visible = true;
            }
        }

        // Holt das minimierte Fenster aus dem Infobereich zurück
        private void RestoreFromTray()
        {
            Show();
            WindowState = FormWindowState.Normal;
            _trayIcon.Visible = false;
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            _timer.Stop();
            _stopwatch.Stop();
            _model.IsRecording = false;
            _trayIcon.Visible = false;
            base.OnFormClosed(e);
        }
    }
}

