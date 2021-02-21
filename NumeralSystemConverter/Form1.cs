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

namespace NumeralSystemConverter
{
    public partial class Form1 : Form
    {
        Control control = new Control();


        public Form1()
        {
            InitializeComponent();
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            sourceNumber.Text = control.Editor.Number;
            //Основание с.сч. исходного числа р1.
            sourceRadix.Value = control.SourceRadix;
            //Основание с.сч. результата р2.
            resultRadix.Value = control.ResultRadix;
            resultNumber.Text = "";
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
            if (commandIndex == 19) { resultNumber.Text = control.DoCommand(commandIndex); }
            else
            {
                if (control.State == Control.StateType.Преобразовано)
                {
                    //очистить содержимое редактора 
                    sourceNumber.Text = control.DoCommand(18);
                }
                //выполнить команду редактирования
                sourceNumber.Text = control.DoCommand(commandIndex);
                resultNumber.Text = "0";
            }
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
            sourceRadix.Value = Convert.ToByte(sourceRadix.Value);
            //Обновить состояние командных кнопок.
            this.UpdateP1();
        }
        //Выполняет необходимые обновления при смене ос. с.сч. р1.
        private void UpdateP1()
        {
            //Сохранить р1 в объекте управление.
            control.SourceRadix = (int)sourceRadix.Value;
            //Обновить состояние командных кнопок.
            this.UpdateButtons();
            sourceNumber.Text = control.DoCommand(18);
            resultNumber.Text = "0";
        }
        //Изменяет значение основания с.сч. результата.
        private void resultRadix_ValueChanged(object sender, EventArgs e)
        {
            resultRadix.Value = Convert.ToByte(resultRadix.Value);
            this.UpdateP2();
            UpdateButtons();
        }
        //Выполняет необходимые обновления при смене ос. с.сч. р2.
        private void UpdateP2()
        {
            //Копировать основание результата.
            control.ResultRadix = (int)resultRadix.Value;
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
            if ((int)e.KeyChar == 8) i = 17;
            if ((int)e.KeyChar == 13) i = 19;
            if ((i < control.SourceRadix) || (i >= 16)) DoCommand(i);
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
