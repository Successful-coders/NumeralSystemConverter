﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NumeralSystemConverter.Converter;
using System.Windows.Forms;

namespace NumeralSystemConverter
{
    public partial class Form1 : Form
    {
        private Control control = new Control();
        string clipboard = "";
        Memory<TPNumber>.FState memoryState = Memory<TPNumber>.FState.Off;
        private int prevRadix = 10;


        public Form1()
        {
            InitializeComponent();
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            sourceNumber.Text = control.Editor.Number;
            //Основание с.сч. исходного числа р1.
            sourceRadix.Value = 10;
            prevRadix = 10;
            //Обновить состояние командных кнопок.
            UpdateButtons();
        }
        //Обработчик события нажатия командной кнопки.
        private void button_Click(object sender, EventArgs e)
        {
            //ссылка на компонент, на котором кликнули мышью
            Button but = (Button)sender;
            //номер выбранной команды
            int commandIndex = Convert.ToInt16(but.Tag);
            DoCommand(commandIndex);
            UpdateButtons();
        }
        //Выполнить команду.
        private void DoCommand(int commandIndex)
        {
            sourceNumber.Text = control.DoCommand(commandIndex, ref clipboard, ref memoryState);
        }
        //Обновляет состояние командных кнопок по основанию с. сч. исходного числа.
        private void UpdateButtons()
        {
            //просмотреть все компоненты формы
            foreach (var i in Controls)
            {
                if (i is Button)//текущий компонент - командная кнопка 
                {
                    Button button = i as Button;
                    int buttonTag = Convert.ToInt16(button.Tag);
                    if (buttonTag < sourceRadix.Value)
                    {
                        button.Enabled = true;
                    }
                    if ((buttonTag >= sourceRadix.Value) && (buttonTag <= 15))
                    {
                        button.Enabled = false;
                    }
                }
            }
        }
        //Изменяет значение основания с.сч. исходного числа.
        private void sourceRadix_ValueChanged(object sender, EventArgs e)
        {
            //Обновить состояние.
            sourceNumber.Text = ConverterFrom10.Convert(ConverterTo10.Convert(sourceNumber.Text, prevRadix), Convert.ToInt32(sourceRadix.Value), 100);
            prevRadix = Convert.ToInt32(sourceRadix.Value);
            control.processor.LeftOperand.ValueString = sourceNumber.Text;
            control.Radix = (int)sourceRadix.Value;
            if (control.Editor.state == Editor.State.Choose || control.Editor.state == Editor.State.EditRight || control.Editor.state == Editor.State.Print)
            {
                control.processor.RightOperand.RadixNumber = control.Radix;
            }
            else
            {
                control.processor.LeftOperand.RadixNumber = control.Radix;
            }
            //Обновить состояние командных кнопок.
            this.UpdateP1();
        }
        //Выполняет необходимые обновления при смене ос. с.сч. р1.
        private void UpdateP1()
        {
            //Сохранить р1 в объекте управление.
            control.Radix = (int)sourceRadix.Value;
            //Обновить состояние командных кнопок.
            this.UpdateButtons();
            //sourceNumber.Text = control.DoCommand(18, ref clipboard, ref memoryState);
            //sourceNumber.Text = "0";
        }
        //Пункт меню Справка.
        private void СправкаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            HelpForm helpForm = new HelpForm();
            helpForm.Show();
        }
        //Пункт меню История.
        private void ИсторияToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            HistoryForm history = new HistoryForm();
            history.Show();
            if (control.History.Count == 0)
            {
                history.TextBox.Text = "История пуста";
            }
            else
            {
                history.TextBox.Text = "";
                for (int i = 0; i < control.History.Count; i++)
                {
                    history.TextBox.Text += control.History[i] + Environment.NewLine;
                }
            }
        }
        //Обработка алфавитно-цифровых клавиш.
        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {
            int i = -1;
            if (e.KeyChar >= 'A' && e.KeyChar <= 'F') i = (int)e.KeyChar - 'A' + 10;
            if (e.KeyChar >= 'a' && e.KeyChar <= 'f') i = (int)e.KeyChar - 'a' + 10;
            if (e.KeyChar >= '0' && e.KeyChar <= '9') i = (int)e.KeyChar - '0';
            if (e.KeyChar == '.') i = 16;
            if ((int)e.KeyChar == 8) i = 19;
            if ((int)e.KeyChar == 13) i = 18;
            if (e.KeyChar == '+') i = 21;
            if (e.KeyChar == '-') i = 22;
            if (e.KeyChar == '*') i = 23;
            if (e.KeyChar == '/') i = 24;
            if (e.KeyChar == '=' || e.KeyChar == '\n' || e.KeyChar == '\r') i = 27;
            if ((i < control.Radix) || (i >= 16)) DoCommand(i);
        }
        //Обработка клавиш управления.
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
                //Клавиша Delete.
                DoCommand(18);
            if (e.KeyCode == Keys.Execute)
                //Клавиша Execute Separator.
                DoCommand(19);
            if (e.KeyCode == Keys.Decimal)
                //Клавиша Decimal.
                DoCommand(16);
        }
    }
}
