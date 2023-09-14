using Autodesk.Revit.DB;
using System.Collections.Generic;

namespace RotateElements
{
    class Iterator
    {
        /// <summary>
        /// Метод который возвращает все элементы цепочки начиная с выбранного в противоположную сторону от второго выбранного элемента.
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="elemForRotate"></param>
        /// <param name="conAxis"> Коннектор ось Z которого является осью вращения ветки </param>
        /// <returns>Возвращает список id элементов</returns>
        public static ICollection<ElementId> GetElements(Document doc, Element elemForRotate, Connector conAxis)
        {
            ICollection<ElementId> listUniqueElements = new List<ElementId>();//список уникальный элементов трассировки

            List<ElementId> elemMoreTwoCon = new List<ElementId>();// список элементов у которых больше двух коннекторов

            List<int> countRemainsAnalyze = new List<int>(); // лист показывающий сколько раз нужно пройти по элементу из "elemMoreTwoCon"

            ConnectorSet connectorSet = GetConnectorSet(elemForRotate); // получаю все коннектора элемента для вращения

            if (connectorSet == null) return listUniqueElements; // если их нет, то возвращаю пустой список метода
            ConnectorSetIterator csi = connectorSet.ForwardIterator(); // получаю итератор из списка коннекторов для прохода по каждому из них

            while (csi.MoveNext()) // проверяю можно ли идти вперёд по коннекторам и если можно то прохожу вперёд
            {
                if (!(csi.Current is Connector connector) || ((Connector)csi.Current).Domain == Domain.DomainUndefined) continue; // проверяю являетмя ли текущий элемент коннектором и если
                                                                     // нет то пропускаю итерацию
                int countCon = GetConnectorSet(connector.Owner, out int countUnusedCon).Size; // получаю общее количество коннекторов
                                                                                        // и количество неприсоединённых коннекторов в текущем семействе

                if (countCon > 2 && !elemMoreTwoCon.Contains(connector.Owner.Id)) 
                {
                    elemMoreTwoCon.Add(connector.Owner.Id); // добавляю в список по которому нужно будет потом пройти ещё n количество раз

                    countRemainsAnalyze.Add(countCon - 2 - countUnusedCon); //определяю сколько раз ещё нужно будет пройти по элементам
                }

                if (!listUniqueElements.Contains(connector.Owner.Id)) 
                {
                    listUniqueElements.Add(connector.Owner.Id);// добавляю уникальный элемент
                }
                ConnectorSet conSet = connector.AllRefs; // получаю у коннектора ссылку на соседний коннектор с которым он соединён

                foreach (Connector elemCon in conSet) // прохожу по списку из одного или двух коннекторов-ссылок (у трубы 2 референса,
                                                      // у инстансев всегда один)
                {
                    if (!listUniqueElements.Contains(elemCon.Owner.Id) && GetConnectorSet(elemCon.Owner) != null
                                                                 && elemCon.Owner.Id != conAxis.Owner.Id)
                    {
                        csi = GetConnectorSet(elemCon.Owner).ForwardIterator(); // проеряю уникальность элемента и в случае если это следующий
                                                                                // элемент сети, то беру у него списко коннекторов
                    }

                }
            }
            for (int i = 0; i < elemMoreTwoCon.Count; i++) // цикл прохода по списку всех уэлементов у которых более двух коннекторов
            {
                ConnectorSet conSet_EMTC = GetConnectorSet(doc.GetElement(elemMoreTwoCon[i])); // получаю спискок коннекторов у элемента "elemMoreTwoCon"

                foreach (Connector con_EMTC in conSet_EMTC) // проходу по всем коннекторам элемента "elemMoreTwoCon"
                {
                    int x = 0; // счётчик захода в ответвление, чтобы отнять потом из элемента "elemMoreTwoCon"
                               // величину показывающую сколько раз по нему нужно пройти

                    ConnectorSet conSet_Refs = con_EMTC.AllRefs; // поиск референсов у коннектора элемента "elemMoreTwoCon"
                    ConnectorSetIterator csi_EMTC = conSet_Refs.ForwardIterator();// получаю итератор из списка референсов для прохода по каждому из них

                    while (csi_EMTC.MoveNext()) // проходчик по референсам, максимум 2 элемента списка
                    {
                        if (!(csi_EMTC.Current is Connector elemCon_EMTC) 
                            || ((Connector)csi_EMTC.Current).Domain == Domain.DomainUndefined) continue;// проверяю является ли текущий элемент коннектором и если
                                                                               // нет то пропускаю итерацию
                        int countCon_EMTC = GetConnectorSet(elemCon_EMTC.Owner, out int countUnused).Size;// получаю общее количество коннекторов
                                                                                              // и неприсоединённые коннектора в текущем семействе

                        if (countCon_EMTC > 2 && !elemMoreTwoCon.Contains(elemCon_EMTC.Owner.Id)) // если на пути прохода итератора встерчается элемент с кол-вом коннекторов >2
                                                                                     //и мы по нему ещё ни разу не проходили до добавляем его в список и даём
                                                                                     //номер-количество проходов 
                        {
                            elemMoreTwoCon.Add(elemCon_EMTC.Owner.Id);
                            countRemainsAnalyze.Add(countCon_EMTC - 2 - countUnused);

                        }

                        ConnectorSet conSet_Refs_2 = elemCon_EMTC.AllRefs;

                        foreach (Connector con_EMTC_2 in conSet_Refs_2)
                        {
                            if (con_EMTC_2.Owner.Id == elemMoreTwoCon[i])
                            {
                                if (!listUniqueElements.Contains(elemCon_EMTC.Owner.Id) && GetConnectorSet(elemCon_EMTC.Owner) != null
                                                                             && elemCon_EMTC.Owner.Id != elemMoreTwoCon[i] && elemCon_EMTC.Owner.Id != conAxis.Owner.Id)
                                {
                                    listUniqueElements.Add(elemCon_EMTC.Owner.Id);
                                    if (x == 0)
                                    {
                                        countRemainsAnalyze[i]--;
                                        x++;
                                    }
                                    csi_EMTC = GetConnectorSet(elemCon_EMTC.Owner).ForwardIterator();
                                }

                            }
                            else
                            {
                                        
                                if (!listUniqueElements.Contains(con_EMTC_2.Owner.Id) && GetConnectorSet(con_EMTC_2.Owner) != null
                                                                          && con_EMTC_2.Owner.Id != elemMoreTwoCon[i] && elemCon_EMTC.Owner.Id != conAxis.Owner.Id)
                                {
                                    listUniqueElements.Add(con_EMTC_2.Owner.Id);
                                    if (x == 0)
                                    {
                                        countRemainsAnalyze[i]--;
                                        x++;
                                    }
                                    csi_EMTC = GetConnectorSet(con_EMTC_2.Owner).ForwardIterator();
                                }
                                        

                            }
                        }
                    }



                        
                }
            }

            return listUniqueElements;
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
