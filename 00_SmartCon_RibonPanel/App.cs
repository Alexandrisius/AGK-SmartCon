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
#endregion

namespace SmartCon
{
    class App : IExternalApplication
    {
        const string tab = "SmartCon";

        private readonly string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        public Result OnStartup(UIControlledApplication a)
        {
            RibbonPanel ribbonPanel_PS = PipeSystemRibbonPanel(a);
            RibbonPanel ribbonPanel_FM = FamlyManRibbonPanel(a);


            #region CreateButtonPipeConnect
            Image LargeImageConnect = Properties.Resources.PipeCon_32х32;
            ImageSource largeImageConnectSaSource = GetImageSource(LargeImageConnect);
            Image imageConnect = Properties.Resources.PipeCon_16х16;
            ImageSource imageConnectSource = GetImageSource(imageConnect);

            PushButtonData PBD_1 = new PushButtonData("Pipe\nConnect", "Pipe Connect", this.path + "\\PipeConnect.dll", "ToConnect.StartPlugin")
            {
                ToolTip = "Присоединение трубопроводных элементов.",
                LongDescription = "Присоединяет коннекторы компонентов трубопроводных систем. Базовым элементом является компонент который пользователь выбрал в первую очередь. ",
                Image = imageConnectSource,
                LargeImage = largeImageConnectSaSource
            };

            PushButton button_Connect_1 = ribbonPanel_PS.AddItem(PBD_1) as PushButton;
            button_Connect_1.Enabled = true;
            #endregion

            //ribbonPanel.AddSeparator();

            #region CreateButtonRotateElements
            Image LargeImageConnect_RE = Properties.Resources.RotatEl_32x32;
            ImageSource largeImageConnectSaSource_RE = GetImageSource(LargeImageConnect_RE);
            Image imageConnect_RE = Properties.Resources.RotatEl_16x16;
            ImageSource imageConnectSource_RE = GetImageSource(imageConnect_RE);

            PushButtonData PBD_RE = new PushButtonData("RotatEL", "Rotate Elements", this.path + "\\RotatEL.dll", "RotatEL.StartClass")
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

            PushButtonData PBD_AE = new PushButtonData("AlignEl", "Align Elements", this.path + "\\AlignEl.dll", "AlignEl.StartClass")
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

            PushButtonData PBD_2 = new PushButtonData("FamilyMan", "Family Manager", this.path + "\\FamilyMan.dll", "FamilyMan.StartClass")
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

        public RibbonPanel PipeSystemRibbonPanel(UIControlledApplication a)
        {
            RibbonPanel ribbonPanel_PS = null;
            try
            {
                a.CreateRibbonTab(tab);
            }
            catch { }
            try
            {
                RibbonPanel panel = a.CreateRibbonPanel(tab, "Pipe System");
            }
            catch { }
            List<RibbonPanel> panels = a.GetRibbonPanels(tab);
            foreach (var p in panels)
            {
                if (p.Name == "Pipe System")
                {
                    ribbonPanel_PS = p;
                }

            }
            return ribbonPanel_PS;
        }

        public RibbonPanel FamlyManRibbonPanel(UIControlledApplication a)
        {
            RibbonPanel ribbonPanel_FM = null;

            try
            {
                RibbonPanel panel = a.CreateRibbonPanel(tab, "Family Manager");
            }
            catch { }
            List<RibbonPanel> panels = a.GetRibbonPanels(tab);
            foreach (var p in panels)
            {
                if (p.Name == "Family Manager")
                {
                    ribbonPanel_FM = p;
                }
            }
            return ribbonPanel_FM;
        }

        public Result OnShutdown(UIControlledApplication a)
        {
            return Result.Succeeded;
        }
    }
}
