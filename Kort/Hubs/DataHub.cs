using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kort.Hubs
{
		public class DataHub : Hub
		{
				public async Task SyncRr(int AaID, DateTime Trh)
				{
						await Clients.Others.SendAsync("SyncRr", AaID, Trh);
        }

        public async Task GroupSyncRr(string groupName, DateTime Trh)
        {
            await Clients.OthersInGroup(groupName).SendAsync("GroupSyncRr", Trh);
            Console.WriteLine($"GroupSyncRr: {groupName} {Context.ConnectionId} {Trh}");
        }

        public async Task AddToGroup(string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
            Console.WriteLine($"AddToGroup: {groupName} {Context.ConnectionId}");

            //await Clients.OthersInGroup(groupName).SendAsync("SyncR", $"{Context.ConnectionId} has joined the group {groupName}.");
        }

        public async Task RemoveFromGroup(string groupName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
            Console.WriteLine($"RemoveFromGroup: {groupName} {Context.ConnectionId}");
        }

        public override Task OnConnectedAsync()
        {
            Console.WriteLine($"{Context.ConnectionId} connected");
            return base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception e)
        {
            Console.WriteLine($"Disconnected {e?.Message} {Context.ConnectionId}");
            await base.OnDisconnectedAsync(e);
        }
    }
}
