using System.Linq;

using Model = Discord.API.AuditLog;
using EntryModel = Discord.API.AuditLogEntry;

namespace Discord.Rest
{
    public class KickAuditLogEntry : RestAuditLogEntry
    {
        private KickAuditLogEntry(BaseDiscordClient discord, EntryModel model, IUser user, RestUser target) : base(discord, model, user)
        {
            Target = target;
        }

        internal static KickAuditLogEntry Create(BaseDiscordClient discord, Model log, EntryModel entry, IUser user)
        {
            var userInfo = log.Users.FirstOrDefault(x => x.Id == entry.TargetId);
            return new KickAuditLogEntry(discord, entry, user, RestUser.Create(discord, userInfo));
        }

        public IUser Target { get; }
    }
}
