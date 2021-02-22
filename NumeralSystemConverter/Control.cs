using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NumeralSystemConverter.Converter;

namespace NumeralSystemConverter
{
    class Control
    {
        //Основание системы сч. исходного числа. 
        private const int DEFAULT_SOURCE_RADIX = 10;
        //Основание системы сч. результата. 
        private const int DEFUALT_RESULT_RADIX = 16;
        //Число разрядов в дробной части результата. 
        private const int DEFAULT_ERROR = 10;

        private History history = new History();
        private Editor editor = new Editor();


        public Control()
        {
            State = StateType.Редактирование;
            SourceRadix = DEFAULT_SOURCE_RADIX;
            ResultRadix = DEFUALT_RESULT_RADIX;
        }


        public string DoCommand(int commandIndex)
        {
            if (commandIndex == 19)
            {
                double decimalResult = ConverterTo10.Convert(editor.Number, (Int16)SourceRadix);
                string result = ConverterFrom10.Convert(decimalResult, (Int32)ResultRadix, Error);
                State = StateType.Преобразовано;
                history.AddRecord(new Record(editor.Number, result, SourceRadix, ResultRadix));
                return result;
            }
            else
            {
                State = StateType.Редактирование;
                return editor.Edit(commandIndex);
            }

        }


        //Свойство для чтения и записи состояние Конвертера.
        public StateType State { get; set; }
        public int SourceRadix { get; set; }
        public int ResultRadix { get; set; }
        public History History => history;
        public Editor Editor => editor;

        //Точность представления результата.
        private int Error => (int)Math.Round(editor.Error * Math.Log(SourceRadix) / Math.Log(ResultRadix) + 0.5);


        public enum StateType
        {
            Редактирование,
            Преобразовано,
        }
    }
}
