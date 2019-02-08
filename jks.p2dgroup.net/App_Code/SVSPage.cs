using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Text;

/// <summary>
/// Summary description for VSPage.
/// </summary>
public partial class SVSPage : System.Web.UI.Page
{
    protected override object LoadPageStateFromPersistenceMedium()
    {

        if (Session["DefaultPageViewState"] != null)
        {

            string viewStateString = Session["DefaultPageViewState"] as string;

            TextReader reader = new StringReader(viewStateString);

            LosFormatter formatter = new LosFormatter();

            object state = formatter.Deserialize(reader);

            return state;

        }

        else

            return base.LoadPageStateFromPersistenceMedium();

        //return null;

    }



    protected override void SavePageStateToPersistenceMedium(object state)
    {

        LosFormatter formatter = new LosFormatter();

        StringWriter writer = new StringWriter();

        formatter.Serialize(writer, state);

        Session["DefaultPageViewState"] = writer.ToString();



    }
}
