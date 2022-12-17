using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace CreateScheduleForAssembly
{
    internal class ElementsMethods
    {
        public static List<ElementId> GetAllSubelements(Document doc, Element elem)
        {
            List<ElementId> allElements = new List<ElementId>();

            if (!(elem is FamilyInstance familyInstance)) return null;
            List<ElementId> subElememts = familyInstance.GetSubComponentIds().ToList();
            if (!subElememts.Any()) return allElements;
            foreach (ElementId item in subElememts)
            {
                allElements.Add(item);
                List<ElementId> list = GetAllSubelements(doc, doc.GetElement(item));
                allElements.AddRange(list);
            }

            return allElements;

        }
        /// <summary>
        /// Метод для сортировки элементов по списку параметров
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="elemId"></param>
        /// <param name="guid">Список уникальных идентификаторов параметров для сортировки элементов.
        /// Порядок подачи параметров определяет порядок сортировки.</param>
        public static List<ElementId> SortElemFromAssembly(Document doc, List<ElementId> elemId, List<Parameter> param)
        {
            List < ElementId > elemIdResult = new List<ElementId>();



            return elemIdResult;
        }
    }
}
