using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;

namespace LossTemp
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

            List<List<ElementId>> lst = Iterator.GetCircuit(con1, con2);



            return Result.Succeeded;

        }
    }
}
