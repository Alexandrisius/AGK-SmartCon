﻿#region Namespaces
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

            #region CreateButtonPipeConnect
            Image LargeImageConnect = Properties.Resources.PipeCon_32х32;
            ImageSource largeImageConnectSaSource = GetImageSource(LargeImageConnect);
            Image imageConnect = Properties.Resources.PipeCon_16х16;
            ImageSource imageConnectSource = GetImageSource(imageConnect);

            PushButtonData PBD_1 = new PushButtonData("Pipe Connect", "Pipe\nConnect", this.path + "\\PipeConnect.dll", "PipeConnect.StartPlugin")
            {
                ToolTip = "Присоединение трубопроводных элементов.",
                LongDescription = "Присоединяет коннекторы компонентов трубопроводных систем. Базовым элементом является компонент который пользователь выбрал в первую очередь. ",
                Image = imageConnectSource,
                LargeImage = largeImageConnectSaSource
            };

            PushButton button_Connect_1 = ribbonPanel_PS.AddItem(PBD_1) as PushButton;

            button_Connect_1.Enabled = true;

            #endregion

            #region CreateButtonRotateElements
            Image LargeImageConnect_RE = Properties.Resources.RotatEl_32x32;
            ImageSource largeImageConnectSaSource_RE = GetImageSource(LargeImageConnect_RE);
            Image imageConnect_RE = Properties.Resources.RotatEl_16x16;
            ImageSource imageConnectSource_RE = GetImageSource(imageConnect_RE);

            PushButtonData PBD_RE = new PushButtonData("Rotate Elements", "Rotate\nElements", this.path + "\\RotateElements.dll", "RotateElements.StartPlugin")
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

            PushButtonData PBD_AE = new PushButtonData("Align Elements", "Align\nElements", this.path + "\\AlignElements.dll", "AlignElements.StartPlugin")
            {
                ToolTip = "Выравнивает элементы по оси выбранного коннектора.",
                LongDescription = "Выравнивает элементы по оси выбранного коннектора со всеми присоединёнными к нему элементами. ",
                Image = imageConnectSource_AE,
                LargeImage = largeImageConnectSaSource_AE
            };

            PushButton button_Connect_AE = ribbonPanel_PS.AddItem(PBD_AE) as PushButton;
            button_Connect_AE.Enabled = true;
            #endregion

            //#region CreateButtonLossTemp
            //Image LargeImageConnect_LT = Properties.Resources.LossTemp_32x32;
            //ImageSource largeImageConnectSaSource_LT = GetImageSource(LargeImageConnect_LT);
            //Image imageConnect_LT = Properties.Resources.LossTemp_16x16;
            //ImageSource imageConnectSource_LT = GetImageSource(imageConnect_LT);

            //PushButtonData PBD_LT = new PushButtonData("Loss Temperature", "Loss\nTemperature", this.path + "\\LossTemp.dll", "LossTemp.StartPlugin")
            //{
            //    ToolTip = "Вычисляет потерю температуры теплоносителя.",
            //    LongDescription = "Вычисляет потерю температуры теплоносителя по длине трубы. ",
            //    Image = imageConnectSource_LT,
            //    LargeImage = largeImageConnectSaSource_LT
            //};

            //PushButton button_Connect_LT = ribbonPanel_PS.AddItem(PBD_LT) as PushButton;
            //button_Connect_LT.Enabled = false;
            //#endregion

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
