using Model = Discord.API.AuditLog;
using EntryModel = Discord.API.AuditLogEntry;

namespace Discord.Rest
{
    public class PruneAuditLogEntry : RestAuditLogEntry
    {
        private PruneAuditLogEntry(BaseDiscordClient discord, EntryModel model, IUser user, int pruneDays, int membersRemoved) : base(discord, model, user)
        {
            PruneDays = pruneDays;
            MembersRemoved = membersRemoved;
        }

        internal static PruneAuditLogEntry Create(BaseDiscordClient discord, Model log, EntryModel entry, IUser user)
        {
            return new PruneAuditLogEntry(discord, entry, user, entry.Options.PruneDeleteMemberDays.Value, entry.Options.PruneMembersRemoved.Value);
        }

        public int PruneDays { get; }
        public int MembersRemoved { get; }
    }
}
