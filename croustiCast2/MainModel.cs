using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace croustiCast
{
    class MainModel
    {
        protected static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        protected String sAppPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase); // Defini le dossier de l'application
        protected String sContentPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "croustiCast");

        protected SqlConnection getConnectionData()
        {
            ConnectionStringSettings parametreConnexion = ConfigurationManager.ConnectionStrings["croustiCastBddConnectionString"];
            string laChaineDeConnexion = parametreConnexion.ConnectionString;
            return new SqlConnection(laChaineDeConnexion);
        }

    }
}
