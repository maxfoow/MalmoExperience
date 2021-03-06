﻿using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RunMission.Framework {
    public class Brain {
        protected readonly Body Body;
        protected readonly Eyes Eyes;
        private Task _brainActivity;
	    private CancellationTokenSource _cancellationTokenSource;

	    public Brain(Body body, Eyes eyes) {
            Body = body;
            Eyes = eyes;
        }

        public void StartToThink() {
	        _cancellationTokenSource = new CancellationTokenSource();
	        _brainActivity = Task.Run(DeepThought, _cancellationTokenSource.Token);
        }

        public async Task StopThinking() {
			_cancellationTokenSource.Cancel();
	        await _brainActivity;
        }

        private async Task DeepThought() {
            while (Body.IsOnMission) {
	            if (_cancellationTokenSource.Token.IsCancellationRequested) {
		            return;
	            }
                Console.WriteLine("Looking for wood");
                while (!Eyes.See("log")) {
                    Thread.Sleep(100);
                }
                // Go to wood
                if (Eyes.See("log")) {
                    var pickaxe = Body.Inventory.Get("diamond_pickaxe");
                    Body.Inventory.SelectItem(pickaxe);
                    var woods = Eyes.WhereIs("log");
                    var wood = woods.FirstOrDefault(o => o.Y == Body.Position.Y + 1.0);
                    Console.WriteLine($"Go to: {wood}");
                    await Body.GoInRangeOf(wood);
                    Console.WriteLine("wood in range :)");
                    await Body.DestroyFocus();
                    Console.WriteLine("wood destroy");
                    return;
                }
                Thread.Sleep(1000000);
            }
	        return;
        }

        private async Task PickUpItem(Item item) {
            await Body.GoTo(item);
        }
    }
}