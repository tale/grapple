
  using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
  using SpacePotato.Source.Util;


  namespace SpacePotato {
    public class ParallaxLayer {

        private static readonly Random Random = new Random();
        
        private readonly int _scaler, _density;

        private readonly Rectangle _bounds;

        private bool _starLayer;
        
        public Image[] ParallaxImages { get; }

        private static readonly Texture2D[] Textures;
        private static readonly Texture2D SmallStar, BigStar, Blackhole;

        static ParallaxLayer() {
            Textures = new[] {
                Loader.texture("Common/Planet1"),
                Loader.texture("Common/Planet2"),
                Loader.texture("Common/Planet3"),
                Loader.texture("Common/Planet4"),
                Loader.texture("Common/Planet5"),
                Loader.texture("Common/Planet6"),
            };

            SmallStar = Loader.texture("Common/Star");
            BigStar = Loader.texture("Common/Sun");
            Blackhole = Loader.texture("Common/Blackhole");
        }
        public ParallaxLayer(Rectangle bounds, int scaler, int density, bool starLayer = false) {
            
            _scaler = scaler;
            _density = density;
            _bounds = bounds;
            _starLayer = starLayer;

            ParallaxImages = GeneratePlanetImageMap();
        }

        private Image[] GeneratePlanetImageMap() {

            var array = new Image[_density];

            int iteration = 1000000;
            int number = 0;

            List<(int, int)> previous = new List<(int, int)>();

            do {

                var x = Random.Next(_bounds.Width);
                var y = Random.Next(_bounds.Height);

                int text = (int) (Random.NextDouble() * 6);
                Texture2D image = Textures[text];

                int planetType = Util.randInt(0, 10);

                float radius = 84;

                if (planetType == 0) {
                    radius = 64;
                    image = Blackhole;
                }

                else if (planetType == 1) {
                    radius = 196;
                    image = BigStar;
                }

                if (CheckPrevious(x, y, previous)) {
                    
                    if (!_starLayer)
                        array[number] = new Image(_bounds.Left + x, _bounds.Top + y, 
                            radius / _scaler, radius / _scaler, image);
                    else
                        array[number] = new Image(_bounds.Left + x, _bounds.Top + y, 
                            40f / _scaler, 40f / _scaler, SmallStar);

                    previous.Add((x, y));
                    number++;
                }

                iteration--;

            } while (iteration > 0 && number < _density);

            return array;


        }


        private bool CheckPrevious(int x, int y, List<(int, int)> previous) {

            foreach ((int, int) prev in previous) {

                var (x2, y2) = prev;

                if (Math.Sqrt(Math.Pow(x - x2, 2) + Math.Pow(y - y2, 2)) < 300.0 / _scaler)
                    return false;
            }
            return true;
        }


        public void Render(SpriteBatch spritebatch) {

            foreach (var image in ParallaxImages) {
                image?.Render(spritebatch, scaler:_scaler);
            }
        }
    }
}
