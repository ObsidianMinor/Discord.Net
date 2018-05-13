using System.Linq;

using Model = Discord.API.AuditLog;
using EntryModel = Discord.API.AuditLogEntry;

namespace Discord.Rest
{
    public class EmoteUpdateAuditLogEntry : RestAuditLogEntry
    {
        private EmoteUpdateAuditLogEntry(BaseDiscordClient discord, EntryModel model, IUser user, ulong id, string oldName, string newName) : base(discord, model, user)
        {
            EmoteId = id;
            OldName = oldName;
            NewName = newName;
        }

        internal static EmoteUpdateAuditLogEntry Create(BaseDiscordClient discord, Model log, EntryModel entry, IUser user)
        {
            var change = entry.Changes.FirstOrDefault(x => x.ChangedProperty == "name");

            var newName = change.NewValue?.ToObject<string>();
            var oldName = change.OldValue?.ToObject<string>();

            return new EmoteUpdateAuditLogEntry(discord, entry, user, entry.TargetId.Value, oldName, newName);
        }

        public ulong EmoteId { get; }
        public string NewName { get; }
        public string OldName { get; }
    }
}
