using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UzmanCrm.CrmService.Common.Helpers
{
    public static class CommonHelper
    {
        public static bool GoToTag(string e)
        {

            if (e.Contains("failed.") || e.Contains("System.ServiceModel.Channel"))
            { 
                return true;
            }
            return false;
        }
    }
}
