using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PipeConnect
{
    public class TurnByClick
    {
        public static void TurnRight(Document doc,Connector con1, double angle)
        {
            Line lineZ = Line.CreateBound(con1.Origin, con1.Origin + con1.CoordinateSystem.BasisZ);

            if (angle != 0)
            {
                using (var tx = new Transaction(doc))
                {
                    tx.Start("TurnRight");

                    ElementTransformUtils.RotateElement(doc, con1.Owner.Id, lineZ, angle * Math.PI/180);

                    tx.Commit();
                }

                
            }

        }
        public static void TurnLeft(Document doc, Connector con1, double angle)
        {
            Line lineZ = Line.CreateBound(con1.Origin, con1.Origin + con1.CoordinateSystem.BasisZ);

            if (angle != 0)
            {
                using (var tx = new Transaction(doc))
                {
                    tx.Start("TurnLeft");

                    ElementTransformUtils.RotateElement(doc, con1.Owner.Id, lineZ, -angle * Math.PI / 180);

                    tx.Commit();
                }
                
            }

        }

        public static void TurnAround(Document doc, Connector con1)
        {

        }
    }
}
