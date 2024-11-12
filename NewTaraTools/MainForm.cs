/*
 * Created by SharpDevelop.
 * User: user
 * Date: 12.11.2024
 * Time: 11:00
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace NewTaraTools
{
    /// <summary>
    /// Description of MainForm.
    /// </summary>
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }
        void UnpackClick(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Tara Files (*.tara)|*.tara|All Files (*.*)|*.*";
                openFileDialog.Multiselect = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    foreach (string filePath in openFileDialog.FileNames)
                    {
                        try
                        {
                            // Определяем папку для распаковки
                            string outputDirectory = Path.Combine(Path.GetDirectoryName(filePath) ?? string.Empty, "Unpacked");

                            // Вызываем метод Unpack для каждого файла
                            Unpacker.Unpack(filePath, outputDirectory);

                            
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Error unpacking file '{Path.GetFileName(filePath)}': {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            break;
                        }
                    }
                    MessageBox.Show($"All TARA files unpacked to '{Path.GetDirectoryName(openFileDialog.FileName) ?? string.Empty}'", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        void PackClick(object sender, EventArgs e)
        {
            List<string> selectedDirs = new List<string>();

            using (FolderBrowserDialog folderDialog = new FolderBrowserDialog())
            {
                folderDialog.Description = "Select Directories for Creating Tara Files";
                folderDialog.ShowNewFolderButton = false;
                folderDialog.Multiselect = true;

                if (folderDialog.ShowDialog() == DialogResult.OK)
                {
                    string createdtara = "";
                    foreach (var selectedDir in folderDialog.SelectedPaths)
                    {
                        try
                        {
                            // Формируем путь к .tara файлу рядом с выбранной директорией
                            string outputFilePath = Path.Combine(Directory.GetParent(selectedDir).FullName, $"{new DirectoryInfo(selectedDir).Name}.tara");

                            // Создаем экземпляр TaraMaker и вызываем метод создания архива
                            TaraMaker taraMaker = new TaraMaker();
                            taraMaker.CreateTaraFile(selectedDir, outputFilePath);

                            //createdtara += $"Tara file created at '{outputFilePath}'\n";
                            
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Error creating Tara file for directory '{selectedDir}': {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            break;
                        }
                    }

                    MessageBox.Show($"All TARA files created", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }
            }


        }
    }
}
