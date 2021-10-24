using System;
using System.Collections.Generic;
using System.IO;

using Rewired;

using UnityEngine;

namespace CatsAreBuggy.Fixes {
    // ReSharper disable once UnusedType.Global
    internal class KeybindsFix : BaseFix {
        public KeybindsFix() : base("Fix the control map not loading on startup.") { }

        // lqd you forgor 💀 to load keybinds on startup lol
        public override void Apply() => On.TitleScreen.Awake += (orig, self) => {
            orig(self);
            if(enabled) LoadControlMap();
        };

        // literally copy-pasted from ControlRemapper.LoadControlMap but without the InitializeUI part lmao
        private static void LoadControlMap() {
            string path = Path.Combine(Application.persistentDataPath, "controls.prefs");
            if(!File.Exists(path)) return;
            foreach(string line in File.ReadAllLines(path)) {
                ControllerType controllerType;
                if(line.IndexOf("JoystickMap", StringComparison.Ordinal) != -1)
                    controllerType = ControllerType.Joystick;
                else if(line.IndexOf("KeyboardMap", StringComparison.Ordinal) != -1)
                    controllerType = ControllerType.Keyboard;
                else continue;
                ReInput.players.GetPlayer(0).controllers.maps.AddMapsFromXml(controllerType, 0, new List<string> {
                    line
                });
            }
            DisableTabPause();
        }

        private static void DisableTabPause() {
            foreach(ControllerMap map in ReInput.players.GetPlayer(0).controllers.maps.GetAllMaps()) {
                foreach(ActionElementMap actionElementMap in map.ButtonMapsWithAction("Pause")) {
                    if(actionElementMap.keyboardKeyCode != KeyboardKeyCode.Tab) continue;
                    actionElementMap.enabled = false;
                    return;
                }
            }
        }
    }
}
