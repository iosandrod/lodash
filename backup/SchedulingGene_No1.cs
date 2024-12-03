using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace Algorithm
{
    public class SchedulingGene
    {
        public int populationSize = 100;
        public double iniCrossRate = 0.8;
        public double iniMutationRate = 0.1;
        public int saveOptNumber = 1;
        public gaStyle optStyle = gaStyle.Makespan;
        public int noEvoAlge = 50;//
        public int evoAgle = 500;//
    }
    public enum gaStyle
    {
        Makespan,        //完成时间最小
        OverallTarget    //综合指标
    }
}