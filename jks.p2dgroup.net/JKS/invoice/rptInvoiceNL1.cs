using System;
using DataDynamics.ActiveReports;
using DataDynamics.ActiveReports.Document;

namespace CBSolutions.ETH.Web.ETC.invoice
{
    public class rptInvoiceNL : ActiveReport
    {
        public rptInvoiceNL()
        {
            InitializeReport();
        }

        #region ActiveReports Designer generated code
        private void InitializeReport()
        {
            this.LoadLayout(this.GetType(), "rptInvoiceNL.rpx");
        }
        #endregion
    }
}
