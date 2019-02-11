using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NetCoreBirdAPI.Algorithms
{
    class KekMeans
    {
        private static readonly int MAX_ITERATIONS = 200;

        /// <summary>
        /// Clusters the data into a predefined amount of clusters and returns the means as an array.
        /// </summary>
        public static double[][] Cluster(double[][] data, int clusterAmount)
        {
            if (clusterAmount < 2)
            {
                clusterAmount = 2;
            }
            if (clusterAmount > data.Length)
            {
                return data;
            }

            bool clustersMoved = true;
            bool isValid = true;

            int[] currentClusters = Initialize(data.Length, clusterAmount);

            double[][] currentMeans = new double[clusterAmount][];
            for (int k = 0; k < clusterAmount; ++k)
            {
                currentMeans[k] = new double[clusterAmount];
            }

            int iterations = 0;
            while (clustersMoved == true && isValid == true && iterations < MAX_ITERATIONS)
            {
                ++iterations;
                isValid = UpdateMeans(data, currentClusters, currentMeans);
                clustersMoved = UpdateClustering(data, currentClusters, currentMeans);
            }

            return currentMeans;
        }

        private static int[] Initialize(int dataCount, int clusterAmount)
        {
            Random random = new Random();
            int[] startClusters = new int[dataCount];
            for (int i = 0; i < clusterAmount; ++i)
            {
                startClusters[i] = i;
            }
            for (int i = clusterAmount; i < startClusters.Length; ++i)
            {
                startClusters[i] = random.Next(0, clusterAmount);
            }
            return startClusters;
        }

        private static bool UpdateMeans(double[][] data, int[] clustering, double[][] means)
        {
            int numClusters = means.Length;
            int[] clusterCounts = new int[numClusters];

            for (int i = 0; i < data.Length; ++i)
            {
                int cluster = clustering[i];
                ++clusterCounts[cluster];
            }

            for (int k = 0; k < numClusters; ++k)
            {
                if (clusterCounts[k] == 0)
                {
                    return false;
                }
            }

            for (int k = 0; k < means.Length; ++k)
            {
                for (int j = 0; j < means[k].Length; ++j)
                {
                    means[k][j] = 0.0;
                }
            }

            for (int i = 0; i < data.Length; ++i)
            {
                int cluster = clustering[i];
                for (int j = 0; j < data[i].Length; ++j)
                {
                    means[cluster][j] += data[i][j];
                }
            }

            for (int k = 0; k < means.Length; ++k)
            {
                for (int j = 0; j < means[k].Length; ++j)
                {
                    means[k][j] /= clusterCounts[k];
                }
            }
            return true;
        }

        private static bool UpdateClustering(double[][] data, int[] clustering, double[][] means)
        {
            int clusterAmount = means.Length;
            bool changed = false;

            int[] newClustering = new int[clustering.Length];
            Array.Copy(clustering, newClustering, clustering.Length);

            double[] distances = new double[clusterAmount];

            for (int i = 0; i < data.Length; ++i)
            {
                for (int k = 0; k < clusterAmount; ++k)
                {
                    distances[k] = CalculateDistance(data[i], means[k]);
                }

                int newCluster = FindMinIndex(distances);
                if (newCluster != newClustering[i])
                {
                    changed = true;
                    newClustering[i] = newCluster;
                }
            }

            if (changed == false)
            {
                return false;
            }


            int[] clusterCounts = new int[clusterAmount];
            for (int i = 0; i < data.Length; ++i)
            {
                int cluster = newClustering[i];
                ++clusterCounts[cluster];
            }

            for (int k = 0; k < clusterAmount; ++k)
            {
                if (clusterCounts[k] == 0)
                {
                    return false;
                }
            }

            Array.Copy(newClustering, clustering, newClustering.Length);

            return true;
        }

        private static double CalculateDistance(double[] dataPoint, double[] otherDataPoint)
        {
            double difference = 0.0;

            for (int j = 0; j < dataPoint.Length; ++j)
            {
                difference += Math.Pow((dataPoint[j] - otherDataPoint[j]), 2);
            }

            return Math.Sqrt(difference);
        }

        private static int FindMinIndex(double[] distances)
        {
            int indexOfMin = 0;
            double smallDist = distances[0];

            for (int k = 0; k < distances.Length; ++k)
            {
                if (distances[k] < smallDist)
                {
                    smallDist = distances[k];
                    indexOfMin = k;
                }
            }

            return indexOfMin;
        }
    }
}