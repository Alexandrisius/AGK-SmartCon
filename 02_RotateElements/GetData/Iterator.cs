using Autodesk.Revit.DB;
using System.Collections.Generic;

namespace RotateElements
{
    class Iterator
    {
        public static ICollection<ElementId> GetElements(Document doc, Element elem_ForRotate, Connector con_Axis)
        {
            ICollection<ElementId> listElements = new List<ElementId>();

            List<ElementId> elemMoreTwoCon = new List<ElementId>();

            List<int> countCon = new List<int>();

            ConnectorSet connectorSet = GetConnectorSet(elem_ForRotate);

            if (connectorSet != null)
            {
                ConnectorSetIterator csi = connectorSet.ForwardIterator();

                while (csi.MoveNext())
                {

                    if (csi.Current is Connector connector)
                    {
                        int countUnused;
                        int count = GetConnectorSet(connector.Owner, out countUnused).Size;

                        if (count > 2 && !elemMoreTwoCon.Contains(connector.Owner.Id))
                        {
                            elemMoreTwoCon.Add(connector.Owner.Id);

                            countCon.Add(count - 2 - countUnused);
                        }

                        if (!listElements.Contains(connector.Owner.Id))
                        {
                            listElements.Add(connector.Owner.Id);
                        }
                        ConnectorSet conSet = connector.AllRefs;

                        foreach (Connector elemCon in conSet)
                        {
                            if (!listElements.Contains(elemCon.Owner.Id) && GetConnectorSet(elemCon.Owner) != null
                                && elemCon.Owner.Id != con_Axis.Owner.Id)
                            {
                                csi = GetConnectorSet(elemCon.Owner).ForwardIterator();
                            }

                        }
                    }
                }
                for (int i = 0; i < elemMoreTwoCon.Count; i++)
                {
                    ConnectorSet conSetGlobal = GetConnectorSet(doc.GetElement(elemMoreTwoCon[i]));

                    foreach (Connector connector in conSetGlobal)
                    {
                        int x = 0;

                        ConnectorSet conSet = connector.AllRefs;
                        ConnectorSetIterator csi_3 = conSet.ForwardIterator();

                        while (csi_3.MoveNext())
                        {
                            if (csi_3.Current is Connector elemCon)
                            {
                                int count;
                                int countUnused;
                                ConnectorSet cs = GetConnectorSet(elemCon.Owner, out countUnused);
                                count = cs.Size;

                                if (count > 2 && !elemMoreTwoCon.Contains(elemCon.Owner.Id))
                                {
                                    elemMoreTwoCon.Add(elemCon.Owner.Id);
                                    countCon.Add(count - 2 - countUnused);

                                }

                                ConnectorSet conSet_2 = elemCon.AllRefs;

                                foreach (Connector item in conSet_2)
                                {
                                    if (item.Owner.Id == elemMoreTwoCon[i])
                                    {
                                        if (!listElements.Contains(elemCon.Owner.Id) && GetConnectorSet(elemCon.Owner) != null
                                            && elemCon.Owner.Id != elemMoreTwoCon[i] && elemCon.Owner.Id != con_Axis.Owner.Id)
                                        {
                                            listElements.Add(elemCon.Owner.Id);
                                            if (x == 0)
                                            {
                                                countCon[i]--;
                                                x++;
                                            }
                                            csi_3 = GetConnectorSet(elemCon.Owner).ForwardIterator();
                                        }

                                    }
                                    else
                                    {
                                        
                                        if (!listElements.Contains(item.Owner.Id) && GetConnectorSet(item.Owner) != null
                                           && item.Owner.Id != elemMoreTwoCon[i] && elemCon.Owner.Id != con_Axis.Owner.Id)
                                        {
                                            listElements.Add(item.Owner.Id);
                                            if (x == 0)
                                            {
                                                countCon[i]--;
                                                x++;
                                            }
                                            csi_3 = GetConnectorSet(item.Owner).ForwardIterator();
                                        }
                                        

                                    }
                                }



                            }
                        }



                        
                    }
                }
            }

            return listElements;
        }

        public static ConnectorSet GetConnectorSet(Element element, out int countUnused)
        {
            if (element is FamilyInstance family)
            {
                ConnectorManager list = family.MEPModel.ConnectorManager;
                countUnused = list.UnusedConnectors.Size;
                ConnectorSet connectorSet = list.Connectors;
                return connectorSet;
            }

            if (element is MEPCurve curve)
            {
                ConnectorManager list = curve.ConnectorManager;
                countUnused = list.UnusedConnectors.Size;
                ConnectorSet connectorSet = list.Connectors;
                return connectorSet;
            }

            countUnused = 0;
            return null;
        }
        public static ConnectorSet GetConnectorSet(Element element)
        {
            if (element is FamilyInstance family)
            {
                ConnectorManager list = family.MEPModel.ConnectorManager;
                ConnectorSet connectorSet = list.Connectors;
                return connectorSet;
            }

            if (element is MEPCurve curve)
            {
                ConnectorManager list = curve.ConnectorManager;
                ConnectorSet connectorSet = list.Connectors;
                return connectorSet;
            }

            return null;
        }


    }
}
