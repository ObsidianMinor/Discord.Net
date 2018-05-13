using System.Linq;

using Model = Discord.API.AuditLog;
using EntryModel = Discord.API.AuditLogEntry;

namespace Discord.Rest
{
    public class OverwriteCreateAuditLogEntry : RestAuditLogEntry
    {
        private OverwriteCreateAuditLogEntry(BaseDiscordClient discord, EntryModel model, IUser user, Overwrite overwrite) : base(discord, model, user)
        {
            Overwrite = overwrite;
        }

        internal static OverwriteCreateAuditLogEntry Create(BaseDiscordClient discord, Model log, EntryModel entry, IUser user)
        {
            var changes = entry.Changes;

            var denyModel = changes.FirstOrDefault(x => x.ChangedProperty == "deny");
            var allowModel = changes.FirstOrDefault(x => x.ChangedProperty == "allow");

            var deny = denyModel.NewValue.ToObject<ulong>();
            var allow = allowModel.NewValue.ToObject<ulong>();

            var permissions = new OverwritePermissions(allow, deny);

            var id = entry.Options.OverwriteTargetId.Value;
            var type = entry.Options.OverwriteType;

            PermissionTarget target = type == "member" ? PermissionTarget.User : PermissionTarget.Role;

            return new OverwriteCreateAuditLogEntry(discord, entry, user, new Overwrite(id, target, permissions));
        }

        public Overwrite Overwrite { get; }
    }
}
