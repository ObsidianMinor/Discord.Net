using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Model = Discord.API.AuditLog;
using EntryModel = Discord.API.AuditLogEntry;

namespace Discord.Rest
{
    public class WebhookDeleteAuditLogEntry : RestAuditLogEntry
    {
        private WebhookDeleteAuditLogEntry(BaseDiscordClient discord, EntryModel model, IUser user, ulong id, ulong channel, WebhookType type, string name, string avatar) : base(discord, model, user)
        {
            WebhookId = id;
            ChannelId = channel;
            Name = name;
            Type = type;
            Avatar = avatar;
        }

        internal static WebhookDeleteAuditLogEntry Create(BaseDiscordClient discord, Model log, EntryModel entry, IUser user)
        {
            var changes = entry.Changes;

            var channelIdModel = changes.FirstOrDefault(x => x.ChangedProperty == "channel_id");
            var typeModel = changes.FirstOrDefault(x => x.ChangedProperty == "type");
            var nameModel = changes.FirstOrDefault(x => x.ChangedProperty == "name");
            var avatarHashModel = changes.FirstOrDefault(x => x.ChangedProperty == "avatar_hash");

            var channelId = channelIdModel.OldValue.ToObject<ulong>();
            var type = typeModel.OldValue.ToObject<WebhookType>();
            var name = nameModel.OldValue.ToObject<string>();
            var avatarHash = avatarHashModel?.OldValue?.ToObject<string>();

            return new WebhookDeleteAuditLogEntry(discord, entry, user, entry.TargetId.Value, channelId, type, name, avatarHash);
        }

        public ulong WebhookId { get; }
        public ulong ChannelId { get; }
        public WebhookType Type { get; }
        public string Name { get; }
        public string Avatar { get; }
    }
}
