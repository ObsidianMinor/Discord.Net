using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Model = Discord.API.AuditLog;
using EntryModel = Discord.API.AuditLogEntry;

namespace Discord.Rest
{
    public class EmoteCreateAuditLogEntry : RestAuditLogEntry
    {
        private EmoteCreateAuditLogEntry(BaseDiscordClient discord, EntryModel model, IUser user, ulong id, string name) : base(discord, model, user)
        {
            EmoteId = id;
            Name = name;
        }

        internal static EmoteCreateAuditLogEntry Create(BaseDiscordClient discord, Model log, EntryModel entry, IUser user)
        {
            var change = entry.Changes.FirstOrDefault(x => x.ChangedProperty == "name");

            var emoteName = change.NewValue?.ToObject<string>();
            return new EmoteCreateAuditLogEntry(discord, entry, user, entry.TargetId.Value, emoteName);
        }

        public ulong EmoteId { get; }
        public string Name { get; }
    }
}
