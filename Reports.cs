using System;
using System.IO;
using Word = Microsoft.Office.Interop.Word;
using Excel = Microsoft.Office.Interop.Excel;
using System.Diagnostics;

namespace EasyTalk
{
    internal class Reports
    {
        public void GenerateLoginReport()
        {
            // Визначення шляху до папки Reports у директорії програми
            string reportsDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Reports");

            // Перевірка наявності папки Reports і створення, якщо вона відсутня
            if (!Directory.Exists(reportsDirectory))
            {
                Directory.CreateDirectory(reportsDirectory);
            }
            string filePath = Path.Combine(reportsDirectory, "LoginReport.docx");
            string currentDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string userData = $"{PersonalData.First_name} {PersonalData.Last_name} ({PersonalData.Login})";
            string content = $"Звіт з авторизації користувачів!\nДата та час входу в систему: {currentDate}\nКористувач: {userData}\n\n";
            SaveToWord(filePath, content);
        }

        public void GenerateRegistrationReport()
        {
            string reportsDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Reports");
            if (!Directory.Exists(reportsDirectory))
            {
                Directory.CreateDirectory(reportsDirectory);
            }
            string filePath = Path.Combine(reportsDirectory, "RegistrationReport.xlsx");
            SaveToExcel(filePath);
        }

        public void GenerateFeedbackReport(string feedbackContent)
        {
            string reportsDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Reports");
            if (!Directory.Exists(reportsDirectory))
            {
                Directory.CreateDirectory(reportsDirectory);
            }
            string filePath = Path.Combine(reportsDirectory, "FeedbackReport.docx");
            string currentDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string userData = $"{PersonalData.First_name} {PersonalData.Last_name} ({PersonalData.Login})";
            string content = $"Звіт з відгуків користувачів!\nДата та час: {currentDate}\nКористувач: {userData}\nВідгук: {feedbackContent}\n\n";
            SaveToWord(filePath, content);
        }

        public void GenerateQuestionReport(string questionContent)
        {
            string reportsDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Reports");
            if (!Directory.Exists(reportsDirectory))
            {
                Directory.CreateDirectory(reportsDirectory);
            }
            string filePath = Path.Combine(reportsDirectory, "QuestionReport.docx");
            string currentDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string userData = $"{PersonalData.First_name} {PersonalData.Last_name} ({PersonalData.Login})";
            string content = $"Звіт з питань користувачів!\nДата та час: {currentDate}\nКористувач: {userData}\nПитання: {questionContent}\n\n";
            SaveToWord(filePath, content);
        }

        public void GenerateUpdateSecurityAdminReport()
        {
            string reportsDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Reports");
            if (!Directory.Exists(reportsDirectory))
            {
                Directory.CreateDirectory(reportsDirectory);
            }
            string filePath = Path.Combine(reportsDirectory, "UpdateSecurityAdminReport.docx");
            string currentDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string userData = $"{PersonalData.First_name} {PersonalData.Last_name} ({PersonalData.Login})";
            string content = $"Звіт про зміну безпеки!\nДата та час: {currentDate}\nКористувач: {userData}\n" +
                             $"Новий логін: {PersonalData.Login}\nНовий пароль: {PersonalData.Password}\n\n";

            SaveToWord(filePath, content);
        }

        public void GenerateUpdatePersonalDataAdminReport()
        {
            string reportsDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Reports");
            if (!Directory.Exists(reportsDirectory))
            {
                Directory.CreateDirectory(reportsDirectory);
            }
            string filePath = Path.Combine(reportsDirectory, "UpdatePersonalDataAdminReport.docx");
            string currentDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string userData = $"{PersonalData.First_name} {PersonalData.Last_name} ({PersonalData.Login})";
            string content = $"Звіт про зміну персональних даних!\nДата та час: {currentDate}\nКористувач: {userData}\n" +
                             $"Нове прізвище: {PersonalData.Last_name}\nНове ім'я: {PersonalData.First_name}\n" +
                             $"По батькові: {PersonalData.Middle_name}\nОпис: {PersonalData.Description}\n\n";

            SaveToWord(filePath, content);
        }

        public void GenerateUpdateSecurityUserReport()
        {
            string reportsDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Reports");
            if (!Directory.Exists(reportsDirectory))
            {
                Directory.CreateDirectory(reportsDirectory);
            }
            string filePath = Path.Combine(reportsDirectory, "UpdateSecurityUserReport.docx");
            string currentDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string userData = $"{PersonalData.First_name} {PersonalData.Last_name} ({PersonalData.Login})";
            string content = $"Звіт про зміну безпеки!\nДата та час: {currentDate}\nКористувач: {userData}\n" +
                             $"Новий логін: {PersonalData.Login}\nНовий пароль: {PersonalData.Password}\n\n";

            SaveToWord(filePath, content);
        }

        public void GenerateUpdatePersonalDataUserReport()
        {
            string reportsDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Reports");
            if (!Directory.Exists(reportsDirectory))
            {
                Directory.CreateDirectory(reportsDirectory);
            }
            string filePath = Path.Combine(reportsDirectory, "UpdatePersonalDataUserReport.docx");
            string currentDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string userData = $"{PersonalData.First_name} {PersonalData.Last_name} ({PersonalData.Login})";
            string content = $"Звіт про зміну персональних даних!\nДата та час: {currentDate}\nКористувач: {userData}\n" +
                             $"Нове прізвище: {PersonalData.Last_name}\nНове ім'я: {PersonalData.First_name}\n" +
                             $"По батькові: {PersonalData.Middle_name}\nE-mail: {PersonalData.Email}\nАдреса: {PersonalData.Address}\n\n";

            SaveToWord(filePath, content);
        }

        public string ReadWordFile(string filePath)
        {
            Word.Application wordApp = new Word.Application();
            Word.Document wordDoc = null;
            string content = string.Empty;

            try
            {
                if (File.Exists(filePath))
                {
                    wordDoc = wordApp.Documents.Open(filePath);
                    content = wordDoc.Content.Text; // Зчитуємо текст із документа
                }
                else
                {
                    throw new FileNotFoundException("Файл не знайдено.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка зчитування Word(.DOCX) файлу: {ex.Message}");
            }
            finally
            {
                wordDoc?.Close(false);
                wordApp.Quit();
            }

            return content;
        }

        public string ReadExcelFile(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("Файл Excel не знайдено.");
            }

            Excel.Application excelApp = null;
            Excel.Workbook workbook = null;
            string result = string.Empty;

            try
            {
                excelApp = new Excel.Application();
                workbook = excelApp.Workbooks.Open(filePath);
                Excel.Worksheet sheet = workbook.Sheets[1];

                // Читаємо всі дані в зручному форматі
                Excel.Range usedRange = sheet.UsedRange;
                for (int row = 1; row <= usedRange.Rows.Count; row++)
                {
                    for (int col = 1; col <= usedRange.Columns.Count; col++)
                    {
                        object cellValue = usedRange.Cells[row, col].Value2;
                        result += (cellValue?.ToString() ?? string.Empty) + "\t"; // Розділяємо табуляцією
                    }
                    result += Environment.NewLine; // Переносимо рядок
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка читання Excel: {ex.Message}");
                throw;
            }
            finally
            {
                workbook?.Close(false);
                excelApp?.Quit();
            }

            return result;
        }

        public void OpenFile(string filePath)
        {
            try
            {
                if (File.Exists(filePath))
                {
                    Process.Start(new ProcessStartInfo(filePath) { UseShellExecute = true });
                }
                else
                {
                    throw new FileNotFoundException("Файл не знайдено.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка відкриття файлу: {ex.Message}");
                throw;
            }
        }

        private void SaveToWord(string filePath, string content)
        {
            Word.Application wordApp = new Word.Application();
            Word.Document wordDoc = null;

            try
            {
                string directoryPath = Path.GetDirectoryName(filePath);
                if (!Directory.Exists(directoryPath))
                {
                    // Якщо папка не існує, створюємо її
                    Directory.CreateDirectory(directoryPath);
                }

                // Перевірка на існування файлу
                if (File.Exists(filePath))
                {
                    wordDoc = wordApp.Documents.Open(filePath);
                }
                else
                {
                    wordDoc = wordApp.Documents.Add();
                }

                // Додаємо новий параграф з переданим вмістом
                Word.Paragraph paragraph = wordDoc.Content.Paragraphs.Add();
                paragraph.Range.Text = content;

                wordDoc.SaveAs(filePath);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка збереження Word(.DOCX) звіту: {ex.Message}");
            }
            finally
            {
                wordDoc?.Close(false);
                wordApp.Quit();
            }
        }

        private void SaveToExcel(string filePath)
        {
            Excel.Application excelApp = new Excel.Application();
            Excel.Workbook workbook = null;
            try
            {
                string directoryPath = Path.GetDirectoryName(filePath);
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                Excel.Worksheet sheet;

                // Вимкнення попереджень Excel
                excelApp.DisplayAlerts = false;

                if (File.Exists(filePath))
                {
                    workbook = excelApp.Workbooks.Open(filePath);
                    sheet = workbook.Sheets[1];
                }
                else
                {
                    workbook = excelApp.Workbooks.Add();
                    sheet = workbook.Sheets[1];
                    sheet.Name = "Звіт з реєстрації користувачів";

                    sheet.Cells[1, 1] = "Дата реєстрації";
                    sheet.Cells[1, 2] = "Прізвище";
                    sheet.Cells[1, 3] = "Ім'я";
                    sheet.Cells[1, 4] = "Логін";

                    // Стилізація заголовків
                    Excel.Range headerRange = sheet.Range[sheet.Cells[1, 1], sheet.Cells[1, 4]];
                    headerRange.Font.Bold = true;
                    headerRange.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
                    headerRange.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
                    headerRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;

                    sheet.Rows[1].AutoFit();
                }

                // Знаходимо останній рядок для додавання нових даних
                int lastRow = sheet.Cells[sheet.Rows.Count, 1].End(Excel.XlDirection.xlUp).Row + 1;

                sheet.Cells[lastRow, 1] = DateTime.Now.ToString("yyyy-MM-dd");
                sheet.Cells[lastRow, 2] = PersonalData.Last_name;
                sheet.Cells[lastRow, 3] = PersonalData.First_name;
                sheet.Cells[lastRow, 4] = PersonalData.Login;

                Excel.Range dataRange = sheet.Range[sheet.Cells[lastRow, 1], sheet.Cells[lastRow, 4]];
                dataRange.Font.Name = "Calibri";
                dataRange.Font.Size = 12;
                dataRange.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;

                sheet.Columns[1].AutoFit();
                sheet.Columns[2].AutoFit();
                sheet.Columns[3].AutoFit();
                sheet.Columns[4].AutoFit();

                workbook.SaveAs(filePath);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка збереження Excel(.XLSX) звіту: {ex.Message}");
            }
            finally
            {
                // Вимкнення попереджень Excel
                excelApp.DisplayAlerts = true;

                workbook?.Close(false);
                excelApp.Quit();
            }
        }
    }
}