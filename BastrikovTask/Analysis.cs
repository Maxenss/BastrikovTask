﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BastrikovTask
{
    static class Analysis
    {
        const int INF = Int32.MaxValue;

        private static Random random = new Random();

        static int[,] TargetMatrix = null;

        public const int RANDOM_MATRIX = 1;
        public const int SYMMETRIC_MATRIX = 2;

        private static int GENERATE_MODE;

        private static int TargetMatrixSize = 0;
        private static int CountOfMatrix = 0;

        // Классы для решения задачи Коммивояжера
        private static BruteForce bruteForce = new BruteForce();
        private static BranchClassic branchClassic = new BranchClassic();
        private static BranchClassicPlus branchClassicPlus = new BranchClassicPlus();

        // Путь для каждого из методов
        static Dictionary<int, int> BruteForceWay;
        static Dictionary<int, int> BranchClassicWay;
        static Dictionary<int, int> BranchClassicPlusWay;

        // Время для каждого из методов
        static long BruteForceTime;
        static long BranchClassicTime;
        static long BranchClassicPlusTime;

        // Сумма расстояний для каждого из методов
        static int BruteForceSum;
        static int BranchClassicSum;
        static int BranchClassicPlusSum;

        static int BruteForceSums;
        static int BranchClassicSums;
        static int BranchClassicPlusSums;

        static bool BruteForceMethodEnable = false;
        static bool BranchMethodEnable = false;
        static bool BranchPlusMethodEnable = false;

        public static readonly List<Dictionary<int, int>> ListWithAllDictionaryBruteForceWays = new List<Dictionary<int, int>>();
        public static readonly List<Dictionary<int, int>> ListWithAllDictionaryBranchClassicWays = new List<Dictionary<int, int>>();
        public static readonly List<Dictionary<int, int>> ListWithAllDictionaryBranchClassicPlusWays = new List<Dictionary<int, int>>();

        public static readonly List<long> ListWithAllBruteForceTime = new List<long>();
        public static readonly List<long> ListWithAllBranchClassicTime = new List<long>();
        public static readonly List<long> ListWithAllBranchClassicPlusTime = new List<long>();

        public static readonly List<int> ListWithBruteForceSum = new List<int>();
        public static readonly List<int> ListWithBranchClassicSum = new List<int>();
        public static readonly List<int> ListWithBranchClassicPlusSum = new List<int>();

        private static int odinakovih = 0;
        private static int luche = 0;
        private static int huge = 0;

        public static String Start(int matrix_size, int matrix_count, int generation_mode,
            bool brute_method, bool branch_method, bool branch_plus_method)
        {

            ClearVariable();

            GENERATE_MODE = generation_mode;

            BruteForceMethodEnable = brute_method;
            BranchMethodEnable = branch_method;
            BranchPlusMethodEnable = branch_plus_method;

            TargetMatrixSize = matrix_size;
            CountOfMatrix = matrix_count;

            GetAllSolutionForAllMatrix();
            Sum();


            return BuildAnalysis();
        }

        // Метод генерирует матрицу 
        private static void GenerateMatrix()
        {
            TargetMatrix = new int[TargetMatrixSize, TargetMatrixSize];

            for (int i = 0; i < TargetMatrixSize; i++)
            {
                for (int j = 0; j < TargetMatrixSize; j++)
                {
                    if (i == j)
                    {
                        TargetMatrix[i, j] = Analysis.INF;
                        continue;
                    }

                    switch (GENERATE_MODE)
                    {
                        case RANDOM_MATRIX:
                            {
                                TargetMatrix[i, j] = random.Next(1, 100);
                                break;
                            }
                        case SYMMETRIC_MATRIX:
                            {

                                int temp = random.Next(1, 100);

                                TargetMatrix[i, j] = temp;
                                TargetMatrix[j, i] = temp;
                                break;
                            }

                    }
                }
            }
        }

        private static void GetAllSolutionForTargetMatrix()
        {
            if (BruteForceMethodEnable)
            {
                BruteForceSolution();
                ListWithAllDictionaryBruteForceWays.Add(BruteForceWay);
                ListWithAllBruteForceTime.Add(BruteForceTime);
                ListWithBruteForceSum.Add(BruteForceSum);
            }

            if (BranchMethodEnable)
            {
                BranchClassicSolution();
                ListWithAllDictionaryBranchClassicWays.Add(BranchClassicWay);
                ListWithAllBranchClassicTime.Add(BranchClassicTime);
                ListWithBranchClassicSum.Add(BranchClassicSum);
            }

            if (BranchPlusMethodEnable)
            {
                BranchClassicPlusSolution();
                ListWithAllDictionaryBranchClassicPlusWays.Add(BranchClassicPlusWay);
                ListWithAllBranchClassicPlusTime.Add(BranchClassicPlusTime);
                ListWithBranchClassicPlusSum.Add(BranchClassicPlusSum);
            }
        }

        private static void GetAllSolutionForAllMatrix()
        {
            for (int i = 0; i < CountOfMatrix; i++)
            {
                GenerateMatrix();
                // Тут я по глупости словил переполнение стека
                GetAllSolutionForTargetMatrix();
            }
        }

        private static void ClearVariable()
        {
            ListWithAllDictionaryBruteForceWays.Clear();
            ListWithAllDictionaryBranchClassicWays.Clear();
            ListWithAllDictionaryBranchClassicPlusWays.Clear();

            ListWithAllBruteForceTime.Clear();
            ListWithAllBranchClassicTime.Clear();
            ListWithAllBranchClassicPlusTime.Clear();

            ListWithBruteForceSum.Clear();
            ListWithBranchClassicSum.Clear();
            ListWithBranchClassicPlusSum.Clear();

            BruteForceSum = 0;
            BranchClassicSum = 0;
            BranchClassicPlusSum = 0;

            BruteForceSums = 0;
            BranchClassicSums = 0;
            BranchClassicPlusSums = 0;

            odinakovih = 0;
            luche = 0;
            huge = 0;

            BruteForceMethodEnable = false;
            BranchMethodEnable = false;
            BranchPlusMethodEnable = false;
        }

        private static void Sum()
        {

            for (int i = 0; i < CountOfMatrix; i++)
            {
                if (BruteForceMethodEnable)
                    BruteForceSums += ListWithBruteForceSum.ElementAt(i);

                if (BranchMethodEnable)
                    BranchClassicSums += ListWithBranchClassicSum.ElementAt(i);

                if (BranchPlusMethodEnable)
                    BranchClassicPlusSums += ListWithBranchClassicPlusSum.ElementAt(i);
            }
        }

        private static String BuildAnalysis()
        {
            String analysis = "";

            analysis += "__________________________________________________________________________________________\n";
            analysis += "Размерность матриц : " + TargetMatrixSize + "*"+ TargetMatrixSize + ", Количество матриц : " + CountOfMatrix + "\n";

            if (BruteForceMethodEnable)
                analysis += "Сумма расстояний для полного перебора : " + BruteForceSums + "\n";

            if (BranchMethodEnable)
                analysis += "Сумма расстояний для метода ветвей и границ : " + BranchClassicSums + "\n";

            if (BranchPlusMethodEnable)
                analysis += "Сумма расстояний для метода ветвей и границ+ : " + BranchClassicPlusSums + "\n";

            if (BranchPlusMethodEnable && BranchMethodEnable)
            {
                Method();

                analysis += "Совпало : " + odinakovih + ", лучше : " + luche + ", хуже : " + huge + "\n";
            }

            return analysis;
        }

        private static void Method()
        {
            for (int i = 0; i < ListWithBranchClassicSum.Count; i++)
            {
                if (ListWithBranchClassicSum.ElementAt(i) == ListWithBranchClassicPlusSum.ElementAt(i))
                    ++odinakovih;
                else if (ListWithBranchClassicSum.ElementAt(i) < ListWithBranchClassicPlusSum.ElementAt(i))
                    ++huge;
                else
                    ++luche;
            }
        }

        private static void BruteForceSolution()
        {
            bruteForce.BruteForceStart(TargetMatrix, 0, out BruteForceSum, out BruteForceWay, out BruteForceTime);
        }

        private static void BranchClassicSolution()
        {
            branchClassic.Start(TargetMatrix, out BranchClassicSum, out BranchClassicWay, out BranchClassicTime, true);
            ;
        }

        private static void BranchClassicPlusSolution()
        {
            branchClassicPlus.Start(TargetMatrix, out BranchClassicPlusSum, out BranchClassicPlusWay, out BranchClassicPlusTime, true);
        }
    }
}
