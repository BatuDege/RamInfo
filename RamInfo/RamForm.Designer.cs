namespace RamInfo
{
    partial class RamForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RamForm));
            this.Ram_SearchText = new System.Windows.Forms.TextBox();
            this.Ram_BtnFilterChart = new System.Windows.Forms.Button();
            this.Ram_BtnDuplikateAddieren = new System.Windows.Forms.Button();
            this.Ram_BtnAufzeichnen = new System.Windows.Forms.Button();
            this.Ram_DGV = new System.Windows.Forms.DataGridView();
            this.ProzessName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Time = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LowRam = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AvgRam = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.HighRam = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ToggleChart = new System.Windows.Forms.DataGridViewButtonColumn();
            this.Ram_LabelTimer = new System.Windows.Forms.Label();
            this.Ram_LabelStatus = new System.Windows.Forms.RichTextBox();
            this.Ram_Chart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.btnWarnFilter = new System.Windows.Forms.Button();
            this.numericUpDownWarnMB = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.Ram_DGV)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Ram_Chart)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownWarnMB)).BeginInit();
            this.SuspendLayout();
            // 
            // Ram_SearchText
            // 
            this.Ram_SearchText.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Ram_SearchText.Location = new System.Drawing.Point(1023, 509);
            this.Ram_SearchText.Name = "Ram_SearchText";
            this.Ram_SearchText.Size = new System.Drawing.Size(217, 20);
            this.Ram_SearchText.TabIndex = 31;
            this.Ram_SearchText.Text = "Prozess Suchen";
            this.Ram_SearchText.Click += new System.EventHandler(this.Ram_SearchText_Click);
            this.Ram_SearchText.TextChanged += new System.EventHandler(this.Ram_SearchText_TextChanged);
            // 
            // Ram_BtnFilterChart
            // 
            this.Ram_BtnFilterChart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Ram_BtnFilterChart.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Ram_BtnFilterChart.Location = new System.Drawing.Point(1008, 449);
            this.Ram_BtnFilterChart.Margin = new System.Windows.Forms.Padding(4);
            this.Ram_BtnFilterChart.Name = "Ram_BtnFilterChart";
            this.Ram_BtnFilterChart.Size = new System.Drawing.Size(116, 53);
            this.Ram_BtnFilterChart.TabIndex = 30;
            this.Ram_BtnFilterChart.Text = "Nur Häkchen Anzeigen";
            this.Ram_BtnFilterChart.UseVisualStyleBackColor = true;
            this.Ram_BtnFilterChart.Click += new System.EventHandler(this.Ram_BtnFilterChart_Click);
            // 
            // Ram_BtnDuplikateAddieren
            // 
            this.Ram_BtnDuplikateAddieren.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Ram_BtnDuplikateAddieren.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Ram_BtnDuplikateAddieren.Location = new System.Drawing.Point(1132, 449);
            this.Ram_BtnDuplikateAddieren.Margin = new System.Windows.Forms.Padding(4);
            this.Ram_BtnDuplikateAddieren.Name = "Ram_BtnDuplikateAddieren";
            this.Ram_BtnDuplikateAddieren.Size = new System.Drawing.Size(116, 53);
            this.Ram_BtnDuplikateAddieren.TabIndex = 28;
            this.Ram_BtnDuplikateAddieren.Text = "Dublikate Addieren";
            this.Ram_BtnDuplikateAddieren.UseVisualStyleBackColor = true;
            this.Ram_BtnDuplikateAddieren.Click += new System.EventHandler(this.Ram_BtnDuplikateAddieren_Click);
            // 
            // Ram_BtnAufzeichnen
            // 
            this.Ram_BtnAufzeichnen.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Ram_BtnAufzeichnen.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Ram_BtnAufzeichnen.Location = new System.Drawing.Point(1023, 606);
            this.Ram_BtnAufzeichnen.Margin = new System.Windows.Forms.Padding(4);
            this.Ram_BtnAufzeichnen.Name = "Ram_BtnAufzeichnen";
            this.Ram_BtnAufzeichnen.Size = new System.Drawing.Size(217, 62);
            this.Ram_BtnAufzeichnen.TabIndex = 26;
            this.Ram_BtnAufzeichnen.Text = "Start/Stop";
            this.Ram_BtnAufzeichnen.UseVisualStyleBackColor = true;
            this.Ram_BtnAufzeichnen.Click += new System.EventHandler(this.Ram_BtnAufzeichnen_Click);
            // 
            // Ram_DGV
            // 
            this.Ram_DGV.AllowUserToAddRows = false;
            this.Ram_DGV.AllowUserToOrderColumns = true;
            this.Ram_DGV.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.Ram_DGV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.Ram_DGV.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ProzessName,
            this.PID,
            this.Time,
            this.LowRam,
            this.AvgRam,
            this.HighRam,
            this.ToggleChart});
            this.Ram_DGV.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Ram_DGV.Location = new System.Drawing.Point(17, 449);
            this.Ram_DGV.Margin = new System.Windows.Forms.Padding(4);
            this.Ram_DGV.Name = "Ram_DGV";
            this.Ram_DGV.ReadOnly = true;
            this.Ram_DGV.Size = new System.Drawing.Size(983, 219);
            this.Ram_DGV.TabIndex = 25;
            this.Ram_DGV.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.Ram_DGV_CellContentClick);
            // 
            // ProzessName
            // 
            this.ProzessName.FillWeight = 250F;
            this.ProzessName.HeaderText = "Prozess";
            this.ProzessName.Name = "ProzessName";
            this.ProzessName.ReadOnly = true;
            this.ProzessName.ToolTipText = "Name des Prozesses";
            this.ProzessName.Width = 250;
            // 
            // PID
            // 
            this.PID.FillWeight = 200F;
            this.PID.HeaderText = "PID";
            this.PID.Name = "PID";
            this.PID.ReadOnly = true;
            this.PID.ToolTipText = "ID des Prozesses";
            this.PID.Width = 75;
            // 
            // Time
            // 
            this.Time.HeaderText = "Seit-start";
            this.Time.Name = "Time";
            this.Time.ReadOnly = true;
            this.Time.ToolTipText = "Startzeit nach der Aufnahem";
            this.Time.Width = 75;
            // 
            // LowRam
            // 
            this.LowRam.FillWeight = 200F;
            this.LowRam.HeaderText = "1% Low (MB)";
            this.LowRam.Name = "LowRam";
            this.LowRam.ReadOnly = true;
            this.LowRam.ToolTipText = "Niedrigster verbrauch";
            this.LowRam.Width = 150;
            // 
            // AvgRam
            // 
            this.AvgRam.FillWeight = 200F;
            this.AvgRam.HeaderText = "Durchschnitt (MB)";
            this.AvgRam.Name = "AvgRam";
            this.AvgRam.ReadOnly = true;
            this.AvgRam.ToolTipText = "Druchschnittlicher verbrauch";
            this.AvgRam.Width = 150;
            // 
            // HighRam
            // 
            this.HighRam.FillWeight = 200F;
            this.HighRam.HeaderText = "1% High (MB)";
            this.HighRam.Name = "HighRam";
            this.HighRam.ReadOnly = true;
            this.HighRam.ToolTipText = "Höhster verbrauch";
            this.HighRam.Width = 150;
            // 
            // ToggleChart
            // 
            this.ToggleChart.HeaderText = "Anzeigen";
            this.ToggleChart.Name = "ToggleChart";
            this.ToggleChart.ReadOnly = true;
            this.ToggleChart.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.ToggleChart.ToolTipText = "Im Diagram anzeigen";
            this.ToggleChart.Width = 73;
            // 
            // Ram_LabelTimer
            // 
            this.Ram_LabelTimer.AutoSize = true;
            this.Ram_LabelTimer.Location = new System.Drawing.Point(14, 13);
            this.Ram_LabelTimer.Name = "Ram_LabelTimer";
            this.Ram_LabelTimer.Size = new System.Drawing.Size(33, 13);
            this.Ram_LabelTimer.TabIndex = 27;
            this.Ram_LabelTimer.Text = "Timer";
            // 
            // Ram_LabelStatus
            // 
            this.Ram_LabelStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Ram_LabelStatus.Location = new System.Drawing.Point(1023, 561);
            this.Ram_LabelStatus.Name = "Ram_LabelStatus";
            this.Ram_LabelStatus.ReadOnly = true;
            this.Ram_LabelStatus.Size = new System.Drawing.Size(217, 38);
            this.Ram_LabelStatus.TabIndex = 29;
            this.Ram_LabelStatus.Text = "Status...";
            // 
            // Ram_Chart
            // 
            chartArea1.Name = "ChartArea1";
            this.Ram_Chart.ChartAreas.Add(chartArea1);
            legend1.Name = "Prozesse";
            this.Ram_Chart.Legends.Add(legend1);
            this.Ram_Chart.Location = new System.Drawing.Point(17, 13);
            this.Ram_Chart.Margin = new System.Windows.Forms.Padding(4);
            this.Ram_Chart.Name = "Ram_Chart";
            this.Ram_Chart.Size = new System.Drawing.Size(1234, 428);
            this.Ram_Chart.TabIndex = 33;
            this.Ram_Chart.Text = "Chart";
            // 
            // btnWarnFilter
            // 
            this.btnWarnFilter.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnWarnFilter.Location = new System.Drawing.Point(1149, 535);
            this.btnWarnFilter.Name = "btnWarnFilter";
            this.btnWarnFilter.Size = new System.Drawing.Size(99, 20);
            this.btnWarnFilter.TabIndex = 34;
            this.btnWarnFilter.Text = "Ram Verbrauch";
            this.btnWarnFilter.UseVisualStyleBackColor = true;
            this.btnWarnFilter.Click += new System.EventHandler(this.BtnWarnFilter_Click);
            // 
            // numericUpDownWarnMB
            // 
            this.numericUpDownWarnMB.Cursor = System.Windows.Forms.Cursors.Hand;
            this.numericUpDownWarnMB.Increment = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.numericUpDownWarnMB.Location = new System.Drawing.Point(1023, 535);
            this.numericUpDownWarnMB.Maximum = new decimal(new int[] {
            64000,
            0,
            0,
            0});
            this.numericUpDownWarnMB.Name = "numericUpDownWarnMB";
            this.numericUpDownWarnMB.Size = new System.Drawing.Size(120, 20);
            this.numericUpDownWarnMB.TabIndex = 35;
            this.numericUpDownWarnMB.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.numericUpDownWarnMB.Value = new decimal(new int[] {
            500,
            0,
            0,
            0});
            // 
            // RamForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1264, 681);
            this.Controls.Add(this.numericUpDownWarnMB);
            this.Controls.Add(this.btnWarnFilter);
            this.Controls.Add(this.Ram_SearchText);
            this.Controls.Add(this.Ram_BtnFilterChart);
            this.Controls.Add(this.Ram_BtnDuplikateAddieren);
            this.Controls.Add(this.Ram_BtnAufzeichnen);
            this.Controls.Add(this.Ram_DGV);
            this.Controls.Add(this.Ram_LabelTimer);
            this.Controls.Add(this.Ram_LabelStatus);
            this.Controls.Add(this.Ram_Chart);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "RamForm";
            this.Text = "RamInfo";
            this.Load += new System.EventHandler(this.RamForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.Ram_DGV)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Ram_Chart)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownWarnMB)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox Ram_SearchText;
        private System.Windows.Forms.Button Ram_BtnFilterChart;
        private System.Windows.Forms.Button Ram_BtnDuplikateAddieren;
        private System.Windows.Forms.Button Ram_BtnAufzeichnen;
        private System.Windows.Forms.DataGridView Ram_DGV;
        private System.Windows.Forms.Label Ram_LabelTimer;
        private System.Windows.Forms.RichTextBox Ram_LabelStatus;
        private System.Windows.Forms.DataVisualization.Charting.Chart Ram_Chart;
        private System.Windows.Forms.DataGridViewTextBoxColumn ProzessName;
        private System.Windows.Forms.DataGridViewTextBoxColumn PID;
        private System.Windows.Forms.DataGridViewTextBoxColumn Time;
        private System.Windows.Forms.DataGridViewTextBoxColumn LowRam;
        private System.Windows.Forms.DataGridViewTextBoxColumn AvgRam;
        private System.Windows.Forms.DataGridViewTextBoxColumn HighRam;
        private System.Windows.Forms.DataGridViewButtonColumn ToggleChart;
        private System.Windows.Forms.Button btnWarnFilter;
        private System.Windows.Forms.NumericUpDown numericUpDownWarnMB;
    }
}