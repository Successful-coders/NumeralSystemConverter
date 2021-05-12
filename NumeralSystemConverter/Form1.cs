using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NumeralSystemConverter.Converter;
using System.Windows.Forms;
using NumeralSystemConverter.TNumbers;
using NumeralSystemConverter.Editors;

namespace NumeralSystemConverter
{
    public partial class Form1 : Form
    {
        private Control control = new Control(new PEditor());
        private int prevRadix = 10;


        public Form1()
        {
            InitializeComponent();
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            числоСPССToolStripMenuItem_Click(null, null);

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
            sourceNumber.Text = control.DoCommand(commandIndex);
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
            control.Editor.Number = sourceNumber.Text;
            control.Radix = (int)sourceRadix.Value;
            if (control.Editor.state == AEditor.State.Choose || control.Editor.state == AEditor.State.EditRight || control.Editor.state == AEditor.State.Print)
            {
                control.processor.RightOperand = new TPNumber(sourceNumber.Text, control.Radix.ToString(), control.processor.RightOperand.ErrorLengthString);
            }
            else
            {
                if (control.processor.LeftOperand != null)
                {
                    control.processor.LeftOperand = new TPNumber(sourceNumber.Text, control.Radix.ToString(), control.processor.LeftOperand.ErrorLengthString);
                }
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
        //Пункт меню Вид.
        private void ВидToolStripLabel3_Click(object sender, EventArgs e)
        {

        }
        //Пункты меню Вид.
        private void числоСPССToolStripMenuItem_Click(object sender, EventArgs e)
        {
            control = new Control(new PEditor());
            prevRadix = 10;
            sourceNumber.Text = "0";
            control.processor.LeftOperand = new TPNumber(0, 10, 0);
            control.processor.RightOperand = new TPNumber(0, 10, 0);

            SetPSetVisible(true);
            SetFSetVisible(false);
            SetCSetVisible(false);
        }
        private void деситичнаяДробьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            control = new Control(new FEditor());
            sourceNumber.Text = "0";
            control.processor.LeftOperand = new TFractNumber();
            control.processor.RightOperand = new TFractNumber();

            SetPSetVisible(false);
            SetCSetVisible(false);
            SetFSetVisible(true);
        }
        private void комплексноеЧислоToolStripMenuItem_Click(object sender, EventArgs e)
        {
            control = new Control(new CEditor());
            sourceNumber.Text = "0";
            control.processor.LeftOperand = new TCompNumber();
            control.processor.RightOperand = new TCompNumber();

            SetPSetVisible(false);
            SetFSetVisible(false);
            SetCSetVisible(true);
        }
        private void SetPSetVisible(bool isVisible)
        {
            Set16NumericSystemButtonsVisible(isVisible);
            sourceRadix.Visible = isVisible;
            sourceRadixLabel.Visible = isVisible;
        }
        private void SetFSetVisible(bool isVisible)
        {
            button33.Visible = isVisible;

            if (isVisible)
            {
                button27.Text = "^2";
            }
            else
            {
                button27.Text = "√";
            }
        }
        private void SetCSetVisible(bool isVisible)
        {
            button34.Visible = isVisible;
            button35.Visible = isVisible;

            if (isVisible)
            {
                button27.Text = "^2";
            }
            else
            {
                button27.Text = "√";
            }
        }
        private void Set16NumericSystemButtonsVisible(bool isVisible)
        {
            button11.Visible = isVisible;
            button12.Visible = isVisible;
            button13.Visible = isVisible;
            button14.Visible = isVisible;
            button15.Visible = isVisible;
            button16.Visible = isVisible;

            button20.Visible = isVisible;
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
            if (e.KeyChar == '_') i = 33;
            if (e.KeyChar == 'i') i = 34;
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
