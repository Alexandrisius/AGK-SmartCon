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

namespace CreateScheduleForAssembly
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

            View viewTemplate = (from v in new FilteredElementCollector(doc)
                                 .OfClass(typeof(View))
                                 .Cast<View>()where v.IsTemplate == true && v.Name == "Template"
                                 select v)
                                 .First();

            using (var tx = new Transaction(doc))
            {
                tx.Start("CreateSchedule");

                var vs = ViewSchedule.CreateSchedule(doc, new ElementId(BuiltInCategory.INVALID));

                vs.Name = "Спецификация_1";

                doc.Regenerate();

                vs.ApplyViewTemplateParameters(viewTemplate);

                var sFieldId = SchedulesMethods.FindField(vs, "3de5f1a4-d560-4fa8-a74f-25d250fb3401").FieldId;

                ScheduleFilter filter = new ScheduleFilter(sFieldId, ScheduleFilterType.Equal, "String");

                vs.Definition.AddFilter(filter);


                tx.Commit();
            }

            return Result.Succeeded;

        }
    }
}
