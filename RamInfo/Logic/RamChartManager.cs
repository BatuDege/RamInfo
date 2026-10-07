using System;
using System.Drawing;
using System.Collections.Generic;
using System.Windows.Forms.DataVisualization.Charting;

namespace RamInfo.Logic
{
    // Verwaltet die Anzeige der Prozessverläufe im Diagramm
    public class RamChartManager
    {
        private const double BytesPerMebibyte = 1024.0 * 1024.0;
        private readonly Chart _chart;
        private readonly Random _rand;

        public RamChartManager(RamDataModel model, Chart chart)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            _chart = chart ?? throw new ArgumentNullException(nameof(chart));
            _rand = new Random();
        }

        // Prüft, ob der benannte Verlauf bereits angezeigt wird
        public bool IstSerieAktiv(string seriesName)
        {
            return _chart.Series.IndexOf(seriesName) >= 0;
        }

        // Entfernt einen vorhandenen Verlauf aus dem Diagramm
        public void EntferneSerie(string seriesName)
        {
            if (_chart.Series.IndexOf(seriesName) >= 0)
            {
                _chart.Series.Remove(_chart.Series[seriesName]);
            }
        }

        // Zeigt Byte-Messwerte in MiB an und liefert die verwendete Linienfarbe
        public Color ZeigeSerie(int pid, string seriesName, List<long> werte, int start)
        {
            var color = pid == -1 ? Color.DarkRed : GetRandomColor();

            var serie = new Series(seriesName)
            {
                ChartType = SeriesChartType.Line,
                XValueType = ChartValueType.Int32,
                Legend = "Prozesse",
                Color = color
            };

            for (int i = 0; i < start; i++)
            {
                serie.Points.AddXY(i, double.NaN);
            }

            for (int i = 0; i < werte.Count; i++)
            {
                double ramMB = werte[i] / BytesPerMebibyte;
                serie.Points.AddXY(start + i, ramMB);
            }

            _chart.Series.Add(serie);

            return color;
        }

        // Leert das Diagramm vor einer neuen Auswertung
        public void EntferneAlleSerien()
        {
            _chart.Series.Clear();
        }

        // Erzeugt eine dunkle Farbe für Linien und ausgewählte Tabellenzeilen
        public Color GetRandomColor()
        {
            return Color.FromArgb(_rand.Next(20, 150), _rand.Next(20, 150), _rand.Next(20, 150));
        }
    }
}
