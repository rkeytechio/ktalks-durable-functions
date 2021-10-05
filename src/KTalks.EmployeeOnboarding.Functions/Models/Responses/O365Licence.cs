using System.Collections.Generic;

namespace KTalks.EmployeeOnboarding.Functions.Models.Responses
{
    public class O365Licence
    {
        public bool OutlookEnabled { get; set; }

        public List<string> LicenceAssigned { get; set; }
    }
}
