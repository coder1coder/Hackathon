using BenchmarkDotNet.Running;
using Hackathon.Tests.Benchmark.Mapping;

BenchmarkRunner.Run<MapsterCastBenchmark>();
BenchmarkRunner.Run<MapsterVsAutomapper>();