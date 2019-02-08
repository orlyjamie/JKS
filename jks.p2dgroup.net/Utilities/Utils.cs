//
using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web.SessionState;

namespace CBSolutions.ETH.Web
{
    #region Enum SecurityAccessTypes
    /// <summary>
    /// possible security access types for user roles
    /// </summary>
    public enum SecurityAccessTypes
    {
        /// <summary>
        /// the menu option is enabled/invisible
        /// </summary>
        Hidden = 0,
        /// <summary>
        /// use can only view information 
        /// </summary>
        View = 1,
        /// <summary>
        /// user can view/edit/add information
        /// </summary>
        Active = 2
    }
    #endregion

    #region Enum Actions
    /// <summary>
    /// All the actions that an user can perform
    /// </summary>
    public enum Actions
    {
        /// <summary>
        /// Add a new company
        /// </summary>
        AddNewCompany = 1,
        /// <summary>
        /// modify an existing company
        /// </summary>
        ChangeCompanyDetails = 2,
        /// <summary>
        /// Add or change an user
        /// </summary>
        AddChangeUser = 5
    }
    #endregion

    #region Enum RoleType
    public enum RoleType
    {
        /// <summary>
        /// 
        /// </summary>
        SuperAdministrator = 0,
        /// <summary>
        /// 
        /// </summary>
        CompanyAdministrator = 1,
        /// <summary>
        /// 
        /// </summary>
        USer = 2
    }
    #endregion

    #region Enum CompanyType
    public enum CompanyType
    {
        /// <summary>
        /// 
        /// </summary>
        InvoiceIn = 1,
        /// <summary>
        ///		
        /// </summary>
        InvoiceOut = 2,
        /// <summary>
        /// 
        /// </summary>
        InvoiceInOut = 3
    }
    #endregion

    /// <summary>
    /// class containing many useful static functions
    /// </summary>
    public class Utils
    {
        #region Default Constructor
        public Utils()
        {
        }
        #endregion

        #region LoginValid
        /// <summary>
        /// if Session-UserID is either 0 or null we force a redirect to the home page
        /// </summary>
        /// <param name="session"></param>
        /// <param name="response"></param>
        public static void LoginValid(HttpSessionState session, HttpResponse response)
        {
            if ((session["UserID"]) == null)
                response.Redirect("../default.aspx");
            if ((int)(session["UserID"]) == 0)
                response.Redirect("../default.aspx");
        }
        #endregion
    }
}
