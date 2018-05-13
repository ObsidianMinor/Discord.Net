using System.Linq;

using Model = Discord.API.AuditLog;
using EntryModel = Discord.API.AuditLogEntry;

namespace Discord.Rest
{
    public class BanAuditLogEntry : RestAuditLogEntry
    {
        private BanAuditLogEntry(BaseDiscordClient discord, EntryModel model, IUser user, IUser target) : base(discord, model, user)
        {
            Target = target;
        }

        internal static BanAuditLogEntry Create(BaseDiscordClient discord, Model log, EntryModel entry, IUser user)
        {
            var userInfo = log.Users.FirstOrDefault(x => x.Id == entry.TargetId);
            return new BanAuditLogEntry(discord, entry, user, RestUser.Create(discord, userInfo));
        }

        public IUser Target { get; }
    }
}
