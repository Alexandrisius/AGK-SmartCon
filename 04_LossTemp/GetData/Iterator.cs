using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;

namespace LossTemp
{
    class Iterator
    {
        public static ICollection<ICollection<ElementId>> GetCircuit(Document doc, Connector con1, Connector con2, List<Element> listRadiators)
        {
            ICollection<ElementId> listElements = new List<ElementId>();

            ConnectorSet connectorSet = GetConnectorSet(con1.Owner);

            if (connectorSet != null)
            {
                ConnectorSetIterator csi = connectorSet.ForwardIterator();

                while (csi.MoveNext())
                {
                    if (csi.Current is Connector connector)
                    {
                        


                    }
                }
            }

            return null;
        }

        public static ConnectorSet GetConnectorSet(Element element)
        {
            if (element is FamilyInstance family)
            {
                ConnectorSet connectorSet = family.MEPModel.ConnectorManager.Connectors;
                return connectorSet;
            }

            if (element is MEPCurve curve)
            {
                ConnectorSet connectorSet = curve.ConnectorManager.Connectors;
                return connectorSet;
            }

            return null;
        }
    }
}
