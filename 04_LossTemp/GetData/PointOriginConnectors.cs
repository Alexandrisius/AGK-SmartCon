using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Plumbing;
using Autodesk.Revit.UI.Selection;

namespace LossTemp
{
    public class PointOriginConnectors
    {
        /// <summary>
        /// Метод который возвращает ближайший коннектор к месту клика мышкой.
        /// </summary>
        /// <param name="sel"></param>
        /// <param name="doc"></param>
        /// <param name="str"></param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentException"></exception>
        public static Connector GetConnector(Selection sel, Document doc)
        {
            double length = double.MaxValue;
            Connector connector = null;
            Reference reference = null;
            XYZ point = null;
            FilterWithPipe filterUnusedCon = new FilterWithPipe();
            try
            {
                reference = sel.PickObject(ObjectType.Element, filterUnusedCon, "Выберите открытый конец системы");
                point = reference.GlobalPoint;
            }
            catch
            {
            }

            if (reference != null)
            {
                using (Element elem = doc.GetElement(reference))
                {

                    if (elem is MEPCurve elemPipe)
                    {
                        ConnectorSet list = elemPipe.ConnectorManager.Connectors;
                        foreach (Connector item in list)
                        {
                            if (!item.IsConnected)
                            {
                                if (item.Origin.DistanceTo(point) < length)
                                {
                                    length = item.Origin.DistanceTo(point);

                                    connector = item;
                                }
                            }
                        }

                        return connector;
                    }

                    if (elem is FamilyInstance family)
                    {
                        ConnectorSet list = family.MEPModel.ConnectorManager.Connectors;
                        foreach (Connector item in list)
                        {
                            if (!item.IsConnected)
                            {
                                if (item.Origin.DistanceTo(point) < length)
                                {
                                    length = item.Origin.DistanceTo(point);
                                    connector = item;
                                }
                            }
                        }
                    }

                    return connector;
                }
            }
            return null;
        }
        public class FilterWithPipe : ISelectionFilter
        {
            public bool AllowElement(Element elem)
            {
                return GetUnusedConnectors(elem);
            }

            public bool AllowReference(Reference reference, XYZ position)
            {
                return false;
            }
            public static bool GetUnusedConnectors(Element elem)
            {
                if (elem is FamilyInstance family)
                {
                    ConnectorManager list = family.MEPModel.ConnectorManager;
                    if (list.UnusedConnectors.Size > 0)
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
}
