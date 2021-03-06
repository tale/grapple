using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework;
using SpacePotato.Source.World;

namespace SpacePotato {
    public class LevelManager {
        
        // Static level management

        public static Level[] _levels;
        public static short CurrentLevel = 1;

        public static Level level;

        public static void SetLevel(short index) {
            CurrentLevel = index;
            updateLevel();
        }

        public static void NextLevel() {
            CurrentLevel = (short) ((CurrentLevel + 1) % _levels.Length);
            updateLevel();
        }

        public static void PreviousLevel() {
            CurrentLevel--;
            if (CurrentLevel == -1) CurrentLevel = (short) (_levels.Length - 1);
            updateLevel();
        }

        public static void ResetLevel() {
            CurrentLevel = 0;
            updateLevel();
        }

        public static void updateLevel() {
            level = _levels[CurrentLevel];
            MainScreen.RecreatePlayer();
        }

        public static void LoadLevels() {
            string[] files = Directory.GetFiles(Paths.dataPath, "*.xml");

            files = files.AsEnumerable().OrderBy(f => f).ToArray();
            
            _levels = new Level[files.Length + 1];

            // random debug level
            List<Planet> level1Planets = new List<Planet>();
            for (int n = 0; n < 50; n++) {

                int planetType = Util.randInt(0, 10);
                
                if (planetType == 1) 
                    level1Planets.Add(new BlackHole(new Vector2(Util.random(-100, 5900),
                    Util.random(-1000, 1000)), 75));
                
                else if (planetType == 2)
                    level1Planets.Add(new Star(new Vector2(Util.random(-100, 5900),
                        Util.random(-1000, 1000)), 200));
                
                else
                    level1Planets.Add(new Planet(new Vector2(Util.random(-100, 5900),
                        Util.random(-1000, 1000)), 100));
            }
            
            
            _levels[0] = new Level(level1Planets, new Rectangle(-100, -1000, 6000, 2000), 1);
            
            // file loaded levels
            for (int i = 1; i <= files.Length; i++) {
                _levels[i] = DataSerializer.Deserialize<Level>(files[i-1]);
                _levels[i].setUpSerialized();
                
                Console.WriteLine(files[i - 1]);
            }

            level = _levels[CurrentLevel];
            
        }
    }
}