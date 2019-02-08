using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
/// <summary>
/// <summary>
/// Summary description for PasswordReset
/// </summary>
/// 
namespace JKS
{
    public class PasswordReset
    {
        public PasswordReset()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public string ResetQuestion { get; set; }
        public string ResetAnswer { get; set; }
        public string ResetLockout { get; set; }
        public string CompanyID { get; set; }
        public int iReturnValue { get; set; }
        public string EmailId { get; set; }
        public string UserName { get; set; }
        public string LoginCompanyId { get; set; }
        public string AwitingApprovalCompany { get; set; }
        public string AwitingApproval { get; set; }
        public string InvoiceNo { get; set; }
        public string DocType { get; set; }
        public string BuyerCompanyID { get; set; }
        public string SupplierCompanyID { get; set; }
        public string Duplicate { get; set; }


        ModelPasswordReset objModelPasswordReset = new ModelPasswordReset();

        public List<PasswordReset> getSecurityQuestion(int UserID)
        {
            List<PasswordReset> lstPasswordReset = objModelPasswordReset.getSecurityQuestion(UserID);

            return lstPasswordReset;
        }

        public List<PasswordReset> checkSaltedPassword(int UserID, string strResetAnswer)
        {
            List<PasswordReset> lstSaltedPassword = objModelPasswordReset.checkSaltedPassword(UserID, strResetAnswer);

            return lstSaltedPassword;
        }


        public List<PasswordReset> ForgotChangePassword(int iUserID, string strNewPassword)
        {
            List<PasswordReset> lstForgotChangePassword = objModelPasswordReset.ForgotChangePassword(iUserID, strNewPassword);

            return lstForgotChangePassword;
        }


        public List<PasswordReset> getEmailByUserID(int UserID)
        {
            List<PasswordReset> lstEmailByUserID = objModelPasswordReset.getEmailByUserID(UserID);

            return lstEmailByUserID;
        }

        public List<PasswordReset> GetUserNameByUserID(int UserID)
        {
            List<PasswordReset> lstUserName = objModelPasswordReset.GetUserNameByUserID(UserID);

            return lstUserName;
        }


        public List<PasswordReset> GetLogInCompanyId(int UserID)
        {
            List<PasswordReset> lstUserName = objModelPasswordReset.GetLogInCompanyId(UserID);

            return lstUserName;
        }


        public List<PasswordReset> GetDuplicates(string InvoiceNo, string DocType, string BuyerCompanyID, string SupplierCompanyID)
        {
            List<PasswordReset> lstUserName = objModelPasswordReset.GetDuplicates(InvoiceNo, DocType, BuyerCompanyID, SupplierCompanyID);

            return lstUserName;
        }

        public List<PasswordReset> UpdateDepartmentId(string InvoiceId, string BuyerCompanyID, string Department, string CodingDescriptionId)
        {
            List<PasswordReset> lstUserName = objModelPasswordReset.UpdateDepartmentId(InvoiceId, BuyerCompanyID, Department, CodingDescriptionId);// CodingDescriptionId Added by Rimi on 30.06.2015
            return lstUserName;
        }


        public List<AwitingApprovalcls> GetApprovals(int UserID)
        {
            List<PasswordReset> lstUserName = objModelPasswordReset.GetApprovals(UserID);
            List<AwitingApprovalcls> lstAwitingApproval = new List<AwitingApprovalcls>();
            foreach (PasswordReset PR in lstUserName)
            {
                AwitingApprovalcls objAwitingApproval = new AwitingApprovalcls();
                objAwitingApproval.Company = PR.AwitingApprovalCompany;
                objAwitingApproval.AwitingApprovals = PR.AwitingApproval;
                lstAwitingApproval.Add(objAwitingApproval);
            }

            return lstAwitingApproval;
        }

        public class AwitingApprovalcls
        {
            public string Company { get; set; }
            public string AwitingApprovals { get; set; }
        }
    }


}