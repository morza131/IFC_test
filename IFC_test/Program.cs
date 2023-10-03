using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Xbim.Common;
using Xbim.Ifc;
using Xbim.Ifc2x3.ProductExtension;

namespace IFC_test
{
    internal class Program
    {
        [STAThreadAttribute]
        static void Main(string[] args)
        {
            //Открытие исходного файла
            string openFilePath = String.Empty;
            string saveFilePath = String.Empty;
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "IFC files (*.ifc)|*.ifc";
                openFileDialog.Title = "Выберите исходный файл тестового задания";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    openFilePath = openFileDialog.FileName;
                    Stream fileStream = openFileDialog.OpenFile();
                }
            }
            using (IfcStore model = IfcStore.Open(openFilePath))
            {                
                using (ITransaction txn = model.BeginTransaction("Transaction"))
                {                    
                    List<IfcBuildingElementProxy> objects = model.Instances
                                                                 .OfType<IfcBuildingElementProxy>()
                                                                 .ToList(); //Все объекты категории "Обобщённые модели"
                    IfcBuildingElementProxy elDrive = model.Instances
                                                           .OfType<IfcBuildingElementProxy>()
                                                           .Where<IfcBuildingElementProxy>(x => x.FriendlyName
                                                           .Contains("ЭП_КП"))
                                                           .FirstOrDefault(); //Нужный объект
                    foreach (IfcBuildingElementProxy element in objects) //Удаление всех объектов "Обобщённые модели", кроме нужного
                    {
                        if (element != elDrive)
                            model.Delete(element);
                    }                    
                    txn.Commit();
                };
                // Сохранение результатов
                Stream myStream;
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "IFC files (*.ifc)|*.ifc";
                saveFileDialog.Title = "Выберите путь сохранения файла";                
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    if ((myStream = saveFileDialog.OpenFile()) != null)
                    {
                        saveFilePath = saveFileDialog.FileName;                        
                        myStream.Close();
                    }
                }
                model.SaveAs(saveFilePath);
            }           
        }
    }
}








