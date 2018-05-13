using System.Linq;

using Model = Discord.API.AuditLog;
using EntryModel = Discord.API.AuditLogEntry;

namespace Discord.Rest
{
    public class EmoteDeleteAuditLogEntry : RestAuditLogEntry
    {
        private EmoteDeleteAuditLogEntry(BaseDiscordClient discord, EntryModel model, IUser user, ulong id, string name) : base(discord, model, user)
        {
            EmoteId = id;
            Name = name;
        }

        internal static EmoteDeleteAuditLogEntry Create(BaseDiscordClient discord, Model log, EntryModel entry, IUser user)
        {
            var change = entry.Changes.FirstOrDefault(x => x.ChangedProperty == "name");

            var emoteName = change.OldValue?.ToObject<string>();

            return new EmoteDeleteAuditLogEntry(discord, entry, user, entry.TargetId.Value, emoteName);
        }

        public ulong EmoteId { get; }
        public string Name { get; }
    }
}
