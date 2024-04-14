using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Perlin
{
    public static float Noise3D(float x, float y, float z, float seed, PerlinSettings settings)
    {
        float noise = 0.0f;

        for (int i = 0; i < settings.octave; ++i)
        {
            // Get all permutations of noise for each individual axis
            float noiseXY = Mathf.PerlinNoise(x * settings.frequency + seed,
                                              y * settings.frequency + seed)
                                              * settings.amplitude;
            float noiseXZ = Mathf.PerlinNoise(x * settings.frequency + seed,
                                              z * settings.frequency + seed)
                                              * settings.amplitude;
            float noiseYZ = Mathf.PerlinNoise(y * settings.frequency + seed,
                                              z * settings.frequency + seed)
                                              * settings.amplitude;
            // Use the average of the noise functions
            noise += (noiseXY + noiseXZ + noiseYZ) / 3.0f;

            settings.amplitude *= settings.persistence;
            settings.frequency *= 2.0f;
        }

        // Use the average of all octaves
        noise = noise / settings.octave;

        return noise;
    }
}
