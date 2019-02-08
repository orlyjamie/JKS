using System;
using DataDynamics.ActiveReports;
using DataDynamics.ActiveReports.Document;

namespace CBSolutions.ETH.Web.ETC.invoice
{
    public class DebitNoteNL : ActiveReport
    {
        public DebitNoteNL()
        {
            InitializeReport();
        }

        #region ActiveReports Designer generated code
        private void InitializeReport()
        {
            this.LoadLayout(this.GetType(), "DebitNoteNL.rpx");
        }
        #endregion
    }
}
