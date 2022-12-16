using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreateScheduleForAssembly
{
    class SchedulesMethods
    {
        public static void AddRegularFieldToSchedule(ViewSchedule schedule, string guid)
        {
            ScheduleDefinition definition = schedule.Definition;


            var schedulableField = schedule.Definition.GetSchedulableFields()
                .FirstOrDefault(x => IsSharedParameterSchedulableField(schedule.Document, x.ParameterId, new Guid(guid)));

            if (schedulableField != null)
            {
                definition.AddField(schedulableField);
            }
        }
        private static bool IsSharedParameterSchedulableField(Document document, ElementId parameterId, Guid sharedParameterId)
        {
            var sharedParameterElement = document.GetElement(parameterId) as SharedParameterElement;

            return sharedParameterElement?.GuidValue == sharedParameterId;
        }
        public static ScheduleField FindField(ViewSchedule schedule, string guid)
        {
            ScheduleDefinition definition = schedule.Definition;
            ScheduleField foundField = null;

            var sharParam = SharedParameterElement.Lookup(schedule.Document, Guid.Parse(guid));
            
            ElementId paramId = sharParam.Id;

            foreach (ScheduleFieldId fieldId in definition.GetFieldOrder())
            {
                foundField = definition.GetField(fieldId);
                if (foundField.ParameterId == paramId)
                {
                    return foundField;
                }
            }

            return null;
        }

    }
}
