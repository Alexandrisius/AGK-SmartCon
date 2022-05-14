using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Plumbing;
using System.Collections.Generic;
using System.Linq;

namespace PipeConnect
{
    public class SelectionUnusedConnectors
    {
        public static bool GetUnusedConnectors(Element elem)
        {
            if (elem is FamilyInstance family)
            {
                ConnectorManager list = family.MEPModel.ConnectorManager;
                if (list.UnusedConnectors.Size > 0 )
                {
                    foreach (Connector item in list.Connectors)
                    {
                        if (item.Domain == Domain.DomainPiping)
                        {
                            return true;
                        }
                        
                    }
                    
                }
            }

            if (elem is Pipe curve)
            {
                ConnectorManager list = curve.ConnectorManager;
                if (list.UnusedConnectors.Size > 0)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
