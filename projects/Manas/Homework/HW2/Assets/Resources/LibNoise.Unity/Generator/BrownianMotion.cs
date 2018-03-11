using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//using Microsoft.Xna.Framework;
using UnityEngine;

namespace LibNoise.Unity.Generator
{
    /// <summary>
    /// Provides a noise module that outputs 3-dimensional ridged-multifractal noise. [GENERATOR]
    /// </summary>
    public class BrownianMotion : ModuleBase
    {
        #region Fields

        private double m_frequency = 1.0;
        private double m_lacunarity = 2.0;
        private QualityMode m_quality = QualityMode.Medium;
        private int m_octaveCount = 6;
        private int m_seed = 0;
        private double[] m_weights = new double[Utils.OctavesMaximum];		
		
        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of RiggedMultifractal.
        /// </summary>
        public BrownianMotion()
            : base(0)
        {
            this.UpdateWeights();
        }

        /// <summary>
        /// Initializes a new instance of RiggedMultifractal.
        /// </summary>
        /// <param name="frequency">The frequency of the first octave.</param>
        /// <param name="lacunarity">The lacunarity of the ridged-multifractal noise.</param>
        /// <param name="octaves">The number of octaves of the ridged-multifractal noise.</param>
        /// <param name="seed">The seed of the ridged-multifractal noise.</param>
        /// <param name="quality">The quality of the ridged-multifractal noise.</param>
        public BrownianMotion(double frequency, double lacunarity, int octaves, int seed, QualityMode quality)
            : base(0)
        {
            this.Frequency = frequency;
            this.Lacunarity = lacunarity;
            this.OctaveCount = octaves;
            this.Seed = seed;
            this.Quality = quality;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the frequency of the first octave.
        /// </summary>
        public double Frequency
        {
            get { return this.m_frequency; }
            set { this.m_frequency = value; }
        }

        /// <summary>
        /// Gets or sets the lacunarity of the ridged-multifractal noise.
        /// </summary>
        public double Lacunarity
        {
            get { return this.m_lacunarity; }
            set
            {
                this.m_lacunarity = value;
                this.UpdateWeights();
            }
        }

        /// <summary>
        /// Gets or sets the quality of the ridged-multifractal noise.
        /// </summary>
        public QualityMode Quality
        {
            get { return this.m_quality; }
            set { this.m_quality = value; }
        }

        /// <summary>
        /// Gets or sets the number of octaves of the ridged-multifractal noise.
        /// </summary>
        public int OctaveCount
        {
            get { return this.m_octaveCount; }
            set { this.m_octaveCount = (int)Mathf.Clamp(value, 1, Utils.OctavesMaximum); }
        }

        /// <summary>
        /// Gets or sets the seed of the ridged-multifractal noise.
        /// </summary>
        public int Seed
        {
            get { return this.m_seed; }
            set { this.m_seed = value; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Updates the weights of the ridged-multifractal noise.
        /// </summary>
        private void UpdateWeights()
        {
            double f = 1.0;
            for (int i = 0; i < Utils.OctavesMaximum; i++)
            {
                this.m_weights[i] = Math.Pow(f, -1.0);
                f *= this.m_lacunarity;
            }
        }

        #endregion

        #region ModuleBase Members

        /// <summary>
        /// Returns the output value for the given input coordinates.
        /// </summary>
        /// <param name="x">The input coordinate on the x-axis.</param>
        /// <param name="y">The input coordinate on the y-axis.</param>
        /// <param name="z">The input coordinate on the z-axis.</param>
        /// <returns>The resulting output value.</returns>
        public override double GetValue(double x, double y, double z)
        {
                        float signal;
                        float value;
                        int curOctave;

                        x *= Frequency;
                        y *= Frequency;
                        z *= Frequency;

                        // Initialize value, fBM starts with 0
                        value = 0;
			
						ModuleBase tmpperl = new Perlin(Frequency,Lacunarity,0.5,OctaveCount,Seed,QualityMode.Medium);
			
                        // Inner loop of spectral construction, where the fractal is built
                        for(curOctave = 0; curOctave < OctaveCount; curOctave++) {

                                // Get the coherent-noise value.
                                signal = (float)tmpperl.GetValue(x, y, z) * (float)m_weights[curOctave];

                                // Add the signal to the output value.
                                value += signal;

                                // Go to the next octave.
                                x *= Lacunarity;
                                y *= Lacunarity;
                                z *= Lacunarity;

                        }//end for

                        //take care of remainder in _octaveCount
                        float remainder = OctaveCount - (int)OctaveCount;
                        if(remainder > 0.0f) {
                                value += remainder * (float)tmpperl.GetValue(x, y, z) * (float)m_weights[curOctave];
                        }//end if

                        return value;
        }

        #endregion
    }
}