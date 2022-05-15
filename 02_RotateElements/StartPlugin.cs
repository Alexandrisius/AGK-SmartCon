using System;
using System.Collections.Generic;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;

namespace RotateElements
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]

    public class StartPlugin : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Application app = uiapp.Application;
            Document doc = uidoc.Document;
            Selection sel = uiapp.ActiveUIDocument.Selection;

            Filter filter = new Filter();
            FilterWithPipe filterWithPipe = new FilterWithPipe();

            Element elem1 = null;
            Element elem2 = null;
            XYZ point = null;
            try
            {
                elem1 = doc.GetElement(sel.PickObject(ObjectType.Element, filter, "Выберите элемент который необходимо повернуть"));
                if (elem1 == null)
                {
                    TaskDialog.Show("Ошибка", "Объект для вращения не найден");
                    return Result.Failed;
                }
            }
            catch (Exception)
            {
                TaskDialog.Show("Внимание", "Объект для вращения не найден");
                return Result.Failed;
            }
            if (elem1 != null)
            {
                try
                {
                    Reference select_2 = sel.PickObject(ObjectType.Element, filterWithPipe, "Выберите элемент который будет являться осью вращения");
                    elem2 = doc.GetElement(select_2);
                    point = select_2.GlobalPoint;
                    if (elem2 == null)
                    {
                        TaskDialog.Show("Ошибка", "Ось вращения не найдена");
                        return Result.Failed;
                    }
                }
                catch (Exception)
                {
                    TaskDialog.Show("Внимание", "Ось вращения не найдена");
                    return Result.Failed;
                }

            }
            if (elem1 != null && elem2 != null)
            {
                using (TransactionGroup transGroup = new TransactionGroup(doc))
                {
                    transGroup.Start("RotateElements");

                    UserRotateElementsControl wpf = new UserRotateElementsControl(doc, elem1, elem2, point);

                    wpf.ShowDialog();

                    transGroup.Assimilate();

                    return Result.Succeeded;
                }
            }
            return Result.Failed;
        }
    }
}

