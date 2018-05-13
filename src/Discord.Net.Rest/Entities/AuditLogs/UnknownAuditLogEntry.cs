using Model = Discord.API.AuditLog;
using EntryModel = Discord.API.AuditLogEntry;

namespace Discord.Rest
{
    public class UnknownAuditLogEntry : RestAuditLogEntry
    {
        private UnknownAuditLogEntry(BaseDiscordClient discord, EntryModel model, IUser user) : base(discord, model, user) { }

        internal static UnknownAuditLogEntry Create(BaseDiscordClient discord, Model log, EntryModel model, IUser user)
        {
            return new UnknownAuditLogEntry(discord, model, user);
        }
    }
}
