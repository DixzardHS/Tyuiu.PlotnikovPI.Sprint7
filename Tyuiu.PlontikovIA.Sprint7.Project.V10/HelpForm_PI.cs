using System.Drawing;
using System.Windows.Forms;

namespace Tyuiu.PlontikovPI.Sprint7.Project.V10
{
    public class HelpForm_PI : Form
    {
        public HelpForm_PI()
        {
            Text = "Руководство пользователя — Tyuiu.PlontikovPI.Sprint7";
            Size = new Size(720, 520);
            StartPosition = FormStartPosition.CenterParent;
            MinimumSize = new Size(500, 350);

            var richTextBoxHelp_PI = new RichTextBox
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                BorderStyle = BorderStyle.FixedSingle,
                Text =
                    "КРАТКОЕ РУКОВОДСТВО ПОЛЬЗОВАТЕЛЯ\r\n\r\n" +
                    "1. Открытие данных\r\n" +
                    "   - Меню «Файл» → «Открыть CSV...» или кнопка «Открыть» на панели инструментов.\r\n" +
                    "   - Выберите CSV-файл с заказами.\r\n\r\n" +
                    "2. Редактирование\r\n" +
                    "   - Данные редактируются непосредственно в таблице.\r\n" +
                    "   - Для добавления заказа нажмите «Добавить».\r\n" +
                    "   - Для удаления выбранного заказа нажмите «Удалить».\r\n\r\n" +
                    "3. Поиск и фильтрация\r\n" +
                    "   - Введите текст в поле «Поиск».\r\n" +
                    "   - Выберите поле поиска в списке «Поле».\r\n" +
                    "   - Укажите минимальную стоимость заказа.\r\n" +
                    "   - Нажмите «Фильтр» или «Сброс».\r\n\r\n" +
                    "4. Сортировка\r\n" +
                    "   - Щёлкните по заголовку столбца таблицы.\r\n\r\n" +
                    "5. Статистика и график\r\n" +
                    "   - Статистика отображается справа по видимым строкам.\r\n" +
                    "   - Гистограмма показывает сумму стоимости заказов по товарам.\r\n\r\n" +
                    "6. Сохранение\r\n" +
                    "   - «Файл» → «Сохранить» или «Сохранить как...».\r\n" +
                    "   - Данные сохраняются в CSV.\r\n\r\n" +
                    "7. Справка\r\n" +
                    "   - «Справка» → «О программе» / «Руководство пользователя»."
            };

            Controls.Add(richTextBoxHelp_PI);
        }
    }
}