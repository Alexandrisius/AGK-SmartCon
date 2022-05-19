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
        public static List<List<ElementId>> GetCircuit(Connector con1, Connector con2)
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
                        if (!listElements.Contains(connector.Owner.Id))
                        {
                            listElements.Add(connector.Owner.Id);
                        }
                        ConnectorSet conSet = connector.AllRefs;

                        foreach (Connector elemCon in conSet)
                        {
                            if (!listElements.Contains(elemCon.Owner.Id) && GetConnectorSet(elemCon.Owner) != null
                                                                         && elemCon.Owner.Id != con2.Owner.Id)
                            {
                                csi = GetConnectorSet(elemCon.Owner).ForwardIterator();
                            }
                        }
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
