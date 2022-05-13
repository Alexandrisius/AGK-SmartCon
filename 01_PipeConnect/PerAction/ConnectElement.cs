using System;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Plumbing;
using Autodesk.Revit.UI;

namespace PipeConnect
{
    public class ConnectElement
    {
        public static void ConnectTo(Connector con1, Connector con2)
        {
            con1.ConnectTo(con2);
        }
    }
}
