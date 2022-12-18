using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace CreateScheduleForAssembly
{
     class SortedByParams : IComparer<ElementId>
    {
        private readonly List<string> _param;
        private readonly Document _doc;
        public SortedByParams(Document doc,List<string> param)
        {
            _param = param;
            _doc = doc;
        }
        public int Compare(ElementId elemId1, ElementId elemId2)
        {
            Element elem1 = _doc.GetElement(elemId1);
            Element elem2 = _doc.GetElement(elemId2);
            

            foreach (string parItem in _param)
            {
                
                if (Guid.TryParse(parItem, out Guid guId))
                {
                    if (elemId1 == null) continue;
                    string str1 = elem1.get_Parameter(guId).AsString();
                    if (str1 == null)
                    {
                        str1 = _doc.GetElement(elem1.GetTypeId()).get_Parameter(guId).AsString();
                    }
                    if (elemId2 == null) continue;
                    string str2 = elem2.get_Parameter(guId).AsString();
                    if (str2 == null)
                    {
                        str2 = _doc.GetElement(elem2.GetTypeId()).get_Parameter(guId).AsString();
                    }

                    int x = String.CompareOrdinal(str2, str1);
                    if (x!= 0)
                    {
                        return x;
                    }
                }
                else if(int.Parse(parItem)<0)
                {
                    if (elemId1 == null) continue;
                    string str1 = elem1.get_Parameter((BuiltInParameter)int.Parse(parItem)).AsString();
                    if (!str1.Any())
                    {
                        str1 = _doc.GetElement(elem1.GetTypeId()).get_Parameter((BuiltInParameter)int.Parse(parItem)).AsString();
                    }
                    if (elemId2 == null) continue;
                    string str2 = elem2.get_Parameter((BuiltInParameter)int.Parse(parItem)).AsString();
                    if (!str2.Any())
                    {
                        str2 = _doc.GetElement(elem2.GetTypeId()).get_Parameter((BuiltInParameter)int.Parse(parItem)).AsString();
                    }

                    int x = String.Compare(str2, str1);
                    if (x != 0)
                    {
                        return x;
                    }
                }
            }


            return 0;
        }
    }
}
