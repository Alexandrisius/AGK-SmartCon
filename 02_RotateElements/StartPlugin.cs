using System;
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
            Document doc = uidoc.Document;
            Selection sel = uiapp.ActiveUIDocument.Selection;

            Filter filter = new Filter();

            Element elemForRotate;
            XYZ clickPoint = null;
            try
            {
                elemForRotate = doc.GetElement(sel.PickObject(ObjectType.Element, filter, "Выберите элемент который необходимо повернуть"));
                if (elemForRotate == null)
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
            if (elemForRotate != null)
            {
                try
                {
                    Reference select_2 = sel.PickObject(ObjectType.PointOnElement,"Выберите ближайшую точку к оси вокруг которой необходимо совершать врщение");
                    clickPoint = select_2.GlobalPoint;
                    
                }
                catch (Exception)
                {
                    TaskDialog.Show("Внимание", "Ось вращения не найдена");
                    return Result.Failed;
                }

            }
            if (elemForRotate != null)
            {
                using (TransactionGroup transGroup = new TransactionGroup(doc))
                {
                    transGroup.Start("RotateElements");

                    UserRotateElementsControl wpf = new UserRotateElementsControl(doc, elemForRotate, clickPoint);

                    wpf.ShowDialog();

                    transGroup.Assimilate();

                    return Result.Succeeded;
                }
            }
            return Result.Failed;
        }
    }
}

