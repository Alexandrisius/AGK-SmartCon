using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.Creation;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using Document = Autodesk.Revit.DB.Document;

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
            View viewTemplate = null;

            try
            {
                viewTemplate = (new FilteredElementCollector(doc).OfClass(typeof(View))
                    .Cast<View>()
                    .Where(v => v.IsTemplate == true && v.Name == "Экспликация_ТХ")).First();

            }
            catch (Exception)
            {
                TaskDialog.Show("Внимание", "Отсутствует шаблон вида 'Экспликация_ТХ' для спецификаций с " +
                    "заданными полями. Галочка закрепления фильтров должна быть снята", TaskDialogCommonButtons.Ok);
                return Result.Failed;
            }


            IEnumerable<AssemblyInstance> assemlby = (new FilteredElementCollector(doc).OfClass(typeof(AssemblyInstance))
                .Cast<AssemblyInstance>()
                .Where(v => v.Name.Contains("Трубопровод")));

            List<string> allScheduleName = (new FilteredElementCollector(doc).OfClass(typeof(ViewSchedule)).Cast<ViewSchedule>())
                .Select(x => x.Name).ToList();


            using (Transaction tx = new Transaction(doc))
            {
                tx.Start("CreateSchedule");

                foreach (AssemblyInstance item in assemlby)
                {
                    if (allScheduleName.Contains(item.Name)) continue;
                    ViewSchedule vs = ViewSchedule.CreateSchedule(doc, new ElementId(BuiltInCategory.INVALID));

                    vs.Name = item.Name;


                    const string ADSK_group_GUID = "3de5f1a4-d560-4fa8-a74f-25d250fb3401";// GUID параметра ADSK_Группирование
                    const string ADSK_position_GUID = "ae8ff999-1f22-4ed7-ad33-61503d85f0f4";// GUID параметра ADSK_Позиция
                    const string ADSK_posScheme_GUID = "95e5eb64-92e1-436b-80d8-f06505defc34";// GUID параметра ADSK_Позиция на схеме
                    const string ADSK_name_GUID = "e6e0f5cd-3e26-485b-9342-23882b20eb43";// GUID параметра ADSK_Наименование
                    const string ADSK_count_GUID = "8d057bb3-6ccd-4655-9165-55526691fe3a";// GUID параметра ADSK_Количество
                    String uniformat = new ElementId(BuiltInParameter.UNIFORMAT_CODE).ToString();

                    doc.Regenerate();

                    if (viewTemplate != null)
                    {
                        vs.ApplyViewTemplateParameters(viewTemplate);
                    }

                    ScheduleFieldId sFieldId = SchedulesMethods.FindField(vs, ADSK_group_GUID).FieldId;
                    

                    ScheduleFilter filter = new ScheduleFilter(sFieldId, ScheduleFilterType.Equal, item.Name);
                   

                    vs.Definition.AddFilter(filter);

                    List<ElementId> listForAdding = new List<ElementId>();

                    foreach (ElementId elemId in item.GetMemberIds())
                    {

                        Element elem = doc.GetElement(elemId);
                        try
                        {
                            elem.get_Parameter(Guid.Parse(ADSK_group_GUID)).Set(vs.Name);
                        }
                        catch (Exception)
                        {

                            continue;
                        }

                        var listsub = ElementsMethods.GetAllSubelements(doc, elem);
                        if (listsub == null) continue;
                        foreach (var subEl in listsub)
                        {
                            try
                            {
                                item.AddMemberIds(new List<ElementId> { subEl });
                            }
                            catch (Exception)
                            {
                                continue;
                            }
                        }


                    }

                    List<string> paramList = new List<string>();
                    paramList.Add(uniformat);
                    paramList.Add(ADSK_name_GUID);

                    //int position = 1;
                    //string name = "";
                    //ICollection<ElementId> listForSort = ElementsMethods.SortElemFromAssembly(doc, item.GetMemberIds(), paramList);
                    //foreach (var sortElem in listForSort)
                    //{
                    //    if (doc.GetElement(sortElem).get_Parameter(Guid.Parse(ADSK_posScheme_GUID)).Set(position.ToString()))
                    //    {
                    //        string name2 = doc.GetElement(sortElem).get_Parameter(Guid.Parse(ADSK_name_GUID)).AsString();
                    //        if (name2 != name)
                    //        {
                    //            position++;
                    //            name = name2;
                    //        }

                    //    }
                    //}

                }
                tx.Commit();
            }

            return Result.Succeeded;

        }
    }
}
