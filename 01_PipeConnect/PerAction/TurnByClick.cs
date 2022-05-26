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

        public static void TurnAroundAxis(Document doc, Connector con1, Connector con2)
        {
            using (var tx = new Transaction(doc))
            {
                tx.Start("MoveElement");

                XYZ newPoint = con2.Origin - con1.Origin;//находим вектор по двум точкам!!!

                ElementTransformUtils.MoveElement(doc, con1.Owner.Id, newPoint);

                doc.Regenerate();

                Line line = PlanePointCalculator.GetNormalAxis(con1, con2);// получаем ось вращения по двум коннекторам!!!

                if (line != null)
                {
                    XYZ vec1 = con1.CoordinateSystem.BasisZ;//!!!

                    XYZ vec2 = con2.CoordinateSystem.BasisZ;
                    double angle2 = vec1.AngleTo(vec2) + Math.PI;
                    ElementTransformUtils.RotateElement(doc, con1.Owner.Id, line, angle2);
                }
                doc.Regenerate();

                if (con1.CoordinateSystem.BasisY.AngleTo(XYZ.BasisZ) != 0)
                {
                    //param.Set(con2.Radius);
                    XYZ vec1 = con1.CoordinateSystem.BasisZ;//!!!
                    XYZ vec2 = con2.CoordinateSystem.BasisZ;

                    Line lineZ = Line.CreateBound(con1.Origin, con1.Origin + con1.CoordinateSystem.BasisZ);//!!!

                    double angle3 = con1.CoordinateSystem.BasisY.AngleTo(con2.CoordinateSystem.BasisY);// угол между векторами двух коннекторов!!!

                    if (angle3 != 0)
                    {
                        ElementTransformUtils.RotateElement(doc, con1.Owner.Id, lineZ, angle3);
                    }

                    double angle4 = con1.CoordinateSystem.BasisY.AngleTo(con2.CoordinateSystem.BasisY);

                    if (angle4 != 0)
                    {
                        ElementTransformUtils.RotateElement(doc, con1.Owner.Id, lineZ, -angle3 * 2);//если угол не стал 0 то поворот против часовой
                    }


                }

                tx.Commit();
            }
        }
    }
}
