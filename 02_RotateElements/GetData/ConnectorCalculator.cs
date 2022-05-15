using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RotateElements
{
    internal class ConnectorCalculator
    {
        public static Line GetAxisByTwoElements(Element elem1, Element elem_2, XYZ point)
        {
            double length = double.MaxValue;
            Connector connector = null;

            if (elem_2 is MEPCurve elemPipe)
            {
                ConnectorSet list = elemPipe.ConnectorManager.Connectors;
                foreach (Connector item in list)
                {
                        if (item.Origin.DistanceTo(point) < length)
                        {
                            length = item.Origin.DistanceTo(point);

                            connector = item;
                        }
                }

                return Line.CreateBound(connector.Origin,
                                            connector.Origin + connector.CoordinateSystem.BasisZ);
            }

            if (elem_2 is FamilyInstance family)
            {
                ConnectorSet list = family.MEPModel.ConnectorManager.Connectors;
                foreach (Connector item in list)
                {
                   
                        if (item.Origin.DistanceTo(point) < length)
                        {
                            length = item.Origin.DistanceTo(point);
                            connector = item;
                        }
                }
            }

            return Line.CreateBound(connector.Origin,
                                            connector.Origin + connector.CoordinateSystem.BasisZ);

            return null;
        }
    }
}
