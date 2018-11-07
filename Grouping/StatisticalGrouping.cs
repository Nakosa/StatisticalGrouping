using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

using System.Windows.Forms;

namespace Grouping
{
    public class StatisticalGrouping
    {
        private struct Stages {
            public static string start = "Начало";
            public static string init = "Инициализация";
            public static string grouping = "Группировка данных";
            public static string groupingSubgroups = "Группировка по подгруппам";
            public static string ending = "Завершение расчётов";
            public static string end = "Конец";
        }

        public struct Borders {
            public double start;
            public double end;

            public Borders(int index, double h, int countS) {
                this.start = index * h;
                this.end = this.start + h;
            }

            public bool Between(double num) {
                return (num >= this.start && num < this.end) ? true : false;
            }
        }

        public struct Data{
            public double[][] arr;
            public int groupingIndex;
            public double Max;
            public double Min;
            public int rowCount;
            public int colCount;
            public int n;
            public double h;
            public Borders[] groupsBorders;
            List<List<List<int>>> groups;
            private int countS;

            public Data(double[][] newarr, int newindex, int s) {
                this.countS = s;
                this.arr = newarr;
                this.groupingIndex = newindex;
                this.rowCount = this.arr.GetLength(0);
                this.colCount = this.rowCount / this.arr.Length;

                this.Max = double.MinValue;
                this.Min = double.MaxValue;

                for (int i = 0; i < this.rowCount; i++)
                {
                    //this.arr[i, this.groupingIndex] = StatisticalGrouping.Round(this.arr[i, this.groupingIndex], this.countS);
                    double number = this.arr[i][this.groupingIndex];
                    if (this.Max < number) {
                        this.Max = number;
                    }
                    if (this.Min > number)
                    {
                        this.Min = number;
                    }
                }

                this.n = (int)(1 + 3.322 * Math.Log10(this.rowCount));
                this.h = (this.Max - this.Min)/this.n;

                //this.groups = new double[this.n][this.rowCount][this.colCount];
                this.groupsBorders = new Borders[this.n];
                for (int i = 0; i < this.n; i++) {
                    this.groupsBorders[i] = new Borders(i, this.h, this.countS);
                }

                List<List<int>> arrIndexesInGroups = new List<List<int>>();
                for (int i = 0; i < this.rowCount; i++) {
                    double number = this.arr[i][this.groupingIndex];
                    for (int groupIndex = 0; groupIndex < this.n; groupIndex++) {
                        if (this.groupsBorders[groupIndex].Between(number)) {
                            this.groups[groupIndex][i] = this.arr[i].ToArray<double>;
                        }
                    }
                }

                //this.groups = new int[][0][0];
            }
        }

        private int countSymbols;
        public Data data;
        public StatisticalGrouping[] innerGroups;

        public bool ready = false;
        public string stage = null;



        public StatisticalGrouping() {
            this.stage = Stages.start;
            this.countSymbols = 8;
        }

        public void Init(double[][] newarr, int[] arrIndexes) {
            this.stage = Stages.init;
            int index = arrIndexes[0];
            this.stage = Stages.grouping;
            this.data = new Data(newarr, index, this.countSymbols);
            if (arrIndexes.Length > 1) {
                this.stage = Stages.groupingSubgroups;
                int[] newArrIndexes = new int[arrIndexes.Length - 1];
                Array.Copy(arrIndexes, 1, newArrIndexes, 0, arrIndexes.Length - 1);
                this.innerGroups = new StatisticalGrouping[this.data.n];
                for(int i = 0; i < this.data.n; i++)
                {
                    this.innerGroups[i] = new StatisticalGrouping();

                    double[][] newArray = new double[this.data.rowCount][this.data.colCount];
                    for (int rowIndex = 0; rowIndex < this.data.rowCount; rowIndex++) {
                        for (int colIndex = 0; colIndex < this.data.colCount; colIndex++) {
                            //newArray[rowIndex, colIndex] = this.data.groups[i, rowIndex, colIndex];
                        }
                    }
                    this.innerGroups[i].Init(newArray, newArrIndexes);
                }
            }
            this.stage = Stages.ending;
            this.complete();
        }

        private void complete() {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            this.stage = Stages.end;
            this.ready = true;
        }

        static double Round(double num, int countS) {
            num = Math.Round(num, countS);
            return num;
        }
    }
}
