using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Web;
using System.Web.Services;
using System.Web.Services.Description;
using System.Web.Services.Protocols;
using System.Xml.Serialization;

[WebServiceBinding(Name = "WSCheckExistingFileFolder", Namespace = "http://localhost:2745/")]
/// <summary>
/// Summary description for WSCheckExistingFileFolder
/// </summary>
public class WSCheckExistingFileFolder : SoapHttpClientProtocol
{
    public WSCheckExistingFileFolder()
    {

    }

    [SoapDocumentMethod("http://localhost:2745/ReturnIfFolderExists")]
    public bool ReturnIfFolderExists(string FolderPath)
    {
        return (bool)this.Invoke("ReturnIfFolderExists", new object[1] { (object)FolderPath })[0];
    }

    [SoapDocumentMethod("http://localhost:2745/ReturnIfFileExists")]
    public bool ReturnIfFileExists(string FilePath)
    {
        return (bool)this.Invoke("ReturnIfFileExists", new object[1] { (object)FilePath })[0];
    }
}