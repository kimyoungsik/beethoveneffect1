using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace beethoven3
{
    class DtwGestureRecognizer
    {
        private readonly int _dimension;
        private readonly double _firstThreshold;
        private readonly double _minimumLength;
        private readonly double _globalThreshold;
        private readonly ArrayList _labels;
        private readonly int _maxSlope;
        private readonly ArrayList _sequences;
        double minDist = double.PositiveInfinity;

        public double MinDist
        {
            get { return minDist; }
            set { minDist = value; }

        }

        public DtwGestureRecognizer(int dim, double threshold, double firstThreshold, double minLen)
        {
            _dimension = dim;
            _sequences = new ArrayList();
            _labels = new ArrayList();
            _globalThreshold = threshold;
            _firstThreshold = firstThreshold;
            _maxSlope = int.MaxValue;
            _minimumLength = minLen;
        }

        public DtwGestureRecognizer(int dim, double threshold, double firstThreshold, int ms, double minLen)
        {
            _dimension = dim;
            _sequences = new ArrayList();
            _labels = new ArrayList();
            _globalThreshold = threshold;
            _firstThreshold = firstThreshold;
            _maxSlope = ms;
            _minimumLength = minLen;
        }

        public void AddOrUpdate(ArrayList seq, string lab)
        {
            int existingIndex = -1;

            for (int i = 0; i < _labels.Count; i++)
            {
                if ((string)_labels[i] == lab)
                {
                    existingIndex = i;
                }
            }

            if (existingIndex >= 0)
            {
                _sequences.RemoveAt(existingIndex);
                _labels.RemoveAt(existingIndex);
            }

            _sequences.Add(seq);
            _labels.Add(lab);
        }

        public string Recognize(ArrayList seq)
        {
            minDist = double.PositiveInfinity;
            string classification = "__UNKNOWN";
            for (int i = 0; i < _sequences.Count; i++)
            {
                var example = (ArrayList)_sequences[i];
                
                if (Dist2((double[])seq[seq.Count - 1], (double[])example[example.Count - 1]) < _firstThreshold)
                {
                    double d = Dtw(seq, example) / example.Count;
                    if (d < minDist)
                    {
                        minDist = d;
                        classification = (string)_labels[i];
                    }
                }
            }

            return (minDist < _globalThreshold ? classification : "__UNKNOWN") + " " /*+minDist.ToString()*/;
        }


        public string RetrieveText()
        {
            string retStr = String.Empty;

            if (_sequences != null)
            {
                for (int gestureNum = 0; gestureNum < _sequences.Count; gestureNum++)
                {
                    retStr += _labels[gestureNum] + "\r\n";

                    int frameNum = 0;
                    foreach (double[] frame in ((ArrayList)_sequences[gestureNum]))
                    {
                        foreach (double dub in (double[])frame)
                        {
                            retStr += dub + "\r\n";
                        }

                        retStr += "~\r\n";

                        frameNum++;
                    }

                    retStr += "----";
                    if (gestureNum < _sequences.Count - 1)
                    {
                        retStr += "\r\n";
                    }
                }
            }

            return retStr;
        }

        public double Dtw(ArrayList seq1, ArrayList seq2)
        {
            var seq1R = new ArrayList(seq1);
            seq1R.Reverse();
            var seq2R = new ArrayList(seq2);
            seq2R.Reverse();
            var tab = new double[seq1R.Count + 1, seq2R.Count + 1];
            var slopeI = new int[seq1R.Count + 1, seq2R.Count + 1];
            var slopeJ = new int[seq1R.Count + 1, seq2R.Count + 1];

            for (int i = 0; i < seq1R.Count + 1; i++)
            {
                for (int j = 0; j < seq2R.Count + 1; j++)
                {
                    tab[i, j] = double.PositiveInfinity;
                    slopeI[i, j] = 0;
                    slopeJ[i, j] = 0;
                }
            }

            tab[0, 0] = 0;

            for (int i = 1; i < seq1R.Count + 1; i++)
            {
                for (int j = 1; j < seq2R.Count + 1; j++)
                {
                    if (tab[i, j - 1] < tab[i - 1, j - 1] && tab[i, j - 1] < tab[i - 1, j] &&
                        slopeI[i, j - 1] < _maxSlope)
                    {
                        tab[i, j] = Dist2((double[])seq1R[i - 1], (double[])seq2R[j - 1]) + tab[i, j - 1];
                        slopeI[i, j] = slopeJ[i, j - 1] + 1;
                        slopeJ[i, j] = 0;
                    }
                    else if (tab[i - 1, j] < tab[i - 1, j - 1] && tab[i - 1, j] < tab[i, j - 1] &&
                             slopeJ[i - 1, j] < _maxSlope)
                    {
                        tab[i, j] = Dist2((double[])seq1R[i - 1], (double[])seq2R[j - 1]) + tab[i - 1, j];
                        slopeI[i, j] = 0;
                        slopeJ[i, j] = slopeJ[i - 1, j] + 1;
                    }
                    else
                    {
                        tab[i, j] = Dist2((double[])seq1R[i - 1], (double[])seq2R[j - 1]) + tab[i - 1, j - 1];
                        slopeI[i, j] = 0;
                        slopeJ[i, j] = 0;
                    }
                }
            }

            double bestMatch = double.PositiveInfinity;
            for (int i = 1; i < (seq1R.Count + 1) - _minimumLength; i++)
            {
                if (tab[i, seq2R.Count] < bestMatch)
                {
                    bestMatch = tab[i, seq2R.Count];
                }
            }

            return bestMatch;
        }

        private double Dist1(double[] a, double[] b)
        {
            double d = 0;
            for (int i = 0; i < _dimension; i++)
            {
                d += Math.Abs(a[i] - b[i]);
            }

            return d;
        }

        private double Dist2(double[] a, double[] b)
        {
            double d = 0;
            for (int i = 0; i < _dimension; i++)
            {
                d += Math.Pow(a[i] - b[i], 2);
            }

            return Math.Sqrt(d);
        }
    }
}
