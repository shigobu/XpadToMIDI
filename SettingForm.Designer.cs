namespace XpadToMIDI
{
    partial class SettingForm
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
            this.MIDIchComboBox = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.TimeSetBox = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.OKbutton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.PCcheckBox = new System.Windows.Forms.CheckBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.ButtonRadio = new System.Windows.Forms.RadioButton();
            this.AnalogRadio = new System.Windows.Forms.RadioButton();
            this.BothRadio = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.TriggerThresholdBox = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.TimeSetBox)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TriggerThresholdBox)).BeginInit();
            this.SuspendLayout();
            // 
            // MIDIchComboBox
            // 
            this.MIDIchComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.MIDIchComboBox.FormattingEnabled = true;
            this.MIDIchComboBox.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10",
            "11",
            "12",
            "13",
            "14",
            "15",
            "16"});
            this.MIDIchComboBox.Location = new System.Drawing.Point(6, 21);
            this.MIDIchComboBox.Name = "MIDIchComboBox";
            this.MIDIchComboBox.Size = new System.Drawing.Size(44, 20);
            this.MIDIchComboBox.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("MS UI Gothic", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label1.Location = new System.Drawing.Point(6, 3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(93, 15);
            this.label1.TabIndex = 1;
            this.label1.Text = "MIDIチャンネル";
            // 
            // TimeSetBox
            // 
            this.TimeSetBox.Location = new System.Drawing.Point(5, 73);
            this.TimeSetBox.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.TimeSetBox.Name = "TimeSetBox";
            this.TimeSetBox.Size = new System.Drawing.Size(43, 19);
            this.TimeSetBox.TabIndex = 2;
            this.TimeSetBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.TimeSetBox.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label2.Location = new System.Drawing.Point(6, 54);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(72, 16);
            this.label2.TabIndex = 3;
            this.label2.Text = "更新頻度";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(54, 75);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(99, 12);
            this.label3.TabIndex = 4;
            this.label3.Text = "ミリ秒間隔(10-100)";
            // 
            // OKbutton
            // 
            this.OKbutton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.OKbutton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.OKbutton.Location = new System.Drawing.Point(145, 187);
            this.OKbutton.Name = "OKbutton";
            this.OKbutton.Size = new System.Drawing.Size(72, 23);
            this.OKbutton.TabIndex = 5;
            this.OKbutton.Text = "OK";
            this.OKbutton.UseVisualStyleBackColor = true;
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(223, 187);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(72, 23);
            this.cancelButton.TabIndex = 6;
            this.cancelButton.Text = "キャンセル";
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // PCcheckBox
            // 
            this.PCcheckBox.AutoSize = true;
            this.PCcheckBox.Location = new System.Drawing.Point(5, 111);
            this.PCcheckBox.Name = "PCcheckBox";
            this.PCcheckBox.Size = new System.Drawing.Size(250, 16);
            this.PCcheckBox.TabIndex = 3;
            this.PCcheckBox.Text = "プログラムチェンジは、値を変更する度に送信する";
            this.PCcheckBox.UseVisualStyleBackColor = true;
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(10, 10);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(1);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(287, 173);
            this.tabControl1.TabIndex = 7;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.PCcheckBox);
            this.tabPage1.Controls.Add(this.MIDIchComboBox);
            this.tabPage1.Controls.Add(this.TimeSetBox);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.label3);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(279, 147);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "基本設定";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.label5);
            this.tabPage2.Controls.Add(this.TriggerThresholdBox);
            this.tabPage2.Controls.Add(this.label4);
            this.tabPage2.Controls.Add(this.groupBox1);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(279, 147);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "トリガー設定";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // ButtonRadio
            // 
            this.ButtonRadio.AutoSize = true;
            this.ButtonRadio.Checked = true;
            this.ButtonRadio.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.ButtonRadio.Location = new System.Drawing.Point(6, 18);
            this.ButtonRadio.Name = "ButtonRadio";
            this.ButtonRadio.Size = new System.Drawing.Size(124, 16);
            this.ButtonRadio.TabIndex = 0;
            this.ButtonRadio.TabStop = true;
            this.ButtonRadio.Text = "ボタン入力として使用";
            this.ButtonRadio.UseVisualStyleBackColor = true;
            this.ButtonRadio.CheckedChanged += new System.EventHandler(this.ButtonRadio_CheckedChanged);
            // 
            // AnalogRadio
            // 
            this.AnalogRadio.AutoSize = true;
            this.AnalogRadio.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.AnalogRadio.Location = new System.Drawing.Point(6, 40);
            this.AnalogRadio.Name = "AnalogRadio";
            this.AnalogRadio.Size = new System.Drawing.Size(134, 16);
            this.AnalogRadio.TabIndex = 1;
            this.AnalogRadio.Text = "アナログ入力として使用";
            this.AnalogRadio.UseVisualStyleBackColor = true;
            this.AnalogRadio.CheckedChanged += new System.EventHandler(this.ButtonRadio_CheckedChanged);
            // 
            // BothRadio
            // 
            this.BothRadio.AutoSize = true;
            this.BothRadio.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.BothRadio.Location = new System.Drawing.Point(6, 62);
            this.BothRadio.Name = "BothRadio";
            this.BothRadio.Size = new System.Drawing.Size(117, 16);
            this.BothRadio.TabIndex = 2;
            this.BothRadio.Text = "ボタン・アナログ両方";
            this.BothRadio.UseVisualStyleBackColor = true;
            this.BothRadio.CheckedChanged += new System.EventHandler(this.ButtonRadio_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.ButtonRadio);
            this.groupBox1.Controls.Add(this.BothRadio);
            this.groupBox1.Controls.Add(this.AnalogRadio);
            this.groupBox1.Font = new System.Drawing.Font("MS UI Gothic", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.groupBox1.Location = new System.Drawing.Point(6, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(152, 91);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "入力方法";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("MS UI Gothic", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label4.Location = new System.Drawing.Point(165, 7);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(70, 15);
            this.label4.TabIndex = 4;
            this.label4.Text = "ボタン感度";
            // 
            // TriggerThresholdBox
            // 
            this.TriggerThresholdBox.Location = new System.Drawing.Point(168, 26);
            this.TriggerThresholdBox.Maximum = new decimal(new int[] {
            120,
            0,
            0,
            0});
            this.TriggerThresholdBox.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.TriggerThresholdBox.Name = "TriggerThresholdBox";
            this.TriggerThresholdBox.Size = new System.Drawing.Size(38, 19);
            this.TriggerThresholdBox.TabIndex = 5;
            this.TriggerThresholdBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.TriggerThresholdBox.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(212, 28);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(49, 12);
            this.label5.TabIndex = 6;
            this.label5.Text = "(10-120)";
            // 
            // SettingForm
            // 
            this.AcceptButton = this.OKbutton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(307, 222);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.OKbutton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SettingForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "設定";
            ((System.ComponentModel.ISupportInitialize)(this.TimeSetBox)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TriggerThresholdBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button OKbutton;
        private System.Windows.Forms.Button cancelButton;
        public System.Windows.Forms.NumericUpDown TimeSetBox;
        public System.Windows.Forms.ComboBox MIDIchComboBox;
        public System.Windows.Forms.CheckBox PCcheckBox;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton ButtonRadio;
        private System.Windows.Forms.RadioButton BothRadio;
        private System.Windows.Forms.RadioButton AnalogRadio;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        public System.Windows.Forms.NumericUpDown TriggerThresholdBox;
    }
}