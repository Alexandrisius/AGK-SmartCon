#region Namespaces
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Reflection;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Windows;
using UIFramework;
using RibbonItem = Autodesk.Revit.UI.RibbonItem;
#endregion

namespace SmartCon
{
    class App : IExternalApplication
    {
        const string _tab = "SmartCon";

        private readonly string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        public Result OnStartup(UIControlledApplication a)
        {
            Autodesk.Revit.UI.RibbonPanel ribbonPanel_PS = PipeSystemRibbonPanel(a);
            Autodesk.Revit.UI.RibbonPanel ribbonPanel_FM = FamlyManRibbonPanel(a);


            #region CreateButtonPipeConnect
            Image LargeImageConnect = Properties.Resources.PipeCon_32х32;
            ImageSource largeImageConnectSaSource = GetImageSource(LargeImageConnect);
            Image imageConnect = Properties.Resources.PipeCon_16х16;
            ImageSource imageConnectSource = GetImageSource(imageConnect);

            PushButtonData PBD_1 = new PushButtonData("Pipe Connect", "Pipe Connect", this.path + "\\PipeConnect.dll", "PipeConnect.StartPlugin")
            {
                ToolTip = "Присоединение трубопроводных элементов.",
                LongDescription = "Присоединяет коннекторы компонентов трубопроводных систем. Базовым элементом является компонент который пользователь выбрал в первую очередь. ",
                Image = imageConnectSource,
                LargeImage = largeImageConnectSaSource
            };

            PushButton button_Connect_1 = ribbonPanel_PS.AddItem(PBD_1) as PushButton;

            button_Connect_1.Enabled = true;

            RibbonToolTip button_Connect_1_ToolTip = new RibbonToolTip()
            {
                Title = "Присоединение трубопроводных элементов.",
                Content = "Присоединяет коннекторы компонентов трубопроводных систем. Базовым элементом является компонент который пользователь выбрал в первую очередь.",
                ExpandedContent = "Обучающий ролик.",
                ExpandedVideo = new Uri("P:/#Resources/02_Software/01_AutodeskRevit/2021/07_Plugins/PassatProjectPlugin/AGK_SmartCon/00_Code/AGK_SmartCon/00_SmartCon_RibonPanel/Resources/PipeCon_Movie.wmv"),
            };
            SetRibbonItemToolTip(button_Connect_1, button_Connect_1_ToolTip);

            #endregion

            #region CreateButtonRotateElements
            Image LargeImageConnect_RE = Properties.Resources.RotatEl_32x32;
            ImageSource largeImageConnectSaSource_RE = GetImageSource(LargeImageConnect_RE);
            Image imageConnect_RE = Properties.Resources.RotatEl_16x16;
            ImageSource imageConnectSource_RE = GetImageSource(imageConnect_RE);

            PushButtonData PBD_RE = new PushButtonData("Rotate Elements", "Rotate Elements", this.path + "\\RotateElements.dll", "RotateElements.StartPlugin")
            {
                ToolTip = "Вращает элементы вокруг оси выбранного коннектора.",
                LongDescription = "Вращает элементы вокруг оси выбранного коннектора со всеми присоединёнными к нему элементами. ",
                Image = imageConnectSource_RE,
                LargeImage = largeImageConnectSaSource_RE
            };

            PushButton button_Connect_RE = ribbonPanel_PS.AddItem(PBD_RE) as PushButton;
            button_Connect_RE.Enabled = true;
            #endregion

            #region CreateButtonAlignElements
            Image LargeImageConnect_AE = Properties.Resources.AlignEl_32x32;
            ImageSource largeImageConnectSaSource_AE = GetImageSource(LargeImageConnect_AE);
            Image imageConnect_AE = Properties.Resources.AlignEl_16x16;
            ImageSource imageConnectSource_AE = GetImageSource(imageConnect_AE);

            PushButtonData PBD_AE = new PushButtonData("Align Elements", "Align Elements", this.path + "\\AlignElements.dll", "AlignElements.StartPlugin")
            {
                ToolTip = "Выравнивает элементы по оси выбранного коннектора.",
                LongDescription = "Выравнивает элементы по оси выбранного коннектора со всеми присоединёнными к нему элементами. ",
                Image = imageConnectSource_AE,
                LargeImage = largeImageConnectSaSource_AE
            };

            PushButton button_Connect_AE = ribbonPanel_PS.AddItem(PBD_AE) as PushButton;
            button_Connect_AE.Enabled = true;
            #endregion

            #region CreateButtonFamilyMan
            Image LargeImageConnect_2 = Properties.Resources.FamilyMan_32х32;
            ImageSource largeImageConnectSaSource_2 = GetImageSource(LargeImageConnect_2);
            Image imageConnect_2 = Properties.Resources.FamilyMan_16х16;
            ImageSource imageConnectSource_2 = GetImageSource(imageConnect_2);

            PushButtonData PBD_2 = new PushButtonData("Family Manager", "Family Manager", this.path + "\\FamilyManager.dll", "FamilyManager.StartPlugin")
            {
                ToolTip = "Менеджер семейств.",
                LongDescription = "Система менеджмента элементов модели. ",
                Image = imageConnectSource_2,
                LargeImage = largeImageConnectSaSource_2
            };

            PushButton button_Connect_2 = ribbonPanel_FM.AddItem(PBD_2) as PushButton;
            button_Connect_2.Enabled = true;
            #endregion

            return Result.Succeeded;
        }

        private BitmapSource GetImageSource(Image img)
        {
            BitmapImage largeImage = new BitmapImage();
            using (MemoryStream ms = new MemoryStream())
            {
                img.Save(ms, ImageFormat.Png);
                ms.Position = 0;
                largeImage.BeginInit();
                largeImage.CacheOption = BitmapCacheOption.OnLoad;
                largeImage.UriSource = null;
                largeImage.StreamSource = ms;
                largeImage.EndInit();
            }
            return largeImage;
        }

        public Autodesk.Revit.UI.RibbonPanel PipeSystemRibbonPanel(UIControlledApplication a)
        {
            Autodesk.Revit.UI.RibbonPanel ribbonPanel_PS = null;
            try
            {
                a.CreateRibbonTab(_tab);
            }
            catch { }
            try
            {
                Autodesk.Revit.UI.RibbonPanel panel = a.CreateRibbonPanel(_tab, "Pipe System");
            }
            catch { }
            List<Autodesk.Revit.UI.RibbonPanel> panels = a.GetRibbonPanels(_tab);
            foreach (var p in panels)
            {
                if (p.Name == "Pipe System")
                {
                    ribbonPanel_PS = p;
                }

            }
            return ribbonPanel_PS;
        }

        public Autodesk.Revit.UI.RibbonPanel FamlyManRibbonPanel(UIControlledApplication a)
        {
            Autodesk.Revit.UI.RibbonPanel ribbonPanel_FM = null;

            try
            {
                Autodesk.Revit.UI.RibbonPanel panel = a.CreateRibbonPanel(_tab, "Family Manager");
            }
            catch { }
            List<Autodesk.Revit.UI.RibbonPanel> panels = a.GetRibbonPanels(_tab);
            foreach (var p in panels)
            {
                if (p.Name == "Family Manager")
                {
                    ribbonPanel_FM = p;
                }
            }
            return ribbonPanel_FM;
        }

        public BitmapImage GetBitmapImage(string imagePath)
        {
            if (File.Exists(imagePath))
                return new BitmapImage(new Uri(imagePath));
            else
                return null;
        }
        

        void SetRibbonItemToolTip(RibbonItem item, RibbonToolTip toolTip)
        {
            var ribbonItem = GetRibbonItem(item);
            if (ribbonItem == null)
                return;
            ribbonItem.ToolTip = toolTip;
        }


        public Autodesk.Windows.RibbonItem GetRibbonItem(RibbonItem item)
        {
            RibbonControl ribbonControl = RevitRibbonControl.RibbonControl;

            foreach (var tab in ribbonControl.Tabs)
            {
                foreach (var panel in tab.Panels)
                {
                    foreach (var ribbonItem in panel.Source.Items)
                    {
                        if (ribbonItem.AutomationName == item.Name)
                            return ribbonItem as Autodesk.Windows.RibbonItem;
                    }
                }
            }

            return null;
        }


        public Result OnShutdown(UIControlledApplication a)
        {
            return Result.Succeeded;
        }
    }
}
