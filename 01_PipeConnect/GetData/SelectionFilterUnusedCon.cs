using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;

namespace PipeConnect
{
    public class SelectionFilterUnusedCon : ISelectionFilter
    {
        public bool AllowElement(Element elem)
        {
            return SelectionUnusedConnectors.GetUnusedConnectors(elem);
        }

        public bool AllowReference(Reference reference, XYZ position)
        {
            return false;
        }
    }

}
