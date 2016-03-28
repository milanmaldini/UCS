using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Net;
using UCS.Database;
using UCS.Logic;

namespace UCS.Core
{
    internal class DatabaseManager
    {
        private readonly string m_vConnectionString;

        public DatabaseManager()
        {
            m_vConnectionString = ConfigurationManager.AppSettings["databaseConnectionName"];
        }

        public void CreateAccount(Level l)
        {
            try
            {
                Debugger.WriteLine("[UCS][UCSDB] Saving new account to database (player id: " + l.GetPlayerAvatar().GetId() + ")");
                using (var db = new ucsdbEntities(m_vConnectionString))
                {
                    db.player.Add(
                        new player
                        {
                            PlayerId = l.GetPlayerAvatar().GetId(),
                            AccountStatus = l.GetAccountStatus(),
                            AccountPrivileges = l.GetAccountPrivileges(),
                            LastUpdateTime = l.GetTime(),
                            Avatar = l.GetPlayerAvatar().SaveToJSON(),
                            GameObjects = l.SaveToJSON()
                        }
                        );
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Debugger.WriteLine("[UCS][UCSDB] An exception occured during CreateAccount processing :", ex);
            }
        }

        public void CreateAlliance(Alliance a)
        {
            try
            {
                using (var db = new ucsdbEntities(m_vConnectionString))
                {
                    db.clan.Add(
                        new clan
                        {
                            ClanId = a.GetAllianceId(),
                            LastUpdateTime = DateTime.Now,
                            Data = a.SaveToJSON()
                        }
                        );
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Debugger.WriteLine("[UCS][UCSDB] An exception occured during CreateAlliance processing :", ex);
            }
        }

        public Level GetAccount(long playerId)
        {
            Level account = null;
            try
            {
                using (var db = new ucsdbEntities(m_vConnectionString))
                {
                    var p = db.player.Find(playerId);

                    if (p != null)
                    {
                        account = new Level();
                        account.SetAccountStatus(p.AccountStatus);
                        account.SetAccountPrivileges(p.AccountPrivileges);
                        account.SetTime(p.LastUpdateTime);
                        account.GetPlayerAvatar().LoadFromJSON(p.Avatar);
                        account.LoadFromJSON(p.GameObjects);
                    }
                }
            }
            catch (Exception ex)
            {
                Debugger.WriteLine("[UCS][UCSDB] An exception occured during GetAccount processing :", ex);
            }
            return account;
        }

        public Alliance GetAlliance(long allianceId)
        {
            Alliance alliance = null;
            try
            {
                using (var db = new ucsdbEntities(m_vConnectionString))
                {
                    var p = db.clan.Find(allianceId);
                    if (p != null)
                    {
                        alliance = new Alliance();
                        alliance.LoadFromJSON(p.Data);
                    }
                }
            }
            catch (Exception ex)
            {
                Debugger.WriteLine("[UCS][UCSDB] An exception occured during GetAlliance processing :", ex);
            }
            return alliance;
        }

        public long GetMaxAllianceId()
        {
            long max = 0;
            using (var db = new ucsdbEntities(m_vConnectionString))
                max = (from alliance in db.clan select (long?) alliance.ClanId ?? 0).DefaultIfEmpty().Max();
            return max;
        }

        public long GetMaxPlayerId()
        {
            long max = 0;
            using (var db = new ucsdbEntities(m_vConnectionString))
                max = (from ep in db.player select (long?) ep.PlayerId ?? 0).DefaultIfEmpty().Max();
            return max;
        }

        public void Save(List<Level> avatars)
        {
            try
            {
                using (var context = new ucsdbEntities(m_vConnectionString))
                {
                    context.Configuration.AutoDetectChangesEnabled = false;
                    context.Configuration.ValidateOnSaveEnabled = false;
                    var transactionCount = 0;
                    foreach (var pl in avatars)
                    {
                        lock (pl)
                        {
                            var p = context.player.Find(pl.GetPlayerAvatar().GetId());
                            if (p != null)
                            {
                                p.LastUpdateTime = pl.GetTime();
                                p.AccountStatus = pl.GetAccountStatus();
                                p.AccountPrivileges = pl.GetAccountPrivileges();
                                p.Avatar = pl.GetPlayerAvatar().SaveToJSON();
                                p.GameObjects = pl.SaveToJSON();
                                context.Entry(p).State = EntityState.Modified;
                            }
                            else
                            {
                                context.player.Add(
                                    new player
                                    {
                                        PlayerId = pl.GetPlayerAvatar().GetId(),
                                        AccountStatus = pl.GetAccountStatus(),
                                        AccountPrivileges = pl.GetAccountPrivileges(),
                                        LastUpdateTime = pl.GetTime(),
                                        Avatar = pl.GetPlayerAvatar().SaveToJSON(),
                                        GameObjects = pl.SaveToJSON()
                                    }
                                    );
                            }
                        }
                        transactionCount++;
                        if (transactionCount >= 500)
                        {
                            context.SaveChanges();
                            transactionCount = 0;
                        }
                    }
                    context.SaveChanges();
                }
                Debugger.WriteLine("[UCS][UCSDB] All players in memory has been saved to database at " + DateTime.Now);
            }
            catch (Exception ex)
            {
                Debugger.WriteLine("[UCS][UCSDB] An exception occured during Save processing for avatars :", ex);
            }
        }

        public void Save(List<Alliance> alliances)
        {
            try
            {
                using (var context = new ucsdbEntities(m_vConnectionString))
                {
                    context.Configuration.AutoDetectChangesEnabled = false;
                    context.Configuration.ValidateOnSaveEnabled = false;
                    var transactionCount = 0;
                    foreach (var alliance in alliances)
                    {
                        lock (alliance)
                        {
                            var c = context.clan.Find((int) alliance.GetAllianceId());
                            if (c != null)
                            {
                                c.LastUpdateTime = DateTime.Now;
                                c.Data = alliance.SaveToJSON();
                                context.Entry(c).State = EntityState.Modified;
                            }
                            else
                            {
                                context.clan.Add(
                                    new clan
                                    {
                                        ClanId = alliance.GetAllianceId(),
                                        LastUpdateTime = DateTime.Now,
                                        Data = alliance.SaveToJSON()
                                    }
                                    );
                            }
                        }
                        transactionCount++;
                        if (transactionCount >= 500)
                        {
                            context.SaveChanges();
                            transactionCount = 0;
                        }
                    }
                    context.SaveChanges();
                }
                Debugger.WriteLine("[UCS][UCSDB] All alliances in memory has been saved to database at " + DateTime.Now);
            }
            catch (Exception ex)
            {
                Debugger.WriteLine("[UCS][UCSDB] An exception occured during Save processing for alliances :", ex);
            }
        }
    }
}