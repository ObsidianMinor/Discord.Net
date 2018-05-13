using System;

using Model = Discord.API.AuditLog;
using EntryModel = Discord.API.AuditLogEntry;

namespace Discord.Rest
{
    public abstract class RestAuditLogEntry : RestEntity<ulong>, IAuditLogEntry
    {
        internal RestAuditLogEntry(BaseDiscordClient discord, EntryModel model, IUser user)
            : base(discord, model.Id)
        {
            Action = model.Action;
            User = user;
            Reason = model.Reason;
        }

        /// <inheritdoc/>
        public ActionType Action { get; }
        /// <inheritdoc/>
        public IUser User { get; }
        /// <inheritdoc/>
        public string Reason { get; }

        internal static RestAuditLogEntry CreateEntry(BaseDiscordClient discord, EntryModel entry, Model model, IUser user)
        {
            Func<BaseDiscordClient, Model, EntryModel, IUser, RestAuditLogEntry> GetFactory(ActionType type)
            {
                switch (type)
                {
                    case ActionType.Ban: return BanAuditLogEntry.Create;
                    case ActionType.ChannelCreated: return ChannelCreateAuditLogEntry.Create;
                    case ActionType.ChannelDeleted: return ChannelDeleteAuditLogEntry.Create;
                    case ActionType.ChannelUpdated: return ChannelUpdateAuditLogEntry.Create;
                    case ActionType.EmojiCreated: return EmoteCreateAuditLogEntry.Create;
                    case ActionType.EmojiDeleted: return EmoteDeleteAuditLogEntry.Create;
                    case ActionType.EmojiUpdated: return EmoteUpdateAuditLogEntry.Create;
                    case ActionType.GuildUpdated: return GuildUpdateAuditLogEntry.Create;
                    case ActionType.InviteCreated: return InviteCreateAuditLogEntry.Create;
                    case ActionType.InviteDeleted: return InviteDeleteAuditLogEntry.Create;
                    case ActionType.InviteUpdated: return InviteUpdateAuditLogEntry.Create;
                    case ActionType.Kick: return KickAuditLogEntry.Create;
                    case ActionType.MemberRoleUpdated: return MemberRoleAuditLogEntry.Create;
                    case ActionType.MemberUpdated: return MemberUpdateAuditLogEntry.Create;
                    case ActionType.MessageDeleted: return MessageDeleteAuditLogEntry.Create;
                    case ActionType.OverwriteCreated: return OverwriteCreateAuditLogEntry.Create;
                    case ActionType.OverwriteDeleted: return OverwriteDeleteAuditLogEntry.Create;
                    case ActionType.OverwriteUpdated: return OverwriteUpdateAuditLogEntry.Create;
                    case ActionType.Prune: return PruneAuditLogEntry.Create;
                    case ActionType.RoleCreated: return RoleCreateAuditLogEntry.Create;
                    case ActionType.RoleDeleted: return RoleDeleteAuditLogEntry.Create;
                    case ActionType.RoleUpdated: return RoleUpdateAuditLogEntry.Create;
                    case ActionType.Unban: return UnbanAuditLogEntry.Create;
                    case ActionType.WebhookCreated: return WebhookCreateAuditLogEntry.Create;
                    case ActionType.WebhookDeleted: return WebhookDeleteAuditLogEntry.Create;
                    case ActionType.WebhookUpdated: return WebhookUpdateAuditLogEntry.Create;
                    default: return UnknownAuditLogEntry.Create;
                }
            }

            return GetFactory(entry.Action)(discord, model, entry, user);
        }
    }
}
