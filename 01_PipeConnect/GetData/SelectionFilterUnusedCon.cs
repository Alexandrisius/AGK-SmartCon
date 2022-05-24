using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;

namespace PipeConnect
{
    public class SelectionFilterUnusedCon : ISelectionFilter
    {
        private readonly Connector con;

        public SelectionFilterUnusedCon(Connector _con)
        {
            con = _con;
        }

        public bool AllowElement(Element elem)
        {
            if (SelectionUnusedConnectors.GetUnusedConnectors(elem) & con == null)
            {
                return true;
            }

            if (SelectionUnusedConnectors.GetUnusedConnectors(elem) & con.Owner.Id != elem.Id)
            {
                return true;
            }
            return false;
        }

        public bool AllowReference(Reference reference, XYZ position)
        {
            return false;
        }
    }

}
