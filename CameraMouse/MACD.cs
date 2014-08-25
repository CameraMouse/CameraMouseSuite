using System;
using System.Collections.Generic;
using System.Text;

namespace CameraMouse
{
    public class MACD
    {
        private EMA emaShort=null;
        private EMA emaLong = null;
        //private EMA signal = null;
        private MovingSum macdSum = null;
        private EMA emaMacdSum = null;
        private double macd=0;
        //private double signalThreshold = 0.0;

        /*
        public bool MACDSignalActive
        {
            get
            {
                return signal.IsActive;
            }
        }*/
        
        public bool MACDActive
        {
            get
            {
                return emaShort.IsActive && emaLong.IsActive;
            }
        }

        public double MACDSum
        {
            get
            {
                return macdSum.Sum;
            }
        }

        public double MACDSumEMA
        {
            get
            {
                return emaMacdSum.EMAverage;
            }
        }
        /*
        public bool ThresholdTrigger
        {
            get
            {
                return signal.IsActive && signal.EMAverage > signalThreshold;
            }
        }
        */

        public double EMAShortVal
        {
            get
            {
                return emaShort.EMAverage;
            }
        }

        public double EMALongVal
        {
            get
            {
                return emaLong.EMAverage;
            }
        }

        public double MACDVal
        {
            get
            {
                return macd;
            }
        }

        public double MACDSumRatio
        {
            get
            {
                if (!emaMacdSum.IsActive)
                    return 1.0;

                return this.macdSum.Sum / emaMacdSum.EMAverage;
            }
        }
        /*
        public double MACDSignal
        {
            get
            {
                return signal.EMAverage;
            }
        }*/
        

        public void Init(int shortPeriod, int longPeriod, int macdSumSize, int emaStartTime)
        {
            emaShort = new EMA();
            emaShort.Init(shortPeriod, 1);
            //emaShort.Init(shortPeriod,emaStartTime);
            emaLong = new EMA();
            emaLong.Init(longPeriod, 1);
            //emaLong.Init(longPeriod, emaStartTime);
            emaMacdSum = new EMA();
            emaMacdSum.Init(longPeriod*5, emaStartTime);

            macdSum = new MovingSum();

            macdSum.Init(macdSumSize);
        
            //signal = new EMA();
            //signal.Init(signalPeriod, emaStartTime);
            //this.signalThreshold = signalThreshold;
        }

        public void Reset()
        {
            emaShort.Reset();
            emaLong.Reset();
            //signal.Reset();
            macd = 0.0;
            macdSum.Reset();
            emaMacdSum.Reset();
         }

        public void AddPoint(double val)
        {
            emaShort.AddPoint(val);
            emaLong.AddPoint(val);

            if (MACDActive)
            {
                macd = val - emaLong.EMAverage;
                if (macd > 0.0)
                    macdSum.AddPoint(macd);
                else
                {
                    macdSum.Reset();
                    emaLong.SetPoint(val);
                }
                emaMacdSum.AddPoint(macdSum.Sum);
            }
        }
    }
}
