using System.Linq;

using Model = Discord.API.AuditLog;
using EntryModel = Discord.API.AuditLogEntry;
using ChangeModel = Discord.API.AuditLogChange;

namespace Discord.Rest
{
    public class MemberUpdateAuditLogEntry : RestAuditLogEntry
    {
        private MemberUpdateAuditLogEntry(BaseDiscordClient discord, EntryModel model, IUser user, IUser target, string newNick, string oldNick) : base(discord, model, user)
        {
            Target = target;
            NewNick = newNick;
            OldNick = oldNick;
        }

        internal static MemberUpdateAuditLogEntry Create(BaseDiscordClient discord, Model log, EntryModel entry, IUser user)
        {
            var changes = entry.Changes.FirstOrDefault(x => x.ChangedProperty == "nick");

            var newNick = changes.NewValue?.ToObject<string>();
            var oldNick = changes.OldValue?.ToObject<string>();

            var targetInfo = log.Users.FirstOrDefault(x => x.Id == entry.TargetId);
            var target = RestUser.Create(discord, targetInfo);

            return new MemberUpdateAuditLogEntry(discord, entry, user, target, newNick, oldNick);
        }

        public IUser Target { get; }
        public string NewNick { get; }
        public string OldNick { get; }
    }
}
