using CompetitionDomain.ControlModule;
using CompetitionDomain.ControlModule.Interfaces;
using ImageDomain.ControlModule;
using ImageDomain.ControlModule.Interfaces;
using PlayerDomain.ControlModule;
using PlayerDomain.ControlModule.Interfaces;
using ServerCommonModule.Configuration;
using ServerCommonModule.Database;
using ServerCommonModule.Database.Interfaces;

public class RepositoryBootstrapper
{
    public enum DatabaseEnvironment { Live, Test }

    public IEnvironmentalParameters Environment { get; }
    public IDbUtilityFactory DbFactory { get; }

    public RepositoryBootstrapper(DatabaseEnvironment env)
    {
        Environment = new EnvironmentalParameters
        {
            ConnectionString = env switch
            {
                DatabaseEnvironment.Live => ServerDefaults.DefaultConnectionString,
                DatabaseEnvironment.Test => ServerDefaults.TestConnectionString,
                _ => throw new ArgumentOutOfRangeException()
            },
            DatabaseType = ServerDefaults.DefaultDatabaseType
        };

        DbFactory = new PgUtilityFactory(Environment, null);
    }


    public IPlayerRepository CreatePlayerRepository()
        => new PlayerRepository(Environment, DbFactory);

    public IRoundRepository CreateRoundRepository()
        => new RoundRepository(Environment, DbFactory);

    public ITournamentRepository CreateTournamentRepository() 
        => new TournamentRepository(Environment, DbFactory); 
    
    public ITeamMembershipRepository CreateTeamMembershipRepository() 
        => new TeamMembershipRepository(Environment,DbFactory);

    public IMatchRepository CreateMatchRepository()
        => new MatchRepository(Environment, DbFactory);

    public ISgfRecordRepository CreateSgfRepository()
        => new SgfRecordRepository(Environment, DbFactory);

    public ITeamRepository CreateTeamRepository()
        => new TeamRepository(Environment, DbFactory);

    public IImageRepository CreateImageRepository()
        => new ImageRepository(Environment, DbFactory);
}
