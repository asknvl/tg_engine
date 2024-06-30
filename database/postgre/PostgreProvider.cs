using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tg_engine.config;
using tg_engine.database.postgre.models;
using tg_engine.dm;

namespace tg_engine.database.postgre
{
    public class PostgreProvider : IPostgreProvider
    {
        #region vars
        private readonly DbContextOptions<PostgreDbContext> dbContextOptions;
        #endregion

        public PostgreProvider(settings_db settings)
        {
            var optionsBuilder = new DbContextOptionsBuilder<PostgreDbContext>();
            optionsBuilder.UseNpgsql($"Host={settings.host};Username={settings.user};Password={settings.password};Database={settings.db_name};Pooling=true;");
            dbContextOptions = optionsBuilder.Options;
        }

        public async Task<List<account>> GetAccountsAsync()
        {
            using (var context = new PostgreDbContext(dbContextOptions))
            {   
                return await context.accounts.ToListAsync();                
            }            
        }

        public async Task<List<channel_account>> GetChannelsAccounts()
        {
            using (var context = new PostgreDbContext(dbContextOptions))
            {
                return await context.channels_accounts.ToListAsync();
            }
        }

        public async Task<List<DMStartupSettings>> GetStatupData()
        {
            using (var context = new PostgreDbContext(dbContextOptions))
            {
                //var query = from account in context.accounts                            
                //            from channel_account in context.channels_accounts
                //            join channel in context.channels on channel_account.channel_id equals channel.id
                //            join source in context.sources on channel.id equals source.channel_id
                //            select new
                //            {
                //                source = source.source_name,
                //                account = account
                //            };


                var query = from account in context.accounts
                            join channelAccount in context.channels_accounts on account.id equals channelAccount.account_id
                            join channel in context.channels on channelAccount.channel_id equals channel.id
                            join source in context.sources on channel.id equals source.channel_id
                            select new
                            {
                                source = source.source_name,
                                account = account
                            };

                List<DMStartupSettings> res = new();

                foreach (var q in query)
                {
                    res.Add(new DMStartupSettings() { 
                    
                        source = q.source,
                        account = q.account
                    
                    });
                }

                return res;
            }
        }
    } 
}
