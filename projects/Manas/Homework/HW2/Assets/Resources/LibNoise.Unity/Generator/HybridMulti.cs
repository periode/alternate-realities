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
    public class HybridMulti : ModuleBase
    {
        #region Fields

        private double m_frequency = 1.0;
        private double m_offset = 0.5;
        private double m_lacunarity = 2.0;
		private double m_gain = 0.5;
        private QualityMode m_quality = QualityMode.Medium;
        private int m_octaveCount = 6;
        private int m_seed = 0;
        private double[] m_weights = new double[Utils.OctavesMaximum];
		private double m_persistence = 0.5;
        
        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of RiggedMultifractal.
        /// </summary>
        public HybridMulti()
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
        public HybridMulti(double frequency, double lacunarity, int octaves, double persistence, int seed, double offset, double gain, QualityMode quality)
            : base(0)
        {
            this.Frequency = frequency;
            this.Lacunarity = lacunarity;
            this.OctaveCount = octaves;
            this.Seed = seed;
			this.Offset = offset;
            this.Persistence = persistence;
			this.Quality = quality;
			this.Gain = gain;
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
        /// Gets or sets the persistence of the perlin noise.
        /// </summary>
        public double Persistence
        {
            get { return this.m_persistence; }
            set { this.m_persistence = value; }
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

		/// <summary>
        /// Gets or sets the seed of the multifractal offset.
        /// </summary>
        public double Offset
        {
            get { return this.m_offset; }
            set { this.m_offset = value; }
        }

		/// <summary>
        /// Gets or sets the seed of the ridged-multifractal noise.
        /// </summary>
        public double Gain
        {
            get { return this.m_gain; }
            set { this.m_gain = value; }
        }
		
		
        #endregion

        #region Methods

        /// <summary>
        /// Updates the weights of the ridged-multifractal noise.
        /// </summary>
        private void UpdateWeights()
        {
            for (int i = 0; i < Utils.OctavesMaximum; i++)
            {
                this.m_weights[i] = Math.Pow(this.m_lacunarity, (float)-i * 0.25f);
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
                        float weight;
						int curOctave;

						ModuleBase tmpperl = new Perlin(Frequency,Lacunarity,Persistence,OctaveCount,Seed,QualityMode.Medium);
			
                        x *= this.m_frequency;
			            y *= this.m_frequency;
			            z *= this.m_frequency;

                        // Initialize value : first unscaled octave of function; later octaves are scaled 
                        value = (float)m_offset + (float)Utils.GradientCoherentNoise3D(x, y, z, m_seed, this.m_quality);
						weight = (float)m_gain * value;
			
                        x *= this.m_lacunarity;
                        y *= this.m_lacunarity;
                        z *= this.m_lacunarity;

                        // inner loop of spectral construction, where the fractal is built
                        for(curOctave = 1; weight> 0.001f && curOctave < m_octaveCount; curOctave++) {
				
						if(weight>1f) {
							weight = 1f;
						}
                        // obtain displaced noise value.
                        signal = (float)m_offset + (float)Utils.GradientCoherentNoise3D(x, y, z, m_seed, this.m_quality);
                        
                        //scale amplitude appropriately for this frequency 
                        signal *= (float)m_weights[curOctave];

                        // scale increment by current altitude of function
                        signal *= value;

                        // Add the signal to the output value.
                        value += signal;
				
						// update the (monotonically decreasing) weighting value
                        weight *= (float)m_gain * signal;


				
                        // Go to the next octave.
                        x *= m_lacunarity;
                        y *= m_lacunarity;
                        z *= m_lacunarity;

                        }//end for

                        //take care of remainder in _octaveCount
                        float remainder = m_octaveCount - (int)m_octaveCount;

                        if(remainder > 0.0f) {
                                signal = (float)tmpperl.GetValue(x, y, z);
                                signal *= (float)m_weights[curOctave];
                                signal *= value;
                                signal *= remainder;
                                value +=  signal;
                        }//end if

                        return value;

        }

        #endregion
    }
}