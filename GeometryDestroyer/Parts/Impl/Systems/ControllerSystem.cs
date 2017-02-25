using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace GeometryDestroyer.Parts.Impl.Systems
{
    /// <summary>
    /// Defines the system that can be used to retrieve the connected controllers.
    /// </summary>
    public class ControllerSystem : IControllerSystem
    {
        private readonly Dictionary<PlayerIndex, GameController> controllerMap = new Dictionary<PlayerIndex, GameController>();

        /// <inheritdoc />
        public IEnumerable<GameController> GetControllers() => this.GetControllers(forceUpdate: false);

        /// <inheritdoc />
        public IEnumerable<GameController> GetControllers(bool forceUpdate)
        {
            for (PlayerIndex i = PlayerIndex.One; i <= PlayerIndex.Four; i++)
            {
                if (GamePad.GetCapabilities(i).IsConnected)
                {
                    if (this.controllerMap.ContainsKey(i) == false)
                    {
                        this.controllerMap.Add(i, new GameController(i));
                    }
                }
                else
                {
                    if (this.controllerMap.ContainsKey(i))
                    {
                        this.controllerMap.Remove(i);
                    }
                }
            }

            if (forceUpdate)
            {
                foreach (var controller in this.controllerMap.Values)
                {
                    controller.Update();
                }
            }

            return this.controllerMap.Values;
        }
    }
}