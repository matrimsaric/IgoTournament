using ServerCommonModule.Database.Interfaces;
using ServerCommonModule.Model;
using ServerCommonModule.Support;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerCommonModule.Database
{

    public class EnvironmentalParameters : IEnvironmentalParameters
    {


        public string ConnectionString { get; set; } = "Host=localhost;Port=5434;Username=postgres;Password=modena;Database=IgoTournament";

        public int ServerSessionTimeoutInSeconds { get; set; }

        public bool ShowVerboseErrors { get; set; }

        public string DatabaseType { get; set; } = string.Empty;

        public ConnectionType Database
        {
            get
            {
                switch (DatabaseType)
                {
                    case ServerConstants.POSTGRESS_NAME:
                        return ConnectionType.POSTGRESS;
                    case ServerConstants.SQL_NAME:
                    default:
                        return ConnectionType.MS_SQL;
                }
            }
        }

        public EnvironmentalParameters()
        {
            //ConnectionString = connectionString;
            ServerSessionTimeoutInSeconds = 60;// default
            ShowVerboseErrors = false;//default
            ConnectionString = String.Empty;
        }


    }
}
