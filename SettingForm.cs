using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace XpadToMIDI
{
    public partial class SettingForm : Form
    {
        public SettingForm(Form1 MainForm)
        {
            InitializeComponent();
            MIDIchComboBox.SelectedIndex = MainForm.MIDIch;
            TimeSetBox.Value = MainForm.timer1.Interval;
            PCcheckBox.Checked = MainForm.PCsendSet;
            TriggerThresholdBox.Value = MainForm.TriggerThreshold;
            if(MainForm.TriggerSet == 1)
            {
                ButtonRadio.Checked = true;
            }
            else if(MainForm.TriggerSet == 2)
            {
                AnalogRadio.Checked = true;
            }
            else if(MainForm.TriggerSet == 3)
            {
                BothRadio.Checked = true;
            }
        }
        public int TriggerSet = 1;

        private void ButtonRadio_CheckedChanged(object sender, EventArgs e)
        {
            if(ButtonRadio.Checked)
            {
                TriggerSet = 1;
            }
            else if(AnalogRadio.Checked)
            {
                TriggerSet = 2;
            }
            else if(BothRadio.Checked)
            {
                TriggerSet = 3;
            }
        }
    }
}
