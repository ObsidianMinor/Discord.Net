﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Discord.WebSocket
{
    internal class ClientState
    {
        private const double AverageChannelsPerGuild = 10.22; //Source: Googie2149
        private const double AverageUsersPerGuild = 47.78; //Source: Googie2149
        private const double CollectionMultiplier = 1.05; //Add 5% buffer to handle growth
        private const double AverageRelationshipsPerUser = 30;

        private readonly ConcurrentDictionary<ulong, SocketChannel> _channels;
        private readonly ConcurrentDictionary<ulong, SocketDMChannel> _dmChannels;
        private readonly ConcurrentDictionary<ulong, SocketGuild> _guilds;
        private readonly ConcurrentDictionary<ulong, SocketGlobalUser> _users;
        private readonly ConcurrentDictionary<ulong, SocketRelationship> _relationships;
        private readonly ConcurrentHashSet<ulong> _groupChannels;

        internal IReadOnlyCollection<SocketChannel> Channels => _channels.ToReadOnlyCollection();
        internal IReadOnlyCollection<SocketDMChannel> DMChannels => _dmChannels.ToReadOnlyCollection();
        internal IReadOnlyCollection<SocketGroupChannel> GroupChannels => _groupChannels.Select(x => GetChannel(x) as SocketGroupChannel).ToReadOnlyCollection(_groupChannels);
        internal IReadOnlyCollection<SocketGuild> Guilds => _guilds.ToReadOnlyCollection();
        internal IReadOnlyCollection<SocketGlobalUser> Users => _users.ToReadOnlyCollection();
        internal IReadOnlyCollection<SocketRelationship> Relationships => _relationships.ToReadOnlyCollection();

        internal IReadOnlyCollection<ISocketPrivateChannel> PrivateChannels =>
            _dmChannels.Select(x => x.Value as ISocketPrivateChannel).Concat(
                _groupChannels.Select(x => GetChannel(x) as ISocketPrivateChannel))
            .ToReadOnlyCollection(() => _dmChannels.Count + _groupChannels.Count);

        public ClientState(int guildCount, int dmChannelCount)
        {
            double estimatedChannelCount = guildCount * AverageChannelsPerGuild + dmChannelCount;
            double estimatedUsersCount = guildCount * AverageUsersPerGuild;
            _channels = new ConcurrentDictionary<ulong, SocketChannel>(ConcurrentHashSet.DefaultConcurrencyLevel, (int)(estimatedChannelCount * CollectionMultiplier));
            _dmChannels = new ConcurrentDictionary<ulong, SocketDMChannel>(ConcurrentHashSet.DefaultConcurrencyLevel, (int)(dmChannelCount * CollectionMultiplier));
            _guilds = new ConcurrentDictionary<ulong, SocketGuild>(ConcurrentHashSet.DefaultConcurrencyLevel, (int)(guildCount * CollectionMultiplier));
            _users = new ConcurrentDictionary<ulong, SocketGlobalUser>(ConcurrentHashSet.DefaultConcurrencyLevel, (int)(estimatedUsersCount * CollectionMultiplier));
            _relationships = new ConcurrentDictionary<ulong, SocketRelationship>(ConcurrentHashSet.DefaultConcurrencyLevel, (int)(AverageRelationshipsPerUser * CollectionMultiplier));
            _groupChannels = new ConcurrentHashSet<ulong>(ConcurrentHashSet.DefaultConcurrencyLevel, (int)(10 * CollectionMultiplier));
        }

        internal SocketChannel GetChannel(ulong id)
        {
            SocketChannel channel;
            if (_channels.TryGetValue(id, out channel))
                return channel;
            return null;
        }
        internal SocketDMChannel GetDMChannel(ulong userId)
        {
            SocketDMChannel channel;
            if (_dmChannels.TryGetValue(userId, out channel))
                return channel;
            return null;
        }
        internal void AddChannel(SocketChannel channel)
        {
            _channels[channel.Id] = channel;

            var dmChannel = channel as SocketDMChannel;
            if (dmChannel != null)
                _dmChannels[dmChannel.Recipient.Id] = dmChannel;
            else
            {
                var groupChannel = channel as SocketGroupChannel;
                if (groupChannel != null)
                    _groupChannels.TryAdd(groupChannel.Id);
            }
        }
        internal SocketChannel RemoveChannel(ulong id)
        {
            SocketChannel channel;
            if (_channels.TryRemove(id, out channel))
            {
                var dmChannel = channel as SocketDMChannel;
                if (dmChannel != null)
                {
                    SocketDMChannel ignored;
                    _dmChannels.TryRemove(dmChannel.Recipient.Id, out ignored);
                }
                else
                {
                    var groupChannel = channel as SocketGroupChannel;
                    if (groupChannel != null)
                        _groupChannels.TryRemove(id);
                }
                return channel;
            }
            return null;
        }

        internal SocketGuild GetGuild(ulong id)
        {
            SocketGuild guild;
            if (_guilds.TryGetValue(id, out guild))
                return guild;
            return null;
        }
        internal void AddGuild(SocketGuild guild)
        {
            _guilds[guild.Id] = guild;
        }
        internal SocketGuild RemoveGuild(ulong id)
        {
            SocketGuild guild;
            if (_guilds.TryRemove(id, out guild))
                return guild;
            return null;
        }

        internal SocketGlobalUser GetUser(ulong id)
        {
            SocketGlobalUser user;
            if (_users.TryGetValue(id, out user))
                return user;
            return null;
        }
        internal SocketGlobalUser GetOrAddUser(ulong id, Func<ulong, SocketGlobalUser> userFactory)
        {
            return _users.GetOrAdd(id, userFactory);
        }
        internal SocketGlobalUser RemoveUser(ulong id)
        {
            SocketGlobalUser user;
            if (_users.TryRemove(id, out user))
                return user;
            return null;
        }

        internal SocketRelationship GetRelationship(ulong id)
        {
            if (_relationships.TryGetValue(id, out SocketRelationship value))
                return value;
            return null;
        }
        internal void AddRelationship(SocketRelationship relationship)
        {
            _relationships[relationship.User.Id] = relationship;
        }
        internal SocketRelationship RemoveRelationship(ulong id)
        {
            if (_relationships.TryRemove(id, out SocketRelationship value))
                return value;
            return null;
        }
    }
}
