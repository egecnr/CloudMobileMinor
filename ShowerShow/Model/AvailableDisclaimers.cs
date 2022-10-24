using System;

namespace ShowerShow.Model
{
    public partial class Disclaimers
    {
        [Flags]
        public enum AvailableDisclaimers { 
            ViewsExpressed, 
            NoResponsibility, 
            PastPerformance, 
            UseAtYourOwnRisk, 
            ErrosAndOmissions, 
            FairUse, 
            Investment, 
            CopyRightNotice, 
            Email}
    }

}
