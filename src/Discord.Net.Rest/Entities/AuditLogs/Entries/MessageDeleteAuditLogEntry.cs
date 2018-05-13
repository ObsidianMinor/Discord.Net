using Model = Discord.API.AuditLog;
using EntryModel = Discord.API.AuditLogEntry;

namespace Discord.Rest
{
    public class MessageDeleteAuditLogEntry : RestAuditLogEntry
    {
        private MessageDeleteAuditLogEntry(BaseDiscordClient discord, EntryModel model, IUser user, ulong channelId, int count) : base(discord, model, user)
        {
            ChannelId = channelId;
            MessageCount = count;
        }

        internal static MessageDeleteAuditLogEntry Create(BaseDiscordClient discord, Model log, EntryModel entry, IUser user)
        {
            return new MessageDeleteAuditLogEntry(discord, entry, user, entry.Options.MessageDeleteChannelId.Value, entry.Options.MessageDeleteCount.Value);
        }

        public int MessageCount { get; }
        public ulong ChannelId { get; }
    }
}
