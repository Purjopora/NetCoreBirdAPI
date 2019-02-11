using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NetCoreBirdAPI.Common;
namespace NetCoreBirdAPI.Algorithms
{
    public static class KekMeansLocationProviderAdapter
    {
        public static readonly int DEFAULT_CLUSTER_AMOUNT = 100;

        private static readonly double START_PARTITION_LONG = 19.5;
        private static readonly double END_PARTITION_LONG = 31.5;
        private static readonly int MAP_PARTITIONS_AMOUNT_LONG = 15;
        private static readonly double PARTITIONS_SIZE_LONG = (END_PARTITION_LONG - START_PARTITION_LONG) / MAP_PARTITIONS_AMOUNT_LONG;

        private static readonly double START_PARTITION_LAT = 60;
        private static readonly double END_PARTITION_LAT = 70;
        private static readonly int MAP_PARTITIONS_AMOUNT_LAT = 10;
        private static readonly double PARTITIONS_SIZE_LAT = (END_PARTITION_LAT - START_PARTITION_LAT) / MAP_PARTITIONS_AMOUNT_LAT;

        private static readonly int MIN_CLUSTERS_PER_PARTITION = 1;
        private static readonly double MIN_FRACTION_COUNT = 0.008;

        //TODO: unnecessary and inefficient transformations? List vs array speed in algorithm?? 

        public static double[][] ClusterPartitions(List<LocationProvider> locations, int clusterAmount)
        {
            double[][] clusters = new double[clusterAmount + (MAP_PARTITIONS_AMOUNT_LAT * MAP_PARTITIONS_AMOUNT_LONG) * MIN_CLUSTERS_PER_PARTITION][];
            int totalCount = locations.Count;


            double[][][][] partitionedLocations = Partition(locations);

            int clusterIndex = 0;
            for (int i = 0; i < partitionedLocations.Length; i++)
            {
                for (int j = 0; j < partitionedLocations[i].Length; j++)
                {
                    int count = partitionedLocations[i][j].Length;
                    double partitionCountFraction = ((double)count) / (double)totalCount;
                    if (partitionCountFraction > MIN_FRACTION_COUNT)
                    {
                        int clustersInPartition = (int)Math.Round((double)count * clusterAmount / totalCount, 0);
                        if (clustersInPartition < MIN_CLUSTERS_PER_PARTITION)
                        {
                            clustersInPartition = MIN_CLUSTERS_PER_PARTITION;
                        }
                        if (count < clustersInPartition)
                        {
                            if (count > MIN_CLUSTERS_PER_PARTITION)
                            {
                                clustersInPartition = count;
                            }
                            else
                            {
                                continue;
                            }
                        }
                        double[][] partitionClusters = KekMeans.Cluster(partitionedLocations[i][j], clustersInPartition);
                        for (int k = 0; k < partitionClusters.Length; k++)
                        {
                            clusters[clusterIndex] = partitionClusters[k];
                            clusterIndex++;
                        }
                    }
                }
            }

            double[][] result = new double[clusterIndex][];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = clusters[i];
            }
            return result;
        }

        private static double[][][][] Partition(List<LocationProvider> locations)
        {
            double[][][][] partitionedLocations = new double[MAP_PARTITIONS_AMOUNT_LAT][][][];
            for (int i = 0; i < MAP_PARTITIONS_AMOUNT_LAT; i++)
            {
                partitionedLocations[i] = new double[MAP_PARTITIONS_AMOUNT_LONG][][];
            }
            for (int i = 0; i < MAP_PARTITIONS_AMOUNT_LAT; i++)
            {
                for (int j = 0; j < MAP_PARTITIONS_AMOUNT_LONG; j++)
                {
                    List<LocationProvider> partitionLocations = new List<LocationProvider>();
                    foreach (LocationProvider loc in locations)
                    {
                        if (loc.getLatitude() > START_PARTITION_LAT + i * PARTITIONS_SIZE_LAT && loc.getLatitude() < START_PARTITION_LAT + (i + 1) * PARTITIONS_SIZE_LAT)
                        {
                            if (loc.getLongitude() > START_PARTITION_LONG + j * PARTITIONS_SIZE_LONG && loc.getLongitude() < START_PARTITION_LONG + (j + 1) * PARTITIONS_SIZE_LONG)
                            {
                                partitionLocations.Add(loc);
                            }
                        }
                    }
                    partitionedLocations[i][j] = Transform(partitionLocations);
                }
            }
            return partitionedLocations;
        }

        private static double[][] Transform(List<LocationProvider> locations)
        {
            double[][] result = new double[locations.Count][];
            for (int i = 0; i < locations.Count; i++)
            {
                double[] location = new double[2];
                location[0] = locations[i].getLatitude();
                location[1] = locations[i].getLongitude();
                result[i] = location;
            }
            return result;
        }
    }
}