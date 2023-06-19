using Cassandra;
using core7_cassandra_angular14.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Cassandra.Storage;
using Microsoft.EntityFrameworkCore.Migrations;

namespace core7_cassandra_angular14.Helpers
{
   public class DataDBContext : DbContext
    {
        protected readonly IConfiguration Configuration;

        public DataDBContext(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {        
//            
            optionsBuilder.UseCassandra("Contact Points=127.0.0.1", "core7", opt =>
            {
                opt.MigrationsHistoryTable(HistoryRepository.DefaultTableName);
                  
// var options = new Cassandra.SSLOptions(SslProtocols.Tls12, true, ValidateServerCertificate);
// options.SetHostNameResolver((ipAddress) => CassandraContactPoint);
// Cluster cluster = Cluster.Builder().WithCredentials(UserName, Password).WithPort(CassandraPort).AddContactPoint(CassandraContactPoint).WithSSL(options).Build();
// ISession session = cluster.Connect();

            }, o => {

                o.WithQueryOptions(new QueryOptions().SetConsistencyLevel(ConsistencyLevel.LocalOne))
                    .WithReconnectionPolicy(new ConstantReconnectionPolicy(1000))
                    .WithRetryPolicy(new DefaultRetryPolicy())
                    .WithLoadBalancingPolicy(new TokenAwarePolicy(Policies.DefaultPolicies.LoadBalancingPolicy))
                    .WithDefaultKeyspace(GetType().Name)
                    .WithPoolingOptions(
                    PoolingOptions.Create()
                        .SetMaxSimultaneousRequestsPerConnectionTreshold(HostDistance.Remote, 1_000_000)
                        .SetMaxSimultaneousRequestsPerConnectionTreshold(HostDistance.Local, 1_000_000)
                        .SetMaxConnectionsPerHost(HostDistance.Local, 1_000_000)
                        .SetMaxConnectionsPerHost(HostDistance.Remote, 1_000_000)
                        .SetMaxRequestsPerConnection(1_000_000)
                );
            });
        }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var timeUuidConverter = new TimeUuidToGuidConverter();
            modelBuilder.EnsureKeyspaceCreated(new KeyspaceReplicationSimpleStrategyClass(2));
            modelBuilder.Entity<User>()
                .ToTable("users")
                .HasKey(p => new { 
                    p.Id, 
                    p.Firstname, 
                    p.Lastname, 
                    p.Email, 
                    p.Mobile, 
                    p.Username, 
                    p.Pasword });
            // modelBuilder.Entity<User>()
            //     .ToTable("applicants")
            //     .Property(p => p.Phones)
            //     .HasColumnType("set<frozen<applicant_addr>>");
            // modelBuilder.Entity<Applicant>()
            //     .ForCassandraSetClusterColumns(_ => _.Order)
            //     .ForCassandraSetClusteringOrderBy(new[] { new CassandraClusteringOrderByOption("Order", CassandraClusteringOrderByOptions.ASC) });
            // modelBuilder.Entity<Applicant>()
            //    .Property(p => p.TimeUuid)
            //    .HasConversion(new TimeUuidToGuidConverter());
            // modelBuilder.Entity<Applicant>()
            //     .Property(p => p.Id)
            //     .HasColumnName("id");
            // modelBuilder.Entity<ApplicantAddress>()
            //     .ToUserDefinedType("applicant_addr")
            //     .HasNoKey();
            // modelBuilder.Entity<ApplicantPhone>()
            //     .ToUserDefinedType("applicant_phone")
            //     .HasNoKey();
        }



    }    
}