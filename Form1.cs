using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Controller;

namespace XpadToMIDI
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public int MIDIch = 0;
        public bool PCsendSet = true;
        bool LoadOnce = false;
        uint selectedController = 0;
        public int TriggerThreshold = 10;
        public int TriggerSet = 1;
        public int analogAsobi = 10;

        //MIDIの送信  
        private void MIDIout(ComboBox cb, NumericUpDown tb1, Control tb2, CheckBox chb)
        {
            if (cb.Text == "割り当てなし" || cb.Text == "無し" || LoadigNow)
            {
                return;
            }

            byte[] byMessage1 = new byte[3];

            byMessage1[1] = byte.Parse(tb1.Text);
            byMessage1[2] = byte.Parse(tb2.Text);

            if (tb2.BackColor == Color.Yellow && cb.Text == "ノート")
            {
                byMessage1[0] = (byte)(0x90 + MIDIch);
                midiOut.PutMIDIMessage(byMessage1);
            }
            else if (cb.Text == "CC" || cb.Text == "PAN")
            {
                if ((tb2.BackColor == SystemColors.Window && chb != null) == false)
                {
                    byMessage1[0] = (byte)(0xB0 + MIDIch);
                    midiOut.PutMIDIMessage(byMessage1);
                }
            }
            else if (cb.Text == "PB")
            {
                byMessage1[0] = (byte)(0xE0 + MIDIch);
                midiOut.PutMIDIMessage(byMessage1);
            }
            else if (cb.Text == "ﾌﾟﾛｸﾞﾗﾑﾁｪﾝｼﾞ送信")
            {
                byMessage1[0] = (byte)(0xc0 + MIDIch);
                byMessage1[2] = 0;
                midiOut.PutMIDIMessage(byMessage1);
            }
            if (tb2.BackColor == SystemColors.Window)
            {
                if (chb == null)
                {
                    return;
                }
                if (chb.Checked)
                {
                    if (cb.Text == "ノート")
                    {
                        byMessage1[0] = (byte)(0x80 + MIDIch);
                        midiOut.PutMIDIMessage(byMessage1);
                        return;
                    }
                    else if (cb.Text == "CC")
                    {
                        byMessage1[0] = (byte)(0xB0 + MIDIch);
                        byMessage1[2] = 0;
                        midiOut.PutMIDIMessage(byMessage1);
                        return;
                    }
                }
            }
        }

        CMIDIOut midiOut = new CMIDIOut();

        //MIDIsetのサブルーチン
        private void MIDIset()
        {
            LoadOnce = false;

            CMIDIIOLib.SetLanguage(CMIDIIOLib.Japanese);
            CMIDIIOLib.SetIsShowDetail(true);

            mIDIDataSet.MIDIDataTable.Clear();

            int devicenum = CMIDIOut.GetDeviceNum();
            for (int count = 0; count < devicenum; count++)
            {
                string deviceName = CMIDIOut.GetDeviceName(count);
                mIDIDataSet.MIDIDataTable.AddMIDIDataTableRow(deviceName);
            }
            MIDIch = 0;
            MIDIchLabel.Text = "ch" + (MIDIch + 1).ToString("D2");

            LoadOnce = true;

        }

        //ロードのイベントハンドラ
        private void Form1_Load(object sender, EventArgs e)
        {
            noteDataSet.NoteData.AddNoteDataRow("ノート");
            noteDataSet.NoteData.AddNoteDataRow("CC");
            noteDataSet.NoteData.AddNoteDataRow("ﾌﾟﾛｸﾞﾗﾑﾁｪﾝｼﾞ送信");
            noteDataSet.NoteData.AddNoteDataRow("割り当て無し");
            ccpbDataSet.CCPBdataTable.AddCCPBdataTableRow("CC");
            ccpbDataSet.CCPBdataTable.AddCCPBdataTableRow("PAN");
            ccpbDataSet.CCPBdataTable.AddCCPBdataTableRow("PB");
            ccpbDataSet.CCPBdataTable.AddCCPBdataTableRow("無し");
            POVpictureBox.Image = XpadToMIDI.Properties.Resources.naka;
            MIDIchLabel.Text = "ch" + (MIDIch + 1).ToString("D2");
            tabControl1.SelectedIndex = 1;
            tabControl1.SelectedIndex = 0;
            timer1.Enabled = true;
        }

        //ショウンのイベントハンドラ
        private void Form1_Shown(object sender, EventArgs e)
        {
            MIDIset();
            LoadData("setting.txt");
            //midiOut.Open(MIDIcomboBox.Text);
            MIDIopen();
        }
        //MIDIデバイスを開く関数
        bool MIDIopenOnse = false;
        private void MIDIopen()
        {
            try
            {
                if (!MIDIopenOnse)
                {
                    midiOut.Open(MIDIcomboBox.Text);
                }
                else
                {
                    midiOut.Reopen(MIDIcomboBox.Text);
                }
                MIDIopenOnse = true;
                tabControl1.Enabled = true;
            }
            catch (ApplicationException)
            {
                tabControl1.Enabled = false;
                MessageBox.Show("MIDIデバイスが開けませんでした。\n他のアプリケーションで使用中の可能性があります。",
                    "MIDIデバイスが開けません",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Hand);
            }
        }

        //MIDI得ボタンクリックのイベントハンドラ
        private void MIDIshutokuButton_Click(object sender, EventArgs e)
        {
            MIDIset();
            //midiOut.Reopen(MIDIcomboBox.Text);
            MIDIopen();
        }

        //引数かける-1の関数
        private int mainasuone(int x)
        {
            x = x * -1;
            return x;
        }

        //軸の状態を+と-で分けて絶対値化
        private void zikustate(out int P, out int M, int state)
        {
            P = 0; M = 0;
            if (state > 0)
            {
                P = Math.Abs(state);
            }
            else if (state < 0)
            {
                M = Math.Abs(state) - 1;
            }
        }

        //タイマーイベントハンドラ
        private void timer1_Tick(object sender, EventArgs e)
        {
            uint i;
            RadioButton[] PadRadiButton = new RadioButton[4] { pad1RadioButton, pad2RadioButton, pad3RadioButton, pad4RadioButton };

            //コントローラーの接続状態確認
            for (i=0;i<4;i++)
            {
                if(XInput.IsConnected(i))
                {
                    PadRadiButton[i].BackColor = Color.Lime;
                }
                else
                {
                    PadRadiButton[i].BackColor = SystemColors.Control;
                }
            }
            //選択されているコントローラーの設定
            if(pad1RadioButton.Checked)
            {
                selectedController = 0;
            }
            else if(pad2RadioButton.Checked)
            {
                selectedController = 1;
            }
            else if(pad3RadioButton.Checked)
            {
                selectedController = 2;
            }
            else if(pad4RadioButton.Checked)
            {
                selectedController = 3;
            }
            
            if(XInput.IsConnected(selectedController) == false || tabControl1.Enabled == false)
            {
                return;
            }

            //トリガーの設定
            if(TriggerSet == 1)
            {
                LTcomboBox.Enabled = false;
                LTcomboBox.SelectedIndex = 3;
                RTcomboBox.Enabled = false;
                RTcomboBox.SelectedIndex = 3;
                button7comboBox.Enabled = true;
                button7textBox1.Enabled = true;
                button7textBox2.Enabled = true;
                button7CheckBox.Enabled = true;
                button8comboBox.Enabled = true;
                button8textBox1.Enabled = true;
                button8textBox2.Enabled = true;
                button8checkBox.Enabled = true;
            }
            else if(TriggerSet == 2)
            {
                button7comboBox.Enabled = false;
                button7comboBox.SelectedIndex = 3;
                button7textBox1.Enabled = false;
                button7textBox2.Enabled = false;
                button7CheckBox.Enabled = false;
                LTcomboBox.Enabled = true;
                button8comboBox.Enabled = false;
                button8comboBox.SelectedIndex = 3;
                button8textBox1.Enabled = false;
                button8textBox2.Enabled = false;
                button8checkBox.Enabled = false;
                RTcomboBox.Enabled = true;
            }
            else if(TriggerSet == 3)
            {
                button7comboBox.Enabled = true;
                button7textBox1.Enabled = true;
                button7textBox2.Enabled = true;
                button7CheckBox.Enabled = true;
                LTcomboBox.Enabled = true;
                button8comboBox.Enabled = true;
                button8textBox1.Enabled = true;
                button8textBox2.Enabled = true;
                button8checkBox.Enabled = true;
                RTcomboBox.Enabled = true;
            }
            XInput Xcont = new XInput(selectedController);
            XInputState contState = Xcont.GetState();
            XInputGamepadState Xpad = contState.Gamepad;
            XInputButtonKind buttonFlags = Xpad.Buttons;

            //↓ボタン
            if (buttonFlags.HasFlag(XInputButtonKind.A))
            {
                button1textBox2.BackColor = Color.Yellow;
                button1textBox2.AutoSize = true;
            }
            else
            {
                button1textBox2.BackColor = SystemColors.Window;
                button1textBox2.AutoSize = false;
            }
            if (buttonFlags.HasFlag(XInputButtonKind.B))
            {
                button2textBox2.BackColor = Color.Yellow;
                button2textBox2.AutoSize = true;
            }
            else
            {
                button2textBox2.BackColor = SystemColors.Window;
                button2textBox2.AutoSize = false;
            }
            if (buttonFlags.HasFlag(XInputButtonKind.X))
            {
                button3textBox2.BackColor = Color.Yellow;
                button3textBox2.AutoSize = true;
            }
            else
            {
                button3textBox2.BackColor = SystemColors.Window;
                button3textBox2.AutoSize = false;
            }
            if (buttonFlags.HasFlag(XInputButtonKind.Y))
            {
                button4textBox2.BackColor = Color.Yellow;
                button4textBox2.AutoSize = true;
            }
            else
            {
                button4textBox2.BackColor = SystemColors.Window;
                button4textBox2.AutoSize = false;
            }
            if (buttonFlags.HasFlag(XInputButtonKind.LeftShoulder))
            {
                button5textBox2.BackColor = Color.Yellow;
                button5textBox2.AutoSize = true;
            }
            else
            {
                button5textBox2.BackColor = SystemColors.Window;
                button5textBox2.AutoSize = false;
            }
            if (buttonFlags.HasFlag(XInputButtonKind.RightShoulder))
            {
                button6textBox2.BackColor = Color.Yellow;
                button6textBox2.AutoSize = true;
            }
            else
            {
                button6textBox2.BackColor = SystemColors.Window;
                button6textBox2.AutoSize = false;
            }
            if (Xpad.LeftTrigger/2 > TriggerThreshold)
            {
                button7textBox2.BackColor = Color.Yellow;
                button7textBox2.AutoSize = true;
            }
            else
            {
                button7textBox2.BackColor = SystemColors.Window;
                button7textBox2.AutoSize = false;
            }
            if (Xpad.RightTrigger/2 > TriggerThreshold)
            {
                button8textBox2.BackColor = Color.Yellow;
                button8textBox2.AutoSize = true;
            }
            else
            {
                button8textBox2.BackColor = SystemColors.Window;
                button8textBox2.AutoSize = false;
            }
            if (buttonFlags.HasFlag(XInputButtonKind.Back))
            {
                button9textBox2.BackColor = Color.Yellow;
                button9textBox2.AutoSize = true;
            }
            else
            {
                button9textBox2.BackColor = SystemColors.Window;
                button9textBox2.AutoSize = false;
            }
            if (buttonFlags.HasFlag(XInputButtonKind.Start))
            {
                button10textBox2.BackColor = Color.Yellow;
                button10textBox2.AutoSize = true;
            }
            else
            {
                button10textBox2.BackColor = SystemColors.Window;
                button10textBox2.AutoSize = false;
            }
            if (buttonFlags.HasFlag(XInputButtonKind.LeftThumb))
            {
                button11textBox2.BackColor = Color.Yellow;
                button11textBox2.AutoSize = true;
            }
            else
            {
                button11textBox2.BackColor = SystemColors.Window;
                button11textBox2.AutoSize = false;
            }
            if (buttonFlags.HasFlag(XInputButtonKind.RightThumb))
            {
                button12textBox2.BackColor = Color.Yellow;
                button12textBox2.AutoSize = true;
            }
            else
            {
                button12textBox2.BackColor = SystemColors.Window;
                button12textBox2.AutoSize = false;
            }
            //十字キー
            if (buttonFlags.HasFlag(XInputButtonKind.DigitalPadUp))
            {
                POVueTextBox2.BackColor = Color.Yellow;
                POVueTextBox2.AutoSize = true;
            }
            else
            {
                POVueTextBox2.BackColor = SystemColors.Window;
                POVueTextBox2.AutoSize = false;
            }
            if (buttonFlags.HasFlag(XInputButtonKind.DigitalPadRight))
            {
                POVmigiTextBox2.BackColor = Color.Yellow;
                POVmigiTextBox2.AutoSize = true;
            }
            else
            {
                POVmigiTextBox2.BackColor = SystemColors.Window;
                POVmigiTextBox2.AutoSize = false;
            }
            if (buttonFlags.HasFlag(XInputButtonKind.DigitalPadDown))
            {
                POVsitaTextBox2.BackColor = Color.Yellow;
                POVsitaTextBox2.AutoSize = true;
            }
            else
            {
                POVsitaTextBox2.BackColor = SystemColors.Window;
                POVsitaTextBox2.AutoSize = false;
            }
            if (buttonFlags.HasFlag(XInputButtonKind.DigitalPadLeft))
            {
                POVhidariTextBox2.BackColor = Color.Yellow;
                POVhidariTextBox2.AutoSize = true;
            }
            else
            {
                POVhidariTextBox2.BackColor = SystemColors.Window;
                POVhidariTextBox2.AutoSize = false;
            }
            //十字キーの画像
            if(POVueTextBox2.BackColor == Color.Yellow && POVmigiTextBox2.BackColor == Color.Yellow)
            {
                POVpictureBox.Image = XpadToMIDI.Properties.Resources.migiue;
            }
            else if(POVmigiTextBox2.BackColor == Color.Yellow && POVsitaTextBox2.BackColor == Color.Yellow)
            {
                POVpictureBox.Image = XpadToMIDI.Properties.Resources.migisita;
            }
            else if(POVsitaTextBox2.BackColor == Color.Yellow && POVhidariTextBox2.BackColor == Color.Yellow)
            {
                POVpictureBox.Image = XpadToMIDI.Properties.Resources.hidarisita;
            }
            else if(POVhidariTextBox2.BackColor == Color.Yellow && POVueTextBox2.BackColor == Color.Yellow)
            {
                POVpictureBox.Image = XpadToMIDI.Properties.Resources.hidariue;
            }
            else if(POVueTextBox2.BackColor == Color.Yellow)
            {
                POVpictureBox.Image = XpadToMIDI.Properties.Resources.ue;
            }
            else if (POVmigiTextBox2.BackColor == Color.Yellow)
            {
                POVpictureBox.Image = XpadToMIDI.Properties.Resources.migi;
            }
            else if (POVsitaTextBox2.BackColor == Color.Yellow)
            {
                POVpictureBox.Image = XpadToMIDI.Properties.Resources.sita;
            }
            else if (POVhidariTextBox2.BackColor == Color.Yellow)
            {
                POVpictureBox.Image = XpadToMIDI.Properties.Resources.hidari;
            }
            else
            {
                POVpictureBox.Image = XpadToMIDI.Properties.Resources.naka;
            }
            
            //アナログ軸↓
            int[] P = new int[6];
            int[] M = new int[6];
            int[] analog = new int[6] { Xpad.ThumbLeftX / 256, Xpad.ThumbLeftY / 256, Xpad.ThumbRightX / 256, Xpad.ThumbRightY / 256, Xpad.LeftTrigger/2, Xpad.RightTrigger/2 };
            TrackBar[] PBar = new TrackBar[5] { XPtrackBar, YPtrackBar, ZPtrackBar, RxPtrackBar, RyPtrackBar };
            TrackBar[] MBar = new TrackBar[5] { XMtrackBar, YMtrackBar, ZMtrackBar, RxMtrackBar, RyMtrackBar };
            TextBox[] PTB = new TextBox[5] { XPtextBox2, YPtextBox2, ZPtextBox2, RxPtextBox2, RTtextBox2 };
            TextBox[] MTB = new TextBox[5] { XMtextBox2, YMtextBox2, ZMtextBox2, RxMtextBox2, LTtextBox2 };
            NumericUpDown[] PTB1 = new NumericUpDown[5] { XPtextBox1, YPtextBox1, ZPtextBox1, RxPtextBox1, RTtextBox1 };
            NumericUpDown[] MTB1 = new NumericUpDown[5] { XMtextBox1, YMtextBox1, ZMtextBox1, RxMtextBox1, LTtextBox1 };
            CheckBox[] PCB = new CheckBox[5] { XPcheckBox, YPcheckBox, ZPcheckBox, RxPcheckBox, RTcheckBox };
            CheckBox[] MCB = new CheckBox[5] { XMcheckBox, YMcheckBox, ZMcheckBox, RxMcheckBox, LTcheckBox };
            ComboBox[] PCom = new ComboBox[5] { XPcomboBox, YPcomboBox, ZPcomboBox, RxPcomboBox, RTcomboBox };
            ComboBox[] MCom = new ComboBox[5] { XMcomboBox, YMcomboBox, ZMcomboBox, RxMcomboBox, LTcomboBox };

            for (i = 0; i < 4; i++)
            {
                zikustate(out P[i], out M[i], analog[i]);
            }
            M[4] = analog[4]; P[4] = analog[5];
            //アナログ軸
            for (i = 0; i < 5; i++)
            {
                PBar[i].Value = P[i];//プラス
                if (P[i] <= analogAsobi) { P[i] = 0; }
                
                if (PCom[i].Text == "PB")
                {
                    PCB[i].Enabled = false;
                    P[i] = P[i] / 2 + 64;
                    PTB1[i].Value = P[i];
                }
                else if (PCom[i].Text == "PAN")
                {
                    PCB[i].Enabled = false;
                    P[i] = P[i] / 2 + 64;
                }
                else if (PCom[i].Text == "CC")
                {
                    PCB[i].Enabled = true;
                }
                else if (PCom[i].Text == "無し")
                {
                    PCB[i].Enabled = false;
                }
                if (PCB[i].Checked)
                {
                    P[i] = 127 - P[i];
                }
                PTB[i].Text = P[i].ToString();

                MBar[i].Value = mainasuone(M[i]);//マイナス
                if (M[i] <= analogAsobi) { M[i] = 0; }

                if (MCom[i].Text == "PB")
                {
                    MCB[i].Enabled = false;
                    M[i] = 64 - M[i] / 2;
                    MTB1[i].Value = M[i];
                }
                else if (PCom[i].Text == "PAN")
                {
                    MCB[i].Enabled = false;
                    M[i] = 64 - M[i] / 2;
                }
                else if (MCom[i].Text == "CC")
                {
                    MCB[i].Enabled = true;
                }
                else if (MCom[i].Text == "無し")
                {
                    MCB[i].Enabled = false;
                }
                if (MCB[i].Checked)
                {
                    M[i] = 127 - M[i];
                }
                MTB[i].Text = M[i].ToString();
                if (PCom[i].Text == "PB" || PCom[i].Text == "PAN")
                {
                    PCB[i].Checked = false;
                    MCB[i].Checked = false;
                }

            }

        }

        //MIDIコンボボックスが選択された時のイベントハンドラ
        private void MIDIcomboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if(LoadOnce)
            {
                //midiOut.Reopen(MIDIcomboBox.Text);
                MIDIopen();
            }
        }

        //オールノートOFFボタンクリックのイベントハンドラ
        private void AllNoteOff_Click(object sender, EventArgs e)
        {
            byte[] byMessage1 = new byte[3] { (byte)(0xB0 + MIDIch), 0x7B, 0x00 };
            midiOut.PutMIDIMessage(byMessage1);
        }

        //MIDIノート送信ここから↓
        //ボタン
        private void textBox1_BackColorChanged(object sender, EventArgs e)
        {
            MIDIout(button1comboBox, button1textBox1, button1textBox2, button1CheckBox);
        }
        private void button2textBox2_BackColorChanged(object sender, EventArgs e)
        {
            MIDIout(button2comboBox, button2textBox1, button2textBox2, button2CheckBox);
        }
        private void button3textBox2_BackColorChanged(object sender, EventArgs e)
        {
            MIDIout(button3comboBox, button3textBox1, button3textBox2, button3CheckBox);
        }
        private void button4textBox2_BackColorChanged(object sender, EventArgs e)
        {
            MIDIout(button4comboBox, button4textBox1, button4textBox2, button4CheckBox);
        }
        private void button5textBox2_BackColorChanged(object sender, EventArgs e)
        {
            MIDIout(button5comboBox, button5textBox1, button5textBox2, button5CheckBox);
        }
        private void button6textBox2_BackColorChanged(object sender, EventArgs e)
        {
            MIDIout(button6comboBox, button6textBox1, button6textBox2, button6CheckBox);
        }
        private void button7textBox2_BackColorChanged(object sender, EventArgs e)
        {
            MIDIout(button7comboBox, button7textBox1, button7textBox2, button7CheckBox);
        }
        private void button8textBox2_BackColorChanged(object sender, EventArgs e)
        {
            MIDIout(button8comboBox, button8textBox1, button8textBox2, button8checkBox);
        }
        private void button9textBox2_BackColorChanged(object sender, EventArgs e)
        {
            MIDIout(button9comboBox, button9textBox1, button9textBox2, button9CheckBox);
        }
        private void button10textBox2_BackColorChanged(object sender, EventArgs e)
        {
            MIDIout(button10comboBox, button10textBox1, button10textBox2, button10CheckBox);
        }
        private void button11textBox2_BackColorChanged(object sender, EventArgs e)
        {
            MIDIout(button11comboBox, button11textBox1, button11textBox2, button11CheckBox);
        }
        private void button12textBox2_BackColorChanged(object sender, EventArgs e)
        {
            MIDIout(button12comboBox, button12textBox1, button12textBox2, button12CheckBox);
        }
        //POV
        private void POVueTextBox2_BackColorChanged(object sender, EventArgs e)
        {
            MIDIout(POVueComboBox, POVueTextBox1, POVueTextBox2, POVueCheckBox);
        }
        private void POVmigiTextBox2_BackColorChanged(object sender, EventArgs e)
        {
            MIDIout(POVmigiComboBox, POVmigiTextBox1, POVmigiTextBox2, POVmigiCheckBox);
        }
        private void POVsitaTextBox2_BackColorChanged(object sender, EventArgs e)
        {
            MIDIout(POVsitaComboBox, POVsitaTextBox1, POVsitaTextBox2, POVsitaCheckBox);
        }
        private void POVhidariTextBox2_BackColorChanged(object sender, EventArgs e)
        {
            MIDIout(POVhidariComboBox, POVhidariTextBox1, POVhidariTextBox2, POVhidariCheckBox);
        }
        //アナログ軸
        private void XMtextBox2_TextChanged(object sender, EventArgs e)
        {
            MIDIout(XMcomboBox, XMtextBox1, XMtextBox2, null);
        }
        private void XPtextBox2_TextChanged(object sender, EventArgs e)
        {
            MIDIout(XPcomboBox, XPtextBox1, XPtextBox2, null);
        }
        private void YMtextBox2_TextChanged(object sender, EventArgs e)
        {
            MIDIout(YMcomboBox, YMtextBox1, YMtextBox2, null);
        }
        private void YPtextBox2_TextChanged(object sender, EventArgs e)
        {
            MIDIout(YPcomboBox, YPtextBox1, YPtextBox2, null);
        }
        private void ZMtextBox2_TextChanged(object sender, EventArgs e)
        {
            MIDIout(ZMcomboBox, ZMtextBox1, ZMtextBox2, null);
        }
        private void ZPtextBox2_TextChanged(object sender, EventArgs e)
        {
            MIDIout(ZPcomboBox, ZPtextBox1, ZPtextBox2, null);
        }
        private void RxMtextBox2_TextChanged(object sender, EventArgs e)
        {
            MIDIout(RxMcomboBox, RxMtextBox1, RxMtextBox2, null);
        }
        private void RxPtextBox2_TextChanged(object sender, EventArgs e)
        {
            MIDIout(RxPcomboBox, RxPtextBox1, RxPtextBox2, null);
        }
        private void RyMtextBox2_TextChanged(object sender, EventArgs e)
        {
            MIDIout(LTcomboBox, LTtextBox1, LTtextBox2, null);
        }
        private void RyPtextBox2_TextChanged(object sender, EventArgs e)
        {
            MIDIout(RTcomboBox, RTtextBox1, RTtextBox2, null);
        }
        //MIDIノート送信ここまで↑

        //フォームクローズ
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            midiOut.Close();
            LoadOnce = false;
            if (SaveCheckBox.Checked)
            {
                SaveData("setting.txt");
            }
        }

        //軸コンボボックスイベントハンドラ
        private void XMcomboBox_DropDownClosed(object sender, EventArgs e)
        {
            if (XMcomboBox.Text == "CC")
            {
                XPtextBox1.Text = "1";
                XMtextBox1.Text = "1";
            }
            if (XMcomboBox.Text == "PAN")
            {
                XPtextBox1.Text = "10";
                XMtextBox1.Text = "10";
            }
            if (((ComboBox)sender).Text == "PB" || ((ComboBox)sender).Text == "無し")
            {
                if (((ComboBox)sender).Text == "PB")
                {
                    XPtextBox1.Enabled = false;
                    XMtextBox1.Enabled = false;
                    XPtextBox2.Enabled = true;
                    XMtextBox2.Enabled = true;
                }
                else
                {
                    XPtextBox1.Enabled = false;
                    XMtextBox1.Enabled = false;
                    XPtextBox2.Enabled = false;
                    XMtextBox2.Enabled = false;
                }
            }
            else
            {
                XPtextBox1.Enabled = true;
                XMtextBox1.Enabled = true;
                XPtextBox2.Enabled = true;
                XMtextBox2.Enabled = true;
            }
        }
        private void YMcomboBox_DropDownClosed(object sender, EventArgs e)
        {
            if (YMcomboBox.Text == "CC")
            {
                YPtextBox1.Text = "1";
                YMtextBox1.Text = "1";
            }
            if (YMcomboBox.Text == "PAN")
            {
                YPtextBox1.Text = "10";
                YMtextBox1.Text = "10";
            }
            if (((ComboBox)sender).Text == "PB" || ((ComboBox)sender).Text == "無し")
            {
                if (((ComboBox)sender).Text == "PB")
                {
                    YPtextBox1.Enabled = false;
                    YMtextBox1.Enabled = false;
                    YPtextBox2.Enabled = true;
                    YMtextBox2.Enabled = true;
                }
                else
                {
                    YPtextBox1.Enabled = false;
                    YMtextBox1.Enabled = false;
                    YPtextBox2.Enabled = false;
                    YMtextBox2.Enabled = false;
                }
            }
            else
            {
                YPtextBox1.Enabled = true;
                YMtextBox1.Enabled = true;
                YPtextBox2.Enabled = true;
                YMtextBox2.Enabled = true;
            }
        }
        private void ZMcomboBox_TextChanged(object sender, EventArgs e)
        {
            if (ZMcomboBox.Text == "CC")
            {
                ZPtextBox1.Text = "1";
                ZMtextBox1.Text = "1";
            }
            if (ZMcomboBox.Text == "PAN")
            {
                ZPtextBox1.Text = "10";
                ZMtextBox1.Text = "10";
            }
            if (((ComboBox)sender).Text == "PB" || ((ComboBox)sender).Text == "無し")
            {
                if (((ComboBox)sender).Text == "PB")
                {
                    ZPtextBox1.Enabled = false;
                    ZMtextBox1.Enabled = false;
                    ZPtextBox2.Enabled = true;
                    ZMtextBox2.Enabled = true;
                }
                else
                {
                    ZPtextBox1.Enabled = false;
                    ZMtextBox1.Enabled = false;
                    ZPtextBox2.Enabled = false;
                    ZMtextBox2.Enabled = false;
                }
            }
            else
            {
                ZPtextBox1.Enabled = true;
                ZMtextBox1.Enabled = true;
                ZPtextBox2.Enabled = true;
                ZMtextBox2.Enabled = true;
            }
        }
        private void RxMcomboBox_TextChanged(object sender, EventArgs e)
        {
            if (RxMcomboBox.Text == "CC")
            {
                RxPtextBox1.Text = "1";
                RxMtextBox1.Text = "1";
            }
            if (RxMcomboBox.Text == "PAN")
            {
                RxPtextBox1.Text = "10";
                RxMtextBox1.Text = "10";
            }
            if (((ComboBox)sender).Text == "PB" || ((ComboBox)sender).Text == "無し")
            {
                if (((ComboBox)sender).Text == "PB")
                {
                    RxPtextBox1.Enabled = false;
                    RxMtextBox1.Enabled = false;
                    RxPtextBox2.Enabled = true;
                    RxMtextBox2.Enabled = true;
                }
                else
                {
                    RxPtextBox1.Enabled = false;
                    RxMtextBox1.Enabled = false;
                    RxPtextBox2.Enabled = false;
                    RxMtextBox2.Enabled = false;
                }
            }
            else
            {
                RxPtextBox1.Enabled = true;
                RxMtextBox1.Enabled = true;
                RxPtextBox2.Enabled = true;
                RxMtextBox2.Enabled = true;
            }
        }
        private void RyMcomboBox_TextChanged(object sender, EventArgs e)
        {
            if (LTcomboBox.Text == "CC")
            {
                RTtextBox1.Text = "1";
                LTtextBox1.Text = "1";
            }
            if (LTcomboBox.Text == "PAN")
            {
                RTtextBox1.Text = "10";
                LTtextBox1.Text = "10";
            }
            if (((ComboBox)sender).Text == "PB" || ((ComboBox)sender).Text == "無し")
            {
                if (((ComboBox)sender).Text == "PB")
                {
                    RTtextBox1.Enabled = false;
                    LTtextBox1.Enabled = false;
                    RTtextBox2.Enabled = true;
                    LTtextBox2.Enabled = true;
                }
                else
                {
                    RTtextBox1.Enabled = false;
                    LTtextBox1.Enabled = false;
                    RTtextBox2.Enabled = false;
                    LTtextBox2.Enabled = false;
                }
            }
            else
            {
                RTtextBox1.Enabled = true;
                LTtextBox1.Enabled = true;
                RTtextBox2.Enabled = true;
                LTtextBox2.Enabled = true;
            }
        }

        //Reset All Controllerボタンクリック
        private void button1_Click(object sender, EventArgs e)
        {
            byte[] byMessage1 = new byte[3] { (byte)(0xB0 + MIDIch), 0x79, 0x00 };
            midiOut.PutMIDIMessage(byMessage1);
        }

        //セーブの関数
        private void SaveData(string filename)
        {
            string strData = " ";
            System.IO.StreamWriter ds = new System.IO.StreamWriter(filename, false, System.Text.Encoding.Default);

            ds.WriteLine("XpadToMIDI setting data Ver 1.0");

            ds.WriteLine(MIDIcomboBox.SelectedIndex);

            int i;
            ComboBox[] buttonCB = new ComboBox[12] { button1comboBox, button2comboBox, button3comboBox, button4comboBox, button5comboBox, button6comboBox, button7comboBox, button8comboBox, button9comboBox, button10comboBox, button11comboBox, button12comboBox };
            NumericUpDown[] buttonTB1 = new NumericUpDown[12] { button1textBox1, button2textBox1, button3textBox1, button4textBox1, button5textBox1, button6textBox1, button7textBox1, button8textBox1, button9textBox1, button10textBox1, button11textBox1, button12textBox1 };
            NumericUpDown[] buttonTB2 = new NumericUpDown[12] { button1textBox2, button2textBox2, button3textBox2, button4textBox2, button5textBox2, button6textBox2, button7textBox2, button8textBox2, button9textBox2, button10textBox2, button11textBox2, button12textBox2 };
            CheckBox[] buttonCHB = new CheckBox[12] { button1CheckBox, button2CheckBox, button3CheckBox, button4CheckBox, button5CheckBox, button6CheckBox, button7CheckBox, button8checkBox, button9CheckBox, button10CheckBox, button11CheckBox, button12CheckBox };
            ComboBox[] POVcb = new ComboBox[4] { POVueComboBox, POVmigiComboBox, POVsitaComboBox, POVhidariComboBox };
            NumericUpDown[] POVtb1 = new NumericUpDown[4] { POVueTextBox1, POVmigiTextBox1, POVsitaTextBox1, POVhidariTextBox1 };
            NumericUpDown[] POVtb2 = new NumericUpDown[4] { POVueTextBox2, POVmigiTextBox2, POVsitaTextBox2, POVhidariTextBox2 };
            CheckBox[] POVchb = new CheckBox[4] { POVueCheckBox, POVmigiCheckBox, POVsitaCheckBox, POVhidariCheckBox };
            NumericUpDown[] zikuTB1 = new NumericUpDown[10] { XMtextBox1, YMtextBox1, ZMtextBox1, RxMtextBox1, LTtextBox1, XPtextBox1, YPtextBox1, ZPtextBox1, RxPtextBox1, RTtextBox1 };
            CheckBox[] zikuCB = new CheckBox[10] { XMcheckBox, YMcheckBox, ZMcheckBox, RxMcheckBox, LTcheckBox, XPcheckBox, YPcheckBox, ZPcheckBox, RxPcheckBox, RTcheckBox };
            ComboBox[] zikuCom = new ComboBox[10] { XMcomboBox, YMcomboBox, ZMcomboBox, RxMcomboBox, LTcomboBox, XPcomboBox, YPcomboBox, ZPcomboBox, RxPcomboBox, RTcomboBox };

            for (i = 0; i < 12; i++)
            {
                strData = buttonCB[i].SelectedIndex + ","
                        + buttonTB1[i].Text + ","
                        + buttonTB2[i].Text + ","
                        + buttonCHB[i].Checked;
                ds.WriteLine(strData);
            }
            for (i = 0; i < 4; i++)
            {
                strData = POVcb[i].SelectedIndex + ","
                        + POVtb1[i].Text + ","
                        + POVtb2[i].Text + ","
                        + POVchb[i].Checked;
                ds.WriteLine(strData);
            }

            for (i = 0; i < 10; i++)
            {
                strData = zikuCom[i].SelectedIndex + ","
                        + zikuTB1[i].Text + ","
                        + zikuCB[i].Checked;
                ds.WriteLine(strData);
            }
            ds.Close();

        }

        bool LoadigNow = false;

        //ロードの関数
        private void LoadData(string fileName)
        {
            
            string delimStr = ",";
            char[] delimiter = delimStr.ToCharArray();
            string[] strData;
            string strLine;

            bool DirectInputRead = false;

            LoadOnce = false;
            LoadigNow = true;

            if (System.IO.File.Exists(fileName))
            {
                if (System.IO.Path.GetExtension(fileName) != ".txt")
                {
                    MessageBox.Show("このファイルは対応していません。",
                    "ファイル読み込みエラー",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Hand);

                    LoadigNow = false;
                    LoadOnce = true;
                    return;
                }

                System.IO.StreamReader dsr = new System.IO.StreamReader(fileName, System.Text.Encoding.Default);

                string settingDataVer = dsr.ReadLine();
                if (settingDataVer != "XpadToMIDI setting data Ver 1.0")
                {
                    DialogResult MBresult = MessageBox.Show("このファイルはDirectInput用です\n\"ボタン13\"と\"Rz軸\"の設定は反映されません\nよろしいですか？？",
                    "ファイル読み込みエラー",
                    MessageBoxButtons.OKCancel,
                    MessageBoxIcon.Hand);

                    if(MBresult == DialogResult.OK)
                    {
                        DirectInputRead = true;
                    }
                    else
                    {
                        dsr.Close();
                        LoadigNow = false;
                        LoadOnce = true;
                        return;
                    }
                    
                }

                try
                {
                    int itemcount = MIDIcomboBox.Items.Count;
                    int itemindex = int.Parse(dsr.ReadLine());
                    if (itemindex < itemcount)
                    {
                        MIDIcomboBox.SelectedIndex = itemindex;
                    }

                    ComboBox[] buttonCB = new ComboBox[16] { button1comboBox, button2comboBox, button3comboBox, button4comboBox, button5comboBox, button6comboBox, button7comboBox, button8comboBox, button9comboBox, button10comboBox, button11comboBox, button12comboBox, POVueComboBox, POVmigiComboBox, POVsitaComboBox, POVhidariComboBox };
                    NumericUpDown[] buttonTB1 = new NumericUpDown[16] { button1textBox1, button2textBox1, button3textBox1, button4textBox1, button5textBox1, button6textBox1, button7textBox1, button8textBox1, button9textBox1, button10textBox1, button11textBox1, button12textBox1, POVueTextBox1, POVmigiTextBox1, POVsitaTextBox1, POVhidariTextBox1 };
                    NumericUpDown[] buttonTB2 = new NumericUpDown[16] { button1textBox2, button2textBox2, button3textBox2, button4textBox2, button5textBox2, button6textBox2, button7textBox2, button8textBox2, button9textBox2, button10textBox2, button11textBox2, button12textBox2, POVueTextBox2, POVmigiTextBox2, POVsitaTextBox2, POVhidariTextBox2 };
                    CheckBox[] buttonCHB = new CheckBox[16] { button1CheckBox, button2CheckBox, button3CheckBox, button4CheckBox, button5CheckBox, button6CheckBox, button7CheckBox, button8checkBox, button9CheckBox, button10CheckBox, button11CheckBox, button12CheckBox, POVueCheckBox, POVmigiCheckBox, POVsitaCheckBox, POVhidariCheckBox };
                    for (int i = 0; i < 16; i++)
                    {
                        if(DirectInputRead && i == 12)
                        {
                            dsr.ReadLine();
                        }
                        strLine = dsr.ReadLine();
                        strData = strLine.Split(delimiter);
                        if (strData.Length == 4)
                        {
                            buttonCB[i].SelectedIndex = int.Parse(strData[0]);
                            buttonCHB[i].Checked = bool.Parse(strData[3]);
                        }
                        else if (strData[0] == "0")
                        {
                            buttonCB[i].SelectedIndex = 0;
                            buttonCHB[i].Checked = true;
                        }
                        else if (strData[0] == "1")
                        {
                            buttonCB[i].SelectedIndex = 0;
                            buttonCHB[i].Checked = false;
                        }
                        else if (strData[0] == "2")
                        {
                            buttonCB[i].SelectedIndex = 1;
                            buttonCHB[i].Checked = true;
                        }
                        else
                        {
                            buttonCB[i].SelectedIndex = int.Parse(strData[0]) - 1;
                        }
                        buttonTB1[i].Value = decimal.Parse(strData[1]);
                        buttonTB2[i].Value = decimal.Parse(strData[2]);
                    }

                    NumericUpDown[] zikuTB1 = new NumericUpDown[10] { XMtextBox1, YMtextBox1, ZMtextBox1, RxMtextBox1, LTtextBox1, XPtextBox1, YPtextBox1, ZPtextBox1, RxPtextBox1, RTtextBox1, };
                    CheckBox[] zikuCB = new CheckBox[10] { XMcheckBox, YMcheckBox, ZMcheckBox, RxMcheckBox, LTcheckBox, XPcheckBox, YPcheckBox, ZPcheckBox, RxPcheckBox, RTcheckBox, };
                    ComboBox[] zikuCom = new ComboBox[10] { XMcomboBox, YMcomboBox, ZMcomboBox, RxMcomboBox, LTcomboBox, XPcomboBox, YPcomboBox, ZPcomboBox, RxPcomboBox, RTcomboBox, };
                    for (int i = 0; i < 10; i++)
                    {
                        strLine = dsr.ReadLine();
                        strData = strLine.Split(delimiter);
                        zikuCom[i].SelectedIndex = int.Parse(strData[0]);
                        zikuTB1[i].Value = decimal.Parse(strData[1]);
                        zikuCB[i].Checked = bool.Parse(strData[2]);
                    }
                }
                catch (FormatException)
                {
                    MessageBox.Show("ファイルの読み込み中にエラーが出ました。",
                    "エラー",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Hand);
                    
                }

                dsr.Close();    
            }
            LoadigNow = false;
            LoadOnce = true;
        }

        //軸データバイトの共通化
        private void datasame(ComboBox CB, NumericUpDown motoTB, NumericUpDown sakiTB)
        {
            if (CB.Text == "PAN")
            {
                sakiTB.Value = motoTB.Value;
            }
            else
            {
                return;
            }
        }
        //↑のイベントハンドラ
        private void XPtextBox1_TextChanged(object sender, EventArgs e)
        {
            datasame(XMcomboBox, XMtextBox1, XPtextBox1);
        }
        private void YMtextBox1_TextChanged(object sender, EventArgs e)
        {
            datasame(YMcomboBox, YMtextBox1, YPtextBox1);
        }
        private void ZMtextBox1_TextChanged(object sender, EventArgs e)
        {
            datasame(ZMcomboBox, ZMtextBox1, ZPtextBox1);
        }
        private void RxMtextBox1_TextChanged(object sender, EventArgs e)
        {
            datasame(RxMcomboBox, RxMtextBox1, RxPtextBox1);
        }
        private void RyMtextBox1_TextChanged(object sender, EventArgs e)
        {
            datasame(LTcomboBox, LTtextBox1, RTtextBox1);
        }
        private void XMtextBox1_TextChanged(object sender, EventArgs e)
        {
            datasame(XPcomboBox, XPtextBox1, XMtextBox1);
        }
        private void YPtextBox1_TextChanged(object sender, EventArgs e)
        {
            datasame(YPcomboBox, YPtextBox1, YMtextBox1);
        }
        private void ZPtextBox1_TextChanged(object sender, EventArgs e)
        {
            datasame(ZPcomboBox, ZPtextBox1, ZMtextBox1);
        }
        private void RxPtextBox1_TextChanged(object sender, EventArgs e)
        {
            datasame(RxPcomboBox, RxPtextBox1, RxMtextBox1);
        }
        private void RyPtextBox1_TextChanged(object sender, EventArgs e)
        {
            datasame(RTcomboBox, RTtextBox1, LTtextBox1);
        }

        //ドラッグ＆ドロップでの読み込み
        private void Form1_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop, false);
            if (files.Length != 1)
            {
                MessageBox.Show("読み込めるファイルは１つのみです。",
                    "エラー",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Hand);
            }
            else
            {
                LoadData(files[0]);
            }

        }
        private void Form1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.All;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        //コンテキストメニュー
        private void 保存ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // ダイアログのタイトルを設定する
            saveFileDialog1.Title = "保存する";

            // 初期表示するディレクトリを設定する
            saveFileDialog1.InitialDirectory = ".";

            // 初期表示するファイル名を設定する
            saveFileDialog1.FileName = "*.txt";

            // ファイルのフィルタを設定する
            saveFileDialog1.Filter = "txtファイル|*.txt|すべてのファイル|*.*";

            // ファイルの種類 の初期設定を 2 番目に設定する (初期値 1)
            saveFileDialog1.FilterIndex = 2;

            // ダイアログボックスを閉じる前に現在のディレクトリを復元する (初期値 false)
            saveFileDialog1.RestoreDirectory = true;

            saveFileDialog1.AddExtension = true;

            // ダイアログを表示し、戻り値が [OK] の場合は、選択したファイルを表示する
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                SaveData(saveFileDialog1.FileName);
            }
        }
        private void 開くToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // ダイアログボックスのタイトル
            openFileDialog1.Title = "開く";

            // 初期表示するディレクトリ
            openFileDialog1.InitialDirectory = ".";

            // デフォルトの選択ファイル名
            openFileDialog1.FileName = "*.txt";

            // [ファイルの種類]に表示するフィルタ
            openFileDialog1.Filter = "txtファイル|*.txt|すべてのファイル(*.*)|*.*";

            // [ファイルの種類]の初期表示インデックス
            openFileDialog1.FilterIndex = 1;

            // ファイル名に拡張子を自動設定するかどうか
            openFileDialog1.AddExtension = true;

            // 複数のファイルを選択するかどうか
            openFileDialog1.Multiselect = false;

            // ファイルが存在しない時に警告メッセージを表示するかどうか
            openFileDialog1.CheckFileExists = true;

            // PATHが存在しない時に警告メッセージを表示するかどうか
            openFileDialog1.CheckPathExists = true;

            // ダイアログを閉じる時に、カレントディレクトリを元に戻すかどうか
            openFileDialog1.RestoreDirectory = true;

            DialogResult btn = openFileDialog1.ShowDialog();
            if (btn == System.Windows.Forms.DialogResult.OK)
            {
                LoadData(openFileDialog1.FileName);
            }
        }

        //一括 コンボボックス
        private void button2_Click(object sender, EventArgs e)
        {
            ComboBox[] bcb = new ComboBox[16] { button1comboBox, button2comboBox, button3comboBox, button4comboBox, button5comboBox, button6comboBox, button7comboBox, button8comboBox, button9comboBox, button10comboBox, button11comboBox, button12comboBox, POVueComboBox, POVmigiComboBox, POVsitaComboBox, POVhidariComboBox };

            for (int i = 0; i < 16; i++)
            {
                bcb[i].SelectedIndex = ikkatsucomboBox1.SelectedIndex;
            }
        }

        //コンボボックスのサイズ変更＆チェックボックスの表示非表示
        private void button1comboBox_TextUpdate(object sender, EventArgs e)
        {
            if (((ComboBox)sender).Text == "ノート" || ((ComboBox)sender).Text == "CC")
            {
                ((ComboBox)sender).Width = 56;
                button1CheckBox.Visible = true;
            }
            else
            {
                ((ComboBox)sender).Width = 126;
                button1CheckBox.Visible = false;
            }

            ((ComboBox)sender).Refresh();

        }
        private void button2comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (((ComboBox)sender).Text == "ノート" || ((ComboBox)sender).Text == "CC")
            {
                ((ComboBox)sender).Width = 56;
                button2CheckBox.Visible = true;
            }
            else
            {
                ((ComboBox)sender).Width = 126;
                button2CheckBox.Visible = false;
            }

            ((ComboBox)sender).Refresh();

        }
        private void button3comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (((ComboBox)sender).Text == "ノート" || ((ComboBox)sender).Text == "CC")
            {
                ((ComboBox)sender).Width = 56;
                button3CheckBox.Visible = true;
            }
            else
            {
                ((ComboBox)sender).Width = 126;
                button3CheckBox.Visible = false;
            }

            ((ComboBox)sender).Refresh();

        }
        private void button4comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (((ComboBox)sender).Text == "ノート" || ((ComboBox)sender).Text == "CC")
            {
                ((ComboBox)sender).Width = 56;
                button4CheckBox.Visible = true;
            }
            else
            {
                ((ComboBox)sender).Width = 126;
                button4CheckBox.Visible = false;
            }

            ((ComboBox)sender).Refresh();

        }
        private void button5comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (((ComboBox)sender).Text == "ノート" || ((ComboBox)sender).Text == "CC")
            {
                ((ComboBox)sender).Width = 56;
                button5CheckBox.Visible = true;
            }
            else
            {
                ((ComboBox)sender).Width = 126;
                button5CheckBox.Visible = false;
            }

            ((ComboBox)sender).Refresh();

        }
        private void button6comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (((ComboBox)sender).Text == "ノート" || ((ComboBox)sender).Text == "CC")
            {
                ((ComboBox)sender).Width = 56;
                button6CheckBox.Visible = true;
            }
            else
            {
                ((ComboBox)sender).Width = 126;
                button6CheckBox.Visible = false;
            }

            ((ComboBox)sender).Refresh();

        }
        private void button7comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (((ComboBox)sender).Text == "ノート" || ((ComboBox)sender).Text == "CC")
            {
                ((ComboBox)sender).Width = 56;
                button7CheckBox.Visible = true;
            }
            else
            {
                ((ComboBox)sender).Width = 126;
                button7CheckBox.Visible = false;
            }

            ((ComboBox)sender).Refresh();

        }
        private void button8comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (((ComboBox)sender).Text == "ノート" || ((ComboBox)sender).Text == "CC")
            {
                ((ComboBox)sender).Width = 56;
                button8checkBox.Visible = true;
            }
            else
            {
                ((ComboBox)sender).Width = 126;
                button8checkBox.Visible = false;
            }

            ((ComboBox)sender).Refresh();

        }
        private void button9comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (((ComboBox)sender).Text == "ノート" || ((ComboBox)sender).Text == "CC")
            {
                ((ComboBox)sender).Width = 56;
                button9CheckBox.Visible = true;
            }
            else
            {
                ((ComboBox)sender).Width = 126;
                button9CheckBox.Visible = false;
            }

            ((ComboBox)sender).Refresh();

        }
        private void button10comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (((ComboBox)sender).Text == "ノート" || ((ComboBox)sender).Text == "CC")
            {
                ((ComboBox)sender).Width = 56;
                button10CheckBox.Visible = true;
            }
            else
            {
                ((ComboBox)sender).Width = 126;
                button10CheckBox.Visible = false;
            }

            ((ComboBox)sender).Refresh();

        }
        private void button11comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (((ComboBox)sender).Text == "ノート" || ((ComboBox)sender).Text == "CC")
            {
                ((ComboBox)sender).Width = 56;
                button11CheckBox.Visible = true;
            }
            else
            {
                ((ComboBox)sender).Width = 126;
                button11CheckBox.Visible = false;
            }

            ((ComboBox)sender).Refresh();

        }
        private void button12comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (((ComboBox)sender).Text == "ノート" || ((ComboBox)sender).Text == "CC")
            {
                ((ComboBox)sender).Width = 56;
                button12CheckBox.Visible = true;
            }
            else
            {
                ((ComboBox)sender).Width = 126;
                button12CheckBox.Visible = false;
            }

            ((ComboBox)sender).Refresh();

        }
        private void POVueComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (((ComboBox)sender).Text == "ノート" || ((ComboBox)sender).Text == "CC")
            {
                ((ComboBox)sender).Width = 60;
                POVueCheckBox.Visible = true;
            }
            else
            {
                ((ComboBox)sender).Width = 130;
                POVueCheckBox.Visible = false;
            }

            ((ComboBox)sender).Refresh();
        }
        private void POVmigiComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (((ComboBox)sender).Text == "ノート" || ((ComboBox)sender).Text == "CC")
            {
                ((ComboBox)sender).Width = 60;
                POVmigiCheckBox.Visible = true;
            }
            else
            {
                ((ComboBox)sender).Width = 130;
                POVmigiCheckBox.Visible = false;
            }

           ((ComboBox)sender).Refresh();
        }
        private void POVsitaComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (((ComboBox)sender).Text == "ノート" || ((ComboBox)sender).Text == "CC")
            {
                ((ComboBox)sender).Width = 60;
                POVsitaCheckBox.Visible = true;
            }
            else
            {
                ((ComboBox)sender).Width = 130;
                POVsitaCheckBox.Visible = false;
            }

           ((ComboBox)sender).Refresh();
        }
        private void POVhidariComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (((ComboBox)sender).Text == "ノート" || ((ComboBox)sender).Text == "CC")
            {
                ((ComboBox)sender).Width = 60;
                POVhidariCheckBox.Visible = true;
            }
            else
            {
                ((ComboBox)sender).Width = 130;
                POVhidariCheckBox.Visible = false;
            }

           ((ComboBox)sender).Refresh();
        }

        //一括　チェックボックス
        private void ikkatsuCheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox[] chekbox = new CheckBox[16] { button1CheckBox, button2CheckBox, button3CheckBox, button4CheckBox, button5CheckBox, button6CheckBox, button7CheckBox, button8checkBox, button9CheckBox, button10CheckBox, button11CheckBox, button12CheckBox, POVueCheckBox, POVhidariCheckBox, POVsitaCheckBox, POVmigiCheckBox };


            ikkatsuCheckBox2.Checked = ikkatsuCheckBox1.Checked;

            for (int i = 0; i < 16; i++)
            {
                chekbox[i].Checked = ikkatsuCheckBox1.Checked;
            }
        }
        private void ikkatsuCheckBox2_CheckedChanged(object sender, EventArgs e)
        {
            ikkatsuCheckBox1.Checked = ikkatsuCheckBox2.Checked;
        }

        //設定ダイアログボックス
        private void 設定ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SettingForm setting = new SettingForm(this);
            DialogResult drRet = setting.ShowDialog();

            if (drRet == DialogResult.OK)
            {
                MIDIch = setting.MIDIchComboBox.SelectedIndex;
                MIDIchLabel.Text = "ch" + (MIDIch + 1).ToString("D2");
                timer1.Interval = (int)setting.TimeSetBox.Value;
                PCsendSet = setting.PCcheckBox.Checked;
                TriggerThreshold = (int)setting.TriggerThresholdBox.Value;
                TriggerSet = setting.TriggerSet;
            }
        }

        //終了ボタン
        private void 終了ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //プログラムチェンジの自動送信
        private void button1textBox1_ValueChanged(object sender, EventArgs e)
        {
            if(LoadigNow)
            {
                return;
            }

            if (button1comboBox.Text == "ﾌﾟﾛｸﾞﾗﾑﾁｪﾝｼﾞ送信" && PCsendSet)
            {
                byte[] byMessage1 = new byte[3];

                byMessage1[0] = (byte)(0xC0 + MIDIch);
                byMessage1[1] = (byte)((NumericUpDown)sender).Value;
                byMessage1[2] = 0;

                midiOut.PutMIDIMessage(byMessage1);
            }
        }
        private void button2textBox1_ValueChanged(object sender, EventArgs e)
        {
            if (LoadigNow)
            {
                return;
            }

            if (button2comboBox.Text == "ﾌﾟﾛｸﾞﾗﾑﾁｪﾝｼﾞ送信" && PCsendSet)
            {
                byte[] byMessage1 = new byte[3];

                byMessage1[0] = (byte)(0xC0 + MIDIch);
                byMessage1[1] = (byte)((NumericUpDown)sender).Value;
                byMessage1[2] = 0;

                midiOut.PutMIDIMessage(byMessage1);
            }
        }
        private void button3textBox1_ValueChanged(object sender, EventArgs e)
        {
            if (LoadigNow)
            {
                return;
            }

            if (button3comboBox.Text == "ﾌﾟﾛｸﾞﾗﾑﾁｪﾝｼﾞ送信" && PCsendSet)
            {
                byte[] byMessage1 = new byte[3];

                byMessage1[0] = (byte)(0xC0 + MIDIch);
                byMessage1[1] = (byte)((NumericUpDown)sender).Value;
                byMessage1[2] = 0;

                midiOut.PutMIDIMessage(byMessage1);
            }
        }
        private void button4textBox1_ValueChanged(object sender, EventArgs e)
        {
            if (LoadigNow)
            {
                return;
            }

            if (button4comboBox.Text == "ﾌﾟﾛｸﾞﾗﾑﾁｪﾝｼﾞ送信" && PCsendSet)
            {
                byte[] byMessage1 = new byte[3];

                byMessage1[0] = (byte)(0xC0 + MIDIch);
                byMessage1[1] = (byte)((NumericUpDown)sender).Value;
                byMessage1[2] = 0;

                midiOut.PutMIDIMessage(byMessage1);
            }
        }
        private void button5textBox1_ValueChanged(object sender, EventArgs e)
        {
            if (LoadigNow)
            {
                return;
            }

            if (button5comboBox.Text == "ﾌﾟﾛｸﾞﾗﾑﾁｪﾝｼﾞ送信" && PCsendSet)
            {
                byte[] byMessage1 = new byte[3];

                byMessage1[0] = (byte)(0xC0 + MIDIch);
                byMessage1[1] = (byte)((NumericUpDown)sender).Value;
                byMessage1[2] = 0;

                midiOut.PutMIDIMessage(byMessage1);
            }
        }
        private void button6textBox1_ValueChanged(object sender, EventArgs e)
        {
            if (LoadigNow)
            {
                return;
            }

            if (button6comboBox.Text == "ﾌﾟﾛｸﾞﾗﾑﾁｪﾝｼﾞ送信" && PCsendSet)
            {
                byte[] byMessage1 = new byte[3];

                byMessage1[0] = (byte)(0xC0 + MIDIch);
                byMessage1[1] = (byte)((NumericUpDown)sender).Value;
                byMessage1[2] = 0;

                midiOut.PutMIDIMessage(byMessage1);
            }
        }
        private void button7textBox1_ValueChanged(object sender, EventArgs e)
        {
            if (LoadigNow)
            {
                return;
            }

            if (button7comboBox.Text == "ﾌﾟﾛｸﾞﾗﾑﾁｪﾝｼﾞ送信" && PCsendSet)
            {
                byte[] byMessage1 = new byte[3];

                byMessage1[0] = (byte)(0xC0 + MIDIch);
                byMessage1[1] = (byte)((NumericUpDown)sender).Value;
                byMessage1[2] = 0;

                midiOut.PutMIDIMessage(byMessage1);
            }
        }
        private void button8textBox1_ValueChanged(object sender, EventArgs e)
        {
            if (LoadigNow)
            {
                return;
            }

            if (button8comboBox.Text == "ﾌﾟﾛｸﾞﾗﾑﾁｪﾝｼﾞ送信" && PCsendSet)
            {
                byte[] byMessage1 = new byte[3];

                byMessage1[0] = (byte)(0xC0 + MIDIch);
                byMessage1[1] = (byte)((NumericUpDown)sender).Value;
                byMessage1[2] = 0;

                midiOut.PutMIDIMessage(byMessage1);
            }
        }
        private void button9textBox1_ValueChanged(object sender, EventArgs e)
        {
            if (LoadigNow)
            {
                return;
            }

            if (button9comboBox.Text == "ﾌﾟﾛｸﾞﾗﾑﾁｪﾝｼﾞ送信" && PCsendSet)
            {
                byte[] byMessage1 = new byte[3];

                byMessage1[0] = (byte)(0xC0 + MIDIch);
                byMessage1[1] = (byte)((NumericUpDown)sender).Value;
                byMessage1[2] = 0;

                midiOut.PutMIDIMessage(byMessage1);
            }
        }
        private void button10textBox1_ValueChanged(object sender, EventArgs e)
        {
            if (LoadigNow)
            {
                return;
            }

            if (button10comboBox.Text == "ﾌﾟﾛｸﾞﾗﾑﾁｪﾝｼﾞ送信" && PCsendSet)
            {
                byte[] byMessage1 = new byte[3];

                byMessage1[0] = (byte)(0xC0 + MIDIch);
                byMessage1[1] = (byte)((NumericUpDown)sender).Value;
                byMessage1[2] = 0;

                midiOut.PutMIDIMessage(byMessage1);
            }
        }
        private void button11textBox1_ValueChanged(object sender, EventArgs e)
        {
            if (LoadigNow)
            {
                return;
            }

            if (button11comboBox.Text == "ﾌﾟﾛｸﾞﾗﾑﾁｪﾝｼﾞ送信" && PCsendSet)
            {
                byte[] byMessage1 = new byte[3];

                byMessage1[0] = (byte)(0xC0 + MIDIch);
                byMessage1[1] = (byte)((NumericUpDown)sender).Value;
                byMessage1[2] = 0;

                midiOut.PutMIDIMessage(byMessage1);
            }
        }
        private void button12textBox1_ValueChanged(object sender, EventArgs e)
        {
            if (LoadigNow)
            {
                return;
            }

            if (button12comboBox.Text == "ﾌﾟﾛｸﾞﾗﾑﾁｪﾝｼﾞ送信" && PCsendSet)
            {
                byte[] byMessage1 = new byte[3];

                byMessage1[0] = (byte)(0xC0 + MIDIch);
                byMessage1[1] = (byte)((NumericUpDown)sender).Value;
                byMessage1[2] = 0;

                midiOut.PutMIDIMessage(byMessage1);
            }
        }
        //あそびボックスの中が変わったときの挙動
        private void asobiNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            analogAsobi = (int)asobiNumericUpDown.Value;
        }
    }
}
