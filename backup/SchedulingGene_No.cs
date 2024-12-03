using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Algorithm
{
    public class SchedulingGene
    {
       
        /// <summary>
        /// 种群数
        /// </summary>
        public int populationSize = 100;
        
        /// <summary>
        /// 初始交叉率
        /// </summary>
        public double iniCrossRate = 0.8;
        
        /// <summary>
        /// 初始变异率
        /// </summary>
        public double iniMutationRate = 0.1;
        
        /// <summary>
        /// 保优个数
        /// </summary>
        /// <remarks>设定从上代直接保留的最优染色体的个数</remarks>
        public int saveOptNumber = 1;
        
        /// <summary>
        /// 寻优方式
        /// </summary>
        /// <remarks>以何种指标寻优</remarks>
        public gaStyle optStyle=gaStyle .Makespan ;
        
        /// <summary>
        /// 连续未进化代数
        /// </summary>
        /// <remarks>5-10</remarks>
        public int noEvoAlge =50;
        
        /// <summary>
        /// 总进化代数
        /// </summary>
        public int evoAgle =500;

    }

    public enum gaStyle
    {
        Makespan,        //完成时间最小
        OverallTarget    //综合指标
    }
}
