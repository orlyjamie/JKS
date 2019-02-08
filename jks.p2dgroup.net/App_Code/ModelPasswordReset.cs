using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
/// <summary>
/// Summary description for ModelPasswordReset
/// </summary>
/// 
namespace JKS
{
    public class ModelPasswordReset
    {
        public ModelPasswordReset()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        DataAccessLayer objDAL = new DataAccessLayer();

        public List<PasswordReset> getSecurityQuestion(int UserID)
        {

            DataTable dTable = new DataTable();
            // create columns for the DataTable
            DataColumn ParameterName = new DataColumn("ParameterName", typeof(string));
            dTable.Columns.Add(ParameterName);
            // create one more column
            DataColumn ParameterValue = new DataColumn("ParameterValue", typeof(string));
            dTable.Columns.Add(ParameterValue);
            dTable.Rows.Add("@UserID", UserID);
            //DataTable dt = objDAL.SelectWParameter(dTable, "sp_getSecurityQuestion");
            DataTable dt = objDAL.SelectWParameter(dTable, "sp_getSecurityQuestion_JKS");


            List<PasswordReset> lstPasswordReset = new List<PasswordReset>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                PasswordReset objPasswordReset = new PasswordReset();
                objPasswordReset.CompanyID = Convert.ToString(dt.Rows[i]["CompanyID"]);
                objPasswordReset.ResetAnswer = Convert.ToString(dt.Rows[i]["ResetAnswer"]);
                objPasswordReset.ResetLockout = Convert.ToString(dt.Rows[i]["ResetLockout"]);
                objPasswordReset.ResetQuestion = Convert.ToString(dt.Rows[i]["ResetQuestion"]);

                lstPasswordReset.Add(objPasswordReset);
            }
            return lstPasswordReset;
        }

        public List<PasswordReset> checkSaltedPassword(int UserID, string strResetAnswer)
        {

            DataTable dTable = new DataTable();
            // create columns for the DataTable
            DataColumn ParameterName = new DataColumn("ParameterName", typeof(string));
            dTable.Columns.Add(ParameterName);
            // create one more column
            DataColumn ParameterValue = new DataColumn("ParameterValue", typeof(string));
            dTable.Columns.Add(ParameterValue);
            dTable.Rows.Add("@UserID", UserID);
            dTable.Rows.Add("@ResetAnswer", strResetAnswer);
            //DataTable dt = objDAL.SelectWParameter(dTable, "sp_CheckSaltedPassword_GRH");
            DataTable dt = objDAL.SelectWParameter(dTable, "sp_CheckSaltedPassword_JKS");

            List<PasswordReset> lstSaltedPassword = new List<PasswordReset>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                PasswordReset objPasswordReset = new PasswordReset();
                objPasswordReset.iReturnValue = Convert.ToInt16(dt.Rows[i]["iReturnValue"]);

                lstSaltedPassword.Add(objPasswordReset);
            }
            return lstSaltedPassword;
        }

        public List<PasswordReset> ForgotChangePassword(int iUserID, string strNewPassword)
        {

            DataTable dTable = new DataTable();
            // create columns for the DataTable
            DataColumn ParameterName = new DataColumn("ParameterName", typeof(string));
            dTable.Columns.Add(ParameterName);
            // create one more column
            DataColumn ParameterValue = new DataColumn("ParameterValue", typeof(string));
            dTable.Columns.Add(ParameterValue);
            dTable.Rows.Add("@UID", iUserID);
            dTable.Rows.Add("@NPassword", strNewPassword);
            //DataTable dt = objDAL.SelectWParameter(dTable, "sp_ForgotChangePassword_Encrypt_GRH");
            DataTable dt = objDAL.SelectWParameter(dTable, "sp_ForgotChangePassword_Encrypt_JKS");

            List<PasswordReset> lstForgotChangePassword = new List<PasswordReset>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                PasswordReset objPasswordReset = new PasswordReset();
                objPasswordReset.iReturnValue = Convert.ToInt16(dt.Rows[i]["iReturnValue"]);

                lstForgotChangePassword.Add(objPasswordReset);
            }
            return lstForgotChangePassword;

        }



        public List<PasswordReset> getEmailByUserID(int UserID)
        {

            DataTable dTable = new DataTable();
            // create columns for the DataTable
            DataColumn ParameterName = new DataColumn("ParameterName", typeof(string));
            dTable.Columns.Add(ParameterName);
            // create one more column
            DataColumn ParameterValue = new DataColumn("ParameterValue", typeof(string));
            dTable.Columns.Add(ParameterValue);
            dTable.Rows.Add("@UserID", UserID);
            //DataTable dt = objDAL.SelectWParameter(dTable, "sp_GetEmailByUserID_GRH");
            DataTable dt = objDAL.SelectWParameter(dTable, "sp_GetEmailByUserID_JKS");

            List<PasswordReset> lstEmailByUserID = new List<PasswordReset>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                PasswordReset objPasswordReset = new PasswordReset();
                objPasswordReset.EmailId = Convert.ToString(dt.Rows[i]["EMail"]);


                lstEmailByUserID.Add(objPasswordReset);
            }
            return lstEmailByUserID;
        }

        public List<PasswordReset> GetUserNameByUserID(int UserID)
        {

            DataTable dTable = new DataTable();
            // create columns for the DataTable
            DataColumn ParameterName = new DataColumn("ParameterName", typeof(string));
            dTable.Columns.Add(ParameterName);
            // create one more column
            DataColumn ParameterValue = new DataColumn("ParameterValue", typeof(string));
            dTable.Columns.Add(ParameterValue);
            dTable.Rows.Add("@UserID", UserID);
            // DataTable dt = objDAL.SelectWParameter(dTable, "sp_GetEmailByUserID_GRH");
            DataTable dt = objDAL.SelectWParameter(dTable, "sp_GetEmailByUserID_JKS");

            List<PasswordReset> lstUserName = new List<PasswordReset>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                PasswordReset objPasswordReset = new PasswordReset();
                objPasswordReset.UserName = Convert.ToString(dt.Rows[i]["UserName"]);


                lstUserName.Add(objPasswordReset);
            }
            return lstUserName;
        }

        public List<PasswordReset> GetLogInCompanyId(int UserID)
        {

            DataTable dTable = new DataTable();
            // create columns for the DataTable
            DataColumn ParameterName = new DataColumn("ParameterName", typeof(string));
            dTable.Columns.Add(ParameterName);
            // create one more column
            DataColumn ParameterValue = new DataColumn("ParameterValue", typeof(string));
            dTable.Columns.Add(ParameterValue);
            dTable.Rows.Add("@UserID", UserID);
            DataTable dt = objDAL.SelectWParameter(dTable, "GetLogInCompanyId");

            List<PasswordReset> lstUserName = new List<PasswordReset>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                PasswordReset objPasswordReset = new PasswordReset();
                objPasswordReset.LoginCompanyId = Convert.ToString(dt.Rows[i]["CompanyId"]);


                lstUserName.Add(objPasswordReset);
            }
            return lstUserName;
        }



        public List<PasswordReset> GetApprovals(int UserID)
        {

            DataTable dTable = new DataTable();
            // create columns for the DataTable
            DataColumn ParameterName = new DataColumn("ParameterName", typeof(string));
            dTable.Columns.Add(ParameterName);
            // create one more column
            DataColumn ParameterValue = new DataColumn("ParameterValue", typeof(string));
            dTable.Columns.Add(ParameterValue);
            dTable.Rows.Add("@UserID", UserID);
            DataTable dt = objDAL.SelectWParameter(dTable, "AwitingApprovals");

            List<PasswordReset> lstUserName = new List<PasswordReset>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                PasswordReset objPasswordReset = new PasswordReset();
                objPasswordReset.AwitingApprovalCompany = Convert.ToString(dt.Rows[i]["CompanyName"]);
                objPasswordReset.AwitingApproval = Convert.ToString(dt.Rows[i]["AwitingApproval"]);
                lstUserName.Add(objPasswordReset);
            }
            return lstUserName;
        }

        public List<PasswordReset> UpdateDepartmentId(string InvoiceId, string BuyerCompanyID, string Department, string CodingDescrptionId)// Added by Rimi on 30.06.2015
        {

            DataTable dTable = new DataTable();
            // create columns for the DataTable
            DataColumn ParameterName = new DataColumn("ParameterName", typeof(string));
            dTable.Columns.Add(ParameterName);
            // create one more column
            DataColumn ParameterValue = new DataColumn("ParameterValue", typeof(string));
            dTable.Columns.Add(ParameterValue);
            dTable.Rows.Add("@InvoiceId", InvoiceId);
            dTable.Rows.Add("@BuyerCompanyID", BuyerCompanyID);
            dTable.Rows.Add("@Department", Department);
            dTable.Rows.Add("@CodingDescrptionId", CodingDescrptionId);// Added by Rimi on 30.06.2015

            //DataTable dt = objDAL.InsertUpdateDelete(dTable, "UpdateDepartmentId");// Commenetd By Rimi on 23rd July 2015
            DataTable dt = objDAL.InsertUpdateDelete(dTable, "UpdateDepartmentId_GRH");// Added By Rimi on 23rd July 2015

            List<PasswordReset> lstUserName = new List<PasswordReset>();

            return lstUserName;
        }





        public List<PasswordReset> GetDuplicates(string InvoiceNo, string DocType, string BuyerCompanyID, string SupplierCompanyID)
        {

            DataTable dTable = new DataTable();
            // create columns for the DataTable
            DataColumn ParameterName = new DataColumn("ParameterName", typeof(string));
            dTable.Columns.Add(ParameterName);
            // create one more column
            DataColumn ParameterValue = new DataColumn("ParameterValue", typeof(string));
            dTable.Columns.Add(ParameterValue);
            dTable.Rows.Add("@InvoiceNo", InvoiceNo);
            dTable.Rows.Add("@DocType", DocType);
            dTable.Rows.Add("@BuyerCompanyID", BuyerCompanyID);
            dTable.Rows.Add("@SupplierCompanyID", SupplierCompanyID);
            DataTable dt = objDAL.SelectWParameter(dTable, "chkDuplicateCNINV");

            List<PasswordReset> lstUserName = new List<PasswordReset>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                PasswordReset objPasswordReset = new PasswordReset();
                objPasswordReset.Duplicate = Convert.ToString(dt.Rows[i]["Duplicate"]);

                lstUserName.Add(objPasswordReset);
            }
            return lstUserName;
        }





    }

}