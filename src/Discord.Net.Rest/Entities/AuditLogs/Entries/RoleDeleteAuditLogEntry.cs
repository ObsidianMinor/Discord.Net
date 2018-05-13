using System.Linq;

using Model = Discord.API.AuditLog;
using EntryModel = Discord.API.AuditLogEntry;

namespace Discord.Rest
{
    public class RoleDeleteAuditLogEntry : RestAuditLogEntry
    {
        private RoleDeleteAuditLogEntry(BaseDiscordClient discord, EntryModel model, IUser user, ulong id, RoleInfo props) : base(discord, model, user)
        {
            RoleId = id;
            Properties = props;
        }

        internal static RoleDeleteAuditLogEntry Create(BaseDiscordClient discord, Model log, EntryModel entry, IUser user)
        {
            var changes = entry.Changes;

            var colorModel = changes.FirstOrDefault(x => x.ChangedProperty == "color");
            var mentionableModel = changes.FirstOrDefault(x => x.ChangedProperty == "mentionable");
            var hoistModel = changes.FirstOrDefault(x => x.ChangedProperty == "hoist");
            var nameModel = changes.FirstOrDefault(x => x.ChangedProperty == "name");
            var permissionsModel = changes.FirstOrDefault(x => x.ChangedProperty == "permissions");

            uint? colorRaw = colorModel?.OldValue?.ToObject<uint>();
            bool? mentionable = mentionableModel?.OldValue?.ToObject<bool>();
            bool? hoist = hoistModel?.OldValue?.ToObject<bool>();
            string name = nameModel?.OldValue?.ToObject<string>();
            ulong? permissionsRaw = permissionsModel?.OldValue?.ToObject<ulong>();

            Color? color = null;
            GuildPermissions? permissions = null;

            if (colorRaw.HasValue)
                color = new Color(colorRaw.Value);
            if (permissionsRaw.HasValue)
                permissions = new GuildPermissions(permissionsRaw.Value);

            return new RoleDeleteAuditLogEntry(discord, entry, user, entry.TargetId.Value,
                new RoleInfo(color, mentionable, hoist, name, permissions));
        }

        public ulong RoleId { get; }
        public RoleInfo Properties { get; }
    }
}
