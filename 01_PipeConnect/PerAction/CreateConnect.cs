using System;
using System.Collections.Generic;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Plumbing;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;



namespace PipeConnect
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class CreateConnect : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Application app = uiapp.Application;
            Document doc = uidoc.Document;
            Selection sel = uiapp.ActiveUIDocument.Selection;

            Connector con1 = PointOriginConnectors.GetConnector(sel, doc);
            if (con1 == null)
            {
                TaskDialog.Show("Ошибка", "Первый объект не найден");
                return Result.Failed;
            }
            Connector con2 = PointOriginConnectors.GetConnector(sel, doc);
            if (con2 == null)
            {
                TaskDialog.Show("Ошибка", "Второй объект не найден");
                return Result.Failed;
            }
            ICollection<ElementId> elementIds = ElementConnectorIterator.IteratorElements(con1.Owner, 3);

            using (TransactionGroup transGroup = new TransactionGroup(doc))
            {
                transGroup.Start("ToConnect");

                using (var tx = new Transaction(doc))
                {
                    tx.Start("MoveElement");
                    XYZ newPoint = con2.Origin - con1.Origin;
                    ElementTransformUtils.MoveElements(doc, elementIds, newPoint);
                    tx.Commit();
                }

                Line line = PlanePointCalculator.GetNormalAxis(con1, con2);

                using (var tx = new Transaction(doc))
                {
                    tx.Start("RotateElement");
                    if (line != null)
                    {
                        XYZ vec1 = con1.CoordinateSystem.BasisZ;
                        XYZ vec2 = con2.CoordinateSystem.BasisZ;
                        double angle2 = vec1.AngleTo(vec2) + Math.PI;
                        ElementTransformUtils.RotateElements(doc, elementIds, line, angle2);
                    }

                    ConnectElement.ConnectTo(con1, con2);
                    tx.Commit();
                }

                transGroup.Assimilate();
                return Result.Succeeded;
            }
        }
    }
}

