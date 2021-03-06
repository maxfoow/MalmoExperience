﻿using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using RunMission.Framework.ObservationModels;

namespace RunMission.Framework {
    public class Eyes {
        #region PreCalcul for Visions
        private static int XLeft = 5;
        private static int XRight = 5;
        private static int XLength = XLeft + XRight + 1;
        private static int ZForward = 5;
        private static int ZBackward = 5;
        private static int ZLength = ZForward + ZBackward + 1;
        private static int YBelow = 3;
        private static int YAbove = 3;
        private static int YLength = YBelow + YAbove + 1;
        #endregion

        protected readonly Body Body;
        protected ObservationModel LastObservation;

        #region Shorcut

        protected IList<NearbyEntity> NearbyEntities => LastObservation.NearbyEntities;
        protected IList<string> Visions => LastObservation.Visions;
        protected LineOfSight LineOfSight => LastObservation.LineOfSight;
        #endregion

        public Eyes(Body body) {
            Body = body;
        }

        public void Refresh(string observationText) {
            LastObservation = JsonConvert.DeserializeObject<ObservationModel>(observationText);
        }

        public bool See(string blockOrItem, string variant = null) {
            if (LastObservation == null) {
                return false;
            }

            return Visions.Any(o => o.Equals(blockOrItem))
                || NearbyEntities.Any(o => o.Name.Equals(blockOrItem));
        }

        public BlockOrItem[] WhereIs(string blockOrItem, string variant = null) {
            if (LastObservation == null) {
                return null;
            }

            var entities = (variant != null
                ? NearbyEntities.Where(o => o.Name.Equals(blockOrItem) && o.Variation.Equals(variant))
                : NearbyEntities.Where(o => o.Name.Equals(blockOrItem)))
                .Select(o => new Item(o.X, o.Y, o.Z, o.Name, o.Variation, Color.None, Facing.Unknown, o.Quantity));
            var blocks = new List<Block>();
            for (var i = 0; i < Visions.Count; i++) {
                if (Visions[i].Equals(blockOrItem)) {
                    var x = Convert.ToSingle(Math.Truncate(Body.Position.X) + i % XLength - XLeft);
                    var idx = i / XLength;
                    var z = Convert.ToSingle(Math.Truncate(Body.Position.Z) + idx % ZLength - ZBackward);
                    var y = Convert.ToSingle(Math.Truncate(Body.Position.Y) + idx / ZLength - YBelow);
                    var block = new Block(x, y, z, blockOrItem, null, Color.None, Facing.Unknown);
                    blocks.Add(block);
                }
            }
            return blocks.Concat<BlockOrItem>(entities).ToArray();
        }

        public bool IsInRangeOf(BlockOrItem blockOrItem) {
            if (LineOfSight == null) {
                return false;
            }
            return LineOfSight.Type == blockOrItem.Name
                   && Math.Truncate(LineOfSight.X) == blockOrItem.X
                   && Math.Truncate(LineOfSight.Y) == blockOrItem.Y
                   && Math.Truncate(LineOfSight.Z) == blockOrItem.Z;
        }

        public Block GetFocusedBlock() {
            if (LineOfSight == null) {
                return null;
            }
            return new Block(Convert.ToSingle(Math.Truncate(LineOfSight.X)),
                Convert.ToSingle(Math.Truncate(LineOfSight.Y)),
                Convert.ToSingle(Math.Truncate(LineOfSight.Z)), 
                LineOfSight.Type, null, Color.None, Facing.Unknown);
        }
    }
}