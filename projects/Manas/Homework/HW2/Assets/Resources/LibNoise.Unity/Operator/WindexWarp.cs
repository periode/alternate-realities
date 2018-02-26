using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LibNoise.Unity.Operator
{
    /// <summary>
    /// Provides a noise module that clamps the output value from a source module to a
    /// range of values. [OPERATOR]
    /// </summary>
    public class WindexWarp : ModuleBase
    {

        #region Constructors

        /// <summary>
        /// Initializes a new instance of Clamp.
        /// </summary>
        public WindexWarp()
            : base(1)
        {
        }

        /// <summary>
        /// Initializes a new instance of WindexWarp.
        /// </summary>
        /// <param name="input">The input module.</param>
        /// <param name="min">The minimum value.</param>
        /// <param name="max">The maximum value.</param>
        public WindexWarp(ModuleBase input)
            : base(1)
        {
            this.m_modules[0] = input;
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
            System.Diagnostics.Debug.Assert(this.m_modules[0] != null);
            double v = (this.m_modules[0].GetValue(x+1, y+1, z)+this.m_modules[0].GetValue(x+1, y, z)+this.m_modules[0].GetValue(x, y+1, z))*0.33;
             
            return v;
        }

        #endregion
    }
}