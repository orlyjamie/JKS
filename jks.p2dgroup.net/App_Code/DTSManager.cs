using System.Threading;
using System;
using System.Web;
using System.Data;
using System.Configuration;
using System.Runtime.InteropServices;
using DTS;
using System.IO;
using System.Runtime;

namespace JKS
{
    public class DTSManager
    {
        /* Prior to running this code, create a DTS package and save it to SQL Server. Then set a reference to
        // the DTSPackage Object Library version 2.0 COM object.
        */
        #region Member Variable Declaration
        public Package2Class package;
        private string serverName = "";
        private string serverUsername = "";
        private string serverPassword = "";
        private string packageName = "";
        #endregion

        #region Property Declaration
        public string ServerName
        {
            set { serverName = value; }
        }

        public string ServerUsername
        {
            set { serverUsername = value; }
        }
        public string ServerPassword
        {
            set { serverPassword = value; }
        }
        public string PackageName
        {
            set { packageName = value; }
        }
        #endregion

        #region Default Constructor
        public DTSManager()
        {
        }
        #endregion

        #region ExecuteSqlServerPackage METHOD
        public string ExecuteSqlServerPackage(int iBuyerCompanyID, string strFromDate, string strToDate, string strType)
        {
            string strMessage = "";

            UCOMIConnectionPointContainer CnnctPtCont = null;
            UCOMIConnectionPoint CnnctPt = null;
            PackageEventsSink PES = null;

            int iCookie = 0;

            try
            {
                package = new Package2Class();
                CnnctPtCont = (UCOMIConnectionPointContainer)package;
                PES = new PackageEventsSink();
                Guid guid = new Guid("10020605-EB1C-11CF-AE6E-00AA004A34D5");

                CnnctPtCont.FindConnectionPoint(ref guid, out CnnctPt);
                CnnctPt.Advise(PES, out iCookie);
                object pVarPersistStgOfHost = null;

                package.LoadFromSQLServer(serverName, serverUsername, serverPassword, DTSSQLServerStorageFlags.DTSSQLStgFlag_Default, null,
                    null, null, packageName, ref pVarPersistStgOfHost);

                foreach (GlobalVariable global in package.GlobalVariables)
                {
                    try
                    {
                        if (global.Name.Equals("BuyerCompanyID"))
                            package.GlobalVariables.Remove(global.Name);

                        if (global.Name.Equals("FromDate"))
                            package.GlobalVariables.Remove(global.Name);

                        if (global.Name.Equals("ToDate"))
                            package.GlobalVariables.Remove(global.Name);

                    }
                    catch (Exception ex) { strMessage = strMessage + ex.Message + Environment.NewLine; }
                }

                //Read all the global variables that are of type string
                package.GlobalVariables.AddGlobalVariable("BuyerCompanyID", iBuyerCompanyID);
                package.GlobalVariables.AddGlobalVariable("FromDate", strFromDate);
                package.GlobalVariables.AddGlobalVariable("ToDate", strToDate);
                package.Execute();
                package.UnInitialize();
                package = null;
                CnnctPt.Unadvise(iCookie); //a connection that is created by IConnectionPoint.Advise must be closed by calling IConnectionPoint.Unadvise to avoid a memory leak

                strMessage = strMessage + "CSV Files generated successfully through DTS package." + Environment.NewLine;
            }
            catch (System.Runtime.InteropServices.COMException ex) { strMessage = strMessage + ex.Message + Environment.NewLine; package.UnInitialize(); package = null; CnnctPt.Unadvise(iCookie); }
            catch (Exception ex) { strMessage = strMessage + ex.Message + Environment.NewLine; package.UnInitialize(); package = null; CnnctPt.Unadvise(iCookie); }

            return (strMessage);
        }
    }
        #endregion exec METHOD

    #region PackageEventsSink class definition
    //This class is responsible for handling DTS Package events. When an event is fired, a message is sent to
    //the console.
    class PackageEventsSink : DTS.PackageEvents
    {
        public void OnQueryCancel(string EventSource, ref bool pbCancel)
        {
            //Console.WriteLine("OnQueryCancel({0})", EventSource);
            pbCancel = false;
        }
        public void OnStart(string EventSource)
        {
            //Console.WriteLine("OnStart({0})", EventSource);
        }
        public void OnProgress(string EventSource, string ProgressDescription, int PercentComplete, int ProgressCountLow, int ProgressCountHigh)
        {
            //Console.WriteLine("OnProgress({0}, {1}, {2}, {3}, {4})", EventSource, ProgressDescription,
            //	PercentComplete, ProgressCountLow, ProgressCountHigh);
        }
        public void OnError(string EventSource, int ErrorCode, string Source, string Description, string HelpFile, int HelpContext, string IDofInterfaceWithError, ref bool pbCancel)
        {
            //Response.Write(EventSource + " --- " + ErrorCode + " --- " + Source + " --- " + Description + " --- " + HelpFile + " --- " + HelpContext);
            //Console.WriteLine("OnError({0}, {1}, {2}, {3}, {4}, {5})", EventSource, ErrorCode, Source, Description,
            //	HelpFile, HelpContext);			
            pbCancel = false;
        }
        public void OnFinish(string EventSource)
        {
            //Console.WriteLine("OnFinish({0})", EventSource);
        }
    }
    #endregion PackageEventsSink class definition
}