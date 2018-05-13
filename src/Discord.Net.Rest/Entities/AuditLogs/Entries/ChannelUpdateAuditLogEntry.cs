using System.Linq;

using Model = Discord.API.AuditLog;
using EntryModel = Discord.API.AuditLogEntry;

namespace Discord.Rest
{
    public class ChannelUpdateAuditLogEntry : RestAuditLogEntry
    {
        private ChannelUpdateAuditLogEntry(BaseDiscordClient discord, EntryModel model, IUser user, ulong id, ChannelInfo before, ChannelInfo after) : base(discord, model, user)
        {
            ChannelId = id;
            Before = before;
            After = after;
        }

        internal static ChannelUpdateAuditLogEntry Create(BaseDiscordClient discord, Model log, EntryModel entry, IUser user)
        {
            var changes = entry.Changes;

            var nameModel = changes.FirstOrDefault(x => x.ChangedProperty == "name");
            var topicModel = changes.FirstOrDefault(x => x.ChangedProperty == "topic");
            var bitrateModel = changes.FirstOrDefault(x => x.ChangedProperty == "bitrate");
            var userLimitModel = changes.FirstOrDefault(x => x.ChangedProperty == "user_limit");

            string oldName = nameModel?.OldValue?.ToObject<string>(),
                newName = nameModel?.NewValue?.ToObject<string>();
            string oldTopic = topicModel?.OldValue?.ToObject<string>(),
                newTopic = topicModel?.NewValue?.ToObject<string>();
            int? oldBitrate = bitrateModel?.OldValue?.ToObject<int>(),
                newBitrate = bitrateModel?.NewValue?.ToObject<int>();
            int? oldLimit = userLimitModel?.OldValue?.ToObject<int>(),
                newLimit = userLimitModel?.NewValue?.ToObject<int>();

            var before = new ChannelInfo(oldName, oldTopic, oldBitrate, oldLimit);
            var after = new ChannelInfo(newName, newTopic, newBitrate, newLimit);

            return new ChannelUpdateAuditLogEntry(discord, entry, user, entry.TargetId.Value, before, after);
        }

        public ulong ChannelId { get; }
        public ChannelInfo Before { get; set; }
        public ChannelInfo After { get; set; }
    }
}
