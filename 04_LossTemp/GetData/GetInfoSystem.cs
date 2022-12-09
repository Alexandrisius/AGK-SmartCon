using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB.Plumbing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LossTemp
{
    class GetInfoSystem
    {
        public static List<Element> GetRadiator(Document doc)
        {
            List<Element> listRadiators = new List<Element>();

            FilteredElementCollector fec = new FilteredElementCollector(doc);

            IEnumerable<Element> listEq = fec.OfCategory(BuiltInCategory.OST_MechanicalEquipment)
                .WhereElementIsNotElementType()
                .ToElements();

            IEnumerable<Element> listPl = fec.OfCategory(BuiltInCategory.OST_PlumbingFixtures)
                .WhereElementIsNotElementType()
                .ToElements();

            IEnumerable<Element> listElem = Enumerable.Union(listPl, listEq);

            foreach (FamilyInstance elem in listElem)
            {
                try
                {
                    ConnectorSet listCon = elem.MEPModel.ConnectorManager.Connectors;
                    if (listCon.Size == 2)
                    {
                        Connector cn1 = null;
                        Connector cn2 = null;
                        foreach (Connector con in listCon)
                        {
                            if ((doc.GetElement(con.MEPSystem.GetTypeId()) as PipingSystemType).FluidTemperature > 300)
                                // выбираеются конектора пренадлежащие системе с температурой теплоносителя больше 300 Кельвинов
                            {
                                if (con.AssignedPipeFlowConfiguration == PipeFlowConfigurationType.Preset)
                                {
                                    if (con.Direction == FlowDirectionType.In)
                                    {
                                        if (con.PipeSystemType == PipeSystemType.SupplyHydronic)
                                        {
                                            cn1 = con;
                                        }
                                    }
                                    if (con.Direction == FlowDirectionType.Out)
                                    {
                                        if (con.PipeSystemType == PipeSystemType.ReturnHydronic)
                                        {
                                            cn2 = con;
                                        }
                                    }
                                }
                                if (cn1 != null && cn2 != null && cn1.Owner.Id == cn2.Owner.Id)
                                {
                                    listRadiators.Add(con.Owner);
                                }
                            }
                        }
                    }
                }
                catch (Exception)
                {
                }
            }

            TaskDialog.Show("!!!", listRadiators.Count.ToString());
            return listRadiators;
        }

        public static List<Element> CheckCollectorElements (Document doc, Element elem)
        {
            List<Element> listFinish = new List<Element>();

            


            return listFinish ;
        }

    }
}
